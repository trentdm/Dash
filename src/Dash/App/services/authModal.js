app.service('authModal', ['$modal', function($modal) {
    //from http://brewhouse.io/blog/2014/12/09/authentication-made-simple-in-single-page-angularjs-applications.html
    return function() {
        var instance = $modal.open({
            templateUrl: 'App/partials/auth.html',
            controller: 'AuthCtrl',
            controllerAs: 'AuthCtrl'
        });

        return instance.result;
    };
}]);