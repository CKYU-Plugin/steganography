using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography
{
    public class ImageHandler
    {




        public void DownloadQQImage(string url , Action<byte[]> callback)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
            request.AddHeader("Referer", "http://qq.com");

            client.ExecuteAsync(request, (response) =>
            {
                callback(response.RawBytes);
            });
         }


    }
}
