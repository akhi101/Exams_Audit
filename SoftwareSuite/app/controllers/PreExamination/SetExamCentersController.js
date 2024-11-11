define(['app'], function (app) {
    app.controller("SetExamCentersController", function ($scope, $http, $localStorage, $state, AppSettings, PreExaminationService,AdmissionService,MarksEntryService, Excel, $timeout) {
      //  $scope.loading = true;
        $scope.edit = true;
        $scope.update = false;
        var examCenters = [];
        var tempId = [];

        $scope.changeExamCentre = function () {
            $scope.result = false;
            $scope.GetCollegeCenters = [];
            $scope.edit = true;
            $scope.update = false;
            examCenters = [];
        }

        $scope.Submit = function () {
            $scope.loading = true;
            $scope.result = false;
            var getAdminExamCenters = PreExaminationService.getAdminExamCentersList($scope.Examyear);
            getAdminExamCenters.then(function (response) {
                //  console.log(response);
                // var response = JSON.parse(response);
                console.log(response);
                if (response.Table.length > 0) {
                    $scope.GetCollegeCenters = response.Table;
                  
                    var finalarr = [];
                    finalarr = response.Table;
                    // var getExamCenters = PreExaminationService.getExaminationCentersList($scope.Student.id, $scope.current_schemeid, $scope.currentAcademicYear, $scope.currentYearMonth);
                    var getExamCenters = PreExaminationService.getExaminationCentersList($scope.Examyear);
                    getExamCenters.then(function (response) {
                        //  var response = JSON.parse(res);
                        console.log(response);
                        var ExamCentertable = [];
                        $scope.ExamCenters = response.Table;
                        $scope.loading = false;
                        $scope.result = true;
                        ExamCentertable = response.Table
                        if (response.Table.length > 0) {
                            finalarr = finalarr.map((obj) => {
                                ExamCentertable.forEach(function (val) {
                                    if (val.ExaminationCenterId === obj.ExaminationCenterId) {
                                        obj.ExaminationCenterName = val.ExaminationCenter;
                                    }

                                })
                                return obj;
                            })
                        }
                        //   console.log(finalarr);
                        $scope.finalarr = finalarr;
                    },
                     function (error) {
                         alert("error while loading semesters");
                         var err = JSON.parse(error);
                         //    console.log(err.Message);          
                     });

                    $scope.loading = false;
                    $scope.Noresult = false;
                    $scope.result = true;
                    //} else {
                    //    $scope.loading = false;
                    //    $scope.Noresult = true;
                    //    $scope.result = false;
                    //}
                } else {
                    alert("No Data Found");
                 //   $scope.edit = false;
                    $scope.loading = false;
                    $scope.Noresult = true;
                    $scope.result = false;
                }
            },
            function (error) {
                alert("error while loading Data");
                var err = JSON.parse(error);
                //    console.log(err.Message);
             //   $scope.edit = false;
                $scope.loading = false;
                $scope.Noresult = true;
                $scope.result = false;
            });
            //}
        }

        
        var getExamMonthYears = PreExaminationService.getExaminationMonthYear();
        getExamMonthYears.then(function (response) {
            console.log(response);
            
            $scope.getExamYearMonth = response.Table;
        },
                function (error) {
                    alert("error while loading semesters");
                    var err = JSON.parse(error);
                    //    console.log(err.Message);          
                });



     

        $scope.changeStuentType = function (studentTypeId) {
     //       alert(studentTypeId);
            $scope.studentTypeId = studentTypeId
           
        }
        
        var getSchemes = AdmissionService.GetSchemes();
        getSchemes.then(function (response) {
            console.log(response);
            $scope.getSchemes = response.Table;
        },
                function (error) {
                    alert("error while loading semesters");
                    var err = JSON.parse(error);
                    //    console.log(err.Message);          
                });

        //var GetExamYearMonth = PreExaminationService.getExamYearMonth();
        //GetExamYearMonth.then(function (response) {
        //    console.log(response);
        //    $scope.getExamYearMonth = response.Table;
        //},
        //        function (error) {
        //            alert("error while loading semesters");
        //            var err = JSON.parse(error);
        //            //    console.log(err.Message);          
        //        });
        


        var getAcademicYears = AdmissionService.GetAcademicYears();
        getAcademicYears.then(function (response) {
            console.log(response);
            $scope.getAcademicyears = response.Table;
        },
                function (error) {
                    alert("error while loading semesters");
                    var err = JSON.parse(error);
                    //    console.log(err.Message);          
                });
        


    


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



       
       
        $scope.changeCenter = function (data) {
           // console.log(data)
           
          
                if (examCenters.length == '0') {
                  //  console.log(data.internal)
                    var marksdata = $scope.pushData($scope.Examyear,data.CollegeId, data.ExaminationCenterId);
                    examCenters.push(marksdata);


                } else if (examCenters.length > 0) {
                    tempId = [];
                    examCenters.map((obj) => {
                        if (obj.CollegeId == data.CollegeId) {
                            obj.Examyear = $scope.Examyear;
                            obj.CollegeId = data.CollegeId;
                            obj.ExaminationCenterId = data.ExaminationCenterId;                          
                            tempId.push(data.CollegeId);
                        }
                       else if (obj.CollegeId != data.CollegeId && !tempId.includes(data.CollegeId)) {
                          //  console.log(data.internal)
                            var marksdata = $scope.pushData($scope.Examyear, data.CollegeId, data.ExaminationCenterId);
                           
                            tempId.push(data.CollegeId);
                            examCenters.push(marksdata);

                        }
                    });          
                
            }
                console.log(examCenters);

        }

      
        $scope.pushData = function (Examyear,CollegeId, ExaminationCenterId) {
            return {
                Examyear:Examyear,
                CollegeId: CollegeId,
                ExaminationCenterId: ExaminationCenterId,
            };
        }

        $scope.editDetaila = function () {
            $scope.edit = false;
            $scope.update = true;
           examCenters = [];
        }
        $scope.SaveData = function () {
            console.log(examCenters);
            var setExaminationCenters = PreExaminationService.setExamCenters(JSON.stringify(examCenters));
            setExaminationCenters.then(function (response) {               
                alert("Data Saved Successfully");
                $scope.edit = true;
                $scope.update = false;                
                var getAdminExamCenters = PreExaminationService.getAdminExamCentersList($scope.Examyear);
                getAdminExamCenters.then(function (response) {
                    //  console.log(response);
                    // var response = JSON.parse(response);
                    console.log(response);
                    //if (response.Table.length > 0) {
                    $scope.GetCollegeCenters = response.Table;
                    var finalarr = [];
                    finalarr = response.Table;
                    // var getExamCenters = PreExaminationService.getExaminationCentersList($scope.Student.id, $scope.current_schemeid, $scope.currentAcademicYear, $scope.currentYearMonth);
                    var getExamCenters = PreExaminationService.getExaminationCentersList($scope.Examyear);
                    getExamCenters.then(function (response) {
                        //  var response = JSON.parse(res);
                        console.log(response);
                        var ExamCentertable = [];
                        $scope.ExamCenters = response.Table;
                        $scope.result = true;
                        ExamCentertable = response.Table
                        if (response.Table.length > 0) {
                            finalarr = finalarr.map((obj) => {
                                ExamCentertable.forEach(function (val) {
                                    if (val.ExaminationCenterId === obj.ExaminationCenterId) {
                                        obj.ExaminationCenterName = val.ExaminationCenter;
                                    }

                                })
                                return obj;
                            })
                        }
                        //   console.log(finalarr);
                        $scope.finalarr = finalarr;
                    },
                     function (error) {
                         alert("error while loading semesters");
                         var err = JSON.parse(error);
                         //    console.log(err.Message);          
                     });

                    $scope.loading = false;
                    $scope.Noresult = false;
                    $scope.result = true;
                    //} else {
                    //    $scope.loading = false;
                    //    $scope.Noresult = true;
                    //    $scope.result = false;
                    //}

                },
                function (error) {
                    alert("error while loading semesters");
                    var err = JSON.parse(error);
                    //    console.log(err.Message);
                    $scope.loading = false;
                    $scope.Noresult = true;
                    $scope.result = false;
                });
               
            },
            function (error) {
                alert("error while Saving Data");
                var err = JSON.parse(error);
                //    console.log(err.Message);
                $scope.loading = false;
                $scope.Noresult = true;
                $scope.result = false;
            });
        }
        $scope.DownloadtoExcel = function (tableid) {
            var exportHref = Excel.tableToExcel(tableid, 'stdentDetails');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "ExaminationCenters.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();

            }, 100);
        }

        $scope.LoadExamTypes = function () {
            alert($scope.currentYearMonth);
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