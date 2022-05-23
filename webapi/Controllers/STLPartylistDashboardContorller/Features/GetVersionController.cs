using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    public class GetVersionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GetVersionController(IConfiguration config, IWebHostEnvironment webhostingEnvironment)
        {
            _config = config;
            _webHostEnvironment = webhostingEnvironment;
        }
        [HttpPost]
        [Route("dashboardversion")]
        public async Task<IActionResult> Task0a()
        {
            var strfilename = "OneUnitedPinas.exe";
            var filePath = _webHostEnvironment.WebRootPath;
            var strFilePath = filePath + "\\" + "Dashboard_Update";
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
                //return Ok(new { result = "error", Message = "Server Path was not exist" });
            }

            if (filePath != null)
            {
                filePath = strFilePath + "\\" + strfilename;
                if (System.IO.File.Exists(filePath))
                {
                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
                    string strVersion = myFileVersionInfo.FileVersion.Trim().ToString();
                    strVersion = strVersion.Replace(".", string.Empty);
                    return Ok(new { result = strVersion, Message = "Download and Update One United Pinas Dashboard" });
                }
                return Ok(new { result = "error", Message = "Update file was not exist" });
            }
            return NotFound();
        }
        [HttpPost]
        [Route("download/update")]
        public async Task<IActionResult> Task0b()
        {
            var strfilename = "OneUnitedPinas.exe";
            var filePath = _webHostEnvironment.WebRootPath;
            var strFilePath = filePath + "\\" + "Dashboard_Update";
            var strFP = HttpContext.Request.Host.ToUriComponent();
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
                //return Ok(new { result = "error", Message = "Server Path was not exist" });
            }

            if (filePath != null)
            {
                filePath = strFilePath + "\\" + strfilename;
                if (System.IO.File.Exists(filePath))
                {
                    string htmlFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "OneUnitedPinas.exe");
                    string contentPath = this._webHostEnvironment.ContentRootPath;
                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
                    string strVersion = myFileVersionInfo.FileVersion.Trim().ToString();
                    strVersion = strVersion.Replace(".", string.Empty);
                    return Ok(new { result = contentPath, Message = "Download and Update One United Pinas Dashboard" });
                }
                return Ok(new { result = "error", Message = "Update file was not exist" });
            }
            return NotFound();
        }
        [HttpPost]
        [Route("download/update1")]
        public FileResult DownLoadZip()
        {
            var webRoot = _webHostEnvironment.WebRootPath;
            var fileName = "updated.zip";
            var tempOutput = webRoot + "/Dashboard_Download/" + fileName;

            using (ZipOutputStream IzipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
            {
                IzipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var imageList = new List<string>();
                var filePath = _webHostEnvironment.WebRootPath;
                var strFilePath = filePath + "\\" + "Dashboard_Update";
                imageList.Add(webRoot + "/Dashboard_Update/OneUnitedPinas.exe");
                imageList.Add(webRoot + "/Dashboard_Update/OneUnitedPinas.pdb");

                if (strFilePath != null)
                {
                    for (int i = 0; i < imageList.Count; i++)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(imageList[i]));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        IzipOutputStream.PutNextEntry(entry);

                        using (FileStream oFileStream = System.IO.File.OpenRead(imageList[i]))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = oFileStream.Read(buffer, 0, buffer.Length);
                                IzipOutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                }
                
                
                IzipOutputStream.Finish();
                IzipOutputStream.Flush();
                IzipOutputStream.Close();
            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);
            if (System.IO.File.Exists(tempOutput))
            {
                System.IO.File.Delete(tempOutput);
            }
            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(String.Format("Nothing found"));

            }

            return File(finalResult, "application/zip", fileName);
        }
    }
}
