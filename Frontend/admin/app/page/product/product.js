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
      });
  }])

  .controller('ProductListCtrl', ['$scope', '$http', 'BE_URL', function ($scope, $http, BE_URL) {
    document.title = 'Product List';

    $scope.BE_URL = BE_URL;
    $scope.products = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    loadProduct($http, $scope);
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
      if ($scope.categories) formData.append("categories", $scope.categories)
      if ($scope.variant) formData.append("variant", $scope.variant)
      if ($scope.price) formData.append("price", $scope.price)
      if ($scope.stock) formData.append("stock", $scope.stock)
      if ($scope.storeId) formData.append("storeId", $scope.storeId)
      createProduct($http, $scope, formData);
    };
  }]);


function loadProduct($http, $scope) {
  $scope.isLoading = true;
  $http.get('/api/products')
    .then(function (response) {
      console.log(response);
      $scope.data = response.data.data;
      $scope.limit = response.data.limit;
      $scope.page = response.data.page;
      $scope.total = response.data.total;
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
      console.log('Product created successfully:', response.data);
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
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

function loadStore($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  return $http.get('/api/stores')
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

// function resetButton() {
//   var resetbtn = $('#reset')
//   resetbtn.on('click', function () {
//     reset()
//   })
// }

// function reset() {

//   $('#title').val('')
//   $('.select-option .head').html('Category')
//   $('select#category').val('')

//   var images = $('.images .img')
//   for (var i = 0; i < images.length; i++) {
//     $(images)[i].remove()
//   }

//   var topic = $('#topic').val('')
//   var message = $('#msg').val('')
// }