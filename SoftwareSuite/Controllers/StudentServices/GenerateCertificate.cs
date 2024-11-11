using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using QRCoder;
using SelectPdf;
using SoftwareSuite.BLL;
using SoftwareSuite.Models.Database;
using SoftwareSuite.Models.Certificate;
using PdfSharp.Pdf.IO;
using System.Configuration;

namespace SoftwareSuite.Controllers.StudentServices
{
    public class GenerateCertificate
    {

        public class InterimData
        {         
            public string InterimCertificateNo { get; set; }
            public string Pin { get; set; }
            public int SchemeId { get; set; }
            public string Name { get; set; }
            public string FatherName { get; set; }
            public string CollegeCode { get; set; }
            public string CollegeName { get; set; }
            public string CollegeAddress { get; set; }
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
            public string MonthYear { get; set; }
            public string TotalMarksInFigure { get; set; }
            public string TotalMarksInWords { get; set; }
            public string PercentageOfMarks { get; set; }
            public string CourseDuration { get; set; }
            public string PassedClass { get; set; }
            public string TotalCreditsEarned { get; set; }
            public string Scheme { get; set; }
            public string Class { get; set; }
            public string CGPA { get; set; }
            public Boolean Is3Backlog { get; set; }

        }

        public class TwshData
        {
            public string RegNo { get; set; }
            public string Name { get; set; }
            public string TwshCertificateNo { get; set; }
            public string FatherName { get; set; }
            public string Course { get; set; }
            public string division { get; set; }
            public string Remarks { get; set; }            
            public string paper1 { get; set; }
            public string paper2 { get; set; }
            public string MonthYear { get; set; }
            public string ApplicationNumber { get; set; }
            public string Gender { get; set; }


        }

        public class MigrationData
        {
            public string MigrationCertificateNo { get; set; }
            public string Pin { get; set; }
            public string Name { get; set; }
            public string FatherName { get; set; }
            public string CourseDuration { get; set; }          
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
            public string MonthYear { get; set; }          
            public string PassedClass { get; set; }
            public string CGPA { get; set; }
            public string Class { get; set; }
            public string TotalCreditsEarned { get; set; }
            public string CollegeName { get; set; }
            public string CollegeCode { get; set; }
            public string CollegeAddress { get; set; }
            public int SchemeId { get; set; }
            public Boolean Is3Backlog { get; set; }

        }

        public class BonafideData
        {
            public string ApplicationNumber { get; set; }
            public string Pin { get; set; }
            public string Name { get; set; }
            public string FatherName { get; set; }   
            public string BranchName { get; set; }
            public string AcademicYear { get; set; }
            public string conduct { get; set; }
            public Boolean IsGovClg { get; set; }
            public string College_Name { get; set; }        
              public string Clg_Address { get; set; }
            public int ServiceType { get; set; }

        }

        public class TCData
        {
            public string TransferCertificateNo { get; set; }
            public string AdmissionNo { get; set; }
            public string Pin { get; set; }
            public string Name { get; set; }
            public string FatherName { get; set; }
            public string MotherName { get; set; }
            public DateTime DateOfBIrth{ get; set; }
            public string LeftClass { get; set; }
            public string Nationality { get; set; }
            public string Religion { get; set; }
            public string Caste { get; set; }
            public DateTime LogDate { get; set; }
            public string ReasonForTc { get; set; }
            public string IdMark1 { get; set; }
            public string IdMark2 { get; set; }
            public string Conduct { get; set; }
            public string Promoted { get; set; }
            public string CollegeDuesPaid { get; set; }
            public DateTime LeftDate { get; set; }
            public DateTime admittedDate { get; set; }
            public string CollegeName { get; set; }
            public string CollegeAddress { get; set; }
            public string StudentRemarks { get; set; }
            public string Station { get; set; }
            public Boolean IsGovClg   { get; set; }


    }

        public class studentintrmMarks
        {
            public int Id { get; set; }
            public string Examination { get; set; }
            public string MaxMarks { get; set; }
            public string MaxSecured { get; set; }
            public string InFigures { get; set; }
            public string Per { get; set; }
            public string InWords { get; set; }
       
        }
        private string GetWebAppRoot()
        {
            var env = ConfigurationManager.AppSettings["SMS_ENV"].ToString();
            string host = (HttpContext.Current.Request.Url.IsDefaultPort) ?
               HttpContext.Current.Request.Url.Host :
               HttpContext.Current.Request.Url.Authority;
            if (env == "PROD")
            {
                host = String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, host);
                return host + "/";
            }
            else if (env == "DEV")
            {
             
                host = String.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, host);
                return host + HttpContext.Current.Request.ApplicationPath;
            }
            return host + "/";
        }

        private static T[][] SliceArray<T>(T[] source, int maxResultElements)
        {
            int numberOfArrays = source.Length / maxResultElements;
            if (maxResultElements * numberOfArrays < source.Length)
                numberOfArrays++;
            T[][] target = new T[numberOfArrays][];
            for (int index = 0; index < numberOfArrays; index++)
            {
                int elementsInThisArray = Math.Min(maxResultElements, source.Length - index * maxResultElements);
                target[index] = new T[elementsInThisArray];
                Array.Copy(source, index * maxResultElements, target[index], 0, elementsInThisArray);
            }
            return target;
        }

        public string GetTrsheetPdf(List<TrSheetCertificateData> TrSheetData)
        {
            string dirPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\TR";         
            CreateIfMissing(dirPath);
            var dir_id = GetTrSheets(TrSheetData);
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\TR\" + dir_id;
            var files = Directory.GetFiles(dir);
            var pdf = Guid.NewGuid().ToString();
            MergePDFs(AppDomain.CurrentDomain.BaseDirectory + @"Reports\" + pdf + ".pdf", files);
            Directory.Delete(dir, true);
            return pdf;
        }

        public string GetODCTrsheetPdf(List<ODCTrSheetData> ODCTrSheetData)
        {
            string dirPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\ODCTR";
            CreateIfMissing(dirPath);
            var dir_id = GetODCTrSheets(ODCTrSheetData);
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\ODCTR\" + dir_id;
            var files = Directory.GetFiles(dir);
            var pdf = Guid.NewGuid().ToString();
            MergePDFs(AppDomain.CurrentDomain.BaseDirectory + @"Reports\" + pdf + ".pdf", files);
            Directory.Delete(dir, true);
            return pdf;
        }

        public static void MergePDFs(string targetPath, params string[] pdfs)
        {
            using (PdfSharp.Pdf.PdfDocument targetDoc = new PdfSharp.Pdf.PdfDocument())
            {
                foreach (string pdf in pdfs)
                {
                    using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                    {
                        for (int i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }
                targetDoc.Save(targetPath);
            }
        }
        public string GetGenuinenessCertificate(DataSet IntrmData)
        {

            List<InterimData> InterimData = IntrmData.Tables[1].DataTableToList<InterimData>().ToList(); ;
            var studentintrmMarks = DataTableHelper.ConvertDataTable<studentintrmMarks>(IntrmData?.Tables[2]).ToArray();

            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            m0argin-left: 10px;
                        }
                        table {  
                            font-family: Helvetica, Arial, sans-serif; /* Nicer font */
                            width: 100%; 
                            border-collapse: collapse;
                            border-spacing: 0; 
                        }

                        td, th, tr { border: 1px solid #000; height: 40px; } /* Make cells a bit taller */

                        th {  
                            font-weight: bold; /* Make sure they're bold */
                        }

                        td {  
                            text-align: center; /* Center our text */
                        }

                      .logoImg {
                                height: 70px !important;
                                width: 70px !important;
                            }

                    .myHr {
                                border-top: 1px solid #000;
                            }

                            .header-top-section {
                                display: none;
                            }

                            .border_btm {
                                border-bottom: 1px solid #ddd;
                                text-transform: uppercase;
                            }

                            .text-uppercase {
                                text-transform: uppercase;
                            }

                            p {
                                text-indent: 50px;
padding :6px
                            }

                            .qr_css {
                                height: 90px;
                            }

                            .marginData {
                                margin: 0px 20px;
                            }

                            .footer_section {
                                display: none;
                            }

                            .footer_section {
                                display: none;
                            }

                            .print_btn {
                                display: none;
                            }

                            .spacer {
                                display: none;
                            }

                            .text-intend {
                                text-indent: 50px;
                                line-height: 2.0;
                                text-align: justify;
                                text-justify: inter-word;
                            }

                            .Line {
                                line-height: normal;
                            }
                             .sm-spacer{
                                     height:20px;
                                 }

                            .a {
                                margin: 25px;
                            }

                            canvas.sa-canvas {
                                border: none;
                            }

                            .interim-spacer {
                                display: none;
                            }

                            .myImg {
                                width: 70% !important;
                                padding:120px;
                                margin-left: auto !important;
                                margin-right: auto !important;
                                display: block!important;
                                opacity:0.3;
                            }

                            .myData {
                                position: absolute;
                            }
                        .table > thead > tr > th {
							vertical-align: middle !important;
							border-bottom: 1px solid #000000 !important;
                            border: 1px solid #000000 !important;
						}

                            .container img {
                                vertical-align: middle;
                            }

                            .container .content {
                                position: absolute!important;
                                top: 0!important;
                                background: rgb(0, 0, 0)!important;
                                background: rgba(255, 255, 255, 0.36)!important;
                                color: #130404!important;
                                width: 100%!important;
                                padding: 20px!important;
                            }
                               .container{
                                position: relative;
                                max-width: 800px;
                                margin: 0 auto;
                            }
                                .table td, .table th {
                                background-color: transparent!important;
                            }
                            .myrow{
                                position:relative!important;
                            }

                            .qr_css {
                                height: 90px;
                            }
                            .image{
                                 background: url(../../../contents/img/big-logo.png) repeat;
                               /*  height: 500px;  You must set a specified height */
                                  background-position: center; /* Center the image */
                                  background-repeat: no-repeat; /* Do not repeat the image */
                                  /* Resize the background image to cover the entire container */
                                  position:relative;
                            }
                           .myImg {
                              width: 60%;
                             padding:120px;
                            margin-left: auto;
                            margin-right: auto;
                            display: block;
                            opacity:0.3;
                            z-index:1000;
                        }
                           .myData{
                               position:absolute;
                           }
                    .container{
                            position: relative;
                            max-width: 800px;
                            margin: 0 auto;
                        }

                    .container img {
                        vertical-align: middle;
                    }

                    .container .content {
                       position: absolute;
                       bottom: 0;
                       background: rgb(0, 0, 0);
                       background: rgba(233, 235, 239, 0.36);
                       color: #130404;
                       width: 100%;
                       padding: 20px;
                    }
                        </style> "
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@" <div class='container'>
                            <div class='row'>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/telangana-logo-png.png' class='img-responsive logoImg pull-left' />
                                    </div>
                                </div>

                                <div class='col-md-8 title'>
                                    <h5 class='text-center hall_head' style='font-size: 20px!important;margin-left: -67px;margin-right: -53px;'><b>STATE BOARD OF TECHNICAL EDUCATION AND TRAINING</b></h5>
                                  <h4 class='text-center hall_head' style='font-size: 20px!important;'><b>TELANGANA</b></h4>
                                    <h5 class='text-center'>
                                        Sanketika Vidya Bhavan, Masab Tank, Hyderabad-500 028.
                                    </h5>
                                    <h5 class='text-center'>Telangana, India.</h5>
                                </div>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' class='img-responsive logoImg pull-right' />
                                    </div>
                                </div>
                            </div>
                    </div>";



            #endregion

            #region PageContent
            //< div class='col-md-10'>
            //                  <div><b class='border_btm'>Interim Certificate No</b>: {InterimData[0].InterimCertificateNo ?? "-"}.</div>                               
            //              </div>
            page += $@"  <div class='container'>
                    <hr class='myHr' />
                    <div class='marginData'>
                        <div class='row'>
                          
                            <div class='col-md-2'>          
                                <img class='qr_css' src='{GenerateQrcode(InterimData[0].Pin + "," + InterimData[0].Name)}' />
                            </div>
                        </div>
                     
                       
                    </div>
                </div>
 <div class='row'>
                    <h3 class='text-center' style='font-size: 20px!important;'><b class='border_btm text-uppercase'>Genuineness Certificate</b></h3>
                     <div>
             <div class='container'>
            <br />
            <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' alt='Notebook' class='myImg'>


            <div class='content'>
                <div class='row '>
                    <p class='text-intend'>
                        This is to certify that Mr./Ms.<b class='border_btm' > {InterimData[0].Name ?? "-"}</b> Son/Daughter of <b class='border_btm' >{InterimData[0].FatherName ?? "-"}</b>
                        bearing Permanent Identification Number(PIN) <b class='border_btm'>{InterimData[0].Pin ?? "-"}</b> has <b class='border_btm'>Passed</b>
                        the ";
            if (InterimData[0].BranchCode != "PH")
            {
                page += $@"<b class='border_btm'>{InterimData[0].CourseDuration ?? "-"}</b> ";
            }
            page += $@"Diploma Course in <b class='border_btm'>{InterimData[0].BranchName ?? "-"}</b> during the Examination held in
                        <b class='border_btm' >{InterimData[0].MonthYear ?? "-"}</b> at <b class='border_btm'>{InterimData[0].CollegeCode ?? "-"}-{InterimData[0].CollegeName ?? "-"},{InterimData[0].CollegeAddress}</b> and he/she was placed in <b class='border_btm'>{InterimData[0].PassedClass ?? "-"}.</b>
                    </p>
                </div>";
            if (InterimData[0].BranchCode != "PH")
            {
                page += $@" <div class='row' >
                    <table class='table'>
                        <thead>
                            <tr>
                                <th class='text-center' rowspan='2' style='width: 25%; border: 1px solid #000000 !important;'>Examination</th>
                                <th class='text-center' rowspan='2' style='width:2%; border: 1px solid #000000 !important;'>Max.<br/> Marks</th>
                                <th class='text-center' rowspan='2' style='width:2%; border: 1px solid #000000 !important;'>Marks <br/>Secured</th>
                                <th class='text-center' colspan='2'>MARKS TAKEN INTO CONSIDERATION FOR THE AWARD OF CLASS</th>
                            </tr>
                            <tr>
                                <th class='text-center' style='width:4%  border: 1px solid #000000 !important;'>IN FIGURES</th>                             
                                <th class='text-center' style='width:10%  border: 1px solid #000000 !important;'>IN WORDS</th>
                            </tr>
                        </thead>
                        <tbody>";

            for (var i = 0; i < studentintrmMarks.Length; i++)
            {
                page += $@"<tr>
                            <td class='text-center'>{studentintrmMarks[i].Examination ?? "-"}</td>
                            <td class='text-center'>{studentintrmMarks[i].MaxMarks ?? "-"}</td>
                            <td class='text-center'>{studentintrmMarks[i].MaxSecured ?? "-"}</td>
                            <td class='text-center'>{studentintrmMarks[i].InFigures ?? "-"}{percheck(studentintrmMarks[i].Per ?? "-")}</td>                           
                            <td class='text-center'>{studentintrmMarks[i].InWords ?? "-"}</td>
                        </tr>";
            }
                page += $@" <tr>
                                    <td class='text-center'> Total Marks In Figure</td>
                                    <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].TotalMarksInFigure}</b> </td>
                                </tr>
                                <tr>
                                    <td class='text-center'> In Words</td>
                                    <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].TotalMarksInWords ?? "-"}</b> </td>
                                </tr>
                                <tr>
                                    <td class='text-center'> Percentage of Marks</td>
                                    <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].PercentageOfMarks ?? "-"}</b> </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                      <div>The Diploma Certificate of the above Candidate is <b>GENUINE</b> and issued by this office.</div>";
            }
            page += $@"
                   <div class='sm-spacer'></div>
					  <div class='sm-spacer'></div>
					   <div class='sm-spacer'></div>
					 <div class='row'>
						<div class='col-md-12'>
							<div style='line-height: 2.0;'><b>Place : Hyderabad</b></div>
                            <div style='line-height: 2.0;'><b>Date :  {DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
						</div>               
					</div>
                </div>             
            </div>";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir + "GC" + InterimData[0].Pin + "_" + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");
            return relativePath;
        }

        public string GetTypewritingCertificate(DataSet IntrmData)
        {

            List<InterimData> InterimData = IntrmData.Tables[1].DataTableToList<InterimData>().ToList(); ;
            var studentintrmMarks = DataTableHelper.ConvertDataTable<studentintrmMarks>(IntrmData?.Tables[2]).ToArray();

            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            m0argin-left: 10px;
                        }
                        table {  
                            font-family: Helvetica, Arial, sans-serif; /* Nicer font */
                            width: 100%; 
                            border-collapse: collapse;
                            border-spacing: 0; 
                        }

                        td, th, tr { border: 1px solid #000; height: 40px; } /* Make cells a bit taller */

                        th {  
                            font-weight: bold; /* Make sure they're bold */
                        }

                        td {  
                            text-align: center; /* Center our text */
                        }

                      .logoImg {
                                height: 70px !important;
                                width: 70px !important;
                            }

                    .myHr {
                                border-top: 1px solid #000;
                            }

                            .header-top-section {
                                display: none;
                            }

                            .border_btm {
                                border-bottom: 1px solid #ddd;
                                text-transform: uppercase;
                            }

                            .text-uppercase {
                                text-transform: uppercase;
                            }

                            p {
                                text-indent: 50px;
padding :6px
                            }

                            .qr_css {
                                height: 90px;
                            }

                            .marginData {
                                margin: 0px 20px;
                            }

                            .footer_section {
                                display: none;
                            }

                            .footer_section {
                                display: none;
                            }

                            .print_btn {
                                display: none;
                            }

                            .spacer {
                                display: none;
                            }

                            .text-intend {
                                text-indent: 50px;
                                line-height: 2.0;
                                text-align: justify;
                                text-justify: inter-word;
                            }

                            .Line {
                                line-height: normal;
                            }
                             .sm-spacer{
                                     height:20px;
                                 }

                            .a {
                                margin: 25px;
                            }

                            canvas.sa-canvas {
                                border: none;
                            }

                            .interim-spacer {
                                display: none;
                            }

                            .myImg {
                                width: 70% !important;
                                padding:120px;
                                margin-left: auto !important;
                                margin-right: auto !important;
                                display: block!important;
                                opacity:0.3;
                            }

                            .myData {
                                position: absolute;
                            }
                        .table > thead > tr > th {
							vertical-align: middle !important;
							border-bottom: 1px solid #000000 !important;
                            border: 1px solid #000000 !important;
						}

                            .container img {
                                vertical-align: middle;
                            }

                            .container .content {
                                position: absolute!important;
                                top: 0!important;
                                background: rgb(0, 0, 0)!important;
                                background: rgba(255, 255, 255, 0.36)!important;
                                color: #130404!important;
                                width: 100%!important;
                                padding: 20px!important;
                            }
                               .container{
                                position: relative;
                                max-width: 800px;
                                margin: 0 auto;
                            }
                                .table td, .table th {
                                background-color: transparent!important;
                            }
                            .myrow{
                                position:relative!important;
                            }

                            .qr_css {
                                height: 90px;
                            }
                            .image{
                                 background: url(../../../contents/img/big-logo.png) repeat;
                               /*  height: 500px;  You must set a specified height */
                                  background-position: center; /* Center the image */
                                  background-repeat: no-repeat; /* Do not repeat the image */
                                  /* Resize the background image to cover the entire container */
                                  position:relative;
                            }
                           .myImg {
                              width: 60%;
                             padding:120px;
                            margin-left: auto;
                            margin-right: auto;
                            display: block;
                            opacity:0.3;
                            z-index:1000;
                        }
                           .myData{
                               position:absolute;
                           }
                    .container{
                            position: relative;
                            max-width: 800px;
                            margin: 0 auto;
                        }

                    .container img {
                        vertical-align: middle;
                    }

                    .container .content {
                       position: absolute;
                       bottom: 0;
                       background: rgb(0, 0, 0);
                       background: rgba(233, 235, 239, 0.36);
                       color: #130404;
                       width: 100%;
                       padding: 20px;
                    }
                        </style>"
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@" <div class='container'>
                            <div class='row'>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/telangana-logo-png.png' class='img-responsive logoImg pull-left' />
                                    </div>
                                </div>

                                <div class='col-md-8 title'>
                                    <h5 class='text-center hall_head' style='font-size: 20px!important;margin-left: -67px;margin-right: -53px;'><b>STATE BOARD OF TECHNICAL EDUCATION AND TRAINING</b></h5>
                                  <h4 class='text-center hall_head' style='font-size: 20px!important;'><b>TELANGANA</b></h4>
                                    <h5 class='text-center'>
                                        Sanketika Vidya Bhavan, Masab Tank, Hyderabad-500 028.
                                    </h5>
                                    <h5 class='text-center'>Telangana, India.</h5>
                                </div>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' class='img-responsive logoImg pull-right' />
                                    </div>
                                </div>
                            </div>
                    </div>";



            #endregion

            #region PageContent
            //< div class='col-md-10'>
            //                  <div><b class='border_btm'>Interim Certificate No</b>: {InterimData[0].InterimCertificateNo ?? "-"}.</div>                               
            //              </div>
            page += $@"  <div class='container'>
                    <hr class='myHr' />
                    <div class='marginData'>
                        <div class='row'>
                          
                            <div class='col-md-2'>          
                                <img class='qr_css' src='{GenerateQrcode(InterimData[0].Pin + "," + InterimData[0].Name)}' />
                            </div>
                        </div>
                     
                       
                    </div>
                </div>
             <div class='container'>
            <br />
            <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' alt='Notebook' class='myImg'>
            <div class='content'>
                <div class='row '>
                    <p class='text-intend'>
                        <p class='text - intend'>
                    This is to certify that Mr./ Ms.< b class='border_btm'>Name</b> Son/Daughter of
<b class='border_btm'>{{FatherName == null ? '______________':FatherName}}</b>
                    with Reg.No. <b class='border_btm'>{{RegNo == null ? '______________':RegNo}}</b><b class='border_btm'></b> has Passed  the<b class='border_btm' >_____________</b><b class='border_btm' >{{Course}}</b>  Examination held in <b class='border_btm' ng-if='!MonthYear'>_____________</b> <b class='border_btm' ng-if='MonthYear'>{{MonthYear}}</b></span> at<b class='border_btm' ng-if='!College'>_____________</b><b class='border_btm' ng-if='College'>{{College}}</b>  has been placed in <b class='border_btm' ng-if='Class'>{{Class}}</b><b class='border_btm' ng-if='!Class'>_____________</b>
                </p>
                    </p>
                </div>

               <div class='row'>
                < div class='col-md-3'></div>
                <div class='col-md-6'>
                    <table class='table'>
                        <thead>

                            <tr>
                                <th class='text-center'>Paper I</th>
                                <th class='text-center'>Paper II</th>
                            </tr>

                        </thead>
                        <tbody>
                            <tr>
                                <td class='text-center'> {{paper}}</td>

                                <td class='text-center'>{{paper2}}</td>


                            </tr>

                        </tbody>




                    </table>
                </div>
                <div class='col-md-3'></div>
            </div>
                   <div class='sm-spacer'></div>
					  <div class='sm-spacer'></div>
					   <div class='sm-spacer'></div>
					 <div class='row'>
						<div class='col-md-12'>
							<div style='line-height: 2.0;'><b>Place : Hyderabad</b></div>
                            <div style='line-height: 2.0;'><b>Date :  {DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
						</div>               
					</div>
                </div>             
            </div>";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir + "GC" + InterimData[0].Pin + "_" + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");
            return relativePath;
        }

        public string percheck (string per) {
            if (per == "") {
                return "";
        } else {
            return $@" ({per})";
            }
        }

        //public string markstable(studentintrmMarks studentintrmMarks)
        //{
        //    if (per == "")
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        return $@"<div class='row'>
        //            <table class='table'>
        //                <thead>
        //                    <tr>
        //                        <th class='text-center' rowspan='2' style='width: 25%; border: 1px solid #000000 !important;'>Examination</th>
        //                        <th class='text-center' rowspan='2' style='width:2%; border: 1px solid #000000 !important;'>Max.<br/> Marks</th>
        //                        <th class='text-center' rowspan='2' style='width:2%; border: 1px solid #000000 !important;'>Marks <br/>Secured</th>
        //                        <th class='text-center' colspan='2'>MARKS TAKEN INTO CONSIDERATION FOR THE AWARD OF CLASS</th>
        //                    </tr>
        //                    <tr>
        //                        <th class='text-center' style='width:4%  border: 1px solid #000000 !important;'>IN FIGURES</th>                             
        //                        <th class='text-center' style='width:10%  border: 1px solid #000000 !important;'>IN WORDS</th>
        //                    </tr>
        //                </thead>
        //                <tbody>";

        //        for (var i = 0; i < studentintrmMarks.Length; i++)
        //        {
        //            page += $@"<tr>
        //                    <td class='text-center'>{studentintrmMarks[i].Examination ?? "-"}</td>
        //                    <td class='text-center'>{studentintrmMarks[i].MaxMarks ?? "-"}</td>
        //                    <td class='text-center'>{studentintrmMarks[i].MaxSecured ?? "-"}</td>
        //                    <td class='text-center'>{studentintrmMarks[i].InFigures ?? "-"}{percheck(studentintrmMarks[i].Per)}</td>                           
        //                    <td class='text-center'>{studentintrmMarks[i].InWords ?? "-"}</td>
        //                </tr>";
        //        }
        //        page += $@" <tr>
        //                            <td class='text-center'> Total Marks In Figure</td>
        //                            <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].TotalMarksInFigure}</b> </td>
        //                        </tr>
        //                        <tr>
        //                            <td class='text-center'> In Words</td>
        //                            <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].TotalMarksInWords ?? "-"}</b> </td>
        //                        </tr>
        //                        <tr>
        //                            <td class='text-center'> Percentage of Marks</td>
        //                            <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].PercentageOfMarks ?? "-"}</b> </td>
        //                        </tr>
        //                    </tbody>
        //                </table>
        //            </div>";
        //    }
        //}

        public string GetInterimCertificate(DataSet IntrmData)
        {

            List <InterimData> InterimData = IntrmData.Tables[1].DataTableToList<InterimData>().ToList(); ;
            var studentintrmMarks = DataTableHelper.ConvertDataTable<studentintrmMarks>(IntrmData?.Tables[2]).ToArray();

            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            m0argin-left: 10px;
                        }
                        table {  
                            font-family: Helvetica, Arial, sans-serif; /* Nicer font */
                            width: 100%; 
                            //border-collapse: collapse;
                            border-spacing: 0; 
                        }

                        td, th, tr { border: 1px solid #000; height: 30px; } /* Make cells a bit taller */

                        th {  
                            font-weight: bold; /* Make sure they're bold */
                        }

                        td {  
                            text-align: center; /* Center our text */
                        }

                      .logoImg {
                                height: 70px !important;
                                width: 70px !important;
                            }

                    .myHr {
                                border-top: 1px solid #000;
                            }

                            .header-top-section {
                                display: none;
                            }

                            .border_btm {
                                border-bottom: 1px solid #ddd;
                                text-transform: uppercase;
                            }

                            .text-uppercase {
                                text-transform: uppercase;
                            }

                            p {
                                text-indent: 50px;
padding :6px
                            }

                            .qr_css {
                                height: 90px;
                            }

                            .marginData {
                                margin: 0px 20px;
                            }

                            .footer_section {
                                display: none;
                            }

                            .footer_section {
                                display: none;
                            }

                            .print_btn {
                                display: none;
                            }

                            .spacer {
                                display: none;
                            }

                            .text-intend {
                                text-indent: 50px;
                                line-height: 2.0;
                                text-align: justify;
                                text-justify: inter-word;
                            }

                            .Line {
                                line-height: normal;
                            }
                             .sm-spacer{
                                     height:20px;
                                 }

                            .a {
                                margin: 25px;
                            }

                            canvas.sa-canvas {
                                border: none;
                            }

                            .interim-spacer {
                                display: none;
                            }

                            .myImg {
                                width: 70% !important;
                                padding:120px;
                                margin-left: auto !important;
                                margin-right: auto !important;
                                display: block!important;
                                opacity:0.3;
                            }

                            .myData {
                                position: absolute;
                            }
                        .table > thead > tr > th {
							vertical-align: middle !important;
							border: 1px solid #000000 !important;
                            border: 1px solid #000000 !important;
						}

                            .container img {
                                vertical-align: middle;
                            }

                            .container .content {
                                position: absolute!important;
                                top: 0!important;
                                background: rgb(0, 0, 0)!important;
                                background: rgba(255, 255, 255, 0.36)!important;
                                color: #130404!important;
                                width: 100%!important;
                                padding: 20px!important;
                            }
                               .container{
                                position: relative;
                                max-width: 800px;
                                margin: 0 auto;
                            }
                                .table td, .table th {
                                background-color: transparent!important;
                            }
                            .myrow{
                                position:relative!important;
                            }

                            .qr_css {
                                height: 90px;
                            }
                            .image{
                                 background: url(../../../contents/img/big-logo.png) repeat;
                               /*  height: 500px;  You must set a specified height */
                                  background-position: center; /* Center the image */
                                  background-repeat: no-repeat; /* Do not repeat the image */
                                  /* Resize the background image to cover the entire container */
                                  position:relative;
                            }
                           .myImg {
                              width: 60%;
                             padding:120px;
                            margin-left: auto;
                            margin-right: auto;
                            display: block;
                            opacity:0.3;
                            z-index:1000;
                        }
                           .myData{
                               position:absolute;
                           }
                    .container{
                            position: relative;
                            max-width: 800px;
                            margin: 0 auto;
                        }

                    .container img {
                        vertical-align: middle;
                    }

                    .container .content {
                       position: absolute;
                       bottom: 0;
                       background: rgb(0, 0, 0);
                       background: rgba(233, 235, 239, 0.36);
                       color: #130404;
                       width: 100%;
                       padding: 20px;
                    }
                      .less_pad{
padding:1px!important;
}
                        </style> "
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@" <div class='container'>
                            <div class='row'>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/telangana-logo-png.png' class='img-responsive logoImg pull-left' />
                                    </div>
                                </div>

                                <div class='col-md-8 title'>
                                    <h5 class='text-center hall_head' style='font-size: 20px!important;margin-left: -67px;margin-right: -53px;'><b>STATE BOARD OF TECHNICAL EDUCATION AND TRAINING</b></h5>
                                  <h4 class='text-center hall_head' style='font-size: 20px!important;'><b>TELANGANA</b></h4>
                                    <h6 class='text-center'>
                                       Sanketika Vidya Bhavan, Masab Tank, Hyderabad­500 028, India.
                                    </h6>
                                  
                                </div>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' class='img-responsive logoImg pull-right' />
                                    </div>
                                </div>
                            </div>
                    </div>";



            #endregion

            #region PageContent
            page += $@"  <div class='container'>
                    <hr class='myHr' />
                    <div class='marginData'>
                        <div class='row'>
                            <div class='col-md-10'>
                                <div><b class='border_btm'>Interim Certificate No</b>: {InterimData[0].InterimCertificateNo ?? "-"}.</div>                               
                            </div>
                            <div class='col-md-2'>          
                                <img class='qr_css' src='{GenerateQrcode(InterimData[0].Pin + "," + InterimData[0].Name)}' />
                            </div>
                        </div>
                      <div class='row'>
                                <h3 class='text-center' style='font-size: 20px!important;'><b class='border_btm text-uppercase'>Interim Certificate</b></h3>
                            </div>
                       
                    </div>
                </div>
             <div class='container'>
            <br />
            <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' alt='Notebook' class='myImg'>
            <div class='content'>";
            if (InterimData[0].SchemeId != 5)
            {
                page += $@"<div class='row '>
                    <p class='text-intend'>
                        This is to certify that Mr./Ms.<b class='border_btm' > {InterimData[0].Name ?? "-"}</b> Son/Daughter of <b class='border_btm' >{InterimData[0].FatherName ?? "-"}</b>
                        bearing Permanent Identification Number(PIN) <b class='border_btm'>{InterimData[0].Pin ?? "-"}</b> has <b class='border_btm' >Passed</b>
                        the <b class='border_btm'>{InterimData[0].CourseDuration ?? "-"}</b> Diploma Course in <b class='border_btm'>{InterimData[0].BranchName ?? "-"}</b>{isThreeBacklog(InterimData[0].Is3Backlog, InterimData[0].MonthYear)} at <b class='border_btm'>{InterimData[0].CollegeCode ?? "-"}-{InterimData[0].CollegeName ?? "-"},{InterimData[0].CollegeAddress}</b>
and he/she was placed in <b class='border_btm'>{InterimData[0].PassedClass ?? "-"}</b>{isThreeBacklogdivision(InterimData[0].Is3Backlog)}..
                    </p>
                </div>";
            }else  if (InterimData[0].SchemeId == 5) { 
                    page += $@"<div class='row'>
	<p class='text-intend'>
                        This is to certify that Mr./Ms.
		<b class='border_btm' > {InterimData[0].Name ?? "-"}</b> Son/Daughter of 
		<b class='border_btm' >{InterimData[0].FatherName ?? "-"}</b>
                        bearing Permanent Identification Number(PIN) 
		<b class='border_btm'>{InterimData[0].Pin ?? "-"}</b> has 
		<b class='border_btm' >Passed</b>
                        the 
		<b class='border_btm'>{InterimData[0].CourseDuration ?? "-"}</b> Diploma Course in 
		<b class='border_btm'>{InterimData[0].BranchName ?? "-"}</b> during the Examination held 
in 
		<b class='border_btm'>{InterimData[0].MonthYear ?? "-"}</b> at 
		<b class='border_btm'>{InterimData[0].CollegeCode ?? "-"}-{InterimData[0].CollegeName ?? "-"}</b> and he/she is placed in 
		<b class='border_btm'>{InterimData[0].Class ?? "-"}</b> with

		<b class='border_btm'>{InterimData[0].CGPA ?? "-"}</b> Cumulative Grade Point Average <b>(CGPA)</b> on a scale of <b>10</b> and earned 
		<b class='border_btm'>{InterimData[0].TotalCreditsEarned ?? "-"}</b> credits.
                    
	</p>
	<div class='sm-spacer'></div>
</div>";
            }

            if(InterimData[0].BranchCode  != "PH" && InterimData[0].SchemeId != 5)
            {
            page += $@" <div class='row'>
                <table class='table'>
                    <thead>
                        <tr>
                            <th class='text-center' rowspan='2' style='width: 25%; border: 1px solid #000000 !important;'>Examination</th>
                            <th class='text-center' rowspan='2' style='width:2%; border: 1px solid #000000 !important;'>Max.<br/> Marks</th>
                            <th class='text-center' rowspan='2' style='width:2%; border: 1px solid #000000 !important;'>Marks<br/> Secured</th>
                            <th class='text-center' colspan='2'>MARKS TAKEN INTO CONSIDERATION FOR THE AWARD OF CLASS</th>
                        </tr>
                        <tr>
                            <th class='text-center' style='width:4%  border: 1px solid #000000 !important;'>IN FIGURES</th>                             
                            <th class='text-center' style='width:10%  border: 1px solid #000000 !important;'>IN WORDS</th>
                        </tr>
                    </thead>
                    <tbody>";

                        for (var i = 0; i<studentintrmMarks.Length; i++)
                            {
                                page += $@"<tr>
                                            <td class='text-center'>{studentintrmMarks[i].Examination ?? "-"}</td>
                                            <td class='text-center'>{studentintrmMarks[i].MaxMarks ?? "-"}</td>
                                            <td class='text-center'>{studentintrmMarks[i].MaxSecured ?? "-"}</td>
                                            <td class='text-center'>{studentintrmMarks[i].InFigures ?? "-"}{percheck(studentintrmMarks[i].Per)}</td>                           
                                            <td class='text-center'>{studentintrmMarks[i].InWords ?? "-"}</td>
                                        </tr>";
                            }
                    page += $@" <tr>
                                <td class='text-center'> Total Marks In Figure</td>
                                <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].TotalMarksInFigure}</b> </td>
                            </tr>
                            <tr>
                                <td class='text-center'> In Words</td>
                                <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].TotalMarksInWords ?? "-"}</b> </td>
                            </tr>
                            <tr>
                                <td class='text-center'> Percentage of Marks</td>
                                <td class='text-center' colspan='5'><b class='text-uppercase '>{InterimData[0].PercentageOfMarks ?? "-"}</b> </td>
                            </tr>
                        </tbody>
                    </table>
                </div>";                
            }

            page += $@"<div class='row'>
                        <p class='text-intend'>
                            This Certificate has been issued as per the request of candidate as an interim measure
                            and is valid till the issue of<b class='text-transform:uppercase;'> Original Diploma Certificate </b> or for a period of <b class='border_btm' >Six Months </b> whichever is earlier.
                        </p>
                    </div>";
            if (InterimData[0].SchemeId == 5)
            {
                page += $@" 
<div class='sm-spacer'></div>
 <div class='row'>
						<div class='col-md-12'>
							<div style='line-height: 2.0;'><b>Place : Hyderabad</b></div>
                            <div style='line-height: 2.0;'><b>Date :  {DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
						</div>               
					</div>
<div class='sm-spacer'></div>
<div class='sm-spacer'></div>
					  <h6><center><b>CGPA EQUIVALENT TO AGGREGATE PERCENTAGE OF MARKS</b></center></h6>
<div class='row'>
					  <div class='col-md-3'></div>
                          <div class='col-md-6'>
					 <table class='table' style='font-size:12px;'>

  <tr style='padding:1px!important'>
    <th class='text-center' style='border: 1px solid #000000 !important;padding:3px!important'>S.NO</th>
    <th class='text-center' style='border: 1px solid #000000 !important;padding:3px!important'>CGPA</th>
    <th class='text-center' style='border: 1px solid #000000 !important;padding:3px!important'>Aggregate Percentage of Marks*</th>
	 <th  class='text-center'style='border: 1px solid #000000 !important;padding:3px!important'>Division*</th>
  </tr>

  <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>1</td>
    <td class='less_pad' padding:1px!important>10</td>
    <td class='less_pad' padding:1px!important>95</td>
	<td rowspan = '3' > First Class with <br>Distinction</td>
  </tr>
  <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>2</td>
    <td class='less_pad' padding:1px!important>9</td>
    <td class='less_pad' padding:1px!important>85</td>
  </tr>
  <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>3</td>
    <td class='less_pad' padding:1px!important>8</td>
    <td class='less_pad' padding:1px!important>75</td>
  </tr>
  <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>4</td>
    <td class='less_pad' padding:1px!important>7</td>
    <td class='less_pad' padding:1px!important>65</td>
	<td class='less_pad' padding:1px!important>First Class</td>
  </tr>
 <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>5</td>
    <td class='less_pad' padding:1px!important>6</td>
    <td class='less_pad' padding:1px!important>55</td>
	<td rowspan = '3' > Second Class</td>
  </tr>
  <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>6</td>
    <td class='less_pad' padding:1px!important>5</td>
    <td class='less_pad' padding:1px!important>45</td>
  </tr>
  <tr style='padding:1px!important'>
    <td class='less_pad' padding:1px!important>7</td>
    <td class='less_pad' padding:1px!important>4</td>
    <td class='less_pad' padding:1px!important>35</td>
  </tr>
 
</table >
</div>
 <div class='col-md-3'></div>
</div>
<div class='row'>
<div class='col-md-3'></div>
<div class='col-md-7'>
		 <table class='table' style = 'margin-top:20px;font-size:12px; style='border: 0px solid #fff !important;' >

  <tr style='border: 0px solid #fff !important; padding:3px!important'>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;'> *Aggregate %</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' >:</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;'>(CGPA-0.5) x 10</td>
	
  </tr>
  <tr style='border: 0px solid #fff !important;'>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'># First Class with Distinction  </td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>:</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>CGPA>=8</td>
  </tr>
  <tr style='border: 0px solid #fff !important;'>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>   First Class</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>:</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>8>CGPA>=6.5</td>
  </tr>
  <tr style='border: 0px solid #fff !important;'>  
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>Second Class</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>:</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>6.5>CGPA>=4</td>
  </tr >
   <tr style='border: 0px solid #fff !important;'>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important;' class='less_pad'>   Pass</td>
    <td style='border: 0px solid #fff !important; padding:3px!important;text-align:left!important' class='less_pad'>:</td>
    <td style='border: 0px solid #fff !important;padding:3px!important;text-align:left!important' class='less_pad'>CGPA<4<br>(Subject to Earning of >=130 Credits)</td>
  </tr>
</table>
</div>					  
 <div class='col-md-2'></div>
</div>";
            }
            if (InterimData[0].SchemeId != 5)
            {
                page += $@" <div class='sm-spacer'></div>
					   <div class='sm-spacer'></div>
					 <div class='row'>
						<div class='col-md-12'>
							<div style='line-height: 2.0;'><b>Place : Hyderabad</b></div>
                            <div style='line-height: 2.0;'><b>Date :  {DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
						</div>               
					</div>";
}
            page += $@"</div>             
            </div>";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir+"INTRM"+ InterimData[0].Pin+"_" + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");
            return relativePath;
        }

        public string GetTwshCertificate(DataSet TwshDat)
        {

            List<TwshData> TwshData = TwshDat.Tables[0].DataTableToList<TwshData>().ToList(); ;
            //var studentintrmMarks = DataTableHelper.ConvertDataTable<studentintrmMarks>(IntrmData?.Tables[2]).ToArray();

            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            m0argin-left: 10px;
                        }
                        table {  
                            font-family: Helvetica, Arial, sans-serif; /* Nicer font */
                            width: 100%; 
                            border-collapse: collapse;
                            border-spacing: 0; 
                        }

                        td, th, tr { border: 1px solid #000; height: 40px; } /* Make cells a bit taller */

                        th {  
                            font-weight: bold; /* Make sure they're bold */
                        }

                        td {  
                            text-align: center; /* Center our text */
                        }

                      .logoImg {
                                height: 70px !important;
                                width: 70px !important;
                            }

                    .myHr {
                                border-top: 1px solid #000;
                            }

                            .header-top-section {
                                display: none;
                            }

                            .border_btm {
                                border-bottom: 1px solid #ddd;
                                text-transform: uppercase;
                            }

                            .text-uppercase {
                                text-transform: uppercase;
                            }

                            p {
                                text-indent: 50px;
padding :6px
                            }

                            .qr_css {
                                height: 90px;
                            }

                            .marginData {
                                margin: 0px 20px;
                            }

                            .footer_section {
                                display: none;
                            }

                            .footer_section {
                                display: none;
                            }

                            .print_btn {
                                display: none;
                            }

                            .spacer {
                                display: none;
                            }

                            .text-intend {
                                text-indent: 50px;
                                line-height: 2.0;
                                text-align: justify;
                                text-justify: inter-word;
                            }

                            .Line {
                                line-height: normal;
                            }
                             .sm-spacer{
                                     height:20px;
                                 }

                            .a {
                                margin: 25px;
                            }

                            canvas.sa-canvas {
                                border: none;
                            }

                            .interim-spacer {
                                display: none;
                            }

                            .myImg {
                                width: 70% !important;
                                padding:120px;
                                margin-left: auto !important;
                                margin-right: auto !important;
                                display: block!important;
                                opacity:0.3;
                            }

                            .myData {
                                position: absolute;
                            }
                        .table > thead > tr > th {
							vertical-align: middle !important;
							border-bottom: 1px solid #000000 !important;
                            border: 1px solid #000000 !important;
						}

                            .container img {
                                vertical-align: middle;
                            }

                            .container .content {
                                position: absolute!important;
                                top: 0!important;
                                background: rgb(0, 0, 0)!important;
                                background: rgba(255, 255, 255, 0.36)!important;
                                color: #130404!important;
                                width: 100%!important;
                                padding: 20px!important;
                            }
                               .container{
                                position: relative;
                                max-width: 800px;
                                margin: 0 auto;
                            }
                                .table td, .table th {
                                background-color: transparent!important;
                            }
                            .myrow{
                                position:relative!important;
                            }

                            .qr_css {
                                height: 90px;
                            }
                            .image{
                                 background: url(../../../contents/img/big-logo.png) repeat;
                               /*  height: 500px;  You must set a specified height */
                                  background-position: center; /* Center the image */
                                  background-repeat: no-repeat; /* Do not repeat the image */
                                  /* Resize the background image to cover the entire container */
                                  position:relative;
                            }
                           .myImg {
                              width: 60%;
                             padding:120px;
                            margin-left: auto;
                            margin-right: auto;
                            display: block;
                            opacity:0.3;
                            z-index:1000;
                        }
                           .myData{
                               position:absolute;
                           }
                    .container{
                            position: relative;
                            max-width: 800px;
                            margin: 0 auto;
                        }

                    .container img {
                        vertical-align: middle;
                    }

                    .container .content {
                       position: absolute;
                       bottom: 0;
                       background: rgb(0, 0, 0);
                       background: rgba(233, 235, 239, 0.36);
                       color: #130404;
                       width: 100%;
                       padding: 20px;
                    }
                        </style> "
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@" <div class='container'>
                            <div class='row'>
                                <div class='col-md-2 logo'>
                                    <div class='logo-image' style='padding:6px!important;'>
                                        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' class='img-responsive logoImg pull-left' />
                                    </div>
                                </div>

                                <div class='col-md-10 title'>
                                    <h3 class='text-center hall_head' style='margin-left: -67px;margin-right: -53px;font-weight:bold;'><b>STATE BOARD OF TECHNICAL EDUCATION AND TRAINING</b></h3>
                                  <h3 class='text-center hall_head' style=''><b>TELANGANA</b></h3>
                                    <h4 class='text-center' style='text-transform: uppercase;font-weight:bold;'>
                                        Marks Card {TwshData[0].MonthYear}
                                    </h4>
                                   
                                </div>
                               
                            </div>
                    </div>";



            #endregion

            #region PageContent
            page += $@"    <div class='container'>
                   

              <div class='container'>
            <br />
           
           
                

                <div class='row'>
                    <table class='table'>
                        <thead>
                           
                            <tr>
                                <th style='font-weight:bold!important; text-align:left!important'>Name</th>                             
                                     <th style='text-align:left!important'>{TwshData[0].Name ?? "-"}</th>
                            </tr>
                        </thead>
                        <tbody>
                                <tr>
                                    <td style='font-weight:bold!important; text-align:left!important'>Gender</td> 
                                    <td style='text-align:left!important'>{TwshData[0].Gender ?? "-"}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight:bold!important; text-align:left!important'>Grade Name</td> 
                                    <td style='text-align:left!important'>{TwshData[0].Course ?? "-"}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight:bold!important; text-align:left!important'>Application Number</td> 
                                    <td style='text-align:left!important'>{TwshData[0].ApplicationNumber ?? "-"}</td>
                                </tr>
                                    <td style='font-weight:bold!important; text-align:left!important'>Hallticket Number</td> 
                                    <td style='text-align:left!important'>{TwshData[0].RegNo}</td>
                              </tbody>
                        </table>

  <table class='table'>
                        <thead>
                           
                            <tr>
                                <th class='text-center'  border: 1px solid #000000 !important;'>P1 MARKS</th>                             
                                <th class='text-center'   border: 1px solid #000000 !important;'>P2 Marks</th>
                                <th class='text-center'   border: 1px solid #000000 !important;'>Result</th>
                                <th class='text-center'   border: 1px solid #000000 !important;'>Division</th>
                            </tr>
                        </thead>
                        <tbody>
                                <tr>
                                    <td class='text-center'>{TwshData[0].paper1 ?? "-"} </td>
                                    <td class='text-center'>{TwshData[0].paper2 ?? "-"}</td>
                                    <td class='text-center'>{TwshData[0].Remarks ?? "-"}</td>
                                    <td class='text-center'>{TwshData[0].division ?? "-"}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
					  <div class='sm-spacer'></div>
					   <div class='sm-spacer'></div>
<div class='sm-spacer'></div>
<div class='sm-spacer'></div>
 <div class='sm-spacer'></div>
					   <div class='sm-spacer'></div>
<div class='sm-spacer'></div>
 <div class='sm-spacer'></div>
<div class='sm-spacer'></div>
					 <div class='row'>
						<div class='col-md-6'>
							<div style = 'line-height: 2.0' ><b> Place : Hyderabad</b></div>
                            <div style = 'line-height: 2.0' ><b> Date :  {DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
						</div>    
                        <div class='col-md-6'>
							<div class='pull-right style = 'line-height: 2.0' ><b> </b></div>
                            </div>
						</div> 
					</div>
                </div>             
            </div>
";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir + "TWSH" + TwshData[0].RegNo + "_" + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");
            return relativePath;
        }

        public string GetDuplicateDiplomaCertificate(DataSet TwshDat)
        {

            List<TwshData> TwshData = TwshDat.Tables[0].DataTableToList<TwshData>().ToList(); ;
            //var studentintrmMarks = DataTableHelper.ConvertDataTable<studentintrmMarks>(IntrmData?.Tables[2]).ToArray();

            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style>
    .bg_image{
         background-image: url('../../../ contents / img / memo.PNG');
         background - repeat:no - repeat;
        height: 1000em;
        width: 100 %;
            background - position: center; /* Center the image */
            background - repeat: no - repeat; /* Do not repeat the image */
                                              /* Resize the background image to cover the entire container */
        position: relative;
        }
      .myImg {
      width: 80%;
    margin-left: auto;
    margin-right: auto;
      /*padding:120px;*/
    display: block;
    opacity:1;
    z-index:1000;
}
    .container{
          position: relative;
            max-width: 800px;
            margin: 0 auto;
        }

            .container img
{
    vertical-align: middle;
}

            .container.content {
               position: absolute;

    bottom: 0;
    top:0px;
    background: rgb(0, 0, 0);
background: rgba(255, 255, 255, 0.36);
color: #130404;
    width: 100%;
    /*padding: 20px;*/
            }
           .Name {
    left: 23em;
    position: absolute;
    top: 14.5em;
}
           .father{
                 left: 9em;
    position: absolute;
    top: 16.5em;
           }
            .year{
                 left: 21em;
    position: absolute;
    top: 18.5em;
           }
             .College{
                 left: 10em;
    position: absolute;
    top: 20em;
           }
              .branch{
                 left: 25em;
    position: absolute;
    top: 22em;
           }
               .pin{
                 left: 10em;
    position: absolute;
    top: 27.5em;
           }
          .TrainingDuration {
        left: 15em;
        position: absolute;
        top: 29.5em;
    }
    .year {
    left: 20.5em;
    position: absolute;
    top: 18.3em;
}
                .MonthYear{
 left: 10em;
    position: absolute;
    top: 25.5em;
                }
                   .class{
 left: 13em;
    position: absolute;
    top: 31em;
                }
              .table-responsive {
    min-height: .01%;
    overflow-x: auto;
    width: 66%;
    font-size: 13px;
    margin: 105px;
    top: 29em;
    position: absolute;
}
                   .disp_none{
                       display:none;
                   }
                   table> tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
    padding: 6px 6px;
    line-height: 1.42857143;
    vertical-align: middle;
    /* border-top: 1px solid #ddd; */
    font-size: 13px;
    border: 0px;
    /* width: 80%; */
}
                table.table {
    clear: both;
    margin-bottom: 6px !important;
    max-width: none !important;
    width: 100%;
}
                   .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
    padding: 4px 4px;
    line-height: 1.42857143;
    vertical-align: middle;
    border-top: 1px solid #000;
    font-size: 12px;
}
                   .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
    padding: 4.5px 6px;
    line-height: 1.42857143;
    vertical-align: middle;
    border-top: 0px solid #000;
    font-size: 12px;
}
    @media print
{
        .myImg
    {
    display: none!important;
    width: 90 %;
    }
          .myImg
    {
        /*width: 80%;*/
        margin - left: auto;
        margin - right: auto;
    /*padding:120px;*/
    display: block;
    opacity: 1;
        z - index:1000;
    }
        .footer_section
    {
    display: none;
    }
        .table-responsive
    {
        min - height: .01 %;
        overflow - x: auto;
    width: 83 %;
        font - size: 13px;
    margin: 44px;
    top: 45em;
    position: absolute;
    }
        .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th
    {
    padding: 6px 6px;
        line - height: 1.42857143;
        vertical - align: middle;
        border - top: 0px solid #ddd;
    font - size: 12px;
    }
        .wrapper
    {
    display: none;
    }
        .sm-spacer
    {
    display: none;
    }
        .mybtn
    {
    display: none;
    }
        .Name
    {
    left: 22em;
    position: absolute;
    top: 19em;
    }
        .father
    {
    left: 5em;
    position: absolute;
    top: 21.5em;
    }
        .year
    {
    left: 20.5em;
    position: absolute;
    top: 24em;
    }
        .College
    {
    left: 8em;
    position: absolute;
    top: 26.3em;
    }
        .branch
    {
    left: 26em;
    position: absolute;
    top: 29em;
    }
        .MonthYear
    {
    left: 7em;
    position: absolute;
    top: 33.5em;
    }
        .pin
    {
    left: 7em;
    position: absolute;
    top: 35.9em;
    }
        .TrainingDuration
    {
    left: 10em;
    position: absolute;
    top: 38.3em;
    }
        .class {
    left: 10em;
    position: absolute;
    top: 40.8em;
}
        .container{
            padding:0px;
        }
            .container{
          position: relative;
            max-width: 800px;
            margin: 0 auto;
        }

            .container img
{
    vertical-align: middle;
}

            .container.content {
               position: absolute;

    bottom: 0;
    top:0px;
    background: rgb(0, 0, 0);
background: rgba(255, 255, 255, 0.36);
color: #130404;
    width: 100%;
    /*padding: 20px;*/
            }
            .time{
                display:none;
            }
            .transp{
                color:transparent;
            }
    }         
           .transp{
                color:transparent;
            }  
</style>"
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@" ";



            #endregion

            #region PageContent
            page += $@" <div class='container' >
        <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/memo.PNG'  class='myImg'>
        <div class='content'>
                   <div class='Name'>{{StudentData.Name}}</div>
            <div class='father'>Son/Daughter Of {{ StudentData.FatherName}}</div> 
            <div class='year'>{{StudentData.CourseDuration}}</div>
            <div class='College'>{{StudentData.CollegeName}}</div>
            <div class='branch'>{{StudentData.BranchName}}</div>
 <div class='year'>{{StudentData.year}}</div>
            <div class='MonthYear'>{{StudentData.MonthYear}}</div>
            <div class='TrainingDuration'>{{TrainingDuration}}</div>
            <div class='pin'>{{StudentData.Pin}}</div>
            <div class='class'>{{StudentData.PassedClass}}</div>
            <div class='row'>
                <div class='col-md-12'>
                    <div class='table-responsive'>
                        <table class='table'>
                            <thead>
                                <tr>
                                    <th rowspan = '2' width='28%' class='disp_none transp'>Examination</th>
                                    <th rowspan = '2' width='10%' class='disp_none'>Max Marks</th>
                                      <th rowspan = '2' width= '10%' class='text-center disp_none' >Marks Secured</th>
                                         <th colspan = '2' width= '40%' class='text-uppercase text-center disp_none'>Marks taken into Consideration for Award of Class</th>

                                    <!-- <th rowspan = '2' width='6%'>Re- marks</th>-->
                                </tr>
                                <tr>
                                    <th class='disp_none'>(In Figures)</th>
                                    <th class='disp_none'>(In Words)</th>

                                </tr>

                            </thead>
                            <tbody>

                                <tr ng-repeat='data in markstable'>
                                    <td width = '23%' class='transp'>{{data.Examination}}</td>
                                    <td class='highlight text-center' width='12%'>{{data.MaxMarks}}</td>
                                    <td class='highlight text-center' width='12%'>{{data.MaxSecured}}</td>
                                    <td class='highlight text-center' width='25%'>{{data.InFigures}} ({{data.Per}})</td>
                                    <td class='highlight'>{{data.InWords}}</td>
                                </tr>
                                <tr>
                                    <td class='total_border transp'>Total Marks In Figures</td>
                                    <td class='bord_top' colspan='2'></td>
                                    <td class='bord_top '><div class='text-center'><b>{{StudentData.TotalMarksInFigure}}</b></div></td>
                                    <td class='total_border'>{{StudentData.TotalMarksInFigure}}</td>
                                </tr>
                                <tr>
                                    <td class='total_border transp'>In Words</td>
                                    <td class='total_border text-center' colspan='4'><span class='highlight'>{{StudentData.TotalMarksInWords}}</span></td>
                                </tr>
                                <tr>
                                    <td class='total_border transp'>Percentage</td>
                                    <td class='total_border text-center' colspan='4'><span class='highlight'>{{StudentData.PercentageOfMarks}}</span></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
</div>
        <div class='sm-spacer'></div>
        <div class='col-md-12'>
            <button class='btn btn-success mybtn pull-right' ng-click='PrintData()'>Print</button>
        </div>
        </div>
";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir + "16001-m-001" + "_" + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");
            return relativePath;
        }
        //internal object GetTransferCertificate(DataSet ds)
        //{
        //    throw new NotImplementedException();
        //}

        public string isThreeBacklog(bool ThreeBacklog, string monthYear) {
            if (!ThreeBacklog)
            {               
                return $@" <span>during the Examination held in <b class='border_btm'> {monthYear} </b></span>";
            }
            else {
                return "";
            }

        }

        public string isThreeBacklogdivision(bool ThreeBacklog)
        {
            if (!ThreeBacklog)
            {
                return "";
            }
            else
            {
                return " division";
            }

        }

        public string IsGovClg(bool IsGovClg)
        {
            if (IsGovClg)
            {
                return $@" <div class='logo-image text-center' >
                  <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/telangana-logo-png.png' class='img-responsive logoImg pull-left' />
              </div>";
            }
            else
            {
                return "";
            }

        }


        public string GetTransferCertificate(DataSet TCDat)
        {
           List<TCData> TCData = TCDat.Tables[1].DataTableToList<TCData>().ToList();
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";

            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                          .logo-image {
       height:80px;
       width:80px;
         margin-left: auto;
  margin-right: auto;
   line-height: 1.6;
      
    }
  .border_btm{
     border-bottom:1px solid black;
      text-transform:uppercase;
 }
    .myfont{
        font-size:16px;
    }
     .myfont1{
         border-bottom:1px dashed black;
        font-size:16px;
    }
     .border_dot{
       
       width:auto;
   }
     .spacer{
         height:100px;
     }
      .sm-spacer{
         height:150px;
     }
     .length{
         padding:5px 0px;
     }
       .generateBtn{
    display:block;
}
  html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            m0argin-left: 10px;
                        }
.container{
max-width:800px;

}
.sm-spacer{
height:50px;
}
.ssm-spacer{
height:20px;
}
                        </style> "
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@"
 <div class='container'>
        <div class='row'>
<div class='col-md-12'>
{IsGovClg(TCData[0].IsGovClg)}
               
            </div>
            </div>
        <div class='row pad'>
        <div class='col-md-12 title'>
            <h3 class='text-center hall_head'><b>{TCData[0].CollegeName ?? "-"}</b></h3>
            
        </div>
        <div class='col-md-12'>
            <h5 class='text-center hall_head'><b>{TCData[0].CollegeAddress ?? "-"}</b></h5>
            
        </div>

        <h3 class='text-center hall_head '><b class='border_btm text-uppercase'>TRANSFER CERTIFICATE</b></h3>
            </div>
        </div>";

            #endregion

            #region PageContent
            page += $@" <div class='container'>
<div class='ssm-spacer'></div>
        <div class='row'>
            
            <div class='col-md-4'>
                <div class='myfont'>TC No: - {TCData[0].TransferCertificateNo ?? "-"}  </div>
 </div>
    <div class='col-md-3'></div>
    <div class='col-md-3'>
                    <div class='myfont'>Admission No: - {TCData[0].AdmissionNo ?? "-"} </div>
            </div>
        
               
           <div class='sm-spacer'></div>
           <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        1.   Name Of the Student(In Block Letters)
                            
                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].Name ?? "-"}</b>
                    </div>
                </div>
            </div>
            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        2.  PIN

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].Pin ?? "-"}</b>
                    </div>
                </div>
            </div>
            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        3.Father's Name

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].FatherName ?? "-"}</b>
                    </div>
                </div>
            </div>
<div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        4.Mother's Name

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].MotherName ?? "-"}</b>
                    </div>
                </div>
            </div>
          
 <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        5.Nationality & Religion

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].Nationality ?? "-"} & {TCData[0].Religion ?? "-"}</b>
                    </div>
                </div>
 </div>
            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        6.Caste

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].Caste ?? "-"}</b>
                    </div>
                </div>
            </div>
<div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        7.Date Of Birth(in Words)

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].DateOfBIrth.ToString("dd-MM-yyyy")}</b>
                    </div>
                </div>
            </div>
            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        8.Date of which he/she was admitted in the Polytechnic

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].admittedDate.ToString("dd-MM-yyyy") ?? "-"}</b>
                    </div>
                </div>
            </div>
<div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        9.Date on which he/she left the Polytechnic

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].LeftDate.ToString("dd-MM-yyyy") ?? "-"}</b>
                    </div>
                </div>
            </div>
              <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        10.Class in which he/she was studying at the time of leaving(in words)

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].LeftClass ?? "-"}</b>
                    </div>
                </div>
            </div>
    <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        11.Whether he/she has paid Course fees or other money due

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].CollegeDuesPaid ?? "-"}</b>
                    </div>
                </div>
            </div>
<div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        12.Whether qualified for promotion to higher Class

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].Promoted ?? "-"}</b>
                    </div>
                </div>
            </div>
            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        13.Conduct and Character

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].Conduct ?? "-"}</b>
                    </div>
                </div>
            </div>
      <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        14.Date on which application for Transfer Certificte was made

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].LogDate.ToString("dd-MM-yyyy")}</b>
                    </div>
                </div>
            </div>
            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        15.Whether he/she has appeared/final the
                           Exams and passed the Class/Discontinued

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>{TCData[0].ReasonForTc ?? "-"}</b>
                    </div>
                </div>
            </div>

            <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        16.Marks of Identification

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>:{TCData[0].IdMark1 ?? "-"}</b>
                        
                    </div>
                </div>
 <div class='col-md-5'>
                    <div class='myfont'>
                     

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>:{TCData[0].IdMark2 ?? "-"}</b>
                        
                    </div>
                </div>
                </div>
         <div class='row length'>
                <div class='col-md-5'>
                    <div class='myfont'>
                        17.Remarks

                    </div>
                </div>
                <div class='col-md-7'>
                    <div class='myfont1'>
                        <b class='border_dot'>:{TCData[0].StudentRemarks ?? "-"}</b>
                        
                    </div>
                </div>
                </div>

          <div class='sm-spacer'></div>
        <div class='sm-spacer'></div>

            <div class='row'>

                <div class='col-md-7'>

<div class='row length'>
                <div class='col-md-8'>
                    <div class='myfont'>
                        Date <b class='border_dot'>: {DateTime.Now.ToString("dd-MM-yyyy")}</b>

                    </div>
                </div>
               
                </div>
<div class='row length'>
                <div class='col-md-8'>
                    <div class='myfont'>
                        Station <b class='border_dot'>: {TCData[0].Station ?? "-"}</b>

                    </div>
                </div>
                
                </div>

                </div>

                <div class='col-md-5'>
                    <div class='col-md-12' style='margin-bottom: 40px;'></div>
				     <div class='col-md-12'>
				      <h5 class='text-center'><b class='text-uppercase '>Principal</b></h5>
				     </div>              
                </div>

            </div>

           
            <div class='sm-spacer'></div>
       
    </div>
    </div>";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir+ "TC" + TCData[0].Pin + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");

            //string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, HttpContext.Current.Request.ApplicationPath).Replace(@"\", "/");
            return relativePath;
        }


        public string GetMigrationCertificate(DataSet MigrationDat)
        {
            List<MigrationData> MigrationData = MigrationDat.Tables[1].DataTableToList<MigrationData>().ToList(); ;
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            margin-left: 10px;
                        } 
                        table {  
                            font-family: Helvetica, Arial, sans-serif; /* Nicer font */
                            width: 100%; 
                            border-collapse: collapse;
                            border-spacing: 0; 
                        } 

                        td, th, tr { border: 1px solid #000; height: 40px; } /* Make cells a bit taller */

                        th {  
                            font-weight: bold; /* Make sure they're bold */
                        }

                        td {  
                            text-align: center; /* Center our text */
                        }

                 .logoImg {
        height: 60px !important;
        width: 60px !important;
    }
 .myHr{
     border-top: 1px solid #000;
 }
 .header-top-section{
     display:none;
 }
 .border_btm{
     border-bottom:1px solid #ddd;
      text-transform:uppercase;
 }
 .qr_css{
     height:100px;
 }
 .text-uppercase{
     text-transform:uppercase;
 }
 p {
  text-indent: 50px;
}
   .marginData{
          margin:0px 20px;
      }
   .footer_section {
       display:none;
   }
    .footer_section {
       display:none;
   }

  .myImg {
        width: 70% !important;
        padding:120px;
        margin-left: auto !important;
        margin-right: auto !important;
        display: block!important;
        opacity:0.3;
    }
    .print_btn{
        display:none;
    }
     .spacer{
       display:none;
      }
     .logo img {
        float: left;
        cursor: pointer;
        margin-right: 0px!important;
    }

      .text-intend {
        text-indent: 50px;
        line-height: 2.5;
        padding-left:15px;
        text-align: justify;
        text-justify: inter-word;
    }
    }

/*---------------------*---------*-Print Css End-*------*-------------------------------*/

    .myImg {
    width: 60%;
    margin-left: auto;
    margin-right: auto;
    padding: 120px;
    display: block;
    opacity: 0.3;
    z-index: 1000;  
}

    .marginData {
        margin: 0px 20px;
        position:relative;
    }
     .container img {
            vertical-align: middle;
        }

        .container .content {
            position: absolute!important;
            top: 0!important;
            background: rgb(0, 0, 0)!important;
            background: rgba(255, 255, 255, 0.36)!important;
            color: #130404!important;
            width: 100%!important;
            padding: 20px!important;
        }
           .container{
          position: relative;
            max-width: 800px;
            margin: 0 auto;
            padding:0px!important;
        }
 
                        </style> "
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@"
 <div class='container'>

        <div class='row'>
            <div class='col-md-2 logo '>
                <div class='logo-image' style='padding:6px!important;'>
                    <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/telangana-logo-png.png' class='img-responsive logoImg pull-left' />
                </div>
            </div>
            <div class='col-md-8 title'>
                <h5 class='text-center hall_head' style='font-size: 20px!important;margin-left: -67px;margin-right: -53px;'><b>STATE BOARD OF TECHNICAL EDUCATION AND TRAINING</b></h5>
                                  <h4 class='text-center hall_head' style='font-size: 20px!important;'><b>TELANGANA</b></h4>
                <h6 class='text-center'>
                   Sanketika Vidya Bhavan, Masab Tank, Hyderabad­500 028, India.
                </h6>
                
            </div>
            <div class='col-md-2 logo '>
                <div class='logo-image' style='padding:6px!important;'>    
<img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' class='img-responsive logoImg pull-right' />             
                </div>
            </div>
        </div>
        <hr class='myHr' />
        <div>";

            #endregion

            #region PageContent
            page += $@"<div class='container'>
                <div class='marginData'>
                    <div class='row'>
                            <div class='col-md-10'>
                                <div><b class='border_btm'>Migration Certificate No</b>: {MigrationData[0].MigrationCertificateNo ?? "-"}.</div>                               
                            </div>
                       
                        <div class='col-md-2'>
                            
                    <img class='qr_css' src='{GenerateQrcode(MigrationData[0].Pin + ',' + MigrationData[0].Name)}' />
                        </div>
                    </div>
   <div>
                    <div class='row'>
                    <h3 class='text-center' style='font-size: 20px!important;'><b class='border_btm text-uppercase'>Migration Certificate</b></h3>
                     <div>
  <div class='container'>

            <img src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' alt='Notebook' class='myImg'>
            <div class='content'>";
 if (MigrationData[0].SchemeId != 5) {
                page += $@"<div class='row'>
                        <p class='text-intend'>
                            This is to certify that
                            <b class='border_btm' >{MigrationData[0].Name ?? "-"}</b> S/o/D/o<b class='border_btm' > {MigrationData[0].FatherName ?? "-"}</b> bearing Permanent Identification Number(PIN) <b class='border_btm' >{MigrationData[0].Pin ?? "-"}</b> has passed the<b class='border_btm' > {MigrationData[0].CourseDuration ?? "-"}</b> Diploma Course
                            in <b class='border_btm' >{MigrationData[0].BranchName ?? "-"}</b>{isThreeBacklog(MigrationData[0].Is3Backlog, MigrationData[0].MonthYear)} and he/she was placed in <b class='border_btm' >{MigrationData[0].PassedClass ?? "-"}</b>{isThreeBacklogdivision(MigrationData[0].Is3Backlog)}.
                            The medium of Instruction was <b>'English'</b>. </p>
                        <p class='text-intend'>
                            The State Board of Technical Education and Training, Telangana, Hyderabad has no Objection to prosecute his/her Higher Studies in any other
                           <b class='text-uppercase'>State / University.</b>
                        </p>
                        <p class='text-intend'>This Certificate is issued on the request of candidate.</p>

                    </div>";
   }else  if (MigrationData[0].SchemeId == 5) {
                    page += $@"<div class='row'>
                    <p class='text-intend'>
                        This is to certify that Mr./Ms.<b class='border_btm'> {MigrationData[0].Name ?? "-"}</b> Son/Daughter of <b class='border_btm' >{MigrationData[0].FatherName ?? "-"}</b>
                        bearing Permanent Identification Number(PIN) <b class='border_btm'>{MigrationData[0].Pin ?? "-"}</b> has <b class='border_btm' >Passed</b>
                        the <b class='border_btm'>{MigrationData[0].CourseDuration ?? "-"}</b> Diploma Course in <b class='border_btm'>{MigrationData[0].BranchName ?? "-"}</b> during the Examination held in <b class='border_btm'>{MigrationData[0].MonthYear ?? "-"}</b> at <b class='border_btm'>{MigrationData[0].CollegeCode ?? "-"} - {MigrationData[0].CollegeName ?? "-"}, {MigrationData[0].CollegeAddress}</b> 
and he/she is placed in <b class='border_btm'>{MigrationData[0].Class ?? "-"}</b> with
<b class='border_btm'>{MigrationData[0].CGPA ?? "-"}</b> Cumulative Grade Point Average <b>(CGPA)</b> on a scale of <b>10</b> and earned <b class='border_btm'>{MigrationData[0].TotalCreditsEarned ?? "-"}</b> credits. The medium of Instruction was <b>'English'</b>.</p>
                     <p class='text-intend'>
                            The State Board of Technical Education and Training, Telangana, Hyderabad has no Objection to prosecute his/her Higher Studies in any other
                           <b class='text-uppercase'>State / University.</b>
                        </p>
                        <p class='text-intend'>This Certificate is issued on the request of candidate.</p>

                </div>
<div class='sm-spacer'></div>
<div class='sm-spacer'></div>";
            }
            page += $@"<div class='sm-spacer'></div>

<div class='sm-spacer'></div>
<div class='sm-spacer'></div>
                        <div style='line-height: 2.0;'><b>Place : Hyderabad</b></div>
                    <div><b>Date : {DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
                </div>
            </di
       
            <div class='col-md-12'>
                <div class='btn btn-success print_btn pull-right' ng-click='printForm()'>Generate</div>
            </div>
</div>
            <div class='spacer'></div>
            
        </div>
    </div>";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir+"MC"+MigrationData[0].Pin + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");          
            return relativePath;
        }

        public string GetBonafideCertificate(DataSet BonafideDat)
        {
            List<BonafideData> BonafideData = BonafideDat.Tables[1].DataTableToList<BonafideData>().ToList(); ;
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\UnsignedCert\";
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            margin-left: 10px;
                        } 
                        table {  
                            font-family: Helvetica, Arial, sans-serif; /* Nicer font */
                            width: 100%; 
                            border-collapse: collapse;
                            border-spacing: 0; 
                        } 

                        td, th, tr { border: 1px solid #000; height: 40px; } /* Make cells a bit taller */

                        th {  
                            font-weight: bold; /* Make sure they're bold */
                        }

                        td {  
                            text-align: center; /* Center our text */
                        }

                 .logoImg {
        height: 60px !important;
        width: 60px !important;
    }
 .myHr{
     border-top: 1px solid #000;
 }
 .header-top-section{
     display:none;
 }
 .border_btm{
     border-bottom:1px solid #ddd;
      text-transform:uppercase;
 }
 .qr_css{
     height:100px;
 }
 .text-uppercase{
     text-transform:uppercase;
 }
 p {
  text-indent: 50px;
}
   .marginData{
          margin:0px 20px;
      }
   .footer_section {
       display:none;
   }
    .footer_section {
       display:none;
   }

  .myImg {
        width: 70% !important;
        padding: 0px 120px;
        margin-left: auto !important;
        margin-right: auto !important;
        display: block!important;
        opacity:0.3;
    }
    .print_btn{
        display:none;
    }
     .spacer{
       display:none;
      }
     .logo img {
        float: left;
        cursor: pointer;
        margin-right: 0px!important;
    }

      .text-intend {
        text-indent: 50px;
        line-height: 2.5;
        padding-left:15px;
        text-align: justify;
        text-justify: inter-word;
    }
  .under_margin{
        margin-bottom:30px;
    }
    }

/*---------------------*---------*-Print Css End-*------*-------------------------------*/

    .myImg {
    width: 60%;
    margin-left: auto;
    margin-right: auto;
    padding: 120px;
    display: block;
    opacity: 0.3;
    z-index: 1000;  
}

    .marginData {
        margin: 0px 20px;
        position:relative;
    }
     .container img {
            vertical-align: middle;
        }

        .container .content {
            position: absolute!important;
            top: 0!important;
            background: rgb(0, 0, 0)!important;
            background: rgba(255, 255, 255, 0.36)!important;
            color: #130404!important;
            width: 100%!important;
            padding: 20px!important;
        }
           .container{
          position: relative;
            max-width: 800px;
            margin: 0 auto;
            padding:0px!important;
        }
  .under_margin{
        margin-bottom:30px;
    }
 
                        </style> "
                   + "</head><body>";

            string sbString = html;


            #region PageHeader
            var page = $@"
 <div class='container'>
<div class='row' >
             <div class='col-md-5'> </div>
            <div class='col-md-2'>
                <div class='logo-image text-center'>
                    {IsGovClg(BonafideData[0].IsGovClg)}
                </div>
            </div>
            <div class='col-md-5'> </div>
            </div>
     

        <div class='row pad'>
            <div class='col-md-12 title'>
                <h3 class='text-center hall_head'><b> {BonafideData[0].College_Name}</b></h3>

            </div>
            <div class='col-md-12'>
                <h5 class='text-center hall_head'><b>{BonafideData[0].Clg_Address}</b></h5>

            </div>

        </div>";
            if (BonafideData[0].ServiceType == 91){
      page += $@"<div class='row'>
        <h3 class='text-center under_margin'><b class='border_btm  text-uppercase'>STUDY-CUM-CONDUCT CERTIFICATE</b></h3>
    </div>";
      }else{
         page += $@"<div class='row'>
            <h3 class='text-center under_margin'><b class='border_btm  text-uppercase'>BONAFIDE CERTIFICATE</b></h3>
        </div>";
        }


            page += $@"</div>";

            #endregion

            #region PageContent
            page += $@" <div class='container'>
       
      <!--  <hr class='myHr' />-->
        <div class='marginData'>
            <div class='row'>
                <div class='col-md-9'>
                    <div>SL NO : <b class=''>{BonafideData[0].ApplicationNumber ?? "-"}</b></div>
                </div>
                <div class='col-md-3'>
                    <div>Date : <b class=''>{DateTime.Now.ToString("dd-MM-yyyy")}</b></div>
                </div>
            </div>
        </div>
       
        <div class='container'>

            <img  src = '{AppDomain.CurrentDomain.BaseDirectory}/contents/img/sbtet-logo.png' alt='Notebook' class='myImg'>
            <div class='content'>
                <div class='row'>
                    <p class='text-intend'>
    
This is to certify that Mr/Ms<b class='border_btm' > {BonafideData[0].Name ?? "-"}</b> S/o/D/o <b class='border_btm' > {BonafideData[0].FatherName ?? "-"},</b> bearing PIN : <b class='border_btm' >{BonafideData[0].Pin ?? "-"}</b> a student of this institution, has";
            if (BonafideData[0].ServiceType == 91)
            {
                page += $@"<span> studied</span>";
            }
            else
            {
                page += $@"<span> studying</span>";
            }
                page += $@" the<b class='border_btm' > Diploma in {BonafideData[0].BranchName ?? "-"}</b>
                     during the Academic Years<b class='border_btm' > {BonafideData[0].AcademicYear ?? "-"}.</b><br>
                        </p>
                    <p class='text-intend'> During this period his/her conduct and character have been found to be<b class='border_btm' > {BonafideData[0].conduct ?? " - "}</b>.


                </p>
                </div>
                <div class='sm-spacer'></div>
                          
            </div>
            
            <div class='sm-spacer'></div>
                        
        </div>
        
        <div class='container'>
            <div class='col-md-8'>
                <h5 class='text-center'><b class='text-uppercase '></b></h5>
            </div>
            
            <div class='col-md-4'>
                <h5 class='text-center'><b class='text-uppercase '>PRINCIPAL</b></h5>
            </div>
        </div>
            <div class='sm-spacer'></div>
        </div>";
            #endregion

            sbString += page;
            sbString += "</body></html>";

            var converter = new HtmlToPdf();
            converter.Options.ExternalLinksEnabled = true;
            converter.Options.DisplayHeader = true;
            converter.Options.DrawBackground = false;
            converter.Options.JavaScriptEnabled = false;
            converter.Options.WebPageWidth = 1024;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var doc = converter.ConvertHtmlString(sbString);
            var path = dir + "SC" + BonafideData[0].Pin + $"{Guid.NewGuid().ToString()}.pdf";
            doc.Save(path);
            doc.Close();
            string relativePath = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, GetWebAppRoot()).Replace(@"\", "/");
            return relativePath;
        }

        public string GetODCTrSheets(List<ODCTrSheetData> ODCTrSheetData)
        {
            var dir_id = Guid.NewGuid().ToString();
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\ODCTR\" + dir_id;
            var path = string.Empty;
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + @"<style type='text/css'>
                         html{
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                        }
                        body {
                            min-width: 1024px;
                            max-width: 1024px;
                            width: 1024px;
                            margin-left: 10px;
                        }
                        thead {
                            border-top: 1px solid;
                            border-bottom: 1px solid;
 
                        }
                        th.text-center.clmWth {
                            width: 16%;
                        }
                      .container-fluid {
                                    font-size: 14px ;
                                    margin: 10px!important;
                                    font-family: monospace!important;
                                }
                                .table > thead > tr {
                                    padding: 2px;
                                }


                                .table > thead > tr > th.myClm {
                                    width: 25% !important;
                                    border-top: 0px !important;
                                }

                                .table > thead > tr > th {
                                   padding: 0px 2px !important;
                                   font-weight: unset !important;
                                    font-size: 14px;
                                }

                                .table > tbody > tr > td {
                                    border-top: 0px !important;
                                    border-bottom: 1px dashed #000 !important;
                                  padding: 0px 2px !important;
                                    font-size: 14px;
                                }

                                .table > thead > tr > th.cln {
                                    /*border-bottom: 0px!important;*/
                                    border-bottom: 0px !important;
                                }

                                .table > thead > tr > th.clm {
                                    /*border-bottom: 0px!important;*/
                                    border-top: 0px !important;
                                }

                                .table > tbody > tr > td.cln {
                                    /*border-bottom: 0px!important;*/
                                    border-bottom: 0px !important;
                                }

                                .footer_section {
                                    display: none;
                                }

                                .top-header {
                                    display: none;
                                }
                                .btm_line{
                                    position: fixed;
                                    bottom: 0px;
                                    z-index: 99;
                                    border-bottom :2px solid #000;
                                }
      }
                        </style> "
                   + "</head><body>";

            string sbString = html;
            var page = string.Empty;
            var pgno = 1;
            var distinctcol = ODCTrSheetData.GroupBy(x => x.CEN)
                                          .Select(grp => grp.First())
                                          .OrderBy(x => x.CEN)
                                          .Distinct()
                                          .ToList();
            foreach (var col in distinctcol)
            {
                var branchdata = ODCTrSheetData.Where(x => x.CEN == col.CEN)
                                                .GroupBy(x => x.BR)
                                                .Select(g => g.First())
                                                .OrderBy(x => x.BR)
                                                .ToArray();
                foreach (var branch in branchdata)
                {
                    ; 
                    var studentarrdata = ODCTrSheetData.Where(x => x.CEN == col.CEN && x.BR == branch.BR)
                                                    .OrderBy(x => x.BR)
                                                    .OrderBy(x => x.PIN)
                                                    .Distinct()
                                                    .ToArray();


                    var groupedItems = SliceArray(studentarrdata, 10);

                    //(CURRICULUM - X)

                    foreach (var studentdata in groupedItems)
                    {
                        #region PageHeader
                        page = $@"<div class='container-fluid'>
                                   <div class='text-center'>STATE BOARD OF TECHNICAL EDUCATION & TRAINING - T.S.HYDERABAD</div>
                                    <div class='col-md-3 pull-right'>Page : {pgno}</div>
                                    <div class='text-center'>PROVISIONAL SUMMARY OF CUMULATIVE RECORDS OF THE CANDIDATE</div>
                                    <div class='text-center'>-------------------SCHEME - {branch.scheme}---------------------------</div>
                                    <div class='col-md-8'>{branch.COURSE} DIPLOMA COURSE IN {branch.BRANCH_NAME}</div>
                                    <div class='col-md-4 pull-right'>EXAMINATION HELD : {branch.MONTH_YEAR}</div>
                                    <div class='col-md-12'>INSTITUTE CODE & NAME :  {branch.CEN}- {branch.CEN_NAME},{branch.CEN_ADDRESS}</div>
                                   <hr class='myHr'/>
                            </div>";
                        #endregion
                        #region PageContent
                        page += $@"<div class='container-fluid'>
                         <table class='table'>
                                    <thead>
                                    <tr>
                                        <th class='text-center' rowspan='2' >P.C.NO</th>
                                        <th class='text-left' style='width:30%;border-bottom:0px!important;' >Name</th>
                                        <th class='text-left clmWth' rowspan='2' >PIN</th>
                                        <th class='text-center cln' > 1SEM </th>
                                        <th class='text-center cln'> 25% </th>
                                        <th class='text-center cln' > 2SEM </th>
                                        <th class='text-center cln'> 25% </th>
                                        <th class='text-center cln'> 3SEM</th>
                                        <th class='text-center  cln'> 4SEM</th>
                                        <th class='text-center cln'> 5SEM</th>
                                        <th class='text-center cln'> 6SEM</th>
                                        <th class='text-center cln'> Total</th>
                                        <th class='text-center cln'> Percent</th>
                                    </tr>
                                    <tr>
                                        <th class='text-left' style='width:30%;border-bottom:0px!important;' >Father Name</th>
                                        <th class='text-center clm' >{branch.MAX_MARKS_1SEM}</th>
                                        <th class='text-center clm'>{Convert.ToInt32(branch.MAX_MARKS_1SEM) / 4}</th>
                                        <th class='text-center clm' >{branch.MAX_MARKS_2SEM}</th>
                                        <th class='text-center clm'>{Convert.ToInt32(branch.MAX_MARKS_2SEM) / 4}</th>
                                        <th class='text-center clm'>{branch.MAX_MARKS_3SEM}</th>
                                        <th class='text-center clm'>{branch.MAX_MARKS_4SEM}</th>
                                        <th class='text-center clm'>{branch.MAX_MARKS_5SEM}</th>
                                        <th class='text-center clm'>{branch.MAX_MARKS_6SEM}</th>
                                        <th class='text-center clm'>total</th>
                                        <th class='text-center clm'>Class</th>
                                    </tr>
                                </thead>";
                        page += @"<tbody>";

                        for (var i = 0; i < studentdata.Length; i++)
                        {                           
                            page += $@"<tr>
                                        <td class='text-center' rowspan='2'> {studentdata[i].SNO}</td>
                                        <td class='text-left cln'>{studentdata[i].NAME}</td>
                                        <td class='text-center cln' >{studentdata[i].PIN}</td>
                                        <td class='text-center cln' >{studentdata[i].TOTAL1S}</td>
                                        <td class='text-center cln'>{studentdata[i].TOTAL1S_25}</td>
                                        <td class='text-center' rowspan='2'>{studentdata[i].TOTAL2S}</td>
                                        <td class='text-center cln' >{studentdata[i].TOTAL2S_25}</td>
                                        <td class='text-center cln' >{studentdata[i].TOTAL3S}</td>
                                        <td class='text-center cln' >{studentdata[i].TOTAL4S}</td>
                                        <td class='text-center cln' >{studentdata[i].TOTAL5S}</td>
                                        <td class='text-center cln' >{studentdata[i].TOTAL6S}</td>
                                        <td class='text-center cln' >{studentdata[i].GRAND_TOTAL}</td>
                                        <td class='text-center cln' >{studentdata[i].PER}</td>                 
                                    </tr>              
                                    <tr>
                                        <td class='text-left' colspan='4'>{studentdata[i].FNAME}</td>
                                        <td class='text-center' colspan='7'>{studentdata[i].CLASS}</td>
                                    </tr>";

                        }
                        page += "</tbody></table></div>";
                        #endregion
                        #region PageFooter
                        page += $@"
                            <div class='container-fluid'>
                                <div>Note : 1) CLASSIFICATION FOR AWARD OF CLASS BASED ON TOTAL OF 25% OF I SEM &  II SEM, 100% OF REMAINING SEMESTERS.</div>
                                <div>2) CLASSIFICATION FOR AWARD OF CLASS BASED ON TOTAL OF 100% OF III SEM TO REMAINING SEMESTERS FOR IVC CANDIDATES.</div>
                                <div>* - RULE 12</div>
        
                                <div class='col-md-3'>TOT NO OF CORR : NIL </div>
                                    <div class='col-md-1'>ASST.</div>
                                <div class='col-md-1'>SUPDT.</div>
                                <div class='col-md-2'>SECRETARY/DY.</div>
                            <div class='col-md-2'>SECRETARY</div>
                                <div class='col-md-3'>CONTROLLER OF EXAMINATION</div>
                            </div>";
                        #endregion

                        sbString += page;
                        sbString += "</body></html>";
                        var converter = new HtmlToPdf();
                        converter.Options.ExternalLinksEnabled = true;
                        converter.Options.DrawBackground = false;
                        converter.Options.JavaScriptEnabled = false;
                        converter.Options.WebPageWidth = 1024;
                        converter.Options.PdfPageSize = PdfPageSize.A4;
                        converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                        var doc = converter.ConvertHtmlString(sbString);
                        path = dir + $"\\{pgno.ToString().PadLeft(6, '0')}.pdf";
                        doc.Save(path);
                        doc.Close();
                        sbString = html;
                        pgno++;
                    }
                }
            }
            return dir_id;
        }


        public string tabledata(string val1, string val2, string val3,string val4,string val5) {
            if (val4 == "*" && val5 != "__")
            {
                return $@" <td class='cln text-center'>{val1}</td>
                                    <td class='cln text-center'>{val2}</td>
                                    <td class='cln text-center'>{val3}<sup>*</sup></td>";
            }
            else if (val4 == "*" && val5 == "__")
            {
                return $@" <td class='cln text-center dotted'>{val1}</td>
                                    <td class='cln text-center dotted'>{val2}</td>
                                    <td class='cln text-center dotted'>{val3}<sup>*</sup></td>";
            }
            else if (val4 != "*" && val5 == "__")
            {
                return $@" <td class='cln text-center dotted'>{val1}</td>
                                    <td class='cln text-center dotted'>{val2}</td>
                                    <td class='cln text-center dotted'>{val3}</td>";
            }
            else {
                return $@" <td class='cln text-center'>{val1}</td>
                                    <td class='cln text-center'>{val2}</td>
                                    <td class='cln text-center'>{val3}</td>";
            }
        }
        public string GetTrSheets(List<TrSheetCertificateData> TrSheetData)
        {
            var dir_id = Guid.NewGuid().ToString();
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\TR\" + dir_id;          
            var path = string.Empty;
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   +$@"<script src= '{AppDomain.CurrentDomain.BaseDirectory}\scripts\jquery-3.3.1.min.js'></script>"
                    + @"<style type='text/css'>
                            .myHr {
                                color: #000;
                                border-bottom: 2pX solid #000;
                            }
                           
                            .table > thead > tr > th {
                                vertical-align: bottom;
                                border-top: 1px solid #000 !important;
                                padding: 2px 6px !important;
                            }
                            .container-fluid{
                                padding: 6px 6px !important;
                                margin: 10px!important;
                                min-height: 100% !important;
                                font-family: monospace!important;
                            }

                            .table > tbody > tr > td {
                                vertical-align: bottom;
                                border-bottom: 1px dashed #000 !important;
                                padding: 2px 6px !important;
                            }

                            .table > thead > tr > th.myClm {
                                width: 25% !important;
                                border-top: 0px !important;
                            }

                            .table > tbody > tr > td {
                                border-top: 0px !important;
                                border-bottom: 1px dashed #000 !important;
                            }

                            .table > thead > tr > th.cln {
                                /*border-bottom: 0px!important;*/
                                border-bottom: 0px !important;
                            }

                            .table > thead > tr > th.clm {
                                /*border-bottom: 0px!important;*/
                                border-top: 0px !important;
                            }

                            .table > tbody > tr > td.cln {
                                /*border-bottom: 0px!important;*/
                                border-bottom: 0px !important;
                            }
                            .btm_line{
                            border-bottom :2px solid #000 !important;
                            }
                            .sm-spacer {
                                margin: 12px;
                            }
                            .sm1-spacer {
                                margin: 15px;
                            }
                            sup {
                              font-size: inherit !important;                            
                            }
                            .dotted{
                                font-weight: 100;
                                text-decoration: underline;
                                text-decoration-style:dashed; 
                                font-size: 0.94em;
                            }
                            .footer {
                               position: fixed;                              
                               bottom: 0;
                               width: 100%;                     
                               text-align: center;
                            }
                   
                            @media print {
                                .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12 {
                                    float: left;
                                }

                                .col-md-12 {
                                    width: 100%;
                                }

                                .col-md-11 {
                                    width: 91.66666666666666%;
                                }

                                .col-md-10 {
                                    width: 83.333333%;
                                }

                                .col-md-9 {
                                    width: 75%;
                                }

                                .col-md-8 {
                                    width: 66.66666666666666%;
                                }

                                .col-md-7 {
                                    width: 58.333333333333336%;
                                }

                                .col-md-6 {
                                    width: 50%;
                                }

                                .col-md-5 {
                                    width: 41.66666666666667%;
                                }

                                .col-md-4 {
                                    width: 33.33333333333333%;
                                }

                                .col-md-3 {
                                    width: 25%;
                                }

                                .col-md-2 {
                                    width: 16.666666666666664%;
                                }

                                .col-md-1 {
                                    width: 8.333333333333332%;
                                }

                                .container-fluid {
                                    font-size: 9px ;
                                    margin: 10px!important;
                                    font-family: monospace!important;
                                }
                                .table > thead > tr {
                                    padding: 2px;
                                }


                                .table > thead > tr > th.myClm {
                                    width: 25% !important;
                                    border-top: 0px !important;
                                }

                                .table > thead > tr > th {
                                   padding: 2px 6px !important;
                                }

                                .table > tbody > tr > td {
                                    border-top: 0px !important;
                                    border-bottom: 1px dashed #000 !important;
                                  padding: 2px 6px !important;
                                }

                                .table > thead > tr > th.cln {
                                    /*border-bottom: 0px!important;*/
                                    border-bottom: 0px !important;
                                }

                                .table > thead > tr > th.clm {
                                    /*border-bottom: 0px!important;*/
                                    border-top: 0px !important;
                                }

                                .table > tbody > tr > td.cln {
                                    /*border-bottom: 0px!important;*/
                                    border-bottom: 0px !important;
                                }

                                .footer_section {
                                    display: none;
                                }

                                .top-header {
                                    display: none;
                                }
                                .btm_line{
                                    position: fixed;
                                    bottom: 0px;
                                    z-index: 99;
                                    border-bottom :2px solid #000;
                                }
                            }
                        </style>"
                         + "</head><body>";

            string sbString = html;
            var page = string.Empty;
            var pgno = 1;
            var distinctcenters = TrSheetData.GroupBy(x => x.CENTRE)                                         
                                            .Select(grp => grp.First())
                                            .OrderBy(x => x.CENTRE)
                                            .Distinct()
                                            .ToList();
            foreach (var cen in distinctcenters)
            {
                var branchdata = TrSheetData.Where(x => x.CENTRE == cen.CENTRE)
                                                .GroupBy(x => x.BRANCH)
                                                .Select(g => g.First())
                                                .OrderBy(x => x.BRANCH)                                              
                                                .ToArray();
                foreach (var branch in branchdata)
                {
              
               
               
                var tempstudentdata = new List<TrSheetCertificateData>();
                  
                var studentarrdata = TrSheetData.Where(x => x.CENTRE == cen.CENTRE && x.BRANCH == branch.BRANCH)                                              
                                                .OrderBy(x => x.BRANCH)                                               
                                                .OrderBy(x => x.PIN)
                                                .Distinct()
                                                .ToArray();

                   
                var groupedItems = SliceArray(studentarrdata,7);

                    foreach (var studentdata in groupedItems)
                        {

                        #region PageHeader
                        page = $@"<div class='container-fluid sm-spacer'>                            
                                <div class='col-md-9 text-center'>STATE BOARD OF TECHNICAL EDUCATION & TRAINING - T.S. HYDERABAD </div>            
                                <div class='col-md-3 pull-right'>Page : {pgno}</div>
                                <div class='col-md-9'>{branch.YEAR} ({branch.SCHEME}) EXAMINATION FOR 3 YEARS DIPLOMA COURSE IN {branch.BRANCH_NAME}</div>
                                <div class='col-md-3'>RUN DATE : {DateTime.Now.ToString("dd-MM-yyyy")}</div>
                                <div class='col-md-4'>TABULATED MARKS SHEET</div>
                                <div class='col-md-8'>CENTER : {branch.CENTRE}-{branch.INST_NAME}, {branch.INST_ADDRESS}</div>
                                <div class='col-md-12'>MONTH & YEAR OF EXAM :  {branch.MONTH_YEAR}</div>             
                                <hr class='myHr' />";
                        #endregion
                        #region PageContent
                        page += $@"<table class='table'>
                        <thead>
                            <tr>
                                <th class='text-center'></th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-cente'>E</th>
                                <th class='text-center'>S</th>
                                <th class='text-center'>TOT</th>

                                <th class='text-center' colspan='3'>TOTAL</th>
                            </tr>
                            <tr>
                                <th class='text-center'>max</th>
                                <th class='text-center'>{branch.SUB1_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB1_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB1_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB2_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB2_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB2_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB3_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB3_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB3_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB4_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB4_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB4_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB5_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB5_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB5_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB6_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB6_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB6_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB7_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB7_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB7_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB8_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB8_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB8_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB9_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB9_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB9_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB10_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB10_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB10_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB11_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB11_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB11_MAX_TOTAL}</th>
                                <th class='text-center'>{branch.SUB12_MAX_END_MARKS}</th>
                                <th class='text-center'>{branch.SUB12_MAX_SESSIONAL}</th>
                                <th class='text-center'>{branch.SUB12_MAX_TOTAL}</th>
                                <th class='text-center'  colspan='3'>{branch.MAX_TOTAL_TOTAL}</th>
                            </tr>
                            <tr>
                                <th class='text-center'>min</th>
                                <th class='text-center'>{branch.Sub1_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub1_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub2_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub2_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub3_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub3_TotalMinMarks}</th>                                
                                <th class='text-center'>{branch.Sub4_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub4_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub5_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub5_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub6_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub6_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub7_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub7_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub8_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub8_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub9_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub9_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub10_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub10_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub11_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub11_TotalMinMarks}</th>
                                <th class='text-center'>{branch.Sub12_EndMinMarks}</th>
                                <th class='text-center'></th>
                                <th class='text-center'>{branch.Sub12_TotalMinMarks}</th>
                                <th class='text-center' colspan='3'></th>
                            </tr>

                            <tr>
                            <th class='text-center'>SUB</th>
                            <th class='text-center' colspan='3'>{branch.SUB1_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB2_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB3_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB4_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB5_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB6_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB7_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB8_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB9_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB10_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB11_CODE}</th>
                            <th class='text-center' colspan='3'>{branch.SUB12_CODE}</th>
                            <th class='text-center' colspan='3'>RESULT</th>
                        </tr>
                        <tr>
                            <th class=''colspan='4'>SLNO</th>
                            <th class='' colspan='6'>PIN</th>
                            <th class='' colspan='12'>NAME</th>
                            <th class='' colspan='12'>FATHER NAME</th>
                            <th class='' colspan='3'>SEX</th>
                            <th class='' colspan='3'>RESULT</th>
                        </tr>
                    </thead>";
                    page += @"<tbody>";

                        for (var i = 0; i < studentdata.Length; i++)
                            {
                                tempstudentdata.Add(studentdata[i]);
                                page += $@"<tr>
                                    <td class='cln' colspan='4'>{tempstudentdata.Count}</td>
                                    <td class='cln' colspan='6'>{studentdata[i].PIN}</td>
                                    <td class='cln' colspan='12'>{studentdata[i].NAME}</td>
                                    <td class='cln' colspan='12'>{studentdata[i].FATHERNAME ?? "-"}</td>
                                    <td class='cln' colspan='3'>{studentdata[i].GENDER ?? "-"}</td>
                                    <td class='cln' colspan='3'>{studentdata[i].RESULT}</td>
                                </tr>
                                <tr> 
                                    <td class='cln text-center'></td>
                                        {tabledata(studentdata[i].SUB1_AWARDED_END_MARKS, studentdata[i].SUB1_AWARDED_SESSIONAL, studentdata[i].SUB1_AWARDED_TOTAL, studentdata[i].R1, studentdata[i].SUB1_P_F)}
                                        {tabledata(studentdata[i].SUB2_AWARDED_END_MARKS, studentdata[i].SUB2_AWARDED_SESSIONAL, studentdata[i].SUB2_AWARDED_TOTAL, studentdata[i].R2, studentdata[i].SUB2_P_F)}
                                        {tabledata(studentdata[i].SUB3_AWARDED_END_MARKS, studentdata[i].SUB3_AWARDED_SESSIONAL, studentdata[i].SUB3_AWARDED_TOTAL, studentdata[i].R3, studentdata[i].SUB3_P_F)}
                                        {tabledata(studentdata[i].SUB4_AWARDED_END_MARKS, studentdata[i].SUB4_AWARDED_SESSIONAL, studentdata[i].SUB4_AWARDED_TOTAL, studentdata[i].R4, studentdata[i].SUB4_P_F)}
                                        {tabledata(studentdata[i].SUB5_AWARDED_END_MARKS, studentdata[i].SUB5_AWARDED_SESSIONAL, studentdata[i].SUB5_AWARDED_TOTAL, studentdata[i].R5, studentdata[i].SUB5_P_F)}
                                        {tabledata(studentdata[i].SUB6_AWARDED_END_MARKS, studentdata[i].SUB6_AWARDED_SESSIONAL, studentdata[i].SUB6_AWARDED_TOTAL, studentdata[i].R6, studentdata[i].SUB6_P_F)}
                                        {tabledata(studentdata[i].SUB7_AWARDED_END_MARKS, studentdata[i].SUB7_AWARDED_SESSIONAL, studentdata[i].SUB7_AWARDED_TOTAL, studentdata[i].R7, studentdata[i].SUB7_P_F)}
                                        {tabledata(studentdata[i].SUB8_AWARDED_END_MARKS, studentdata[i].SUB8_AWARDED_SESSIONAL, studentdata[i].SUB8_AWARDED_TOTAL, studentdata[i].R8, studentdata[i].SUB8_P_F)}
                                        {tabledata(studentdata[i].SUB9_AWARDED_END_MARKS, studentdata[i].SUB9_AWARDED_SESSIONAL, studentdata[i].SUB9_AWARDED_TOTAL, studentdata[i].R9, studentdata[i].SUB9_P_F)}
                                        {tabledata(studentdata[i].SUB10_AWARDED_END_MARKS, studentdata[i].SUB10_AWARDED_SESSIONAL, studentdata[i].SUB10_AWARDED_TOTAL, studentdata[i].R10, studentdata[i].SUB10_P_F)}
                                        {tabledata(studentdata[i].SUB11_AWARDED_END_MARKS, studentdata[i].SUB11_AWARDED_SESSIONAL, studentdata[i].SUB11_AWARDED_TOTAL, studentdata[i].R11, studentdata[i].SUB11_P_F)}
                                        {tabledata(studentdata[i].SUB12_AWARDED_END_MARKS, studentdata[i].SUB12_AWARDED_SESSIONAL, studentdata[i].SUB12_AWARDED_TOTAL, studentdata[i].R12, studentdata[i].SUB12_P_F)}                                     

                                    <td class='cln text-center'  colspan='3'>{studentdata[i].AWARDED_TOTAL_TOTAL}</td>
                                </tr>
                                <tr>
                                     <td colspan='22'></td>
                                     <td colspan='18'>RE: {studentdata[i].RE}</td>
                                </tr>"; 
                        }

                        page += "</tbody> </table>";

                        if (tempstudentdata.Count == studentarrdata.Length)
                        {
                            page += $@"<div class='row'>
                                <div class='col-md-4'>{branch.SUB1_CODE}-{branch.SUB1_NAME}</div>
                                <div class='col-md-4'>{branch.SUB2_CODE}-{branch.SUB2_NAME}</div>
                                <div class='col-md-4'>{branch.SUB3_CODE}-{branch.SUB3_NAME}</div>
                                <div class='col-md-4'>{branch.SUB4_CODE}-{branch.SUB4_NAME}</div>
                                <div class='col-md-4'>{branch.SUB5_CODE}-{branch.SUB5_NAME}</div>
                                <div class='col-md-4'>{branch.SUB6_CODE}-{branch.SUB6_NAME}</div>
                                <div class='col-md-4'>{branch.SUB7_CODE}-{branch.SUB7_NAME}</div>
                                <div class='col-md-4'>{branch.SUB8_CODE}-{branch.SUB8_NAME}</div>
                                <div class='col-md-4'>{branch.SUB9_CODE}-{branch.SUB9_NAME}</div>
                                <div class='col-md-4'>{branch.SUB10_CODE}-{branch.SUB10_NAME}</div>
                                <div class='col-md-4'>{branch.SUB11_CODE}-{branch.SUB11_NAME}</div>
                                <div class='col-md-4'>{branch.SUB12_CODE}-{branch.SUB12_NAME}</div>
                            </div>";
                            #endregion
                        }                    


                        #region PageFooter
                        page += $@" <div class='sm1-spacer'></div>
                            <div class='btm_line footer'>
                            <div class='col-md-1'>E- ENDEXAM </div> 
                            <div class='col-md-1'>S- SESS</div>
                            <div class='col-md-2'>*- EARLIER PASS</div>
                            <div class='col-md-2'>TOT NO OF CORR: NIL</div>
                            <div class='col-md-1'>ASST.</div>
                            <div class='col-md-1'>SUPDT.</div>
                            <div class='col-md-2'>ASST/SECRETARY.</div>
                            <div class='col-md-2'>CONTROLLER OF EXAMINATION</div>
                        </div></div>";
                        #endregion
                       
                        sbString += page;
                        sbString += "</body></html>";
                        var converter = new HtmlToPdf();
                        converter.Options.ExternalLinksEnabled = true;
                        converter.Options.DrawBackground = false;
                        converter.Options.JavaScriptEnabled = false;
                        converter.Options.WebPageWidth = 1024;
                        converter.Options.PdfPageSize = PdfPageSize.A4;
                        converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                        var doc = converter.ConvertHtmlString(sbString);
                        path = dir + $"\\{pgno.ToString().PadLeft(6, '0')}.pdf";
                        doc.Save(path);
                        doc.Close();
                        sbString = html;
                        pgno++;
                    }
                    tempstudentdata.Clear();
                }
            } 
            return dir_id;
        }


        private void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public string GenerateQrcode(string QRcodeUID)
        {
            string dirPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\BarcodeImages\";
            if (!File.Exists(dirPath + QRcodeUID + ".jpg"))
            {
                System.Drawing.Bitmap qrCodeImage = null;
                if (QRcodeUID.Length > 0)
                {

                    QRCodeGenerator _qrCode = new QRCodeGenerator();
                    QRCodeData _qrCodeData = _qrCode.CreateQrCode(QRcodeUID, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(_qrCodeData);
                    qrCodeImage = qrCode.GetGraphic(20);

                }
                CreateIfMissing(dirPath);
                qrCodeImage?.Save(dirPath + QRcodeUID + ".jpg", ImageFormat.Jpeg);
                qrCodeImage?.Dispose();
            }
            return dirPath + QRcodeUID + ".jpg";
        }
    }
}
