'use strict';

/* Controllers */

app.controller('relatorioSavCtrl', ['$scope', '$rootScope', '$filter', '$http', 'toaster',
    function ($scope, $rootScope, $filter, $http, toaster) {


        //obter número da semana atual
        Date.prototype.getWeekNumber = function () {
            var d = new Date(Date.UTC(this.getFullYear(), this.getMonth(), this.getDate()));
            var dayNum = d.getUTCDay() || 7;
            d.setUTCDate(d.getUTCDate() + 4 - dayNum);
            var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
            return Math.ceil((((d - yearStart) / 86400000) + 1) / 7)
        };


        //obter ultima semana do ano
        $scope.maxSemana = function (ano) {
            var semana = new Date(ano, 11, 31).getWeekNumber();
            //caso coincida a primeira semana do ano seguinte, altera a ultima semana para 52
            if (semana == 1) {
                semana = 52;
            }
            return semana;
        };


        //array para guardar variaveis, como a semana, a serie(ano) e a ultima semana do ano
        $scope.dados = {};
        $scope.dados.semana = "";
        $scope.dados.serie = "";
        $scope.dados.uSemana = "";
        $scope.dados.emProducaoCols = 2;
        $scope.dados.semanaAtual = new Date().getWeekNumber();
        $scope.totais = {};


        //array para guardar o objeto com os parametros de pesquisa, como a semana de entrega e a serie(ano)
        $scope.paramsPesquisa = {};
        $scope.paramsPesquisa.semanaEntrega = new Date().getWeekNumber();
        $scope.paramsPesquisa.ateSemanaEntrega = $scope.paramsPesquisa.semanaEntrega + 1;
        $scope.paramsPesquisa.semanaEntregaAteBool = false;
        $scope.paramsPesquisa.serie = "";






        //Http request para obter setores e series
        $http.get(serviceBase + "api/Relatorios/RelatorioSemanaDados")
            .success(function (data, status, headers, config) {
                $scope.setoresEncomenda = data.setoresEncomenda;
                $scope.series = data.series;
                //atribui ao select o último valor do array series
                $scope.series.forEach(function (s) {
                    if (s.serieDefeito) {
                        $scope.paramsPesquisa.serie = s;
                    }
                });

                //define o máximo possível para a escolha da semana, apartir do ano
                $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));

            });

        $scope.submit = function () {
            $http.post(serviceBase + "api/Relatorios/PesquisaRelatorioSav/?pesqParam=", $scope.paramsPesquisa)
                .success(function (data, status, headers, config) {
                    $scope.tabelaSav = data;
                    console.log($scope.tabelaSav);
                })
        }


        $scope.change = function () {
            $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));

        }

        $scope.show = true;
    }]);


