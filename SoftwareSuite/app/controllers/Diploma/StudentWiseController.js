define(['app'], function (app) {
    app.factory('Excel', function ($window) {
        //alert("hello");
        var uri = 'data:application/vnd.ms-excel;base64,',
            template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
            base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
            format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
        return {
            tableToExcel: function (tableId, worksheetName) {               
                var table = $(tableId),
                    ctx = { worksheet: worksheetName, table: table.html() },
                    href = uri + base64(format(template, ctx));
                return href;
            }
        };
    });
    app.controller("StudentWiseController", function ($scope, $state, $stateParams, AppSettings, StudentWiseService, Excel, $timeout) {
      

        //get schemes data for dropdown

        var SCHEMESEMINFO = StudentWiseService.GetSchemeDataForResults();
        $scope.pin = "";
        SCHEMESEMINFO.then(function (data) {
            if (data.length > 0) {
              
               $scope.schemeinfo = data;
            
            }
        }, function (error) {
            alert(error);
        });

       

        $scope.showData = 0
        // end Scheme Sem branch Information
        $scope.OpenProgram = function (GridFormToOpen) {
            var strroute = '' + GridFormToOpen;
            $state.go(strroute);
        }
        $scope.OpenDashboard = function () {
            $state.go('Dashboard')
        }
        $scope.GoToHome = function () {
            $state.go('Exam');
        }
        $scope.MyProfile = function () {
            alert("ok");
        }
        $scope.MyCollege = function () {
            if (AppSettings.TypeFlag == 'C') {
                $state.go('Exam.CollegeInfo');
            }
            else {
                alert("This is not College user");
                return;
            }
        }
        $scope.OpenCollegeSemWiseReport = function () {
            $state.go('CollegeSemWise');
        }
        $scope.OpenBranchWiseReport = function () {
            alert("Branch Wise");
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
        //$scope.$on('onUnload', function(e) {
        //    delete $localStorage.authorizationData;
        //    sessionStorage.loggedIn = "no";
        //});
        //drilldowncoding

        // sem and examtype dropdown  
        $scope.loadSemExamTypes = function (SchemeId) {
            $scope.seminfo = [];
            $scope.examtypeinfo = [];
            $scope.pin = "";
            var SemExamInfo = StudentWiseService.GetExamTypeForResults(SchemeId);
            SemExamInfo.then(function (data) {
               
                if (data.Table.length > 0) {
                   
                    // $scope.schemeinfo = data[0].schemeInfo;
                    //$scope.seminfo = data[0].semInfo;
                    $scope.seminfo = data.Table;
                    $scope.examtypeinfo = data.Table1;
                    // $scope.branchinfo = data[0].branchInfo;
                }

            }, function (error) {                
                alert(error);
            });
        }

        $scope.loadExamTypes = function () {
            $scope.showData = 0;
            // Branch Information
            $scope.pin = "";
            var ExamTypeInfo = StudentWiseService.GetExamTypeInfo($scope.scheme, $scope.sem);

            ExamTypeInfo.then(function (data) {
                if (data.length > 0) {
                    $scope.examtypeinfo = data[0].typeInfo;
                }

            }, function (error) {
                alert(error);
            });
            // Branch Information
        }
        $scope.PrintStudentResult = function () {

            var divName = "idtoDivPrintAdmin";
            var divToPrint = document.getElementById(divName);
            $("#studentresult1").hide();
            var domClone = divToPrint.cloneNode(true);
            var $printSection = document.getElementById("printSection");
            if (!$printSection) {
                var $printSection = document.createElement("div");
                $printSection.id = "printSection";
                document.body.appendChild($printSection);
            }
            $printSection.innerHTML = "";
            $printSection.appendChild(domClone);
            window.print();
            document.body.removeChild($printSection);
            $("#studentresult1").show();

        };

        $scope.hidePreviousResult = function () {
            $scope.showData = 0;
        }
    
        $scope.Submit = function () {
            if (($scope.scheme == undefined) || ($scope.scheme == "0") || ($scope.scheme == "")) {
                alert("Select Scheme");
                return false;
            }
            if (($scope.sem == undefined) || ($scope.sem == "0") || ($scope.sem == "")) {
                alert("Select Sem");
                return false;
            }
            if (($scope.examtype == undefined) || ($scope.examtype == "0") || ($scope.examtype == "")) {
                alert("Select Exam");
                return false;
            }

            if (($scope.Pin == undefined) || $scope.Pin == "") {
                alert("Enter Student Pin ");
                return false;
            }
            $scope.showData = 0
            $scope.LoadImg = true;
            $scope.ResultNotFound = false;
            $scope.ResultFound = false;           
            if ($scope.scheme == '5') {
                $scope.ResultFound = false;
                $scope.ResultNotFound = true;
                $scope.LoadImg = false;

                //var ResultData = StudentWiseService.GetStudentWiseReport($scope.sem, $scope.Pin, $scope.scheme, $scope.examtype);
                //ResultData.then(function (data) {                      
                //    if (data.length > 0) {                       
                //        if (data[0].studentWiseReport.length > 0) {
                //            $scope.showData = 1;
                //            $scope.ResultFound = true;
                //            $scope.ResultNotFound = false;
                //            // alert(data[0].studentWiseReport[0].Semister);
                //            $scope.semyear = data[0].studentWiseReport[0].Semister;
                //            $scope.pin = $scope.Pin;
                //            $scope.StudentWiseReportData = data[0].studentWiseReport;
                //            if ($scope.examtype == '10') {
                //                //var total = 0;
                //                //for (var i = 0; i < data[0].studentWiseReport.length; i++) {
                //                //    var subjecttotal = data[0].studentWiseReport[i].SubjectTotal;
                //                //    total += subjecttotal;
                //                //}

                //                $scope.getTotalMarks = data[0].studentSubjectTotal[0].TotalSubjects;
                //                //var total = 0;
                //                //for (var i = 0; i < data[0].studentWiseReport.length; i++) {
                //                //    var GradePoint = data[0].studentWiseReport[i].GradePoint;
                //                //    total += GradePoint;
                //                //}
                //                $scope.getTotalGradePoints = data[0].studentSubjectTotal[0].TotalSubjects;
                //                //var Result = "Pass";
                //                //for (var i = 0; i < data[0].studentWiseReport.length; i++) {
                //                //    if (data[0].studentWiseReport[i].Result == "F") {
                //                //        Result = "Promoted";
                //                //        break;
                //                //    }
                //                //}
                //                //var total = 0;
                //                //for (var i = 0; i < data[0].studentWiseReport.length; i++) {
                //                //    var Maxcredits = data[0].studentWiseReport[i].MaxCredits;
                //                //    total += Maxcredits;
                //                //}
                //                $scope.getMaxCredits = data[0].studentSubjectTotal[0].TotalCredits;
                //                //var total = 0;
                //                //for (var i = 0; i < data[0].studentWiseReport.length; i++) {
                //                //    var credits = data[0].studentWiseReport[i].CreditsGained;
                //                //    total += credits;
                //                //}
                //                $scope.getCreditsGained = data[0].studentSubjectTotal[0].TotalCreditsEarned;
                //                //var total = 0;
                //                //for (var i = 0; i < data[0].studentWiseReport.length; i++) {
                //                //    var gradepoints = data[0].studentWiseReport[i].GradePoint * data[0].studentWiseReport[i].CreditsGained;
                //                //    total += gradepoints;
                //                //}
                //                $scope.getGradePointsGained = data[0].studentSubjectTotal[0].TotalGrades;
                //                $scope.LoadImg = false;
                //                $scope.Result = data[0].studentSubjectTotal[0].Result;

                //                $scope.SGPA = data[0].studentSGPACGPAInfo[0].SGPA;
                //                $scope.CGPA = data[0].studentSGPACGPAInfo[0].CGPA;
                //                $scope.studentInfo = data[0].studentInfo[0];
                //                $scope.BranchSubjectGradeInfo = data[0].branchSubjectGradeInfo;
                //                $scope.studentSubjectTotal = data[0].studentSubjectTotal;
                //                $scope.AcadamicYear = data[0].studentSubjectTotal[0].AcadamicYear;
                //                $scope.CgpaTotalPoints = data[0].studentSGPACGPAInfo[0].CgpaTotalPoints;
                //                $scope.CgpaTotalCredits = data[0].studentSGPACGPAInfo[0].CgpaTotalCredits;
                //            }
                //            else {
                //                $scope.AcadamicYear = data[0].studentSubjectTotal[0].AcadamicYear;
                //                $scope.studentInfo = data[0].studentInfo[0];
                //                $scope.LoadImg = false;
                //            }


                //        }
                //        else {
                //            $scope.ResultFound = false;
                //            $scope.ResultNotFound = true;
                //            $scope.LoadImg = false;
                //        }
                //    }
                //    else {
                //        $scope.ResultFound = false;
                //        $scope.ResultNotFound = true;
                //        $scope.LoadImg = false;                       
                //    }
                //    }, function (error) {
                //        alert(error);
                //    });             

            }
            else  {
               
                    var ResultData = StudentWiseService.GetOldStudentWiseReport($scope.sem, $scope.Pin, $scope.scheme, $scope.examtype);
                    ResultData.then(function (data) {                      
                        if ( data.Table.length > 0) {
                           
                            $scope.showData = 1;
                            $scope.ResultFound = true;
                            $scope.ResultNotFound = false;
                           
                            $scope.LoadImg = false;
                            if (data.Table.length > 0) {
                                $scope.semyear = data.Table[0].Semister;
                            } else {
                                $scope.semyear = data.Table1[0].Sem;
                            }
                            $scope.pin = $scope.Pin;
                            $scope.scheme = $scope.scheme;                           
                            $scope.AcadamicYear = data.Table1[0].AcadamicYear == null || data.Table1[0].AcadamicYear == 'undefined' ? false : data.Table1[0].AcadamicYear;
                            $scope.studentInfo = data.Table1[0];
                            $scope.StudentWiseReportData = data.Table;
                            if (data.Table.length > 0) {
                                $scope.Result = data.Table[0].Result;
                                $scope.Total = data.Table[0].Total;
                            }
                            else {
                                $scope.Result = "";
                                $scope.Total = "";
                            }

                        }
                  
                        else {
                            $scope.ResultFound = false;
                            $scope.ResultNotFound = true;
                            $scope.LoadImg = false;
                        }
                
                        
                      
                                       
                    }, function (error) {
                        alert(error);
                    });

            }
           
         
        }
        
        $scope.DownloadPdfResult = function (tableid) {
            // alert(tableid + ":" + AppSettings.ExportToPdfUrl);
            //alert("pdf : " + tableid);
            html2canvas($('#idtoDivPrintAdmin'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("Table.pdf");
                }
            });
        }

        $scope.DownloadExcelResult = function (tableId) {
            // alert(AppSettings.ExportToExcelUrl);
            //alert("Excel : "+tableId);
            var exportHref = Excel.tableToExcel(tableId, 'StudentResult');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.PrintDashBoard = function ()
        {
            var divName = "";
            if ($scope.adminuser == true) {
                divName = "idtoDivPrintAdmin";
            }
            else {
                divName = "DivIdToPrint";
            }

            var divToPrint = document.getElementById(divName);

            var newWin = window.open('', 'Print-Window');

            newWin.document.open();

            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');

            newWin.document.close();

            setTimeout(function () { newWin.close(); }, 10);
        } 
    });
});












