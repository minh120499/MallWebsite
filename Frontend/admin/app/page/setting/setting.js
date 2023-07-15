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
      $scope.lodash = _;
      $scope.initFacilities = [];
      $scope.initFloors = [];
      $scope.facilities = [];
      $scope.floors = [];
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.isChangeFacilities = false;
      $scope.isChangeFloors = false;
      $scope.editIndex = -1;
      $scope.editContext = "";
      $scope.editItem = {};

      loadSetting($http, $scope);

      $scope.changeFacility = (e, idx) => {
        $scope.facilities = $scope.facilities.map((f, i) => {
          if (i !== idx) return f;
          if (e.name !== undefined) f.name = e.name;
          if (e.description !== undefined) f.description = e.description;
          return f;
        });
      }

      $scope.changeFloors = (e, idx) => {
        $scope.floors = $scope.floors.map((f, i) => {
          if (i !== idx) return f;
          if (e.name !== undefined) f.name = e.name;
          if (e.description !== undefined) f.description = e.description;
          return f;
        });
      }

      $scope.handleEdit = (index, context) => {
        if (context === "facilities") {
          $scope.facilities[index] = $scope.editItem;
        }
        if (context === "floors") {
          $scope.floors[index] = $scope.editItem;
        }
        editSetting($http, $scope, context);
      }

      $scope.edit = function (index, context) {
        $scope.editContext = context
        $scope.editIndex = index;
        if (context === "facilities") {
          $scope.editItem = $scope.facilities[index]
        }
        if (context === "floors") {
          $scope.editItem = $scope.floors[index]
        }
      }

      $scope.remove = function (index, type) {
        if (type === "facilities") {
          $scope.facilities = $scope.facilities.filter((_, i) => i !== index);
        }
        if (type === "floors") {
          $scope.floors = $scope.floors.filter((_, i) => i !== index);
        }
        checkChange($scope);
      }

      $scope.toggleStatus = () => {
        $scope.editItem = {
          ...$scope.editItem,
          status: $scope?.editItem?.status === "active" ? "inactive" : "active"
        }
      }

      $scope.delete = function (id, type) {
        deleteSetting($http, $scope, id, type);
      }

      $scope.saveSetting = function () {
        saveSetting($http, $scope);
      };

      $scope.addFacilityLine = function () {
        addFacilityLine($scope);
        checkChange($scope);
      };

      $scope.addFloorLine = function () {
        addFloorLine($scope);
        checkChange($scope);
      };
    }]);

function loadSetting($http, $scope) {
  $scope.isLoading = true;

  $http.get(`/api/settings`)
    .then(function (response) {
      $scope.facilities = response.data.facilities || [];
      $scope.floors = response.data.floors || [];
      $scope.initFacilities = response.data.facilities;
      $scope.initFloors = response.data.floors;
      $scope.isLoading = false;
    })
    .catch(function (error) {
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
      $scope.facilities = response.data.facilities || [];
      $scope.floors = response.data.floors || [];
      $scope.initFacilities = response.data.facilities;
      $scope.initFloors = response.data.floors;
      showSuccessToast("Update success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error;
      showErrorToast("An error occurred!");
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

function checkChange($scope) {
  $scope.isChangeFacilities = !$scope.lodash.isEqual($scope.facilities, $scope.initFacilities)
  $scope.isChangeFloors = !$scope.lodash.isEqual($scope.floors, $scope.initFloors)
};

function deleteSetting($http, $scope, ids, type) {
  $scope.isLoading = true;
  if (type === "floors") {
    $http.delete(`/api/settings/floors?ids=${ids}`)
      .then(function () {
        showSuccessToast("Delete success!");
        loadSetting($http, $scope);
      })
      .catch(function (error) {
        $scope.error = error;
        $scope.isLoading = false;
        showErrorToast(getErrorsMessage(error));
      });
  } else {
    $http.delete(`/api/settings/facilities?ids=${ids}`)
      .then(function () {
        loadSetting($http, $scope);
        showSuccessToast("Delete success!");
      })
      .catch(function (error) {
        $scope.error = error;
        $scope.isLoading = false;
        showErrorToast(getErrorsMessage(error));
      });
  }
}

function editSetting($http, $scope) {

  if ($scope.editContext === "floors") {
    $http.put(`/api/settings/floors/${$scope.editItem.id}`, $scope.editItem)
      .then(function () {
        loadSetting($http, $scope);
        $scope.editIndex = -1;
        $scope.editContext = "";
        $scope.editItem = {};
        showSuccessToast("Update success!");
      })
      .catch(function (error) {
        $scope.error = error;
        $scope.isLoading = false;
        showErrorToast(getErrorsMessage(error));
      });
  } else {
    $http.put(`/api/settings/facilities/${$scope.editItem.id}`, $scope.editItem)
      .then(function () {
        loadSetting($http, $scope);
        $scope.editIndex = -1;
        $scope.editContext = "";
        $scope.editItem = {};
        showSuccessToast("Update success!");
      })
      .catch(function (error) {
        showErrorToast(getErrorsMessage(error));
        $scope.error = error;
        $scope.isLoading = false;
      });
  }
}