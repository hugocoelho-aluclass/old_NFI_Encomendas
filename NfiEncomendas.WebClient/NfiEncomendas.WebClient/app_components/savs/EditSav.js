'use strict';

/* Controllers */

app.controller('SavEditCtrl', ['$scope', '$rootScope' , '$http', '$stateParams', '$state', 'toaster', '$timeout', 'savFileUpload', 'SavGlobalService',
function ($scope, $rootScope, $http, $stateParams, $state, toaster, $timeout, savFileUpload, SavGlobalService) {
    if ($rootScope._utilizador && $rootScope._utilizador.comercial)
    {
        $state.go('app.dashboard-v1');
    }

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
    $scope.sav = {};
    $scope.sav.serie = $stateParams.serie;
    $scope.sav.numSav = $stateParams.num;
    $scope.sav.dataResolvida = null;
    $scope.select = {};
    $scope.select.tiposEncomenda = {};
    $scope.select.tiposEncomendaNum = "";
    $scope.select.estadoRecolha = "";
    
    $scope.select.serie;
    $scope.load = false;


    $scope.upload = function () {
        //console.log("asd");
    };

    //console.log($stateParams.id);
    $scope.DataPedidoOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataPedidoOpened = true;
    };

    $scope.dataSavOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.dataSavOpened = true;
    };
    $scope.DataSaidaOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataSaidaOpened = true;
    };
    $scope.DataEstadoOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataEstadoOpened = true;
    };
    $scope.DataRespostaClienteOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataRespostaClienteOpened = true;
    };
    $scope.DataResolvidaOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataResolvidaOpened = true;
    };
    //DataExpedicaoOpened
    $scope.DataExpedicaoOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.DataExpedicaoOpened = true;
    };
    $scope.DataResolvidaOpened = false;

    $scope.editIsFocus = false;

    $scope.datePickerState = {};

    $scope.getDatePickerState = function (name) {
        if ($scope.datePickerState[name] === undefined) {
            $scope.datePickerState[name] = {
                opened: false
            };
        }
        return $scope.datePickerState[name].opened;
    };

    $scope.setDatePickerStateOpen = function (name, $event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.datePickerState[name] = {
            opened: true
        }
    }
    

    $scope.editFocus = (function (val) {
        $scope.editIsFocus = val;
        if (!$scope.editIsFocus) $scope.atualizaNumDoc();

    });

    $scope.atualizaNumDoc = (function () {

        if ($scope.sav === undefined || ($scope.sav.numDoc == $stateParams.num && $scope.sav.nomeSerie == $stateParams.serie) || $scope.editIsFocus) return;

        $state.go("app.savs.edit", { serie: $scope.sav.nomeSerie, num: $scope.sav.numDoc });

       
    });
    


    $scope.$watch("select.serie", function (v) {
        if ($scope.dados === undefined || v.numSerie == $scope.sav.nomeSerie) return;
        $scope.dados.series.forEach(function (s) {
            if (s == v) {
                $scope.sav.nomeSerie = s.numSerie;
                $scope.sav.numDoc = s.ultDoc + 1;
            }
        });
    });


    $scope.$watch("select.cliente", function (v) {        
        if (v !== undefined && v.id != $scope.sav.numCliente) {
            $scope.sav.numCliente = v.id;
            $scope.select.clienteNum = v.id;
        }
    });


    $scope.$watch("select.clienteNum", function (v) {
        
        if ($scope.dados === undefined || v == undefined) return;
        if (v != $scope.sav.numCliente) {
            $scope.select.cliente = undefined;
            $scope.dados.clientes.forEach(function (c) {
                if (c.id == v) {
                    $scope.select.cliente = c;
                }
            });

            if ($scope.select.cliente === undefined) return;
            $scope.sav.numCliente = $scope.select.cliente.id;
    
            $scope.select.numCliente = v;

        }
    });

    $scope.$watch("select.produto", function (v) {
        if (v !== undefined && v.id != $scope.sav.produtoNum) {
            $scope.sav.produtoNum = v.id;
            $scope.select.produtoNum = v.id;
        }
    });

    $scope.$watch("select.produtoNum", function (v) {
        if ($scope.dados === undefined) return;
        if (v != $scope.sav.produtoNum) {
            $scope.select.produto = undefined;
            $scope.dados.produtoSav.forEach(function (c) {                
                if (c.id == v) {
                    $scope.select.produto = c;
                }
            });

            if ($scope.select.produto === undefined) return;
            $scope.sav.produtoNum = $scope.select.produto.id;              
            $scope.select.produtoNum = v;
        }
    });


    $scope.$watch("select.tipoAvaria", function (v) {
        if (v !== undefined && v.id != $scope.sav.numTipoAvaria) {
            $scope.sav.tipoAvariaNum = v.id;
            $scope.select.tipoAvariaNum = v.id;
        }
    });

    $scope.$watch("select.tipoAvariaNum", function (v) {
        if ($scope.dados === undefined) return;
        if (v != $scope.sav.tipoAvariaNum) {
            $scope.select.tipoAvaria = undefined;
            $scope.dados.tipoAvaria.forEach(function (c) {
                if (c.id == v) {
                    $scope.select.tipoAvaria = c;
                }
            });

            if ($scope.select.tipoAvaria === undefined) return;
            $scope.sav.tipoAvariaNum = $scope.select.tipoAvaria.id;
            $scope.select.tipoAvariaNum = v;
        }
    });

    $scope.$watch("select.departamento", function (v) {
        if (v !== undefined && v.id != $scope.sav.numDepartamento) {
            $scope.sav.departamentoNum = v.id;
            $scope.select.departamentoNum = v.id;
        }
    });

    $scope.$watch("select.departamentoNum", function (v) {
        if ($scope.dados === undefined) return;
        if (v != $scope.sav.departamentoNum) {
            $scope.select.departamento = undefined;
            $scope.dados.departamentos.forEach(function (c) {
                if (c.id == v) {
                    $scope.select.departamento = c;
                }
            });

            if ($scope.select.departamento === undefined) return;
            $scope.sav.departamentoNum = $scope.select.departamento.id;
            $scope.select.departamentoNum = v;
        }
    });

    $scope.$watch("select.estadoSav", function (v) {
        if (v !== undefined && v.id != $scope.sav.numEstadoSav) {
            $scope.sav.estadoSavNum = v.id;
            $scope.select.estadoSavNum = v.id;
        }
    });

    $scope.$watch("select.estadoSavNum", function (v) {
        if ($scope.dados === undefined) return;
        if (v != $scope.sav.estadoSavNum) {
            $scope.select.estadoSav = undefined;
            $scope.dados.estadoSav.forEach(function (c) {
                if (c.id == v) {
                    $scope.select.estadoSav = c;
                }
            });

            if ($scope.select.estadoSav === undefined) return;
            $scope.sav.estadoSavNum = $scope.select.estadoSav.id;
            $scope.select.estadoSavNum = v;
        }
    });

    $scope.$watch("select.tiposEncomenda", function (v) {
        if (v !== undefined && v.numTipoEncomenda != $scope.sav.numTipoEncomenda) {
            $scope.sav.numTipoEncomenda = v.numTipoEncomenda;
            $scope.select.tiposEncomendaNum = v.numTipoEncomenda;
        }
    });

    $scope.$watch("select.setor", function (v) {
        
        if (v !== undefined && (v.id != $scope.sav.setorid )) {
            $scope.sav.setorid = v.id;
            $scope.select.setor = v;
        }
    });

    $scope.$watch("select.estadoRecolha", function (v) {
        // if (v !== undefined && v.id != $scope.sav.estadoRecolha.estadoRecolha) {          
        //     $scope.select.estadoRecolha = v.numTipoEncomenda;
        // }
    });

    $scope.$watch("select.tiposEncomendaNum", function (v) {
        if ($scope.dados === undefined) return;
        if (v != $scope.sav.numTipoEncomenda) {
            $scope.select.tiposEncomenda = undefined;
            $scope.dados.tiposEncomenda.forEach(function (c) {
                if (c.numTipoEncomenda == v) {
                    $scope.select.tiposEncomenda = c;
                }
            });

            if ($scope.select.tiposEncomenda === undefined) return;
            $scope.sav.numTipoEncomenda = $scope.select.tiposEncomenda.numTipoEncomenda;

            $scope.select.tiposEncomendaNum = v;
        }
    });



    $scope.cancel = function () {
        $state.go("app.savs.tabela");
    };

    $scope.atualizaSav = (function () {
        //if ($scope.sav === undefined || $scope.sav.id === 0) return;
        if ($scope.sav === undefined || ($scope.sav.numDoc == $stateParams.num && $scope.sav.nomeSerie == $stateParams.serie) || $scope.editIsFocus) return;
        
        carregarSav($scope.sav.numDoc, $scope.sav.numDoc);
    });

    $scope.anular = (function () {
        if ($rootScope._utilizador && $rootScope._utilizador.comercial)
        {
            $state.go('app.dashboard-v1');
        }
        $scope.sav.anulada = !$scope.sav.anulada;
        $scope.submit();
    });
    $scope.dateDifFc = function (d1, d2) {

        //var t2 = (new Date(d2)).getTime();
        //var t1 = (new Date(d1)).getTime();
        //return parseInt((t2 - t1) / (24 * 3600 * 1000)) + 1;
    };

    $scope.dateDiffDays = 0;
    $scope.$watchCollection("[sav.dataSav,sav.dataEstado]", function (v) {
        $scope.dateDiffDays = $scope.dateDifFc($scope.sav.dataSav, $scope.sav.dataEstado);
    });

    $scope.log = function (l) {
        console.log(l);
    }

    function carregarSav(serie, num) {
        $scope.load = false;

        $http.get(serviceBase + "api/Savs/EditSav/?serie=" + serie + "&numDoc=" + num)
            .success(function (data, status, headers, config) {
                $scope.dados = data;
                $scope.sav = data.sav;
                
                if ($scope.sav.numCliente != "0") {
                    $scope.dados.clientes.forEach(function (s) {
                        if (s.id == $scope.sav.numCliente) {
                            $scope.select.cliente = s;
                            $scope.select.clienteNum = s.id;
                            return false;
                        }
                    });
                }
                $scope.dados.departamentos.forEach(function (s) {
                    if (s.id == $scope.sav.departamentoNum) {
                        $scope.select.departamento = s;
                        $scope.select.departamentoNum = s.id;
                        return false;
                        
                    }
                });
                $scope.dados.estadoSav.forEach(function (s) {
                    if (s.id == $scope.sav.estadoSavNum) {
                        $scope.select.estadoSav = s;
                        $scope.select.estadoSavNum = s.id;
                        return false;
                    }
                });
                $scope.dados.produtoSav.forEach(function (s) {
                    if (s.id == $scope.sav.produtoNum) {
                        $scope.select.produto = s;
                        $scope.select.produtoNum = s.id;
                        return false;
                    }
                });
                $scope.dados.tipoAvaria.forEach(function (s) {
                    if (s.id == $scope.sav.tipoAvariaNum) {
                        $scope.select.tipoAvaria = s;
                        $scope.select.tipoAvariaNum = s.id;
                        return false;
                    }
                });

                $scope.dados.series.forEach(function (s) {
                    if (s.numSerie == $scope.sav.nomeSerie) {
                        $scope.select.serie = s;
                        return false;
                    }
                });

                $scope.select.setor = $scope.dados.setores.find(x => x.id == $scope.sav.setorId);



                if ($scope.sav.recolha != undefined) {
                    var defaultDate = "2010-01-01T00:00:00";
                    var dateKeys = ["dataRecolha", "dataChegadaPrevista", "dataPedidoRecolha"]
                    for (var i = 0; i < dateKeys.length; i++) {
                        if ($scope.sav.recolha[dateKeys[i]] == defaultDate) {
                            $scope.sav.recolha[dateKeys[i]] ="";
                        }
                    }
                    
                    if ($scope.sav.recolha.estadoRecolha)
                    {
                        // console.log(  $scope.sav.recolha.estadoRecolha,  $scope.dados.estadosRecolha);
                        $scope.sav.recolha.estadoRecolha = $scope.dados.estadosRecolha.find(x => x.id == $scope.sav.recolha.estadoRecolha.id);

                        // console.log(  $scope.dados.estadosRecolha, $scope.sav.recolha.estadoRecolha);
                    }

                }
                $scope.sav.nDias = SavGlobalService.nDias2($scope.sav);

                $timeout(function () {
                    $scope.load = true;
                }, 200);
            });
    }

    //carregarSav($stateParams.id);
    carregarSav($stateParams.serie, $stateParams.num);
    
    $scope.submit = function () {
        if ($rootScope._utilizador && $rootScope._utilizador.comercial)
        {
            $state.go('app.dashboard-v1');
        }
        $http.post(serviceBase + "api/Savs/AtualizaSav/", $scope.sav)
        .success(function (data, status, headers, config) {

            //toaster.pop("success", "Sav Atualizada", "");
            localStorage.setItem("setToaster", "success|Sav Atualizada");


            //$state.go("app.savs.tabela");
            //window.close();
            if (window.close() === undefined) {
                window.history.go(-1);
            }

        });
    }

    $scope.encerrarSAV = function () {
        
        $http.post(serviceBase + "api/Savs/MarcarEncerrada/", $scope.sav)
        .success(function (data, status, headers, config) {

            toaster.pop("success", "Sav Atualizada", "");
            $state.go("app.savs.tabela");
            
        });
    }

    $scope.uploadFile = function (e) {
        var file = $scope.myFile;
        
        //console.dir(file);
        var uploadUrl = serviceBase + "api/Anexos/InserirAnexo/";
        savFileUpload.successEv = function (data) {
            if ($scope.sav.anexos == undefined) $scope.sav.anexos = [];
            $scope.sav.anexos.push(data);
            
        }
        savFileUpload.uploadFileToUrl(file, uploadUrl);

        //this.preventDefault();
    };

    $scope.getAnexo = function (anexo) {

        //$scope.paramsPesquisa.tipoRetorno = "phantom-pdf";
        //var importantStuff = window.open('', '_blank');
        //importantStuff.document.write($("#imgLoader").html());

        $http.get(serviceBase + "api/Anexos/GetAnexo/" +anexo.id, { responseType: 'arraybuffer' })
         .success(function (data) {
             var myBlob = new Blob(["example"], { type: 'text/html' })
             var blobURL = (window.URL || window.webkitURL).createObjectURL(new Blob([data], { type: 'application/octet-binary' }));
             var anchor = document.createElement("a");
             anchor.download = anexo.nome;
             anchor.href = blobURL;
             anchor.click();

             //importantStuff.cont
             //importantStuff.location.href = URL.createObjectURL(new Blob([data], { type: 'application/octet-binary' }));

         });
    }

    
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };
}]);

app.service('savFileUpload', ['$http', function ($http) {
    this.successEv = {};
    var savFileUpload = this;
    this.uploadFileToUrl = function (file, uploadUrl) {
        var fd = new FormData();
        fd.append('file', file);
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
       .success(function (data) {
           savFileUpload.successEv(data);
       })
       .error(function () {
       });
    }

}]);
