using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        public class data
        {
            public string base64Image { get; set; }
        }

        // GET: api/<FaceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody] data base64Image)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "04b3d203a0b74db08bba5834f5f43223");

            string url = "https://face-recognition-masked-unmasked.cognitiveservices.azure.com/customvision/v3.0/Prediction/68e29f1e-edea-4108-85e4-4a92480efe57/classify/iterations/Mask%20Immune%20Face%20Recognition%20System/image";

            HttpResponseMessage response;

            byte[] byteData = Convert.FromBase64String(base64Image.base64Image);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}