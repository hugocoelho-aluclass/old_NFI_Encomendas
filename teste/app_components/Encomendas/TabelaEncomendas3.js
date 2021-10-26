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
        $scope.dados.emProducaoCols = 2;

        /**
         * Iniciar variaveis para esconder e dimensionara tabelas
         * */
        $scope.entregaTipo = true;
        $scope.entregaTipoCols = 2;
        $scope.concluidoTipo = true;
        $scope.concluidoTipoCols = 2;
        $scope.produzidoTipo = true;
        $scope.produzidoTipoCols = 1;

        $scope.entrega = true;
        $scope.entregaCols = 2;
        $scope.concluido = true;
        $scope.concluidoCols = 2;
        $scope.produzido = true;
        $scope.produzidoCols = 1;


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
                    $scope.tabelas.tipoEncEntregas = data.tabelaTipoEncomenda;
                    $scope.tabelas.totalEncomendas = data.totaisTabela;

                    for (let i = 0; i < $scope.setoresEncomenda.length; i++) {
                        //em cada setor criar as variaveis para gravar os totais de encomendas para entrega e já produzidas.
                        $scope.setoresEncomenda[i].totalPrix = 0;
                        $scope.setoresEncomenda[i].totalWis = 0;
                        $scope.setoresEncomenda[i].totalResto = 0;
                        $scope.setoresEncomenda[i].total = 0;
                        $scope.setoresEncomenda[i].totalConcluidoPrix = 0;
                        $scope.setoresEncomenda[i].totalConcluidoWis = 0;
                        $scope.setoresEncomenda[i].totalConcluidoResto = 0;
                        $scope.setoresEncomenda[i].totalConcluido = 0;
                        $scope.setoresEncomenda[i].totalProdPrix = 0;
                        $scope.setoresEncomenda[i].totalProdWis = 0;
                        $scope.setoresEncomenda[i].totalProdResto = 0;
                        $scope.setoresEncomenda[i].totalProd = 0;
                        for (let x = 0; x < $scope.tabelas.tipoEncEntregas.length; x++) {
                            if ($scope.setoresEncomenda[i].setorId == $scope.tabelas.tipoEncEntregas[x].setorEncomenda_IdSetorEncomenda) {
                                //calcula e preenche as variaveis criadas
                                $scope.setoresEncomenda[i].totalPrix += $scope.tabelas.tipoEncEntregas[x].totalPrix;
                                $scope.setoresEncomenda[i].totalWis += $scope.tabelas.tipoEncEntregas[x].totalWis;
                                $scope.setoresEncomenda[i].totalResto += $scope.tabelas.tipoEncEntregas[x].totalResto;
                                $scope.setoresEncomenda[i].totalConcluidoPrix += $scope.tabelas.tipoEncEntregas[x].totalConcluidoPrix;
                                $scope.setoresEncomenda[i].totalConcluidoWis += $scope.tabelas.tipoEncEntregas[x].totalConcluidoWis;
                                $scope.setoresEncomenda[i].totalConcluidoResto += $scope.tabelas.tipoEncEntregas[x].totalConcluidoResto;
                                $scope.setoresEncomenda[i].totalProdPrix += $scope.tabelas.tipoEncEntregas[x].totalProdPrix;
                                $scope.setoresEncomenda[i].totalProdWis += $scope.tabelas.tipoEncEntregas[x].totalProdWis;
                                $scope.setoresEncomenda[i].totalProdResto += $scope.tabelas.tipoEncEntregas[x].totalProdResto;

                            }
                        }
                        // calcula total
                        $scope.setoresEncomenda[i].total = $scope.setoresEncomenda[i].totalPrix + $scope.setoresEncomenda[i].totalWis + $scope.setoresEncomenda[i].totalResto;
                        $scope.setoresEncomenda[i].totalConcluido = $scope.setoresEncomenda[i].totalConcluidoPrix + $scope.setoresEncomenda[i].totalConcluidoWis + $scope.setoresEncomenda[i].totalConcluidoResto;
                        $scope.setoresEncomenda[i].totalProd = $scope.setoresEncomenda[i].totalProdPrix + $scope.setoresEncomenda[i].totalProdWis + $scope.setoresEncomenda[i].totalProdResto;
                    }
                    console.log($scope);
                });
            //guarda os valores da pesquisa noutra variavel para apresentar no inicio do relatório, sem que seja alterado quando alteramos os valores na form
            $scope.dados.semana = $scope.paramsPesquisa.semanaEntrega;
            $scope.dados.serie = $scope.paramsPesquisa.serie.numSerie;
            
        }

        /**
         * função para fazer download do relatório Excel
         * o parametro { responseType: 'arraybuffer' }, indica que vai receber da API os dados como um arraybuffer
         * blob: classe Binary Large Objetc
         * Guardamos esses dados classe blob e indicamos o formato/aplicação
         * e criamos um elemento html com o link para o ficheiro simulamos o evento click, para iniciar o download.
         * */
        $scope.downloadExcel = function () {
            $http.post(serviceBase + "api/NPOI/RelatorioProdSemanal/?pesqParam=", $scope.paramsPesquisa, { responseType: 'arraybuffer' })
                .success(function (data, status, headers, config) {
                    var file = new Blob([data], { type: 'application/x-www-form-urlencoded' });
                    var fileURL = URL.createObjectURL(file);
                    var hiddenElement = document.createElement('a');
                    hiddenElement.href = fileURL
                    hiddenElement.target = '_blank';
                    hiddenElement.download = 'relatorio.xlsx';
                    hiddenElement.click();
                    
                });
        }

        /**
         * Funções para expandir colunas da tabela
         * */
        $scope.expandirEntrega = function () {
            if ($scope.entrega === true) {
                $scope.entrega = false;
                $scope.entrega = false;
                $scope.entregaCols = 5;
            } else if ($scope.entrega === false) {
                $scope.entrega = true;
                $scope.entregaCols = 2
            }     
        }

        $scope.expandirConcluido = function () {
            if ($scope.concluido === true) {
                $scope.concluido = false;
                $scope.concluidoCols = 5;
            } else if ($scope.concluido === false) {
                $scope.concluido = true;
                $scope.concluidoCols = 2;
            }
        }

        $scope.expandirProduzido = function () {
            if ($scope.produzido === true) {
                $scope.produzido = false;
                $scope.produzidoCols = 4;
            } else if ($scope.produzido === false) {
                $scope.produzido = true;
                $scope.produzidoCols = 1;
            }
        }

        $scope.expandirEntregaTipo = function () {
            if ($scope.entregaTipo === true) {
                $scope.entregaTipo = false;
                $scope.entregaTipoCols = 5;
            } else if ($scope.entregaTipo === false) {
                $scope.entregaTipo = true;
                $scope.entregaTipoCols = 2
            }
        }

        $scope.expandirConcluidoTipo = function () {
            if ($scope.concluidoTipo === true) {
                $scope.concluidoTipo = false;
                $scope.concluidoTipoCols = 5;
            } else if ($scope.concluidoTipo === false) {
                $scope.concluidoTipo = true;
                $scope.concluidoTipoCols = 2;
            }
        }

        $scope.expandirProduzidoTipo = function () {
            if ($scope.produzidoTipo === true) {
                $scope.produzidoTipo = false;
                $scope.produzidoTipoCols = 4;
            } else if ($scope.produzidoTipo === false) {
                $scope.produzidoTipo = true;
                $scope.produzidoTipoCols = 1;
            }
        }


        //sempre que se seleciona uma serie/ano diferente ele verifica qual a última semana do ano, para definir um limite na form do ano
        $scope.change = function () {
            $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));

        }
        
        $scope.show = true;
    }]);


