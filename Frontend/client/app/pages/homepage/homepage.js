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

      $scope.data = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      loadBanner($http, $scope, paginationService);

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };
    }]);


async function loadBanner($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  await $http.get(`/api/banners${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      console.log("Banner", response);
      $scope.data = response.data.data;
      $scope.total = response.data.total;
      paginationService.setPage(response.data.page)
      paginationService.setLimit(response.data.limit)
      paginationService.setTotal(response.data.total)
      $scope.isLoading = false;
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });

  setTimeout(function () {
    $('.slick').slick({
      // autoplay: true,
      autoplaySpeed: 2000,
      dots: true,
      slidesToShow: 1,
      slidesToScroll: 1,
      prevArrow: '<button class="slide-arrow prev-arrow"></button>',
      nextArrow: '<button class="slide-arrow next-arrow"></button>',
    })
  }, 1000)
}

