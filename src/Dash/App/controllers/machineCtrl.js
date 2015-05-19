app.controller('MachineCtrl', ['$scope', '$timeout', 'machineService', 'chartService', function ($scope, $timeout, machineService, chartService) {
    var successCallback = function (data) {
        $scope.machines = data;
        $scope.selectMachine(0);
    }

    var errorCallback = function (data) {
        if (data.responseMessage)
            $scope.$emit('alert', { type: 'danger', msg: data.responseStatus.message });
        else
            $scope.$emit('alert', { type: 'warning', msg: 'Unable to receive data.' });
    };

    $scope.isActiveMachine = function(machine) {
        return machine == $scope.currentMachine;
    }

    $scope.selectMachine = function (index) {
        $scope.currentMachine = $scope.machines[index];
        var chartData = [];
        chartData.push($scope.currentMachine);

        var chartData2 = [];
        chartData2.push($scope.machines[2]);
        chartService.createMachineSummaryChart(chartData, chartData2);
    }

    machineService.get(successCallback, errorCallback);

    (function poll() {
        $scope.promise = $timeout(function () {
            machineService.get(function (data) {
                var prevIndex = $scope.machines.indexOf($scope.currentMachine);
                $scope.machines = data;
                $scope.selectMachine(prevIndex > -1 ? prevIndex : 0);
            }, errorCallback);
            poll();
        }, 60000);
    })();

    $scope.$on('$locationChangeStart', function () {
        $timeout.cancel($scope.promise);
    });
}]);