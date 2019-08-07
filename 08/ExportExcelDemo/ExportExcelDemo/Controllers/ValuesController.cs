namespace ExportExcelDemo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            List<Student> lstStudent = StaticDataOfStudent.ListStudent;
            string[] columns = { "ID", "Name", "Age" , "Gender" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(lstStudent, "Sheet1", columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "MyStudent.xlsx");   
        }
    }
}
