define(['app'], function (app) {
    app.controller("PaymentResponseController", function ($scope, $http, $localStorage, $state, $stateParams, AppSettings) {

        var authData = $localStorage.authorizationData;
        $scope.userName = authData.userName;
        $scope.College_Code = authData.College_Code;
        AppSettings.College_Name = authData.College_Name;
        $scope.College_Name = authData.College_Name;
        AppSettings.userName = authData.userName;
        $scope.BranchId = authData.BranchId;
        $scope.CollegeID = authData.CollegeID;
        $scope.userType = authData.SystemUserTypeId;
        
        var paymentdetails = $stateParams.data;
        var detail = JSON.parse(atob(paymentdetails));
        $scope.transactionno = detail.txnrefno;
        $scope.refno = detail.Refno;
        $scope.collegecode = detail.addinfo1;
        $scope.branch = detail.addinfo3;
        $scope.sem = detail.addinfo5;
        $scope.scheme = detail.addinfo4;        
        $scope.exam = detail.addinfo2;
        $scope.amount = detail.amount;
        $scope.Status = detail.statusdesc;


        function decodeBase64(s) {
            var e = {}, i, b = 0, c, x, l = 0, a, r = '', w = String.fromCharCode, L = s.length;
            var A = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            for (i = 0; i < 64; i++) { e[A.charAt(i)] = i; }
            for (x = 0; x < L; x++) {
                c = e[s.charAt(x)]; b = (b << 6) + c; l += 6;
                while (l >= 8) { ((a = (b >>> (l -= 8)) & 0xff) || (x < (L - 2))) && (r += w(a)); }
            }
            return r;
        };
      
        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }

        function GetParameterencript() {
            var str = "";
            var url = window.location.href.split('/');
            for (var i = url.length-1; i >= 0; i--) {
                //PaymentResponse
                if (url[i] == "PaymentResponse") {
                    break;
                }
                str = url[i]+str;
            }
            return str;
        }
        $scope.status = GetParameterValues("status");

        var s = GetParameterencript().split('?');
        $scope.decodedString = JSON.parse(decodeBase64(s[0]));

        $scope.amount = $scope.decodedString.amount; 
        // $scope.printMarksEntered = function () {
          
        //     var divName = "idtoDivPrint";
        //     var $markstable = document.createElement("div");
        //     $markstable.innerHTML = '';
        //     $markstable.className = "table";
        
        //     var divToPrint = document.getElementById(divName);
        //     var temp = document.body.innerHTML;
      
        //     var domClone = divToPrint.cloneNode(true);
        //     var $printSection = document.getElementById("printSection");
        //     if ($printSection) {
        //         var $printSection = document.createElement("div");
        //         $printSection.id = "printSection";
               
        //         document.body.appendChild($printSection);

        //         var $ele1 = document.createElement("div");
        //         $ele1.className = "row";

        //         var $ele2 = document.createElement("div");
        //         $ele2.className = "col-lg-2 col-md-12";

        //         var $ele3 = document.createElement("div");
        //         $ele3.className = "col-lg-10 col-md-12";

               
        //         $ele1.appendChild($ele3);

        //         $printSection.appendChild($ele1);

        //         $printSection.appendChild($ele1);
        //         $printSection.appendChild($markstable);
        //     }        
        //     window.print();        

        // }
        $scope.back =function(){
            var redir = $localStorage.assessment.redirecturl==""||$localStorage.assessment.redirecturl==""?"":$localStorage.assessment.redirecturl;
            //window.location.replace(redir);
            $state.go(redir);
        }
        $scope.printMarksEntered = function () {
          
            var divName = "idtoDivPrint";
            var $markstable = document.createElement("div");
            $markstable.innerHTML = '';
            $markstable.className = "table";      


         
            var divToPrint = document.getElementById(divName);
            var temp = document.body.innerHTML;
        //    $("#markslist").hide();
            var domClone = divToPrint.cloneNode(true);
            var $printSection = document.getElementById("printSection");
            if ($printSection) {
                var $printSection = document.createElement("div");
                $printSection.id = "printSection";
                //var $ele1 = document.createElement("div");
                //$ele1.className = "sbtet_img";             
               // var divToPrintheads = bl.getElementById("divtitle");
                //var divToPrintheaded = al.getElementById("divtop");
                //var divToPrinthead = el.getElementById("divtoadd");
               // $markstable.appendChild(divToPrintheads);
                //$markstable.appendChild(divToPrintheaded);
                //$markstable.appendChild(divToPrinthead);


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
    })
})