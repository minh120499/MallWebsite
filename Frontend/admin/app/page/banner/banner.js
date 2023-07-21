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

  .controller('BannerListCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
    function ($scope, $http, $rootScope, $location, paginationService) {
      document.title = 'Banner List';

      const { query, page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.data = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      loadBanner($http, $scope, paginationService);

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };
    }])

  .controller('BannerCreateCtrl', ['$scope', '$http', 'paginationService', function ($scope, $http, paginationService) {
    document.title = 'Create Banner';

    $scope.banners = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.bannerName = "";
    $scope.fileData = "";
    $scope.fileName = "";

    loadStore($http, $scope, paginationService);

    $scope.uploadImage = function () {
      uploadImage($scope);
    };

    $scope.createBanner = function () {
      createBanner($http, $scope, { name: $scope.bannerName, image: $scope.fileData });
    };
  }]);


function loadBanner($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  $http.get(`/api/banners${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      console.log("Banner", response);
      $scope.banners = response.data.data;
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

function loadStore($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  $http.get('/api/stores')
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