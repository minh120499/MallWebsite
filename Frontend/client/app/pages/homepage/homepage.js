// $(document).ready(function () {
//   $('.slick').slick({
//     // autoplay: true,
//     autoplaySpeed: 2000,
//     dots: true,
//     slidesToShow: 1,
//     slidesToScroll: 1,
//     prevArrow: '<button class="slide-arrow prev-arrow"></button>',
//     nextArrow: '<button class="slide-arrow next-arrow"></button>',
//   })
// })
angular
  .module('myApp.homepage', ['ngRoute'])
  .config([
    '$routeProvider',
    function ($routeProvider) {
      $routeProvider.when('/', {
        templateUrl: 'pages/homepage/homepage.html',
        controller: 'HomePageCtrl',
      })
    },
  ])
  .controller('HomePageCtrl', [
    function () {
      setTimeout(function () {
        $('.slick').slick({
          // autoplay: true,
          autoplaySpeed: 2000,
          dots: true,
          slidesToShow: 1,
          slidesToScroll: 1,
          prevArrow: '<button class="slide-arrow prev-arrow"></button>',
          nextArrow: '<button class="slide-arrow next-arrow"></button>',
        })
      }, 1000)
    },
  ])
