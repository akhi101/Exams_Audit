using System;
using System.Web.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;
using FromBody = System.Web.Http.FromBodyAttribute;
using SoftwareSuite.BLL;
using SoftwareSuite.Models.Database;
using SoftwareSuite.Models.Assessment;

namespace SoftwareSuite.Controllers.Assessment
{
    public class AssessmentController : BaseController
    {
        #region Get Methods

        [HttpGet, ActionName("getActiveSemester")]
        public string getActiveSemester()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_ACTIVE_SEMESTER";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_ACTIVE_SEMESTER", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getAllSemesters")]
        public string getAllSemesters()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec  ADM_GET_AllSemesterDetails";
                var res = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("ADM_GET_AllSemesterDetails", 0, ex.Message);
                throw ex;
            }
        }

        [HttpGet, ActionName("getSubjectsFaculty")]
        public string getSubjectsFaculty()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_ADMISSION_GET_FacultyDetails";
                var dt = dbHandler.ReturnData(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("USP_GET_ACTIVE_SEMESTER", 0, ex.Message);
                return ex.Message;
            }
        }


        [HttpGet, ActionName("getStudentType")]
        public string GetStudentType()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_Student_type";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_Student_type", 0, ex.Message);
                return ex.Message;
            }
        }
        [HttpGet, ActionName("getExamtypeR")]
        public string getExamtypeR()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_EXAM_type_P";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("USP_GET_EXAM_type_P", 0, ex.Message);
                return ex.Message;
            }
        }
        [HttpGet, ActionName("getPresentStudentType")]
        public string GetPresentStudentType()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_SFP_GETSTUDENTTYPE";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_SFP_GETSTUDENTTYPE", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getSemistersSetData")]
        public string getSemistersSetData()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_Assessment_Semesters";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_Assessment_Semesters", 0, ex.Message);
                return ex.Message;
            }
        }

    
            


        [HttpGet, ActionName("getSemByScheme")]
        public string getSemByScheme(int StudentTypeId, int SchemeId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@StudentTypeId", StudentTypeId);
                param[1] = new SqlParameter("@SchemeId", SchemeId);            
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_Assessment_GET_SemestersByScheme", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_Assessment_GET_SemestersByScheme", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getSchemeWiseExams")]
        public string getSchemeWiseExams(int StudentTypeId, int SchemeId, int SemId,int SubjectTypeId,int ExamMonthYearId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[5];
                param[0] = new SqlParameter("@StudentTypeId", StudentTypeId);
                param[1] = new SqlParameter("@SchemeId", SchemeId);
                param[2] = new SqlParameter("@SemId", SemId);
                param[3] = new SqlParameter("@SubjectTypeId", SubjectTypeId);
                param[4] = new SqlParameter("@ExamMonthYearId", ExamMonthYearId);
                var dt = dbHandler.ReturnDataWithStoredProcedure("USP_GET_Schemewise_Exams", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("USP_GET_Schemewise_Exams", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("GetStatisticalReports")]
        public string GetStatisticalReports(int semid)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@semid", semid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_GET_AssessmentNeedToInactiveList", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("ADM_GET_AssessmentNeedToInactiveList", 0, ex.Message);
                return ex.Message;
            }

        }




        [HttpGet, ActionName("getBranchName")]
        public string getBranchName(string branchcode)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@branchcode", branchcode);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_Branch_Name", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_Branch_Name", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getExamTypes")]
        public string getExamTypes(int schemeid)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@schemeid", schemeid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_EXAM_TYPE", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_EXAM_TYPE", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getSchemes")]
        public string getSchemes(int StudentTypeId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@StudentTypeId", StudentTypeId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_Assessment_GET_Schemes", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("USP_Assessment_GET_Schemes", 0, ex.Message);
                return ex.Message;
            }

        }


        [HttpGet, ActionName("getExamTypesBySem")]
        public string getExamTypesBySem(int @StudentTypeId,int Schemeid)
        {
         
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[2];
                param[0] = new SqlParameter("@StudentTypeId", StudentTypeId);
                param[1] = new SqlParameter("@Schemeid", Schemeid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_Assessment_GET_ExamTypes", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("ADM_Assessment_GET_ExamTypes", 0, ex.Message);
                return ex.Message;
            }

        }
        


        [HttpGet, ActionName("getMarksEntryDates")]
        public string getMarksEntryDates(int AcademicId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@AcademicYearId", AcademicId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_EXAM_DATES", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_EXAM_DATES", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("GetFeeAcademicYearsActive")]
        public string GetFeeAcademicYearsActive(string CollegeId)
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GetAcademicYearsActive";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GetAcademicYearsActive", 0, ex.Message);
                return ex.Message;
            }
        }

        [HttpGet, ActionName("getAcademicYearsActive")]
        public string getAcademicYearsActive()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_ACADEMIC_YEARS";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_ACADEMIC_YEARS", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getSchemeStatus")]
        public string getSchemeStatus()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_SCHEMES";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_SCHEMES", 0, ex.Message);
                return ex.Message;
            }

        }


        [HttpGet, ActionName("getExamType")]
        public string getExamType(int schemeid)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[1];
                param[0] = new SqlParameter("@schemeid", schemeid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_EXAM_TYPE", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_EXAM_TYPE", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getActiveExamTypes")]
        public string getActiveExamTypes()
        {
            try
            {
                var dbHandler = new dbHandler();
                string StrQuery = "";
                StrQuery = "exec USP_GET_ActiveExamTypes";
                var dt = dbHandler.ReturnDataSet(StrQuery);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                dbHandler.SaveErorr("USP_GET_ActiveExamTypes", 0, ex.Message);
                return ex.Message;
            }
        }

        //public string getCollegeAssessmentReports(int semid)
        //{
        //    try
        //    {
        //        var dbHandler = new dbHandler();
        //        var param = new SqlParameter[1];
        //        param[0] = new SqlParameter("@semid", semid);

        //        var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_Assessment_CollegeReports", param);
        //        return JsonConvert.SerializeObject(dt);
        //    }
        //    catch (Exception ex)
        //    {
                
        //        dbHandler.SaveErorr("USP_GET_Assessment_CollegeReports", 0, ex.Message);
        //        return ex.Message;
        //    }

        //}

        public string getCollegeAssessmentReports(string collegecode, int examtypeid,int studentType)
        {
            try
            {
                var dbHandler = new dbHandler();

                var param = new SqlParameter[0];
                if (studentType == 2)
                {
                    param = new SqlParameter[2];
                    param[0] = new SqlParameter("@collegecode", collegecode);
                    param[1] = new SqlParameter("@studentType", studentType);

                }
                else
                {
                    param = new SqlParameter[3];
                    param[0] = new SqlParameter("@collegecode", collegecode);
                    param[1] = new SqlParameter("@examtypeid", examtypeid);
                    param[2] = new SqlParameter("@studentType", studentType);
                }
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_Assessment_CollegeReports", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_Assessment_CollegeReports", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getSemSubjects")]
        public string getSemSubjects(int semid, string branchCode, int loadedScheme, int subType, int examTypeid, string collegecode,int studenttypeid,int AcademicYearId,int ExamMonthYearId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[9];
                param[0] = new SqlParameter("@semid", semid);
                param[1] = new SqlParameter("@branchcode", branchCode);
                param[2] = new SqlParameter("@schemeid", loadedScheme);
                param[3] = new SqlParameter("@subtype", subType);
                param[4] = new SqlParameter("@examtypeid", examTypeid);
                param[5] = new SqlParameter("@collegecode", collegecode);
                param[6] = new SqlParameter("@studenttypeid", studenttypeid);
                param[7] = new SqlParameter("@academicyearid", AcademicYearId);
                param[8] = new SqlParameter("@ExamMonthYearId", ExamMonthYearId);
                var dt = dbHandler.ReturnDataWithStoredProcedure("USP_GET_SEM_SUBJECTS", param); 
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_GET_SEM_SUBJECTS", 0, ex.Message);
                return ex.Message;
            }

        }

        [HttpGet, ActionName("getSchemeWiseExamTypes")]
        public string getSchemeWiseExamTypes(int AcademicYearId,int StudentTypeId, int SchemeId, int SemId,int ExamMonthYearId)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[5];
                param[0] = new SqlParameter("@AcademicYearId", AcademicYearId);
                param[1] = new SqlParameter("@StudentTypeId", StudentTypeId);
                param[2] = new SqlParameter("@SchemeId", SchemeId);
                param[3] = new SqlParameter("@SemId", SemId);
                param[4] = new SqlParameter("@ExamMonthYearId", ExamMonthYearId);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("USP_GET_Schemewise_Exams_Types", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {

                dbHandler.SaveErorr("USP_GET_Schemewise_Exams_Types", 0, ex.Message);
                return ex.Message;
            }

        }

        #endregion
        #region POST Methods

        [HttpPost, ActionName("PostMarksEntryDates")]
        public string PostMarksEntryDates([FromBody]SetDatesMarksEntryreqdata ReqData)
        {
            try
            {
                string clientIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                var dbHandler = new dbHandler();
                var param = new SqlParameter[12];
                param[0] = new SqlParameter("@examid", ReqData.examId);
                param[1] = new SqlParameter("@semid", ReqData.semid);
                param[2] = new SqlParameter("@AcademicYearId", ReqData.AcademicYearId);
                param[3] = new SqlParameter("@username", ReqData.userName);
                param[4] = new SqlParameter("@fromdate", ReqData.fromDate);
                param[5] = new SqlParameter("@todate", ReqData.toDate);
                param[6] = new SqlParameter("@finedate", ReqData.fineDate);
                param[7] = new SqlParameter("@ipaddress", clientIpAddress);
                param[8] = new SqlParameter("@fine", ReqData.fine);
                param[9] = new SqlParameter("@studenttypeid", ReqData.Studenttypeid);
                param[10] = new SqlParameter("@schemeid", ReqData.schemeid);
                param[11] = new SqlParameter("@ExamMonthYearId", ReqData.ExamMonthYearId);
                var dt = dbHandler.ReturnDataWithStoredProcedure("USP_SET_EXAM_DATES", param);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("USP_SET_EXAM_DATES", 0, ex.Message);
                return ex.Message;

            }

        }

        [HttpPost, ActionName("updateStudentDetails")]
        public string updateStudentDetails([FromBody]studentDetails request)
        {
            try
            {
                var dbHandler = new dbHandler();
                var param = new SqlParameter[46];
                param[0] = new SqlParameter("@StudentId", request.StudentId);
                param[1] = new SqlParameter("@profilephoto", request.profilephoto);
                param[2] = new SqlParameter("@CandidateSign", request.CandidateSign);
                param[3] = new SqlParameter("@CategoryId", request.CategoryId);
                param[4] = new SqlParameter("@SpecialCategoryId", request.SpecialCategoryId);
                param[5] = new SqlParameter("@TenthRollNo", request.TenthRollNo);
                param[6] = new SqlParameter("@TenthYear", request.TenthYear);
                param[7] = new SqlParameter("@TenthBoard", request.TenthBoard);
                param[8] = new SqlParameter("@TenthHallTicketNo", request.TenthHallTicketNo);
                param[9] = new SqlParameter("@StudentRecided", request.StudentRecided);
                //param[10] = new SqlParameter("@PolycetHallTicketNo", request.PolycetHallTicketNo);
                param[10] = new SqlParameter("@QualificationId", request.QualificationId);
                param[11] = new SqlParameter("@ReligionId", request.ReligionId);
                param[12] = new SqlParameter("@Region", request.Region);
                param[13] = new SqlParameter("@MinorityType", request.MinorityType);
                param[14] = new SqlParameter("@PermanentAddress", request.PermanentAddress);
                param[15] = new SqlParameter("@TempararyAddress", request.TempararyAddress);
                param[16] = new SqlParameter("@HouseNo", request.HouseNo);
                param[17] = new SqlParameter("@VillageorTown", request.VillageorTown);
                param[18] = new SqlParameter("@DistrictId", request.DistrictId);
                param[19] = new SqlParameter("@MandalId", request.MandalId);
                param[20] = new SqlParameter("@Pincode", request.Pincode);
                param[21] = new SqlParameter("@IsPhysicallyHandicaped", request.IsPhysicallyHandicaped);
                param[22] = new SqlParameter("@FatherAadhaarNo", request.FatherAadhaarNo);
                param[23] = new SqlParameter("@MotherAadhaarNo", request.MotherAadhaarNo);
                param[24] = new SqlParameter("@IsFatherGorthEmp", request.IsFatherGorthEmp);
                param[25] = new SqlParameter("@Income", request.Income);
                param[26] = new SqlParameter("@IncomeCategory", request.IncomeCategory);
                param[27] = new SqlParameter("@Occupation", request.Occupation);
                param[28] = new SqlParameter("@CasteNo", request.CasteNo);
                param[29] = new SqlParameter("@BankId", request.BankId);
                param[30] = new SqlParameter("@BankAccountNo", request.BankAccountNo);
                param[31] = new SqlParameter("@IfscCode", request.IfscCode);
                param[32] = new SqlParameter("@BankBranch", request.BankBranch);
                //param34] = new SqlParameter("@ShiftId", request.ShiftId);
                //param[35] = new SqlParameter("@PIN", request.PIN);
                param[33] = new SqlParameter("@Name", request.Name);
                param[34] = new SqlParameter("@FatherName", request.FatherName);
                param[35] = new SqlParameter("@MotherName", request.MotherName);
                param[36] = new SqlParameter("@Gender", request.Gender);
                param[37] = new SqlParameter("@DateOfBIrth", request.DateOfBIrth);
                //param[41] = new SqlParameter("@CourseID", request.CourseID);
                param[38] = new SqlParameter("@AadharNo", request.AadharNo);
                param[39] = new SqlParameter("@EmailId", request.EmailId);
                param[40] = new SqlParameter("@ParentContact", request.ParentContact);
                param[41] = new SqlParameter("@StudentContact", request.StudentContact);
                param[42] = new SqlParameter("@CollegeCode", request.CollegeCode);
                //param[47] = new SqlParameter("@SchemeId", request.SchemeId);
                //param[48] = new SqlParameter("@AcademicYearId", request.AcademicYearId);
                //param[49] = new SqlParameter("@BranchID", request.BranchID);
                param[43] = new SqlParameter("@AttendeeId", request.AttendeeId);
                param[44] = new SqlParameter("@Activeflag", request.Activeflag);
                param[45] = new SqlParameter("@semid", request.semid);
                var dt = dbHandler.ReturnDataWithStoredProcedureTable("ADM_SET_UpdateStudentDetails", param);
                return JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                
                dbHandler.SaveErorr("ADM_SET_UpdateStudentDetails", 0, ex.Message);
                return ex.Message;
            }

        }


        #endregion
    }
}
