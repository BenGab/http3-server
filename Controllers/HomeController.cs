using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class HomeController : ControllerBase {
    const string seriesMainpath = "/home/bengab/Development/series";
    private readonly ConcurrentDictionary<string, byte[]> dicomCache;

    private void LoadCacheBySeries(string[] filepaths) {
        foreach(var path in filepaths) {
            var filename = Path.GetFileNameWithoutExtension(path)?.Replace("image-", string.Empty);

            if(filename == null) {
                continue;
            }

            var bytes = System.IO.File.ReadAllBytes(path);
            dicomCache.TryAdd(filename, bytes);
        }
    }

    public HomeController()
    {
        dicomCache = new ConcurrentDictionary<string, byte[]>();
    }

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

        var files = Directory.GetFiles(path);
        Task.Factory.StartNew(() => {
            LoadCacheBySeries(files);
        }, TaskCreationOptions.DenyChildAttach);

        return files.AsEnumerable()
                .Select(path => Path.GetFileNameWithoutExtension(path) ?? string.Empty)
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Replace("image-", string.Empty)).ToList();
    }

    [Route("/studies/{study}/series/{series}/instances/{instance}")]
    public FileStreamResult GetInstance(string study, string series, string instance) {
        byte[] fileStreamBytes;
        if(dicomCache.TryGetValue(instance, out fileStreamBytes)) {
            Stream result = new MemoryStream(fileStreamBytes);
            return new FileStreamResult(result, "application/dicom");
        }

        string path = $"{seriesMainpath}/series-{series}/image-{instance}.dcm";
        Stream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
 
        return new FileStreamResult(stream, "application/dicom");
    }
}