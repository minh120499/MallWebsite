angular.module('myApp.home', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', {
      templateUrl: 'homePage/home.html',
      controller: 'HomePageCtrl'
    });
  }])

  .controller('HomePageCtrl', [function () {

  }]);