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
    public class UsersController : Controller
    {
        public ActionResult AllUsers()
        {
            return View();
        }

        public ActionResult BatchUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> uploadBatchAgents)
        {
            try
            {
                if (uploadBatchAgents != null && uploadBatchAgents.Any())
                {
                    foreach (HttpPostedFileBase File in uploadBatchAgents)
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
                                                var UsernameList = new List<string> { };
                                                var PhonenumberList = new List<string> { };
                                                var PasswordList = new List<string> { };

                                                foreach (DataTable resultsTable in results.Tables)
                                                {
                                                    foreach (DataRow row in resultsTable.Rows)
                                                    {
                                                        if (row.IsNull(0))
                                                        {
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            UsernameList.Add(row.ItemArray[0].ToString().Trim());
                                                            PhonenumberList.Add(row.ItemArray[1].ToString().Trim());
                                                            PasswordList.Add(row.ItemArray[2].ToString().Trim());
                                                        }
                                                    }
                                                }

                                                var UsernameCommaSeparatedString = string.Join(",", UsernameList);
                                                var PhonenumberCommaSeparatedString = string.Join(",", PhonenumberList);
                                                var PasswordCommaSeparatedString = string.Join(",", PasswordList);


                                                DBHelper.Instance.SaveAgents(PhonenumberCommaSeparatedString, UsernameCommaSeparatedString,
                                                    PasswordCommaSeparatedString);

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

                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK, "High Value Subscribers Successfully uploaded.");
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