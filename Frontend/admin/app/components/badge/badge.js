export default angular.module('myApp').component('badge', {
  templateUrl: 'components/badge/badge.html',
  transclude: true,
  bindings: {
    type: '<',
    content: '<'
  },
});