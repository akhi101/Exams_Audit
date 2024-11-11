define(['app'], function (app) {
    app.controller("ExamResultsDashBoardController", function ($scope, $http, $state) {
      
    
        var SubmodulesList = [];
       
        var obj = {};
        obj.SysModName = 'Diploma Results';
        obj.SysModID = '3';
        obj.Module = 'StudentResult';
        obj.ModuleImageClass = 'small-box bg-yellow';
        SubmodulesList.push(obj);

        var obj = {};
        obj.SysModName = 'TW & SH Results';
        obj.SysModID = '4';
        obj.Module = 'TypingAndShorthand';
        obj.ModuleImageClass = 'small-box bg-blue';
        SubmodulesList.push(obj);
        $scope.SubmodulesList = SubmodulesList;

      //$scope.OpenExamModule = function (Modulename) {
          
        //    var strroute = 'ResultsDashboard.' + Modulename.Module;
        //    $state.go(strroute);
        //     }
        $scope.stboclogin = function () {
            $state.go('login');
        }

       
    });
})