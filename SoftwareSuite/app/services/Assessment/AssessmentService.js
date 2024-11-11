define(['app'], function (app) {
    app.service("AssessmentService", function (DataAccessService) {
        this.GetCollegesSchemeSemInfo = function (CollegeId) {
            var paramObject = { "CollegeId": CollegeId };
            var promise = DataAccessService.getDataWithPara('Admission/GetCollegesSchemeSemInfo', paramObject);
            return promise;
        };
        this.GetAcademicYearsActive = function () {
            var promise = DataAccessService.getDataAll('Assessment/getAcademicYearsActive');
            return promise;
        };
        
        this.getSchemewiseExams = function (schemeid) {
            var parmObject = { "schemeid": schemeid };
            return DataAccessService.getDataWithPara('Assessment/getSchemeWiseExams', parmObject);
        };

        this.getbranchNameById = function (branchId) {
            var paramObject = { "branchcode": branchId };
            return DataAccessService.getDataWithPara('Assessment/getBranchName', paramObject);
        };

        this.getActiveSemester = function () {
            return DataAccessService.getDataAll('Assessment/getActiveSemester');
        };
        this.getSchemeStatus = function () {
            return DataAccessService.getDataAll('Assessment/getSchemeStatus');
        };
        this.getSemistersSetData = function () {
            return DataAccessService.getDataAll('Assessment/getSemistersSetData');
        };
        
        this.getStatisticalReports = function (semisterId) {
            var paramObject = { "semid": semisterId };
            return DataAccessService.getDataWithPara('Assessment/GetStatisticalReports', paramObject);
        };
        this.getExamType = function (schemeid) {
            var paramObject = { "schemeid": schemeid };
            return DataAccessService.getDataWithPara('Assessment/getExamType', paramObject);
        };
        this.getActiveExamTypes = function () {

            return DataAccessService.getDataAll('Assessment/getActiveExamTypes');
        };
        this.getCollegeAssessmentReports = function (collegecode, examtypeid) {

            var paramObject = { "collegecode": collegecode, "examtypeid": examtypeid };
            return DataAccessService.getDataWithPara('Assessment/getCollegeAssessmentReports', paramObject);
        };
        this.getAdminReports = function (examtypeid) {
            var paramObject = { "examtypeid": examtypeid };
            return DataAccessService.getDataWithPara('AssessmentReports/getAdminReports', paramObject);
        };
        this.getAdminReportsCollege = function (examtypeid, collegecode) {
            var paramObject = { "examtypeid": examtypeid, "collegecode": collegecode };
            return DataAccessService.getDataWithPara('AssessmentReports/getAdminReportsCollege', paramObject);
        };
        this.getAdminBranchReports = function (examtypeid, collegecode, branchid, subid, semid) {
            var paramObject = { "examtypeid": examtypeid, "collegecode": collegecode, "branchid": branchid, "subid": subid, "semid": semid };
            return DataAccessService.getDataWithPara('AssessmentReports/getAdminBranchReports', paramObject);
        };
        this.getStudentReport = function (pin) {
            var paramObject = { "pin": pin };
            return DataAccessService.getDataWithPara('AssessmentReports/getStudentReport', paramObject);
        };
     

        this.updateMarks = function (marksList) {
            return DataAccessService.postData('AssessmentReports/UpdateStudentMarks', marksList);
        };
        
     
    });

});