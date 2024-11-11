using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PdfSharp.Pdf.IO;
using SelectPdf;
using SoftwareSuite.Models.PreExamination;

namespace SoftwareSuite.Controllers.PreExamination
{
    public class SeatingPlan
    {
        
        private void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
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

        public string SeatingPlanPdf(List<SeatingPlanData> SeatingData)
        {
            string dirPath = AppDomain.CurrentDomain.BaseDirectory + @"Reports\Seating";
            CreateIfMissing(dirPath);
            var dir_id = SeatingPdfs(SeatingData);
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\Seating\" + dir_id;
            var files = Directory.GetFiles(dir);
            var pdf = "SeatingPlan_"+Guid.NewGuid().ToString();
            MergePDFs(AppDomain.CurrentDomain.BaseDirectory + @"Reports\" + pdf + ".pdf", files);
            Directory.Delete(dir, true);
            return pdf;
        }
        public string SeatingPdfs(List<SeatingPlanData> SeatingData)
        {
            var dir_id = Guid.NewGuid().ToString();
            var dir = AppDomain.CurrentDomain.BaseDirectory + @"Reports\Seating\" + dir_id;
            var path = string.Empty;
            CreateIfMissing(dir);
            string html = @"<html>"
                   + "<head>"
                   + $"<title></title>"
                   + $@"<link href = '{AppDomain.CurrentDomain.BaseDirectory}\contents\css\bootstrap4.min.css' rel = 'stylesheet'  type = 'text/css' />"
                   + $@"<script src= '{AppDomain.CurrentDomain.BaseDirectory}\scripts\jquery-3.3.1.min.js'></script>"
                    + @"<style type='text/css'>
                             table > thead > tr > th {
                                 padding: 4px 2px;
								border: 1px solid;
                            }
                            .container-fluid{
                                padding: 6px 6px !important;
                                margin: 10px!important;
                               min-height: unset;
                            }

                            table > tbody > tr > td {
                              padding: 4px 2px;
								border: 1px solid;
                            }
                           .col{
								margin-left: 15px;
								margin-right: 15px
						   }
                           
                        </style>"
                         + "</head><body>";

            string sbString = html;
            var page = string.Empty;
            var pgno = 1;
            var colstyle = 3;
            float width = 1;
            var fntsz = "15 px";
          
            var distinctHalls = SeatingData.GroupBy(x => x.HallName)
                                            .Select(grp => grp.First())                                           
                                            .Distinct()
                                            .ToList();
            foreach (var hall in distinctHalls)
            {
                var hallcolumns = SeatingData.Where(x => x.HallName == hall.HallName)
                                                .GroupBy(x => x.ColumnId)
                                                .Select(g => g.First())
                                                .OrderBy(x => x.ColumnId)
                                                 .Distinct()
                                                .ToList();
                var halldetail = SeatingData.Where(x => x.HallName == hall.HallName)                                              
                                               .Distinct()
                                               .ToArray();
                #region PageHeader
                page = $@"<br/><br/><div class='row'><div class='col-md-2'></div><div class='col-md-1'><img height='50' class='float: right;' src='https://sbtet.telangana.gov.in/contents/img/big-logo.png' /></div><div class='col-md-9'>
                                <h4 class='text-center head_print'><b>STATE BOARD OF TECHNICAL EDUCATION AND TRAINING, TELANGANA</b></h5>
                                <h6 class='text-center'><b>SEATING PLAN</b></h6></div>
                            </div>
                            <div class='col-md-9'>
                                <div class='head_text'><b>Exam Center Code :</b>  {halldetail[0].CollegeCode} - {halldetail[0].CollegeName}</div>
                                <div class='head_text'><b>Exam Hall :</b>  {halldetail[0].HallName}</div>
                                <div class='head_text'><b>Exam Time :</b>  {halldetail[0].ExamDate}</div>                              
                            </div><div class='col-md-3'></div></div>";
                #endregion
                #region PageContent
                page += @"<br/><div class='row col-md-12'><hr>
						</div><div class='row container-fluid'>";

                List<int> columntemp = new List<int>();
                foreach (var column in hallcolumns)
                {
                    var hallrows = SeatingData.Where(x => x.HallName == hall.HallName && x.ColumnId == column.ColumnId)
                                                   .GroupBy(x => x.RowId)
                                                    .Select(g => g.First())
                                                    .OrderBy(x => x.RowId)
                                                     .Distinct()
                                                    .ToList();

                  
                    

                   
                   
                    foreach (var row in hallrows)
                    {
                        var studposition = SeatingData.Where(x => x.HallName == hall.HallName && x.ColumnId == column.ColumnId && x.RowId == row.RowId)
                                                   .OrderBy(x => x.StudentPosition)
                                                    .Distinct()
                                                    .ToArray();

                        var studpos = SeatingData.Where(x => x.HallName == hall.HallName && x.ColumnId == column.ColumnId && x.RowId == row.RowId)
                                                  .GroupBy(x => x.StudentPosition)
                                                   .Select(g => g.First())                                                 
                                                    .Distinct()
                                                   .ToList();
                        if (hallcolumns.Count * studpos.Count <= 12)
                        {
                            colstyle = 3;
                            // width = 3;
                            width = hallcolumns.Count;   //(float)Math.Round(100.00 / (hallcolumns.Count), 2);
                            fntsz = "12 px";
                        }
                        else if (hallcolumns.Count * studpos.Count > 12 && hallcolumns.Count * studpos.Count <= 16)
                        {
                            colstyle = 3;
                            // width = 3;
                            width = hallcolumns.Count; // (float)Math.Round(100.00 / (hallcolumns.Count), 2);
                            fntsz = "12 px";
                        }
                        else {
                            colstyle = 4;
                            width = hallcolumns.Count; //    (float)Math.Round (100.00 / (hallcolumns.Count),2);                           
                            fntsz = "12 px";
                        }

                        if (!columntemp.Contains(column.ColumnId)) {
                            columntemp.Add(column.ColumnId);

                            page += $@"<div class='row'><div class='col'>";
                            page += @"<table><thead>";
                            page += @"<tr><th id='tbl_head1'><b>S.No</b></th>";
                            foreach (var stpos in studposition)
                            {
                                page += $@"<th id='tbl_head1' style='font-size: {fntsz}'><b>Student {stpos.StudentPosition }</b></th>";
                            }
                            page += @"</tr></thead><tbody>";

                        }                      
                        page += $@"<tr><td id='tbl_head1' style='font-size: {fntsz}' >{ row.RowId }</td>";
                        foreach (var stpos in studposition)
                        {
                            page += $@"<td id='tbl_head1'>{stpos.Pin }</td>";
                        }
                        page += @"</tr>";

                    }
                    page += @"</tbody> </table></div></div>";
                  

                }
                page += "</div><div class='row col-md-12' col-md-12'><hr></div><br/><div class='row'><div class='col-md-6'><div class='pull-left'><p>No. of Students Present: </p><p>Total No. of Students: </p></div></div>";
                page += "<div class='col-md-4'><div class='pull-Right'><p>No. of Students Absent: </p><p>Invigilator Signature: </p></div></div><div class='col-md-2'></div>";
                page += "</div>";
                #endregion


                //#region PageFooter <p style='page-break-before: always;'></p>
                //page += $@" <div class='sm-spacer'></div>
                //    <div class='btm_line footer'>
                //    <div class='col-md-1'>E- ENDEXAM </div> 
                //    <div class='col-md-1'>S- SESS</div>
                //    <div class='col-md-2'>*- EARLIER PASS</div>
                //    <div class='col-md-2'>TOT NO OF CORR: NIL</div>
                //    <div class='col-md-1'>ASST.</div>
                //    <div class='col-md-1'>SUPDT.</div>
                //    <div class='col-md-2'>ASST/SECRETARY.</div>
                //    <div class='col-md-2'>CONTROLLER OF EXAMINATION</div>
                //</div></div>";
                //#endregion

                sbString += page;
                        sbString += "</body></html>";
                        var converter = new HtmlToPdf();
                        converter.Options.ExternalLinksEnabled = true;
                        converter.Options.DrawBackground = false;
                        converter.Options.JavaScriptEnabled = false;
                        converter.Options.WebPageWidth = 1520;
                        converter.Options.PdfPageSize = PdfPageSize.A3;
                        converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                        var doc = converter.ConvertHtmlString(sbString);
                        path = dir + $"\\{pgno.ToString().PadLeft(6, '0')}.pdf";
                        doc.Save(path);
                        doc.Close();
                        sbString = html;
                        pgno++;
                  
                    //tempstudentdata.Clear();
                }
            
            return dir_id;
        }
    }
}
