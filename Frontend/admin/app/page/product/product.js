angular.module('myApp.product', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/products', {
        templateUrl: 'page/product/product.html',
        controller: 'ProductListCtrl'
      })
      .when('/products/create', {
        templateUrl: 'page/product/product-create.html',
        controller: 'ProductCreateCtrl'
      })
      .when('/products/:id/edit', {
        templateUrl: 'page/product/product-edit.html',
        controller: 'ProductEditCtrl'
      });
  }])

  .controller('ProductListCtrl', ['$scope', '$location', '$http', 'paginationService',
    function ($scope, $location, $http, paginationService) {
      document.title = 'Product List';

      const { query, page, limit } = $location.search();
      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.products = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.deleteModal = false;

      loadProduct($http, $scope, paginationService);


      $scope.handleDeleteProduct = () => {
        const ids = $scope.selectProduct.id;
        deleteProduct($http, $scope, ids)
          .then(() => {
            $scope.deleteModal = false;
            $location.path('/products').replace();
            loadProduct($http, $scope, paginationService);
          })
      }

      $scope.showDeleteConfirm = (product) => {
        $scope.deleteModal = true;
        $scope.selectProduct = product;
      }
      $scope.closeDeleteConfirm = () => $scope.deleteModal = false;
    }])

  .controller('ProductCreateCtrl', ['$scope', '$http', 'paginationService', function ($scope, $http, paginationService) {
    document.title = 'Create Product';

    $scope.limit = 10;
    $scope.page = 1;
    $scope.total = 0;
    $scope.products = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.name = "";
    $scope.store = "";
    $scope.stock = "";
    $scope.price = "";

    loadCategory($http, $scope, paginationService)
      .then(() => {
        loadStore($http, $scope, paginationService);
      });

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createProduct = function () {
      const formData = new FormData();
      if ($scope.name) formData.append("name", $scope.name)
      if ($scope.code) formData.append("code", $scope.code)
      if ($scope.fileData) formData.append("formFile", $scope.fileData)
      if ($scope.description) formData.append("description", $scope.description)
      if ($scope.brand) formData.append("brand", $scope.brand)
      if ($scope.categories) formData.append("categories", JSON.stringify($scope.categories))
      if ($scope.categories) formData.append("categoriesIds", JSON.stringify([$scope.categoryId]))
      if ($scope.variant) formData.append("variant", $scope.variant)
      if ($scope.price) formData.append("price", $scope.price)
      if ($scope.stock) formData.append("inStock", $scope.stock)
      if ($scope.storeId) formData.append("storeId", $scope.storeId)
      createProduct($http, $scope, formData);
    };
  }])
  .controller('ProductEditCtrl', ['$scope', '$http', '$filter', '$routeParams', '$location', 'paginationService',
    function ($scope, $http, $filter, $routeParams, $location, paginationService) {
      document.title = 'Edit Product';

      $scope.product = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.productName = "";
      $scope.fileData = "";
      $scope.fileName = "";
      $scope.productId = $routeParams.id;

      $scope.formattedDate = formattedDate;
      $scope.timeDifference = timeDifference;

      loadCategory($http, $scope)
        .then(() => {
          return getProductById($http, $scope);
        })
        .then(() => {
          $scope.productStatus = $scope?.product?.status || 'active'
          $scope.productName = $scope?.product?.name
          $scope.categoryId = $scope.categories.find((s) => s.id === $scope.product.category.id)
          $scope.floorId = $scope.floors.find((s) => s.id === $scope.product.floor.id)
          $scope.facilityId = $scope.facilities.find((s) => s.id == $scope.product.facilities)
          $scope.price = $scope?.product.variants.price
          $scope.stock = $scope?.product.variants.inStock
        });
      $scope.uploadImage = function () {
        uploadImage($scope);
      };

      $scope.toggleStatus = () => {
        $scope.productStatus = $scope.productStatus === "active" ? "inactive" : "active"
      };

      $scope.updateProduct = function () {
        const formData = new FormData();
        if ($scope.product.name) formData.append("name", $scope.product.name)
        if ($scope.product.code) formData.append("code", $scope.product.code)
        if ($scope.product.brand) formData.append("brand", $scope.product.brand)
        if ($scope.categoryId) formData.append("categoryId", typeof $scope.categoryId === "number" ? $scope.categoryId : $scope.categoryId.id)
        if ($scope.storeId) formData.append("storeId", typeof $scope.storeId === "number" ? $scope.storeId : $scope.storeId.id)
        if ($scope.price) formData.append("price", $scope.price)
        if ($scope.stock) formData.append("inStock", $scope.stock)
        if ($scope.product.description) formData.append("description", $scope.product.description)
        if ($scope.productStatus) formData.append("status", $scope.productStatus)
        if ($scope.fileData) formData.append("formFile", $scope.fileData)
        if ($scope.image) formData.append("image", $scope.product.image)
        updateProduct($http, $scope, formData)
          .finally(() => {
            getProductById($http, $scope);
          });
      }
    }]);


function loadProduct($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  $http.get(`/api/products${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      console.log(response);
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
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function createProduct($http, $scope, formData) {
  $scope.isLoading = true;
  $http.post('/api/products', formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function (response) {
      showSuccessToast("Create product success!");
    })
    .catch(function (error) {
      showErrorToast(getErrorsMessage(error));
      $scope.error = error.data ? error.data : error
    });
}

function getProductById($http, $scope) {
  $scope.isLoading = true;

  return $http.get(`/api/products/${$scope.productId}`)
    .then(function (response) {
      $scope.product = response.data;
      $scope.isLoading = false;
    })
    .then(() => {
      renderImage($scope.product.image);
      $scope.productId = $scope.product.find((s) => s.id === $scope.productId)
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function updateProduct($http, $scope, formData) {
  $scope.isLoading = true;

  return $http.put(`/api/products/${$scope.productId}`, formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function (response) {
      showSuccessToast("Update product success!");
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
    images.innerHtml = ''
    var file = uploader[0].files[0];
    $scope.fileName = file.name;
    var reader = new FileReader()
    reader.onload = function (event) {
      $scope.$apply(function () {
        $scope.fileData = file;
        $scope.images = [event.target.result];
      });
      images.prepend('<div class="img" style="background-image: url(\'' + event.target.result + '\');" rel="' + event.target.result + '"><span>remove</span></div>')
    }
    reader.readAsDataURL(uploader[0].files[0])
  })

  images.on('click', '.img', function () {
    $(this).remove()
  })
};

function loadCategory($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);
  params.append('type', 'product');

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

function deleteProduct($http, $scope, ids) {
  $scope.isLoading = true;

  return $http.delete(`/api/products?ids=${ids}`)
    .then(function (response) {
      showSuccessToast("Delete product success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast(getErrorsMessage(error));
    });
}