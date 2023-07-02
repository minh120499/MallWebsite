angular.module('myApp.banner', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/banners', {
        templateUrl: 'page/banner/banner.html',
        controller: 'BannerListCtrl'
      })
      .when('/banners/create', {
        templateUrl: 'page/banner/banner-create.html',
        controller: 'BannerCreateCtrl'
      });
  }])

  .controller('BannerListCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Banner List';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    loadBanner($http, $scope);
  }])

  .controller('BannerCreateCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Create Banner';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    createBanner($http, $scope, { id: 1 });
  }]);


function loadBanner($http, $scope) {
  $scope.isLoading = true;
  $http.get('/api/banners')
    .then(function (response) {
      console.log(response);
      $scope.data = response.data;
      $scope.isLoading = false;
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function createBanner($http, $scope, request) {
  $scope.isLoading = true;
  $http.post('/api/banners', request)
    .then(function (response) {
      console.log('Banner created successfully:', response.data);
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
    });
}