export default angular.module('myApp').component('customeTable', {
  templateUrl: 'components/custome-table/custome-table.html',
  bindings: {
    data: '='
  },
  controller: ['$scope', function CustomeTableController($scope) {
    // $scope.sidebarVisible = true;

    // $scope.toggleSidebar = function () {
    //   $scope.sidebarVisible = !$scope.sidebarVisible;
    // };
  }]
});