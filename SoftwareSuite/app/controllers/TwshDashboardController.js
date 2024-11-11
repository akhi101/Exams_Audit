
define(['app'], function (app) {
    app.controller("TwshDashBoardController", function ($scope, $http, $state) {

        var SubmodulesList = [];
        var obj = {};
        obj.SysModName = 'TW & SH Results';
        obj.SysModID = '2';
        obj.Module = 'ShorthandResult';
        obj.ModuleImageClass = 'small-box bg-blue';
        obj.href ='';
        SubmodulesList.push(obj);
      

        var obj = {};
        // Apply for ! 
        obj.SysModName = 'CBT TYPEWRITING Exam';
        obj.SysModID = '3';
        obj.Module = 'cbtResult';
        obj.ModuleImageClass = 'small-box bg-olive';
        obj.href ='https://sbtet.telangana.gov.in/index.html#!/Twsh/OnlineApplication';
        SubmodulesList.push(obj);
        $scope.SubmodulesList = SubmodulesList;
       
    });
})