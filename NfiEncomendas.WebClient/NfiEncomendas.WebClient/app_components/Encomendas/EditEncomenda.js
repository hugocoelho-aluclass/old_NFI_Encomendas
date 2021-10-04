'use strict';

/* Controllers */

app.controller('EncomendaEditCtrl', ['$scope', '$rootScope', '$http', '$stateParams', '$state', 'toaster', '$timeout',
    function ($scope,$rootScope, $http, $stateParams, $state, toaster, $timeout) {

        $scope.preventSubmit = function () {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode == 13) {
                if (!e) var e = window.event;

                e.cancelBubble = true;
                e.returnValue = false;

                if (e.stopPropagation) {
                    e.stopPropagation();
                    e.preventDefault();
                }
                event.srcElement.blur();
            }
        }

        $scope.encomenda = {};
        $scope.encomenda.serie = $stateParams.serie;
        $scope.encomenda.numEncomenda = $stateParams.num;
        $scope.select = {};
        $scope.select.tipoEncomenda = {};
        $scope.select.tipoEncomendaNum = "";
        $scope.select.cliente = {};
        $scope.select.clienteNum = "";
        $scope.select.serie;
        $scope.load = false;

        //console.log($stateParams.id);
        $scope.DataPedidoOpen = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.DataPedidoOpened = true;
        };
        $scope.DataAprovacaoOpen = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.DataAprovacaoOpened = true;
        };
        $scope.editIsFocus = false;

        $scope.editFocus = (function (val) {
            $scope.editIsFocus = val;

            if (!$scope.editIsFocus) $scope.atualizaNumDoc();

        });

        $scope.atualizaNumDoc = (function () {

            if ($scope.encomenda === undefined || ($scope.encomenda.numDoc == $stateParams.num && $scope.encomenda.nomeSerie == $stateParams.serie) || $scope.editIsFocus) return;

            $state.go("app.encomendas.edit", { serie: $scope.encomenda.nomeSerie, num: $scope.encomenda.numDoc });
            
        });


        $scope.$watch("select.cliente", function (v) {
            if (v !== undefined && v.numCliente != $scope.encomenda.numCliente) {
                $scope.encomenda.numCliente = v.numCliente;
                $scope.select.clienteNum = v.numCliente;
            }
        });

        $scope.$watch("select.clienteNum", function (v) {
            if ($scope.dados === undefined) return;
            if (v != $scope.encomenda.numCliente) {
                $scope.select.cliente = undefined;
                $scope.dados.clientes.forEach(function (c) {

                    if (c.numCliente == v) {
                        $scope.select.cliente = c;
                    }
                });

                if ($scope.select.cliente === undefined) return;
                $scope.encomenda.numCliente = $scope.select.cliente.numCliente;
                //if ($scope.select.cliente === undefined)
                //{
                //    alert("Cliente não encontrado");
                //}           
                $scope.select.clienteNum = v;
            }
        });

        $scope.$watch("select.tipoEncomenda", function (v) {
            if (v !== undefined && v.numTipoEncomenda != $scope.encomenda.numTipoEncomenda) {
                $scope.encomenda.numTipoEncomenda = v.numTipoEncomenda;
                $scope.select.tipoEncomendaNum = v.numTipoEncomenda;
            }
        });

        $scope.$watch("select.tipoEncomendaNum", function (v) {
            if ($scope.dados === undefined) return;
            if (v != $scope.encomenda.numTipoEncomenda) {
                $scope.select.tipoEncomenda = undefined;
                $scope.dados.tipoEncomenda.forEach(function (c) {

                    if (c.numTipoEncomenda == v) {
                        $scope.select.tipoEncomenda = c;
                    }
                });

                if ($scope.select.tipoEncomenda === undefined) return;
                $scope.encomenda.numTipoEncomenda = $scope.select.tipoEncomenda.numTipoEncomenda;
                //if ($scope.select.tipoEncomenda === undefined)
                //{
                //    alert("TipoEncomenda não encontrado");
                //}           
                $scope.select.tipoEncomendaNum = v;
            }
        });

        $scope.cancel = function () {
            $state.go("app.encomendas.tabela");
        };

        $scope.atualizaEncomenda = (function () {
            if ($scope.encomenda === undefined || $scope.encomenda.nomeSerie === undefined || $scope.encomenda.numDoc === undefined) return;
            carregarEncomenda($scope.encomenda.nomeSerie, $scope.encomenda.numDoc);
        });

        $scope.anular = (function () {
            $scope.encomenda.anulada = !$scope.encomenda.anulada;
            $scope.submit();
        });

        function carregarEncomenda(serie, num) {
            $scope.load = false;

            $http.get(serviceBase + "api/Encomendas/EditEncomenda/?serie=" + serie + "&numDoc=" + num)
                .success(function (data, status, headers, config) {
                    if (data.encomenda.numEncomenda != $stateParams.id) {
                        //$state.go("app.Encomendas.edit", { id: data.Encomenda.numEncomenda });
                    }
                    $scope.dados = data;
                    $scope.encomenda = data.encomenda;
                    $scope.dados.clientes.forEach(function (s) {
                        if (s.numCliente == $scope.encomenda.numCliente) {
                            $scope.select.cliente = s;
                            $scope.select.clienteNum = s.numCliente;
                        }
                    });

                    $scope.dados.tipoEncomenda.forEach(function (s) {
                        if (s.numTipoEncomenda == $scope.encomenda.numTipoEncomenda) {
                            $scope.select.tipoEncomenda = s;
                            $scope.select.tipoEncomendaNum = s.numTipoEncomenda;
                        }
                    });

                    $scope.dados.series.forEach(function (s) {
                        if (s.numSerie == $scope.encomenda.nomeSerie) {
                            $scope.select.serie = s;
                        }
                    });

                    $scope.$watch("select.serie", function (v) {
                        if ($scope.dados === undefined || v.numSerie == $scope.encomenda.nomeSerie) return;
                        $scope.dados.series.forEach(function (s) {
                            if (s == v) {
                                $scope.encomenda.nomeSerie = s.numSerie;
                                $scope.encomenda.numDoc = s.ultDoc + 1;
                            }
                        });
                    });

                    $timeout(function () {
                        $scope.load = true;
                    }, 200);

                    var comprasCounter = 0; 
                    var comprasMax = $scope.encomenda.compras.length;

                    var comprasMostra = function () {
                        $scope.encomenda.compras[comprasCounter].mostra = true;
                        comprasCounter++;
                        if (comprasCounter < comprasMax) {
                            $timeout(
                                comprasMostra
                                , 50);
                        }
                    };
                    $timeout(comprasMostra, 300);
                });
        }

        carregarEncomenda($stateParams.serie, $stateParams.num);

        $scope.submit = function () {
            if ($rootScope._utilizador && $rootScope._utilizador.comercial)
            {
                alert("utilizador comercial não pode editar encomendas");
            }
            else
            {
                $http.post(serviceBase + "api/Encomendas/AtualizaEncomenda/", $scope.encomenda)
                    .success(function (data, status, headers, config) 
                    {
                        toaster.pop("success", "Encomenda Atualizada", "");
                        $state.go("app.encomendas.edit", { serie: 0, num: 0 });
                        $scope.atualizaNumDoc();
                    }
                );
            }
        };
    }
]);


