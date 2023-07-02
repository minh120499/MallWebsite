angular.module('myApp.banner', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/banners', {
      templateUrl: 'page/banner/banner.html',
      controller: 'BannerPageCtrl'
    });
  }])

  .controller('BannerPageCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.data = 'Welcome to our website!';
    $scope.oldData = 'Hello world';
    $scope.newData = 'Welcome to our website!';
    $scope.toggle = true;

    $http.get('/api/banners')
      .then(function (response) {
        console.log(response);
        $scope.data = response.data;
      })
      .catch(function (error) {
        console.log('Error fetching data:', error);
      });

    $scope.changeText = function () {
      console.log(12);
      $scope.toggle = !$scope.toggle;
      $scope.data = $scope.toggle ? $scope.oldData : $scope.newData;
    };
  }])

  // .controller('CustomeTable', ['$scope', function ($scope) {
  //   $scope.$watch('data', function (newValue) {
  //     // Logic khi giá trị của data thay đổi
  //     console.log(newValue); // In ra giá trị mới của data
  //   });
  // }]);