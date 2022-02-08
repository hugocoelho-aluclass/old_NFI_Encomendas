'use strict';

/* Controllers */

app.controller('relatorioEncomendaCtrl', ['$scope', '$rootScope', '$filter', '$http', 'toaster',
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


        /**
         * Iniciar variaveis para esconder e dimensionara tabelas
         * */
        $scope.capacidadeTipo = true;
        $scope.capacidadeTipoCols = 2;
        $scope.entregaTipo = true;
        $scope.entregaTipoCols = 2;
        $scope.concluidoTipo = true;
        $scope.concluidoTipoCols = 2;
        $scope.porConcluirTipo = true;
        $scope.porConcluirTipoCols = 2;
        $scope.produzidoTipo = true;
        $scope.produzidoTipoCols = 2;
        $scope.atrasadoTipo = true;
        $scope.atrasadoTipoCols = 1;

        $scope.capacidade = true;
        $scope.capacidadeCols = 2;
        $scope.entrega = true;
        $scope.entregaCols = 2;
        $scope.concluido = true;
        $scope.concluidoCols = 2;
        $scope.porConcluir = true;
        $scope.porConcluirCols = 2;
        $scope.produzido = true;
        $scope.produzidoCols = 2;
        $scope.atrasado = true;
        $scope.atrasadoCols = 1;


        //padrão de cores para as células
        $scope.corVermelho = 'rgba(255, 99, 71, 0.2)';
        $scope.corAmarelo = 'rgba(255, 220, 0, 0.4)';
        $scope.corAzul = 'rgba(0, 100, 255, 0.2)';
        $scope.corVerde = 'rgba(60, 179, 113, 0.2)';


        //array para guardar o objeto com os parametros de pesquisa, como a semana de entrega e a serie(ano)
        $scope.paramsPesquisa = {};
        $scope.paramsPesquisa.semanaEntrega = new Date().getWeekNumber();
        $scope.paramsPesquisa.ateSemanaEntrega = $scope.paramsPesquisa.semanaEntrega + 1;
        $scope.paramsPesquisa.semanaEntregaAteBool = false;
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

        //submit da pesquisa, pela semana e serie(ano), para obter dados para o relatório
        $scope.submit = function () {
            //caso seja selecionado um intervalo de tempo
            if ($scope.paramsPesquisa.semanaEntregaAteBool) {
                $http.post(serviceBase + "api/Relatorios/PesquisaRelatorioEncomendasAte/?pesqParam=", $scope.paramsPesquisa)
                    .success(function (data, status, headers, config) {
                        $scope.tabelas.tipoEncEntregas = null;
                        $scope.listaSemanas = [];

                        // percorre todas as semanas, para preencher o array local e criar novos campos   
                        for (let i = 0; i < data.listaTabelasRelatorio.length; i++) {
                            $scope.listaSemanas.push(new Object());
                            $scope.listaSemanas[i].tipoEncEntregas = data.listaTabelasRelatorio[i].tabelaTipoEncomenda;
                            $scope.listaSemanas[i].totaisTabela = data.listaTabelasRelatorio[i].totaisTabela;
                            $scope.listaSemanas[i].ano = data.listaTabelasRelatorio[i].ano;
                            $scope.listaSemanas[i].semana = data.listaTabelasRelatorio[i].semana;
                            //variavel para esconder e mostrar os dados detalhados da semana
                            $scope.listaSemanas[i].show = false;
                            $scope.listaSemanas[i].setoresEncomenda = [];

                            // percorre os setores para adicionar os totais de todas as semanas
                            for (let it = 0; it < $scope.setoresEncomenda.length; it++) {
                                $scope.listaSemanas[i].setoresEncomenda.push(new Object());
                                $scope.listaSemanas[i].setoresEncomenda[it].setorId = $scope.setoresEncomenda[it].setorId;
                                $scope.listaSemanas[i].setoresEncomenda[it].setorNome = $scope.setoresEncomenda[it].setorNome;
                                $scope.listaSemanas[i].setoresEncomenda[it].TiposEncomenda = $scope.setoresEncomenda[it].TiposEncomenda;
                            }

                        }

                        //percorre a lista das semanas e adicionar cores as celulas
                        for (let i = 0; i < $scope.listaSemanas.length; i++) {
                            $scope.adicionarCor($scope.listaSemanas[i]);
                            $scope.criarTotais($scope.listaSemanas[i].setoresEncomenda, $scope.listaSemanas[i]);
                        }

                        $scope.setorTotais();
                        $scope.semanasTotais();

                        console.log($scope);
                    });

            } else {
                
                $http.post(serviceBase + "api/Relatorios/PesquisaRelatorioEncomendas/?pesqParam=", $scope.paramsPesquisa)
                    .success(function (data, status, headers, config) {
                        //guardar tabelas da query nas variaveis da scope
                        $scope.listaSemanas = null;
                        $scope.tabelas.tipoEncEntregas = data.tabelaTipoEncomenda;
                        $scope.tabelas.totalEncomendas = data.totaisTabela;
                       
                        //adicionar cores nas celulas
                        $scope.adicionarCor($scope.tabelas);              

                        //criar totais das tabelas
                        $scope.criarTotais($scope.setoresEncomenda, $scope.tabelas);

                    });
            }
            
            
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
        $scope.expandirCapacidade = function () {
            if ($scope.capacidade === true) {
                $scope.capacidade = false;
                $scope.capacidade = false;
                $scope.capacidadeCols = 5;
            } else if ($scope.capacidade === false) {
                $scope.capacidade = true;
                $scope.capacidadeCols = 2
            }
        }

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

        $scope.expandirPorConcluir = function () {
            if ($scope.porConcluir === true) {
                $scope.porConcluir = false;
                $scope.porConcluirCols = 5;
            } else if ($scope.porConcluir === false) {
                $scope.porConcluir = true;
                $scope.porConcluirCols = 2;
            }
        }

        $scope.expandirProduzido = function () {
            if ($scope.produzido === true) {
                $scope.produzido = false;
                $scope.produzidoCols = 5;
            } else if ($scope.produzido === false) {
                $scope.produzido = true;
                $scope.produzidoCols = 2;
            }
        }

        $scope.expandirAtrasado = function () {
            if ($scope.atrasado === true) {
                $scope.atrasado = false;
                $scope.atrasadoCols = 4;
            } else if ($scope.atrasado === false) {
                $scope.atrasado = true;
                $scope.atrasadoCols = 1;
            }
        }

        $scope.expandirCapacidadeTipo = function () {
            if ($scope.capacidadeTipo === true) {
                $scope.capacidadeTipo = false;
                $scope.capacidadeTipoCols = 5;
            } else if ($scope.capacidadeTipo === false) {
                $scope.capacidadeTipo = true;
                $scope.capacidadeTipoCols = 2
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

        $scope.expandirPorConcluirTipo = function () {
            if ($scope.porConcluirTipo === true) {
                $scope.porConcluirTipo = false;
                $scope.porConcluirTipoCols = 5;
            } else if ($scope.porConcluirTipo === false) {
                $scope.porConcluirTipo = true;
                $scope.porConcluirTipoCols = 2;
            }
        }


        $scope.expandirProduzidoTipo = function () {
            if ($scope.produzidoTipo === true) {
                $scope.produzidoTipo = false;
                $scope.produzidoTipoCols = 5;
            } else if ($scope.produzidoTipo === false) {
                $scope.produzidoTipo = true;
                $scope.produzidoTipoCols = 2;
            }
        }

        $scope.expandirAtrasadoTipo = function () {
            if ($scope.atrasadoTipo === true) {
                $scope.atrasadoTipo = false;
                $scope.atrasadoTipoCols = 4;
            } else if ($scope.atrasadoTipo === false) {
                $scope.atrasadoTipo = true;
                $scope.atrasadoTipoCols = 1;
            }
        }

        /**
         * função para expandir semana
         * @param {any} i - numero da semana do array
         */
        $scope.expandirSemana = function (i) {
            if ($scope.listaSemanas[i].show == true) {
                $scope.listaSemanas[i].show = false
            } else {
                $scope.listaSemanas[i].show = true
            }
        }


        /**
         * Função para calcular totais por setor/semana e definir cores
         * @param {any} objSetor, variavel com objeto do setor que se vai calcular os totais
         * @param {any} objTipo, variavel com o objeto com os totais e tipos de encomenda
         */
        $scope.criarTotais = function (objSetor, objTipo) {
            for (let i = 0; i < objSetor.length; i++) {
                //em cada setor cria as variaveis para gravar os totais de encomendas para entrega e já produzidas.
                objSetor[i].totalPrix = 0;
                objSetor[i].totalWis = 0;
                objSetor[i].totalResto = 0;
                objSetor[i].total = 0;
                objSetor[i].totalConcluidoPrix = 0;
                objSetor[i].totalConcluidoWis = 0;
                objSetor[i].totalConcluidoResto = 0;
                objSetor[i].totalConcluido = 0;
                objSetor[i].totalProdPrix = 0;
                objSetor[i].totalProdWis = 0;
                objSetor[i].totalProdResto = 0;
                objSetor[i].totalProd = 0;
                objSetor[i].totalAtrasadoPrix = 0;
                objSetor[i].totalAtrasadoWis = 0;
                objSetor[i].totalAtrasadoResto = 0;
                objSetor[i].totalAtrasado = 0;
                objSetor[i].totalPrixCor = $scope.corVerde;
                objSetor[i].totalWisCor = $scope.corVerde;
                objSetor[i].totalRestoCor = $scope.corVerde;
                objSetor[i].totalProdPrixCor = "";
                objSetor[i].totalProdWisCor = "";
                objSetor[i].totalProdRestoCor = "";
                objSetor[i].totalCor = "";
                objSetor[i].totalProdCor = "";

                for (let x = 0; x < objTipo.tipoEncEntregas.length; x++) {
                    if (objSetor[i].setorId == objTipo.tipoEncEntregas[x].setorEncomenda_IdSetorEncomenda) {
                        //calcula e preenche as variaveis criadas
                        objSetor[i].totalPrix += objTipo.tipoEncEntregas[x].totalPrix;
                        objSetor[i].totalWis += objTipo.tipoEncEntregas[x].totalWis;
                        objSetor[i].totalResto += objTipo.tipoEncEntregas[x].totalResto;
                        objSetor[i].totalConcluidoPrix += objTipo.tipoEncEntregas[x].totalConcluidoPrix;
                        objSetor[i].totalConcluidoWis += objTipo.tipoEncEntregas[x].totalConcluidoWis;
                        objSetor[i].totalConcluidoResto += objTipo.tipoEncEntregas[x].totalConcluidoResto;
                        objSetor[i].totalProdPrix += objTipo.tipoEncEntregas[x].totalProdPrix;
                        objSetor[i].totalProdWis += objTipo.tipoEncEntregas[x].totalProdWis;
                        objSetor[i].totalProdResto += objTipo.tipoEncEntregas[x].totalProdResto;
                        objSetor[i].totalAtrasadoPrix += objTipo.tipoEncEntregas[x].totalAtrasadoPrix;
                        objSetor[i].totalAtrasadoWis += objTipo.tipoEncEntregas[x].totalAtrasadoWis;
                        objSetor[i].totalAtrasadoResto += objTipo.tipoEncEntregas[x].totalAtrasadoResto;
                        $scope.corPorSemana(objSetor[i], objTipo.tipoEncEntregas[x]);
                    }
                }

                // calcula total
                objSetor[i].total = objSetor[i].totalPrix + objSetor[i].totalWis + objSetor[i].totalResto;
                objSetor[i].totalConcluido = objSetor[i].totalConcluidoPrix + objSetor[i].totalConcluidoWis + objSetor[i].totalConcluidoResto;
                objSetor[i].totalProd = objSetor[i].totalProdPrix + objSetor[i].totalProdWis + objSetor[i].totalProdResto;
                objSetor[i].totalAtrasado = objSetor[i].totalAtrasadoPrix + objSetor[i].totalAtrasadoWis + objSetor[i].totalAtrasadoResto;


                // Atribui cores as celulas dos totais onde já se ultrapassou ou se está no limite da produção máxima do tipo de encomenda.
                if (objSetor[i].totalPrixCor == $scope.corVermelho || objSetor[i].totalWisCor == $scope.corVermelho || objSetor[i].totalRestoCor == $scope.corVermelho) {
                    objSetor[i].totalCor = $scope.corVermelho;
                } else if (objSetor[i].totalPrixCor == $scope.corAmarelo || objSetor[i].totalWisCor == $scope.corAmarelo || objSetor[i].totalRestoCor == $scope.corAmarelo) {
                    objSetor[i].totalCor = $scope.corAmarelo;
                } else {
                    objSetor[i].totalCor = $scope.corVerde;
                }

                if (objSetor[i].totalProdPrixCor == $scope.corAzul || objSetor[i].totalProdWisCor == $scope.corAzul || objSetor[i].totalProdRestoCor == $scope.corAzul) {
                    objSetor[i].totalProdCor = $scope.corAzul;
                } 

            }

        }


        /**
         * Função para atribuir cores as celulas onde já se ultrapassou ou se já está no limite da produção máxima do tipo de encomenda por setor.
         * @param {any} corTotal, objeto setor
         * @param {any} corCampo, objeto tipo de encomenda
         */
        $scope.corPorSemana = function (corTotal, corCampo) {
            //verifica todos os campos de uma semana, para decidir a cor da celula
            if (corTotal.totalPrixCor != $scope.corVermelho) {
                if (corCampo.totalPrixCor == $scope.corVermelho) {
                    corTotal.totalPrixCor = $scope.corVermelho;
                } else if (corCampo.totalPrixCor == $scope.corAmarelo) {
                    corTotal.totalPrixCor = $scope.corAmarelo;
                }
            }

            if (corTotal.totalWisCor != $scope.corVermelho) {
                if (corCampo.totalWisCor == $scope.corVermelho) {
                    corTotal.totalWisCor = $scope.corVermelho;
                } else if (corCampo.totalWisCor == $scope.corAmarelo) {
                    corTotal.totalWisCor = $scope.corAmarelo;
                }
            }

            if (corTotal.totalRestoCor != $scope.corVermelho) {
                if (corCampo.totalRestoCor == $scope.corVermelho) {
                    corTotal.totalRestoCor = $scope.corVermelho;
                } else if (corCampo.totalRestoCor == $scope.corAmarelo) {
                    corTotal.totalRestoCor = $scope.corAmarelo;
                }
            }


            //se na coluna tiver alguma cor, altera o total da mesma para a mesma cor
            if (corTotal.totalProdPrixCor != $scope.corAzul && corCampo.totalProdPrixCor == $scope.corAzul) {
                corTotal.totalProdPrixCor = $scope.corAzul;
            }

            if (corTotal.totalProdWisCor != $scope.corAzul && corCampo.totalProdWisCor == $scope.corAzul) {
                corTotal.totalProdWisCor = $scope.corAzul;
            }

            if (corTotal.totalProdRestoCor != $scope.corAzul && corCampo.totalProdRestoCor == $scope.corAzul) {
                corTotal.totalProdRestoCor = $scope.corAzul;
            }
        }


        /**
         * Função para atribuir cores às celulas onde o numero de encomendas já ultrapassou ou está perto do limite de produção
         * @param {any} obj, objeto que com as tabelas e totais
         */
        $scope.adicionarCor = function (obj) {
            //percorre todo o tipo de encomendas
            for (let x = 0; x < obj.tipoEncEntregas.length; x++) {
                //atribui a cor a coluna da semana para as encomendas, através da função cor
                obj.tipoEncEntregas[x].totalPrixCor = $scope.cor(obj.tipoEncEntregas[x].capacidadePrix, obj.tipoEncEntregas[x].totalPrix);
                obj.tipoEncEntregas[x].totalWisCor = $scope.cor(obj.tipoEncEntregas[x].capacidadeWis, obj.tipoEncEntregas[x].totalWis);
                obj.tipoEncEntregas[x].totalRestoCor = $scope.cor(obj.tipoEncEntregas[x].capacidadeResto, obj.tipoEncEntregas[x].totalResto);

                //caso algum das colunas tenha alguma das cores de maior importância, altera a cor da celula de total dessa semana
                if (obj.tipoEncEntregas[x].totalPrixCor == $scope.corVermelho || obj.tipoEncEntregas[x].totalWisCor == $scope.corVermelho || obj.tipoEncEntregas[x].totalRestoCor == $scope.corVermelho) {
                    obj.tipoEncEntregas[x].totalCor = $scope.corVermelho;
                } else if (obj.tipoEncEntregas[x].totalPrixCor == $scope.corAmarelo || obj.tipoEncEntregas[x].totalWisCor == $scope.corAmarelo || obj.tipoEncEntregas[x].totalRestoCor == $scope.corAmarelo) {
                    obj.tipoEncEntregas[x].totalCor = $scope.corAmarelo;
                } else {
                    obj.tipoEncEntregas[x].totalCor = $scope.corVerde;
                }

                //atribui a cor a coluna da semana para o produção, através da função cor
                obj.tipoEncEntregas[x].totalProdPrixCor = $scope.corProduzido(obj.tipoEncEntregas[x].capacidadePrix, obj.tipoEncEntregas[x].totalProdPrix);
                obj.tipoEncEntregas[x].totalProdWisCor = $scope.corProduzido(obj.tipoEncEntregas[x].capacidadeWis, obj.tipoEncEntregas[x].totalProdWis);
                obj.tipoEncEntregas[x].totalProdRestoCor = $scope.corProduzido(obj.tipoEncEntregas[x].capacidadeResto, obj.tipoEncEntregas[x].totalProdResto);

                //caso algum das colunas tenha ultrapassado o máximo de produção da semana, altera a cor da celula do total dessa semana, para a mesma cor 
                if (obj.tipoEncEntregas[x].totalProdPrixCor == $scope.corAzul || obj.tipoEncEntregas[x].totalProdWisCor == $scope.corAzul || obj.tipoEncEntregas[x].totalProdRestoCor == $scope.corAzul) {
                    obj.tipoEncEntregas[x].totalProdCor = $scope.corAzul;
                } else {
                    obj.tipoEncEntregas[x].totalProdCor = null;
                }

                //altera a cor do total das colunas das encomendas, com a cor com o maior grau de importancia presente nessa coluna
                if (obj.tipoEncEntregas[x].totalCor == $scope.corVermelho) {
                    obj.totalC = $scope.corVermelho;
                    if (obj.tipoEncEntregas[x].totalPrixCor == $scope.corVermelho) {
                        obj.totalPrixC = $scope.corVermelho;
                    } else if (obj.tipoEncEntregas[x].totalPrixCor == $scope.corAmarelo) {
                        obj.totalPrixC = $scope.corAmarelo;
                    }
                    if (obj.tipoEncEntregas[x].totalWisCor == $scope.corVermelho) {
                        obj.totalWisC = $scope.corVermelho;
                    } else if (obj.tipoEncEntregas[x].totalWisCor == $scope.corAmarelo) {
                        obj.totalWisC = $scope.corAmarelo;
                    }
                    if (obj.tipoEncEntregas[x].totalRestoCor == $scope.corVermelho) {
                        obj.totalRestoC = $scope.corVermelho;
                    } else if (obj.tipoEncEntregas[x].totalRestoCor == $scope.corAmarelo) {
                        obj.totalRestoC = $scope.corAmarelo;
                    }
                } else if (obj.tipoEncEntregas[x].totalCor == $scope.corAmarelo && obj.totalC != $scope.corVermelho) {
                    obj.totalC = $scope.corAmarelo;
                    if (obj.tipoEncEntregas[x].totalPrixCor == $scope.corAmarelo) {
                        obj.totalPrixC = $scope.corAmarelo;
                    }
                    if (obj.tipoEncEntregas[x].totalWisCor == $scope.corAmarelo) {
                        obj.totalWisC = $scope.corAmarelo;
                    }
                    if (obj.tipoEncEntregas[x].totalRestoCor == $scope.corAmarelo) {
                        obj.totalRestoC = $scope.corAmarelo;
                    }
                }

                //altera a cor do total das colunas da produção, com a cor azul caso esteja presente nessa coluna
                if (obj.tipoEncEntregas[x].totalProdCor == $scope.corAzul) {
                    obj.totalProdC = $scope.corAzul;
                    if (obj.tipoEncEntregas[x].totalProdPrixCor == $scope.corAzul) {
                        obj.totalProdPrixC = $scope.corAzul;
                    }
                    if (obj.tipoEncEntregas[x].totalProdWisCor == $scope.corAzul) {
                        obj.totalProdWisC = $scope.corAzul;
                    }
                    if (obj.tipoEncEntregas[x].totalProdRestoCor == $scope.corAzul) {
                        obj.totalProdRestoC = $scope.corAzul;
                    }
                }
            }
        }


        /**
         * Função para verificar se o numero de encomendas já ultrapassou ou está perto do limite de produção
         * @param {any} cap, capacidade máxima de produção do tipo de encomenda
         * @param {any} total, número de de encomendas
         */
        $scope.cor = function (cap, total) {
            var temp;
            if (cap == 0) {
                temp = 0
            } else {
                temp = total / cap;
            }

            if (temp >= 1 || total > cap) {
                return $scope.corVermelho;
            } else if (temp >= 0.9) {
                return $scope.corAmarelo;
            } else {
                return $scope.corVerde;
            }
        }

        /**
         * Função para verificar se já ultrapassamos o máximo de produção
         * @param {any} cap, capacidade máxima de produção do tipo de encomenda
         * @param {any} total, total já produzido na semana
         */
        $scope.corProduzido = function (cap, total) {
            if (total > cap) {
                return $scope.corAzul;
            } 
            return null;
        }

        /**
         * Função para calcular total de cada setor mediante as semanas escolhidas
         * */
        $scope.setorTotais = function () {
            for (let i = 0; i < $scope.setoresEncomenda.length; i++) {
                $scope.setoresEncomenda[i].totalPrix = $scope.setoresEncomenda[i].totalWis = $scope.setoresEncomenda[i].totalResto = 0;
                $scope.setoresEncomenda[i].totalConcluidoPrix = $scope.setoresEncomenda[i].totalConcluidoWis = $scope.setoresEncomenda[i].totalConcluidoResto = 0;
                $scope.setoresEncomenda[i].totalProdPrix = $scope.setoresEncomenda[i].totalProdWis = $scope.setoresEncomenda[i].totalProdResto = 0;
                for (let x = 0; x < $scope.listaSemanas.length; x++) {

                    $scope.setoresEncomenda[i].totalPrix += $scope.listaSemanas[x].setoresEncomenda[i].totalPrix;
                    $scope.setoresEncomenda[i].totalWis += $scope.listaSemanas[x].setoresEncomenda[i].totalWis;
                    $scope.setoresEncomenda[i].totalResto += $scope.listaSemanas[x].setoresEncomenda[i].totalResto;

                    $scope.setoresEncomenda[i].totalConcluidoPrix += $scope.listaSemanas[x].setoresEncomenda[i].totalConcluidoPrix;
                    $scope.setoresEncomenda[i].totalConcluidoWis += $scope.listaSemanas[x].setoresEncomenda[i].totalConcluidoWis;
                    $scope.setoresEncomenda[i].totalConcluidoResto += $scope.listaSemanas[x].setoresEncomenda[i].totalConcluidoResto;

                    $scope.setoresEncomenda[i].totalProdPrix += $scope.listaSemanas[x].setoresEncomenda[i].totalProdPrix;
                    $scope.setoresEncomenda[i].totalProdWis += $scope.listaSemanas[x].setoresEncomenda[i].totalProdWis;
                    $scope.setoresEncomenda[i].totalProdResto += $scope.listaSemanas[x].setoresEncomenda[i].totalProdResto;

                }
               
            }
           
        }


        /**
         * 
         * */
        $scope.semanasTotais = function () {
            $scope.totais.totalPrix = $scope.totais.totalWis = $scope.totais.totalResto = 0;
            $scope.totais.totalConcluidoPrix = $scope.totais.totalConcluidoWis = $scope.totais.totalConcluidoResto = 0;
            $scope.totais.totalProdPrix = $scope.totais.totalProdWis = $scope.totais.totalProdResto = 0;
            for (let i = 0; i < $scope.listaSemanas.length; i++) {
                $scope.totais.totalPrix += $scope.listaSemanas[i].totaisTabela.totalPrix;
                $scope.totais.totalWis += $scope.listaSemanas[i].totaisTabela.totalWis;
                $scope.totais.totalResto += $scope.listaSemanas[i].totaisTabela.totalResto;

                $scope.totais.totalConcluidoPrix += $scope.listaSemanas[i].totaisTabela.totalConcluidoPrix;
                $scope.totais.totalConcluidoWis += $scope.listaSemanas[i].totaisTabela.totalConcluidoWis;
                $scope.totais.totalConcluidoResto += $scope.listaSemanas[i].totaisTabela.totalConcluidoResto;

                $scope.totais.totalProdPrix += $scope.listaSemanas[i].totaisTabela.totalProdPrix;
                $scope.totais.totalProdWis += $scope.listaSemanas[i].totaisTabela.totalProdWis;
                $scope.totais.totalProdResto += $scope.listaSemanas[i].totaisTabela.totalProdResto;

            }
        }

        /**
         * Sempre que um setor é expandido, coloca a variavel show como false
         * */
        /*$scope.$watch('setoresEncomenda', function (isOpen, t, scope) {
            for (let i = 0; i < $scope.listaSemanas.length; i++) {
                scope.listaSemanas[i].show = false;
            }
        }, true);*/

        

        /**
         * sempre que se seleciona uma serie/ano diferente ele verifica qual a última semana do ano, para definir um limite na form do ano
         * */
        $scope.change = function () {
            $scope.dados.uSemana = $scope.maxSemana(parseInt($scope.paramsPesquisa.serie.numSerie));

        }
        
        $scope.show = true;
    }]);


