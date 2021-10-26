'use strict';

/* Controllers */

app.controller('EditSerieCtrl', ['$scope', '$http', '$stateParams', '$state', 'ngTableParams', 'toaster',
function ($scope, $http, $stateParams, $state, ngTableParams, toaster) {

    $scope.serie = {};
    $scope.serie.numSerie = $stateParams.id;

    //console.log($stateParams.id);
    $scope.atualizaSerie = (function () {        
        $scope.serie.numSerie.replace(/\s+/g, '');//.toUpperCase();

        $scope.serie.numSerie = $scope.serie.numSerie.toUpperCase();
        if ($scope.serie.numSerie !== undefined && $scope.serie.numSerie.length > 0) {
            carregarserie($scope.serie.numSerie);
        }

    });


    function carregarserie(id) {
        $http.get(serviceBase + "api/Series/EditSerie/" + id)
            .success(function (data, status, headers, config) {
                if (data.serie.numSerie != $stateParams.id) {
                    //$state.go("app.series.edit", { id: data.serie.numserie });
                }
                $scope.dados = data;
                $scope.serie = data.serie;
            });

    }
    carregarserie($stateParams.id);

    $scope.submit = function () {
        $http.post(serviceBase + "api/Series/AtualizaSerie/", $scope.serie)
        .success(function (data, status, headers, config) {

            toaster.pop("success", "Serie Atualizada", "");
            $state.go("app.series.tabela");
        });
    }


}]);
