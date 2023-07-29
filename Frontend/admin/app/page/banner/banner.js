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

  .controller('BannerListCtrl', ['$scope', '$http', '$rootScope', '$location', 'BE_URL', 'paginationService',
    function ($scope, $http, $rootScope, $location, BE_URL, paginationService) {
      document.title = 'Banner List';

      const { query, page, limit } = $location.search();

      $scope.BE_URL = BE_URL;
      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;
      $scope.query = query || "";

      $scope.banners = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;

      loadBanner($http, $scope, paginationService);

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };
    }])

  .controller('BannerCreateCtrl', ['$scope', '$http', '$filter', 'paginationService', function ($scope, $http, $filter, paginationService) {
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
      const formData = new FormData();
      if ($scope.bannerName) formData.append("name", $scope.bannerName)
      if ($scope.storeId) formData.append("storeId", $scope.storeId)
      if ($scope.fileData) formData.append("formFile", $scope.fileData)
      if ($scope.bannerStart) formData.append("startOn", $filter('date')($scope.bannerStart, 'yyyy-MM-ddTHH:mm:ss'))
      if ($scope.bannerEnd) formData.append("endOn", $scope.bannerEnd)
      createBanner($http, $scope, formData);
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

function createBanner($http, $scope, formData) {
  $scope.isLoading = true;

  $http.post('/api/banners', formData, {
    headers: { 'Content-Type': undefined },
    transformRequest: angular.identity
  })
    .then(function (response) {
      showSuccessToast("Create banner success!");
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

function loadStore($http, $scope, paginationService) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);

  $http.get(`/api/stores${params.size ? "?" + params.toString() : ""}`)
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