'use strict';
app.factory('SavGlobalService', ['$resource', '$http', function ($resource, $http) {
    var res = this;
    res.nDias = function (sav) {     
        var n = res.differenceInDays(sav.dataEntrada, sav.estadoResolvido ? sav.dataResolvido : new Date());
        return n;
    }

    res.nDias2 = function (sav) {
        //        console.log("dr = ", sav.dataResolvida, sav.marcarResolvida)
        var n = res.differenceInDays(sav.dataSav, sav.marcarResolvida ? sav.dataResolvida : new Date());

        return n;
    }


    res.differenceInDays = function (date1, date2) {

        var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
        var firstDate = new Date(date1);
        firstDate.setHours(0, 0, 0, 0);
        var secondDate = new Date(date2);
        secondDate.setHours(0, 0, 0, 0);

        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay))) + 1;

        return diffDays;

    };
    return res;


}]);