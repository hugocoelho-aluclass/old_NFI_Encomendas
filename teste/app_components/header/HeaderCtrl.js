//HeaderCtrl

app.controller('HeaderCtrl', ['$scope', '$rootScope', '$http', '$interval', '$timeout', '$modal','$window', 'toaster',
    function ($scope, $rootScope, $http, $interval, $timeout, $modal,$window, toaster) {
    //$scope.nomeCompleto = "Nome user";
    //GetMe

    $scope.AtualizaGetMe = function () {
        $http.get(serviceBase + "api/Account/GetMe")
            .success(function (data, status, headers, config) {
                $rootScope._utilizador = data;
                app.utilizador = data;
            });
    };

    $scope.AtualizaGetMe();
    app.utilizador = $rootScope._utilizador;

    $scope.VerificaAtualiaGetMe = function () {
        var agora = Math.round(new Date().getTime() / 1000);
        if (agora - secondsTimer > 100) $scope.AtualizaGetMe();
    };

 
    $scope.perfilAlteraPassword = function (enc) {
        //enc.marcarEntregue = true;

        var modalInstance = $modal.open({
            templateUrl: 'app_components/Header/AlterarPassword.html',
            controller: 'ModalHeaderAlteraPasswordCtrl',
            backdrop: 'static',
            keyboard: false,

            backdropClick: false,
            resolve: {
                idUtuilizador: function () {
                    return $rootScope._utilizador.login;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) { }, function () { });
    }

    $scope.utilizadorConfigs = function () {
        //enc.marcarEntregue = true;

        var modalInstance = $modal.open({
            templateUrl: 'app_components/Header/ConfigsUtilizador.html',
            controller: 'ConfigsUtilizadorCtrl',
            backdrop: 'static',
            keyboard: false,
            backdropClick: false,
            resolve: {

            }
        });

        modalInstance.result.then(function (selectedItem) { }, function () { });
    }


    //check Local Storage - baseado em cookies, tabela SAVS
   
    $scope.verificaToaster = function () {
        //setToaster
        try {
            if (localStorage.getItem("setToaster") !== null) {
                var args = localStorage.getItem("setToaster");
                args = args.split("|");
                //console.log(args);
                toaster.pop(args[0], args[1], args[2]);
                localStorage.removeItem("setToaster");
            }
        } catch (e) {
        }
    };

        $window.onfocus = function () {
            if ($scope && $timeout)    $timeout(100,   $scope.verificaToaster || (() => true));
    }

    $interval($scope.verificaToaster, 2000);

}]);

app.controller('ModalHeaderAlteraPasswordCtrl', ['$scope', '$modalInstance', '$http', 'idUtilizador', 'toaster', function ($scope, $modalInstance, $http, idUtilizador,toaster) {
    //  $scope.ModalEncomenda = encomenda;
    $scope.modalPw = {};

    $scope.tecla = function (keyEvent) {

        if (keyEvent.which === 13) {
            keyEvent.preventDefault();
            keyEvent.stopPropagation();

        }
        //alert('Im a lert');
    };


    $scope.ok = function () {

        //console.log("fazer post" + idUtilizador);
        var antigaMd5 = MD5($scope.modalPw.pwAntiga);
        var novaMd5 = MD5($scope.modalPw.pwNova);
        //console.log("antiga " + antigaMd5);

        $http.post(serviceBase + "api/Dashboard/AlterarPassword?login=" + idUtilizador + "&pwAntiga=" + antigaMd5 + "&pwNova=" + novaMd5)
           .success(function (data, status, headers, config) {
               // console.log(data);
               if (data === '"pwErrada"') {
                   toaster.pop("error", "Password antiga errada", "");
               }
               else {
                   toaster.pop("success", "Password alterada!", "");
                   $modalInstance.close('sucesso');
               }
           });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])
;

app.controller('ConfigsUtilizadorCtrl', ['$scope', '$modalInstance', '$http', 'toaster', function ($scope, $modalInstance, $http, toaster) {
    $scope.perfil = {};
    $scope.dataDashboardIsOpen = false;
    $http.get(serviceBase + "api/Account/GetMyPerfil")
          .success(function (data, status, headers, config) {
              $scope.perfil = data;
          });



    $scope.ok = function () {
        $http.post(serviceBase + "api/Account/UpdateMyPerfil", $scope.perfil)
           .success(function (data, status, headers, config) {
             
                   toaster.pop("success", "Ok!", "");
                   $modalInstance.close('sucesso');
               
           });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])
;
