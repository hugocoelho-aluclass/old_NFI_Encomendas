'use strict';

/* Controllers */

app.controller('ModalConfirmacao', ['$scope', '$modalInstance', '$http', 'problema', 'gravar', 'toaster', function ($scope, $modalInstance, $http, problema, gravar, toaster) {
    $scope.ModalProblema = problema;

    $scope.eficacia = ['', 'Cancelado' , 'Não eficaz', 'Parcialmente eficaz', 'Eficaz'];

    $scope.submit = function () {
        gravar($scope.ModalProblema);
        $modalInstance.close('submit');
    };

   

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}]);




app.controller('EditProblemasCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster', '$modal',
    function ($scope, $http, $stateParams, $state, ngTableParams, toaster, $modal) {


        $scope.select = {};
        $scope.problema = {};
        $scope.problema.idProblema = $stateParams.id;
        $scope.statparams = $stateParams;
        $scope.problema.eficacia = "";
        $scope.requireAvaliacao = false;
        /*let d = new Date();
        $scope.problema.dataCriacao = d.toLocaleDateString();
        console.log($scope.problema.dataCriacao); //moment(d).format('DD-MM-YYYY');*/

        $scope.$watch("select.departamento", function (v) {
            if (v !== undefined && v.idDepartamento != $scope.problema.idDepartamento) {
                $scope.problema.idDepartamento = v.idDepartamento;
            }
        });




        function carregarProblema(i) {
            $http.get(serviceBase + "api/Problemas/EditProblema/?id=" + i)
                .success(function (data, status, header, config) {
                    console.log("carregar");
                    $scope.problema = data.problema;
                    $scope.departamentos = data.departamentos;
                    $scope.idAnt = data.idAnt;
                    $scope.idProx = data.idProx;
                    $scope.departamentos.forEach(function (d) {
                        if (d.idDepartamento == $scope.problema.idDepartamento) {
                            $scope.select.departamento = d;
                            
                        }
                    });

                    if ($scope.problema.eficacia == null) {
                        $scope.problema.eficacia = 0;
                    }

                    let tempArr = $scope.problema.dataCriacao.split(/-|T/, 3);
                  
                    $scope.dataCriacao = tempArr[0] + '-' + tempArr[1] + '-' + tempArr[2];

                });
        }


        $scope.editFocus = (function (val) {
            $scope.editIsFocus = val;

            if (!$scope.editIsFocus) $scope.atualizaIdProblema();

        });

        carregarProblema($stateParams.id);




        $scope.atualizaIdProblema = (function () {

            if ($scope.problema === undefined || $scope.problema.idProblema == $stateParams.id || $scope.editIsFocus) return;
            $state.go("app.problemas.edit", { id: $scope.problema.idProblema });

        });

        /*$scope.$watch('problema.idProblema', function (newValue, oldValue) {
            if (newValue != oldValue) {
                console.log("watch");
                carregarProblema(newValue);
            }
        });*/

        $scope.$watch('problema.eficacia', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (newValue == 0 || newValue == null) {
                    $scope.requireAvaliacao = false;
                } else {
                    $scope.requireAvaliacao = true;
                }
            }
        });


        $scope.confirmarProb = function (prob) {
            if ($scope.problema.eficacia == 0 || $scope.problema.eficacia == null) {

                $scope.gravar(prob);

            } else {
                var modalInstance = $modal.open({
                    templateUrl: 'app_components/Problemas/ModalConfirmacao.html',
                    controller: 'ModalConfirmacao',
                    resolve: {
                        problema: function () {
                            return prob;
                        },
                        gravar: function () {
                            return $scope.gravar;
                        }
                        
                    }
                });

            }

        }


        $scope.gravar = function (prob) {
            $http.post(serviceBase + "api/Problemas/AtualizaProblemas/", prob)
                .success(function (data, status, headers, config) {
                    if (prob.eficacia == 2) {
                        console.log("nao eficaz");
                        prob.idAnterior = prob.idProblema;
                        prob.idProblema = 0;
                        prob.acaoImplementar = null;
                        prob.acompanhamento = null;
                        prob.acompanhamento = null;
                        prob.avaliacaoEficacia = null;
                        prob.eficacia = 0;
                        toaster.pop("success", "Gerado novo registo", "");
                        $scope.gravar(prob);
                    } else {
                        console.log("foi atualizado");
                        toaster.pop("success", "Problema Atualizado", "");
                        carregarProblema(data);
                    }
                    

                });
        }


        /*$scope.gravar = function () {
            $http.post(serviceBase + "api/Problemas/AtualizaProblemas/", $scope.problema)
                .success(function (data, status, headers, config) {
                    toaster.pop("success", "Problema Atualizado", "");
                    if ($scope.problema.eficacia == 2) {
                        $state.go("app.problemas.edit2", { "id": 0, "desc": $scope.problema.descricao, "causa": $scope.problema.descricaoCausa });
                    } else {
                        carregarProblema($scope.problema.idProblema);
                    }
                    //$state.go("app.problemas.edit", { "id": $scope.problema.idProblema });
                });
        }*/

        /*$scope.submit = function () {
            $http.post(serviceBase + "api/Problemas/AtualizaProblemas/", $scope.problema)
                .success(function (data, status, headers, config) {
                    console.log("entrou: atualizar");
                    toaster.pop("success", "Problema Atualizado", "");
                    $state.go("app.problemas.tabela");
                    console.log(data);
                });
        }*/

        $scope.cancel = function () {
            $state.go("app.problemas.tabela");
        };

        //console.log($scope);
    }]);
