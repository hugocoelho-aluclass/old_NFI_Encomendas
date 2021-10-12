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
        $scope.dados = [];
        $scope.dados.semana = "";
        $scope.dados.serie = "";
        $scope.dados.uSemana = "";

        //array para guardar o objeto com os parametros de pesquisa, como a semana de entrega e a serie(ano)
        $scope.paramsPesquisa = {};
        $scope.paramsPesquisa.semanaEntrega = new Date().getWeekNumber();
        $scope.paramsPesquisa.serie = "";

        //arrais para guardar os objetos obtidos da base de dados
        //setores
        $scope.setoresEncomenda = [];
        //series(ano)
        $scope.series = [];

        //objeto para guardar as tabelas com a informação obtida para o relatório
        $scope.tabelas = {};
        $scope.tabelas.tipoEncEntregas = [];
        $scope.tabelas.totalEncomendas = [];


        //Http request para obter setores e series
        $http.get(serviceBase + "api/Encomendas/RelatorioSemanaDados")
            .success(function (data, status, headers, config) {
                $scope.setoresEncomenda = data.setoresEncomenda;
                $scope.series = data.series;
                //atribui ao select o último valor do array series
                $scope.paramsPesquisa.serie = $scope.series[$scope.series.length - 1];
                //define o máximo possível para a escolha da semana, apartir do ano
                $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));
            });

        //submit da pesquisa, pela semana e serie(ano), para obter dados para o relatório
        $scope.submit = function () {
            $http.post(serviceBase + "api/Encomendas/PesquisaRelatorioEncomendas/?pesqParam=", $scope.paramsPesquisa)
                .success(function (data, status, headers, config) {
                    //guardar tabelas da query nas variaveis da scope
                    $scope.tabelas.tipoEncEntregas = data.listaProduzir;
                    $scope.tabelas.totalEncomendas = data.totalProduzido;

                    for (let i = 0; i < $scope.setoresEncomenda.length; i++) {
                        //em cada setor criar as variaveis para gravar os totais de encomendas para entrega e já produzidas.
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
                                //calcula e preenche as variaveis criadas
                                $scope.setoresEncomenda[i].totalPrix += $scope.tabelas.tipoEncEntregas[x].totalPrix;
                                $scope.setoresEncomenda[i].totalWis += $scope.tabelas.tipoEncEntregas[x].totalWis;
                                $scope.setoresEncomenda[i].totalResto += $scope.tabelas.tipoEncEntregas[x].totalResto;
                                $scope.setoresEncomenda[i].totalPrixProd += $scope.tabelas.tipoEncEntregas[x].totalPrixProd;
                                $scope.setoresEncomenda[i].totalWisProd += $scope.tabelas.tipoEncEntregas[x].totalWisProd;
                                $scope.setoresEncomenda[i].totalRestoProd += $scope.tabelas.tipoEncEntregas[x].totalRestoProd;

                            }
                        }
                        // calcula total
                        $scope.setoresEncomenda[i].total = $scope.setoresEncomenda[i].totalPrix + $scope.setoresEncomenda[i].totalWis + $scope.setoresEncomenda[i].totalResto;
                        $scope.setoresEncomenda[i].totalProd = $scope.setoresEncomenda[i].totalPrixProd + $scope.setoresEncomenda[i].totalWisProd + $scope.setoresEncomenda[i].totalRestoProd;
                    }
                    
                });
            //guarda os valores da pesquisa noutra variavel para apresentar no inicio do relatório, sem que seja alterado quando alteramos os valores na form
            $scope.dados.semana = $scope.paramsPesquisa.semanaEntrega;
            $scope.dados.serie = $scope.paramsPesquisa.serie.numSerie;
            
        }

        //sempre que se seleciona uma serie/ano diferente ele verifica qual a última semana do ano, para definir um limite na form do ano
        $scope.change = function () {
            $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));
        }
        
        $scope.show = true;
    }]);


