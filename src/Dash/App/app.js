'use strict';

var app = angular.module('app', [
    'ngAnimate',
    'ngCookies',
    'ui.router',
    'ui.bootstrap'
]);

app.config(['$stateProvider', '$httpProvider', '$urlRouterProvider',
    function ($stateProvider, $httpProvider, $urlRouterProvider) {
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: 'App/partials/home.html',
                controller: 'HomeCtrl',
                data: {
                    requireLogin: false
                }
            })
            .state('machine', {
                url: '/machine',
                templateUrl: 'App/partials/machine.html',
                controller: 'MachineCtrl',
                data: {
                    requireLogin: false
                }
            })
            .state('about', {
                url: '/about',
                abstract: true,
                templateUrl: 'App/partials/about.html',
                data: {
                    requireLogin: false
                }
            })
            .state('about.index', {
                url: '',
                templateUrl: 'App/partials/about.index.html',
                data: {
                    requireLogin: false
                }
            })
            .state('about.form', {
                url: '/form',
                templateUrl: 'App/partials/about.form.html',
                data: {
                    requireLogin: false
                }
            })
            .state('about.sheet', {
                url: '/sheet',
                templateUrl: 'App/partials/about.sheet.html',
                data: {
                    requireLogin: false
                }
            })
            .state('about.data', {
                url: '/data',
                templateUrl: 'App/partials/about.data.html',
                data: {
                    requireLogin: false
                }
            });

        $urlRouterProvider.otherwise('/');
        
        $httpProvider.interceptors.push(function ($timeout, $q, $injector) {
            var authModal, $http, $state;

            // this trick must be done so that we don't receive
            // `Uncaught Error: [$injector:cdep] Circular dependency found`
            $timeout(function () {
                authModal = $injector.get('authModal');
                $http = $injector.get('$http');
                $state = $injector.get('$state');
            });

            return {
                responseError: function (rejection) {
                    return rejection;//may want to force modal on 401 auth failure, but not at this time
                    if (rejection.status !== 401) {
                        return rejection;
                    }

                    var deferred = $q.defer();

                    authModal()
                      .then(function () {
                          deferred.resolve($http(rejection.config));
                      })
                      .catch(function () {
                          $state.go('home');
                          deferred.reject(rejection);
                      });

                    return deferred.promise;
                }
            };
        });
    }
]);

app.run(['$rootScope', '$state', 'authModal', 'authService', 'versionService',
    function ($rootScope, $state, authModal, authService, versionService) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            versionService.getVersionInfo(
                function (version) {
                    if (version.isOutOfDate) {
                        $rootScope.$broadcast('alert', { type: 'warning', msg: 'Client version is out of date. Please refresh your browser.' });
                    }
                },
                function () {
                    $rootScope.$broadcast({ type: 'danger', msg: 'Server could not be reached. Please try again later.' });
                });

            if (toState.data.requireLogin && !authService.user.isAuthenticated) {
                event.preventDefault();

                authModal()
                    .then(function(data) {
                        return $state.go(toState.name, toParams);
                    })
                    .catch(function() {
                        return $state.go('home');
                    });
            }
        });
    }]);