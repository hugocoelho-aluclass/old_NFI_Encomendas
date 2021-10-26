'use strict';

/* Controllers */

app.controller('EstadoRecolhaEditCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.estadoRecolha = {};    
    $scope.estadoRecolha.id = $stateParams.id;

   //console.log($stateParams.id);

    $scope.$watch('estadoRecolha.id', function (newValue, oldValue) {

       if (newValue != oldValue)
       {
           
           carregarestadoRecolha(newValue);
       }
    });

    $scope.refreshAddresses = function (address) { };

   
    function carregarestadoRecolha(id)
    {   
        $http.get(serviceBase + "api/EstadosRecolha/EditEstadoRecolha/" + id)
            .success(function (data, status, headers, config) {
                if (data.estadoRecolha.numEstadoRecolha != $stateParams.id)
                {
                    //$state.go("app.estadoRecolha.edit", { id: data.estadoRecolha.numestadoRecolha });
                }
              $scope.dados = data;
              $scope.estadoRecolha = data.estadoRecolha;
          });

    }
    carregarestadoRecolha($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/EstadosRecolha/AtualizaEstadoRecolha/", $scope.estadoRecolha)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Estado de Recolha Atualizado", "");
            $state.go("app.estadosRecolha.tabela");
        });
    }


}]);
