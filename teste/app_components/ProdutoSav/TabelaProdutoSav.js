'use strict';

/* Controllers */

app.controller('produtoSavsTabelaCtrl', ['$scope', '$filter', '$http', 'ngTableParams',
function ($scope, $filter, $http, ngTableParams, blockUI) {
    // blockUI.start();
    $scope.dados = [];
    var data = $scope.dados;
    //  blockUI.stop();
    $http.get(serviceBase + "api/ProdutoSavs/TabelaProdutoSav")
        .success(function (data, status, headers, config) {
            $scope.dados = data;
            //console.log(data);
            data = $scope.dados;
            //   blockUI.stop();
            $scope.tableParams = new ngTableParams({
                page: 1,            // show first page
                count: 100,         // count per page                    
                // count per page
                // count per page
                filter: {
                    //nome: ''       // initial filter
                },
                sorting: {
                    //nome: 'asc'     // initial sorting
                }
            }, {
                total: data.length, // length of data
                getData: function ($defer, params) {
                    // use build-in angular filter
                    var filteredData = params.filter() ?
                             $filter('filter')(data, params.filter()) : data;

                    var orderedData = params.sorting() ?
                            $filter('orderBy')(filteredData, params.orderBy()) :
                            data;

                    params.total(orderedData.length); // set total for recalc pagination
                    $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });
        });
}]);
//api/AdminOperadores/OperadoresAtivos