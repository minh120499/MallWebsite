angular.module('myApp.info', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider.when('/info', {
        templateUrl: 'pages/info/info.html',
        controller: 'InfoCtrl',
      })
    },
  ])
  .controller('InfoCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
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

      $scope.sendFeedBack = function () {
        if (!$scope.name) {
          showErrorToast("Name is not valid")
          return;
        }

        if (!$scope.email) {
          showErrorToast("Email is required")
          return;
        } else {
          const emailPattern = /^[\w\.-]+@[\w\.-]+\.\w+$/;
          if (!emailPattern.test($scope.email)) {
            showErrorToast("Email is not valid")
            return;
          }
        }

        if (!$scope.message) {
          showErrorToast("Message is not valid")
          return;
        }

        const formData = new FormData();
        if ($scope.name) formData.append("name", $scope.name)
        if ($scope.email) formData.append("email", $scope.email)
        if ($scope.message) formData.append("message", $scope.message)

        sendFeedBack($http, $scope, formData);
      }
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

function sendFeedBack($http, $scope, formData) {
  return $http.post(`/api/feedbacks`, formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function (response) {
      $scope.banners = response.data.data;
      showSuccessToast("Thank for your feedback!");
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
      showErrorToast(getErrorsMessage(error) || "An error occurred");
    });
}

function getErrorsMessage(error) {
  if (error?.data?.error) {
    return error?.data?.error;
  }

  if (!error?.data?.errors[0]) {
    return "";
  }
  return Object.values(error.data.errors[0]);
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
