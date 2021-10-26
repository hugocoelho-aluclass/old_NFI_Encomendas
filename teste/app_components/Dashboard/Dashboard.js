'use strict';
app.controller('ModalFechaEncomendaCtrl', ['$scope', '$modalInstance', '$http', 'encomenda', 'toaster', function ($scope, $modalInstance, $http, encomenda, toaster) {
    $scope.ModalEncomenda = encomenda;

    $scope.ok = function () {
        $http.post(serviceBase + "api/Dashboard/MarcarExpedida/?encId=" + $scope.ModalEncomenda.idEncomenda + "&dataExp=" + $scope.ModalEncomenda.dataExpedidoString + "&fatura=" + $scope.ModalEncomenda.fatura)

            .success(function (data, status, headers, config) {
                toaster.pop("success", "Encomenda Atualizada", "");
                $modalInstance.close('sucesso');
            });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}])
    ;

app.controller('DashboardCtrl', ['$scope', '$rootScope', '$filter', '$http', '$modal', 'toaster',
    function ($scope, $rootScope, $filter, $http, $modal, toaster) {
        //$scope.dataDesde = "n";
        $scope.dataDesde = angular.copy($rootScope._utilizador.dashboardDesde).split("T")[0];
        //console.log(JSON.stringify(  $rootScope._utilizador));
        $scope.dataDesdeOpen = false;
        $scope.dados = {};
        $scope.dados.totalEncomendas = "--";
        $scope.dados.totalPendentes = "--";
        $scope.dados.totalProducao = "--";
        $scope.dados.totalExpedidas = "--";

        $scope.dados.totalCaixilhos = "--";
        $scope.dados.totalPortoesAp = "--";
        $scope.dados.totalPortoesSold = "--";
        $scope.dados.totalEstores = "--";

        $scope.dados.totalEncSemSemana = "--";
        $scope.dados.totalEncAtrasada = "--";
        $scope.dados.totalEncEstaSemana = "--";
        $scope.dados.totalEncProxSemana1 = "--";
        $scope.dados.totalEncProxSemana2 = "--";
        $scope.dados.totalEncProxSemana3 = "--";
        $scope.dados.totalEncProxSemana4mais = "--";
        $scope.dataDesdeOpen = false;

        $scope.marcarComoEntregue = function (enc) {
            //enc.marcarEntregue = true;

            var modalInstance = $modal.open({
                templateUrl: 'app_components/Dashboard/FechaEncomenda.html',
                controller: 'ModalFechaEncomendaCtrl',
                resolve: {
                    encomenda: function () {
                        return enc;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                location.reload(true);
                $scope.atualizaDashboard();
                // $scope.selected = selectedItem;
            }, function () {
                // console.log('Modal dismissed at: ' + new Date());
            });
        }


        $scope.marcarProducao = function (enc) {
            //console.log(enc);
            // console.log(enc.dataExpedidoString.toString());
            $http.post(serviceBase + "api/Dashboard/AtualizaEstado/?encId=" + enc.idEncomenda + "&estado=1")
                .success(function (data, status, headers, config) {

                    toaster.pop("success", "Encomenda Atualizada", "");
                    location.reload(true);
                });
        }

        $scope.marcarCancelada = function (enc) {
            //console.log(enc);

            $http.post(serviceBase + "api/Dashboard/AtualizaEstado/?encId=" + enc.idEncomenda + "&estado=3")

                .success(function (data, status, headers, config) {

                    toaster.pop("success", "Encomenda Atualizada", "");
                    location.reload(true);

                });
        }
        $scope.status = {};
        $scope.status.semSemanaEntrega = false;
        $scope.status.semSemanaEntregaLoading = false;
        $scope.status.encAtrasadaLoading = false;
        $scope.status.encProducaoLoading = false;
        $scope.status.encEstaSemanaLoading = false;
        $scope.status.enc1SemanaLoading = false;
        $scope.status.enc2SemanaLoading = false;
        $scope.status.enc3SemanaLoading = false;
        $scope.status.enc4SemanaLoading = false;

        $scope.status.encCompraPendentesLoading = false;

        $scope.encomendasSemSemana;
        var encomendaPad = "0000";
        $scope.semSemanaEntregaOpen = function () {
            //console.log($scope.status.semSemanaEntregaLoading);
            if ($scope.status.semSemanaEntregaOpened && $scope.encomendasSemSemana == null && $scope.status.semSemanaEntregaLoading == false) {
                $scope.status.semSemanaEntregaLoading = true;
                // console.log("carrega daddos");
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: -1 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendasSemSemana = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }

        $scope.encAtrasadaOpen = function () {
            //console.log($scope.status.semSemanaEntregaLoading);
            if ($scope.status.encAtrasadaOpened && $scope.encomendasAtrasadas == null && $scope.status.encAtrasadaLoading == false) {
                $scope.status.encAtrasadaLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: -2 }
                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendasAtrasadas = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });

                    });

            }
        }

        $scope.encProducaoOpen = function () {
            //console.log($scope.status.semSemanaEntregaLoading);
            if ($scope.status.producaoOpened && $scope.encomendasProducao == null && $scope.status.encProducaoLoading == false) {
                $scope.status.encProducaoLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: -3 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendasProducao = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }

        $scope.encCompraPendentesOpen = function () {
            //  console.log($scope.status.semCompraOpened + "_" + $scope.encomendassemCompra + "_" + $scope.status.encCompraPendentesLoading);
            if ($scope.status.semCompraOpened && $scope.encomendassemCompra == null && $scope.status.encCompraPendentesLoading == false) {
                $scope.status.encCompraPendentesLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: -5 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendasComprasPendentes = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }
        $scope.encEstaSemanaOpen = function () {
            if ($scope.status.encEstaSemanaOpened && $scope.encomendasEstaSemana == null && $scope.status.encEstaSemanaLoading == false) {
                $scope.status.encEstaSemanaLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: 0 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendasEstaSemana = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }

        $scope.enc1SemanaOpen = function () {
            if ($scope.status.enc1SemanaOpened && $scope.encomendas1Semana == null && $scope.status.enc1SemanaLoading == false) {
                $scope.status.enc1SemanaLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: 1 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendas1Semana = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }

        $scope.enc2SemanaOpen = function () {
            if ($scope.status.enc2SemanaOpened && $scope.encomendas2Semana == null && $scope.status.enc2SemanaLoading == false) {
                $scope.status.enc2SemanaLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: 2 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/2/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendas2Semana = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }

        $scope.enc3SemanaOpen = function () {
            if ($scope.status.enc3SemanaOpened && $scope.encomendas3Semana == null && $scope.status.enc3SemanaLoading == false) {
                $scope.status.enc3SemanaLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: 3 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/3/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendas3Semana = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }

        $scope.enc4SemanaOpen = function () {
            if ($scope.status.enc4SemanaOpened && $scope.encomendas4Semana == null && $scope.status.enc4SemanaLoading == false) {
                $scope.status.enc4SemanaLoading = true;
                var req = { dataDesde: $scope.dataDesde.split('T')[0], tipo: -4 }

                $http.post(serviceBase + "api/Dashboard/DashboardEncomendas/", req)
                    .success(function (data, status, headers, config) {
                        $scope.encomendas4Semana = data;
                        data.encomendas.forEach(function (e) {
                            e.serieNumEncomenda = e.nomeSerie + "/" + (encomendaPad.substring(0, encomendaPad.length - ("" + e.numDoc).length) + ("" + e.numDoc))
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
                            if (e.dataExpedidoString === undefined) e.dataExpedidoString = "";
                        });
                    });
            }
        }




        $scope.atualizaDashboard = function () {
            //$scope.dataDesde = $rootScope._utilizador.dashboardDesde;



            $http.get(serviceBase + "api/Dashboard/DashboardHomeDate/" + $scope.dataDesde.split("T")[0])
                .success(function (data, status, headers, config) {
                    $scope.dados = data;



                    $scope.encomendasSemSemana = undefined;
                    $scope.status.semSemanaEntregaLoading = false;
                    $scope.encomendasAtrasadas = undefined;
                    $scope.status.encAtrasadaLoading = false;
                    $scope.encomendasProducao = undefined;
                    $scope.status.encProducaoLoading = false;
                    $scope.encomendasComprasPendentes = undefined;
                    $scope.status.encCompraPendentesLoading = false;
                    $scope.encomendasEstaSemana = undefined;
                    $scope.status.encEstaSemanaLoading = false;
                    $scope.encomendas1Semana = undefined;
                    $scope.status.enc1SemanaLoading = false;
                    $scope.encomendas2Semana = undefined;
                    $scope.status.enc2SemanaLoading = false;
                    $scope.encomendas3Semana = undefined;
                    $scope.status.enc3SemanaLoading = false;
                    $scope.encomendas4Semana = undefined;
                    $scope.status.enc4SemanaLoading = false;


                });
        }

        $scope.atualizaDashboard();

        $scope.log = function (v) {
            console.log(v);
        }

    }])