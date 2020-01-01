using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F5;
using System.IO;
using F5.James;

namespace Steganography
{
    public class _Steganography
    {

        public bool decode(out string data, Stream stream_image,  string password = "71C54CE9-A4DE-446A-9201-63BB0E184ADC")
        {
            string o = string.Empty;
            bool isSteganography = false;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    using (JpegExtract extractor = new JpegExtract(ms, password))
                    {
                        isSteganography = extractor.Extract(stream_image);
                    }
                    if (isSteganography)
                    {
                        ms.Position = 0;
                        StreamReader reader = new StreamReader(ms, System.Text.Encoding.UTF8, true);
                        o = reader.ReadToEnd();
                        data = o;
                        return isSteganography;
                    }

                }
            }
            catch { }

            data = "";
            return isSteganography;
        }

        public Stream encode(Image image, string contect , int quality = 80, string comment = "Steganography", string password = "71C54CE9-A4DE-446A-9201-63BB0E184ADC")
        {
            MemoryStream s_out = new MemoryStream();
            try
            {
                Stream s_msg = null;
                string test = contect;
                byte[] byteArray = Encoding.UTF8.GetBytes(test);
                s_msg = new MemoryStream(byteArray);

                using (JpegEncoder jpg = new JpegEncoder(image, s_out, comment, quality))
                {
                    jpg.Compress(s_msg, password);
                }
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }

            return s_out;
        }

     }
}
