angular.module('myApp', [
  'ngRoute',
  'ui.router',
  'myApp.dashboard',
  'myApp.banner',
])
  .config(['$locationProvider', '$routeProvider', '$httpProvider', function ($locationProvider, $routeProvider, $httpProvider) {
    var backendUrl = 'http://localhost:5062';

    $locationProvider.hashPrefix('');

    $routeProvider
      .when('/', {
        templateUrl: 'page/dashboard/dashboard.html',
        controller: 'DashboardCtrl'
      })
      .otherwise({ redirectTo: '/' });

    $httpProvider.interceptors.push(['$q', function ($q) {
      return {
        request: function (config) {
          // Kiểm tra nếu URL không chứa domain backend
          if (config.url.indexOf('/api/') === 0) {
            // Thêm domain backend vào URL
            config.url = 'http://localhost:5062' + config.url;
          }
          return config;
        }
      };
    }]);
  }]);

angular.module('myApp').component('app', {
  templateUrl: 'app.html',
});

angular.module('myApp').component('appHeader', {
  templateUrl: 'header.html',
  controller: ['$scope', function SideBarController($scope) {
    $scope.sidebarVisible = true;

    $scope.toggleSidebar = function () {
      $scope.sidebarVisible = !$scope.sidebarVisible;
    };
  }]
})
  .config(function ($stateProvider) {
    var dashboardState = {
      name: 'dashboard',
      url: '/',
      template: '<div ng-click="toggleSidebar()">Dashboard</div>'
    }

    var bannersState = {
      name: 'banners',
      url: '/banners',
      template: '<div ng-click="toggleSidebar()">Banners</div>'
    }

    $stateProvider.state(dashboardState);
    $stateProvider.state(bannersState);
  });

angular.module('myApp').component('sideBar', {
  templateUrl: 'sideBar.html',
  controller: ['$scope', function SideBarController($scope) {
    $scope.sidebarVisible = true;

    $scope.toggleSidebar = function () {
      $scope.sidebarVisible = !$scope.sidebarVisible;
    };
  }]
});

angular.module('myApp').component('appFooter', {
  templateUrl: 'footer.html',
});