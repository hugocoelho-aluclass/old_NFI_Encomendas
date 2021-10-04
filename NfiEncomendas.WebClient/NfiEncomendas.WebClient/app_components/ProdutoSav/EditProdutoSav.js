'use strict';

/* Controllers */

app.controller('ProdutoSavEditCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.produtoSav = {};    
    $scope.produtoSav.numProdutoSav = $stateParams.id;

   //console.log($stateParams.id);

    $scope.$watch('produtoSav.numProdutoSav', function (newValue, oldValue) {

       if (newValue != oldValue)
       {
           
           carregarprodutoSav(newValue);
       }
    });

    $scope.refreshAddresses = function (address) { };

   
    function carregarprodutoSav(id)
    {   
        $http.get(serviceBase + "api/ProdutoSavs/EditProdutoSav/" + id)
            .success(function (data, status, headers, config) {
                if (data.produtoSav.numProdutoSav != $stateParams.id)
                {
                    //$state.go("app.produtoSavs.edit", { id: data.produtoSav.numprodutoSav });
                }
              $scope.dados = data;
              $scope.produtoSav = data.produtoSav;
          });

    }
    carregarprodutoSav($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/ProdutoSavs/AtualizaProdutoSav/", $scope.produtoSav)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Tipo de Avaria Atualizado", "");
            $state.go("app.produtoSavs.tabela");
        });
    }


}]);
