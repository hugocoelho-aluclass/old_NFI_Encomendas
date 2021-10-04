'use strict';

/* Controllers */

app.controller('EditDepartamentoSavCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.departamentoSav = {};    
    $scope.departamentoSav.numDepartamentoSav = $stateParams.id;

   //console.log($stateParams.id);

    $scope.$watch('departamentoSav.numDepartamentoSav', function (newValue, oldValue) {

       if (newValue != oldValue)
       {
           
           carregardepartamentoSav(newValue);
       }
    });

    $scope.refreshAddresses = function (address) { };

   
    function carregardepartamentoSav(id)
    {   
        $http.get(serviceBase + "api/DepartamentoSavs/EditDepartamentoSav/" + id)
            .success(function (data, status, headers, config) {
                if (data.departamentoSav.numDepartamentoSav != $stateParams.id)
                {
                    //$state.go("app.departamentoSavs.edit", { id: data.departamentoSav.numdepartamentoSav });
                }
              $scope.dados = data;
              $scope.departamentoSav = data.departamentoSav;
          });

    }
    carregardepartamentoSav($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/DepartamentoSavs/AtualizaDepartamentoSav/", $scope.departamentoSav)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Departamento SAV Atualizado", "");
            $state.go("app.departamentoSavs.tabela");
        });
    }


}]);
