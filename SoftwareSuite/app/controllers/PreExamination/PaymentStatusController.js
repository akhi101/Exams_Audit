define(['app'], function (app) {
    app.controller("PaymentStatusController", function ($scope, $http, $localStorage, $state, PreExaminationService, MarksEntryService, $uibModal) {

        var authData = $localStorage.authorizationData;
        SystemUserTypeId = authData.SystemUserTypeId;
        $scope.challanNumber;
        $scope.LoadImg = false;
        $scope.paymentResponseFound = false;
        $scope.showData = false;
        $scope.success = false;
        $scope.dataInform = false;
        if (SystemUserTypeId == 1) {
            $scope.showData = true;
        }
        $scope.SystemUserTypeId = SystemUserTypeId;
        $scope.checkChallanNumber = function (challanNumber) {
            $scope.challanNumber = challanNumber;
            $scope.LoadImg = true;
            if ($scope.challanNumber != "") {
                console.log($scope.challanNumber);

                var getPaymentStatus = PreExaminationService.getPaymentStatus($scope.challanNumber);
                getPaymentStatus.then(function (detail) {
                    $scope.LoadImg = false;
                    $scope.paymentResponseFound = false;

                    detail = JSON.parse(detail);
                    $scope.transactionno = detail.txnrefno;
                    $scope.dataInform = true;
                    if ($scope.transactionno == 'NA') {
                        $scope.paymentResponseFound = true;
                        $scope.dataInform = false;
                    }

                    $scope.refno = detail.Refno;
                    $scope.collegecode = detail.addinfo1;
                    $scope.branch = detail.addinfo3;
                    $scope.sem = detail.addinfo5;
                    $scope.scheme = detail.addinfo4;
                    $scope.exam = detail.addinfo2;

                    detail.amount = (detail.amount * 1).toString();
                    $scope.amount = detail.amount;


                    var specialchar = 0;
                    if (detail.statusdesc.includes('-')) {
                        for (var i = 0; i < detail.statusdesc.length; i++)
                            if (detail.statusdesc.charAt(i) == '-') {
                                specialchar = i;
                                break;
                            }
                        detail.statusdesc = detail.statusdesc.substring(specialchar + 1, detail.statusdesc.length);
                    }
                    $scope.Status = detail.statusdesc;

                    if ($scope.Status == 'Success Transaction') {
                        $scope.success = true;
                    } else {
                        $scope.success = false;
                    }

                    $scope.pins = detail.pins;


                }, function (error) {
                    $scope.LoadImg = false;
                });
            }
            else {
                alert("Enter Challan Number");
                $scope.LoadImg = true;
                $scope.paymentResponseFound = true;
            }
        }


        $scope.printMarksEntered = function () {

            var divName = "idtoDivPrint";
            var $markstable = document.createElement("div");
            $markstable.innerHTML = '';
            $markstable.className = "table";



            var divToPrint = document.getElementById(divName);
            var temp = document.body.innerHTML;

            //var domClone = divToPrint.cloneNode(true);
            var $printSection = document.getElementById("printSection");
            if ($printSection) {
                var $printSection = document.createElement("div");
                $printSection.id = "printSection";


                document.body.appendChild($printSection);

                var $ele1 = document.createElement("div");
                $ele1.className = "row";

                var $ele2 = document.createElement("div");
                $ele2.className = "col-lg-2 col-md-12";

                var $ele3 = document.createElement("div");
                $ele3.className = "col-lg-10 col-md-12";


                $ele1.appendChild($ele3);

                $printSection.appendChild($ele1);

                $printSection.appendChild($ele1);
                $printSection.appendChild($markstable);

            }

            window.print();
        }
    });
});