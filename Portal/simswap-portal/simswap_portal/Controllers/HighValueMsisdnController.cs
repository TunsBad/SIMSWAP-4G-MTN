using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using simswap_portal.Models;

namespace simswap_portal.Controllers
{
    public class HighValueMsisdnController : Controller
    {
        DBHelper _dbhelper = new DBHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadMsisdn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> uploadBatch)
        {
            try
            {
                if (uploadBatch != null && uploadBatch.Any())
                {
                    foreach (HttpPostedFileBase File in uploadBatch)
                    {
                        var batchFile = File;
                        if (batchFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(batchFile.FileName);
                            var filePath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                            batchFile.SaveAs(filePath);

                            if (fileName != null)
                            {
                                switch (Path.GetExtension(batchFile.FileName))
                                {
                                    case ".xlsx":
                                        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                                        {
                                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                                            {
                                                var results = reader.AsDataSet();
                                                var CorrectList = new List<string> { };

                                                foreach (DataTable resultsTable in results.Tables)
                                                {
                                                    foreach (DataRow row in resultsTable.Rows)
                                                    {
                                                        if (row.IsNull(0))
                                                        {
                                                            continue;
                                                            //throw new NotImplementedException("Empty Rows not allowed, Check in a Future update");
                                                        }
                                                        else
                                                        {
                                                            CorrectList.Add(row.ItemArray[0].ToString().Trim());
                                                        }
                                                    }
                                                }

                                                var CommaSeparatedString = string.Join(",", CorrectList);
                                                DBHelper.Instance.SaveHighValueSubscribers(CommaSeparatedString);

                                            }
                                        }
                                        break;

                                    default:
                                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Unsupported File Type");
                                }
                            }
                            else
                            {
                                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Select a file with a name");
                            }
                        }
                        else
                        {
                            return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Cannot process empty file.");
                        }
                    }
                }
                else
                {
                    return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "No Files Found in Request");
                }
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK,
                    "High Value Subscribers Successfully uploaded.");
            }
            catch (Exception ex)
            {
                //TODO Log JSON Data of Exception
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest,
                    $"Server Error, Something Happened on out End Please Try Again\n{ex.Message}");
            }
        }

       
    }
}



