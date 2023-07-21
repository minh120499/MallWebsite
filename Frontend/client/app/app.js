angular.module('myApp', ['ngRoute', 'myApp.homepage'])

  .config(['$locationProvider', '$routeProvider', '$httpProvider', function ($locationProvider, $routeProvider, $httpProvider) {
    var backendUrl = 'http://localhost:5062';

    $locationProvider.hashPrefix('');

    $routeProvider
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
