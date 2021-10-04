'use strict';
// Source: js/app.js
angular.module('app', [ 

    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngSanitize',
    'ngTouch',
    'ngStorage',
    'ui.router',
    'ui.bootstrap',    
    'ui.load',
    'ui.jq',
    'ui.validate',
    'toaster',
    'oc.lazyLoad',
    'ngTable',
    //'pascalprecht.translate',
    'ng.httpLoader',
    'ngCookies',
    'ui.select',
    //'blockUI',
    'pasvaz.bindonce', 'angular-jquery-maskedinput'
    
]);

var serviceBase = (document.location.origin + document.location.pathname) + "apiserver/";
var secondsTimer = 0;

function RefreshTimer()
{    
    secondsTimer = Math.round(new Date().getTime() / 1000);
}

// Source: js/config.js
// config

var app =  
angular.module('app')
  .config(
    [        '$controllerProvider', '$compileProvider', '$filterProvider', '$provide',
    function ($controllerProvider,   $compileProvider,   $filterProvider,   $provide) {
        
        // lazy controller, directive and service
        app.controller = $controllerProvider.register;
        app.directive  = $compileProvider.directive;
        app.filter     = $filterProvider.register;
        app.factory    = $provide.factory;
        app.service    = $provide.service;
        app.constant   = $provide.constant;
        app.value = $provide.value;
        app.utilizador = null
        
    }
  ])
   .config([
          'httpMethodInterceptorProvider',
          function (httpMethodInterceptorProvider) {
              httpMethodInterceptorProvider.whitelistDomain(document.domain);
          }
   ]);

app.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
});





// Source: js/config.lazyload.js
// lazyload config

angular.module('app')
    /**
   * jQuery plugin config use ui-jq directive , config the js and css files that required
   * key: function name of the jQuery plugin
   * value: array of the css js file located
   */
  .constant('JQ_CONFIG', {
      easyPieChart:   ['vendor/jquery/charts/easypiechart/jquery.easy-pie-chart.js'],
      sparkline:      ['vendor/jquery/charts/sparkline/jquery.sparkline.min.js'],
      plot:           ['vendor/jquery/charts/flot/jquery.flot.min.js', 
                          'vendor/jquery/charts/flot/jquery.flot.resize.js',
                          'vendor/jquery/charts/flot/jquery.flot.tooltip.min.js',
                          'vendor/jquery/charts/flot/jquery.flot.spline.js',
                          'vendor/jquery/charts/flot/jquery.flot.orderBars.js',
                          'vendor/jquery/charts/flot/jquery.flot.pie.min.js'],
      slimScroll:     ['vendor/jquery/slimscroll/jquery.slimscroll.min.js'],
      sortable:       ['vendor/jquery/sortable/jquery.sortable.js'],
      nestable:       ['vendor/jquery/nestable/jquery.nestable.js',
                          'vendor/jquery/nestable/nestable.css'],
      filestyle:      ['vendor/jquery/file/bootstrap-filestyle.min.js'],
      slider:         ['vendor/jquery/slider/bootstrap-slider.js',
                          'vendor/jquery/slider/slider.css'],
      chosen:         ['vendor/jquery/chosen/chosen.jquery.min.js',
                          'vendor/jquery/chosen/chosen.css'],
      TouchSpin:      ['vendor/jquery/spinner/jquery.bootstrap-touchspin.min.js',
                          'vendor/jquery/spinner/jquery.bootstrap-touchspin.css'],
      wysiwyg:        ['vendor/jquery/wysiwyg/bootstrap-wysiwyg.js',
                          'vendor/jquery/wysiwyg/jquery.hotkeys.js'],
      dataTable:      ['vendor/jquery/datatables/jquery.dataTables.min.js',
                          'vendor/jquery/datatables/dataTables.bootstrap.js',
                          'vendor/jquery/datatables/dataTables.bootstrap.css'],
      vectorMap:      ['vendor/jquery/jvectormap/jquery-jvectormap.min.js', 
                          'vendor/jquery/jvectormap/jquery-jvectormap-world-mill-en.js',
                          'vendor/jquery/jvectormap/jquery-jvectormap-us-aea-en.js',
                          'vendor/jquery/jvectormap/jquery-jvectormap.css'],
      footable:       ['vendor/jquery/footable/footable.all.min.js',
                          'vendor/jquery/footable/footable.core.css']
      }
  )
  // oclazyload config
  .config(['$ocLazyLoadProvider', function($ocLazyLoadProvider) {
      // We configure ocLazyLoad to use the lib script.js as the async loader
      $ocLazyLoadProvider.config({
          debug: false,
          events: true,
          modules: [
              {
                  name: 'ngGrid',
                  files: [
                      'vendor/modules/ng-grid/ng-grid.min.js',
                      'vendor/modules/ng-grid/ng-grid.min.css',
                      'vendor/modules/ng-grid/theme.css'
                  ]
              },
              {
                  name: 'ui.select',
                  files: [
                      'vendor/modules/angular-ui-select/select.min.js',
                      'vendor/modules/angular-ui-select/select.min.css'
                  ]
              },
              {
                  name:'angularFileUpload',
                  files: [
                    'vendor/modules/angular-file-upload/angular-file-upload.min.js'
                  ]
              },
              {
                  name:'ui.calendar',
                  files: ['vendor/modules/angular-ui-calendar/calendar.js']
              },
              {
                  name: 'ngImgCrop',
                  files: [
                      'vendor/modules/ngImgCrop/ng-img-crop.js',
                      'vendor/modules/ngImgCrop/ng-img-crop.css'
                  ]
              },
              {
                  name: 'angularBootstrapNavTree',
                  files: [
                      'vendor/modules/angular-bootstrap-nav-tree/abn_tree_directive.js',
                      'vendor/modules/angular-bootstrap-nav-tree/abn_tree.css'
                  ]
              },
              {
                  name: 'toaster',
                  files: [
                      'vendor/modules/angularjs-toaster/toaster.js',
                      'vendor/modules/angularjs-toaster/toaster.css'
                  ]
              },
              {
                  name: 'textAngular',
                  files: [
                      'vendor/modules/textAngular/textAngular-sanitize.min.js',
                      'vendor/modules/textAngular/textAngular.min.js'
                  ]
              },
              {
                  name: 'vr.directives.slider',
                  files: [
                      'vendor/modules/angular-slider/angular-slider.min.js',
                      'vendor/modules/angular-slider/angular-slider.css'
                  ]
              }
          ]
      });
  }])
;
// Source: js/config.router.js
/// <reference path="../app_components/admin/GestaoOperadores/GestaoOperadores.js" />
/// <reference path="config.router.js" />
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
                    url: '/edit/:id',
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
                  .state('app.encomendas.tabela3', {
                      url: '/tabela3',
                      templateUrl: 'app_components/encomendas/TabelaEncomendas3.html',
                      resolve: {
                          deps: ['$ocLazyLoad',
                              function ($ocLazyLoad) {
                                  return $ocLazyLoad.load(['app_components/encomendas/TabelaEncomendas3.js']);
                              }]
                      }
                  })
                .state('app.encomendas.edit', {
                    url: '/edit/:serie/:num',
                    templateUrl: 'app_components/encomendas/EditEncomenda.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function ($ocLazyLoad) {
                                return $ocLazyLoad.load(['app_components/encomendas/EditEncomenda.js', ]);
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
                    url: '/edit/:id',
                    templateUrl: 'app_components/savs/EditSav.html',
                    resolve: {
                        deps: ['$ocLazyLoad',
                            function ($ocLazyLoad) {
                                return $ocLazyLoad.load(['app_components/savs/EditSav.js', ]);
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
              .state('app.ui', {
                  url: '/ui',
                  template: '<div ui-view class=""></div>'
              })
              .state('app.ui.buttons', {
                  url: '/buttons',
                  templateUrl: 'tpl/ui_buttons.html'
              })
              .state('app.ui.icons', {
                  url: '/icons',
                  templateUrl: 'tpl/ui_icons.html'
              })
              .state('app.ui.grid', {
                  url: '/grid',
                  templateUrl: 'tpl/ui_grid.html'
              })
              .state('app.ui.widgets', {
                  url: '/widgets',
                  templateUrl: 'tpl/ui_widgets.html'
              })
              .state('app.ui.bootstrap', {
                  url: '/bootstrap',
                  templateUrl: 'tpl/ui_bootstrap.html'
              })
              .state('app.ui.sortable', {
                  url: '/sortable',
                  templateUrl: 'tpl/ui_sortable.html'
              })
              .state('app.ui.portlet', {
                  url: '/portlet',
                  templateUrl: 'tpl/ui_portlet.html'
              })
              .state('app.ui.timeline', {
                  url: '/timeline',
                  templateUrl: 'tpl/ui_timeline.html'
              })
              .state('app.ui.tree', {
                  url: '/tree',
                  templateUrl: 'tpl/ui_tree.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('angularBootstrapNavTree').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/tree.js');
                                }
                            );
                        }
                      ]
                  }
              })
              .state('app.ui.toaster', {
                  url: '/toaster',
                  templateUrl: 'tpl/ui_toaster.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('toaster').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/toaster.js');
                                }
                            );
                        }]
                  }
              })
              .state('app.ui.jvectormap', {
                  url: '/jvectormap',
                  templateUrl: 'tpl/ui_jvectormap.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('js/controllers/vectormap.js');
                        }]
                  }
              })
              .state('app.ui.googlemap', {
                  url: '/googlemap',
                  templateUrl: 'tpl/ui_googlemap.html',
                  resolve: {
                      deps: ['uiLoad',
                        function (uiLoad) {
                            return uiLoad.load([
                              'js/app/map/load-google-maps.js',
                              'js/app/map/ui-map.js',
                              'js/app/map/map.js']).then(
                                function () {
                                    return loadGoogleMaps();
                                }
                              );
                        }]
                  }
              })
              .state('app.chart', {
                  url: '/chart',
                  templateUrl: 'tpl/ui_chart.html',
                  resolve: {
                      deps: ['uiLoad',
                        function (uiLoad) {
                            return uiLoad.load('js/controllers/chart.js');
                        }]
                  }
              })
              // table
              .state('app.table', {
                  url: '/table',
                  template: '<div ui-view></div>'
              })
              .state('app.table.static', {
                  url: '/static',
                  templateUrl: 'tpl/table_static.html'
              })
              .state('app.table.datatable', {
                  url: '/datatable',
                  templateUrl: 'tpl/table_datatable.html'
              })
              .state('app.table.footable', {
                  url: '/footable',
                  templateUrl: 'tpl/table_footable.html'
              })
              .state('app.table.grid', {
                  url: '/grid',
                  templateUrl: 'tpl/table_grid.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('ngGrid').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/grid.js');
                                }
                            );
                        }]
                  }
              })
              // form
              .state('app.form', {
                  url: '/form',
                  template: '<div ui-view class="fade-in"></div>',
                  resolve: {
                      deps: ['uiLoad',
                        function (uiLoad) {
                            return uiLoad.load('js/controllers/form.js');
                        }]
                  }
              })
              .state('app.form.elements', {
                  url: '/elements',
                  templateUrl: 'tpl/form_elements.html'
              })
              .state('app.form.validation', {
                  url: '/validation',
                  templateUrl: 'tpl/form_validation.html'
              })
              .state('app.form.wizard', {
                  url: '/wizard',
                  templateUrl: 'tpl/form_wizard.html'
              })
              .state('app.form.fileupload', {
                  url: '/fileupload',
                  templateUrl: 'tpl/form_fileupload.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('angularFileUpload').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/file-upload.js');
                                }
                            );
                        }]
                  }
              })
              .state('app.form.imagecrop', {
                  url: '/imagecrop',
                  templateUrl: 'tpl/form_imagecrop.html',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('ngImgCrop').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/imgcrop.js');
                                }
                            );
                        }]
                  }
              })
              .state('app.form.select', {
                  url: '/select',
                  templateUrl: 'tpl/form_select.html',
                  controller: 'SelectCtrl',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('ui.select').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/select.js');
                                }
                            );
                        }]
                  }
              })
              .state('app.form.slider', {
                  url: '/slider',
                  templateUrl: 'tpl/form_slider.html',
                  controller: 'SliderCtrl',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('vr.directives.slider').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/slider.js');
                                }
                            );
                        }]
                  }
              })
              .state('app.form.editor', {
                  url: '/editor',
                  templateUrl: 'tpl/form_editor.html',
                  controller: 'EditorCtrl',
                  resolve: {
                      deps: ['$ocLazyLoad',
                        function ($ocLazyLoad) {
                            return $ocLazyLoad.load('textAngular').then(
                                function () {
                                    return $ocLazyLoad.load('js/controllers/editor.js');
                                }
                            );
                        }]
                  }
              })
              // pages
              .state('app.page', {
                  url: '/page',
                  template: '<div ui-view class="fade-in-down"></div>'
              })
              .state('app.page.profile', {
                  url: '/profile',
                  templateUrl: 'tpl/page_profile.html'
              })
              .state('app.page.post', {
                  url: '/post',
                  templateUrl: 'tpl/page_post.html'
              })
              .state('app.page.search', {
                  url: '/search',
                  templateUrl: 'tpl/page_search.html'
              })
              .state('app.page.invoice', {
                  url: '/invoice',
                  templateUrl: 'tpl/page_invoice.html'
              })
              .state('app.page.price', {
                  url: '/price',
                  templateUrl: 'tpl/page_price.html'
              })
              .state('app.docs', {
                  url: '/docs',
                  templateUrl: 'tpl/docs.html'
              })
              // others
              .state('lockme', {
                  url: '/lockme',
                  templateUrl: 'tpl/page_lockme.html'
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
// Source: js/main.js
/* Controllers */

angular.module('app')
  .controller('AppCtrl', ['$scope', '$localStorage', '$window', '$rootScope',
    function ($scope, $localStorage, $window, $rootScope) {
        // add 'ie' classes to html


        var isIE = !!navigator.userAgent.match(/MSIE/i);
        isIE && angular.element($window.document.body).addClass('ie');
        isSmartDevice($window) && angular.element($window.document.body).addClass('smart');


        // config
        $scope.app = {
            name: 'N.F.I. ',
            version: '2',
            // for chart colors
            color: {
                primary: '#7266ba',
                info: '#23b7e5',
                success: '#27c24c',
                warning: '#fad733',
                danger: '#f05050',
                light: '#e8eff0',
                dark: '#3a3f51',
                black: '#1c2b36'
            },
            settings: {
                themeID: 1,
                navbarHeaderColor: 'bg-danger',
                navbarCollapseColor: 'bg-white-only',
                asideColor: 'bg-dark',
                headerFixed: true,
                asideFixed: false,
                asideFolded: false,
                asideDock: false,
                container: false
            }
        }
       

        // save settings to local storage
        //nfiEncAuthorizationData
        if (angular.isDefined($localStorage.nfiEncSettings)) {
            $scope.app.settings = $localStorage.nfiEncSettings;
        } else {
            $localStorage.nfiEncSettings = $scope.app.settings;
        }
        $scope.$watch('app.settings', function () {
            if ($scope.app.settings.asideDock && $scope.app.settings.asideFixed) {
                // aside dock and fixed must set the header fixed.
                $scope.app.settings.headerFixed = true;
            }
            // save to local storage
            $localStorage.nfiEncSettings = $scope.app.settings;
        }, true);

        var appUtilizador = $scope.$watch(function () {
            return app.utilizador;
        }, function () {
            $scope.mostra = app.utilizador != null;
            //console.log("mostra " + $scope.mostra +  app.utilizador);
            // if ($scope.mostra == true) appUtilizador();
            //$scope.$apply();
        });

        // angular translate
        //$scope.lang = { isopen: false };
        //$scope.langs = {en:'English', de_DE:'German', it_IT:'Italian'};
        //$scope.selectLang = $scope.langs[$translate.proposedLanguage()] || "English";
        //$scope.setLang = function(langKey, $event) {
        //  // set the current lang
        //  $scope.selectLang = $scope.langs[langKey];
        //  // You can change the language during runtime
        //  $translate.use(langKey);
        //  $scope.lang.isopen = !$scope.lang.isopen;
        //};

        function isSmartDevice($window) {
            // Adapted from http://www.detectmobilebrowsers.com
            var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
            // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
            return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
        }

    }]);
// Source: js/services/ui-load.js
/**
 * 0.1.1
 * Deferred load js/css file, used for ui-jq.js and Lazy Loading.
 * 
 * @ flatfull.com All Rights Reserved.
 * Author url: http://themeforest.net/user/flatfull
 */

angular.module('ui.load', [])
	.service('uiLoad', ['$document', '$q', '$timeout', function ($document, $q, $timeout) {

		var loaded = [];
		var promise = false;
		var deferred = $q.defer();

		/**
		 * Chain loads the given sources
		 * @param srcs array, script or css
		 * @returns {*} Promise that will be resolved once the sources has been loaded.
		 */
		this.load = function (srcs) {
			srcs = angular.isArray(srcs) ? srcs : srcs.split(/\s+/);
			var self = this;
			if(!promise){
				promise = deferred.promise;
			}
      angular.forEach(srcs, function(src) {
      	promise = promise.then( function(){
      		return src.indexOf('.css') >=0 ? self.loadCSS(src) : self.loadScript(src);
      	} );
      });
      deferred.resolve();
      return promise;
		}

		/**
		 * Dynamically loads the given script
		 * @param src The url of the script to load dynamically
		 * @returns {*} Promise that will be resolved once the script has been loaded.
		 */
		this.loadScript = function (src) {
			if(loaded[src]) return loaded[src].promise;

			var deferred = $q.defer();
			var script = $document[0].createElement('script');
			script.src = src;
			script.onload = function (e) {
				$timeout(function () {
					deferred.resolve(e);
				});
			};
			script.onerror = function (e) {
				$timeout(function () {
					deferred.reject(e);
				});
			};
			$document[0].body.appendChild(script);
			loaded[src] = deferred;

			return deferred.promise;
		};

		/**
		 * Dynamically loads the given CSS file
		 * @param href The url of the CSS to load dynamically
		 * @returns {*} Promise that will be resolved once the CSS file has been loaded.
		 */
		this.loadCSS = function (href) {
			if(loaded[href]) return loaded[href].promise;

			var deferred = $q.defer();
			var style = $document[0].createElement('link');
			style.rel = 'stylesheet';
			style.type = 'text/css';
			style.href = href;
			style.onload = function (e) {
				$timeout(function () {
					deferred.resolve(e);
				});
			};
			style.onerror = function (e) {
				$timeout(function () {
					deferred.reject(e);
				});
			};
			$document[0].head.appendChild(style);
			loaded[href] = deferred;

			return deferred.promise;
		};
}]);
// Source: js/services/AuthService.js
angular.module('app').factory('authService', ['$http', '$q', '$localStorage', function ($http, $q, $localStorage) {
   
    var authServiceFactory = {};
    var _authentication = {
        isAuth: false,
        userName: "",
        isLoggedOut: false
    };
    var _saveRegistration = function (registration) {
       
        _logOut();
        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
   
            return response;
        });

    };

    var _login = function (loginData) {
        //console.log("_login");
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            $localStorage.nfiEncAuthorizationData = { token: response.access_token, userName: loginData.userName };

            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;
            _authentication.isLoggedOut = false;
            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {
        $http.post(serviceBase + 'api/account/logout', { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })

        delete $localStorage.nfiEncAuthorizationData;
        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.isLoggedOut = true;
       // app.utilizador = null;
        //console.log("_logOut");
    };

    var _fillAuthData = function () {
        
        var authData = $localStorage.nfiEncAuthorizationData;
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.isLoggedOut = false;
        }
        else {
            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.isLoggedOut = false;
        }
       //console.log("_fillAuthData return");
       //console.log(authData);
    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);

angular.module('app').factory('authInterceptorService', ['$q', '$injector', '$location', '$localStorage', function ($q, $injector, $location, $localStorage) {
    //console.log("authInterceptorService");
    var authInterceptorServiceFactory = {};

    var _request = function (config) {
        //console.log("HTTP REQUEST");
        //console.log(config);
        config.headers = config.headers || {};

        var authData = $localStorage.nfiEncAuthorizationData;
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }
        RefreshTimer();
        return config;
    }

    var _responseError = function (rejection) {
        //console.log("_responseError");
        //console.log(rejection);
        if (rejection.status === 401) {
            var authService = $injector.authService;
            var authData = $localStorage.nfiEncAuthorizationData;
            
            if (authData) {
                if (authData.useRefreshTokens) {
              //      console.log("useRefreshTokens");
                   // $location.path('/refresh');
                    
                    return $q.reject(rejection);
                }
            }
            //console.log("useRefreshTokens = false");
           // authService.logOut();

            if (rejection.config.url.indexOf("Account/GetMe") > -1)
            {
                //authService.logOut();
              //  console.log("erro get me");
                $location.path('/access/signout');
                //caltion.reload(true);
            }
            else
                {
                $location.path('/access/signout');
                //$location.path('access/401');
            }
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);

angular.module('app').factory('tokensManagerService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var tokenManagerServiceFactory = {};
    //console.log("tokensManagerService");
    var _getRefreshTokens = function () {
        //console.log("_getRefreshTokens");
        return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
            return results;
        });
    };

    var _deleteRefreshTokens = function (tokenid) {
        //console.log("_deleteRefreshTokens");
        return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
            return results;
        });
    };

    tokenManagerServiceFactory.deleteRefreshTokens = _deleteRefreshTokens;
    tokenManagerServiceFactory.getRefreshTokens = _getRefreshTokens;
    //console.log("return tokenManagerServiceFactory");
    return tokenManagerServiceFactory;

}]);

angular.module('app').constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

angular.module('app').config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
    $httpProvider.interceptors.push('httpMethodInterceptor');
});

angular.module('app').run(['authService', function (authService) {
    authService.fillAuthData();
}]);
app.controller('tokensManagerController', ['$scope', 'tokensManagerService', function ($scope, tokensManagerService) {

    $scope.refreshTokens = [];

    tokensManagerService.getRefreshTokens().then(function (results) {

        $scope.refreshTokens = results.data;

    }, function (error) {
       // alert(error.data.message);
    });

    $scope.deleteRefreshTokens = function (index, tokenid) {

        tokenid = window.encodeURIComponent(tokenid);

        tokensManagerService.deleteRefreshTokens(tokenid).then(function (results) {

            $scope.refreshTokens.splice(index, 1);

        }, function (error) {
          //  alert(error.data.message);
        });
    }

}]);
// Source: js/filters/fromNow.js
/* Filters */
// need load the moment.js to use this filter. 
angular.module('app')
  .filter('fromNow', function() {
    return function(date) {
      return moment(date).fromNow();
    }
  });
// Source: js/directives/setnganimate.js
angular.module('app')
  .directive('setNgAnimate', ['$animate', function ($animate) {
    return {
        link: function ($scope, $element, $attrs) {
            $scope.$watch( function() {
                return $scope.$eval($attrs.setNgAnimate, $scope);
            }, function(valnew, valold){
                $animate.enabled(!!valnew, $element);
            });
        }
    };
  }]);
// Source: js/directives/ui-butterbar.js
angular.module('app')
  .directive('uiButterbar', ['$rootScope', '$anchorScroll', function($rootScope, $anchorScroll) {
     return {
      restrict: 'AC',
      template:'<span class="bar"></span>',
      link: function(scope, el, attrs) {        
        el.addClass('butterbar hide');
        scope.$on('$stateChangeStart', function(event) {
          $anchorScroll();
          el.removeClass('hide').addClass('active');
        });
        scope.$on('$stateChangeSuccess', function( event, toState, toParams, fromState ) {
          event.targetScope.$watch('$viewContentLoaded', function(){
            el.addClass('hide').removeClass('active');
          })
        });
      }
     };
  }]);
// Source: js/directives/ui-focus.js
angular.module('app')
  .directive('uiFocus', function($timeout, $parse) {
    return {
      link: function(scope, element, attr) {
        var model = $parse(attr.uiFocus);
        scope.$watch(model, function(value) {
          if(value === true) {
            $timeout(function() {
              element[0].focus();
            });
          }
        });
        element.bind('blur', function() {
           scope.$apply(model.assign(scope, false));
        });
      }
    };
  });
// Source: js/directives/ui-fullscreen.js
 angular.module('app')
  .directive('uiFullscreen', ['uiLoad', '$document', '$window', function(uiLoad, $document, $window) {
    return {
      restrict: 'AC',
      template:'<i class="fa fa-expand fa-fw text"></i><i class="fa fa-compress fa-fw text-active"></i>',
      link: function(scope, el, attr) {
        el.addClass('hide');
        uiLoad.load('vendor/libs/screenfull.min.js').then(function(){
          // disable on ie11
          if (screenfull.enabled && !navigator.userAgent.match(/Trident.*rv:11\./)) {
            el.removeClass('hide');
          }
          el.on('click', function(){
            var target;
            attr.target && ( target = $(attr.target)[0] );            
            screenfull.toggle(target);
          });
          $document.on(screenfull.raw.fullscreenchange, function () {
            if(screenfull.isFullscreen){
              el.addClass('active');
            }else{
              el.removeClass('active');
            }
          });
        });
      }
    };
  }]);
// Source: js/directives/ui-jq.js
/**
 * 0.1.1
 * General-purpose jQuery wrapper. Simply pass the plugin name as the expression.
 *
 * It is possible to specify a default set of parameters for each jQuery plugin.
 * Under the jq key, namespace each plugin by that which will be passed to ui-jq.
 * Unfortunately, at this time you can only pre-define the first parameter.
 * @example { jq : { datepicker : { showOn:'click' } } }
 *
 * @param ui-jq {string} The $elm.[pluginName]() to call.
 * @param [ui-options] {mixed} Expression to be evaluated and passed as options to the function
 *     Multiple parameters can be separated by commas
 * @param [ui-refresh] {expression} Watch expression and refire plugin on changes
 *
 * @example <input ui-jq="datepicker" ui-options="{showOn:'click'},secondParameter,thirdParameter" ui-refresh="iChange">
 */
angular.module('ui.jq', ['ui.load']).
  value('uiJqConfig', {}).
  directive('uiJq', ['uiJqConfig', 'JQ_CONFIG', 'uiLoad', '$timeout', function uiJqInjectingFunction(uiJqConfig, JQ_CONFIG, uiLoad, $timeout) {

  return {
    restrict: 'A',
    compile: function uiJqCompilingFunction(tElm, tAttrs) {

      if (!angular.isFunction(tElm[tAttrs.uiJq]) && !JQ_CONFIG[tAttrs.uiJq]) {
        throw new Error('ui-jq: The "' + tAttrs.uiJq + '" function does not exist');
      }
      var options = uiJqConfig && uiJqConfig[tAttrs.uiJq];

      return function uiJqLinkingFunction(scope, elm, attrs) {

        function getOptions(){
          var linkOptions = [];

          // If ui-options are passed, merge (or override) them onto global defaults and pass to the jQuery method
          if (attrs.uiOptions) {
            linkOptions = scope.$eval('[' + attrs.uiOptions + ']');
            if (angular.isObject(options) && angular.isObject(linkOptions[0])) {
              linkOptions[0] = angular.extend({}, options, linkOptions[0]);
            }
          } else if (options) {
            linkOptions = [options];
          }
          return linkOptions;
        }

        // If change compatibility is enabled, the form input's "change" event will trigger an "input" event
        if (attrs.ngModel && elm.is('select,input,textarea')) {
          elm.bind('change', function() {
            elm.trigger('input');
          });
        }

        // Call jQuery method and pass relevant options
        function callPlugin() {
          $timeout(function() {
            elm[attrs.uiJq].apply(elm, getOptions());
          }, 0, false);
        }

        function refresh(){
          // If ui-refresh is used, re-fire the the method upon every change
          if (attrs.uiRefresh) {
            scope.$watch(attrs.uiRefresh, function() {
              callPlugin();
            });
          }
        }

        if ( JQ_CONFIG[attrs.uiJq] ) {
          uiLoad.load(JQ_CONFIG[attrs.uiJq]).then(function() {
            callPlugin();
            refresh();
          }).catch(function() {
            
          });
        } else {
          callPlugin();
          refresh();
        }
      };
    }
  };
}]);
// Source: js/directives/ui-module.js
angular.module('app')
  .directive('uiModule', ['MODULE_CONFIG','uiLoad', '$compile', function(MODULE_CONFIG, uiLoad, $compile) {
    return {
      restrict: 'A',
      compile: function (el, attrs) {
        var contents = el.contents().clone();
        return function(scope, el, attrs){
          el.contents().remove();
          uiLoad.load(MODULE_CONFIG[attrs.uiModule])
          .then(function(){
            $compile(contents)(scope, function(clonedElement, scope) {
              el.append(clonedElement);
            });
          });
        }
      }
    };
  }]);
// Source: js/directives/ui-nav.js
angular.module('app')
  .directive('uiNav', ['$timeout', function($timeout) {
    return {
      restrict: 'AC',
      link: function(scope, el, attr) {
        var _window = $(window), 
        _mb = 768, 
        wrap = $('.app-aside'), 
        next, 
        backdrop = '.dropdown-backdrop';
        // unfolded
        el.on('click', 'a', function(e) {
          next && next.trigger('mouseleave.nav');
          var _this = $(this);
          _this.parent().siblings( ".active" ).toggleClass('active');
          _this.next().is('ul') &&  _this.parent().toggleClass('active') &&  e.preventDefault();
          // mobile
          _this.next().is('ul') || ( ( _window.width() < _mb ) && $('.app-aside').removeClass('show off-screen') );
        });

        // folded & fixed
        el.on('mouseenter', 'a', function(e){
          next && next.trigger('mouseleave.nav');
          $('> .nav', wrap).remove();
          if ( !$('.app-aside-fixed.app-aside-folded').length || ( _window.width() < _mb ) || $('.app-aside-dock').length) return;
          var _this = $(e.target)
          , top
          , w_h = $(window).height()
          , offset = 50
          , min = 150;

          !_this.is('a') && (_this = _this.closest('a'));
          if( _this.next().is('ul') ){
             next = _this.next();
          }else{
            return;
          }
         
          _this.parent().addClass('active');
          top = _this.parent().position().top + offset;
          next.css('top', top);
          if( top + next.height() > w_h ){
            next.css('bottom', 0);
          }
          if(top + min > w_h){
            next.css('bottom', w_h - top - offset).css('top', 'auto');
          }
          next.appendTo(wrap);

          next.on('mouseleave.nav', function(e){
            $(backdrop).remove();
            next.appendTo(_this.parent());
            next.off('mouseleave.nav').css('top', 'auto').css('bottom', 'auto');
            _this.parent().removeClass('active');
          });

          $('.smart').length && $('<div class="dropdown-backdrop"/>').insertAfter('.app-aside').on('click', function(next){
            next && next.trigger('mouseleave.nav');
          });

        });

        wrap.on('mouseleave', function(e){
          next && next.trigger('mouseleave.nav');
          $('> .nav', wrap).remove();
        });
      }
    };
  }]);
// Source: js/directives/ui-scroll.js
angular.module('app')
  .directive('uiScroll', ['$location', '$anchorScroll', function($location, $anchorScroll) {
    return {
      restrict: 'AC',
      link: function(scope, el, attr) {
        el.on('click', function(e) {
          $location.hash(attr.uiScroll);
          $anchorScroll();
        });
      }
    };
  }]);
// Source: js/directives/ui-shift.js
angular.module('app')
  .directive('uiShift', ['$timeout', function($timeout) {
    return {
      restrict: 'A',
      link: function(scope, el, attr) {
        // get the $prev or $parent of this el
        var _el = $(el),
            _window = $(window),
            prev = _el.prev(),
            parent,
            width = _window.width()
            ;

        !prev.length && (parent = _el.parent());
        
        function sm(){
          $timeout(function () {
            var method = attr.uiShift;
            var target = attr.target;
            _el.hasClass('in') || _el[method](target).addClass('in');
          });
        }
        
        function md(){
          parent && parent['prepend'](el);
          !parent && _el['insertAfter'](prev);
          _el.removeClass('in');
        }

        (width < 768 && sm()) || md();

        _window.resize(function() {
          if(width !== _window.width()){
            $timeout(function(){
              (_window.width() < 768 && sm()) || md();
              width = _window.width();
            });
          }
        });
      }
    };
  }]);
// Source: js/directives/ui-toggleclass.js
angular.module('app')
  .directive('uiToggleClass', ['$timeout', '$document', function($timeout, $document) {
    return {
      restrict: 'AC',
      link: function(scope, el, attr) {
        el.on('click', function(e) {
          e.preventDefault();
          var classes = attr.uiToggleClass.split(','),
              targets = (attr.target && attr.target.split(',')) || Array(el),
              key = 0;
          angular.forEach(classes, function( _class ) {
            var target = targets[(targets.length && key)];            
            ( _class.indexOf( '*' ) !== -1 ) && magic(_class, target);
            $( target ).toggleClass(_class);
            key ++;
          });
          $(el).toggleClass('active');

          function magic(_class, target){
            var patt = new RegExp( '\\s' + 
                _class.
                  replace( /\*/g, '[A-Za-z0-9-_]+' ).
                  split( ' ' ).
                  join( '\\s|\\s' ) + 
                '\\s', 'g' );
            var cn = ' ' + $(target)[0].className + ' ';
            while ( patt.test( cn ) ) {
              cn = cn.replace( patt, ' ' );
            }
            $(target)[0].className = $.trim( cn );
          }
        });
      }
    };
  }]);
// Source: js/controllers/bootstrap.js
/* Controllers */

  // bootstrap controller
  app.controller('AccordionDemoCtrl', ['$scope', function($scope) {
    $scope.oneAtATime = true;

    $scope.groups = [
      {
        title: 'Accordion group header - #1',
        content: 'Dynamic group body - #1'
      },
      {
        title: 'Accordion group header - #2',
        content: 'Dynamic group body - #2'
      }
    ];

    $scope.items = ['Item 1', 'Item 2', 'Item 3'];

    $scope.addItem = function() {
      var newItemNo = $scope.items.length + 1;
      $scope.items.push('Item ' + newItemNo);
    };

    $scope.status = {
      isFirstOpen: true,
      isFirstDisabled: false
    };
  }])
  ; 
  app.controller('AlertDemoCtrl', ['$scope', function($scope) {
    $scope.alerts = [
      { type: 'success', msg: 'Well done! You successfully read this important alert message.' },
      { type: 'info', msg: 'Heads up! This alert needs your attention, but it is not super important.' },
      { type: 'warning', msg: 'Warning! Best check yo self, you are not looking too good...' }
    ];

    $scope.addAlert = function() {
      $scope.alerts.push({type: 'danger', msg: 'Oh snap! Change a few things up and try submitting again.'});
    };

    $scope.closeAlert = function(index) {
      $scope.alerts.splice(index, 1);
    };
  }])
  ; 
  app.controller('ButtonsDemoCtrl', ['$scope', function($scope) {
    $scope.singleModel = 1;

    $scope.radioModel = 'Middle';

    $scope.checkModel = {
      left: false,
      middle: true,
      right: false
    };
  }])
  ; 
  app.controller('CarouselDemoCtrl', ['$scope', function($scope) {
    $scope.myInterval = 5000;
    var slides = $scope.slides = [];
    $scope.addSlide = function() {
      slides.push({
        image: 'img/c' + slides.length + '.jpg',
        text: ['Carousel text #0','Carousel text #1','Carousel text #2','Carousel text #3'][slides.length % 4]
      });
    };
    for (var i=0; i<4; i++) {
      $scope.addSlide();
    }
  }])
  ; 
  app.controller('DropdownDemoCtrl', ['$scope', function($scope) {
    $scope.items = [
      'The first choice!',
      'And another choice for you.',
      'but wait! A third!'
    ];

    $scope.status = {
      isopen: false
    };

    $scope.toggled = function(open) {
      //console.log('Dropdown is now: ', open);
    };

    $scope.toggleDropdown = function($event) {
      $event.preventDefault();
      $event.stopPropagation();
      $scope.status.isopen = !$scope.status.isopen;
    };
  }])
  ; 
  app.controller('ModalInstanceCtrl', ['$scope', '$modalInstance', 'items', function($scope, $modalInstance, items) {
    $scope.items = items;
    $scope.selected = {
      item: $scope.items[0]
    };

    $scope.ok = function () {
      $modalInstance.close($scope.selected.item);
    };

    $scope.cancel = function () {
      $modalInstance.dismiss('cancel');
    };
  }])
  ; 
  app.controller('ModalDemoCtrl', ['$scope', '$modal', '$log', function($scope, $modal, $log) {
    $scope.items = ['item1', 'item2', 'item3'];
    $scope.open = function (size) {
      var modalInstance = $modal.open({
        templateUrl: 'myModalContent.html',
        controller: 'ModalInstanceCtrl',
        size: size,
        resolve: {
          items: function () {
            return $scope.items;
          }
        }
      });

      modalInstance.result.then(function (selectedItem) {
        $scope.selected = selectedItem;
      }, function () {
        $log.info('Modal dismissed at: ' + new Date());
      });
    };
  }])
  ; 
  app.controller('PaginationDemoCtrl', ['$scope', '$log', function($scope, $log) {
    $scope.totalItems = 64;
    $scope.currentPage = 4;

    $scope.setPage = function (pageNo) {
      $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function() {
      $log.info('Page changed to: ' + $scope.currentPage);
    };

    $scope.maxSize = 5;
    $scope.bigTotalItems = 175;
    $scope.bigCurrentPage = 1;
  }])
  ; 
  app.controller('PopoverDemoCtrl', ['$scope', function($scope) {
    $scope.dynamicPopover = 'Hello, World!';
    $scope.dynamicPopoverTitle = 'Title';
  }])
  ; 
  app.controller('ProgressDemoCtrl', ['$scope', function($scope) {
    $scope.max = 200;

    $scope.random = function() {
      var value = Math.floor((Math.random() * 100) + 1);
      var type;

      if (value < 25) {
        type = 'success';
      } else if (value < 50) {
        type = 'info';
      } else if (value < 75) {
        type = 'warning';
      } else {
        type = 'danger';
      }

      $scope.showWarning = (type === 'danger' || type === 'warning');

      $scope.dynamic = value;
      $scope.type = type;
    };
    $scope.random();

    $scope.randomStacked = function() {
      $scope.stacked = [];
      var types = ['success', 'info', 'warning', 'danger'];

      for (var i = 0, n = Math.floor((Math.random() * 4) + 1); i < n; i++) {
          var index = Math.floor((Math.random() * 4));
          $scope.stacked.push({
            value: Math.floor((Math.random() * 30) + 1),
            type: types[index]
          });
      }
    };
    $scope.randomStacked();
  }])
  ; 
  app.controller('TabsDemoCtrl', ['$scope', function($scope) {
    $scope.tabs = [
      { title:'Dynamic Title 1', content:'Dynamic content 1' },
      { title:'Dynamic Title 2', content:'Dynamic content 2', disabled: true }
    ];
  }])
  ; 
  app.controller('RatingDemoCtrl', ['$scope', function($scope) {
    $scope.rate = 7;
    $scope.max = 10;
    $scope.isReadonly = false;

    $scope.hoveringOver = function(value) {
      $scope.overStar = value;
      $scope.percent = 100 * (value / $scope.max);
    };
  }])
  ; 
  app.controller('TooltipDemoCtrl', ['$scope', function($scope) {
    $scope.dynamicTooltip = 'Hello, World!';
    $scope.dynamicTooltipText = 'dynamic';
    $scope.htmlTooltip = 'I\'ve been made <b>bold</b>!';
  }])
  ; 
  app.controller('TypeaheadDemoCtrl', ['$scope', '$http', function($scope, $http) {
    $scope.selected = undefined;
    $scope.states = ['Alabama', 'Alaska', 'Arizona', 'Arkansas', 'California', 'Colorado', 'Connecticut', 'Delaware', 'Florida', 'Georgia', 'Hawaii', 'Idaho', 'Illinois', 'Indiana', 'Iowa', 'Kansas', 'Kentucky', 'Louisiana', 'Maine', 'Maryland', 'Massachusetts', 'Michigan', 'Minnesota', 'Mississippi', 'Missouri', 'Montana', 'Nebraska', 'Nevada', 'New Hampshire', 'New Jersey', 'New Mexico', 'New York', 'North Dakota', 'North Carolina', 'Ohio', 'Oklahoma', 'Oregon', 'Pennsylvania', 'Rhode Island', 'South Carolina', 'South Dakota', 'Tennessee', 'Texas', 'Utah', 'Vermont', 'Virginia', 'Washington', 'West Virginia', 'Wisconsin', 'Wyoming'];
    // Any function returning a promise object can be used to load values asynchronously
    $scope.getLocation = function(val) {
      return $http.get('http://maps.googleapis.com/maps/api/geocode/json', {
        params: {
          address: val,
          sensor: false
        }
      }).then(function(res){
        var addresses = [];
        angular.forEach(res.data.results, function(item){
          addresses.push(item.formatted_address);
        });
        return addresses;
      });
    };
  }])
  ; 
  app.controller('DatepickerDemoCtrl', ['$scope', function($scope) {
    $scope.today = function() {
      $scope.dt = new Date();
    };
    $scope.today();

    $scope.clear = function () {
      $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function(date, mode) {
      return ( mode === 'day' && ( date.getDay() === 0 || date.getDay() === 6 ) );
    };

    $scope.toggleMin = function() {
      $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.open = function($event) {
      $event.preventDefault();
      $event.stopPropagation();

      $scope.opened = true;
    };

    $scope.dateOptions = {
      formatYear: 'yy',
      startingDay: 1,
      class: 'datepicker'
    };

    $scope.initDate = new Date('2016-15-20');
    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
  }])
  ; 
  app.controller('TimepickerDemoCtrl', ['$scope', function($scope) {
    $scope.mytime = new Date();

    $scope.hstep = 1;
    $scope.mstep = 15;

    $scope.options = {
      hstep: [1, 2, 3],
      mstep: [1, 5, 10, 15, 25, 30]
    };

    $scope.ismeridian = true;
    $scope.toggleMode = function() {
      $scope.ismeridian = ! $scope.ismeridian;
    };

    $scope.update = function() {
      var d = new Date();
      d.setHours( 14 );
      d.setMinutes( 0 );
      $scope.mytime = d;
    };

    $scope.changed = function () {
      //console.log('Time changed to: ' + $scope.mytime);
    };

    $scope.clear = function() {
      $scope.mytime = null;
    };
  }]);
// Source: js/controllers/signin.js
/* Controllers */
  // signin controller
app.controller('SigninFormController', ['$scope', '$location', '$state', 'authService', '$cookies', function ($scope, $location, $state, authService, $cookies) {
    $scope.user = {};
   
    $scope.login = function () {
        $scope.loginData = {
            userName: $scope.user.email,
            password: $scope.user.passwordForm == $scope.user.passwordCookie ? $scope.user.passwordCookie : MD5($scope.user.passwordForm),
            save: $scope.user.guardarDados
        };

        authService.login($scope.loginData).then(function (response) {
            //console.log("$scope.user.guardarDados = " + $scope.user.guardarDados);
            //console.log($scope.user);
            if ($scope.user.guardarDados) {
                //console.log("guardar cookie");

                $cookies._userSaved_email = $scope.user.email;
                $cookies._userSaved_password = $scope.user.passwordForm == $scope.user.passwordCookie ? $scope.user.passwordCookie : MD5($scope.user.passwordForm);
            }
            else {
                $cookies._userSaved_email = "";
                $cookies._userSaved_password = "";

            }
            $state.go('app.dashboard-v1');
            //$location.path('/app/Dashboard-v1/');
        },
         function (err) {
             if (err.error == "invalid_grant") {
                 $scope.authError = "Nome de Utilizador ou Password errados.";
             }
             else if (err.error == "account_disabled") {
                 $scope.authError = "A conta de Utilizador está desactivada. Contacte o administrador.";
             }
             else {
                 $scope.authError = err.error_description;
             }

         });
    };


    if ($cookies._userSaved_email) {
        $scope.user.email = $cookies._userSaved_email;
        $scope.user.passwordForm = $cookies._userSaved_password;
        $scope.user.passwordCookie = $cookies._userSaved_password;
        $scope.user.guardarDados = true;
        if (authService.authentication.isLoggedOut == false)    $scope.login();
    }
    else {
        $scope.user.email = "";
        $scope.user.passwordForm = "";
        $scope.user.passwordCookie = "";
        $scope.user.guardarDados = true;
    }
    
    if (authService.authentication.isAuth == true)
    {        
        $state.go('app.dashboard-v1');
    }
    else
        {    
    $scope.authError = "";
    $scope.message = "";
 
    }
}]);





//    ['$scope', '$http', '$state', function ($scope, $http, $state) {
//    $scope.user = {};
//    $scope.authError = null;
//    $scope.login = function() {
//      $scope.authError = null;
//      // Try to login
//      $http.post('api/login', {email: $scope.user.email, password: $scope.user.password})
//      .then(function(response) {
//        if ( !response.data.user ) {
//          $scope.authError = 'Email or Password not right';
//        }else{
//          $state.go('app.dashboard-v1');
//        }
//      }, function(x) {
//        $scope.authError = 'Server Error';
//      });
//    };
//  }])
//;