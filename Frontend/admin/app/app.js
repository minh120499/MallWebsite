angular.module('myApp', [
  'ngRoute',
  'myApp.view1',
  'myApp.view2',
  'myApp.home',
])
  .config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('');
    $routeProvider
      .when('/view1', {
        templateUrl: 'homePage/home.html'
      })
    // .when('/view2', {
    //   template: '<my-app-view2></my-app-view2>'
    // })
    // .otherwise({ redirectTo: '/' });
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