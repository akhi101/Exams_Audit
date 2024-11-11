define(['app'], function (app) {
    app.service("SessionalsService", function (DataAccessService) {       
        this.getSemSubjects = function (semid, branchCode, loadedScheme, subType, examTypeid,collegecode) {
            var paramObject = { "semid": semid, "branchCode": branchCode, "loadedScheme": loadedScheme, "subType": subType, "examTypeid": examTypeid, "collegecode": collegecode };
            return DataAccessService.getDataWithPara('Assessment/getSemSubjects', paramObject);

        }
    });
});
