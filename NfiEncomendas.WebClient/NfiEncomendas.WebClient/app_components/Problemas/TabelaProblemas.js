'use strict';

/* Controllers */

app.controller('problemasTabelaCtrl', ['$scope', '$rootScope', '$filter', '$state', '$http', 'toaster', 'SavGlobalService', 'ngTableParams',
    function ($scope, $rootScope, $filter, $state, $http, toaster, SavGlobalService, ngTableParams) {
        // blockUI.start();

        $scope.arrEficacia = ["", "Cancelado", "Não Eficaz", "Parcialmente Eficaz", "Eficaz"];

        $http.get(serviceBase + "api/Problemas/TabelaProblemas")
            .success(function (data, status, headers, config) {

                $scope.dados = data;

                for (let i = 0; i < $scope.dados.length; i++) {
                    $scope.dados[i].nomeEficacia = $scope.arrEficacia[$scope.dados[i].eficacia];
                }
                console.log($scope.dados);
                data = $scope.dados;

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
