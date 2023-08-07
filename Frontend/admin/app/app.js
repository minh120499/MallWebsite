angular.module('myApp', [
  'ngRoute',
  'ui.router',
  'myApp.dashboard',
  'myApp.banner',
  'myApp.store',
  'myApp.product',
  'myApp.feedback',
  'myApp.setting',
  'myApp.category',
])
  .config(['$locationProvider', '$routeProvider', '$httpProvider', '$rootScopeProvider',
    function ($locationProvider, $routeProvider, $httpProvider, $rootScopeProvider) {
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
            if (config.url.indexOf('/api/') === 0) {
              config.url = backendUrl + config.url;
            }
            return config;
          }
        };
      }]);
    }]);

angular.module('myApp').service('paginationService', function () {
  let page = 1;
  let limit = 10;
  let total = 0;

  return {
    getPage: function () {
      return page;
    },
    setPage: function (value) {
      page = value;
    },
    getLimit: function () {
      return limit;
    },
    setLimit: function (value) {
      limit = value;
    },
    getTotal: function () {
      return total;
    },
    setTotal: function (value) {
      total = value;
    }
  };
});

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
      template: '<div>Dashboard</div>'
    }

    var bannersState = {
      name: 'banners',
      url: '/banners',
      template: '<div>Banners</div>'
    }

    var categoriesState = {
      name: 'categories',
      url: '/categories',
      template: '<div>Categories</div>'
    }

    var storesState = {
      name: 'stores',
      url: '/stores',
      template: '<div>Stores</div>'
    }

    var productsState = {
      name: 'products',
      url: '/products',
      template: '<div>Products</div>'
    }

    var ordersState = {
      name: 'orders',
      url: '/orders',
      template: '<div>Orders</div>'
    }

    var usersState = {
      name: 'users',
      url: '/users',
      template: '<div>Users</div>'
    }

    var feedbacksState = {
      name: 'feedbacks',
      url: '/feedbacks',
      template: '<div>Feedbacks</div>'
    }

    var settingsState = {
      name: 'settings',
      url: '/settings',
      template: '<div>Settings</div>'
    }

    $stateProvider.state(dashboardState);
    $stateProvider.state(bannersState);
    $stateProvider.state(categoriesState);
    $stateProvider.state(storesState);
    $stateProvider.state(productsState);
    $stateProvider.state(ordersState);
    $stateProvider.state(usersState);
    $stateProvider.state(feedbacksState);
    $stateProvider.state(settingsState);
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