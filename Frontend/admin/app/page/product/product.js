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

  .controller('ProductListCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Product List';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    loadProduct($http, $scope);
  }])

  .controller('ProductCreateCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Create Product';
    
    $scope.limit = 10;
    $scope.page = 1;
    $scope.total = 0;
    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.productName = "";
    $scope.store = "";
    $scope.stock = "";
    $scope.price = "";


    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createProduct = function () {
      createProduct($http, $scope, {
        name: $scope.productName,
        storeId: $scope.store,
        available: $scope.stock,
        price: $scope.price
      });
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

function createProduct($http, $scope, request) {
  $scope.isLoading = true;
  $http.post('/api/products', request)
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
      $scope.fileData = event.target.result;
      images.prepend('<div class="img" style="background-image: url(\'' + event.target.result + '\');" rel="' + event.target.result + '"><span>remove</span></div>')
    }
    reader.readAsDataURL(uploader[0].files[0])
  })

  images.on('click', '.img', function () {
    $(this).remove()
  })
};

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