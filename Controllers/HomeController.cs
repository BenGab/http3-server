using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HomeController : ControllerBase {
    const string seriesMainpath = "/home/bengab/Development/series"; 

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

    [Route("/studies/{study}/series/{series}")]
    public List<string> GetSeries(string study, string series) {
        string path = $"{seriesMainpath}/series-{series}";

        if(!Directory.Exists(path)) {
            return [];
        }

        return Directory.GetFiles(path).AsEnumerable()
                .Select(path => Path.GetFileNameWithoutExtension(path) ?? string.Empty)
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Replace("image-", string.Empty)).ToList();
    }

    [Route("/studies/{study}/series/{series}/instances/{instance}")]
    public FileStreamResult GetInstance(string study, string series, string instance) {
        string path = $"{seriesMainpath}/series-{series}/image-{instance}.dcm";
        Stream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
 
        return new FileStreamResult(stream, "application/dicom");
    }
}