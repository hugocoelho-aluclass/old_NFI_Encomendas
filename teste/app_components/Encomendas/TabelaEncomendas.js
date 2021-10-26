'use strict';

/* Controllers */

app.controller('tabelaEncomendasCtrl', ['$scope', '$rootScope', '$filter', '$http', 'ngTableParams',
function ($scope, $rootScope, $filter, $http, ngTableParams, blockUI) {
    // blockUI.start();
    $scope.dados = [];
    var data = $scope.dados;
    //ngTableParams.count = 0;
    $scope.classeEstado = function (encomenda) {

        if (encomenda.estadoRow == 4 && encomenda.estado == 2 && (!encomenda.fatura || encomenda.fatura == null || encomenda.fatura == "")) {
            return "estado-blue";
        }

        return "estado-e" + encomenda.estadoRow;
    }

    var encomendaPad = "00000";
    $scope.pesq = { dataDesde:  angular.copy($rootScope._utilizador.dashboardDesde).split("T")[0]  };
    $scope.dataDesdeOpen = false;
    
    var atualizaTabela = function () {

        var req = { dataDesde: $scope.pesq.dataDesde.split('T')[0] }
        $http.get(serviceBase + "api/Encomendas/TabelaEncomendas/"+req.dataDesde)
            .success(function (data, status, headers, config) {
                $scope.dados = data;

                data = $scope.dados;
                //   blockUI.stop();

                data.forEach(function (e) {
                    e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc));
                    //console.log(e.serieNumEncomenda, e.numDoc);
                    if (e.estado == 0) {
                        e.estadoDesc = "Pendente";
                    }
                    else if (e.estado == 1) {
                        e.estadoDesc = "Em Produção";
                    }
                    else if (e.estado == 2) {
                        e.estadoDesc = "Entregue";
                    }
                    else if (e.estado == 3) {
                        e.estadoDesc = "Cancelada";
                    }
                    else if (e.estado == 4) {
                        e.estadoDesc = "Pronta";
                    }
                });
                $scope.tableParams = new ngTableParams({
                    page: 1,            // show first page
                    count: 100,         // count per page
                    filter: {
                        //nome: ''       // initial filter
                    },
                    sorting: {
                        serieNumEncomenda: 'desc'     // initial sorting
                    }
                }, {
                    total: data.length, // length of data
                    getData: function ($defer, params) {
                        // use build-in angular filter
                        var filteredData = params.filter() ?
                                $filter('filter')(data, params.filter()) : data;

                        var orderedData = params.sorting() ?
                                $filter('orderBy')(filteredData, params.orderBy()) : data;

                        params.total(orderedData.length);
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    }
                });
            });

    }
    atualizaTabela();
    $scope.atualizaTabela = atualizaTabela;
    $scope.gerarPdf = (function () {
        function newHotness2() {
            var htmlSource = $('#tableEncomendas')[0];
            var pdf = new jsPDF2('l'),
                pdfInternals = pdf.internal,
                pdfPageSize = pdfInternals.pageSize,
                pdfScaleFactor = pdfInternals.scaleFactor,
                pdfPageWidth = pdfPageSize.width,
                pdfPageHeight = pdfPageSize.height,
                pdfPageWidthPx = pdfPageWidth * pdfScaleFactor,
                // pdfPageHeightPx = pdfPageHeight * pdfScaleFactor,
                // pdfPageRatio = pdfPageWidth / pdfPageHeight,
                count = 0;
            var addPage = function (img, height, width) {
                pdf.addImage(img, 0, -(pdfPageHeight * count), width, height);
                count++;
                if ((pdfPageHeight * count) < height) {
                    pdf.addPage();
                    addPage(img, height, width);
                }
            }

            html2canvas(htmlSource, {
                onrendered: function (canvas) {
                    canvas.toBlob(function (blob) {
                        var urlCreator = window.URL || window.webkitURL;
                        var imgUrl = urlCreator.createObjectURL(blob);
                        var img = new Image();
                        img.src = imgUrl;
                        img.onload = function () {
                            var width, height;
                            if (img.width < pdfPageWidthPx) {
                                width = img.width / pdfScaleFactor;
                            } else {
                                width = pdfPageWidth;
                            }
                            height = img.height / (img.width / width);
                            addPage(img, height, width, 0);
                            // pdf.addImage(img, 0, 0, width, height);
                            // pdf.addPage();
                            // pdf.addImage(img, 0, -pdfPageHeight, width, height);
                            pdf.save('new-hotness-two.pdf');
                        };
                    });
                }
            });
        }

        newHotness2();

    });

}]);
//api/AdminOperadores/OperadoresAtivos