'use strict';

/* Controllers */

angular.module('app')
  .controller('AppCtrl', ['$scope', '$localStorage', '$window', '$rootScope',
    function ($scope, $localStorage, $window, $rootScope) {
        // add 'ie' classes to html


        var isIE = !!navigator.userAgent.match(/MSIE/i);
        isIE && angular.element($window.document.body).addClass('ie');
        isSmartDevice($window) && angular.element($window.document.body).addClass('smart');


        // config
        $scope.app = {
            name: 'N.F.I. ',
            version: '2',
            // for chart colors
            color: {
                primary: '#7266ba',
                info: '#23b7e5',
                success: '#27c24c',
                warning: '#fad733',
                danger: '#f05050',
                light: '#e8eff0',
                dark: '#3a3f51',
                black: '#1c2b36'
            },
            settings: {
                themeID: 1,
                navbarHeaderColor: 'bg-danger',
                navbarCollapseColor: 'bg-white-only',
                asideColor: 'bg-dark',
                headerFixed: true,
                asideFixed: false,
                asideFolded: false,
                asideDock: false,
                container: false
            }
        }
       

        // save settings to local storage
        //nfiEncAuthorizationData
        if (angular.isDefined($localStorage.nfiEncSettings)) {
            $scope.app.settings = $localStorage.nfiEncSettings;
        } else {
            $localStorage.nfiEncSettings = $scope.app.settings;
        }
        $scope.$watch('app.settings', function () {
            if ($scope.app.settings.asideDock && $scope.app.settings.asideFixed) {
                // aside dock and fixed must set the header fixed.
                $scope.app.settings.headerFixed = true;
            }
            // save to local storage
            $localStorage.nfiEncSettings = $scope.app.settings;
        }, true);

        var appUtilizador = $scope.$watch(function () {
            return app.utilizador;
        }, function () {
            $scope.mostra = app.utilizador != null;
            //console.log("mostra " + $scope.mostra +  app.utilizador);
            // if ($scope.mostra == true) appUtilizador();
            //$scope.$apply();
        });

        // angular translate
        //$scope.lang = { isopen: false };
        //$scope.langs = {en:'English', de_DE:'German', it_IT:'Italian'};
        //$scope.selectLang = $scope.langs[$translate.proposedLanguage()] || "English";
        //$scope.setLang = function(langKey, $event) {
        //  // set the current lang
        //  $scope.selectLang = $scope.langs[langKey];
        //  // You can change the language during runtime
        //  $translate.use(langKey);
        //  $scope.lang.isopen = !$scope.lang.isopen;
        //};

        function isSmartDevice($window) {
            // Adapted from http://www.detectmobilebrowsers.com
            var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
            // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
            return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
        }

    }]);