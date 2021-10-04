'use strict';

/* Controllers */

app.controller('TipoAvariaEditCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.tipoAvaria = {};    
    $scope.tipoAvaria.numTipoAvaria = $stateParams.id;

   //console.log($stateParams.id);

    $scope.$watch('tipoAvaria.numTipoAvaria', function (newValue, oldValue) {

       if (newValue != oldValue)
       {
           
           carregartipoAvaria(newValue);
       }
    });

    $scope.refreshAddresses = function (address) { };

   
    function carregartipoAvaria(id)
    {   
        $http.get(serviceBase + "api/TipoAvarias/EditTipoAvaria/" + id)
            .success(function (data, status, headers, config) {
                if (data.tipoAvaria.numTipoAvaria != $stateParams.id)
                {
                    //$state.go("app.tipoAvarias.edit", { id: data.tipoAvaria.numtipoAvaria });
                }
              $scope.dados = data;
              $scope.tipoAvaria = data.tipoAvaria;
          });

    }
    carregartipoAvaria($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/TipoAvarias/AtualizaTipoAvaria/", $scope.tipoAvaria)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Tipo de Avaria Atualizado", "");
            $state.go("app.tipoAvarias.tabela");
        });
    }


}]);
