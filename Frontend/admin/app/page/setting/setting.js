angular.module('myApp.setting', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/setting', {
        templateUrl: 'page/setting/setting.html',
        controller: 'MallCtrl'
      })
  }])

  .controller('MallCtrl', ['$scope', '$http', '$rootScope',
    function ($scope, $http, $rootScope) {
      document.title = 'Settings';

      $scope.facilities = [];
      $scope.floors = [];
      $scope.error = undefined;
      $scope.isLoading = false;

      loadSetting($http, $scope);

      $scope.changeFacility = (e, idx) => {
        $scope.facilities = $scope.facilities.map((f, i) => {
          if (i !== idx) return;
          return {
            ...f,
            name: e.name
          }
        });
      }

      $scope.edit = function (index, item, type) {
        if (type === "facilities") {
          $scope.facilities = $scope.facilities.map((f, i) => {
            if (i === index) {
              return item
            }
            return f;
          });
        }
        if (type === "floors") {
          $scope.floors = $scope.floors.map((f, i) => {
            if (i === index) {
              return item
            }
            return f;
          });
        }
      }

      $scope.remove = function (index, type) {
        if (type === "facilities") {
          $scope.facilities = $scope.facilities.filter((_, i) => i !== index);
          return;
        }
        if (type === "floors") {
          $scope.floors = $scope.floors.filter((_, i) => i !== index);
          return;
        }
      }

      $scope.saveSetting = function () {
        saveSetting($http, $scope);
      };

      $scope.addFacilityLine = function () {
        addFacilityLine($scope);
      };

      $scope.addFloorLine = function () {
        addFloorLine($scope);
      };
    }]);

function loadSetting($http, $scope) {
  $scope.isLoading = true;

  $http.get(`/api/settings`)
    .then(function (response) {
      console.log("Setting", response);
      $scope.facilities = response.data.facilities || [];
      $scope.floors = response.data.floors || [];
      $scope.isLoading = false;
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function saveSetting($http, $scope) {
  $scope.isLoading = true;
  const request = {
    facilities: $scope.facilities,
    floors: $scope.floors,
  }
  $http.post('/api/settings', request)
    .then(function (response) {
      console.log('Setting created successfully:', response.data);
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error;
      console.log(error);
      showErrorToast("có lỗi xảy ra")
    })
    .finally(function () {
      $scope.isLoading = false;
    });
}

function addFacilityLine($scope) {
  $scope.facilities = [...$scope.facilities, {
    name: undefined
  }];
};

function addFloorLine($scope) {
  $scope.floors = [...$scope.floors, {
    name: undefined,
    area: undefined,
    description: undefined,
  }];
};
