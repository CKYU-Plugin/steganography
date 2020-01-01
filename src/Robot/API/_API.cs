using RestSharp;
using Steganography.Extension;
using Steganography.Property;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Robot.Code
{
    public class _API
    {
        public static string Api_GuidGetPicLink(string imagecode)
        {
            switch (HandlerProperty.robot)
            {
                case RobotType.MPQ:
                    return MPQMessageAPI.Api_GuidGetPicLink(imagecode);
                case RobotType.Test:
                    //   return "https://gchat.qpic.cn/gchatpic_new/2818618094/581834397-2561439453-9FAF911609585D70552823EA8913E0E0/0?vuin=2143310855&term=2";
                    return "http://gchat.qpic.cn/gchatpic_new/1512361598/1512361598-2327262009-48A743B5E1E5B894F8BF8A804A90E1C4/0?vuin=2143310855&term=1";
                case RobotType.CQ:
                    try
                    {
                        string path = String.Format(@"data\image\{0}.cqimg", imagecode.Split('=')[1].Replace("]", ""));
                        if (File.Exists(path))
                        {
                            IniFile ini = new IniFile(path);
                            string url = ini.IniReadValue("image", "url");
                            return url;
                        }
                    }
                    catch { }
                    return "";
                default:
                    return "";
            }
        }

        public static string Api_Sha1(Image image)
        {
            string image_sha1 = "";

            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = new byte[1];
            imgBytes = (byte[])converter.ConvertTo(image, imgBytes.GetType());
            SHA1Managed sha = new SHA1Managed();
            byte[] imgHash1 = sha.ComputeHash(imgBytes);
            var sb = new StringBuilder(imgHash1.Length * 2);

            foreach (byte b in imgHash1)
            {
                sb.Append(b.ToString("X2"));
            }
            image_sha1 = sb.ToString();

            return image_sha1;
        }

        public static string Api_UploadPic(Image image, string qq="", int uploadtype=0, string gdid="")
        {

            var file_image_format = typeof(System.Drawing.Imaging.ImageFormat).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).ToList().ConvertAll(property => property.GetValue(null, null)).Single(image_format => image_format.Equals(image.RawFormat));

            string image_sha1 = "";//System.Guid.NewGuid().ToString().Replace("-", "");

            image_sha1 = Api_Sha1(image);

            string filename = image_sha1 + "." + file_image_format.ToString().ToLower();

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "data");
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"data\image");
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"data\image\Steganography");

            string path = @"data\image\";
            string pluginpath = @"Steganography\";
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path + pluginpath + filename;

            image.Save(fullpath, image.RawFormat);

            switch (HandlerProperty.robot)
            {
                case RobotType.MPQ:
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, image.RawFormat);
                        string imageguid = MPQMessageAPI.Api_Upload(qq, fullpath, ms.ToArray());
                        return imageguid;
                    }
                case RobotType.Test:
                case RobotType.CQ:
                        string name = "[CQ:image,file=" + pluginpath + filename + "]";
                        return name;
                default:
                    return "";
            }
        }

        public static int Api_OutPut(string outstring)
        {
            switch (HandlerProperty.robot)
            {
                case RobotType.MPQ:
                    return MPQMessageAPI.Api_OutPut(outstring);
                case RobotType.CQ:
                    return CQAPI.AddLog(HandlerProperty.CQ_AuthCode, 0, 0, outstring);
                case RobotType.Test:
                    Console.WriteLine(outstring);
                    return 0;
                default:
                    return 0;
            }
        }

        public static void SendMessage(string qq, int msgType, string content, string gdid, string robotQQ,int msgSubType)
        {
            long _qq = 0;
            long _gdid = 0;
            Int64.TryParse(qq, out _qq);
            Int64.TryParse(gdid, out _gdid);

            switch (HandlerProperty.robot)
            {
                case RobotType.MPQ:
                    MPQMessageAPI.Api_SendMsg(robotQQ, msgType, msgSubType, gdid, qq, content);
                    break;
                case RobotType.CQ:
                    if(msgType.In(1 , 4))
                    {
                        CQAPI.SendPrivateMessage(HandlerProperty.CQ_AuthCode, _qq, content);
                    }
                    else if(msgType.In(2, 3))
                    {
                        CQAPI.SendGroupMessage(HandlerProperty.CQ_AuthCode, _gdid, content);
                    }
                    break;
                default:
                    Console.WriteLine(content);
                    break;
            }
        }
    }
}
