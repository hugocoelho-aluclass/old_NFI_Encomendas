'use strict';

/* Controllers */

app.controller('TipoEncomendaEditCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
    function ($scope, $http, $stateParams, $state, ngTableParams, toaster) {

        $scope.tipoEncomenda = {};
        $scope.tipoEncomenda.numTipoEncomenda = $stateParams.id;
        $scope.dadosExtra = { setoresEncomenda: [] };
     
        $scope.select = {};
        $http.get(serviceBase + "api/setoresEncomenda/TabelaSetores")
            .success(function (data, status, headers, config) {
                $scope.dadosExtra.setoresEncomenda = data;
                setSetorEncomendaId();
            });


        $scope.$watch('tipoEncomenda.numTipoEncomenda', function (newValue, oldValue) {
            if (newValue != oldValue) {
                carregartipoEncomenda(newValue);
            }
        });

        //$scope.refreshAddresses = function (address) { };

        $scope.$watch("select.setorId", function (v) {
            if (v === undefined) return;
            $scope.tipoEncomenda.setorId = v.id;
        });


        function carregartipoEncomenda(id) {
            $http.get(serviceBase + "api/TipoEncomendas/EditTipoEncomenda/" + id)
                .success(function (data, status, headers, config) {
                    $scope.tipoEncomenda = data.tipoEncomenda;
                    //$scope.select.setorId = $scope.tipoEncomenda.setorId;
                    if (data.tipoEncomenda.numTipoEncomenda != $stateParams.id) {
                        $scope.tipoEncomenda.capacidadePrix = $scope.tipoEncomenda.capacidadeWis = $scope.tipoEncomenda.capacidadeResto = 0;
                    }
                    setSetorEncomendaId();
                });

        }
        carregartipoEncomenda($stateParams.id);

        $scope.submit = function () {
            $http.post(serviceBase + "api/TipoEncomendas/AtualizaTipoEncomenda/", $scope.tipoEncomenda)
                .success(function (data, status, headers, config) {

                    toaster.pop("success", "Tipo de Encomenda Atualizado", "");
                    $state.go("app.tipoEncomendas.tabela");
                });
        }
        function setSetorEncomendaId() {
            if (!$scope.tipoEncomenda || $scope.dadosExtra.setoresEncomenda === [])
                return;

            const findSetorEncomenda = $scope.dadosExtra.setoresEncomenda.find(x => x.id === $scope.tipoEncomenda.setorId);
            if (findSetorEncomenda) $scope.select.setorId = findSetorEncomenda;
        }


    }]);
