define(['app'], function (app) {
    app.factory('Excel', function ($window) {
        //alert("hello");
        var uri = 'data:application/vnd.ms-excel;base64,',
            template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
            base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
            format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
        return {
            tableToExcel: function (tableId, worksheetName) {
                var table = $(tableId),
                    href = uri + base64(format(template, ctx));
                return href;
            }
        };
    });

    app.controller("TypingCumShorthandResultsController", function ($scope, $state, $stateParams, AdminService,StudentResultService, Excel, $timeout, $rootScope) {
           
        $scope.searchResult = false;
        $scope.LoadImg = false;
        $scope.StudentDetailsFound = false;
        $scope.StudentHallTicket = "";

      
        $scope.DownloadExcelResult = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, '');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = "";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        $scope.SessionCaptcha = sessionStorage.getItem('SessionCaptcha')

        var captcha = PreExaminationService.GetCaptchaString($scope.SessionCaptcha);
        captcha.then(function (response) {
            try {
                var res = JSON.parse(response);
                $scope.GetCatcha = res[0].Text;
                $scope.CaptchaImage = res[0].Image;

            } catch (err) {
                $scope.GetCatcha = ''
            }
        }, function (error) {
            $scope.GetCatcha = ''
            alert('Unable to load Captcha')
        });

        $scope.GetCaptchaData = function () {
            var captcha = PreExaminationService.GetCaptchaString($scope.SessionCaptcha);
            captcha.then(function (response) {
                try {
                    var res = JSON.parse(response);
                    $scope.GetCatcha = res[0].Text;
                    $scope.CaptchaImage = res[0].Image;

                } catch (err) {
                    $scope.GetCatcha = ''
                }
            }, function (error) {
                $scope.GetCatcha = ''
                alert('Unable to load Captcha')
            });
        }

        $scope.ValidateCaptchaText = function (PinNumber, StudtypeId) {


            if ($scope.CaptchaText == undefined || $scope.CaptchaText == "") {
                $scope.CaptchaText = "";
                alert("Enter Captcha");
                $scope.loginbutton = false;
                return;
            };

            var captcha = PreExaminationService.ValidateCaptchaText($scope.SessionCaptcha, $scope.CaptchaText, $scope.StudentHallTicket);
            captcha.then(function (res) {
                var response = JSON.parse(res)
                var Data = JSON.parse(response[0].Data)
                //var response = Data;
                if (response[0].ResponceCode == '200') {
                    //alert(response[0].ResponceDescription)
                    $scope.CaptchaText = "";
                    $scope.GetCatcha = response[0].Captcha
                    var captcha = JSON.parse(response[0].Captcha)
                    $scope.CaptchaImage = captcha[0].Image;
                    $scope.LoadImg = false;
                    $scope.DetailsNotFound = false;
                    $scope.DetailsFound = true;
                    $scope.Submit()
                    //  var resp = Data;


                } else {
                    alert(response[0].ResponceDescription)
                    $scope.CaptchaText = "";
                    $scope.GetCatcha = response[0].Captcha
                    var captcha = JSON.parse(response[0].Captcha)

                    $scope.CaptchaImage = captcha[0].Image;
                    $scope.Login.CaptchaText = "";
                    $scope.loginbutton = false;

                }

            }, function (error) {
                $scope.GetCatcha = ''
                alert('Unable to load Captcha')
            });
        }




        $scope.CardYear ='';
        $scope.Submit = function () {
         //   $scope.searchResult = true;
            $scope.LoadImg = true;
			$scope.ResultNotFound = false;
            $scope.StudentDetailsFound = false;
            var GetTypeWritingShorthandinfo = StudentResultService.GetTypeWritingShorthandReport($scope.StudentHallTicket);
            GetTypeWritingShorthandinfo.then(function (response) {
                if(response.length>0){
                    $scope.LoadImg = false;
					$scope.ResultNotFound = false;
                    $scope.StudentDetailsFound = true;
                    $scope.report = response[0];
                    $scope.CardYear = response[0].MONTH_YEAR;
                }else{
					$scope.resultfound = false;
                        $scope.ResultNotFound = true;
                        $scope.LoadImg = false;
				}
               
            }, function (error) {
                $scope.LoadImg = false;
				$scope.ResultNotFound = true;
                $scope.StudentDetailsFound = false;
                console.log(error);
            });
        }

        $scope.PrintStudentResult = function () {

            var divName = "idtoDivPrintAdmin";
            var divToPrint = document.getElementById(divName);
            var temp = document.body.innerHTML;
            $("#studentresult1").hide();
            var domClone = divToPrint.cloneNode(true);
            var $printSection = document.getElementById("printSection");
            //document.body.innerHTML = "";
            if (!$printSection) {
                var $printSection = document.createElement("div");
                $printSection.id = "printSection";
                document.body.appendChild($printSection);
            }
            $printSection.innerHTML = "";
            $printSection.appendChild(domClone);
            // alert($printSection.innerHTML);
            window.print();
            document.body.removeChild($printSection);
            $("#studentresult1").show();
        };
           
   
    });

});

