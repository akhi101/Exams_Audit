define(['app'], function (app) {
    app.controller("CheckOnRoleController", function ($scope, $http, $localStorage, $state, AppSettings, PreExaminationService, AssessmentService, MarksEntryService, Excel, $timeout) {

        var authData = $localStorage.authorizationData;
        $scope.userTypeId = authData.SystemUserTypeId
        $scope.Student = {};
        $scope.Student.id = '';
        $scope.isShowResults = false;
        $scope.isPrincipalTable = false;
        $scope.isHodTable = false;
        $scope.isAllchecked = false;
        var PaymentStudentList = [];

        Array.prototype.remByVal = function (val) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] === val) {
                    this.splice(i, 1);
                    i--;
                }
            }
            return this;
        }


        $scope.selectEntity = function (data) {
            $scope.isTopChecked = false;
            if (data != null) {
                if (!PaymentStudentList.includes(data)) {
                    PaymentStudentList.push(data);
                }
                else if (PaymentStudentList.includes(data)) {
                    PaymentStudentList.remByVal(data);
                }

            }

        };


        $scope.selectAll = function () {
            if ($scope.isAllchecked == true) {

                $scope.isAllchecked = false;
                PaymentStudentList = [];
            }
            else if ($scope.isAllchecked == false) {
                $scope.isAllchecked = true;
                PaymentStudentList = [];
                for (var i = 0; i < $scope.ExamPayment.length; i++) {
                    PaymentStudentList.push($scope.ExamPayment[i].Pin);
                }
            }
            $scope.PaymentStudentList = PaymentStudentList;

        };

        $scope.approve = function () {
            if (PaymentStudentList.length > 0) {
                console.log(PaymentStudentList);
            }
            else
                alert("Select any student to pay");
        }

        $scope.GetOnrole = function (data) {
            $localStorage.authorizationData.onROleDetails = data;
            $state.go("Dashboard")
        }
        //if (authData.SystemUserTypeId == 1) {
        //  //  authData.SystemUserTypeId = 2;
        //}

        var ApprovalData = $localStorage.authorizationData.onRoleDetails;

        var ApprovalList = PreExaminationService.getApprovalList(ApprovalData.type, authData.SystemUserTypeId, ApprovalData.CollegeCode, ApprovalData.BranchCode, ApprovalData.Semester);
        ApprovalList.then(function (response) {
            $scope.tableData = [];
          //  $scope.ExamPayment = response;
            $scope.tableData.push({ rows: response, cols: Object.keys(response[0]) });
           //   $scope.ExamPayment = Usersdata;
            if (response.length > 0) {
                $scope.isShowResults = true;
                if (authData.SystemUserTypeId == 2) {
                    $scope.isPrincipalTable = true;
                }
                else if (authData.SystemUserTypeId == 3) {
                    $scope.isHodTable = true;
                }
            }
            else {
                alert("No Data Found")
            }

        },
        function (error) {
            alert("error while loading Academic Year");
        });

        if (authData.SystemUserTypeId == 3 || authData.SystemUserTypeId == 2 || authData.SystemUserTypeId == 1) {
            $scope.isShowTags = true;
            if (authData.SystemUserTypeId == 2) {
                $scope.isPrincipalTable = true;
            }
            else if (authData.SystemUserTypeId == 3) {
                $scope.isHodTable = true;
            } else {
                $scope.isPrincipalTable = true;
                $scope.userTypeId = 1;
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

                    var getAdmissionsubmod = PreExaminationService.getApproveExamFee($scope.userTypeId, $scope.Student.id);
                    getAdmissionsubmod.then(function (Usersdata) {

                       
                        if (Usersdata.length > 0) {
                            $scope.isShowResults = true;
                          //  $scope.tableData.push({ rows: Usersdata, cols: Object.keys(Usersdata) });
                            $scope.ExamPayment = Usersdata;
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
        }

        $scope.DownloadtoExcel = function (tableid) {
            var exportHref = Excel.tableToExcel(tableid, 'stdentDetails');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "StudentFeepaymentReport.xls";
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
                a.download = "StudentFeepaymentReport.xls";
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