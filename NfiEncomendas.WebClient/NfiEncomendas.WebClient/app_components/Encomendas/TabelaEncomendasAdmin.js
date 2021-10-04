'use strict';

/* Controllers */

app.controller('tabelaEncomendasAdminCtrl', ['$scope', '$rootScope', '$filter', '$http', 'toaster',
    function ($scope, $rootScope, $filter, $http, toaster) {
        // blockUI.start();
        
        $scope.dados = [];
        var data = $scope.dados;

        $scope.cliente = {};
        $scope.clientesLista = [];

        $scope.clientesSelect = {};
        // $scope.clientesSelect.colors = ['Blue', 'Red'];
        $scope.clientesSelect.selectedclientesLista = [];// [$scope.clientesLista[5], $scope.clientesLista[4]];
        $scope.clientesSelect.selectedclientesListaWithGroupBy = [];// [$scope.clientesLista[8], $scope.clientesLista[6]];
        $scope.clientesSelect.selectedclientesListaSimple = [];// ['samantha@email.com', 'wladimir@email.com'];

        $scope.estado = {};

        $scope.estadosLista = [{ nome: "Pendente", numEstado: 0 }, { nome: "Em Produção", numEstado: 1 }, { nome: "Pronta", numEstado: 4 }, { nome: "Entregue", numEstado: 2 }, { nome: "Cancelada", numEstado: 3 }];
        $scope.estadosSelect = {};
        $scope.estadosSelect.selectedEstadosLista = [];
        $scope.estadosSelect.selectedEstadosListaWithGroupBy = [];
        $scope.estadosSelect.selectedEstadosListaSimple = [];
        $scope.tiposEncomendaLista = [];
        $scope.tiposEncomendaSelect = {};
        $scope.tiposEncomendaSelect.selectedEstadosLista = [];
        $scope.tiposEncomendaSelect.selectedEstadosListaWithGroupBy = [];
        $scope.tiposEncomendaSelect.selectedEstadosListaSimple = [];

        $scope.dataPedido = "2015-01-01";
        $scope.DataPedidoOpened = false;

        $http.get(serviceBase + "api/Encomendas/PesquisaEncomenda")
            .success(function (data, status, headers, config) {
                $scope.seriesLista = [{ "numSerie": "--" }].concat(data.series);
                $scope.paramsPesquisa.serie = $scope.seriesLista[0].numSerie;

                $scope.clientesLista = data.clientes;
                $scope.tiposEncomendaLista = data.tipoEncomenda;
                $scope.setoresEncomenda = data.setoresEncomenda;
            });

        $scope.DataPedidoOpen = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.DataPedidoOpened = true;
        };

        $scope.paramsPesquisa = {};
        $scope.paramsPesquisa.clientes = [];
        $scope.paramsPesquisa.dataPedidoDesdeBool = true;
        $scope.paramsPesquisa.dataPedidoDesdeValue = $scope.dataPedido = angular.copy($rootScope._utilizador.dashboardDesde).split("T")[0];
        $scope.paramsPesquisa.dataPedidoAteBool = false;
        $scope.paramsPesquisa.dataPedidoAteValue = "";
        $scope.paramsPesquisa.dataEntregaDesdeBool = false;
        $scope.paramsPesquisa.dataEntregaDesdeValue = "";
        $scope.paramsPesquisa.dataEntregaAteBool = false;
        $scope.paramsPesquisa.dataEntregaAteValue = "";
        $scope.paramsPesquisa.mostrarFaturados = 1;
        $scope.paramsPesquisa.nomeObra = "";
        $scope.paramsPesquisa.fatura = "";
        $scope.paramsPesquisa.agruparCliente = false;
        $scope.paramsPesquisa.ordenacao = 2;

        $scope.pesqRes = {};
        $scope.pesqRes.encomendas = [];

        $scope.submit = function () {
            $http.post(serviceBase + "api/Encomendas/PesquisaEncomendaAdmin/?pesqParam=", $scope.paramsPesquisa)
                .success(function (data, status, headers, config) {
                    var encomendaPad = "00000";
                    $scope.pesqRes = data;
                    $scope.pesqRes.encomendas.forEach(function (e) {
                        if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        e.estadoDesc = $scope.estadosLista.find(function (el) {
                            return el.numEstado == e.estado;
                        }).nome;
                    });
                    $scope.paramsPesquisa.agruparCliente = $scope.paramsPesquisa.ordenacao == 3 || $scope.paramsPesquisa.ordenacao == 5 || $scope.paramsPesquisa.ordenacao == 7;

                    toaster.pop("success", "Pesquisa OK.", "");
                });
        }

        $scope.linkPdf = function () {
            $scope.paramsPesquisa.tipoRetorno = "phantom-pdf";
            var res = "\"" + ((JSON.stringify($scope.paramsPesquisa))).replace(/"/g, "'") + "\"";
            return res;
        }

        $scope.pdfPesquisa = function () {
            $scope.paramsPesquisa.tipoRetorno = "phantom-pdf";

            $http.post(serviceBase + "api/Encomendas/RelatorioEncomendasPdf/", $scope.paramsPesquisa)
                .success(function (data, status, headers, config) {
                    //.attr("href","/pdf/"+data);
                    window.open(serviceBase + "/Relatorios/Relatorio/" + data.uniqueId);
                });
        }

        $scope.addSetor = function () {
            const selectedSetor = $scope.paramsPesquisa.setorEncomenda;
            const setoresFiltered = $scope.tiposEncomendaLista.filter(x => x.setorId == selectedSetor);
            $scope.paramsPesquisa.tipoEncomenda = [...$scope.paramsPesquisa.tipoEncomenda, ...setoresFiltered];
        }
        $scope.show = true;
    }]);
