const headerComponent = () => {

  class HeaderComponent {
    user = "";

    constructor() {
      this.user = 'world';
    }
  }

  return angular.module('MallApp').component('mallHeader', {
    templateUrl: 'components/header/header.component.html',
    bindings: {
      hero: '='
    },
    controller: HeaderComponent
  });
}

export default headerComponent;