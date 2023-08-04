export default angular.module('myApp')
  .component('filterBox', {
    templateUrl: 'components/filter-box/filter-box.html',
    controller: ['$scope', '$location', 'paginationService', function PaginationController($scope, $location, paginationService) {
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
    }]
  });