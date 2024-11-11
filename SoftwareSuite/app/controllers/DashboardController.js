define(['app'], function (app) {
    app.controller("DashBoardController", function ($scope, $http, $state,AdminService) {
        var userType = '999';
		 var getNotifications = AdminService.GetNotificationByUser(userType);
			 getNotifications.then(function (response) {
			 $scope.Notification = response;
		 },
		 function (error) {	
			var err = JSON.parse(error);
		 });

        var modulesList = [];

        var obj = {};
        obj.SysModName = 'Diploma ';
        obj.SysModID = '1';
        obj.Module = 'DiplomaDashboard';
        obj.ModuleImageClass = 'small-box bg-yellow';
        modulesList.push(obj);

        var obj = {};
        obj.SysModName = 'TW & SH';
        obj.SysModID = '2';
        obj.Module = 'TwshDashboard';
        obj.ModuleImageClass = 'small-box bg-blue';
        modulesList.push(obj);
        // $scope.modulesList = modulesList;
      
        var obj = {};
        obj.SysModName = 'CCIC';
        obj.SysModID = '3';
        obj.Module = 'https://sbtet.telangana.gov.in/CCIC/Search/StudentResults';
        // obj.Module = 'https://sbtet.telangana.gov.in/CCIC/Search/StudentResults';
        obj.ModuleImageClass = 'small-box bg-blue';
        modulesList.push(obj);
        $scope.modulesList = modulesList;
      

        $scope.stboclogin = function () {
            $state.go('login');
        }


    });
})