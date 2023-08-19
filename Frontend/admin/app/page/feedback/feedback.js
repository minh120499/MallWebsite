angular.module('myApp.feedback', ['ngRoute'])

  .config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/feedbacks', {
        templateUrl: 'page/feedback/feedback.html',
        controller: 'FeedbackListCtrl'
      })
  }])

  .controller('FeedbackListCtrl', ['$scope', '$http', 'paginationService', function ($scope, $http, paginationService) {
    document.title = 'Feedback List';

    $scope.data = undefined;
    $scope.error = undefined;
    $scope.isLoading = false;
    $scope.limit = 10;
    $scope.page = 1;
    $scope.total = 0;

    $scope.handleUpdateFeedback = (feedback) => {
      $scope.selectFeedback = feedback;
      updateFeedback($http, $scope);
    }

    $scope.showDeleteConfirm = (feedback) => {
      console.log(feedback);
      $scope.deleteModal = true;
      $scope.selectFeedback = feedback;
    }
    $scope.handleDeleteFeedback = () => {
      const ids = $scope.selectFeedback.id;
      $scope.deleteModal = false;
      deleteFeedback($http, $scope, ids)
        .then(() => {
          $location.path('/feedbacks').replace();
          loadFeedback($http, $scope, paginationService);
        })
    }
    $scope.closeDeleteConfirm = () => $scope.deleteModal = false;

    setTimeout(() => {
      loadFeedback($http, $scope);
    }, 300);
  }])

function loadFeedback($http, $scope, paginationService) {
  $scope.isLoading = true;
  return $http.get('/api/feedbacks')
    .then(function (response) {
      console.log(response);
      $scope.data = response.data.data;
      $scope.limit = response.data.limit;
      $scope.page = response.data.page;
      $scope.total = response.data.total;
      $scope.isLoading = false;
    })
    .catch(function (error) {
      console.log('Error fetching data:', error);
      $scope.error = error;
      $scope.isLoading = false;
    });
}

function updateFeedback($http, $scope) {
  return $http.put(`/api/feedbacks/${$scope.selectFeedback.id}`)
    .then(function (response) {
      showSuccessToast("Update feedbacks success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast(getErrorsMessage(error));
    });
}


function deleteFeedback($http, $scope, ids) {
  $scope.isLoading = true;

  return $http.delete(`/api/feedbacks?ids=${ids}`)
    .then(function (response) {
      showSuccessToast("Delete feedbacks success!");
    })
    .catch(function (error) {
      $scope.error = error.data ? error.data : error
      showErrorToast(getErrorsMessage(error));
    });
}