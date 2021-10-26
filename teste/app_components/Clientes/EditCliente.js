'use strict';

/* Controllers */

app.controller('EditClienteCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams,$state, ngTableParams, toaster) {

    $scope.cliente = {};    
    $scope.cliente.numCliente = $stateParams.id;

   //console.log($stateParams.id);
    var timeout;
   $scope.$watch('cliente.numCliente', function (newValue, oldValue) {
       if (newValue != oldValue)
       {        
           clearTimeout(timeout);
           timeout = setTimeout(() => { carregarCliente(newValue); }, 500);
            
       }
    });
   
    function carregarCliente(id)
    {   
        $http.get(serviceBase + "api/Clientes/EditCliente/" + id)
            .success(function (data, status, headers, config) {
                if (data.cliente.numCliente != $stateParams.id)
                {
                    //$state.go("app.clientes.edit", { id: data.cliente.numCliente });
                }
              $scope.dados = data;
              $scope.cliente = data.cliente;
          });

    }
    carregarCliente($stateParams.id);

    $scope.submit = function ()
    {
        $http.post(serviceBase + "api/Clientes/AtualizaCliente/", $scope.cliente)
        .success(function (data, status, headers, config) {
           
            toaster.pop("success", "Cliente Atualizado", "");
            $state.go("app.clientes.tabela");
        });
    }


}]);
