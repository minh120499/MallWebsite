angular.module('myApp.dashboard', ['ngRoute'])
  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/dashboard', {
      templateUrl: 'page/dashboard/dashboard.html',
      controller: 'DashboardCtrl'
    });
  }])
  .controller('DashboardCtrl', [function () {

  }]);