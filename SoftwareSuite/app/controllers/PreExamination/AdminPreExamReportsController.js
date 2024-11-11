define(['app'], function (app) {
    app.controller("AdminPreExamReportsController", function ($scope, $http, $localStorage, $state, $stateParams, AppSettings, MarksEntryService, PreExaminationService, Excel, $timeout) {
        var authData = $localStorage.authorizationData;
    
        $scope.loading = true;
        var AcademicYearsActive = PreExaminationService.GetAdminPreExamReports();
        AcademicYearsActive.then(function (response) {
            var response = JSON.parse(response);
            console.log(response);
            if (response.Table.length > 0) {
                $scope.loading = false;
                $scope.reports = true;
                $scope.Noreports = false;
                $scope.getReports = response.Table;
                //CollegeCode: "001   "
                //Condonation: 0
                //Detained: 0
                //Elgible: 465
                //FeeNotPaid: 465
                //FeePaid: 0
                //OnRoll: 1640
                var OnRoll = 0
                var Elgible = 0;
                var St = 0;
                var FeePaid = 0;
                var Condonation = 0;
                var FeeNotPaid = 0;
                var Detained = 0;

                for (var i = 0; i < response.Table.length; i++) {
                    if (response.Table[i].OnRoll != null)
                        OnRoll = OnRoll + response.Table[i].OnRoll;
                    if (response.Table[i].Elgible != null)
                        Elgible = Elgible + response.Table[i].Elgible;

                    if (response.Table[i].FeePaid != null)
                        FeePaid = FeePaid + response.Table[i].FeePaid;

                    if (response.Table[i].FeeNotPaid != null)
                        FeeNotPaid = FeeNotPaid + response.Table[i].FeeNotPaid;

                    if (response.Table[i].Condonation != null)
                        Condonation = Condonation + response.Table[i].Condonation;
                    if (response.Table[i].Detained != null)
                        Detained = Detained + response.Table[i].Detained;

                    
                }
                $scope.OnRoll = OnRoll;
                $scope.Elgible = Elgible;
                $scope.FeePaid = FeePaid;
                $scope.FeeNotPaid = FeeNotPaid;
                $scope.Condonation = Condonation;
                $scope.Detained = Detained;
            
            } else {
                $scope.loading = false;
                $scope.reports = false;
                $scope.Noreports = true;
            }

        },
        function (error) {
            $scope.loading = false;
            $scope.reports = false;
            $scope.Noreports = true;
            alert("error while loading data");
        });

        $scope.openDetails = function (data) {
          console.log(data)
            
          var CollegeCode = data.CollegeCode;
           
           
          localStorage.setItem('CollegeCode', CollegeCode);
            $state.go('Dashboard.PreExamination.PreExamReports')
        }
        $scope.DownloadExcel = function (tableid) {
            var exportHref = Excel.tableToExcel(tableid, 'AdminReports');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.remove();
                a.download = "AdminFeepaymentReport.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();

            }, 100);
        }


    })
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
    })
})