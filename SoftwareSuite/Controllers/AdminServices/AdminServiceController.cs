﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using SoftwareSuite.Models.Database;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Newtonsoft.Json;
using RestSharp;
using System.Web;
using SoftwareSuite.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SoftwareSuite.Controllers.PreExamination;
using System.Text.RegularExpressions;

namespace SoftwareSuite.Controllers.AdminServices
{
    public class AdminServiceController : ApiController
    {

        [HttpGet, ActionName("GetAllCourses")]
        public HttpResponseMessage GetAllCourses()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_BranchMasterAndGradeMaster";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_BranchMasterAndGradeMaster", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
            [HttpGet, ActionName("GetStaffTypes")]
        public HttpResponseMessage GetStaffTypes()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_StaffTypes";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_StaffTypes", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet, ActionName("GetDownloadsList")]
        public HttpResponseMessage GetDownloadsList()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_DownloadsForAdmin";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_DownloadsForAdmin", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet, ActionName("GetCaptchaString10")]
        public string GetCaptchaString10()
        {
            var dbHandler = new dbHandler();
            try
            {

                string strCaptchaString = "";
                //if (Captcha == null)
                //{

                int intZero = '0';
                int intNine = '9';
                int intA = 'A';
                int intZ = 'Z';
                int intCount = 0;
                int intRandomNumber = 0;
                //string strCaptchaString = "";

                Random random = new Random(System.DateTime.Now.Millisecond);

                while (intCount < 10)
                {
                    intRandomNumber = random.Next(intZero, intZ);
                    if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                    {
                        strCaptchaString = strCaptchaString + (char)intRandomNumber;
                        intCount = intCount + 1;
                    }
                }

                return strCaptchaString;

            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_SET_ReleaseTcPin", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("GetCaptchaString")]
        public string GetCaptchaString(string SessionId)
        {
            var dbHandler = new dbHandler();
            try
            {
                string strCaptchaString = "";
                //int intZero = '0';
                //int intNine = '9';
                int intA = 'A';
                int intZ = 'Z';
                int intCount = 0;
                int intRandomNumber = 0;
                //string strCaptchaString = "";

                Random random = new Random(System.DateTime.Now.Millisecond);

                while (intCount < 5)
                {
                    intRandomNumber = random.Next(intA, intZ);
                    if ((intRandomNumber >= intA) && (intRandomNumber <= intZ))
                    {
                        strCaptchaString = strCaptchaString + (char)intRandomNumber;
                        intCount = intCount + 1;
                    }
                }
                SetSessionId(SessionId, strCaptchaString);
                var skyblue = System.Drawing.ColorTranslator.FromHtml("#1F497D");
                //var white = System.Drawing.ColorTranslator.FromHtml("linear-gradient(90deg, rgba(237,245,255,1) 0%, rgba(204,223,247,1) 100%)");
                string str = ConvertTextToImage(strCaptchaString, "sans-serif", 35, Color.White, skyblue, 250, 65).ToString();

                List<person> p = new List<person>();
                person p1 = new person();

                p1.Image = str;
                //p1.Text = strCaptchaString;
                p.Add(p1);

                return JsonConvert.SerializeObject(p);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_SET_ReleaseTcPin", 0, ex.Message);
                return ex.Message;
            }
        }

        public string ConvertTextToImage(string txt, string fontname, int fontsize, Color bgcolor, Color fcolor, int width, int Height)
        {
            Bitmap bmp = new Bitmap(width, Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {

                Font font = new Font(fontname, fontsize);
                graphics.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(txt, font, new SolidBrush(fcolor), 0, 0);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();


            }
            Bitmap bImage = bmp;  // Your Bitmap Image
            System.IO.MemoryStream ms = new MemoryStream();
            bImage.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage);
            return SigBase64;

        }

        internal class Output
        {
            public string captcha { get; set; }
            public string ResponceCode { get; internal set; }
            public string ResponceDescription { get; internal set; }
            public string Captcha { get; internal set; }
            public string Data { get; internal set; }
        }

        [HttpPost, ActionName("ValidateCaptcha")]
        public string ValidateCaptcha(JsonObject data)
        {
            var dbHandler = new dbHandler();
            List<Output> p = new List<Output>();
            Output p1 = new Output();
            var captcha = string.Empty;
            try
            {

                var PinMatch = ValidatePin(data["Pin"].ToString());
                if (PinMatch == "200")
                {
                    var param = new SqlParameter[2];
                param[0] = new SqlParameter("@SessionId", data["SessionId"]);
                param[1] = new SqlParameter("@Captcha", data["Captcha"]);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_ExamsCaptchaSessionLog", param);

                if (dt.Rows[0]["ResponseCode"].ToString() == "200")
                {
                    PreExaminationController PreExamination = new PreExaminationController();
                    p1.Data = PreExamination.GetStudentFeePaymentDetails(data["Pin"].ToString(), Convert.ToInt32(data["StudentTypeId"]), Convert.ToInt32(data["EMYR"]));
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = dt.Rows[0]["ResponseCode"].ToString();
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);

                }
                else
                {
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }

                }
                else
                {
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = "PIN Not Matched";
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ExamsCaptchaSessionLog", 0, ex.Message);
                captcha = GetCaptchaString(data["SessionId"].ToString());
                p1.ResponceCode = "400";
                p1.ResponceDescription = ex.Message;
                p1.Captcha = captcha;
                p.Add(p1);
                return JsonConvert.SerializeObject(p);
                //return ex.Message;
            }
        }


        
            [HttpPost, ActionName("ValidateGenuineCaptchaText")]
        public string ValidateGenuineCaptchaText(JsonObject data)
        {
            var dbHandler = new dbHandler();
            List<Output> p = new List<Output>();
            Output p1 = new Output();
            var captcha = string.Empty;
            try
            {
                var PinMatch = ValidatePin(data["Pin"].ToString());
                if (PinMatch == "200")
                {
               
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@SessionId", data["SessionId"]);
                param[1] = new SqlParameter("@Captcha", data["Captcha"]);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_ExamsCaptchaSessionLog", param);
                if (dt.Rows[0]["ResponseCode"].ToString() == "200")
                {
                    PreExaminationController PreExamination = new PreExaminationController();
                    p1.Data = PreExamination.getGenuinenessCheckDetailsByPin(data["pin"].ToString());
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = dt.Rows[0]["ResponseCode"].ToString();
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                else
                {
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                 }
                else
                {
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = "PIN Not Matched";
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ExamsCaptchaSessionLog", 0, ex.Message);
                captcha = GetCaptchaString(data["SessionId"].ToString());
                p1.ResponceCode = "400";
                p1.ResponceDescription = ex.Message;
                p1.Captcha = captcha;
                p.Add(p1);
                return JsonConvert.SerializeObject(p);
                //return ex.Message;
            }
        }

        [HttpGet, ActionName("ValidatePin")]
        public string ValidatePin(string Pin)
        {
            string ResponceCode = "";
            try
            {
               

                var match = Regex.IsMatch(Pin, @"^[A-Za-z0-9-]*$");
                if (match)
                {
                    ResponceCode = "200";
                  
                }
                else
                {
                    ResponceCode = "400";
                    
                }
                return ResponceCode;
            }
            catch (Exception ex)
            {
                ResponceCode = "400";               
                return ResponceCode;
            }
        }

      

    [HttpPost, ActionName("ValidateAttendanceCaptcha")]
        public string ValidateAttendanceCaptcha(JsonObject data)
        {
            var dbHandler = new dbHandler();
            List<Output> p = new List<Output>();
            Output p1 = new Output();
            var captcha = string.Empty;
            try
            {
             var  PinMatch= ValidatePin(data["Pin"].ToString());
                if (PinMatch == "200") {
                    //if (PinMatch.ResponceCode == '200') { }
                    var param = new SqlParameter[2];
                param[0] = new SqlParameter("@SessionId", data["SessionId"]);
                param[1] = new SqlParameter("@Captcha", data["Captcha"]);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_ExamsCaptchaSessionLog", param);
                if (dt.Rows[0]["ResponseCode"].ToString() == "200")
                {
                    PreExaminationController PreExamination = new PreExaminationController();
                    p1.Data = PreExamination.getAttendanceReport(data["Pin"].ToString());
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = dt.Rows[0]["ResponseCode"].ToString();
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                else
                {
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                }
                else
                {
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = "PIN Not Matched";
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ExamsCaptchaSessionLog", 0, ex.Message);
                captcha = GetCaptchaString(data["SessionId"].ToString());
                p1.ResponceCode = "400";
                p1.ResponceDescription = ex.Message;
                p1.Captcha = captcha;
                p.Add(p1);
                return JsonConvert.SerializeObject(p);
                //return ex.Message;
            }
        }

        [HttpPost, ActionName("ValidateCaptchaText")]
        public string ValidateCaptchaText(JsonObject data)
        {
            var dbHandler = new dbHandler();
            List<Output> p = new List<Output>();
            Output p1 = new Output();
            var captcha = string.Empty;
            try
            {
                var PinMatch = ValidatePin(data["Pin"].ToString());
                if (PinMatch == "200")
                {
                
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@SessionId", data["SessionId"]);
                param[1] = new SqlParameter("@Captcha", data["Captcha"]);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_ExamsCaptchaSessionLog", param);

                if (dt.Rows[0]["ResponseCode"].ToString() == "200")
                {
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = dt.Rows[0]["ResponseCode"].ToString();
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                else
                {
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                }
                else
                {
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = "PIN Not Matched";
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ExamsCaptchaSessionLog", 0, ex.Message);
                captcha = GetCaptchaString(data["SessionId"].ToString());
                p1.ResponceCode = "400";
                p1.ResponceDescription = ex.Message;
                p1.Captcha = captcha;
                p.Add(p1);
                return JsonConvert.SerializeObject(p);
                //return ex.Message;
            }
        }
        

        [HttpPost, ActionName("ValidateHtCaptcha")]
        public string ValidateHtCaptcha(JsonObject data)
        {
            var dbHandler = new dbHandler();
            List<Output> p = new List<Output>();
            Output p1 = new Output();
            var captcha = string.Empty;
            try
            {
                var PinMatch = ValidatePin(data["Pin"].ToString());
                if (PinMatch == "200")
                {
              

                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@SessionId", data["SessionId"]);
                param[1] = new SqlParameter("@Captcha", data["Captcha"]);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_ExamsCaptchaSessionLog", param);

                if (dt.Rows[0]["ResponseCode"].ToString() == "200")
                {
                    PreExaminationController PreExamination = new PreExaminationController();
                    if (Convert.ToInt32(data["StudentTypeId"]) == 1) {
                        p1.Data = PreExamination.GetRegularHallticket(data["Pin"].ToString(), data["DateOfBirth"].ToString(), Convert.ToInt32(data["StudentTypeId"]), Convert.ToInt32(data["EMYR"]));
                   }
                    else {
                        p1.Data = PreExamination.GetBacklogHallticket(data["Pin"].ToString(), data["DateOfBirth"].ToString(), Convert.ToInt32(data["StudentTypeId"]), data["Exammonthyearid"].ToString());
                    }
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = dt.Rows[0]["ResponseCode"].ToString();
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);

                }
                else
                {
                    captcha = GetCaptchaString(data["SessionId"].ToString());
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = dt.Rows[0]["ResponseDescription"].ToString();
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
                }
                else
                {
                    p1.ResponceCode = "400";
                    p1.ResponceDescription = "PIN Not Matched";
                    p1.Captcha = captcha;
                    p.Add(p1);
                    return JsonConvert.SerializeObject(p);
                }
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ExamsCaptchaSessionLog", 0, ex.Message);
                captcha = GetCaptchaString(data["SessionId"].ToString());
                p1.ResponceCode = "400";
                p1.ResponceDescription = ex.Message;
                p1.Captcha = captcha;
                p.Add(p1);
                return JsonConvert.SerializeObject(p);
                //return ex.Message;
            }
        }


        public class person
        {
            public string ResponseCode { get; set; }
            public string ResponceCode { get; set; }
            public string ResponceDescription { get; set; }
            public string file { get; set; }
            public string Barcode { get; set; }
            public string Image { get; set; }
            public string Data { get; set; }

        }

        [HttpGet, ActionName("SetSessionId")]
        public string SetSessionId(string SessionId, string Captcha)
        {
            var dbHandler = new dbHandler();
            try
            {

                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@SessionId", SessionId);
                param[1] = new SqlParameter("@Captcha", Captcha);
                var dt = dbHandler.ReturnDataWithStoredProcedure("USP_SET_ExamsCaptchaSessionLog", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_SET_ExamsCaptchaSessionLog", 0, ex.Message);
                return ex.Message;
            }
        }


        [HttpGet, ActionName("GetActiveDownloadsList")]
        public HttpResponseMessage GetActiveDownloadsList()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_DownloadsForWebSite";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_DownloadsForWebSite", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet, ActionName("getUserTypes")]
        public HttpResponseMessage getUserTypes()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_UserTypes";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_UserTypes", 0, ex.Message);
                throw ex;
            }
        }

        [HttpGet, ActionName("getNotifications")]
        public HttpResponseMessage getNotifications()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_AllNotifications";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_AllNotifications", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.Gone, ex);
            }
          
        }


        [HttpGet, ActionName("getCircularTypes")]
        public HttpResponseMessage getCircularTypes()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_CircularTypes";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_CircularTypes", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.Gone, ex);
            }

        }

        [HttpGet, ActionName("getStaffList")]
        public HttpResponseMessage getStaffList()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_StaffInfo";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_StaffInfo", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.Gone, ex);
            }

        }
        


        [HttpGet, ActionName("getStaffActive")]
        public HttpResponseMessage getStaffActive()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_StaffActive";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_StaffActive", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        
                [HttpGet, ActionName("GetCollegesList")]
        public HttpResponseMessage GetCollegesList()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_COLLEGESLIST";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_COLLEGESLIST", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }


        [HttpGet, ActionName("GetModulesbyRole")]
        public HttpResponseMessage GetModulesbyRole(int usertypeid)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@usertypeid", usertypeid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_Login_GET_Modules", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_Login_GET_Modules ", 0, ex.Message);
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpGet, ActionName("DeleteStaff")]
        public HttpResponseMessage DeleteStaff(int id)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_DEL_Staff", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_DEL_Staff ", 0, ex.Message);
                throw ex;
            }

        }

        [HttpGet, ActionName("DeleteCircular")]
        public HttpResponseMessage DeleteCircular(int id)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_DEL_CIRCULARS", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_DEL_CIRCULARS ", 0, ex.Message);
                throw ex;
            }

        }

        [HttpGet, ActionName("DeleteDownloads")]
        public HttpResponseMessage DeleteDownloads(int id)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_DEL_Downloads", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_DEL_Downloads ", 0, ex.Message);
                throw ex;
            }

        }
        

        [HttpGet, ActionName("DeleteTender")]
        public HttpResponseMessage DeleteTender(int id)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_DEL_Tenders", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_DEL_Tenders ", 0, ex.Message);
                throw ex;
            }

        }

        [HttpGet, ActionName("SwitchCircular")]
        public HttpResponseMessage SwitchCircular(int id,int IsActive)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@IsActive", IsActive);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SWITCH_CIRCULARS", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SWITCH_CIRCULARS ", 0, ex.Message);
                throw ex;
            }

        }

        [HttpGet, ActionName("SwitchDownloads")]
        public HttpResponseMessage SwitchDownloads(int id, int IsActive)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@IsActive", IsActive);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SWITCH_Downloads", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SWITCH_Downloads ", 0, ex.Message);
                throw ex;
            }

        }

        
        [HttpGet, ActionName("SwitchStaff")]
        public HttpResponseMessage SwitchStaff(int id, int IsActive)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@IsActive", IsActive);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SWITCH_Staff", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SWITCH_Staff ", 0, ex.Message);
                throw ex;
            }

        }


        [HttpGet, ActionName("SwitchTender")]
        public HttpResponseMessage SwitchTender(int id, int IsActive)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@IsActive", IsActive);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SWITCH_Tenders", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SWITCH_Tenders ", 0, ex.Message);
                throw ex;
            }

        }



        [HttpGet, ActionName("GetAllModulesbyRole")]
        public HttpResponseMessage GetAllModulesbyRole(int usertypeid)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@usertypeid", usertypeid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_ALLModules", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_Login_GET_Modules ", 0, ex.Message);
                throw ex;
            }

        }

        [HttpGet, ActionName("GetSubModulesbyRole")]
        public HttpResponseMessage GetSubModulesbyRole(int usertypeid,int moduleid)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@usertypeid", usertypeid);
                param[1] = new SqlParameter("@moduleid", moduleid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_Login_GET_SubModules ", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_Login_GET_SubModules ", 0, ex.Message);
                throw ex;
            }

        }
        

              [HttpGet, ActionName("SetSubModuleInactive")]
        public HttpResponseMessage SetSubModuleInactive(int usertypeid, int moduleId,int SubModuleId,int IsActive)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[4];
                param[0] = new SqlParameter("@usertypeid", usertypeid);
                param[1] = new SqlParameter("@moduleId", moduleId);
                param[2] = new SqlParameter("@SubModuleId", SubModuleId);
                param[3] = new SqlParameter("@IsActive", IsActive);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_Login_SET_SubmodueInctive ", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_Login_SET_SubmodueInctive ", 0, ex.Message);
                throw ex;
            }

        }


        [HttpGet, ActionName("SetModuleInactive")]
        public HttpResponseMessage SetModuleInactive(int usertypeid, int moduleId, int IsActive)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[3];
                param[0] = new SqlParameter("@usertypeid", usertypeid);
                param[1] = new SqlParameter("@moduleId", moduleId);
                param[2] = new SqlParameter("@IsActive", IsActive);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_Login_SET_modueInctive ", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_Login_SET_modueInctive ", 0, ex.Message);
                throw ex;
            }

        }


        [HttpGet, ActionName("NotificationInactive")]
        public HttpResponseMessage NotificationInactive(int Id)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id ", Id);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SET_NotificationInactive ", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SET_NotificationInactive ", 0, ex.Message);
                throw ex;
            }

        }
        public class notification
        {
            public string Notification { get; set; }
            public int UserTypeId { get; set; }
            public DateTime fromDate { get; set; }
            public DateTime ToDate { get; set; }

        }



      


        
                [HttpGet, ActionName("getUserType")]
        public HttpResponseMessage getUserType()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_UserType";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_UserType", 0, ex.Message);
                throw ex;
            }
        }

        [HttpGet, ActionName("getActiveBranches")]
        public HttpResponseMessage getActiveBranches()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_ActiveBranches";
                return Request.CreateResponse(HttpStatusCode.OK, dbHandler.ReturnDataSet(StrQuery));
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ActiveBranches", 0, ex.Message);
                throw ex;
            }
        }



        [HttpGet, ActionName("SwitchUserStatus")]
        public HttpResponseMessage SwitchUserStatus(string UserId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserId ", UserId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SET_UserIdActiveInActive", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SET_UserIdActiveInActive", 0, ex.Message);
                throw ex;
            }
        }

        [HttpGet, ActionName("GetNotificationByUser")]
        public HttpResponseMessage GetNotificationByUser(int UserTypeId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                string clientIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                param[0] = new SqlParameter("@UserTypeId", UserTypeId);
                param[1] = new SqlParameter("@clientIpAddress", clientIpAddress);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_Notification", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_Notification", 0, ex.Message);
                throw ex;
            }
        }

        
             [HttpGet, ActionName("GetNotificationsActiveByUser")]
        public HttpResponseMessage GetNotificationsActiveByUser(int UserTypeId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserTypeId", UserTypeId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_Notification_Active", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_Notification_Active", 0, ex.Message);
                throw ex;
            }
        }


        [HttpGet, ActionName("GetCircularByUser")]
        public HttpResponseMessage GetCircularByUser(int UserTypeId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserTypeId", UserTypeId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("SBP_GET_CircularByUserType", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("SBP_GET_CircularByUserType", 0, ex.Message);
                throw ex;
            }
        }
   


              [HttpGet, ActionName("GetBranchList")]
        public HttpResponseMessage GetBranchList(string @CollegeCode)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@CollegeCode", CollegeCode);
               
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_GET_BRANCHLIST", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_BRANCHLIST", 0, ex.Message);
                throw ex;
            }
        }

        [HttpGet, ActionName("getActiveSchemes")]
        public string getActiveSchemes()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_SCHEMES";
                var res = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_SCHEMES", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getCirculars")]
        public string getCirculars()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_Circular";
                var res = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_Circular", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getTenders")]
        public string getTenders()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_Tenders";
                var res = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_Tenders", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getCircularsActive")]
        public string getCircularsActive()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_Circulars_Active";
                var res = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_Circulars_Active", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getTendersActive")]
        public string getTendersActive()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec ADM_GET_Tenders_Active";
                var res = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_Tenders_Active", 0, ex.Message);
                return ex.Message;
            }
        }



        [HttpGet, ActionName("GetUserIdStatus")]
        public HttpResponseMessage GetUserIdStatus(string UserName)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserName", UserName);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_GET_UserIdStatus", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_GET_UserIdStatus", 0, ex.Message);
                throw ex;
            }

        }


        public class userDetails
        {
            public int UserTypeId { get; set; }
            public string UserName { get; set; }
            public string UserPassword { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address1 { get; set; }
            public string EmailId { get; set; }
            public string CellNo { get; set; }
            public int CollegeId { get; set; }
            public int BranchId { get; set; }


        }

        [HttpPost, ActionName("createUser")]
        public HttpResponseMessage createUser([FromBody]userDetails request)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[11];
                param[0] = new SqlParameter("@UserTypeId", request.UserTypeId);
                param[1] = new SqlParameter("@UserName", request.UserName);
                param[2] = new SqlParameter("@UserPassword", request.UserPassword);
                param[3] = new SqlParameter("@ExpiryDate", request.ExpiryDate);
                param[4] = new SqlParameter("@FirstName", request.FirstName);
                param[5] = new SqlParameter("@LastName", request.LastName);
                param[6] = new SqlParameter("@Address1", request.Address1);
                param[7] = new SqlParameter("@EmailId", request.EmailId);
                param[8] = new SqlParameter("@CellNo", request.CellNo);
                param[9] = new SqlParameter("@CollegeId", request.CollegeId);
                param[10] = new SqlParameter("@BranchId", request.BranchId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SET_CreateNewLogin", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("ADM_SET_CreateNewLogin", 0, ex.Message);
                throw ex;
            }
        }
        //[HttpPost, ActionName("PostNotification")]
        //public HttpResponseMessage PostNotification([FromBody]notification request)
        //{
        //    try
        //    {
        //        var dbHandler = new dbHandler();
        //        var param = new SqlParameter[4];
        //        param[0] = new SqlParameter("@Notification ", request.Notification);
        //        param[1] = new SqlParameter("@UserTypeId ", request.UserTypeId);
        //        param[2] = new SqlParameter("@fromDate ", request.fromDate);
        //        param[3] = new SqlParameter("@ToDate ", request.ToDate);
        //        var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_SET_Notification", param);
        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        dbHandler.SaveErorr("USP_SET_Notification", 0, ex.Message);
        //        throw ex;
        //    }

        //}

        public class NotificationData
        {
            public string Notification  { get; set; }
            public int UserTypeId { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
          
        }



        [HttpPost, ActionName("PostNotification")]
        public string PostNotification([FromBody]JsonObject NotificationData)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@Json", NotificationData["Json"]);
                var res = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SET_Notification", param);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("ADM_SET_Notification", 0, ex.Message);
                return ex.Message;
            }
        }
        [HttpGet, ActionName("GetSemester")]
        public HttpResponseMessage GetSemester(int UserType)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserType", UserType);

                var dt = dbHandler.ReturnDataWithStoredProcedure("USP_Admission_GET_SemSchemes ", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_Admission_GET_SemSchemes ", 0, ex.Message);
                throw ex;
            }
        }

      

    [HttpPost, ActionName("SaveScheamdata")]
        public HttpResponseMessage SaveScheamdata([FromBody]JsonObject request)
        {
            try
            {

                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserType", request["UserType"]);
               
                param[1] = new SqlParameter("@json", request["json"]);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_Admission_SET_SemSchemes", param);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                return response;
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("USP_Admission_SET_SemSchemes", 0, ex.Message);
                throw ex;
            }
        }

    }

    public class AdminServiceBaseController : BaseController
    {

        [HttpPost, ActionName("uploadFile")]
        public string uploadFile([FromBody]HttpPostedFileBase file, string Title,string Description,string Ids)
        {
            var path = string.Empty;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var eh = new ExcelHelper();
                    path = Path.Combine(Server.MapPath("~/Circulars/"), fileName);
                    file.SaveAs(path);

                    String[] spearator = { "\\softwaresuite\\SoftwareSuite", "" };
                    Int32 count = 2;

                    String[] strlist = path.Split(spearator, count,StringSplitOptions.None);
                    strlist[1] = strlist[1].Replace("\\","/");
                    //  return path;
                    var dbHandler = new dbHandler();
                    var param = new SqlParameter[4];
                    param[0] = new SqlParameter("@Title", Title);
                    param[1] = new SqlParameter("@Description", Description);
                    param[2] = new SqlParameter("@Url", strlist[1]);
                    param[3] = new SqlParameter("@Ids", Ids);
                    var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SET_Circular", param);
                    return JsonConvert.SerializeObject(dt);
                    //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, dt);
                    //return response;
                }

            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("GetFeePaymentReports", 1, ex.Message + "\n-----------\n" + ex.StackTrace);
                return ex.StackTrace;
            }
            return "0";
        }

     

    }

}
