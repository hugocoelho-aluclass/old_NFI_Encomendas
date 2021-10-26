'use strict';

/* Controllers */

app.controller('EstatisticasGeraisCtrl', ['$scope', '$rootScope', '$filter', '$http', 'toaster',
    function ($scope, $rootScope, $filter, $http, toaster) {
        $scope.pesq = {
            dataDesde: moment().subtract(1, 'months'),
            dataAte: moment()
        };
        $scope.dados = [];
        var data = $scope.dados;
        //$scope.dataDesde = moment().subtract(1, 'months');
        //$scope.dataAte = moment();

        $scope.atualizaDashboard = function () {
            var paramsPesq = {
                dataPesquisaDesde: $scope.pesq.dataDesde,
                dataPesquisaAte: $scope.pesq.dataAte
            };
            $http.post(serviceBase + "api/estatisticas/PesquisaEstatisticas/?pesq=", paramsPesq)
                .success(function (data, status, headers, config) {
                    $scope.pesqRes = data;

                    toaster.pop("success", "Pesquisa OK.", "");
                }
           );
        };

        $scope.atualizaDashboard();
    }
]);
