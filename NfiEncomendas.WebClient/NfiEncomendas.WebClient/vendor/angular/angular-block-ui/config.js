/// <reference path="angular-block-ui.ng.html" />
blkUI.constant('blockUIConfig', {
   templateUrl: 'angular-block-ui.ng.html',
    delay: 250,
    message: "Loading ...",
    autoBlock: true,
    resetOnException: true,
    requestFilter: angular.noop,
    autoInjectBodyBlock: true,
    cssClass: 'block-ui block-ui-anim-fade'
});

