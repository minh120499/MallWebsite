export default angular.module('myApp').component('customeTable', {
  templateUrl: 'components/table/table.html',
  bindings: {
    data: '='
  },
  controller: ['$scope', function TableController($scope) {
    // $scope.sidebarVisible = true;

    // $scope.toggleSidebar = function () {
    //   $scope.sidebarVisible = !$scope.sidebarVisible;
    // };
  }]
});