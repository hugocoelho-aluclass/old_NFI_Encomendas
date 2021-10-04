'use strict';
//CsvController


app.controller('csvController', ['$scope', '$http', '$stateParams', '$state', 'toaster',
function ($scope, $http, $stateParams, $state, toaster) {
    $scope.submitClientes = function ()
    {
        
        $http.post(serviceBase + "api/CSV/CsvClientes/", $scope.csvClientes)
            .success(function (data, status, headers, config) {

                toaster.pop("success", "CSV Atualizada", "");
              
            });
        
    }
    $scope.submitTiposEncomenda = function ()
    {
        
        $http.post(serviceBase + "api/CSV/CsvTiposEncomenda/", $scope.csvClientes)
            .success(function (data, status, headers, config) {

                toaster.pop("success", "CSV Atualizada", "");
              
            });
    }
    $scope.submitEncomendas = function () {

        $http.post(serviceBase + "api/CSV/CsvEncomendas/", $scope.csvClientes)
            .success(function (data, status, headers, config) {

                toaster.pop("success", "CSV Atualizada", "");

            });
    }
    //novo
    $scope.submitTiposAvaria = function () {

        $http.post(serviceBase + "api/CSV/CsvTiposAvaria/", $scope.csvClientes)
            .success(function (data, status, headers, config) {

                toaster.pop("success", "CSV Atualizada", "");

            });
    }

    $scope.submitProdutos = function () {

        $http.post(serviceBase + "api/CSV/CsvProdutos/", $scope.csvClientes)
            .success(function (data, status, headers, config) {

                toaster.pop("success", "CSV Atualizada", "");

            });
    }
    $scope.submitDepartamentos = function () {

        $http.post(serviceBase + "api/CSV/CsvDepartamentos/", $scope.csvClientes)
            .success(function (data, status, headers, config) {

                toaster.pop("success", "CSV Atualizada", "");

            });
    }
}]);