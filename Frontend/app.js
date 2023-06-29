import headerComponent from "./components/header/header.component.js"

angular.module('MallApp', []).controller('MallCtrl', function MallCtrl() {
  this.hero = {
    name: 'Spawn'
  };
});

angular.module('MallApp').component('app', {
  templateUrl: 'app.component.html',
});

headerComponent();