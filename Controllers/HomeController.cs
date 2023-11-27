using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HomeController : ControllerBase {
    [Route("")]
    [Route("Home")]
    [Route("Home/Index")]
    public String Index() {
        return "Hello DICOM";
    }
}