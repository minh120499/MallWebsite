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
      })
      .when('/stores/:id/edit', {
        templateUrl: 'page/store/store-edit.html',
        controller: 'StoreEditCtrl'
      });
  }])

  .controller('StoreListCtrl', ['$scope', '$http', '$location', 'paginationService',
    function ($scope, $http, $location, paginationService) {
      document.title = 'Store List';

      const { query, page, limit } = $location.search();
      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.stores = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      loadStore($http, $scope, paginationService);

      $scope.handleDeleteStore = () => {
        const ids = $scope.selectStore.id;
        deleteStore($http, $scope, ids)
          .then(() => {
            $scope.deleteModal = false;
            $location.path('/stores').replace();
            loadStore($http, $scope, paginationService);
          })
      }

      $scope.showDeleteConfirm = (store) => {
        $scope.deleteModal = true;
        $scope.selectStore = store;
      }
      $scope.closeDeleteConfirm = () => $scope.deleteModal = false;
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

    loadCategory($http, $scope)
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
      $scope.isLoading = true;
      createStore($http, $scope, formData)
        .finally(() => {
          $scope.isLoading = false;
        });
    };
  }])
  .controller('StoreEditCtrl', ['$scope', '$http', '$filter', '$routeParams', '$location', 'paginationService',
    function ($scope, $http, $filter, $routeParams, $location, paginationService) {
      document.title = 'Edit Store';

      $scope.store = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.storeName = "";
      $scope.fileData = "";
      $scope.fileName = "";
      $scope.storeId = $routeParams.id;

      $scope.formattedDate = formattedDate;
      $scope.timeDifference = timeDifference;

      loadSetting($http, $scope)
        .then(() => {
          return loadCategory($http, $scope);
        })
        .then(() => {
          return getStoreById($http, $scope);
        })
        .then(() => {
          $scope.storeStatus = $scope?.store?.status || 'active'
          $scope.storeName = $scope?.store?.name
          $scope.categoryId = $scope.categories.find((s) => s.id === $scope.store.category.id)
          $scope.floorId = $scope.floors.find((s) => s.id === $scope.store.floor.id)
          $scope.facilityId = $scope.facilities.find((s) => s.id == $scope.store.facilities)
        });
      $scope.uploadImage = function () {
        uploadImage($scope);
      };

      $scope.toggleStatus = () => {
        $scope.storeStatus = $scope.storeStatus === "active" ? "inactive" : "active"
      };

      $scope.updateStore = function () {
        const formData = new FormData();
        if ($scope.store.name) formData.append("name", $scope.store.name)
        if ($scope.floorId) formData.append("floorId", typeof $scope.floorId === "number" ? $scope.floorId : $scope.floorId.id)
        if ($scope.categoryId) formData.append("categoryId", typeof $scope.categoryId === "number" ? $scope.categoryId : $scope.categoryId.id)
        if ($scope.fileData) formData.append("formFile", $scope.fileData)
        if ($scope.facilityIds) formData.append("facilityIds", $scope.facilityIds)
        if ($scope.store.phone) formData.append("phone", $scope.store.phone)
        if ($scope.store.email) formData.append("email", $scope.store.email)
        if ($scope.store.description) formData.append("description", $scope.store.description)
        if ($scope.storeStatus) formData.append("status", $scope.storeStatus)
        if ($scope.image) formData.append("image", $scope.store.image)
        updateStore($http, $scope, formData)
          .finally(() => {
            getStoreById($http, $scope);
          });
      }
    }]);

function loadStore($http, $scope, paginationService) {
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

function updateStore($http, $scope, formData) {
  $scope.isLoading = true;

  return $http.put(`/api/stores/${$scope.storeId}`, formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function (response) {
      showSuccessToast("Update store success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast(getErrorsMessage(error));
    });
}

function createStore($http, $scope, formData) {
  return $http.post('/api/stores', formData, {
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

function loadCategory($http, $scope) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);
  params.append('type', 'store');

  return $http.get(`/api/categories${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      console.log("Category", response);
      $scope.categories = response.data.data;
      $scope.total = response.data.total;
      $scope.isLoading = false;

    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function deleteStore($http, $scope, ids) {
  $scope.isLoading = true;

  return $http.delete(`/api/stores?ids=${ids}`)
    .then(function (response) {
      showSuccessToast("Delete store success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast(getErrorsMessage(error));
    });
}