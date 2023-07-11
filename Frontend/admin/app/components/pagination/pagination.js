export default angular.module('myApp')
  .component('pagination', {
    templateUrl: 'components/pagination/pagination.html',
    controller: ['$scope', '$location', 'paginationService', function PaginationController($scope, $location, paginationService) {
      $scope.total = paginationService.getTotal();
      $scope.page = paginationService.getPage();
      $scope.limit = paginationService.getLimit();

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
        paginationService.setLimit(limit);
        $location.search('limit', limit);
      };
    }]
  });