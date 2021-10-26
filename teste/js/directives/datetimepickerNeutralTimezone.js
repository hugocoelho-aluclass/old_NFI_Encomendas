(function () {
    'use strict';

    var directiveId = 'datetimepickerNeutralTimezone';

    var app = angular.module('app');
    app.directive(directiveId, [directive]);
    
    function directive() {
        return {
            restrict: 'A',
            priority: 1,
            require: 'ngModel',
            link: function (scope, element, attrs, ctrl) {
                ctrl.$formatters.push(function (value) {
                    
                    //console.log(value, element, attrs, ctrl);
                    var date = new Date(Date.parse(value));
                    date = new Date(date.getTime() + (-60000 * date.getTimezoneOffset()));
                    //moment().format("YYYY-MM-01")
                    //return date.toISOString().split("T")[0];

                    var m = moment(date);
                    return m.isValid() ? m.format("YYYY-MM-DD") : '';
                    //return moment(date).format("YYYY-MM-DD");
                });

                ctrl.$parsers.push(function (value) {
                    if (value == undefined) return;
                    var date = new Date(value.getTime() - (60000 * value.getTimezoneOffset()));
                    //return date.toISOString().split("T")[0];


                    var m = moment(date);
                    return m.isValid() ? m.format("YYYY-MM-DD") : '';
                    //return moment(date).format();

                });
            }
        };
    };


})();
