export default angular.module('myApp').component('badge', {
  templateUrl: 'components/badge/badge.html',
  bindings: {
    type: '<',
    content: '<'
  },
});