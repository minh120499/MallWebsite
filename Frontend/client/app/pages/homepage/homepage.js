angular.module('myApp.homepage', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider.when('/', {
        templateUrl: 'pages/homepage/homepage.html',
        controller: 'HomePageCtrl',
      })
    },
  ])
  .controller('HomePageCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
    function ($scope, $http, $rootScope, $location, paginationService) {
      document.title = 'Home';

      const { query, page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.banners = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      loadBanner($http, $scope, paginationService)
        .then(() => loadStore($http, $scope,))
        .then(startSlick);

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };
    }]);


function loadBanner($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  return $http.get(`/api/banners${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      $scope.banners = response.data.data;
      // $scope.total = response.data.total;
      // paginationService.setPage(response.data.page)
      // paginationService.setLimit(response.data.limit)
      // paginationService.setTotal(response.data.total)
      $scope.isLoading = false;
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
};

function loadStore($http, $scope) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  return $http.get(`/api/stores${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      $scope.stores = response.data.data;
      $scope.limit = response.data.limit;
      $scope.page = response.data.page;
      $scope.total = response.data.total;
      $scope.isLoading = false;
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
}

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
