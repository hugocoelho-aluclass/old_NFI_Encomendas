'use strict';

/* Controllers */

app.controller('savsTabelaCtrl', ['$scope', '$rootScope', '$filter','$state', '$http', 'toaster', 'SavGlobalService',
function ($scope, $rootScope, $filter, $state,$http, toaster, SavGlobalService) {
    // blockUI.start();
   
    if ($rootScope._utilizador && $rootScope._utilizador.comercial)
    {
        $state.go('app.dashboard-v1');
    }
    $scope.dados = [];
    var data = $scope.dados;

    $scope.cliente = {};
    $scope.clientesLista = [];
    $scope.serieLista = [];
    $scope.setoresLista = [];

    $scope.clientesSelect = {};
    // $scope.clientesSelect.colors = ['Blue', 'Red'];
    $scope.clientesSelect.selectedclientesLista = [];
    $scope.clientesSelect.selectedclientesListaWithGroupBy = [];
    $scope.clientesSelect.selectedclientesListaSimple = [];


    $scope.departamentosLista = [];
    $scope.departamentosSelect = {};
    $scope.departamentosSelect.selectedDepartamentosLista = [];
    $scope.departamentosSelect.selectedDepartamentosListaWithGroupBy = [];
    $scope.departamentosSelect.selectedDepartamentosListaSimple = [];

    $scope.estadoSavLista = [];
    $scope.estadoSavSelect = {};
    $scope.estadoSavSelect.selectedEstadosLista = [];
    $scope.estadoSavSelect.selectedEstadosListaWithGroupBy = [];
    $scope.estadoSavSelect.selectedEstadosListaSimple = [];


    $scope.tipoAvariaLista = [];
    $scope.tipoAvariaSelect = {};
    $scope.tipoAvariaSelect.selectedEstadosLista = [];
    $scope.tipoAvariaSelect.selectedEstadosListaWithGroupBy = [];
    $scope.tipoAvariaSelect.selectedEstadosListaSimple = [];

    $scope.produtosLista = [];
    $scope.produtosSelect = {};
    $scope.produtosSelect.selectedEstadosLista = [];
    $scope.produtosSelect.selectedEstadosListaWithGroupBy = [];
    $scope.produtosSelect.selectedEstadosListaSimple = [];

    $scope.dataEntrada = "2015-01-01";
    $scope.DataEntradaOpened = false;


    $scope.DataEntradaOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataEntradaOpened = true;
    };

    $scope.paramsPesquisa = {};
    $scope.paramsPesquisa.clientes = [];
    $scope.paramsPesquisa.departamentos = [];
    $scope.paramsPesquisa.estadoSav = [];
    $scope.paramsPesquisa.tipoAvaria = [];
    $scope.paramsPesquisa.produtos = [];
    $scope.paramsPesquisa.setores = [];


    $scope.paramsPesquisa.dataEntradaDesdeBool = true;
    $scope.paramsPesquisa.dataEntradaDesdeValue = $scope.dataDesde = angular.copy($rootScope._utilizador.dashboardDesde).split("T")[0];
    $scope.paramsPesquisa.dataEntradaAteBool = false;
    $scope.paramsPesquisa.dataEntradaAteValue = "";
    $scope.paramsPesquisa.dataResolvidoDesdeBool = false;
    $scope.paramsPesquisa.dataResolvidoDesdeValue = "";
    $scope.paramsPesquisa.dataResolvidoAteBool = false;
    $scope.paramsPesquisa.dataResolvidoAteValue = "";

    $scope.paramsPesquisa.ordenacao = 0;
    $scope.pesqRes = {};
    $scope.pesqRes.savs = [];

    $scope.seriesLista = [{ "numSerie": "--" }];

    $http.get(serviceBase + "api/Savs/PesquisaSav")
    .success(function (data, status, headers, config) {

        $scope.clientesLista = data.clientes;

        $scope.seriesLista = [{ "numSerie": "--" }].concat(data.series);
        $scope.paramsPesquisa.serie = $scope.seriesLista[0].numSerie;
        $scope.departamentosLista = data.departamentos;
        $scope.departamentosLista.forEach(function (e) {
            if (e.preSeleccionado) {
                $scope.paramsPesquisa.departamentos.push(e);
            }
        });

        $scope.estadoSavLista = data.estadoSav;
        $scope.estadoSavLista.forEach(function (e) {

            if (e.preSeleccionado) {

                $scope.paramsPesquisa.estadoSav.push(e);
            }

        });
        $scope.produtosLista = data.produtoSav;
        $scope.tipoAvariaLista = data.tipoAvaria;
        $scope.setoresLista = data.setores;

        $scope.submit();

    });

    $scope.submit = function () {
        $http.post(serviceBase + "api/Savs/PesquisaSavRes/?pesqParam=", $scope.paramsPesquisa)
        .success(function (data, status, headers, config) {
            var savPad = "0000";
            $scope.pesqRes = data;
            $scope.pesqRes.savs.forEach(function (e) {
                e.nDias = e.numDias; // SavGlobalService.nDias(e);
            });
            toaster.pop("success", "Pesquisa OK.", "");
        });
    }


    $scope.linkPdf = function () {
        //var link = serviceBase + "api/Savs/PesquisaSavRes/&" + $.param($scope.paramsPesquisa);
        $scope.paramsPesquisa.tipoRetorno = "phantom-pdf";
        // console.log($scope.paramsPesquisa);
        var res = "\"" + ((JSON.stringify($scope.paramsPesquisa))).replace(/"/g, "'") + "\"";
        //console.log(res);
        return res;
    }

    $scope.pdfPesquisa = function () {
        $scope.paramsPesquisa.tipoRetorno = "phantom-pdf";

        $http.post(serviceBase + "api/Savs/RelatorioSavsPdf/", $scope.paramsPesquisa)
        .success(function (data, status, headers, config) {
            //.attr("href","/pdf/"+data);
            window.open(serviceBase + "api/Relatorios/Relatorio/" + data.uniqueId);
        });
    }


    $scope.editSav = function (sav)
    {
        var url = $state.href("app.savs.edit", { serie: sav.nomeSerie, num: sav.numDoc });
        window.open(url, '_blank');
        localStorage.removeItem("setToaster");

        //$state.go("app.savs.edit", { serie: sav.nomeSerie, num: sav.numDoc }, { newtab: true });
    }
}]);
