define(['app'], function (app) {
    app.service("PaymentService", function (DataAccessService) {
        this.getHashValue = function (url, marchantid, subMarchantid, addInfo1, addInfo3, addInfo4, addInfo5, addInfo6, addInfo7, chalanaNo, amount) {
            var paramObject = { "redirecturl": url, "marchantid": marchantid, "subMarchantid": subMarchantid, "addInfo1": addInfo1, "addInfo3": addInfo3, "addInfo4": addInfo4, "addInfo5": addInfo5, "addInfo6": addInfo6, "addInfo7": addInfo7, "chalanaNo": chalanaNo, "amount": amount };
            return DataAccessService.getDataWithPara('api/BillDesk/getHashValue', paramObject);
        },
        this.billDeskResponse = function (msg) {
            var paramObject = { "msg": msg };
            return DataAccessService.postData('api/BillDesk/getPaymentResponse', paramObject);
        }
        this.billDeskS2SResponse = function (chalanaNo) {
            var paramObject = { "chalanaNo": chalanaNo };
            return DataAccessService.getDataWithPara('Payment/FindchalanaNo', paramObject);
        }
        this.getSomeValue = function (url, challanaNo) {
            var paramObject = { "url": url, "challanaNo": challanaNo };
            console.log(paramObject)
            return DataAccessService.getDataWithPara('api/BillDesk/getSomeValue', paramObject);
        },

        this.callSms = function (chalanaNo) {
            var paramObject = { "ChallanNumber": chalanaNo };
            return DataAccessService.getDataWithPara('api/PreExamination/SendSms', paramObject);
        }
    });
});