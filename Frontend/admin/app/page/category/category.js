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
      })
      .when('/categories/:id/edit', {
        templateUrl: 'page/category/category-edit.html',
        controller: 'CategoryEditCtrl'
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

      $scope.handleSearch = () => {
        loadCategories($http, $scope, paginationService);
      }

      $scope.handleKeyPress = function (event) {
        if (event.key === "Enter") {
          loadCategories($http, $scope, paginationService);
        }
      };

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
    $scope.type = "product"
    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createCategory = function () {
      const formData = new FormData();
      if ($scope.name) formData.append("name", $scope.name)
      if ($scope.type) formData.append("type", $scope.type)
      if ($scope.fileData) formData.append("formFile", $scope.fileData)
      $scope.isLoading = true;
      createCategory($http, $scope, formData)
        .finally(() => {
          $scope.isLoading = false;
        });
    };
  }])
  .controller('CategoryEditCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
    document.title = 'Edit Category';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.name = "";
    $scope.fileData = "";
    $scope.fileName = "";
    $scope.categoryId = $routeParams.id;


    getCategoriesById($http, $scope);

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.toggleStatus = () => {
      $scope.categoryStatus = $scope.categoryStatus === "active" ? "inactive" : "active"
    };

    $scope.updateCategory = function () {
      const formData = new FormData();
      if ($scope.categoryName) formData.append("name", $scope.categoryName)
      if ($scope.type) formData.append("type", $scope.type)
      if ($scope.type) formData.append("type", $scope.type)
      if ($scope.fileData) formData.append("formFile", $scope.fileData)
      if ($scope.categoryStatus) formData.append("status", $scope.categoryStatus)
      if ($scope.image) formData.append("image", $scope.image)
      $scope.isLoading = true;
      updateCategory($http, $scope, formData)
        .finally(() => {
          getCategoriesById($http, $scope);
          $scope.isLoading = false;
        });;
    };
  }])



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
  return $http.post('/api/categories', formData, {
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

function getCategoriesById($http, $scope) {
  $scope.isLoading = true;

  return $http.get(`/api/categories/${$scope.categoryId}`)
    .then(function (response) {
      const { name, type, image, status } = response.data;
      $scope.category = response.data;
      $scope.categoryName = name;
      $scope.type = type;
      $scope.image = image;
      $scope.categoryStatus = status;
      $scope.isLoading = false;
    })
    .then(() => {
      renderImage($scope.image);
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
    });
}

function updateCategory($http, $scope, formData) {
  return $http.put(`/api/categories/${$scope.categoryId}`, formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function () {
      // loadCategories($http, $scope, paginationService);
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