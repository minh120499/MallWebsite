angular.module('myApp', [
  'ngRoute',
])
  .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('');

    $routeProvider
      .when('/', {
        templateUrl: 'pages/homepage/homepage.html',
      })
      .otherwise({ redirectTo: '/' });
  }]);

angular.module('myApp').component('app', {
  templateUrl: 'app.html',
});