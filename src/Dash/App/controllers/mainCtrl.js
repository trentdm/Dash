app.controller('MainCtrl', ['$scope', '$state', '$timeout', 'authModal', 'authService', function($scope, $state, $timeout, authModal, authService) {
    $scope.alerts = [];

    $scope.addAlert = function(alert) {
        $scope.alerts.push(alert);
        $timeout(function() {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }, 8000);
    };

    $scope.$on('alert', function (event, alert) {
        $scope.addAlert(alert);
    });

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };

    $scope.isActive = function (tab) {
        var index = $state.current.name.indexOf(tab);
        return index > -1;
    };

    $scope.user = authService.user;

    $scope.signin = function() {
        authModal();
    };

    $scope.signout = function() {
        authService.signout();
    };
}]);