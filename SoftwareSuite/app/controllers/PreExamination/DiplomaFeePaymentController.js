define(['app'], function(app) {
    app.controller("DiplomaFeePaymentController", function($scope, $http, $localStorage, $state, PreExaminationService, MarksEntryService, $uibModal, PaymentService) {
        $scope.secondClick = false;
        $scope.allItemsSelected = true;
        $scope.allItemsSelectedthing = true;
        $scope.PaymentDetails = {}
        var authData = $localStorage.authorizationData;
        $scope.SystemUserTypeId = authData.SystemUserTypeId;
        $scope.LoadImg = false;
        $scope.DetailsFound = false;
        $scope.DetailsNotFound = false;
        $scope.DetainedDetailsFound = false;
        $scope.isBackLog = false;
        $scope.PaymentStudent = [];
        var LoadExamTypeBysem = MarksEntryService.getStudentType();
        $scope.Sems = "";
        $scope.feepayingpins = "";
        $scope.noteChallan = false;
        LoadExamTypeBysem.then(function(response) {
            if (response.Table.length > 0) {
                $scope.StudentType = response.Table;
            } else {
                $scope.StudentType = [];
                alert("No Student found on this Record");
            }
        },
            function(error) {
                alert("error while loading Student Types");
                console.log(error);
            });

        PaymentStudent = [];
        PaymentStudentList = [];

        $scope.selectAll = function() {

            $scope.allItemsSelectedthing = true;


            if ($scope.isAllchecked == true) {

                $scope.isAllchecked = false;


                for (var i = 0; i < $scope.studentSubData.length; i++) {
                    $scope.studentSubData[i].isChecked = false;
                }

                dataPay.Amount = 0;
                $scope.studentLateFee = 0;
                $scope.studentTatkalFee = 0;

                PaymentStudent = [];
                PaymentStudentList = [];
                $scope.studentSubjectsSelected = PaymentStudentList.length;
                $scope.studentTotalFee = ($scope.studentExamFee) * (PaymentStudentList.length);
                if ($scope.FinalstudentTatkalFee > 0) {
                    $scope.studentTotalFee = $scope.FinalstudentTatkalFee;
                }
            }
            else if ($scope.isAllchecked == false) {
                $scope.isAllchecked = true;
                for (var i = 0; i < $scope.studentSubData.length; i++) {
                    $scope.studentSubData[i].isChecked = true;
                }
                PaymentStudentList = [];
                PaymentStudent = [];


                for (var i = 0; i < $scope.studentSubData.length; i++) {
                    dataPay = {};

                    dataPay.Semester = $scope.studentSubData[i].Semester;
                    dataPay.Sem = $scope.studentSubData[i].Semester;

                    if ($scope.PaymentDetails.length > 0) {
                        dataPay.Amount = 0;
                        $scope.studentLateFee = 0;
                        $scope.studentTatkalFee = 0;
                        $scope.ExamFee = 0;
                        for (var j = 0; j < $scope.PaymentDetails.length; j++) {
                            //dataPay.Amount += $scope.PaymentDetails[i].ExamFee + $scope.PaymentDetails[i].LateFee + $scope.PaymentDetails[i].TatkalFee;
                            $scope.studentLateFee += $scope.PaymentDetails[j].LateFee;
                            $scope.studentTatkalFee += $scope.PaymentDetails[j].TatkalFee;
                            $scope.ExamFee += $scope.PaymentDetails[j].ExamFee;
                        }

                        //$scope.studentTotalFee = dataPay.Amount;
                    }
                    //dataPay.Amount = $scope.studentTotalFee;
                    if ($scope.studentTatkalFee > 0) {
                        dataPay.Amount = $scope.studentTatkalFee + $scope.ExamFee;
                        dataPay.isTatkalFee = true;
                        $scope.studentTotalFee = dataPay.Amount;
                    }
                    else if ($scope.studentLateFee > 0) {
                        dataPay.Amount = $scope.studentLateFee + $scope.ExamFee;
                        dataPay.isLateFee = false;
                        $scope.studentTotalFee = dataPay.Amount;
                    }
                    else {
                        dataPay.Amount = $scope.ExamFee;
                        $scope.studentTotalFee = dataPay.Amount;
                        dataPay.isLateFee = false;
                        dataPay.isTatkalFee = false;
                    }


                    //$scope.studentSubjectsSelected;
                    dataPay.Pin = $scope.studPin;
                    //for (var j = 0; j < $scope.ActiveSemesters.length; j++) {
                    //    if ($scope.ActiveSemesters[j].semid == $scope.current_schemeid) {
                    //        dataPay.Semester = $scope.ActiveSemesters[j].semester;
                    //        Semester = $scope.ActiveSemesters[j].semester;
                    //        break;
                    //    }
                    //}
                    dataPay.UserName = authData.userName;
                    dataPay.StudentTypeId = $scope.Studtype;
                    dataPay.Name = $scope.studentName;

                    //if ($scope.studentSubData[i].Condonation !== undefined)
                    //    dataPay.Condonation = $scope.ExamPayment[i].Condonation;
                    //else
                    //    dataPay.Condonation = "";
                    //dataPay.Penality = $scope.ExamPayment[i].Penality;
                    //dataPay.Tatkal = $scope.ExamPayment[i].Tatkal;
                    //dataPay.CertificateFee = $scope.ExamPayment[i].CertificateFee;
                    PaymentStudent.push(dataPay);
                    PaymentStudentList.push($scope.studentSubData[i].Semester);
                    $scope.studentSubjectsSelected = PaymentStudentList.length;
                    //$scope.studentTotalFee = ($scope.studentExamFee) * (PaymentStudentList.length);
                    //if ($scope.FinalstudentTatkalFee > 0) {
                    //    //$scope.studentTotalFee = $scope.FinalstudentTatkalFee;
                    //}
                }
            }
            $scope.studentSubjectsSelected = PaymentStudentList.length;
            $scope.PaymentStudent = PaymentStudent;
            $scope.PaymentStudentList = PaymentStudentList;

        };


        $scope.selectVAll = function() {

            if ($scope.allItemsSelectedthing == true) {


                if ($scope.isAllchecked == false) {

                    $scope.isAllchecked = false;


                    for (var i = 0; i < $scope.studentSubData.length; i++) {
                        $scope.studentSubData[i].isChecked = false;
                    }
                    PaymentStudent = [];
                    PaymentStudentList = [];
                }
                else if ($scope.isAllchecked == true) {
                    $scope.isAllchecked = true;
                    for (var i = 0; i < $scope.studentSubData.length; i++) {
                        $scope.studentSubData[i].isChecked = true;
                    }
                    PaymentStudentList = [];
                    PaymentStudent = [];
                    for (var i = 0; i < $scope.studentSubData.length; i++) {
                        dataPay = {};

                        dataPay.Semester = $scope.studentSubData[i].Semester;
                        dataPay.Sem = $scope.studentSubData[i].Semester;

                        if ($scope.PaymentDetails.length > 0) {
                            dataPay.Amount = 0;
                            $scope.studentLateFee = 0;
                            $scope.studentTatkalFee = 0;
                            $scope.ExamFee = 0;
                            for (var j = 0; j < $scope.PaymentDetails.length; j++) {
                                //dataPay.Amount += $scope.PaymentDetails[i].ExamFee + $scope.PaymentDetails[i].LateFee + $scope.PaymentDetails[i].TatkalFee;
                                $scope.studentLateFee += $scope.PaymentDetails[j].LateFee;
                                $scope.studentTatkalFee += $scope.PaymentDetails[j].TatkalFee;
                                $scope.ExamFee += $scope.PaymentDetails[j].ExamFee;
                            }

                            //$scope.studentTotalFee = dataPay.Amount;
                        }
                        //dataPay.Amount = $scope.studentTotalFee;
                        if ($scope.studentTatkalFee > 0) {
                            dataPay.Amount = $scope.studentTatkalFee + $scope.ExamFee;
                            dataPay.isTatkalFee = true;
                            $scope.studentTotalFee = dataPay.Amount;
                        }
                        else if ($scope.studentLateFee > 0) {
                            dataPay.Amount = $scope.studentLateFee + $scope.ExamFee;
                            dataPay.isLateFee = false;
                            $scope.studentTotalFee = dataPay.Amount;
                        }
                        else {
                            dataPay.Amount = $scope.ExamFee;
                            $scope.studentTotalFee = dataPay.Amount;
                            dataPay.isLateFee = false;
                            dataPay.isTatkalFee = false;
                        }
                        //$scope.studentSubjectsSelected;
                        dataPay.Pin = $scope.studPin;
                        //for (var j = 0; j < $scope.ActiveSemesters.length; j++) {
                        //    if ($scope.ActiveSemesters[j].semid == $scope.current_schemeid) {
                        //        dataPay.Semester = $scope.ActiveSemesters[j].semester;
                        //        Semester = $scope.ActiveSemesters[j].semester;
                        //        break;
                        //    }
                        //}
                        dataPay.UserName = authData.userName;
                        dataPay.StudentTypeId = $scope.Studtype;
                        dataPay.Name = $scope.studentName;
                        //if ($scope.studentSubData[i].Condonation !== undefined)
                        //    dataPay.Condonation = $scope.ExamPayment[i].Condonation;
                        //else
                        //    dataPay.Condonation = "";
                        //dataPay.Penality = $scope.ExamPayment[i].Penality;
                        //dataPay.Tatkal = $scope.ExamPayment[i].Tatkal;
                        //dataPay.CertificateFee = $scope.ExamPayment[i].CertificateFee;
                        PaymentStudent.push(dataPay);
                        PaymentStudentList.push($scope.studentSubData[i].Semester);


                        //for (var i = 0 ; i < PaymentStudent.length; i++) {
                        //    $scope.studentTotalFee = 0;
                        //    $scope.studentTotalFee +=  PaymentStudent.Amount;
                        //}
                    }
                }

            }
            if ($scope.allItemsSelectedthing == false) {
                $scope.allItemsSelectedthing = true;
            }
            $scope.PaymentStudent = PaymentStudent;
            $scope.PaymentStudentList = PaymentStudentList;

        };


        $scope.selectEntity = function(data) {
            if (data != null) {
                if (!PaymentStudentList.includes(data.Semester)) {
                    dataPay = {};
                    dataPay.Semester = data.Semester;
                    dataPay.Sem = data.Semester;

                    if ($scope.PaymentDetails.length > 0) {
                        for (var i = 0; i < $scope.PaymentDetails.length; i++) {
                            if ($scope.PaymentDetails[i].Semester == data.Semester) {
                                $scope.studentLateFee += $scope.PaymentDetails[i].LateFee;
                                $scope.studentTatkalFee += $scope.PaymentDetails[i].TatkalFee;
                                $scope.ExamFee += $scope.PaymentDetails[i].ExamFee;
                            }
                        }
                    }

                    //dataPay.Amount = $scope.studentTotalFee;
                    if ($scope.studentTatkalFee > 0) {
                        dataPay.Amount = $scope.studentTatkalFee + $scope.ExamFee;
                        dataPay.isTatkalFee = true;
                        $scope.studentTotalFee = dataPay.Amount;
                    }
                    else if ($scope.studentLateFee > 0) {
                        dataPay.Amount = $scope.studentLateFee + $scope.ExamFee;
                        dataPay.isLateFee = false;
                        $scope.studentTotalFee = dataPay.Amount;
                    }
                    else {
                        dataPay.Amount = $scope.ExamFee;
                        $scope.studentTotalFee = dataPay.Amount;
                        dataPay.isLateFee = false;
                        dataPay.isTatkalFee = false;
                    }

                    //dataPay.Amount = data.Total;
                    dataPay.Name = $scope.studentName;
                    //dataPay.StudentContact = data.StudentContact;
                    //for (var j = 0; j < $scope.ActiveSemesters.length; j++) {
                    //    if ($scope.ActiveSemesters[j].semid == $scope.current_schemeid) {
                    //        dataPay.Semester = $scope.ActiveSemesters[j].semester;
                    //        Semester = $scope.ActiveSemesters[j].semester;
                    //        break;
                    //    }
                    //}
                    dataPay.UserName = authData.userName;
                    dataPay.StudentTypeId = $scope.Studtype;
                    dataPay.Name = $scope.studentName;

                    //dataPay.ExamFee = data.ExamFee;

                    //if (data.Condonation !== undefined)
                    //    dataPay.Condonation = data.Condonation;
                    //else
                    //    dataPay.Condonation = "";
                    //dataPay.Penality = data.Penality;
                    //dataPay.Tatkal = data.Tatkal;
                    //dataPay.CertificateFee = data.CertificateFee;

                    dataPay.Pin = $scope.studPin;
                    PaymentStudent.push(dataPay);
                    PaymentStudentList.push(data.Semester);
                    $scope.studentSubjectsSelected = PaymentStudentList.length;
                    //$scope.studentTotalFee = (($scope.studentExamFee) * (PaymentStudentList.length)) + $scope.FinalstudentLateFee;
                    //if ($scope.FinalstudentTatkalFee > 0) {
                    //    $scope.studentTotalFee = $scope.FinalstudentTatkalFee;
                    //}
                }
                else if (PaymentStudentList.includes(data.Semester)) {
                    PaymentStudentList.remByVal(data.Semester);
                    PaymentStudent.remElementByVal(data.Semester);
                    $scope.studentSubjectsSelected = PaymentStudentList.length;
                    if ($scope.PaymentDetails.length > 0) {
                        for (var i = 0; i < $scope.PaymentDetails.length; i++) {
                            if ($scope.PaymentDetails[i].Semester == data.Semester) {
                                $scope.studentLateFee -= $scope.PaymentDetails[i].LateFee;
                                $scope.studentTatkalFee -= $scope.PaymentDetails[i].TatkalFee;
                                $scope.ExamFee -= $scope.PaymentDetails[i].ExamFee;
                            }
                        }
                    }

                    //dataPay.Amount = $scope.studentTotalFee;
                    if ($scope.studentTatkalFee > 0) {
                        dataPay.Amount = $scope.studentTatkalFee + $scope.ExamFee;
                        dataPay.isTatkalFee = true;
                        $scope.studentTotalFee = dataPay.Amount;
                    }
                    else if ($scope.studentLateFee > 0) {
                        dataPay.Amount = $scope.studentLateFee + $scope.ExamFee;
                        dataPay.isLateFee = false;
                        $scope.studentTotalFee = dataPay.Amount;
                    }
                    else {
                        dataPay.Amount = $scope.ExamFee;
                        $scope.studentTotalFee = dataPay.Amount;
                        dataPay.isLateFee = false;
                        dataPay.isTatkalFee = false;
                    }

                    //$scope.studentTotalFee = (($scope.studentExamFee) * (PaymentStudentList.length)) + $scope.FinalstudentLateFee;
                    if (PaymentStudentList.length == 0) {
                        $scope.allItemsSelectedthing = false;
                        $scope.studentTotalFee = 0;
                    }
                    if ($scope.FinalstudentTatkalFee > 0) {
                        $scope.studentTotalFee = $scope.FinalstudentTatkalFee;
                    }
                }
                $scope.PaymentStudent = PaymentStudent;
                $scope.PaymentStudentList = PaymentStudentList;
            }


        };

        Array.prototype.remByVal = function(val) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] === val) {
                    this.splice(i, 1);
                    break;
                }
            }
            return this;
        }

        Array.prototype.remElementByVal = function(val) {
            for (var i = 0; i < this.length; i++) {
                if (this[i].Semester === val) {
                    this.splice(i, 1);
                    break;
                }
            }
            return this;
        }

        $scope.changedVal = function() {
            $scope.DetailsFound = false;
        }

        $scope.getStudentDetails = function(Pin, Studtype) {

            $scope.Studtype = Studtype;
            if (Studtype == "1") {
                $scope.isBackLog = false;
                if ($scope.Student === undefined) {
                    alert("Slelect Student Type");
                    return;
                }

                else if ($scope.Student !== undefined) {
                    $scope.LoadImg = true;
                    $scope.DetailsFound = false;
                    $scope.DetainedDetailsFound = false;
                    var StudtypeId = parseInt(Studtype);
                    if ($scope.SystemUserTypeId == 1) {
                        var studentDetails = PreExaminationService.GetStudentFeePaymentDetailsforAdmin(Pin, StudtypeId, $scope.SystemUserTypeId);
                        studentDetails.then(function(res) {
                            $scope.LoadImg = false;
                            $scope.DetailsNotFound = false;
                            $scope.DetailsFound = true;
                            var resp = JSON.parse(res);
                            if (resp.Table !== undefined && resp.Table[0].ResponceCode == '200') {

                                $scope.studPin = resp.Table1[0].Pin;
                                $scope.Photo = resp.Table1[0].Photo;
                                $scope.studentScheme = resp.Table1[0].Scheme;
                                $scope.studentBranch = resp.Table1[0].Branch;
                                $scope.studentSem = resp.Table1[0].Semester;
                                $scope.studentName = resp.Table1[0].Name;
                                $scope.studentFatherName = resp.Table1[0].FatherName;
                                $scope.studentattendance = resp.Table1[0].PresemptiveAttendance;
                                $scope.studentPaymentStatus = resp.Table1[0].Status;
                                $scope.FinalstudentTatkalFee = resp.Table1[0].TatkalFee;
                                $scope.studentTatkalFee = resp.Table1[0].TatkalFee;
                                $scope.studentCondonationFee = resp.Table1[0].Condonation;
                                $scope.FinalstudentLateFee = resp.Table1[0].LateFee;
                                $scope.studentLateFee = resp.Table1[0].LateFee;
                                $scope.studentExamFee = resp.Table1[0].ExamFee;
                                $scope.studentTotalFee = resp.Table1[0].TotalFee;
                                $scope.studentCertificateFee = resp.Table1[0].CertificateFee;
                                $scope.studentExamCenterName = resp.Table1[0].ExaminationCenter;
                                $scope.studentExamCenterCode = resp.Table1[0].ExaminationCenterCode;
                                $scope.studentSubData = resp.Table2;
                                $scope.DetailsNotFound = false;
                                $scope.LoadImg = false;

                            }
                            else if (resp.Table[0].ResponceCode == '400') {
                                $scope.LoadImg = false;
                                $scope.DetailsFound = false;
                                $scope.DetailsNotFound = false;
                                $scope.DetainedDetailsFound = true;
                                $scope.DetainedDetailsFoundWithData = resp.Table[0].ResponceDescription;
                            }
                            else {

                                $scope.LoadImg = false;
                                $scope.DetailsFound = false;
                                $scope.DetailsNotFound = true;
                                alert(resp.Table[0].ResponceDescription);

                            }

                        }, function(err) {
                            $scope.LoadImg = false;
                            $scope.DetailsFound = false;
                            $scope.DetailsNotFound = false;

                        });


                    } else {

                        var studentDetails = PreExaminationService.GetStudentFeePaymentDetails(Pin, StudtypeId);
                        studentDetails.then(function(res) {

                            $scope.LoadImg = false;
                            $scope.DetailsNotFound = false;
                            $scope.DetailsFound = true;
                            var resp = JSON.parse(res);
                            if (resp.Table !== undefined && resp.Table[0].ResponceCode == '200') {

                                $scope.studPin = resp.Table1[0].Pin;
                                $scope.Photo = resp.Table1[0].Photo;
                                $scope.studentScheme = resp.Table1[0].Scheme;
                                $scope.studentBranch = resp.Table1[0].Branch;
                                $scope.studentSem = resp.Table1[0].Semester;
                                $scope.studentName = resp.Table1[0].Name;
                                $scope.studentFatherName = resp.Table1[0].FatherName;
                                $scope.studentattendance = resp.Table1[0].PresemptiveAttendance;
                                $scope.studentPaymentStatus = resp.Table1[0].Status;
                                $scope.FinalstudentTatkalFee = resp.Table1[0].TatkalFee;
                                $scope.studentTatkalFee = resp.Table1[0].TatkalFee;
                                $scope.studentCondonationFee = resp.Table1[0].Condonation;
                                $scope.FinalstudentLateFee = resp.Table1[0].LateFee;
                                $scope.studentLateFee = resp.Table1[0].LateFee;
                                $scope.studentExamFee = resp.Table1[0].ExamFee;
                                $scope.studentTotalFee = resp.Table1[0].TotalFee;
                                $scope.studentCertificateFee = resp.Table1[0].CertificateFee;
                                $scope.studentExamCenterName = resp.Table1[0].ExaminationCenter;
                                $scope.studentExamCenterCode = resp.Table1[0].ExaminationCenterCode;
                                $scope.studentSubData = resp.Table2;
                                $scope.DetailsNotFound = false;
                                $scope.LoadImg = false;

                            }
                            else if (resp.Table[0].ResponceCode == '400') {
                                $scope.LoadImg = false;
                                $scope.DetailsFound = false;
                                $scope.DetailsNotFound = false;
                                $scope.DetainedDetailsFound = true;
                                $scope.DetainedDetailsFoundWithData = resp.Table[0].ResponceDescription;
                            }
                            else {

                                $scope.LoadImg = false;
                                $scope.DetailsFound = false;
                                $scope.DetailsNotFound = true;
                                alert(resp.Table[0].ResponceDescription);

                            }

                        }, function(err) {
                            $scope.LoadImg = false;
                            $scope.DetailsFound = false;
                            $scope.DetailsNotFound = false;

                        });



                    }


                }
            }

            else if (Studtype == "2") {
                $scope.isAllchecked = true;
                $scope.isBackLog = true;
                if ($scope.Student === undefined) {
                    alert("Slelect Student Type");
                    return;
                }

                else if ($scope.Student !== undefined) {
                    $scope.LoadImg = true;
                    $scope.DetailsFound = false;
                    $scope.DetainedDetailsFound = false;
                    var StudtypeId = parseInt(Studtype);
                    var studentDetails = PreExaminationService.GetStudentFeePaymentDetails(Pin, StudtypeId);
                    studentDetails.then(function(resp) {



                        $scope.LoadImg = false;
                        $scope.DetailsNotFound = false;
                        $scope.DetailsFound = true;
                        resp = JSON.parse(resp);
                        //var resp = {
                        //    "Table": [{
                        //        "ResponceCode": "200",
                        //        "ResponceDescription": "Required Data Pushed."
                        //    }],
                        //    "Table1": [{
                   
                        //        "Pin": "18001-M-005",
                        //        "Name": "BHUPATHI JAY VARSHITH",
                        //        "FatherName": "BHUPATHI RAMESH",
                        //        "Scheme": "C18",
                        //        "CollegeCode": "001  ",
                        //        "Branch": "MECHANICAL ENGINEERING",
                        //        "ExaminationCenter": "027 - GOVT POLYTECHNIC,NALGONDA",
                        //        "PresemptiveAttendance": 87,
                        //        "ExamFee": 550,
                        //        "Condonation": 0,
                        //        "LateFee": 0,
                        //        "TatkalFee": 0,
                        //        "TotalFee": 550,
                        //        "CertificateFee": 600
                        //    }],
                        //    "Table2": [{
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301l",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301k",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        },{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //    }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        },
                        //        {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }, {
                        //        "Semester": "3SEM",
                        //        "Subjects": [{
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }, {
                        //            "SubjectCode": "18M-301F",
                        //            "SubjectName": "APPLIED ENGINEERING MATHEMATICS",
                        //            "ExamDate": "",
                        //            "ExamTime": ""
                        //        }]
                        //    }]
                        //};
                        var newDataTable = [];

                        if (resp.Table !== undefined && resp.Table[0].ResponceCode == '200') {

                            //var newDataTable = [];
                            var SemesterList = [];

                            for (var i = 0; i < resp.Table2.length; i++) {
                                if (!SemesterList.includes(resp.Table2[i].yrsem)) {
                                    SemesterList.push(resp.Table2[i].yrsem);
                                    var temp = {};
                                    temp.Subjects = [];
                                    //var Subjects = {};

                                    temp.Semester = resp.Table2[i].yrsem;
                                    temp.scheme = resp.Table2[i].scheme;
                                    temp.isChecked = true;

                                    for (var j = 0; j < resp.Table2.length; j++) {
                                        var Subject = {};
                                        if (resp.Table2[j].yrsem == temp.Semester) {
                                            Subject.SubjectCode = resp.Table2[j].sub1;
                                            Subject.SubjectName = resp.Table2[j].subname;
                                            Subject.ExamDate = resp.Table2[j].ExamDate;
                                            Subject.ExamTime = resp.Table2[j].ExamTime;
                                            temp.Subjects.push(Subject);
                                        }
                                    }

                                    newDataTable.push(temp);
                                }
                            }




                            $scope.studPin = resp.Table1[0].Pin;
                            $scope.Photo = resp.Table1[0].Photo;
                            $scope.studentScheme = resp.Table1[0].Scheme;
                            $scope.studentBranch = resp.Table1[0].Branch;
                            $scope.studentSem = resp.Table1[0].Semester;
                            $scope.studentName = resp.Table1[0].Name;
                            $scope.studentFatherName = resp.Table1[0].FatherName;
                            $scope.studentSubjectsSelected = resp.Table1[0].TotalSemSelect;
                            $scope.studentPaymentStatus = resp.Table1[0].Status;
                            $scope.FinalstudentTatkalFee = resp.Table1[0].TatkalFee;
                            $scope.studentTatkalFee = resp.Table1[0].TatkalFee;
                            $scope.studentCondonationFee = resp.Table1[0].Condonation;
                            $scope.FinalstudentLateFee = resp.Table1[0].LateFee;
                            $scope.studentLateFee = resp.Table1[0].LateFee;
                            $scope.studentExamFee = resp.Table1[0].ExamFee;
                            $scope.studentTotalFee = resp.Table1[0].TotalFee;
                            $scope.studentCertificateFee = resp.Table1[0].CertificateFee;
                            $scope.studentExamCenterName = resp.Table1[0].ExaminationCenter;
                            $scope.studentExamCenterCode = resp.Table1[0].ExaminationCenterCode;
                            $scope.PaymentDetails = resp.Table3;
                            $scope.studentSubData = newDataTable;
                            $scope.DetailsNotFound = false;
                            $scope.LoadImg = false;
                            $scope.selectVAll();

                        }
                        else if (resp.Table !== undefined && resp.Table[0].ResponceCode == '400') {
                            $scope.LoadImg = false;
                            $scope.DetailsFound = false;
                            $scope.DetailsNotFound = false;
                            $scope.DetainedDetailsFound = true;
                            $scope.DetainedDetailsFoundWithData = resp.Table[0].ResponceDescription;
                        }
                        else {
                            $scope.LoadImg = false;
                            $scope.DetailsFound = false;
                            $scope.DetailsNotFound = true;
                            alert(resp.Table[0].ResponceDescription);

                        }

                    }, function(err) {
                        $scope.LoadImg = false;
                        $scope.DetailsFound = false;
                        $scope.DetailsNotFound = false;

                    });
                }
            }
        }



        $scope.Proceedtopayfine = function() {
            var College_Code = "admin";
            $scope.noteChallan = false;
            $scope.secondClick = false;
            //var marchantid = "TSSBTET"; // live
            var marchantid = "TSSBTET"; // test
            try {
                College_Code = authData.College_Code == null ? "admin" : authData.College_Code;
            } catch (err) {
            }
            var addInfo1 = College_Code;
            var addInfo3 = $scope.feepayingpins;
            var addInfo4 = "NA"//$scope.loadedScheme.Scheme;
            var addInfo5 = "NA";//Semester;
            var addInfo6 = "SINGLE"//PaymentType;
            var addInfo7 = "NA";
            var amount = "";
            if ($scope.Student.id == 1) {
                addInfo5 = "REGULAR";
                amount = $scope.AmountDB.toFixed(2) == null || $scope.AmountDB.toFixed(2) == "" ? $scope.studentTotalFee.toFixed(2) : $scope.AmountDB.toFixed(2);

            }
            else if ($scope.Student.id == 2) {
                amount = $scope.FinalAmountDB.toFixed(2);
                addInfo5 = "BACKLOG";
                addInfo4 = $scope.Sems;
            }
            var subMarchantid = "TSDOFP";
            $localStorage.PaymentGatewayResponse = {};
            redirecturl = {
                redirecturl: "Dashboard.PreExamination.DiplomaFeePayment"
            }
            $localStorage.PaymentGatewayResponse = redirecturl;

            var location = window.location.origin;

            PreExaminationService.RequestLog(marchantid, subMarchantid, addInfo1, addInfo3, addInfo4, addInfo5, addInfo6, addInfo7, $scope.challan, amount, "studentType", "json");
            // $localStorage.assessment.redirecturl = 'Dashboard.AssessmentDashboard.Assessment.TheorySubjectList';
            //localhost:65321/Payment/BulkBillResponse
            //'ss.sbtet.telangana.gov.in/API/Payment/BulkBillResponse'
            var proceedfinePayment = PaymentService.getHashValue(location + "/Payment/BulkBillResponse", marchantid, subMarchantid, addInfo1, addInfo3, addInfo4, addInfo5, addInfo6, addInfo7, $scope.challan, amount);
            proceedfinePayment.then(function(resp) {
                if (resp != "" && resp != undefined) {
                    // var req = "https://uat.billdesk.com/pgidsk/PGIMerchantPayment?msg="
                    var req = "https://pgi.billdesk.com/pgidsk/PGIMerchantPayment?msg=" + resp   // live url
                    //var req = "https://uat.billdesk.com/pgidsk/PGIMerchantPayment?msg=KALYANTEST|429|NA|2|NA|NA|NA|INR|NA|R|kalyantest|NA|NA|F|8850062965|test-developer@candere.com|187|NA|NA|NA|NA|http://127.0.0.1/candere_repo/scheme/billdesk/response|9F4E06C08698DA6338428E2A36141826468E8E31C83F3B814F831AE6D6D27CFD";
                    //   var req = "https://pgi.billdesk.com/pgidsk/PGIMerchantPayment?msg=" + resp // test url
                    window.location.replace(req);
                }
            }, function(err) {
                $scope.noteChallan = false;
                $scope.secondClick = true;
                console.log(err);
            });
        }

        $scope.closeModal = function() {
            $scope.modalInstance.close();
        }




        $scope.notedChallan = function() {
            if ($scope.noteChallan == true) {
                $scope.noteChallan = false;
            } else {
                $scope.noteChallan = true;
            }
        }


        $scope.isStudentContact = true;
        $scope.payNow = function() {


            if ($scope.Student === undefined) {
                alert("Select Student Type");
            }
            else if ($scope.Student !== undefined) {

                if ($scope.Student.id == 1) {

                    var PaymentStudent = [];
                    datajson = {};
                    datajson.Pin = $scope.studPin;
                    datajson.Semester = $scope.studentSubData[0].Semester + "(" + $scope.studentSubData[0].studentScheme + ")";
                    datajson.Amount = $scope.studentTotalFee.toFixed(2);
                    datajson.UserName = null;
                    datajson.StudentTypeId = $scope.Student.id;
                    PaymentStudent.push(datajson);
                }

                //UserName":"M_001","Pin":"123","Semester":"3sem(c18)","Amount":"100","IpAddress":""
                if (PaymentStudent === undefined) {
                    PaymentStudent = $scope.PaymentStudent;
                }
                var getChallanDetails = PreExaminationService.getChanllanForExamFee(JSON.stringify(PaymentStudent).toString());
                getChallanDetails.then(function(Usersdata) {
                    $scope.userJsonData = JSON.parse(Usersdata);

                    if ($scope.Student.id == 1) {
                        if ($scope.userJsonData.Table.length > 0) {
                            if ($scope.userJsonData.Table[0].ResponceCode === undefined && $scope.userJsonData.Table[0].ChalanaNumber !== undefined) {
                                var feepayingpins = "";
                                var Sems = "";
                                $scope.StudentVerData = $scope.userJsonData.Table;

                                if ($scope.userJsonData.Table.length > 0 && $scope.Student.id == 2) {
                                    for (var i = 0; i < $scope.userJsonData.Table.length; i++) {
                                        Sems += $scope.userJsonData.Table[i].Semester + ",";
                                    }
                                    Sems = Sems.substring(0, Sems.length - 1);
                                    $scope.Sems = Sems;
                                }


                                if ($scope.userJsonData.Table[0].StudentContact != null && $scope.userJsonData.Table[0].StudentContact != "") {
                                    $scope.UpdatedContactDetail = $scope.userJsonData.Table[0].StudentContact;
                                    if ($scope.UpdatedContactDetail.toString().length == 10) {
                                        $scope.isStudentContact = false;
                                        $scope.StundetContactbuttonDesable = true;
                                    }
                                }
                                else {
                                    $scope.UpdatedContactDetail = 0
                                }
                                if ($scope.userJsonData.Table.length > 0 && $scope.Student.id == 1) {
                                    //feepayingpins = $scope.userJsonData.Table[0].Pin;
                                    for (let i = 0; i < $scope.userJsonData.Table.length; i++) {
                                        feepayingpins += $scope.userJsonData.Table[i].Pin + ",";
                                    }
                                }
                                else if ($scope.userJsonData.Table.length > 0 && $scope.Student.id == 2) {
                                    feepayingpins = $scope.userJsonData.Table[0].Pin + ",";
                                }

                                feepayingpins = feepayingpins.substring(0, feepayingpins.length - 1);
                                $scope.feepayingpins = feepayingpins;

                                $scope.challan = $scope.userJsonData.Table[0].ChalanaNumber;
                                $scope.AmountDB = $scope.userJsonData.Table1[0].TotalAmount;
                                $scope.FinalAmountDB = $scope.userJsonData.Table1[0].TotalAmount;
                                if ($scope.userJsonData.Table.length > 0) {
                                    $scope.modalInstance = $uibModal.open({
                                        templateUrl: "/app/views/DiplomaFeePaymentPopup.html",
                                        size: 'xlg',
                                        scope: $scope,
                                        windowClass: 'modal-fit-att',
                                    });
                                }
                                else {
                                    $scope.noPaymentelected = false;
                                }
                            }
                            else if ($scope.userJsonData.Table[0].ResponceCode !== undefined) {
                                alert($scope.userJsonData.Table[0].Message);
                            }
                        }
                    }
                    else if ($scope.Student.id == 2) {
                        if ($scope.userJsonData.Table[0].ResponceCode === undefined && $scope.userJsonData.Table1[0].ChalanaNumber !== undefined) {
                            var feepayingpins = "";
                            var Sems = "";
                            $scope.StudentVerData = $scope.userJsonData.Table1;

                            if ($scope.userJsonData.Table.length > 0 && $scope.Student.id == 2) {
                                for (var i = 0; i < $scope.userJsonData.Table.length; i++) {
                                    Sems += $scope.userJsonData.Table[i].Semester + ",";
                                }
                                Sems = Sems.substring(0, Sems.length - 1);
                                $scope.Sems = Sems;
                            }


                            if ($scope.userJsonData.Table[0].StudentContact != null && $scope.userJsonData.Table[0].StudentContact != "") {
                                $scope.UpdatedContactDetail = $scope.userJsonData.Table[0].StudentContact;
                                if ($scope.UpdatedContactDetail.toString().length == 10) {
                                    $scope.isStudentContact = false;
                                    $scope.StundetContactbuttonDesable = true;
                                }
                            }
                            else {
                                $scope.UpdatedContactDetail = 0
                            }
                            if ($scope.userJsonData.Table.length > 0 && $scope.Student.id == 1) {
                                //feepayingpins = $scope.userJsonData.Table[0].Pin;
                                for (let i = 0; i < $scope.userJsonData.Table.length; i++) {
                                    feepayingpins += $scope.userJsonData.Table[i].Pin + ",";
                                }
                            }
                            else if ($scope.userJsonData.Table.length > 0 && $scope.Student.id == 2) {
                                feepayingpins = $scope.userJsonData.Table[0].Pin + ",";
                            }

                            feepayingpins = feepayingpins.substring(0, feepayingpins.length - 1);
                            $scope.feepayingpins = feepayingpins;

                            $scope.challan = $scope.userJsonData.Table1[0].ChalanaNumber;
                            $scope.AmountDB = $scope.userJsonData.Table2[0].TotalAmount;
                            $scope.FinalAmountDB = $scope.userJsonData.Table2[0].TotalAmount;
                            if ($scope.userJsonData.Table.length > 0) {
                                $scope.modalInstance = $uibModal.open({
                                    templateUrl: "/app/views/DiplomaFeePaymentPopup.html",
                                    size: 'xlg',
                                    scope: $scope,
                                    windowClass: 'modal-fit-att',
                                });
                            }
                            else {
                                $scope.noPaymentelected = false;
                            }
                        }
                        else if ($scope.userJsonData.Table[0].ResponceCode !== undefined) {
                            alert($scope.userJsonData.Table[0].Message);
                        }
                    }
                    else {
                        alert("Some thing Went Wrong");
                    }
                }, function(err) {
                    $scope.isShowResults = false;
                    console.log(err);
                });

            }




            //backdrop: 'static',

            //});
            //else
            //    alert("Select any student to pay");
        }


        $scope.UpdateStudentContact = function(num) {

            if (num.toString().length == 10) {
                var UpdatedNumber = PreExaminationService.UpdateStudentsContact($scope.feepayingpins, num);
                UpdatedNumber.then(function(res) {
                    if (res[0].Response == '200') {
                        $scope.UpdatedContactDetail = num;
                        $scope.isStudentContact = false;
                    }

                }, function(err) {
                });
            }
        };


        $scope.StundetContactbuttonDesable = false;

        $scope.changeNumber = function(num) {
            if (num.toString().length == 10) {
                $scope.StundetContactbuttonDesable = true;
                $scope.isStudentContact = true;
            }
            else {
                $scope.StundetContactbuttonDesable = false;
                $scope.isStudentContact = false;
            }
        };

    });
});