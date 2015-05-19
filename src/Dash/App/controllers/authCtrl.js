app.controller('AuthCtrl', ['$rootScope', '$scope', 'authService', 'preferencesService', function($rootScope, $scope, authService, preferencesService) {
    var authSuccess = function (data, status, headers, config) {
        if (status == 200) {
            authService.user.name = data.userName;
            authService.user.id = data.userId;
            authService.user.sessionId = data.sessionId;
            authService.user.isAuthenticated = true;
            preferencesService.get(data.userId, function (prefs) {
                 authService.user.preferences = prefs;
            });
            $scope.$close(data);
            $rootScope.$broadcast('alert', { type: 'success', msg: 'Welcome, ' + data.userName });
        } else {
            authError(data, status, headers, config);
        }
    };

    var authError = function (data, status, headers, config) {
        authService.user.name = undefined;
        authService.user.id = undefined;
        authService.user.sessionId = undefined;
        authService.user.isAuthenticated = false;
        authService.user.preferences = undefined;
        if (data.responseStatus)
            $rootScope.$broadcast('alert', { type: 'warning', msg: 'Woops! ' + data.responseStatus.message });
        else
            $rootScope.$broadcast('alert', { type: 'warning', msg: 'Server error, please try again later.' });
    };
    
    $scope.register = function (name, pass, email) {
        authService.register(name, pass, email, authSuccess, authError);
    };

    $scope.signin = function(name, pass) {
        authService.signin(name, pass, authSuccess, authError);
    };

    $scope.cancel = $scope.$dismiss;
}]);