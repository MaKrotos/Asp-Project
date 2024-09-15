using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net.Http.Headers;
using System.Reflection;
using TendersLib;

namespace Core_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TendersController : ControllerBase
    {
        private readonly string _tendersFolder = Path.Combine(Directory.GetCurrentDirectory(), "Tenders");

        [HttpGet("files")]
        public IActionResult GetFiles()
        {
            if (!AuthenticateRequest(Request))
            {
                return Unauthorized();
            }

            var xlsFiles = Directory.GetFiles(_tendersFolder, "*.xls").Select(Path.GetFileName);
            var xlsxFiles = Directory.GetFiles(_tendersFolder, "*.xlsx").Select(Path.GetFileName);
            var files = xlsFiles.Concat(xlsxFiles).ToList();

            return Ok(files);
        }



        [HttpGet("data/{fileName}")]
        public IActionResult GetData(string fileName)
        {
            if (!AuthenticateRequest(Request))
            {
                return Unauthorized();
            }

            var filePath = Path.Combine(_tendersFolder, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];

                    var columnIndices = new Dictionary<string, int>();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        columnIndices[table.Rows[0][i].ToString()] = i;
                    }

                    var tenders = new List<Tender>();

                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        tenders.Add(new Tender
                        {
                            Name = row[columnIndices["Название тендера"]].ToString(),
                            DateStart = row[columnIndices["Дата начала"]].ToString(),
                            DateEnd = row[columnIndices["Дата окончания"]].ToString(),
                            URL = row[columnIndices["URL тендерной площадки"]].ToString()
                        });
                    }

                    return Ok(tenders);
                }
            }
        }

        private bool AuthenticateRequest(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authHeader = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = System.Text.Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            return username == "mvcuser" && password == "mvcpassword";
        }
    }
}

