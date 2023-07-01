angular.module('myApp', [
  'ngRoute',
  'myApp.view1',
  'myApp.view2',
  'myApp.home',
])
  .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');
    $routeProvider.otherwise({ redirectTo: '/' });
  }]);

angular.module('myApp').component('app', {
  templateUrl: 'app.html',
});

angular.module('myApp').component('appHeader', {
  templateUrl: 'header.html',
});