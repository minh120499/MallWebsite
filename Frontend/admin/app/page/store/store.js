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

  .controller('StoreListCtrl', ['$scope', '$http', '$location', 'BE_URL', 'paginationService',
    function ($scope, $http, $location, BE_URL, paginationService) {
      document.title = 'Store List';

      const { query, page, limit } = $location.search();
      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.BE_URL = BE_URL;
      $scope.data = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      loadStore($http, $scope, paginationService);
    }])

  .controller('StoreCreateCtrl', ['$scope', '$http', 'paginationService', function ($scope, $http, paginationService) {
    document.title = 'Create Store';

    $scope.limit = 10;
    $scope.page = 1;
    $scope.total = 0;
    $scope.error = undefined;
    $scope.facilities = undefined;
    $scope.floors = undefined;
    $scope.categories = undefined;
    $scope.isLoading = false;
    $scope.name = "";
    $scope.location = "";

    loadCategory($http, $scope, paginationService)
      .then(() => {
        loadSetting($http, $scope);
      });

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createStore = function () {
      const formData = new FormData();
      if ($scope.name) formData.append("name", $scope.name)
      if ($scope.floorId) formData.append("floorId", $scope.floorId)
      if ($scope.categoryId) formData.append("categoryId", $scope.categoryId)
      if ($scope.bannersIds) formData.append("bannersIds", $scope.bannersIds)
      if ($scope.facilityIds) formData.append("facilityIds", $scope.facilityIds)
      if ($scope.description) formData.append("description", $scope.description)
      if ($scope.fileData) formData.append("formFile", $scope.fileData)
      if ($scope.phone) formData.append("phone", $scope.phone)
      if ($scope.email) formData.append("email", $scope.email)
      createStore($http, $scope, formData);
    };
  }]);

function loadStore($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  $http.get(`/api/stores${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      $scope.data = response.data.data;
      $scope.limit = response.data.limit;
      $scope.page = response.data.page;
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
}

function createStore($http, $scope, formData) {
  $scope.isLoading = true;
  console.log($scope);
  $http.post('/api/stores', formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
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
      $scope.fileData = file;
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

  return $http.get(`/api/settings`)
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

function loadCategory($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  return $http.get(`/api/categories${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      console.log("Category", response);
      $scope.categories = response.data.data;
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
}