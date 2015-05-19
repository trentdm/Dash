app.service('experienceService', ['$http', function ($http) {
    this.get = function (successCallback, errorCallback) {
        $http.get('/api/experience/summary')
            .success(function (data, status, headers, config) {
                if (status == 200) {
                    successCallback(data.results);
                } else
                    errorCallback(data, status);
            })
            .error(function(data, status, headers, config) {
                errorCallback(data, status);
            });
    };

    this.getById = function (id, successCallback, errorCallback) {
        $http.get('/api/experience/summary' + id)
            .success(function(data, status, headers, config) {
                successCallback(data.results);
            })
            .error(function(data, status, headers, config) {
                errorCallback(data, status);
            });
    };
}]);