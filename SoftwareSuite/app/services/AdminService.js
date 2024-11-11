define(['app'], function (app) {
    app.service("AdminService", function (DataAccessService) {

        this.GetUserTypes = function () {
            var promise = DataAccessService.getDataWithPara('api/AdminService/getUserTypes');
            return promise;
        }
        this.GetActiveBranches = function () {
            var promise = DataAccessService.getDataWithPara('api/AdminService/getActiveBranches');
            return promise;
        }
        
        this.getNotifications = function () {
            var promise = DataAccessService.getDataWithPara('api/AdminService/getNotifications');
            return promise;
        }

        this.getUserType = function () {
            var promise = DataAccessService.getDataWithPara('api/AdminService/getUserType');
            return promise;
        }

        
        this.GetNotificationByUser = function (usertypeid) {
            var paramObj = { "usertypeid": usertypeid };
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetNotificationByUser', paramObj);
            return promise;
        }
        
        this.GetModulesbyRole = function (usertypeid) {
            var paramObj = { "usertypeid": usertypeid };
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetModulesbyRole', paramObj);
            return promise;
        }

        this.GetAllModulesbyRole = function (usertypeid) {
            var paramObj = { "usertypeid": usertypeid };
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetAllModulesbyRole', paramObj);
            return promise;
        }

        this.GetSubModulesbyRole = function (usertypeid, moduleid) {
            var paramObj = { "usertypeid": usertypeid, "moduleid": moduleid };
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetSubModulesbyRole', paramObj);
            return promise;
        }


        this.SetSubModuleInactive = function (usertypeid,moduleId,SubModuleId,IsActive) {
            var paramObj = {
                "usertypeid": usertypeid, "moduleId": moduleId,"SubModuleId":SubModuleId,"IsActive":IsActive 
            };
            var promise = DataAccessService.getDataWithPara('api/AdminService/SetSubModuleInactive', paramObj);
            return promise;
        }

        this.SetModuleInactive = function (usertypeid, moduleId, IsActive) {
            var paramObj = {
                "usertypeid": usertypeid, "moduleId": moduleId, "IsActive": IsActive
            };
            var promise = DataAccessService.getDataWithPara('api/AdminService/SetModuleInactive', paramObj);
            return promise;
        }

        
        

        this.NotificationInactive = function (Id) {
            var paramObj = { "Id": Id };
            var promise = DataAccessService.getDataWithPara('api/AdminService/NotificationInactive', paramObj);
            return promise;
        }

        this.PostNotification = function (NotificationData) {           
            var paramObj = { "Json": NotificationData };
            var promise = DataAccessService.postData('api/AdminService/PostNotification', paramObj);
            return promise;
        };


        this.getUserIdStatus = function (UserName) {
            var paramObj = { "UserName": UserName };
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetUserIdStatus', paramObj);
            return promise;
        }
        this.switchUserState = function (UserId) {
            var paramObj = { "UserId": UserId };
            var promise = DataAccessService.getDataWithPara('api/AdminService/SwitchUserStatus', paramObj);
            return promise;
        }

        this.getcollegesList = function () {         
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetCollegesList');
            return promise;
        }

        this.getActiveSchemes = function () {
            var promise = DataAccessService.getDataWithPara('api/AdminService/getActiveSchemes');
            return promise;
        }

        

        this.getBranchesList = function (CollegeCode) {
           
            var paramObj = {"CollegeCode": CollegeCode};
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetBranchList', paramObj);
            return promise;
        }
        
        
        this.CreateUser = function (UserTypeId, UserName, UserPassword, ExpiryDate, FirstName, LastName, Address1, EmailId, CellNo, CollegeId, BranchId) {
            var paramObject = {
                "UserTypeId": UserTypeId, "UserName": UserName, "UserPassword": UserPassword, "ExpiryDate": ExpiryDate, "FirstName": FirstName, "LastName": LastName,
                "Address1": Address1, "EmailId": EmailId, "CellNo": CellNo, "CollegeId": CollegeId, "BranchId": BranchId
            };
            console.log(paramObject);
            var promise = DataAccessService.postData('api/AdminService/createUser', paramObject);
            return promise;
        };


        this.GetSemester = function (UserType) {

            var paramObj = { "UserType": UserType };
            var promise = DataAccessService.getDataWithPara('api/AdminService/GetSemester', paramObj);
            return promise;
        }

        this.SaveScheamdata = function (UserType, json) {
            var paramObject = { "UserType":UserType, "json": json };
            return DataAccessService.postData('api/AdminService/SaveScheamdata', paramObject);
        }

    })
})