'use strict';

/**
 * Config for the router
 */
angular.module('app')
    .run(
        ['$rootScope', '$state', '$stateParams',
            function ($rootScope, $state, $stateParams) {
                $rootScope.$state = $state;
                $rootScope.$stateParams = $stateParams;
            }
        ]
    )
    .config(
        ['$stateProvider', '$urlRouterProvider',
            function ($stateProvider, $urlRouterProvider) {

                $urlRouterProvider
                    .otherwise('/app/dashboard-v1');
                $stateProvider
                    .state('app', {
                        abstract: true,
                        url: '/app',

                        templateUrl: 'tpl/app.html',
                        resolve: {
                        }
                    })
                    //ADMIN
                    .state('app.admin', {
                        url: '/admin',
                        template: '<div ui-view class=""></div>',
                        deps: ['$ocLazyLoad',
                            function ($ocLazyLoad) {
                                return $ocLazyLoad.load(['app_components/admin/admin.js']);
                            }]
                    })
                    .state('app.admin.gestaoOperadores', {
                        url: '/gestaoOperadores',
                        templateUrl: 'app_components/Admin/GestaoOperadores/GestaoOperadores.tpl.html',
                        controller: 'adminGestaoOperadores',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/admin/GestaoOperadores/GestaoOperadores.js']);
                                }]
                        }
                    })
                    .state('app.admin.gestaoOperadores_edit', {
                        url: '/Edit/:id',
                        templateUrl: 'app_components/Admin/GestaoOperadores/EditOperador.tpl.html',
                        controller: 'adminGestaoOperadoresEdit',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/admin/GestaoOperadores/EditOperador.js']);
                                }]
                        }
                    })
                    .state('app.clientes', {
                        url: '/clientes',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.clientes.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/Clientes/TabelaClientes.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Clientes/TabelaClientes.js']);
                                }]
                        }
                    })
                    .state('app.clientes.edit', {
                        url: '/edit/:id/:desc',
                        templateUrl: 'app_components/Clientes/EditCliente.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Clientes/EditCliente.js']);
                                }]
                        }
                    })
                    .state('app.tipoEncomendas', {
                        url: '/tipoEncomendas',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.tipoEncomendas.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/TipoEncomendas/TabelaTipoEncomenda.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/TipoEncomendas/TabelaTipoEncomenda.js']);
                                }]
                        }
                    })
                    .state('app.tipoEncomendas.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/TipoEncomendas/EditTipoEncomenda.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/TipoEncomendas/EditTipoEncomenda.js']);
                                }]
                        }
                    })
                    .state('app.tipoAvarias', {
                        url: '/tipoAvarias',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.tipoAvarias.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/TipoAvarias/TabelaTipoAvaria.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/TipoAvarias/TabelaTipoAvaria.js']);
                                }]
                        }
                    })
                    .state('app.tipoAvarias.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/TipoAvarias/EditTipoAvaria.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/TipoAvarias/EditTipoAvaria.js']);
                                }]
                        }
                    })
                    .state('app.setores', {
                        url: '/setores',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.setores.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/Setores/TabelaSetor.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Setores/TabelaSetor.js']);
                                }]
                        }
                    })
                    .state('app.setores.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/Setores/EditSetor.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Setores/EditSetor.js']);
                                }]
                        }
                    })
                    .state('app.setoresEncomenda', {
                        url: '/setoresEncomenda',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.setoresEncomenda.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/SetoresEncomenda/TabelaSetor.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/SetoresEncomenda/TabelaSetor.js']);
                                }]
                        }
                    })
                    .state('app.setoresEncomenda.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/SetoresEncomenda/EditSetor.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/SetoresEncomenda/EditSetor.js']);
                                }]
                        }
                    })
                    .state('app.produtoSavs', {
                        url: '/produtoSavs',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.produtoSavs.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/ProdutoSav/TabelaProdutoSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/ProdutoSav/TabelaProdutoSav.js']);
                                }]
                        }
                    })
                    .state('app.produtoSavs.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/ProdutoSav/EditProdutoSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/ProdutoSav/EditProdutoSav.js']);
                                }]
                        }
                    })
                    .state('app.departamentoSavs', {
                        url: '/DepartamentoSavs',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.departamentoSavs.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/DepartamentoSav/TabelaDepartamentoSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/DepartamentoSav/TabelaDepartamentoSav.js']);
                                }]
                        }
                    })
                    .state('app.departamentoSavs.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/DepartamentoSav/EditDepartamentoSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/DepartamentoSav/EditDepartamentoSav.js']);
                                }]
                        }
                    })

                    .state('app.series', {
                        url: '/series',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.series.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/series/TabelaSeries.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/series/TabelaSeries.js']);
                                }]
                        }
                    })
                    .state('app.series.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/series/EditSerie.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/series/EditSerie.js']);
                                }]
                        }
                    })
                    .state('app.estadoSav', {
                        url: '/estadoSav',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.estadoSav.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/estadoSav/TabelaestadoSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/estadoSav/TabelaestadoSav.js']);
                                }]
                        }
                    })
                    .state('app.estadoSav.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/estadoSav/EditestadoSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/estadoSav/EditestadoSav.js']);
                                }]
                        }
                    })
                    .state('app.encomendas', {
                        url: '/encomendas',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.encomendas.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/encomendas/TabelaEncomendas.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/encomendas/TabelaEncomendas.js']);
                                }]
                        }
                    })
                    .state('app.encomendas.tabela2', {
                        url: '/tabela2',
                        templateUrl: 'app_components/encomendas/TabelaEncomendas2.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/encomendas/TabelaEncomendas2.js']);
                                }]
                        }
                    })
                    .state('app.encomendas.tabelaProducao', {
                        url: '/tabelaProd',
                        templateUrl: 'app_components/encomendas/TabelaEncomendasProd.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/encomendas/TabelaEncomendasProd.js']);
                                }]
                        }
                    })
                    .state('app.encomendas.tabelaAdmin', {
                        url: '/tabelaAdmin',
                        templateUrl: 'app_components/encomendas/TabelaEncomendasAdmin.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/encomendas/TabelaEncomendasAdmin.js']);
                                }]
                        }
                    })
                    .state('app.encomendas.edit', {
                        url: '/edit/:serie/:num',
                        templateUrl: 'app_components/encomendas/EditEncomenda.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/encomendas/EditEncomenda.js',]);
                                }]
                        }
                    })
                    .state('app.savs', {
                        url: '/savs',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.savs.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/savs/Tabelasavs.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/savs/Tabelasavs.js']);
                                }]
                        }
                    })
                    .state('app.savs.tabela2', {
                        url: '/tabela2',
                        templateUrl: 'app_components/savs/Tabelasavs2.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/savs/Tabelasavs2.js']);
                                }]
                        }
                    })
                    .state('app.savs.edit', {
                        url: '/edit/:serie/:num',
                        templateUrl: 'app_components/savs/EditSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/savs/EditSav.js']);
                                }]
                        }
                    })
                    .state('app.estatisticas', {
                        url: '/estatisticas',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.estatisticas.estatisticasGeral', {
                        url: '/geral',
                        templateUrl: 'app_components/Estatisticas/EstatisticasGerais.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Estatisticas/EstatisticasGerais.js']);
                                }]
                        }
                    })
                    .state('app.problemas', {
                        url: '/problemas',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.problemas.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/problemas/TabelaProblemas.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/problemas/TabelaProblemas.js']);
                                }]
                        }
                    })

                    .state('app.problemas.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/Problemas/EditProblemas.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Problemas/EditProblemas.js']);
                                }]
                        }
                        
                    })


                /*.state('app.problemas.edit2', {
                    url: '/edit/:id/:desc/:causa',
                    templateUrl: 'app_components/Problemas/EditProblemas.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function ($ocLazyLoad) {
                                return $ocLazyLoad.load(['app_components/Problemas/EditProblemas.js']);
                            }]
                    }
                })*/

                    .state('app.relatorios', {
                        url: '/relatorios',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.relatorios.relatorioEncomenda', {
                        url: '/relatorioEncomenda',
                        templateUrl: 'app_components/Relatorios/RelatorioEncomenda.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Relatorios/RelatorioEncomenda.js']);
                                }]
                        }
                    })

                    .state('app.relatorios.relatorioSav', {
                        url: '/relatorioSav',
                        templateUrl: 'app_components/Relatorios/RelatorioSav.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/Relatorios/RelatorioSav.js']);
                                }]
                        }
                    })

                    .state('app.recolhas', {
                        url: '/recolhas',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.recolhas.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/recolhas/Tabelarecolhas.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/recolhas/TabelaRecolhas.js']);
                                }]
                        }
                    })
                    .state('app.estadosRecolha', {
                        url: '/estadosRecolha',
                        template: '<div ui-view class=""></div>'
                    })
                    .state('app.estadosRecolha.tabela', {
                        url: '/tabela',
                        templateUrl: 'app_components/estadosRecolha/TabelaEstadoRecolha.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/estadosRecolha/TabelaEstadoRecolha.js']);
                                }]
                        }
                    })
                    .state('app.estadosRecolha.edit', {
                        url: '/edit/:id',
                        templateUrl: 'app_components/estadosRecolha/EditEstadoRecolha.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/estadosRecolha/EditEstadoRecolha.js']);
                                }]
                        }
                    })
                    .state('app.csv', {
                        url: '/csv',
                        templateUrl: 'app_components/CSV/CSV.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['app_components/csv/csv.js']);
                                }]
                        }
                    })

                    .state('app.dashboard-v1', {
                        url: '/dashboard-v1',
                        templateUrl: 'tpl/app_dashboard_v1.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {

                                    return $ocLazyLoad.load(['app_components/dashboard/dashboard.js']);
                                }]
                        }
                    })
                    .state('app.dashboard-v2', {
                        url: '/dashboard-v2',
                        templateUrl: 'tpl/app_dashboard_v2.html',
                        resolve: {
                            deps: ['$ocLazyLoad',
                                function ($ocLazyLoad) {
                                    return $ocLazyLoad.load(['js/controllers/chart.js']);
                                }]
                        }
                    })

                    .state('access', {
                        url: '/access',
                        template: '<div ui-view class="fade-in-right-big smooth"></div>'
                    })
                    .state('access.signin', {
                        url: '/signin/:returnUrl',
                        templateUrl: 'tpl/page_signin.html',
                        resolve: {
                            deps: ['uiLoad',
                                function (uiLoad) {
                                    return uiLoad.load(['js/controllers/signin.js']);
                                }]
                        }
                    })
                    .state('access.signout', {
                        url: '/signout',
                        templateUrl: 'tpl/page_signout.html',
                        resolve: {
                            deps: ['uiLoad',
                                function (uiLoad) {
                                    return uiLoad.load(['js/controllers/signout.js']);
                                }]
                        }
                    })
                    .state('access.signup', {
                        url: '/signup',
                        templateUrl: 'tpl/page_signup.html',
                        resolve: {
                            deps: ['uiLoad',
                                function (uiLoad) {
                                    return uiLoad.load(['js/controllers/signup.js']);
                                }]
                        }
                    })
                    .state('access.forgotpwd', {
                        url: '/forgotpwd',
                        templateUrl: 'tpl/page_forgotpwd.html'
                    })
                    .state('access.404', {
                        url: '/404',
                        templateUrl: 'tpl/page_404.html'
                    })
                    .state('app.401', {
                        url: '/401',
                        templateUrl: 'tpl/page_401.html'
                    })
            }
        ]
    );

//angular.module('app').config(function (blockUIConfig) {

//    // Change the default overlay message
//    blockUIConfig.message = 'Por favor Aguarde';

//    // Change the default delay to 100ms before the blocking is visible
//    blockUIConfig.delay = 50;

//});