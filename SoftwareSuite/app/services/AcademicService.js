define(['app'], function (app) {
    app.service("AcademicService", function (DataAccessService) {
        this.GetCollegesSchemeSemInfo = function (CollegeId) {
            var paramObject = { "CollegeId": CollegeId };
            var promise = DataAccessService.getDataWithPara('api/Admission/GetCollegesSchemeSemInfo', paramObject);
            return promise;
        }
        this.GetAcademicYearsActive = function () {
            var promise = DataAccessService.getDataAll('api/Assessment/getAcademicYearsActive');
            return promise;
        }
        

        this.GetFacultyData = function (Pin, FeedbackId, Otp) {
           
            var paramObject = {
                "Pin": Pin, "FeedbackId": FeedbackId, "Otp": Otp 
            }         
            var promise = DataAccessService.getDataWithPara('api/PreExamination/getFacultyData', paramObject);
            return promise;
        }

        this.getAttendanceReport = function (Pin) {
            var paramObject = {
                "Pin": Pin
            };
            return DataAccessService.getDataWithPara('api/PreExamination/getAttendanceReport', paramObject);
        };

        this.GenerateOtpForFeedback = function (Pin, FeedbackId) {
            var paramObject = {
                "Pin": Pin, "FeedbackId": FeedbackId
            };
            return DataAccessService.getDataWithPara('api/PreExamination/GenerateOtpForFeedback', paramObject);
        };
         
        this.GetSyllabusCoverage = function (SubjectId, CollegeCode, ShiftId)  {
            var paramObject = {
                "SubjectId": SubjectId,
                "CollegeCode": CollegeCode,
                "ShiftId": ShiftId
            };
            return DataAccessService.getDataWithPara('api/PreExamination/GetSyllabusCoverage', paramObject);
        }

        
        this.getHodSubjectList = function (CollegeCode, CourseBranchId, SchemeId, SemId, ShiftId) {
            var paramObject = { "CollegeCode": CollegeCode, "CourseBranchId": CourseBranchId, "SchemeId": SchemeId, "SemId": SemId, "ShiftId": ShiftId };
            return DataAccessService.getDataWithPara('Academic/getHodSubjectList', paramObject);
        };
        

        this.getSchemes = function () {        
            return DataAccessService.getDataWithPara('Admission/getSchemes');
        };

        this.getSemBySchemes = function (StudentTypeId, SchemeId) {
            var paramObject = { "StudentTypeId": StudentTypeId, "SchemeId": SchemeId };
            return DataAccessService.getDataWithPara('Assessment/getSemByScheme', paramObject);
        };


        this.getSchemewiseExams = function (schemeid) {
            var parmObject = { "schemeid": schemeid };
            return DataAccessService.getDataWithPara('api/Assessment/getSchemeWiseExams', parmObject);
        }

        this.getbranchNameById = function (branchId) {
            var paramObject = { "branchcode": branchId };
            return DataAccessService.getDataWithPara('api/Assessment/getBranchName', paramObject);
        }


        this.getExamType = function (schemeid) {
            var paramObject = { "schemeid": schemeid };
            return DataAccessService.getDataWithPara('api/Assessment/getExamType', paramObject);
        }

        this.getActiveSemester = function () {
            return DataAccessService.getDataAll('Academic/getActiveSemester');
        }
        this.getSchemeStatus = function () {
            return DataAccessService.getDataAll('Academic/getSchemeStatus');
        }
        this.PostSetDates = function (Schemeid, semid, StartDate, EndDate, AcademicYearId) {
            var paramObject = { "Schemeid": Schemeid, "semid": semid, "StartDate": StartDate, "EndDate": EndDate, "AcademicYearId": AcademicYearId };
            var promise = DataAccessService.postData('Academic/PostSetDates', paramObject);
            return promise;
        }
       
        
        this.SetFeedbackData = function (PIN, Collegecode, BranchId, ScheameID, SemId, Json) {
         
            var paramObject = { "PIN": PIN, "Collegecode": Collegecode, "BranchId": BranchId, "ScheameID": ScheameID, "SemId": SemId, "json": Json };
        
            var promise = DataAccessService.postData('api/PreExamination/SetFeedbackData', paramObject);
            return promise;
        }


        this.GetSetSemesterData = function () {
            return DataAccessService.getDataWithPara('Academic/GetSetSemesterData');
        };

        this.GetDescription = function () {
            return DataAccessService.getDataWithPara('Academic/GetDescriptionData');
        };


        this.getFacultyDetails = function (collegecode) {
            var paramObject = { "collegecode": collegecode };
            return DataAccessService.getDataWithPara('Academic/AFFCollegeTecHStaffInfo', paramObject);
        };
        this.GetMappingList = function (CollegeCode, CourseBranchId, SchemeId, SemId, ShiftId) {
            var paramObject = { "CollegeCode": CollegeCode, "CourseBranchId": CourseBranchId, "SchemeId": SchemeId, "SemId": SemId, "ShiftId": ShiftId};
            return DataAccessService.getDataWithPara('Academic/GetFacultyMappingList', paramObject);
        };
        this.SetMappingList = function (CollegeCode, CourseBranchId, ShiftId, data) {
            var paramObject = { "CollegeCode": CollegeCode, "CourseBranchId": CourseBranchId, "ShiftId": ShiftId, "data": data };
            return DataAccessService.getDataWithPara('Academic/SetFacultyMappingList', paramObject);
        };
        this.GenrateOtp = function (FeddBackId, Pin, mobile) {
            var paramObject = {FeddBackId:"FeddBackId", "Pin": Pin, "mobile": mobile };
            return DataAccessService.getDataWithPara('Academic/GenrateOtp', paramObject);
        };

        this.sendOtp = function (Pin) {
            var paramObject = {
                "Pin": Pin
            };
            return DataAccessService.getDataWithPara('api/PreExamination/SendOtpForCertificates', paramObject);
        };


    });
});