'use strict';
var _isLoggedOut = false;
/* Controllers */
  // signin controller
app.controller('SignOutController',['$scope', '$location', '$state', 'authService', function ($scope, $location,$state, authService) {
 
    authService.logOut();

    //$scope.app.name = "APP";
    $scope.authError = "Logout Sucesso!";
    $state.go('access.signin'); 

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