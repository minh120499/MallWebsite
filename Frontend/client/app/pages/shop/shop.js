angular.module('myApp.shop', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider.when('/shop', {
        templateUrl: 'pages/shop/shop.html',
        controller: 'ShopCtrl',
      })
    },
  ])
  .controller('ShopCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
    function ($scope, $http, $rootScope, $location, paginationService) {
      document.title = 'Home';

      const { query, page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.stores = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      $scope.isLeft = function (index) {
        return index % 4 === 0 || index % 4 === 1;
      };

      loadStore($http, $scope, paginationService)

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };

      $scope.onKeyPress = function(event) {
        if (event.keyCode === 13) {
          $scope.searchStore();
        }
      };

      $scope.searchStore = function () {
        const urlParams = window.location.hash.split("query=");
        if (urlParams[1]) {
          const queryValue = urlParams[1].split('&')[0];
          if (queryValue === $scope.query) {
            console.log(12);
            return;
          }
        }
        loadStore($http, $scope, paginationService);
        $location.search('limit', $scope.limit);
        $location.search('page', $scope.page);
        $location.search('query', $scope.query);
      }
    }]);


function loadStore($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  return $http.get(`/api/stores${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      $scope.stores = response.data.data;
      $scope.total = response.data.total;
      paginationService.setPage(response.data.page)
      paginationService.setLimit(response.data.limit)
      paginationService.setTotal(response.data.total)
      $scope.isLoading = false;
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
};
