using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HomeController : ControllerBase {
    [Route("")]
    [Route("Home")]
    [Route("Home/Index")]
    public String Index() {
        return "Hello DICOM";
    }

    [Route("studies/{study}/rendered")]
    public FileStreamResult Rendered(string study) {
        string mimeType = "application/dicom";

        Stream stream = System.IO.File.OpenRead("DICOM/0002.DCM");

        return new FileStreamResult(stream, mimeType);
    }
}