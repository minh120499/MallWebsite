angular.module('myApp.feedback', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/feedbacks', {
        templateUrl: 'page/feedback/feedback.html',
        controller: 'FeedbackListCtrl'
      })
  }])

  .controller('FeedbackListCtrl', ['$scope', '$http', function ($scope, $http) {
    document.title = 'Feedback List';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;

    loadFeedback($http, $scope);
  }])

function loadFeedback($http, $scope) {
  $scope.isLoading = true;
  $http.get('/api/feedbacks')
    .then(function (response) {
      console.log(response);
      $scope.data = response.data;
      $scope.isLoading = false;
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}