'use strict';

/* Controllers */

app.controller('adminGestaoOperadoresEdit', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams, $state, ngTableParams, toaster) {

    $scope.operador = {};
    $scope.operador.id = $stateParams.id;
    $scope.operador.departamentos = [];
    $scope.departamentosLista = [];
    $scope.departamentosSelect = {};
    $scope.departamentosSelect.selectedDepartamentosLista = [];// [$scope.clientesLista[5], $scope.clientesLista[4]];
    $scope.departamentosSelect.selectedDepartamentosListaWithGroupBy = [];// [$scope.clientesLista[8], $scope.clientesLista[6]];
    $scope.departamentosSelect.selectedDepartamentosListaSimple = []
    $http.get(serviceBase + "api/AdminOperadores/EditarOperador/" + $stateParams.id)
       .success(function (data, status, headers, config) {
           $scope.departamentosLista = data.departamentos;
           $scope.dados = data;

           $scope.operador = data.operador;

           var opDeps = angular.copy($scope.operador.departamentos);
           $scope.operador.departamentos = [];

           for (var i = 0; i < opDeps.length; i++) {
               for (var j = 0; j < $scope.departamentosLista.length; j++) {
                   console.log($scope.departamentosLista[j], opDeps[i]);
                   if ($scope.departamentosLista[j].id == opDeps[i].id) {
                       console.log("T", $scope.departamentosLista[j], opDeps[i]);
                       $scope.operador.departamentos.push($scope.departamentosLista[j]);
                       continue
                   }
               }
           }    
       });


    $scope.submit = function () {
        if ($scope.operador.passwordForm) {
            $scope.operador.password = MD5($scope.operador.passwordForm);
            $scope.operador.password_confirm = "";
        }

        $http.post(serviceBase + "api/AdminOperadores/AtualizaOperador/", $scope.operador)
        .success(function (data, status, headers, config) {

            toaster.pop("success", "Operador Atualizado", "Mensagem");
            $state.go("app.admin.gestaoOperadores");
        });
    }

    $scope.AtualizaOperadorPassword = function () {
        var dados = {
            operadorId: $stateParams.id,
            Md5Password: MD5($scope.operador.passwordForm)
        }

        $http.post(serviceBase + "api/AdminOperadores/AtualizaOperadorPassword/", dados)
        .success(function (data, status, headers, config) {

            toaster.pop("success", "Operador Atualizado", "Mensagem");
            $state.go("app.admin.gestaoOperadores");
        });
    }
}]);
