app.service('machineService', ['$http', function ($http) {
    this.get = function (successCallback, errorCallback) {
        $http.get('/api/machine/summary')
            .success(function (data, status, headers, config) {
                if (status == 200) {
                    successCallback(data.results);
                } else
                    errorCallback(data, status);
            })
            .error(function (data, status, headers, config) {
                errorCallback(data, status);
            });
    };

    this.getById = function (id, successCallback, errorCallback) {
        $http.get('/api/machine/summary' + id)
            .success(function (data, status, headers, config) {
                successCallback(data.results);
            })
            .error(function (data, status, headers, config) {
                errorCallback(data, status);
            });
    };
}]);