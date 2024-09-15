using Microsoft.AspNetCore.Mvc;

namespace Asp.Excel
{
    [ApiController]
    [Route("api/[controller]")]
    public class TendersController : ControllerBase
    {
        private readonly ExcelTenderService _tenderService;

        public TendersController(ExcelTenderService tenderService)
        {
            _tenderService = tenderService;
        }

        [HttpGet("files")]
        public IActionResult GetExcelFiles()
        {
            var files = _tenderService.GetExcelFiles();
            return Ok(files);
        }

        [HttpGet("{fileName}")]
        public IActionResult GetTenders(string fileName)
        {
            var tenders = _tenderService.GetTenders(fileName);
            return Ok(tenders);
        }
    }

}
