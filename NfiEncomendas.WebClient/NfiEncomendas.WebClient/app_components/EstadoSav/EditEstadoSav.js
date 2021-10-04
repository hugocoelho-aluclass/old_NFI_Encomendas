'use strict';

/* Controllers */

app.controller('EstadoSavEditCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.estadoSav = {};    
    $scope.estadoSav.numEstadoSav = $stateParams.id;

   //console.log($stateParams.id);

    $scope.$watch('estadoSav.numEstadoSav', function (newValue, oldValue) {

       if (newValue != oldValue)
       {
           
           carregarestadoSav(newValue);
       }
    });

    $scope.refreshAddresses = function (address) { };

   
    function carregarestadoSav(id)
    {   
        $http.get(serviceBase + "api/EstadoSav/EditEstadoSav/" + id)
            .success(function (data, status, headers, config) {
                if (data.estadoSav.numEstadoSav != $stateParams.id)
                {
                    //$state.go("app.estadoSavs.edit", { id: data.estadoSav.numestadoSav });
                }
              $scope.dados = data;
              $scope.estadoSav = data.estadoSav;
          });

    }
    carregarestadoSav($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/EstadoSav/AtualizaEstadoSav/", $scope.estadoSav)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Estado de Sav Atualizado", "");
            $state.go("app.estadoSav.tabela");
        });
    }


}]);
