angular.module('myApp.category', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/categories', {
        templateUrl: 'page/category/category.html',
        controller: 'CategoryListCtrl'
      })
      .when('/categories/create', {
        templateUrl: 'page/category/category-create.html',
        controller: 'CategoryCreateCtrl'
      });
  }])

  .controller('CategoryListCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
    function ($scope, $http, $rootScope, $location, paginationService) {
      document.title = 'Category List';

      const { query, page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.data = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.deleteModal = false;

      loadCategories($http, $scope, paginationService);

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };

      $scope.changeCategory = (e) => {
        if (e.name !== undefined) editItem.name = e.name;
        if (e.image !== undefined) editItem.image = e.image;
        if (e.type !== undefined) editItem.type = e.type;
        if (e.description !== undefined) editItem.description = e.description;
      };

      $scope.handleEdit = () => {
        editCategory($http, $scope, paginationService);
      };

      $scope.edit = function (index) {
        $scope.editIndex = index;
        $scope.editItem = $scope.data[index];
      };

      $scope.delete = function (id) {
        deleteCategory($http, $scope, id, paginationService);
      };

      $scope.toggleStatus = () => {
        $scope.editItem = {
          ...$scope.editItem,
          status: $scope?.editItem?.status === "active" ? "inactive" : "active"
        }
      }

      $scope.uploadImage = function () {
        uploadImage($scope);
      };

      $scope.handleDeleteCategory = () => {
        const id = $scope.selectCategory.id;
        deleteCategory($http, $scope, id, paginationService)
          .then(() => {
            $scope.deleteModal = false;
            $location.path('/categories').replace();
            loadCategories($http, $scope, paginationService);
          })
      }

      $scope.showDeleteConfirm = (category) => {
        $scope.deleteModal = true;
        $scope.selectCategory = category;
      }

      $scope.closeDeleteConfirm = () => $scope.deleteModal = false;
    }])

  .controller('CategoryCreateCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Create Category';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.name = "";
    $scope.fileData = "";
    $scope.fileName = "";

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createCategory = function () {
      const formData = new FormData();
      if ($scope.name) formData.append("name", $scope.name)
      if ($scope.type) formData.append("type", $scope.type)
      if ($scope.fileData) formData.append("formFile", $scope.fileData)
      createCategory($http, $scope, formData);
    };
  }]);


function loadCategories($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  $http.get(`/api/categories${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      console.log("Category", response);
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
}

function createCategory($http, $scope, formData) {
  $scope.isLoading = true;
  $http.post('/api/categories', formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function (response) {
      showSuccessToast("Create category success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast("An error occurred!");
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

function editCategory($http, $scope, paginationService) {
  const formData = new FormData();
  if ($scope.editItem?.name) formData.append("name", $scope.editItem.name)
  if ($scope.editItem?.type) formData.append("type", $scope.editItem.type)
  if ($scope.editItem?.image) formData.append("image", $scope.editItem.image)
  if ($scope.editItem?.fileData) formData.append("formFile", $scope.fileData)
  if ($scope.editItem?.id) formData.append("id", $scope.editItem.id)
  if ($scope.editItem?.status) formData.append("status", $scope.editItem.status)

  $http.put(`/api/categories/${$scope.editItem.id}`, formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function () {
      loadCategories($http, $scope, paginationService);
      $scope.editIndex = -1;
      $scope.editItem = {};
      showSuccessToast("Update success!");
    })
    .catch(function (error) {
      showErrorToast(getErrorsMessage(error));
      $scope.error = error;
      $scope.isLoading = false;
    });

}

function deleteCategory($http, $scope, ids, paginationService) {
  $scope.isLoading = true;
  return $http.delete(`/api/categories/?ids=${ids}`)
    .then(function () {
      showSuccessToast("Delete success!");
      loadCategories($http, $scope, paginationService);
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
      showErrorToast(getErrorsMessage(error));
    });
}