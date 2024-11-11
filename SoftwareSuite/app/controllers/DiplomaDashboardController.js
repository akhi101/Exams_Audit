define(['app'], function (app) {
    app.controller("DiplomaDashboardController", function ($scope, $http, $state) {
        var SubmodulesList = [];
        var obj = {};
        obj.SysModName = 'Exam Fee Payment';
        obj.SysModID = '1';
        obj.Module = 'DiplomaFeePayment';
        obj.ModuleImageClass = 'small-box bg-aqua';
        SubmodulesList.push(obj);

        var obj = {};
        obj.SysModName = 'Hall Ticket Download';
        obj.SysModID = '2';
        obj.Module = 'Hallticket';
        obj.ModuleImageClass = 'small-box bg-blue';
        SubmodulesList.push(obj);     

        var obj = {};
        obj.SysModName = 'Diploma Results';
        obj.SysModID = '3';
        obj.Module = 'StudentResult';
        obj.ModuleImageClass = 'small-box bg-yellow';
        SubmodulesList.push(obj);

        var obj = {};
        obj.SysModName = 'Student Attendance';
        obj.SysModID = '4';
        obj.Module = 'StudentAttendance';
        obj.ModuleImageClass = 'small-box bg-olive';
        SubmodulesList.push(obj);
        var obj = {};

        obj.SysModName = 'Student Feedback';
        obj.SysModID = '5';
        obj.Module = 'https://sbtet.telangana.gov.in/index.html#!/index/StudentFeedback';
        obj.ModuleImageClass = 'small-box bg-teal';
        SubmodulesList.push(obj);
        var obj = {};
        obj.SysModName = 'Consolidated Result';
        obj.SysModID = '6';
        obj.Module = 'StudentConsolidatedResult';
        obj.ModuleImageClass = 'small-box bg-maroon';
        SubmodulesList.push(obj);
        var obj = {};
        obj.SysModName = '3 Backlog Exemption';
        obj.SysModID = '7';
        obj.Module = 'StudentOnlineRequest';
        obj.ModuleImageClass = 'small-box bg-green';
        SubmodulesList.push(obj);
        var obj = {};
        obj.SysModName = 'Student Services';
        obj.SysModID = '8';
        obj.Module = 'StudentRequestForm';
        obj.ModuleImageClass = 'small-box bg-purple';
        SubmodulesList.push(obj);
       
        var obj = {};
        obj.SysModName = 'Genuineness Check';
        obj.SysModID = '9';
        obj.Module = 'GenuinenessCheckForm';
        obj.ModuleImageClass = 'small-box bg-orange';
        SubmodulesList.push(obj);
        // $scope.SubmodulesList = SubmodulesList;
        var obj = {};
        obj.SysModName = 'Two Years Engineering Certificate';
        obj.SysModID = '10';
        obj.Module = '2Years';
        obj.ModuleImageClass = 'small-box bg-yellow';
        obj.href ='https://sbtet.telangana.gov.in/index.html#!/index/TwoYearsCertificateRequest';
        SubmodulesList.push(obj);
        $scope.SubmodulesList = SubmodulesList;
     console.log($scope.SubmodulesList)
    });
})