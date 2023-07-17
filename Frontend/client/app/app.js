angular.module('myApp', [
  'ngRoute',
])
  .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('');

  }]);

angular.module('myApp').component('app', {
  templateUrl: 'app.html',
});

angular.module('myApp').component('appHeader', {
  templateUrl: 'header.html',
});

angular.module('myApp').component('appFooter', {
  templateUrl: 'footer.html',
});