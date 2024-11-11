define(['app'], function (app) {
    app.controller("PreExamReportsController", function ($scope, $http, $localStorage, $state, AppSettings, PreExaminationService, AssessmentService, MarksEntryService, Excel, $timeout) {

        $scope.ExamType = 'Regular';
        var authData = $localStorage.authorizationData;

        $scope.userId = authData.SysUserID
        $scope.Student = {};
        $scope.Student.id = '';
        $scope.isShowResults = false;
        $scope.isShowTags = false;
        $scope.access1 = false;
        $scope.isPrincipalTable = false;
        $scope.isAdminTable = false;
        $scope.isHodTable = false;
        $scope.userTypeId = authData.SystemUserTypeId;

        $scope.GetOnrole = function (data) {
            data.type = 1;
            $localStorage.authorizationData.onRoleDetails = data;
            $state.go("Dashboard.PreExamination.CheckOnRole");
        };

        $scope.GetFeeEligibleList = function (data) {
            data.type = 2;
            $localStorage.authorizationData.onRoleDetails = data;
            $state.go("Dashboard.PreExamination.CheckOnRole");
        };

        $scope.GetFeePayedList = function (data) {
            data.type = 3;
            $localStorage.authorizationData.onRoleDetails = data;
            $state.go("Dashboard.PreExamination.CheckOnRole");
        }
        $scope.GetFeeNotPayedList = function (data) {
            data.type = 4;
            $localStorage.authorizationData.onRoleDetails = data;
            $state.go("Dashboard.PreExamination.CheckOnRole");
        }

        $scope.CondonationList = function (data) {
            data.type = 5;
            $localStorage.authorizationData.onRoleDetails = data;
            $state.go("Dashboard.PreExamination.CheckOnRole");
        }

        $scope.DetainedList = function (data) {
            data.type = 6;
            $localStorage.authorizationData.onRoleDetails = data;
            $state.go("Dashboard.PreExamination.CheckOnRole");
        }


        if (authData.SystemUserTypeId == 3 || authData.SystemUserTypeId == 2) {
            $scope.isShowTags = true;
            if (authData.SystemUserTypeId == 2) {
                $scope.isPrincipalTable = true;
            }
			else if (authData.SystemUserTypeId == 3) {
                  $scope.isHodTable = true;
            }
            else if (authData.SystemUserTypeId == 1) {
                $scope.isAdminTable = false;
            }

            var LoadExamTypeBysem = MarksEntryService.getStudentType();
            LoadExamTypeBysem.then(function (response) {
                if (response.Table.length > 0) {
                    $scope.StudentType = response.Table;
                } else {
                    $scope.StudentType = [];
                    alert("No Student found on this Record");
                }
            },
            function (error) {
                alert("error while loading Student Types");
                console.log(error);
            });


            var AcademicYearsActive = AssessmentService.GetAcademicYearsActive();
            AcademicYearsActive.then(function (response) {
                $scope.years = response.Table[0];

            },
            function (error) {
                alert("error while loading Academic Year");
            });
            $scope.showPaymentDetails = function () {

                if ($scope.Student.id !== undefined && $scope.Student.id != '') {

                    var getAdmissionsubmod = PreExaminationService.getApproveExamFee(authData.SysUserID, $scope.Student.id);
                    getAdmissionsubmod.then(function (response) {


                        if (response.length > 0) {
                            var Onroll = 0
                            var FeeEligible = 0;
                           
                            var FeePayed = 0;
                            var FeeNotPayed = 0;
                            var Condonation = 0;
                          

                            for (var i = 0; i < response.length; i++) {
                                if (response[i].Onroll != null)
                                    Onroll = Onroll + response[i].Onroll;
                                if (response[i].FeeEligible != null)
                                    FeeEligible = FeeEligible + response[i].FeeEligible;

                                if (response[i].FeePayed != null)
                                    FeePayed = FeePayed + response[i].FeePayed;

                                if (response[i].FeeNotPayed != null)
                                    FeeNotPayed = FeeNotPayed + response[i].FeeNotPayed;

                                if (response[i].Condonation != null)
                                    Condonation = Condonation + response[i].Condonation;

                               

                                
                            }
                            $scope.Onroll = Onroll;
                            $scope.FeeEligible = FeeEligible;
                            $scope.FeePayed = FeePayed;
                            $scope.FeeNotPayed = FeeNotPayed;
                            $scope.Condonation = Condonation;
                          
                            $scope.isShowResults = true;
                            $scope.ExamPayment = response;

                            $scope.isShowResults = true;
                            $scope.ExamPayment = response;
                        }
                        else {
                            $scope.isShowResults = false;
                            $scope.AcademicModules = [];
                            alert("No Data Found");
                        }

                    }, function (err) {
                        $scope.isShowResults = false;
                        console.log(err);
                    });
                }
                else {
                    alert("please select required fields");
                }

            }
        } else if (authData.SystemUserTypeId == 1) {
            $scope.isShowTags = true;
            $scope.isAdminTable = true;
            $scope.CollegeCode = localStorage.getItem('CollegeCode');
          
            var LoadExamTypeBysem = MarksEntryService.getStudentType();
            LoadExamTypeBysem.then(function (response) {
                if (response.Table.length > 0) {
                    $scope.StudentType = response.Table;
                } else {
                    $scope.StudentType = [];
                    alert("No Student found on this Record");
                }
            },
            function (error) {
                alert("error while loading Student Types");
                console.log(error);
            });


            var AcademicYearsActive = AssessmentService.GetAcademicYearsActive();
            AcademicYearsActive.then(function (response) {
                $scope.years = response.Table[0];

            },
            function (error) {
                alert("error while loading Academic Year");
            });
            //$scope.showPaymentDetails = function () {

                if ($scope.userId !== undefined && $scope.userId != '') {

                    var getAdmissionsubmod = PreExaminationService.getAdminCollegePreExamReports($scope.userId,$scope.CollegeCode);
                    getAdmissionsubmod.then(function (response) {


                        if (response.length > 0) {

                            var Onroll = 0
                            var FeeEligible = 0;
                            var FeeNotPayed = 0;
                            var FeePayed = 0;
                            var Condonation = 0;
                            var Detained = 0;

                            for (var i = 0; i < response.length; i++) {
                                if (response[i].Onroll != null)
                                    Onroll = Onroll + response[i].Onroll;
                                if (response[i].FeeEligible != null)
                                    FeeEligible = FeeEligible + response[i].FeeEligible;

                                if (response[i].FeePayed != null)
                                    FeePayed = FeePayed + response[i].FeePayed;

                                if (response[i].FeeNotPayed != null)
                                    FeeNotPayed = FeeNotPayed + response[i].FeeNotPayed;

                                if (response[i].Condonation != null)
                                    Condonation = Condonation + response[i].Condonation;
                                if (response[i].Detained != null)
                                    Detained = Detained + response[i].Detained;


                            }
                            $scope.Onroll = Onroll;
                            $scope.FeeEligible = FeeEligible;
                            $scope.FeePayed = FeePayed;
                            $scope.FeeNotPayed = FeeNotPayed;
                            $scope.Condonation = Condonation;
                            $scope.Detained = Detained;
                            $scope.isShowResults = true;
                            $scope.ExamPayment = response;
                        }
                        else {
                            $scope.isShowResults = false;
                            $scope.AcademicModules = [];
                            alert("No Data Found");
                        }

                    }, function (err) {
                        $scope.isShowResults = false;
                        console.log(err);
                    });
                }
                else {
                    alert("please select required fields");
                }

            //}

        }

        $scope.DownloadtoExcel = function (tableid) {
            var exportHref = Excel.tableToExcel(tableid, 'stdentDetails');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "PrincipalFeepaymentReport.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();

            }, 100);
        }

        $scope.DownloadedExcel = function (tableid) {
            var exportHref = Excel.tableToExcel(tableid, 'StudentReports');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "AdminFeepaymentReport.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();

            }, 100);
        }

   
        $scope.DownloadExcel = function (tableid) {
            var exportHref = Excel.tableToExcel(tableid, 'stdentReports');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "HodFeepaymentReport.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();

            }, 100);
        }


    })
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
})