define(['app'], function (app) {
    app.service("MarksEntryService", function (DataAccessService) {
        this.PostMarksEntryDates = function (examId, semid, userName, AcademicYearId, fromDate, toDate, fineDate, fineAmount, studenttypeid) {
            var paramObject = { "examId": examId,"semid":semid,"userName":userName,"AcademicYearId":AcademicYearId,"fromDate":fromDate,"toDate":toDate,"fineDate":fineDate,"fine":fineAmount,"Studenttypeid": studenttypeid};
            var promise = DataAccessService.postData('Assessment/PostMarksEntryDates', paramObject);
            return promise;
        },
        this.GetExamType = function (SchemeId) {
            var paramObject = {"schemeid":SchemeId };
            var promise = DataAccessService.getDataWithPara('Assessment/getExamTypes', paramObject);
            return promise;
        }
        this.GetMarksEntryDates = function (AcademicId) {
            var paramObject = { "AcademicId": AcademicId };
            var promise = DataAccessService.getDataWithPara('Assessment/getMarksEntryDates', paramObject);
            return promise;
        },
         this.getDatesFineAmount = function (examid,semid, AcademicId) {
             var paramObject = { "examid": examid, "semid": semid, "Academicid": AcademicId };
             var promise = DataAccessService.getDataWithPara('MarksEntry/getDatesFineAmount', paramObject);
             return promise;
         },
         this.getSubmitStatus = function (collegeCode, branchCode, AcademicId, semId, examtypeId) {
             var paramObject = { "collegeCode": collegeCode, "branchCode": branchCode, "AcademicId": AcademicId, "semId": semId, "examtypeId": examtypeId };
             var promise = DataAccessService.getDataWithPara('MarksEntry/getSubmitStatus', paramObject);
             return promise;
         },
         this.SubmitAllMarksEntered = function (AcademicId, semId, examtypeId) {
             var paramObject = { "AcademicId": AcademicId, "semId": semId, "examtypeId": examtypeId };
             var promise = DataAccessService.postData('MarksEntry/SubmitAllMarksEntered', paramObject);
             return promise;
         },
            this.SubmitMarksEntered = function (collegeCode, branchCode, AcademicId, semId, examtypeId, subId) {
            var paramObject = { "collegeCode": collegeCode, "branchCode": branchCode, "AcademicId": AcademicId, "semId": semId, "examtypeId": examtypeId, "subId": subId };
            var promise = DataAccessService.postData('MarksEntry/SubmitMarksEntered', paramObject);
            return promise;
        },
         this.getStudentType = function () {
             return DataAccessService.getDataAll('Assessment/getStudentType');
         },
         this.GetPresentStudentType = function () {
             return DataAccessService.getDataAll('Assessment/GetPresentStudentType');
         },
         this.getSubjectPinList = function (Academicid, SchemeId, collegecode, semid, branchId, subId, examtypeid) {            
            var paramObject = { "Academicid": Academicid, "SchemeId": SchemeId, "collegecode": collegecode, "semid": semid, "branchId": branchId, "subId": subId,"examtype": examtypeid };
            return DataAccessService.getDataWithPara('MarksEntry/getSubjectPinList', paramObject);
        }, 

        this.getReportSubjectPinList = function (Academicid, SchemeId, collegecode, semid, branchId, subId, examtypeid) {
            var paramObject = { "Academicid": Academicid, "SchemeId": SchemeId, "collegecode": collegecode, "semid": semid, "branchId": branchId, "subId": subId, "examtype": examtypeid };
            return DataAccessService.getDataWithPara('MarksEntry/getReportSubjectPinList', paramObject);
        },
         this.editMarksEntry = function (collegecode, branchcode, semid, examtypeid, subid, pin) {
            var paramObject = { "collegecode": collegecode, "branchcode": branchcode, "semid": semid, "examtypeid": examtybripeid, "subid": subid, "pin": pin};
            return DataAccessService.getDataWithPara('MarksEntry/editMarksEntry', paramObject);
        },
        this.PostStudentMarks = function (examtypeId, MarksList) {          
            var paramObject = { "examtype": examtypeId, "marksdata": MarksList };
            return DataAccessService.postData('MarksEntry/PostSemExamMarks', paramObject);
        },
            this.getPaymentDetails = function (Amount, CollegeCode, BranchCode, SemId, SchemeId, Academicid, examTypeid) {
            var paramObject = { "Amount": Amount, "CollegeCode": CollegeCode, "BranchCode": BranchCode, "SemId": SemId, "SchemeId": SchemeId, "Academicid": Academicid, "examTypeid": examTypeid};
            return DataAccessService.getDataWithPara('MarksEntryFinePayment/getPaymentDetails', paramObject);
        }
        });
    });