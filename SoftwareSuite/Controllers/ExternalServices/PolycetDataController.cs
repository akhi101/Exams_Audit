using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoftwareSuite.Models.Database;

namespace SoftwareSuite.Controllers.ExternalServices
{
    public class PolycetDataController  : ApiController
    {
        #region Post Methods
        [HttpPost, ActionName("PostStudentData")]
        public HttpResponseMessage PostStudentData(HttpRequestMessage request)
        {
            try {

                var apikey = request.Headers.GetValues("apikey").FirstOrDefault();
                var apikeyOrig = ConfigurationManager.AppSettings["PolycetDataSharingApiKey"].ToString();
                if (apikey != apikeyOrig) {
                    var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                    response.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"403\",\"respdesc\" = \"Invalid Api Key\"\" }"), System.Text.Encoding.UTF8, "application/json");                  
                    return response;
                }
            }
            catch (Exception) {
                var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                response.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"403\",\"respdesc\" = \"Invalid Api Key\"\" }"), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            try {

                string PolycetDatajson = "" + request.Content.ReadAsStringAsync().Result;
              
                if (PolycetDatajson != "") {
                    JObject obj = JObject.Parse(PolycetDatajson);
                  
                    var dbHandler = new dbHandler();
                    var param = new SqlParameter[30];
                    param[0] = new SqlParameter("@Candidatename", obj["CandidateName"]);
                    param[1] = new SqlParameter("@FatherName", obj["FatherName"]);
                    param[2] = new SqlParameter("@MotherName", obj["MotherName"]);
                    param[3] = new SqlParameter("@Gender", obj["Gender"]);
                    param[4] = new SqlParameter("@DateOfBirth", obj["DateOfBirth"]);
                    param[5] = new SqlParameter("@E_mail", obj["E_mail"]);
                    param[6] = new SqlParameter("@ParentContact", obj["ParentContact"]);
                    param[7] = new SqlParameter("@StudentContact", obj["StudentContact"]);
                    param[8] = new SqlParameter("@TenthYear", obj["TenthYear"]);
                    param[9] = new SqlParameter("@TenthBoard", obj["TenthBoard"]);
                    param[10] = new SqlParameter("@TenthHallTicketNo", obj["TenthHallTicketNo"]);
                    param[11] = new SqlParameter("@Category", obj["Category"]);
                    param[12] = new SqlParameter("@IsPhysicallyHandicaped", obj["IsPhysicallyHandicaped"]);
                    param[13] = new SqlParameter("@TemporaryAddress", obj["TemporaryAddress"]);
                    param[14] = new SqlParameter("@HouseNo", obj["HouseNo"]);
                    param[15] = new SqlParameter("@VillageorTown", obj["VillageorTown"]);
                    param[16] = new SqlParameter("@District", obj["District"]);
                    param[17] = new SqlParameter("@Mandal", obj["Mandal"]);
                    param[18] = new SqlParameter("@Pincode", obj["Pincode"]);
                    param[19] = new SqlParameter("@Income", obj["Income"]);
                    param[20] = new SqlParameter("@Occupation", obj["Occupation"]);
                    param[21] = new SqlParameter("@CasteNo", obj["CasteNo"]);
                    param[22] = new SqlParameter("@ADMISSIONType", obj["ADMISSIONType"]);
                    param[23] = new SqlParameter("@PolycetHallTicketNo", obj["PolycetHallTicketNo"]);
                    param[24] = new SqlParameter("@Polycetyear", obj["Polycetyear"]);                  
                    param[25] = new SqlParameter("@POLYCETRank", obj["POLYCETRank"]);
                    param[26] = new SqlParameter("@feeexempted", obj["feeexempted"]);
                    param[27] = new SqlParameter("@sbtet_collegecode", obj["sbtet_collegecode"]);
                    param[28] = new SqlParameter("@sbtet_branch_code", obj["sbtet_branch_code"]);
                    param[29] = new SqlParameter("@ActiveStatus", obj["ActiveStatus"]);
                    DataTable dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_SET_NICPolycetDataIntoAdmission", param);
                  
                    if (dt.Rows.Count > 0) {
                        int rescode = (int)dt.Rows[0][0];
                        string respdesc = (string)dt.Rows[0][1];
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"" + rescode + "\",\"respdesc\" : \"" + respdesc + "\"\" }"), System.Text.Encoding.UTF8, "application/json");
                         return response;


                    }
                    else {

                        var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"500\",\"respdesc\" : \"Server Error\"\" }"), System.Text.Encoding.UTF8, "application/json");
                        return response;

                    }
                }
            }
            catch (FormatException) {

                var response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
                response.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"406\",\"respdesc\" : \"Unprocessable Entity, Check the json data format \" }"), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"500\",\"respdesc\" : \"" + ex.Message + "\" }"), System.Text.Encoding.UTF8, "application/json");
                return response;

            }
            var res = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            res.Content = new StringContent(JsonConvert.SerializeObject("{\"respcode\":\"406\",\"respdesc\" : \"Unprocessable Entity, Check the json data format \" }"), System.Text.Encoding.UTF8, "application/json");
            return res;
        }





        #endregion
	}
}