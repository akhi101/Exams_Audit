define(['app'], function (app) {
    app.controller("TransactionReportController", function ($scope, $http, $localStorage, $state, AppSettings, PreExaminationService, MarksEntryService) {
         $scope.StudentType = [];
         $scope.ExcelView = false;
        $scope.isShowResults = false;

        var LoadExamTypeBysem = MarksEntryService.getStudentType();
        LoadExamTypeBysem.then(function (response) {
           
            if (response.Table.length > 0) {
              
                $scope.StudentType = response.Table;
              
            } else {
                $scope.StudentType = [];
               
            }
        },

        function (error) {
            alert("error while Data");
            console.log(error);
        });
        $scope.GetTransaction = function () {

            $scope.isShowResults = true;

            $scope.ExcelView = true;

            var studenttType = $scope.Student.id;
            var fromdate = moment($scope.setFromDate).format("DD-MM-YYYY");
            var todate = moment($scope.setToDate).format("DD-MM-YYYY");
            fromdate = fromdate.toString() + " 00:00:00";
            todate = todate.toString() + " 00:00:00";
            var GetTransaction = PreExaminationService.GetTransaction(studenttType, fromdate, todate);
                  GetTransaction.then(function (response) {
                $scope.GetTransactionData = [];
                
                $scope.GetTransactionData.push({ rows: response.Table, cols: Object.keys(response.Table[0]) });
                $scope.isShowResults = true;
            },
            function (error) {
                alert("error data is not getting");
                var err = JSON.parse(error);
                console.log(err.Message);


            });
        }
        $scope.DownloadExcelResult = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'TransactionReport');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = "TransactionReport.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }
        app.factory('Excel', function ($window) {
            //alert("hello");
            var uri = 'data:application/vnd.ms-excel;base64,',
                template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
                base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
                format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
            return {
                tableToExcel: function (tableId, worksheetName) {
                    var table = $(tableId);
                    ctx = { worksheet: worksheetName, table: table.html() },
                        href = uri + base64(format(template, ctx));
                    return href;
                }
            };
        });
        $scope.Setdate = function () {
            if (Date.parse($scope.setFromDate) > Date.parse($scope.setToDate)) {
                alert("Not greater then from date");
                $scope.setToDate = '';

                return false;
            }
        }


    });
});