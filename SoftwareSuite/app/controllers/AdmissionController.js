define(['app'], function (app) {
    app.directive('convertToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (val) {
                    return val != null ? parseInt(val, 10) : null;
                });
                ngModel.$formatters.push(function (val) {
                    return val != null ? '' + val : null;
                });
            }
        };
    });
    app.factory('Excel', function ($window) {
        //alert("hello");
        var uri = 'data:application/vnd.ms-excel;base64,',
            template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
            base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
            format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
        return {
            tableToExcel: function (tableId, worksheetName) {
                 var table = $(tableId);             
                ctx = { worksheet: worksheetName, table: table.html() },
                    href = uri + base64(format(template, ctx));
                return href;
            }
        };
    });
    app.controller("AdmissionController", function ($scope, $http, $localStorage,$location, $state, $stateParams, AppSettings, MenuService,SystemUserService, AdmissionService, Excel, $timeout) {
        $scope.college = null;
        $scope.AcademicId = 0;
        $scope.Admission = {};       
        var authData = $localStorage.authorizationData;
       // console.log(authData);
        $scope.userName = authData.userName;
        AppSettings.userName = authData.userName;
        AppSettings.LoggedUserId = authData.SysUserID;
        AppSettings.CollegeID = authData.CollegeID;
        $scope.CollegeID = authData.CollegeID;
        AppSettings.AcdYrID = authData.AcdYrID;
        AppSettings.PrevAdmNo = authData.PrevAdmNo;
        AppSettings.TypeFlag = authData.TypeFlag;
        AppSettings.MngtTypID = authData.MngtTypID;
        AppSettings.SysUsrGrpID = authData.SysUsrGrpID;
        AppSettings.SeqNo = authData.SeqNo;
        AppSettings.DistrictIDs = authData.DistrictIDs;
        $scope.College_Code = authData.College_Code;
        AppSettings.College_Name = authData.College_Name;
        $scope.College_Name = authData.College_Name;
        $scope.SystemUserTypeId = authData.SystemUserTypeId;
        $scope.BranchId = authData.BranchId;
        $scope.IsPrinciple = false;
        //   alert($scope.BranchId);
        // alert($scope.College_Code);
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
       
        //temporary validation
        //var temp = typeof $scope.College_Code;
        var regex = /^\d+$/;
        var temp = regex.test($scope.userName);
        if (temp && ($scope.CollegeID != "0" && $scope.CollegeID!= "")) {
            $scope.IsPrinciple = true;
        }
        //if ($scope.SystemUserTypeId == "2") {
        //       $scope.IsPrinciple = true;
        //  }


        $scope.OpenReAdmissionList = function () {
            $state.go("Dashboard.Admission.ReAdmission");
        }
              
       
        $scope.OpenStudentWise = function () {
            $state.go("StudentResult");
        }      


        $scope.OpenPinGeneratedReport = function () {
            $state.go("Dashboard.Admission.PinGeneratedReport");
        },

        $scope.GetAttendanceReport = function () {
           
            if ($scope.college !=null) {
                 authData.College_Code = $scope.college;
            } else {
                authData.College_Code = $scope.College_Code;
            }               
          
            $state.go("Dashboard.Admission.GetAttendanceReport");
        }

        if(AppSettings.UserRights.length == 0) { AppSettings.UserRights = authData.UserRights; }
        var UsersRightsdata = [];
        UsersRightsdata = AppSettings.UserRights;
        var programsList = [];
        var RequestList = [];
        var ReportsList = [];
        for (var i = 0; i < UsersRightsdata.length; i++) {
            if (UsersRightsdata[i].ModuleRouteName == "Admission") {
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

        //   alert("Admission: " + programsList.length);
        $scope.programsList = programsList;
        $scope.RequestList = RequestList;
        $scope.ReportsList = ReportsList;

        $scope.OpenProgram = function (GridFormToOpen) {
            var strroute = 'Admission.' + GridFormToOpen;
            $state.go(strroute);
        }
        if (AppSettings.CollegeCatName == 'GOVERNMENT') {
            $scope.ShowGovtCollege = true;
        } else {
            $scope.ShowGovtCollege = false;
        }
        $scope.OpenDashboard = function () {
            $state.go('Dashboard')
        }
        $scope.MyProfile = function () {
            //$state.go('Admission.UserProfile');
        }
        $scope.GoToHome = function () {
            $state.go('Dashboard.Admission');
        }
        $scope.MyCollege = function () {
            $state.go('Admission.CollegeInfo');

        }
        $scope.OpenStudentSearchReport = function () {
            $state.go('Dashboard.Admission.SearchStudent');

        },
        $scope.Transferstudent = function () {
            $state.go('Dashboard.Admission.TransferStudent');

        },
         //$scope.Transferstudent = function () {
         //    $state.go('Admission.TransferStudent');

         //}
        $scope.logOut = function () {
            sessionStorage.loggedIn = "no";
            delete $localStorage.authorizationData;          
            var logUser = SystemUserService.postUserLogout($scope.userName);
            logUser.then(function (response) {              
             
            }, function (err) {
                alert(err);
            });
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
            $state.go('login')
        },

        //$scope.$on('onBeforeUnload', function (e, confirmation) {
        //    confirmation.message = "If you refresh or close browser, your session will expire and all data will be lost";
        //    e.preventDefault();
        //});
        //$scope.$on('onUnload', function (e) {
        //    delete $localStorage.authorizationData;
        //    sessionStorage.loggedIn = "no";
        //});
        $scope.OpenRegisterStudent = function () {
            //alert("hi");
            $state.go('StudentReg');
            //$state.go('login')
            //$state.go('StudentReg')
        }

        //drilldown coding

        $scope.export = function () {
            var doc = new jsPDF('p', 'pt', 'a4');
            var source = document.getElementById('alldata').innerHTML;
            var margins = {
                top: 10,
                bottom: 10,
                left: 10,
                width: 595
            };
            doc.fromHTML(
                source, // HTML string or DOM elem ref.
                margins.left,
                margins.top, {
                    'width': margins.width,
                    'elementHandlers': specialElementHandlers
                },
                function (dispose) {
                    // dispose: object with X, Y of the last line add to the PDF 
                    //          this allow the insertion of new lines after html
                    doc.save('Test.pdf');
                }, margins);
        }
        $scope.adminuser = false;
        $scope.ifcoluser = false;

        $scope.exportTableToExcel = function (tableid) {           
            var exportHref = Excel.tableToExcel(tableid, 'stdentDetails');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = "Studentdetails.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }



        $scope.loadYears = function () {
           
            var AcademicYears = AdmissionService.GetAcademicYearsActive(AppSettings.CollegeID);
            AcademicYears.then(function (data, status, headers, config, error) {
                $scope.AcademicYears = data.Table;

                //console.log($scope.AcademicYears);
                $scope.AcademicId = $scope.AcademicYears[0].AcademicId;
                AppSettings.AcademicId = $scope.AcademicYears[0].AcademicId;
                authData.AcademicId = $scope.AcademicYears[0].AcademicId;


                if ($scope.CollegeID == 0) {
                    $scope.LoadImg = true;
                    $scope.StudentDetailsFound = false;
                    $scope.CollegeDetailsFound = false;
                    var CollegesInfo = AdmissionService.GetCollegesSchemeSemInfo(AppSettings.CollegeID);
                    CollegesInfo.then(function (data) {
                        if (data.Table.length > 0) {
                            $scope.collegeinfo = data.Table;                          
                            if ($scope.collegeinfo[0].CollegeCode != null) {
                                $scope.college = $scope.collegeinfo[0].CollegeCode;
                                AppSettings.College_Name = $scope.collegeinfo[0].college_name;
                            } else {
                                $scope.college = authData.userName;
                                AppSettings.College_Name = authData.College_Name;
                            }
                            var AcademicYearId = $scope.AcademicId;
                            authData.AcdYrID = AcademicYearId;
                            var hodData = AdmissionService.GetDataForAdmissionDashboard(AppSettings.LoggedUserId, $scope.college, AcademicYearId);
                            hodData.then(function (hodData, status, headers, config, error) {
                                $scope.hodData = hodData;
                                $scope.LoadImg = false;
                                $scope.StudentDetailsFound = true;
                                $scope.CollegeDetailsFound = true;
                                // alert(hodData.Table2[0].AcademicId);
                                if (hodData.Table1[0].AcademicId == undefined) { 
                                    $scope.Academic = "";
                                } else {
                                    $scope.Academic = hodData.Table1[0].AcademicId;
                                  
                                }
                                //  $scope.$digest();
                            }, function (error) {
                                alert("data not found");
                            });
                        }

                    }, function (error) {
                        alert(error);
                    });
                }
                else {
                   
                    var AcademicYearId;
                    if ($scope.AcademicId == null)
                        AcademicYearId = 0;
                    else
                        AcademicYearId = $scope.AcademicId;

                    if (AcademicYearId != null && AcademicYearId != '0') {
                        $scope.LoadImg = true;
                        $scope.StudentDetailsFound = false;
                        $scope.CollegeDetailsFound = false;

                        var hodData = AdmissionService.GetDataForAdmissionDashboard(AppSettings.LoggedUserId, $scope.College_Code, AcademicYearId);
                        hodData.then(function (hodData, status, headers, config, error) {
                            $scope.hodData = hodData;
                            $scope.LoadImg = false;
                            $scope.StudentDetailsFound = true;
                            $scope.CollegeDetailsFound = true;
                            //console.log(hodData);
                            // $scope.Academic = hodData.Table2[0].AcademicId;
                            // alert(hodData.Table[0].BankIfsc);
                            var Intake = 0;
                            var Allotted = 0;
                            var Reported = 0;
                            var DataNotUpdated = 0;
                            var AadharNotUpdated = 0;
                            for (var i = 0; i < hodData.Table1.length; i++) {
                                if (hodData.Table1[i].Intake != null)
                                    Intake = Intake + hodData.Table1[i].Intake;
                                if (hodData.Table1[i].Allotted != null)
                                    Allotted = Allotted + hodData.Table1[i].Allotted;
                                if (hodData.Table1[i].Reported != null)
                                    Reported = Reported + hodData.Table1[i].Reported;
                                if (hodData.Table1[i].DataStatus != null)
                                    DataNotUpdated = DataNotUpdated + hodData.Table1[i].DataStatus;
                                if (hodData.Table1[i].AadharVerified != null)
                                    AadharNotUpdated = AadharNotUpdated + hodData.Table1[i].AadharVerified;
                            }
                            $scope.Intake = Intake;
                            $scope.Allotted = Allotted;
                            $scope.Reported = Reported;
                            $scope.DataNotUpdated = DataNotUpdated;
                            $scope.AadharNotUpdated = AadharNotUpdated;
                            //  $scope.$digest();
                        }, function (error) {
                            alert(error);
                        });
                    } else {
                        alert('AcademicId is undefined');
                    }
                }


            }, function (error) { alert(error); });
        }
        $scope.loadAdmissionDataByYear = function (AcademicYearId) {
            if ($scope.CollegeID == 0) {
                $scope.LoadImg = true;
                $scope.StudentDetailsFound = false;
                $scope.CollegeDetailsFound = false;
                var CollegesInfo = AdmissionService.GetCollegesSchemeSemInfo(AppSettings.CollegeID);
                CollegesInfo.then(function (data) {
                    if (data.Table.length > 0) {
                        $scope.collegeinfo = data.Table;
                        $scope.college = $scope.collegeinfo[0].CollegeCode;
                        var AcademicYear = AcademicYearId;
                        var AcademicYear = $scope.AcademicId;
                        var hodData = AdmissionService.GetDataForAdmissionDashboard(AppSettings.LoggedUserId, $scope.college, AcademicYearId);
                        hodData.then(function (hodData, status, headers, config, error) {
                            $scope.hodData = hodData;
                            $scope.LoadImg = false;
                            $scope.StudentDetailsFound = true;
                            $scope.CollegeDetailsFound = true;
                            // alert(hodData.Table2[0].AcademicId);
                            $scope.Academic = hodData.Table1[0].AcademicId;
                            //  $scope.$digest();
                        }, function (error) {
                            alert("data not found");
                        });
                    }

                }, function (error) {
                    alert(error);
                });
            }
            else {
              
                var AcademicYearId;
                if (document.getElementById("AcademicYear") == null)
                    AcademicYearId = 0;
                else
                    AcademicYearId = document.getElementById("AcademicYear").value;

                if (AcademicYearId != null && AcademicYearId != '0') {
                    $scope.LoadImg = true;
                    $scope.StudentDetailsFound = false;
                    $scope.CollegeDetailsFound = false;
                    var hodData = AdmissionService.GetDataForAdmissionDashboard(AppSettings.LoggedUserId, $scope.College_Code, AcademicYearId);
                    hodData.then(function (hodData, status, headers, config, error) {
                        $scope.hodData = hodData;
                        $scope.LoadImg = false;
                        $scope.StudentDetailsFound = true;
                        $scope.CollegeDetailsFound = true;
                        //console.log(hodData);
                        // $scope.Academic = hodData.Table2[0].AcademicId;
                        // alert(hodData.Table[0].BankIfsc);
                        var Intake = 0;
                        var Allotted = 0;
                        var Reported = 0;
                        var DataNotUpdated = 0;
                        var AadharNotUpdated = 0;
                        for (var i = 0; i < hodData.Table1.length; i++) {
                            if (hodData.Table1[i].Intake != null)
                                Intake = Intake + hodData.Table1[i].Intake;
                            if (hodData.Table1[i].Allotted != null)
                                Allotted = Allotted + hodData.Table1[i].Allotted;
                            if (hodData.Table1[i].Reported != null)
                                Reported = Reported + hodData.Table1[i].Reported;
                            if (hodData.Table1[i].DataStatus != null)
                                DataNotUpdated = DataNotUpdated + hodData.Table1[i].DataStatus;
                            if (hodData.Table1[i].AadharVerified != null)
                                AadharNotUpdated = AadharNotUpdated + hodData.Table1[i].AadharVerified;
                        }
                        $scope.Intake = Intake;
                        $scope.Allotted = Allotted;
                        $scope.Reported = Reported;
                        $scope.DataNotUpdated = DataNotUpdated;
                        $scope.AadharNotUpdated = AadharNotUpdated;
                        //  $scope.$digest();
                    }, function (error) {
                        alert(error);
                    });
                } else {
                    alert('AcademicId is undefined');
                }
            }
        }

        $scope.DownloadtoExcel = function (tableid) {
          var exportHref = Excel.tableToExcel(tableid, 'stdentDetails');           
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "Studentdetails.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }
       
        $scope.DownloadtoPdf = function (tableid) {
            
            var height = $(tableid).height();
            $(tableid).height('auto');
            var scaleBy = 5;
            var w = 1000;
            var h = 1000;
        
            var div = document.querySelector(tableid);
            var canvas = document.createElement('canvas');
            canvas.width = w * scaleBy;
            canvas.height = h * scaleBy;
            canvas.style.width = w + 'px';
            canvas.style.height = h + 'px';
            var context = canvas.getContext('2d');
            context.scale(scaleBy, scaleBy);
            html2canvas(div, {
               // canvas: canvas,
                onrendered: function (canvas) {
                    thecanvas = canvas;
                    var data = thecanvas.toDataURL();
                   
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500
                        }],
                    };
                    pdfMake.createPdf(docDefinition).download("Table.pdf");
                   
                }
            });
        }

        $scope.showDetails = function (Schemeid, Semesterid, Branchid) {
            //alert("scheme : "+Schemeid+" Semester : "+Semesterid+"Branch : "+Branchid +"Academic Year : "+AcademicYearid);
            var AcademicYearId = 0;
           
            //alert(document.getElementById("AcademicYear").value);
            if ($scope.AcademicId == null || $scope.AcademicId == "? undefined:undefined ?")
                AcademicYearId = $scope.AcademicId;
            else
                AcademicYearId = $scope.AcademicId;
            // alert("Academic Year : " + AcademicYearId);
            // alert("scheme : " + Schemeid + " Semester : " + Semesterid + "Branch : " + Branchid + "Academic Year : " + AcademicYearId);
            // var collegecode = document.getElementById("CollegeName").value;
            var collegecode = $scope.college;
            if (collegecode == undefined || collegecode == null) {
                authData.College_Code = $scope.College_Code;
            }else{
                authData.College_Code = collegecode;
            }

           // AppSettings.College_Code = collegecode;
            authData.SchemeId = Schemeid;
            authData.SemesterId = Semesterid;
            authData.BranchId = Branchid;
            authData.AcdYrID = AcademicYearId;
            $state.go('Dashboard.Admission.StudentRegList');
            //$state.go('Admission.StudentRegList');
        }
        $scope.loadAdmissionData = function () {

            $scope.showData = 0;
            $scope.LoadImg = true;
            $scope.StudentDetailsFound = false;
            $scope.CollegeDetailsFound = false;
            // alert(document.getElementById("CollegeName"));
            var collegecode = document.getElementById("CollegeName").value;
            // alert(collegecode);
            // alert("in load admission" + collegecode);
            if ($scope.CollegeID !== 0) {
                $scope.college = $scope.College_Code;
            }
            else {
                if ($scope.college === null)
                    $scope.college = "001";
                else
                    $scope.college = collegecode;
            }

            //var AcademicYear = document.getElementById("AcademicYear").value;
            //if (AcademicYear !== undefined && AcademicYear !== null) {
            //    if (AcademicYear.includes("number")) {
            //        AcademicYear = AcademicYear.
            //    }
            //}
            // $scope.$digest();
            // var AcademicYear = $scope.AcademicId;
            var scope = angular.element(document.getElementById("AcademicYear")).scope();
            var AcademicYear = scope.AcademicId;

            // var AcademicYear = ;
            //if (AcademicYear === "" || AcademicYear.includes("undefined"))
            //    AcademicYear = 0;
            // alert(AcademicYear);


          

            if ($scope.college === "") {
                $scope.college = collegecode;
            }

           

            var hodData = AdmissionService.GetDataForAdmissionDashboard(AppSettings.LoggedUserId, $scope.college, AcademicYear);
            hodData.then(function (hodData, status, headers, config, error) {
                
                $scope.hodData = hodData;
                $scope.LoadImg = false;
                $scope.StudentDetailsFound = true;
                $scope.CollegeDetailsFound = true;
                // alert(hodData.Table2[0].AcademicId);
              
                $scope.Academic = hodData.Table1[0].AcademicId;
                //  $scope.$digest();
            }, function (error) {
                alert("data not found");
            });
        }
    });
});







