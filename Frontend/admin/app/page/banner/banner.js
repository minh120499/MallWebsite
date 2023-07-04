angular.module('myApp.banner', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/banners', {
        templateUrl: 'page/banner/banner.html',
        controller: 'BannerListCtrl'
      })
      .when('/banners/create', {
        templateUrl: 'page/banner/banner-create.html',
        controller: 'BannerCreateCtrl'
      });
  }])

  .controller('BannerListCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Banner List';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    loadBanner($http, $scope);
  }])

  .controller('BannerCreateCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Create Banner';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.bannerName = "";
    $scope.fileData = "";
    $scope.fileName = "";

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createBanner = function () {
      createBanner($http, $scope, { name: $scope.bannerName, image: $scope.fileData });
    };
  }]);


function loadBanner($http, $scope) {
  $scope.isLoading = true;
  $http.get('/api/banners')
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

function createBanner($http, $scope, request) {
  $scope.isLoading = true;
  $http.post('/api/banners', request)
    .then(function (response) {
      console.log('Banner created successfully:', response.data);
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