define(['AdmissionRoutes', 'services/routeResolver', 'angularroute', 'uibootstraptpls', 'datetimepicker', 'moment'], function (config, routeResolver) { //,'angular-animate'
    var app = angular.module('app', ['ui.router', 'ngStorage', 'ngLoadingSpinner', 'ui.bootstrap', 'ui.bootstrap.datetimepicker']); //,'ngAnimate','uiCropper'
    app.config(
        [
            '$stateProvider',
            '$urlRouterProvider',
            '$locationProvider',
            '$controllerProvider',
            '$compileProvider',
            '$filterProvider',
            '$provide',
           
            function ($stateProvider, $urlRouterProvider, $locationProvider, $controllerProvider, $compileProvider, $filterProvider, $provide) {
                app.controller = $controllerProvider.register;
                app.directive = $compileProvider.directive;
                app.filter = $filterProvider.register;
                app.factory = $provide.factory;
                app.service = $provide.service;
                $locationProvider.hashPrefix('');
             
                if (config.routes !== undefined) {
                    angular.forEach(config.routes, function (route, path) {
                        $stateProvider.state(path, { url: route.url, templateUrl: route.templateUrl, resolve: routeResolver(route.dependencies) });
                    });

                }

                if (config.defaultRoutePaths !== undefined) {
                    $urlRouterProvider.otherwise(config.defaultRoutePaths);
                }
            
                app.compileProvider = $compileProvider;
            }
        ]);
    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    })
   
    
    
    app.factory('beforeUnload', function ($rootScope, $window) {
        $window.onbeforeunload = function (e) {
            var confirmation = {};
            var event = $rootScope.$broadcast('onBeforeUnload', confirmation);
            if (event.defaultPrevented) {
                return confirmation.message;
            }
        };
        $window.onunload = function () {
            $rootScope.$broadcast('onUnload');
        };
        return {};
    })

    //app.factory('clearCache', function ($cacheFactory) {
    //    $cacheFactory.destroy(); beforeUnload
    //})

    app.run(function ($rootScope, $state, $location, $localStorage, $window, $http, $document) {
        var WebApiUrl = '/api/';
        var d = new Date();
        var n = d.getTime();  //n in ms
        var WebApiUrl = '/api/';
        var data = { "userName": data !== null ? data : $localStorage.authorizationData.userName };
        //   delete $localStorage

        var request = {
            method: 'GET',
            url: WebApiUrl + "/AdminService/GetCaptchaString10",
            data: data,
            headers: {
                'Content-Type': 'application/json'
            }
        };
        $http(request).then(function (d) {
            sessionStorage.removeItem('SessionCaptcha');
            console.log(d.data)
            sessionStorage.setItem('SessionCaptcha', d.data);
        }, function (err) {
            let error = JSON.parse(err);
            alert(error.message);
        });

       
    
        window.moment = require('moment');       
        if (sessionStorage.loggedIn == undefined) {
            delete $localStorage.authorizationData;
            sessionStorage.loggedIn = "no";
        }
        else if (sessionStorage.loggedIn == "no") {
            delete $localStorage.authorizationData;
        }
        var authData = $localStorage.authorizationData;
        if (authData == undefined) {

            $state.go('Dashboard');
	       // $state.go('maintainance');			
        };
			$rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
             //var WebApiUrl = 'http://testapi.hebeon.com/';
            var WebApiUrl = '/api/';
            var authData = $localStorage.authorizationData;
            //if ((sessionStorage.loggedIn == "no" || authData == undefined) && toState.name != 'login' && toState.name !== 'ForgetPassword' && toState.name !== 'ForgetPasswordSaved' && toState.name !== 'GovtColEnroll' && toState.name !== 'GovtColEnrollExist' && toState.name !== 'GovtColEnrollList' && toState.name !== 'ReportViewer1Controller' && (toState.name.indexOf("StudentOnlineRequest") != 0)) {
            if ((authData == undefined) && toState.name != 'login' && toState.name !== 'Components' && toState.name !== 'ForgetPassword' && toState.name !== 'ForgetPasswordSaved' && toState.name !== 'GovtColEnroll' && toState.name !== 'GovtColEnrollExist' && toState.name !== 'GovtColEnrollList' && toState.name !== 'ReportViewer1Controller' && toState.name !== 'StudentResult' && toState.name !== 'ComingSoon' && (toState.name.indexOf("StudentOnlineRequest") != 0)) {
                event.preventDefault();
                $state.transitionTo("login", null, { notify: false });
                $state.go('login');
                return;
            }
            if (authData) {
              //  config.headers.Authorization = 'Bearer ' + authData.token;
            }
          
           
            if (toState.name.indexOf("Results") == 0) {             
                if (toState.name != 'Results' && toState.name != 'login') {
                    var UsersRightsdata = [];
                    UsersRightsdata = $localStorage.authorizationData.UserRights;
                    for (var i = 0; i < UsersRightsdata.length; i++) {
                        var PrName;
                            PrName = 'Results.' + UsersRightsdata[i].GridFormToOpen;
                        if (PrName == toState.name) {
                            if ((UsersRightsdata[i].isaddable != 'Y') && (UsersRightsdata[i].isupdatable != 'Y') && (UsersRightsdata[i].isdeletable != 'Y') && (UsersRightsdata[i].isprintable != 'Y')) {
                                event.preventDefault();
                                    $state.transitionTo("Results", null, { notify: false });
                                    $state.go('Results');
                               
                            } else {
                                var request = {
                                    method: 'GET',
                                    url: WebApiUrl + "/api/SystemUser/GetUpdateSystemUserLog/?xFrmString=" + UsersRightsdata[i].SysProgName + "&xFrmProgramID=" + UsersRightsdata[i].SysProgID + "&xFrmOpenedByUserID=" + $localStorage.authorizationData.SysUserID + "",
                                    data: '',
                                    headers: {
                                        'Content-Type': undefined
                                    }
                                };
                                $http(request)
                                    .success(function (d) {
                                        alert("yes");
                                    })
                                    .error(function () {
                                    });
                            }
                        }
                      
                    }
                }
            }else if (toState.name.indexOf("Assessment") == 0) {              
                if (toState.name != 'Assessment' && toState.name != 'login') {
                    var UsersRightsdata = [];
                    UsersRightsdata = $localStorage.authorizationData.UserRights;
                    for (var i = 0; i < UsersRightsdata.length; i++) {
                        var PrName;
                        PrName = 'Assessment.' + UsersRightsdata[i].GridFormToOpen;
                        if (PrName == toState.name) {
                            if ((UsersRightsdata[i].isaddable != 'Y') && (UsersRightsdata[i].isupdatable != 'Y') && (UsersRightsdata[i].isdeletable != 'Y') && (UsersRightsdata[i].isprintable != 'Y')) {
                                event.preventDefault();
                                $state.transitionTo("Assessment", null, { notify: false });
                                $state.go('Assessment');
                               
                            } else {
                                var request = {
                                    method: 'GET',
                                    url: WebApiUrl + "/api/SystemUser/GetUpdateSystemUserLog/?xFrmString=" + UsersRightsdata[i].SysProgName + "&xFrmProgramID=" + UsersRightsdata[i].SysProgID + "&xFrmOpenedByUserID=" + $localStorage.authorizationData.SysUserID + "",
                                    data: '',
                                    headers: {
                                        'Content-Type': undefined
                                    }
                                };
                                $http(request)
                                    .success(function (d) {
                                        alert("yes");
                                    })
                                    .error(function () {
                                    });
                            }
                        }
                      
                    }
                }
            }
        })
    });
       
    var serviceBase = '/api/';
    //var serviceBase = 'http://192.168.1.98:85/';

    app.constant('ngAuthSettings', {
        apiServiceBaseUri: serviceBase,
        clientId: 'baseApp',
        //WebApiUrl: 'http://localhost:65322/',
        //WebApiUrl: 'http://202.62.85.194/',
        ExportToExcelUrl: '',
        ExportToWordUrl: '',
        ExportToPdfUrl: '',
        LoggedUserId: 0,
        UserRights: [],
        CompanyId: 0,
        CompanyName: 'State Board of Technical Education and Training, Telangana',
        CollegeID: 0,
        AcdYrID: 0,
        PrevAdmNo: 0,
        StudentApprovalData: [],
        YrName: 2019,
        CollegeCatName: '',
        college_name1: '',
        MngtTypID: 0,
        SysUsrGrpID: 0,
        SeqNo: 0,
        DistrictIDs: ''
    });
    return app;
});

