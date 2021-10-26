'use strict';

/* Controllers */
  // signin controller
app.controller('SigninFormController', ['$scope', '$location', '$state', 'authService', '$cookies', function ($scope, $location, $state, authService, $cookies) {
    $scope.user = {};
   
    $scope.login = function () {
        $scope.loginData = {
            userName: $scope.user.email,
            password: $scope.user.passwordForm == $scope.user.passwordCookie ? $scope.user.passwordCookie : MD5($scope.user.passwordForm),
            save: $scope.user.guardarDados
        };

        authService.login($scope.loginData).then(function (response) {
            //console.log("$scope.user.guardarDados = " + $scope.user.guardarDados);
            //console.log($scope.user);
            if ($scope.user.guardarDados) {
                //console.log("guardar cookie");

                $cookies._userSaved_email = $scope.user.email;
                $cookies._userSaved_password = $scope.user.passwordForm == $scope.user.passwordCookie ? $scope.user.passwordCookie : MD5($scope.user.passwordForm);
            }
            else {
                $cookies._userSaved_email = "";
                $cookies._userSaved_password = "";

            }
            $state.go('app.dashboard-v1');
            //$location.path('/app/Dashboard-v1/');
        },
         function (err) {
             if (err.error == "invalid_grant") {
                 $scope.authError = "Nome de Utilizador ou Password errados.";
             }
             else if (err.error == "account_disabled") {
                 $scope.authError = "A conta de Utilizador está desactivada. Contacte o administrador.";
             }
             else {
                 $scope.authError = err.error_description;
             }

         });
    };


    if ($cookies._userSaved_email) {
        $scope.user.email = $cookies._userSaved_email;
        $scope.user.passwordForm = $cookies._userSaved_password;
        $scope.user.passwordCookie = $cookies._userSaved_password;
        $scope.user.guardarDados = true;
        if (authService.authentication.isLoggedOut == false)    $scope.login();
    }
    else {
        $scope.user.email = "";
        $scope.user.passwordForm = "";
        $scope.user.passwordCookie = "";
        $scope.user.guardarDados = true;
    }
    
    if (authService.authentication.isAuth == true)
    {        
        $state.go('app.dashboard-v1');
    }
    else
        {    
    $scope.authError = "";
    $scope.message = "";
 
    }
}]);





//    ['$scope', '$http', '$state', function ($scope, $http, $state) {
//    $scope.user = {};
//    $scope.authError = null;
//    $scope.login = function() {
//      $scope.authError = null;
//      // Try to login
//      $http.post('api/login', {email: $scope.user.email, password: $scope.user.password})
//      .then(function(response) {
//        if ( !response.data.user ) {
//          $scope.authError = 'Email or Password not right';
//        }else{
//          $state.go('app.dashboard-v1');
//        }
//      }, function(x) {
//        $scope.authError = 'Server Error';
//      });
//    };
//  }])
//;