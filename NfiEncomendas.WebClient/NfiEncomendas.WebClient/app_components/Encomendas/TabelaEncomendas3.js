'use strict';

/* Controllers */

app.controller('tabela3EncomendasCtrl', ['$scope', '$rootScope', '$filter', '$http', 'toaster',
    function ($scope, $rootScope, $filter, $http, toaster) {

        //obter número da semana atual
        Date.prototype.getWeekNumber = function () {
            var d = new Date(Date.UTC(this.getFullYear(), this.getMonth(), this.getDate()));
            var dayNum = d.getUTCDay() || 7;
            d.setUTCDate(d.getUTCDate() + 4 - dayNum);
            var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
            return Math.ceil((((d - yearStart) / 86400000) + 1) / 7)
        };

        $scope.maxSemana = function (ano) {
            var semana = new Date(ano, 11, 31).getWeekNumber();
            if (semana == 1) {
                semana = 52;
            }
            return semana;
        };

        $scope.dados = [];
  
        
        $scope.dados.semana = "";
        $scope.dados.serie = "";
        $scope.dados.uSemana = "";

        //calculo para obter primeiro e ultimo dia da semana
    /*    var simple = new Date(2023, 0, 1 + (40 - 1) * 7);
        var dow = simple.getDay();
        $scope.dados.isemana = simple;
        if (dow <= 4)
            $scope.dados.isemana.setDate(simple.getDate() - simple.getDay() + 1);
        else
            $scope.dados.isemana.setDate(simple.getDate() + 8 - simple.getDay());

        $scope.dados.fsemana = new Date($scope.dados.isemana);
        $scope.dados.fsemana.setDate($scope.dados.isemana.getDate() + 6);*/

        $scope.paramsPesquisa = {};
        $scope.paramsPesquisa.semanaEntrega = new Date().getWeekNumber();
        //$scope.paramsPesquisa.serie = new Date().getFullYear();
        $scope.paramsPesquisa.serie = "";
        /*$scope.paramsPesquisa.isemana = $scope.dados.isemana;
        $scope.paramsPesquisa.fsemana = $scope.dados.fsemana.toISOString().split('T')[0];*/

        $scope.setoresEncomenda = [];
        $scope.series = [];
        
        $scope.tabelas = {};
        $scope.tabelas.tipoEncEntregas = [];
        $scope.tabelas.totalEncomendas = [];



        $http.get(serviceBase + "api/Encomendas/RelatorioSemanaDados")
            .success(function (data, status, headers, config) {
                $scope.setoresEncomenda = data.setoresEncomenda;
                $scope.series = data.series;
                $scope.paramsPesquisa.serie = $scope.series[$scope.series.length - 1];
                $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));
            });

        $scope.submit = function () {
            $http.post(serviceBase + "api/Encomendas/PesquisaEncomendaTeste/?pesqParam=", $scope.paramsPesquisa)
                .success(function (data, status, headers, config) {
                    $scope.tabelas.tipoEncEntregas = data.listaProduzir;
                    $scope.tabelas.totalEncomendas = data.totalProduzido;

                    for (let i = 0; i < $scope.setoresEncomenda.length; i++) {
                        $scope.setoresEncomenda[i].totalPrix = 0;
                        $scope.setoresEncomenda[i].totalWis = 0;
                        $scope.setoresEncomenda[i].totalResto = 0;
                        $scope.setoresEncomenda[i].total = 0;
                        $scope.setoresEncomenda[i].totalPrixProd = 0;
                        $scope.setoresEncomenda[i].totalWisProd = 0;
                        $scope.setoresEncomenda[i].totalRestoProd = 0;
                        $scope.setoresEncomenda[i].totalProd = 0;
                        for (let x = 0; x < $scope.tabelas.tipoEncEntregas.length; x++) {
                            if ($scope.setoresEncomenda[i].setorId == $scope.tabelas.tipoEncEntregas[x].setorEncomenda_IdSetorEncomenda) {
                                $scope.setoresEncomenda[i].totalPrix += $scope.tabelas.tipoEncEntregas[x].totalPrix;
                                $scope.setoresEncomenda[i].totalWis += $scope.tabelas.tipoEncEntregas[x].totalWis;
                                $scope.setoresEncomenda[i].totalResto += $scope.tabelas.tipoEncEntregas[x].totalResto;
                                $scope.setoresEncomenda[i].totalPrixProd += $scope.tabelas.tipoEncEntregas[x].totalPrixProd;
                                $scope.setoresEncomenda[i].totalWisProd += $scope.tabelas.tipoEncEntregas[x].totalWisProd;
                                $scope.setoresEncomenda[i].totalRestoProd += $scope.tabelas.tipoEncEntregas[x].totalRestoProd;

                            }
                        }
                        $scope.setoresEncomenda[i].total = $scope.setoresEncomenda[i].totalPrix + $scope.setoresEncomenda[i].totalWis + $scope.setoresEncomenda[i].totalResto;
                        $scope.setoresEncomenda[i].totalProd = $scope.setoresEncomenda[i].totalPrixProd + $scope.setoresEncomenda[i].totalWisProd + $scope.setoresEncomenda[i].totalRestoProd;
                    }
                    
                });

            $scope.dados.semana = $scope.paramsPesquisa.semanaEntrega;
            $scope.dados.serie = $scope.paramsPesquisa.serie.numSerie;
 
            console.log($scope);
            
        }


        $scope.change = function () {
            $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));
            console.log($scope);
        }
        

        console.log($scope);
        $scope.show = true;
    }]);


