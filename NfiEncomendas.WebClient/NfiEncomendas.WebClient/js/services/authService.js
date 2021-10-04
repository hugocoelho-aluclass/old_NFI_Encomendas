'use strict';
angular.module('app').factory('authService', ['$http', '$q', '$localStorage', function ($http, $q, $localStorage) {
   
    var authServiceFactory = {};
    var _authentication = {
        isAuth: false,
        userName: "",
        isLoggedOut: false
    };
    var _saveRegistration = function (registration) {
       
        _logOut();
        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
   
            return response;
        });

    };

    var _login = function (loginData) {
        //console.log("_login");
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            $localStorage.nfiEncAuthorizationData = { token: response.access_token, userName: loginData.userName };

            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;
            _authentication.isLoggedOut = false;
            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {
        $http.post(serviceBase + 'api/account/logout', { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })

        delete $localStorage.nfiEncAuthorizationData;
        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.isLoggedOut = true;
       // app.utilizador = null;
        //console.log("_logOut");
    };

    var _fillAuthData = function () {
        
        var authData = $localStorage.nfiEncAuthorizationData;
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.isLoggedOut = false;
        }
        else {
            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.isLoggedOut = false;
        }
       //console.log("_fillAuthData return");
       //console.log(authData);
    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);

angular.module('app').factory('authInterceptorService', ['$q', '$injector', '$location', '$localStorage', function ($q, $injector, $location, $localStorage) {
    //console.log("authInterceptorService");
    var authInterceptorServiceFactory = {};

    var _request = function (config) {
        //console.log("HTTP REQUEST");
        //console.log(config);
        config.headers = config.headers || {};

        var authData = $localStorage.nfiEncAuthorizationData;
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }
        RefreshTimer();
        return config;
    }

    var _responseError = function (rejection) {
        //console.log("_responseError");
        //console.log(rejection);
        if (rejection.status === 401) {
            var authService = $injector.authService;
            var authData = $localStorage.nfiEncAuthorizationData;
            
            if (authData) {
                if (authData.useRefreshTokens) {
              //      console.log("useRefreshTokens");
                   // $location.path('/refresh');
                    
                    return $q.reject(rejection);
                }
            }
            //console.log("useRefreshTokens = false");
           // authService.logOut();

            if (rejection.config.url.indexOf("Account/GetMe") > -1)
            {
                //authService.logOut();
              //  console.log("erro get me");
                $location.path('/access/signout');
                //caltion.reload(true);
            }
            else
                {
                $location.path('/access/signout');
                //$location.path('access/401');
            }
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);

angular.module('app').factory('tokensManagerService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var tokenManagerServiceFactory = {};
    //console.log("tokensManagerService");
    var _getRefreshTokens = function () {
        //console.log("_getRefreshTokens");
        return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
            return results;
        });
    };

    var _deleteRefreshTokens = function (tokenid) {
        //console.log("_deleteRefreshTokens");
        return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
            return results;
        });
    };

    tokenManagerServiceFactory.deleteRefreshTokens = _deleteRefreshTokens;
    tokenManagerServiceFactory.getRefreshTokens = _getRefreshTokens;
    //console.log("return tokenManagerServiceFactory");
    return tokenManagerServiceFactory;

}]);

angular.module('app').constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

angular.module('app').config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
    $httpProvider.interceptors.push('httpMethodInterceptor');
});

angular.module('app').run(['authService', function (authService) {
    authService.fillAuthData();
}]);
app.controller('tokensManagerController', ['$scope', 'tokensManagerService', function ($scope, tokensManagerService) {

    $scope.refreshTokens = [];

    tokensManagerService.getRefreshTokens().then(function (results) {

        $scope.refreshTokens = results.data;

    }, function (error) {
       // alert(error.data.message);
    });

    $scope.deleteRefreshTokens = function (index, tokenid) {

        tokenid = window.encodeURIComponent(tokenid);

        tokensManagerService.deleteRefreshTokens(tokenid).then(function (results) {

            $scope.refreshTokens.splice(index, 1);

        }, function (error) {
          //  alert(error.data.message);
        });
    }

}]);