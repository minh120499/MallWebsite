export default angular.module('myApp')
  .component('appHeader', {
    templateUrl: 'components/header/header.html',
    controller: ['$scope', '$location', '$http', 'paginationService',
      function PaginationController($scope, $location, $http, paginationService) {
        $scope.page = $location.path().slice(1) || "home";

        $scope.setPage = function (page) {
          $scope.page = page
        }
      }]
  });