angular.module('myApp.store', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/stores', {
        templateUrl: 'page/store/store.html',
        controller: 'StoreListCtrl'
      })
      .when('/stores/create', {
        templateUrl: 'page/store/store-create.html',
        controller: 'StoreCreateCtrl'
      });
  }])

  .controller('StoreListCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Store List';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    loadStore($http, $scope);
  }])

  .controller('StoreCreateCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Create Store';
    
    $scope.limit = 10;
    $scope.page = 1;
    $scope.total = 0;
    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.name = "";
    $scope.location = "";

    loadSetting($http, $scope);

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createStore = function () {
      const request = {
        name: $scope.name,
        floorId: $scope.floorId,
        categoryId: $scope.categoryId,
        description: $scope.description,
        
      }
      // createStore($http, $scope, { name: $scope.id, location: $scope.location });
    };
  }]);


function loadStore($http, $scope) {
  $scope.isLoading = true;
  $http.get('/api/stores')
    .then(function (response) {
      $scope.data = response.data.data;
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

function createStore($http, $scope, request) {
  $scope.isLoading = true;
  $http.post('/api/stores', request)
    .then(function () {
      showSuccessToast("Create store success!");
      loadStore($http, $scope);
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast(getErrorsMessage(error));
    });
}

function uploadImage($scope) {
  var uploader = $('<input type="file" accept="image/*" />')
  var images = $('.images')

  uploader.click();

  uploader.on('change', function () {
    var file = uploader[0].files[0];
    $scope.fileName = file.name;
    var reader = new FileReader()
    reader.onload = function (event) {
      $scope.fileData = event.target.result;
      images.prepend('<div class="img" style="background-image: url(\'' + event.target.result + '\');" rel="' + event.target.result + '"><span>remove</span></div>')
    }
    reader.readAsDataURL(uploader[0].files[0])
  })

  images.on('click', '.img', function () {
    $(this).remove()
  })
};

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