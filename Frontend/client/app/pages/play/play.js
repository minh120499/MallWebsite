angular.module('myApp.play', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider
        .when('/play', {
          templateUrl: 'pages/play/play.html',
          controller: 'PlayCtrl',
        })
        .when('/play/:id', {
          templateUrl: 'pages/play/play-detail.html',
          controller: 'PlayDetailCtrl',
        })
    },
  ])
  .controller('PlayCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
    function ($scope, $http, $rootScope, $location, paginationService) {
      document.title = 'Home';

      const { query, page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;

      $scope.banners = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.loadFilter = false;

      loadStore($http, $scope, $location, paginationService)
        .then(() => {
          $scope.loadFilter = true;
        })

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };
    }])
  .controller('PlayDetailCtrl', ['$scope', '$http', '$rootScope', '$location', '$routeParams', 'paginationService',
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
  const { query, floorId, categoryId, facilityIds } = $location.search();

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  query && params.append('query', query);
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

const startSlick = () => {
  setTimeout(() => {
    $('.slick').slick({
      // autoplay: true,
      autoplaySpeed: 2000,
      dots: true,
      slidesToShow: 1,
      slidesToScroll: 1,
      prevArrow: '<button class="slide-arrow prev-arrow"></button>',
      nextArrow: '<button class="slide-arrow next-arrow"></button>',
    })
  })
}

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