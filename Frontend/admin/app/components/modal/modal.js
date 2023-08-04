export default angular.module('myApp').component('modal', {
  templateUrl: 'components/modal/modal.html',
  transclude: true,
  bindings: {
    type: '<',
    content: '<'
  },
});