export default angular.module('myApp')
  .component('pagination', {
    templateUrl: 'components/pagination/pagination.html',
    controller: ['$scope', '$location', 'paginationService', function PaginationController($scope, $location, paginationService) {
      $scope.total = paginationService.getTotal() || 0;
      $scope.page = Number($location.search().page) || paginationService.getPage() || 1;
      $scope.limit =Number($location.search().limit) || paginationService.getLimit() || 10;
      $scope.paginationMax = $scope.page === Math.ceil($scope.total / ($scope.limit || 10))

      $scope.pagination = Array.from({ length: Math.ceil($scope.total / ($scope.limit || 10)) }, (_, index) => ({
        page: index + 1,
        isActive: $scope.page === index + 1,
      }));
      $scope.paginationLength = $scope.pagination.length;

      $scope.handlePrev = () => {
        $scope.page = $scope.page - 1;
        paginationService.setPage($scope.page);
        $location.search('page', $scope.page);
        window.scrollTo(0, 630)
      }

      $scope.handleNext = () => {
        $scope.page = $scope.page + 1;
        paginationService.setPage($scope.page);
        $location.search('page', $scope.page);
        window.scrollTo(0, 630)
      }

      $scope.handlePageClick = function (page) {
        paginationService.setPage(page);
        $location.search('page', page);
        window.scrollTo(0, 630)
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
        window.scrollTo(0, 630)
      };
    }]
  });