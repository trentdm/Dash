app.service('authService', ['$http', '$cookieStore', function ($http, $cookieStore) {
    var self = this;
    var cookie = $cookieStore.get('user');
    if (cookie) {
        this.user = cookie;
    } else {
        this.user = {
            name: undefined,
            id: undefined,
            isAuthenticated: false,
            preferences: {
                matchesPerPage: 5,
                teamsPerPage: 10,
                playersPerPage: 10
            }
        };
    }

    this.register = function(name, pass, email, successCallback, errorCallback) {
        $http.post('/register', {
            userName: name,
            password: pass,
            email: email,
            autoLogin: true,
            rememberMe: true
        })
        .success(function(data, status, headers, config) {
            successCallback(data, status, headers, config);
            $cookieStore.put('user', self.user);
        })
        .error(function(data, status, headers, config) {
            errorCallback(data, status, headers, config);
        });
    };

    this.signin = function(name, pass,successCallback, errorCallback) {
        $http.post('/auth/credentials', {
            userName: name,
            password: pass,
            rememberMe: true
        })
        .success(function(data, status, headers, config) {
            successCallback(data, status, headers, config);
            $cookieStore.put('user', self.user);
        })
        .error(function(data, status, headers, config) {
            errorCallback(data, status, headers, config);
        });
    };

    this.signout = function() {
        if (this.user.isAuthenticated) {
            this.user.name = undefined;
            this.user.id = undefined;
            this.user.sessionId = undefined;
            this.user.isAuthenticated = false;
            this.user.preferences = {
                matchesPerPage: 5,
                teamsPerPage: 10,
                playersPerPage: 10
            }
            $cookieStore.remove('user');
        }
        return this.user;
    };
}]);