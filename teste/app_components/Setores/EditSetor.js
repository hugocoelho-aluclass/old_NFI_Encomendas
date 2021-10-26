'use strict';

/* Controllers */

app.controller('SetorEditCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.setor = {};    
    $scope.setor.numsetor = $stateParams.id;

   //console.log($stateParams.id);

    $scope.$watch('setor.numSetor', function (newValue, oldValue) {

       if (newValue != oldValue)
       {
           
           carregarsetor(newValue);
       }
    });

    $scope.refreshAddresses = function (address) { };
   
    function carregarsetor(id)
    {   
        $http.get(serviceBase + "api/setores/Editsetor/" + id)
            .success(function (data, status, headers, config) {
                if (data.setor.numsetor != $stateParams.id)
                {
                    //$state.go("app.setores.edit", { id: data.setor.numsetor });
                }
              $scope.dados = data;
              $scope.setor = data.setor;
          });

    }
    carregarsetor($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/setores/AtualizaSetor/", $scope.setor)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Setor Atualizado", "");
            $state.go("app.setores.tabela");
        });
    }


}]);
