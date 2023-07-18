angular.module('myApp', ['ngRoute', 'myApp.homepage']).config([
  '$locationProvider',
  '$routeProvider',
  function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('')

    $routeProvider.otherwise({ redirectTo: '/' })
  },
])

angular.module('myApp').component('app', {
  templateUrl: 'app.html',
})
