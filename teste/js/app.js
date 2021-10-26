'use strict';

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
    //'ngCookies',
    'ui.select',
    //'blockUI',
    'pasvaz.bindonce',
    'angular-jquery-maskedinput',
    'angularMoment',
    'colorpicker.module'

]);

var serviceBase = (document.location.origin + document.location.pathname) + "apiserver_app/";
var secondsTimer = 0;

function RefreshTimer()
{    
    secondsTimer = Math.round(new Date().getTime() / 1000);
}
