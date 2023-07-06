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

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.storeName = "";
    $scope.location = "";

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createStore = function () {
      createStore($http, $scope, { name: $scope.storeName, location: $scope.location });
    };
  }]);


function loadStore($http, $scope) {
  $scope.isLoading = true;
  $http.get('/api/stores')
    .then(function (response) {
      console.log(response);
      $scope.data = response.data;
      $scope.isLoading = false;
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function createStore($http, $scope, request) {
  $scope.isLoading = true;
  $http.post('/api/stores', request)
    .then(function (response) {
      console.log('Store created successfully:', response.data);
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