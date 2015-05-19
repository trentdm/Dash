app.controller('HomeCtrl', ['$scope', '$timeout', 'experienceService', 'chartService', function ($scope, $timeout, experienceService, chartService) {
    var successCallback = function(data) {
        chartService.createExperienceSummaryChart(data);
    }

    var errorCallback = function (data) {
        if (data.responseMessage)
            $scope.$emit('alert', { type: 'danger', msg: data.responseStatus.message });
        else
            $scope.$emit('alert', { type: 'warning', msg: 'Unable to receive data.' });
    };

    experienceService.get(successCallback, errorCallback);

    (function poll() {
        $scope.promise = $timeout(function () {
            experienceService.get(successCallback, errorCallback);
            poll();
        }, 60000);
    })();

    $scope.$on('$locationChangeStart', function () {
        $timeout.cancel($scope.promise);
    });
}]);