angular.module('myApp.dine', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider.when('/dine', {
        templateUrl: 'pages/dine/dine.html',
        controller: 'DineCtrl',
      })
    },
  ])
  .controller('DineCtrl', ['$scope', '$http', '$rootScope', '$location', 'paginationService',
    function ($scope, $http, $rootScope, $location, paginationService) {
      document.title = 'Home';

      const { page, limit } = $location.search();

      $scope.limit = Number(limit || 10);
      $scope.page = Number(page || 1);
      $scope.total = 0;

      $scope.stores = undefined;
      $scope.error = undefined;
      $scope.isLoading = false;
      $scope.loadFilter = false;

      $scope.isLeft = function (index) {
        return index % 4 === 0 || index % 4 === 1;
      };

      loadStore($http, $scope, $location, paginationService)
        .then(() => {
          $scope.loadFilter = true;
        })

      $scope.handlePageClick = function () {
        console.log('Button clicked!');
      };
    }]);


function loadStore($http, $scope, $location, paginationService) {
  $scope.isLoading = true;

  var params = $location.search();
  const { query, floorId, categoryId, facilityIds } = params;
  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  query && params.append('query', query);
  floorId && params.append('floorId', floorId);
  categoryId && params.append('categoryId', categoryId);
  facilityIds && params.append('facilityIds', facilityIds);

  return $http.get(`/api/stores${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      $scope.stores = response.data.data;
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
};
