export default angular.module('myApp')
  .component('filterBox', {
    templateUrl: 'components/filter-box/filter-box.html',
    controller: ['$scope', '$location', '$http', 'paginationService',
      function PaginationController($scope, $location, $http, paginationService) {
        $scope.query = undefined;
        $scope.total = paginationService.getTotal() || 0;
        $scope.page = paginationService.getPage() || 1;
        $scope.limit = paginationService.getLimit() || 10;

        $scope.pagination = Array.from({ length: Math.ceil($scope.total / ($scope.limit || 10)) }, (_, index) => ({
          page: index + 1,
          isActive: $scope.page === index + 1,
        }));
        $scope.paginationLength = $scope.pagination.length;

        $scope.handlePageClick = function (page) {
          paginationService.setPage(page);
          $location.search('page', page);
        };

        $scope.handleLimitClick = function (limit) {
          if (limit * $scope.page > $scope.total) {
            paginationService.setLimit(limit);
            $location.search('limit', limit);
            paginationService.setPage(1);
            $location.search('page', 1);
          } else {
            paginationService.setLimit(limit);
            $location.search('limit', limit);
          }
        };


        $scope.handleSearch = function () {
          $location.search('query', $scope.query);
          $location.search('floorId', $scope.floorId);
          $location.search('categoryId', $scope.categoryId);
          $location.search('facilityIds', $scope.facilityIds);
        }

        $scope.handleKeyPress = function (event) {
          if (event.key === "Enter") {
            $scope.handleSearch();
          }
        };

        loadSetting($http, $scope)
          .then(() => {
            return loadCategory($http, $scope)
          })
          .then(() => {
            var params = $location.search();
            const { floorId, categoryId, facilityIds } = params;
            $scope.floorId = floorId;
            $scope.categoryId = categoryId;
            $scope.facilityIds = facilityIds;
          })
      }]
  });

function loadSetting($http, $scope) {
  $scope.isLoading = true;

  return $http.get(`/api/settings?status=active`)
    .then(function (response) {
      $scope.facilities = response.data.facilities || [];
      $scope.floors = response.data.floors || [];
      $scope.initFacilities = response.data.facilities;
      $scope.initFloors = response.data.floors;
      $scope.isLoading = false;
    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function loadCategory($http, $scope) {
  $scope.isLoading = true;

  var params = new URLSearchParams();
  $scope.page && params.append('page', $scope.page);
  $scope.limit && params.append('limit', $scope.limit);
  $scope.query && params.append('query', $scope.query);
  params.append('type', 'store');

  return $http.get(`/api/categories${params.size ? "?" + params.toString() : ""}`)
    .then(function (response) {
      $scope.categories = response.data.data;
      $scope.total = response.data.total;
      $scope.isLoading = false;

    })
    .catch(function (error) {
      $scope.error = error;
      $scope.isLoading = false;
    });
}