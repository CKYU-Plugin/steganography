using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Property
{
    public class Config
    {


        public static string GetAppFolder()
        {
            switch (HandlerProperty.robot)
            {
                case RobotType.MPQ:
                    return "Plugin";
                case RobotType.CQ:
                    return "app";
                default:
                    return "";
            }
        }

        public static void LoadProperty()
        {
            HandlerProperty.SProperty.version = 5;

            try
            {
                if (File.Exists(GetAppFolder() + @"\SteganographyConfig.json"))
                {
                    string readText = File.ReadAllText(GetAppFolder() + @"\SteganographyConfig.json");
                    List<SetProperty> tmp = JsonConvert.DeserializeObject<List<SetProperty>>(readText);
                    if (tmp.Count > 0)
                    {
                        if (tmp.FirstOrDefault().version == HandlerProperty.SProperty.version)
                        {
                            HandlerProperty.SProperty = tmp.FirstOrDefault();
                        }
                        else
                        {
                            SaveProperty();
                        }
                    }
                }
                else
                {
                    SaveProperty();
                }
            }
            catch(Exception e) { Console.WriteLine(e); }
        }

        public static void SaveProperty()
        {
            try
            {
                List<SetProperty> tmp = new List<SetProperty>();
                tmp.Add(HandlerProperty.SProperty);
                var json = JsonConvert.SerializeObject(tmp);
                File.WriteAllText(GetAppFolder() + @"\SteganographyConfig.json", json);
            }
            catch (Exception e) { Console.WriteLine(e); }
        }


        }
}
