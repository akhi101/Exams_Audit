define(['app'], function (app) {
    app.controller("ResultsController", function ($scope, $http, $localStorage, $state, AppSettings, MenuService, DrillDownService) {
       // $state.go('Results.BranchWise');
      
        var authData = $localStorage.authorizationData;
        $scope.userName = authData.userName;
        AppSettings.userName = authData.userName;
        AppSettings.LoggedUserId = authData.SysUserID;
        AppSettings.CollegeID = authData.CollegeID;
        $scope.CollegeID = authData.CollegeID;
        AppSettings.AcdYrID = authData.AcdYrID;
        AppSettings.PrevAdmNo = authData.PrevAdmNo;
        AppSettings.TypeFlag = authData.TypeFlag;
        AppSettings.MngtTypID = authData.MngtTypID;
        AppSettings.SysUsrGrpID = authData.SysUsrGrpID,
        AppSettings.SeqNo = authData.SeqNo,
        AppSettings.DistrictIDs = authData.DistrictIDs
        $scope.College_Code = authData.College_Code;
        $scope.College_Name = authData.College_Name;
        if ((AppSettings.TypeFlag != 'B') && (AppSettings.TypeFlag != 'D')) {
            //AppSettings.CollegeCatName = $localStorage.CollegeData.CollegeCatName;
            //AppSettings.Clg_Type = $localStorage.CollegeData.Clg_Type;
            //AppSettings.college_name1 = $localStorage.CollegeData.college_name1;

            //var AcdYrClgCntList = MenuService.GetInsertAcdYrClg(AppSettings.CollegeID, AppSettings.AcdYrID, AppSettings.LoggedUserId);
            //AcdYrClgCntList.then(function (data, status, headers, config, error) {
            //}, function (error) {
            //    alert(error);
            //});

            $scope.college_name1 = AppSettings.college_name1;
        }
        $scope.OpenDashboard = function () {
            $state.go('Dashboard')
        }

        if (AppSettings.UserRights.length == 0) { AppSettings.UserRights = authData.UserRights; }
        var UsersRightsdata = [];
        UsersRightsdata = AppSettings.UserRights;
        var programsList = [];
        var RequestList = [];
        var ReportsList = [];
        for (var i = 0; i < UsersRightsdata.length; i++) {
            if (UsersRightsdata[i].ModuleRouteName == "Results") {
                if ((UsersRightsdata[i].isaddable != 'Y') && (UsersRightsdata[i].isupdatable != 'Y') && (UsersRightsdata[i].isdeletable != 'Y') && (UsersRightsdata[i].isprintable != 'Y')) {
                }
                else {
                    if ((UsersRightsdata[i].ProgramType == "T") || (UsersRightsdata[i].ProgramType == "M")) {
                        if (UsersRightsdata[i].GridFormToOpen == 'PreYearAdmissionEntry') {
                            if ((AppSettings.PrevAdmNo == "") || (AppSettings.PrevAdmNo == undefined) || (AppSettings.PrevAdmNo == 0)) {
                                var obj = {};
                                obj.SysProgName = UsersRightsdata[i].SysProgName;
                                obj.SysProgID = UsersRightsdata[i].SysProgID;
                                obj.GridFormToOpen = UsersRightsdata[i].GridFormToOpen;
                                obj.PrgImgClass = UsersRightsdata[i].PrgImgClass;
                                obj.IsActive = UsersRightsdata[i].IsActive;
                                programsList.push(obj);
                            } else {
                            }
                        } else {
                            if (UsersRightsdata[i].GridFormToOpen != 'CollegeInfo') {
                                var obj = {};
                                obj.SysProgName = UsersRightsdata[i].SysProgName;
                                obj.SysProgID = UsersRightsdata[i].SysProgID;
                                obj.GridFormToOpen = UsersRightsdata[i].GridFormToOpen;
                                obj.PrgImgClass = UsersRightsdata[i].PrgImgClass;
                                obj.IsActive = UsersRightsdata[i].IsActive;
                                programsList.push(obj);
                            }
                        }
                    } else if (UsersRightsdata[i].ProgramType == "Q") {
                        var obj = {};
                        obj.SysProgName = UsersRightsdata[i].SysProgName;
                        obj.SysProgID = UsersRightsdata[i].SysProgID;
                        obj.GridFormToOpen = UsersRightsdata[i].GridFormToOpen;
                        RequestList.push(obj);
                    } else if (UsersRightsdata[i].ProgramType == "R") {
                        var obj = {};
                        obj.SysProgName = UsersRightsdata[i].SysProgName;
                        obj.SysProgID = UsersRightsdata[i].SysProgID;
                        obj.GridFormToOpen = UsersRightsdata[i].GridFormToOpen;
                        ReportsList.push(obj);
                    }
                }
            }
        }        
        $scope.programsList = programsList;
        $scope.RequestList = RequestList;
        $scope.ReportsList = ReportsList;


        $scope.OpenBranchWiseReport = function () {
            $state.go('Dashboard.Results.BranchWise');
        }
        $scope.OpenStudentWiseReport = function () {
            $state.go('Dashboard.Results.StudentWise');
        }
        $scope.logOut = function () {
            sessionStorage.loggedIn = "no";
            delete $localStorage.authorizationData;
            var InsertLoginList = MenuService.GetUpdateLogoutInfo(AppSettings.LoggedUserId, $scope.userName);
            InsertLoginList.then(function (Districtdata, status, headers, config, error) {
            }, function (error) {
                alert(error);
            });
            $scope.authentication = {
                isAuth: false,
                UserId: 0,
                userName: ""
            };
            $state.go('login');
        }
    });
});












