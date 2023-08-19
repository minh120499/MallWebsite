angular.module('myApp.shop', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider
        .when('/shop', {
          templateUrl: 'pages/shop/shop.html',
          controller: 'ShopCtrl',
        })
        .when('/shop/:id', {
          templateUrl: 'pages/shop/shop-detail.html',
          controller: 'ShopDetailCtrl',
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
      $scope.loadFilter = false;

      $scope.isLeft = function (index) {
        return index % 4 === 0 || index % 4 === 1;
      };

      loadStore($http, $scope, $location, paginationService)
        .then(() => {
          $scope.loadFilter = true;
        });

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };

      $scope.onKeyPress = function (event) {
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
        $scope.loadFilter = false;
        loadStore($http, $scope, paginationService)
          .then(() => {
            $scope.loadFilter = true;
          });
        $location.search('limit', $scope.limit);
        $location.search('page', $scope.page);
        $location.search('query', $scope.query);
      }
    }])
  .controller('ShopDetailCtrl', ['$scope', '$http', '$rootScope', '$location', '$routeParams', 'paginationService',
    function ($scope, $http, $rootScope, $location, $routeParams, paginationService) {
      document.title = 'Shop';

      const { query, page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.store = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.storeId = $routeParams.id;

      $scope.isLeft = function (index) {
        return index % 4 === 0 || index % 4 === 1;
      };


      getStoreById($http, $scope)
        .then(() => {
          return getStoreProduct($http, $scope, paginationService);
        })
        .finally(() => {
          $scope.isLoading = false;
        })

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };

      $scope.onKeyPress = function (event) {
        if (event.keyCode === 13) {
          $scope.searchStore();
        }
      };
    }]);


function loadStore($http, $scope, $location, paginationService) {
  $scope.isLoading = true;

  var params = $location.search();
  const { query, floorId, categoryId, facilityIds } = params;
  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  ($scope.query || query) && params.append('query', $scope.query || query);
  floorId && params.append('floorId', floorId);
  categoryId && params.append('categoryId', categoryId);
  facilityIds && params.append('facilityIds', facilityIds);

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

function getStoreById($http, $scope) {
  $scope.isLoading = true;

  return $http.get(`/api/stores/${$scope.storeId}`)
    .then(function (response) {
      $scope.store = response.data;
      $scope.isLoading = false;
    })
    .then(() => {
      renderImage($scope.store.image);
      $scope.storeId = $scope.stores.find((s) => s.id === $scope.storeId)
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function getStoreProduct($http, $scope, paginationService) {
  return $http.get(`/api/stores/${$scope.storeId}/products`)
    .then(function (response) {
      $scope.products = response.data.data;
      $scope.total = response.data.total;
      paginationService.setPage(response.data.page)
      paginationService.setLimit(response.data.limit)
      paginationService.setTotal(response.data.total)
    })
    .catch(function (error) {
      $scope.error = error;
    });
}