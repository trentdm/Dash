app.service('versionService', ['$http', function ($http) {
    this.getVersionInfo = function(successCallback, errorCallback) {
        $http.get('/api/version')
            .success(function (data, status, headers, config) {
                var localVersion = 0.2;
                this.version = {
                    local: localVersion,
                    server: data.result.fullVersion,
                    isOutOfDate: localVersion < data.result.fullVersion
                };
                successCallback(version);
                return version;
            })
            .error(function(data, status, headers, config) {
                errorCallback(data, status);
            });
    }
}]);