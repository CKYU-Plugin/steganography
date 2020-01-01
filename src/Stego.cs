using ImgMetadata;
using MapImage;
using Newtonsoft.Json;
using RestSharp;
using Steganography.Extension;
using Steganography.Property;
using Steganography.Robot.Code;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Steganography
{
    public static class Stego
    {

        private class Command_Steganography
        {
            public Command_step step;
            public List<string> imagecode = new List<string>();
            public string contect = "";
            public string password = "";
            public bool autopassword = false;
            public int count = 0;
            public DateTime Reset;
        }

        private enum Command_step : int
        {
            init = -1,
            decode_image = 0,
            decode_image_password = 1,
            decode_dectect = 2,
            encode_image = 10,
            encode_image_contect = 11,
            encode_image_password = 12,
            encode_image_password_auto = 13,
            encode_image_password_encode = 14,
            metadata = 20,
            statistics = 30,
            end = 99,
            set = 100,
            set_refresh = 110,
            set_quota = 120,
            set_sensitiveword = 130,
            set_monitor = 140,
            set_notification_admin = 141,
            set_notification_target = 142,
            set_notification_delete = 143,
        }

        private class StegoBackup
        {
            public string imagecode { get; set; }
            public string sha1 { get; set; }
            public ImageStegoVlaue contect { get; set; }
            public string createdby { get; set; }
            public List<string> creater { get; set; }
            public DateTime createdate { get; set; }
            public int read { get; set; }
            public List<string> guest { get; set; }
        }

        private static ConcurrentDictionary<string, Command_Steganography> dict = new ConcurrentDictionary<string, Command_Steganography>();
        private static ReaderWriterLock Backup_locker = new ReaderWriterLock();

        public static void Run(string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, string targetPassive, string msgContent , int messageid)
        {
            if (msgType.In(1, 2, 3, 4))
            {
                bool isatme;
                string at = msgType == 1 ? null : msgSrc;
                List<string> imagecode = new List<string>();
                Match match_atme;
                bool iscomand = false;

                msgContent = msgContent.Trim();
                match_atme = Regex.Match(msgContent, _Code.Code_At_Regex());
                isatme = match_atme.Success;
                if (isatme) { msgContent = msgContent.Replace(match_atme.Value, ""); }
                foreach (Match regmatch in Regex.Matches(msgContent, _Code.Code_Image_Regex()))
                {
                    if (regmatch.Success)
                    {
                        imagecode.Add(regmatch.Groups[0].Value);
                    }
                }

                if (HandlerProperty.SProperty.Set_monitor)
                {
                    if (imagecode.Count > 0)
                    {
                        foreach (var i in imagecode)
                        {
                            Run_Metadata(i, robotQQ, 1, msgSubType, msgSrc, targetActive, at, messageid, true);
                        }
                    }
                }


                if ((msgContent.Contains(HandlerProperty.SProperty.Keyword_end)))
                {
                    if (dict.Where(w => w.Key == msgSrc).ToList().Count > 0)
                    {
                        dict[msgSrc].step = Command_step.end;
                        _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_end),
                            targetActive, robotQQ, msgSubType);
                    }
                    else
                    {
                        _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_startfirst),
                            targetActive, robotQQ, msgSubType);
                    }
                    return;
                }

                if (msgContent.Contains(HandlerProperty.SProperty.Keyword))
                {
                    if (dict.Where(w => w.Key == msgSrc).ToList().Count == 0)
                    {
                        dict.TryAdd(msgSrc, new Command_Steganography { step = Command_step.init, Reset = DateTime.Now });
                    }
                    else
                    {
                        if (dict.Where(w => w.Key == msgSrc).Where(w => w.Value.step == Command_step.end).ToList().Count > 0)
                        {
                            dict[msgSrc].step = Command_step.init;
                        }
                    }
                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_start),
                        targetActive, robotQQ, msgSubType);
                    return;
                }

                if (dict.Where(w => w.Key == msgSrc).Where(w=>w.Value.step== Command_step.init).ToList().Count > 0)
                {

                    if (msgType != 1)
                    {
                        if (HandlerProperty.SProperty.isAtMe)
                        {
                            if (!isatme) { return; }
                        }
                    }

                    if (msgContent.Contains(HandlerProperty.SProperty.Keyword_encode))
                    {
                        dict[msgSrc].step = Command_step.encode_image;
                        iscomand = true;
                    }

                    if (msgContent.Contains(HandlerProperty.SProperty.Keyword_decode))
                    {
                        dict[msgSrc].step = Command_step.decode_image;
                        iscomand = true;
                    }

                    if (msgContent.Contains(HandlerProperty.SProperty.Keyword_metadata))
                    {
                        dict[msgSrc].step = Command_step.metadata;
                        iscomand = true;
                    }

                    if (msgContent.Contains(HandlerProperty.SProperty.Keyword_statistics))
                    {
                        if (msgSrc == HandlerProperty.SProperty.admin)
                        {
                            dict[msgSrc].step = Command_step.statistics;
                            iscomand = true;
                        }
                        else
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_setErrMsg),
targetActive, robotQQ, msgSubType);
                        }
                    }

                    if (msgContent.Contains(HandlerProperty.SProperty.Keyword_set))
                    {
                        if (msgSrc == HandlerProperty.SProperty.admin)
                        {
                            dict[msgSrc].step = Command_step.set;
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_set),
    targetActive, robotQQ, msgSubType);
                        }
                        else
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_setErrMsg),
targetActive, robotQQ, msgSubType);
                        }
                    }

                }

                if (iscomand)
                {
                    if ((DateTime.Now - dict[msgSrc].Reset).TotalMinutes > HandlerProperty.SProperty.Set_Refresh)
                    {
                        dict[msgSrc].count = 0;
                        dict[msgSrc].Reset = DateTime.Now;
                    }
                    if (dict[msgSrc].count == HandlerProperty.SProperty.Set_Quota)
                    {
                        _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_over,"", (DateTime.Now - dict[msgSrc].Reset).TotalMinutes),
                            targetActive, robotQQ, msgSubType);
                        dict[msgSrc].step = Command_step.init;
                        return;
                    }
                }

                if(dict.Where(w => w.Key == msgSrc).ToList().Count == 0) { return; }

                switch (dict[msgSrc].step)
                {
                    case Command_step.statistics:
                        foreach (var i in dict[msgSrc].imagecode)
                        {
                            Run_statistics(i, robotQQ, msgType, msgSubType, at, targetActive, targetPassive, false);
                        }
                        Run_statistics(string.Empty, robotQQ, msgType, msgSubType, at, targetActive, targetPassive, false);
                        dict[msgSrc].imagecode.Clear();
                        dict[msgSrc].step = Command_step.init;
                        return;
                    case Command_step.set:
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_refresh))
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_refresh),
    targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.set_refresh;
                            return;
                        }
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_quota))
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_quota),
    targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.set_quota;
                            return;
                        }
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_sensitiveword))
                        {
                            HandlerProperty.SProperty.Set_sensitiveword = !HandlerProperty.SProperty.Set_sensitiveword;
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_sensitivewords_ok),
    targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.init;
                            return;
                        }
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_monitor))
                        {
                            HandlerProperty.SProperty.Set_monitor = !HandlerProperty.SProperty.Set_monitor;
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_monitor_onoff),
targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.init;
                            return;
                        }
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_notification_admin))
                        {
                            HandlerProperty.SProperty.Set_notification_admin = !HandlerProperty.SProperty.Set_notification_admin;
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_notification_admin_onoff),
targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.init;
                            return;
                        }
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_notification_target))
                        {
                            HandlerProperty.SProperty.Set_notification_target = !HandlerProperty.SProperty.Set_notification_target;
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_notification_target_onoff),
targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.init;
                            return;
                        }
                        if (msgContent.Contains(HandlerProperty.SProperty.Keyword_notification_delete))
                        {
                            HandlerProperty.SProperty.Set_notification_delete = !HandlerProperty.SProperty.Set_notification_delete;
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_notification_delete_onoff),
targetActive, robotQQ, msgSubType);
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].step = Command_step.init;
                            return;
                        }
                        return;
                    case Command_step.set_refresh:
                        bool isnum_refresh = false;
                        int tmp_refresh = HandlerProperty.SProperty.Set_Refresh;
                        isnum_refresh = Int32.TryParse(Regex.Match(msgContent, @"\d+").Value,out tmp_refresh);
                        HandlerProperty.SProperty.Set_Refresh = tmp_refresh;
                        if (tmp_refresh > 0)
                        {
                            if (isnum_refresh)
                            {
                                _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_refresh_ok),
    targetActive, robotQQ, msgSubType);
                                dict[msgSrc].imagecode.Clear();
                                dict[msgSrc].count += 1;
                                dict[msgSrc].step = Command_step.init;
                            }
                        }
                        return;
                    case Command_step.set_quota:
                        bool isnum_quota = false;
                        int tmp_quota = HandlerProperty.SProperty.Set_Quota;
                        isnum_quota = Int32.TryParse(Regex.Match(msgContent, @"\d+").Value, out tmp_quota);
                        if (tmp_quota >= 0)
                        {
                            HandlerProperty.SProperty.Set_Quota = tmp_quota;
                            if (isnum_quota)
                            {
                                _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_quota_ok),
    targetActive, robotQQ, msgSubType);
                                dict[msgSrc].imagecode.Clear();
                                dict[msgSrc].count += 1;
                                dict[msgSrc].step = Command_step.init;
                            }
                        }
                        return;
                    case Command_step.metadata:
                        dict[msgSrc].imagecode.AddRange(imagecode);
                        if (dict[msgSrc].imagecode.Count == 0)
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(at, HandlerProperty.SProperty.Msg_metadataMsg),
                                targetActive, robotQQ, msgSubType);
                            return;
                        }
                        else
                        {
                            foreach (var i in dict[msgSrc].imagecode)
                            {
                                Run_Metadata(i, robotQQ, msgType, msgSubType, at, targetActive, targetPassive,messageid, false);

                            }
                            dict[msgSrc].imagecode.Clear();
                            dict[msgSrc].count += 1;
                            dict[msgSrc].step = Command_step.init;
                        }
                        return;

                    case Command_step.encode_image:
                        dict[msgSrc].imagecode.AddRange(imagecode);
                        if (dict[msgSrc].imagecode.Count == 0)
                        {
                            _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_encodeMsg,
                                targetActive, robotQQ, msgSubType);
                            return;
                        }
                        else
                        {
                            _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_encodeMsg2,
                                targetActive, robotQQ, msgSubType);
                            dict[msgSrc].step = Command_step.encode_image_contect;
                        }
                        return;
                    case Command_step.encode_image_contect:
                        dict[msgSrc].contect = msgContent;
                        _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_encodeMsg3,
    targetActive, robotQQ, msgSubType);
                        dict[msgSrc].step = Command_step.encode_image_password;
                        return;
                    case Command_step.encode_image_password:
                        dict[msgSrc].password = msgContent;
                        _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_encodeMsg4,
targetActive, robotQQ, msgSubType);
                        dict[msgSrc].step = Command_step.encode_image_password_auto;
                        return;
                    case Command_step.encode_image_password_auto:
                        if (msgContent.Equals("是") | msgContent.Equals("1") | msgContent.Equals("yes"))
                        {
                            dict[msgSrc].autopassword = true;
                        }
                        else
                        {
                            dict[msgSrc].autopassword = false;
                        }
                        _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_encodeMsg5,
targetActive, robotQQ, msgSubType);
                        foreach (var i in dict[msgSrc].imagecode)
                        {
                            if (dict[msgSrc].autopassword)
                            {

                            }
                            Run_Encode(i, msgContent, dict[msgSrc].contect, robotQQ, msgType, msgSubType, at, targetActive, targetPassive, dict[msgSrc].autopassword);
                        }
                        dict[msgSrc].imagecode.Clear();
                        dict[msgSrc].count += 1;
                        dict[msgSrc].step = Command_step.init;
                        return;
                    case Command_step.decode_image:
                        dict[msgSrc].imagecode.AddRange(imagecode);
                        if (dict[msgSrc].imagecode.Count == 0)
                        {
                            _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_decodeMsg,
                                targetActive, robotQQ, msgSubType);
                            return;
                        }
                        else
                        {
                            _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_decodeMsg2,
                                targetActive, robotQQ, msgSubType);
                            dict[msgSrc].step = Command_step.decode_image_password;
                        }
                        return;

                    case Command_step.decode_image_password:
                        _API.SendMessage(msgSrc, msgType, HandlerProperty.SProperty.Msg_decodeMsg3,
                            targetActive, robotQQ, msgSubType);
                        foreach (var i in dict[msgSrc].imagecode)
                        {
                            Run_Decode(i, msgContent, robotQQ, msgType, msgSubType, at, targetActive, targetPassive);
                        }
                        dict[msgSrc].imagecode.Clear();
                        dict[msgSrc].count += 1;
                        dict[msgSrc].step = Command_step.init;
                        return;
                }

            }
        }

        private static string ReplaceKeyword(string qq,string msg, string imageCode = "", double datediff = 0)
        {
            msg = (qq!=null ? _Code.Code_At(qq) + Environment.NewLine : "") + msg;

           return msg.Replace("{关秘图密语}", HandlerProperty.SProperty.Keyword_end)
                    .Replace("{秘图密语}", HandlerProperty.SProperty.Keyword)
                    .Replace("{加密}", HandlerProperty.SProperty.Keyword_encode)
                    .Replace("{解密}", HandlerProperty.SProperty.Keyword_decode)
                    .Replace("{查坐标}", HandlerProperty.SProperty.Keyword_metadata)
                    .Replace("{图}", imageCode)
                    .Replace("{刷新时间V}", HandlerProperty.SProperty.Set_Refresh.ToString())
                    .Replace("{刷新时间G}", (HandlerProperty.SProperty.Set_Refresh - (int)datediff).ToString())
                    .Replace("{限额V}", HandlerProperty.SProperty.Set_Quota.ToString())
                    .Replace("{设定}", HandlerProperty.SProperty.Keyword_set)
                    .Replace("{刷新时间}", HandlerProperty.SProperty.Keyword_refresh)
                    .Replace("{限额}", HandlerProperty.SProperty.Keyword_quota)
                    .Replace("{限额}", HandlerProperty.SProperty.Keyword_quota)
                    .Replace("{敏感词筛查}", HandlerProperty.SProperty.Keyword_sensitiveword)
                    .Replace("{敏感词筛查S}", HandlerProperty.SProperty.Set_sensitiveword ? "启用" : "关闭")
                    .Replace("{!敏感词筛查S}", HandlerProperty.SProperty.Set_sensitiveword ? "关闭" : "启用")
                    .Replace("{监控}", HandlerProperty.SProperty.Keyword_monitor)
                    .Replace("{监控S}", HandlerProperty.SProperty.Set_monitor ? "启用" : "关闭")
                    .Replace("{!监控S}", HandlerProperty.SProperty.Set_monitor ? "关闭" : "启用")
                    .Replace("{通知主人}", HandlerProperty.SProperty.Keyword_notification_admin)
                    .Replace("{通知主人S}", HandlerProperty.SProperty.Set_notification_admin ? "启用" : "关闭")
                    .Replace("{!通知主人S}", HandlerProperty.SProperty.Set_notification_admin ? "关闭" : "启用")
                    .Replace("{通知目标}", HandlerProperty.SProperty.Keyword_notification_target)
                    .Replace("{通知目标S}", HandlerProperty.SProperty.Set_notification_target ? "启用" : "关闭")
                    .Replace("{!通知目标S}", HandlerProperty.SProperty.Set_notification_target ? "关闭" : "启用")
                    .Replace("{撒消}", HandlerProperty.SProperty.Keyword_notification_delete)
                    .Replace("{撒消S}", HandlerProperty.SProperty.Set_notification_delete ? "启用" : "关闭")
                    .Replace("{!撒消S}", HandlerProperty.SProperty.Set_notification_delete ? "关闭" : "启用")
                    .Replace("{统计}", HandlerProperty.SProperty.Keyword_statistics)
                    .Replace("{主人}", HandlerProperty.SProperty.admin)
                    ;
        }

        private static void Run_statistics(string imageCode, string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, string targetPassive, bool ismonitor)
        {
            if (imageCode != string.Empty)
            {
                string url = _API.Api_GuidGetPicLink(imageCode);
                if (url != "")
                {
                    var client = new RestClient(url);
                    var request = new RestRequest("", Method.GET);
                    client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                    request.AddHeader("Referer", "http://qq.com");

                    client.ExecuteAsync(request, (response) =>
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            using (var memoryStream = new MemoryStream(response.RawBytes))
                            {
                                Image image_out = Image.FromStream(memoryStream);
                                string sha1 = _API.Api_Sha1(image_out);
                                image_out.Dispose();
                                string filepath = @"data\image\Steganography\" + sha1 + ".bk";
                                if (File.Exists(filepath))
                                {
                                    Backup_locker.AcquireReaderLock(int.MaxValue);
                                    StegoBackup sb = JsonConvert.DeserializeObject<StegoBackup>(File.ReadAllText(filepath));
                                    Backup_locker.ReleaseReaderLock();
                                    if (sb != null)
                                    {
                                        string message = 
$@"
{sb.imagecode}
作者:{sb.creater}
日期:{sb.createdate}
內容:{sb.contect}
解密数:{sb.read-1}
解密者:{string.Join(",",sb.guest.ToList())}
";
                                        _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, message),
    targetActive, robotQQ, msgSubType);
                                    }
                                }
                                else
                                {
                                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_statisticsErrMsg),
targetActive, robotQQ, msgSubType);
                                }
                             }
                        }
                    });
                }
            }
            else
            {
                List<StegoBackup> sbl = new List<StegoBackup>();

                Backup_locker.AcquireReaderLock(int.MaxValue);
                foreach (var file in Directory.GetFiles(@"data\image\Steganography", "*.bk"))
                {
                    StegoBackup sb = JsonConvert.DeserializeObject<StegoBackup>(File.ReadAllText(file));
                    if (sb != null)
                    {
                        sbl.Add(sb);
                    }
                }
                Backup_locker.ReleaseReaderLock();

                sbl = sbl.OrderByDescending(o => o.read).ToList();
                string message = "";
                for (int i = 0; i < 10; i++)
                {
                    if (sbl.Count <= i) { break; }

                message +=
$@"第{i+1}名 ({sbl[i].read-1}解密)
{sbl[i].imagecode}
作者:{sbl[i].createdby}
日期:{sbl[i].createdate}
內容:{sbl[i].contect.value}
解密者:{string.Join(",", sbl[i].guest.ToList())}
";
                }
                _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, message),
targetActive, robotQQ, msgSubType);

            }
        }

        private static void Run_Metadata(string imageCode, string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, string targetPassive, int messageid, bool ismonitor)
        {
            try
            {
                string url = _API.Api_GuidGetPicLink(imageCode);

                if (url != "")
                {
                    var client = new RestClient(url);
                    var request = new RestRequest("", Method.GET);
                    client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                    request.AddHeader("Referer", "http://qq.com");

                    client.ExecuteAsync(request, (response) =>
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ImageMetadata imd = new ImageMetadata();
                            imd = Get_MetaData(response.RawBytes);
                            if (imd.GPS_LatLon != "")
                            {
                          //      _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, imd.GPS_LatLon), targetActive, robotQQ, msgSubType);
                                MapImage(imd, robotQQ, msgType, msgSubType, msgSrc, targetActive , messageid, ismonitor);
                            }
                            else
                            {
                                if (!ismonitor)
                                {
                                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_metadataErrMsg, imageCode),
                                        targetActive, robotQQ, msgSubType);
                                }
                            }
                        }
                        else
                        {
                            if (!ismonitor)
                            {
                                _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.UrlErrorMessage2),
targetActive, robotQQ, msgSubType);
                            }
                        }
                    });
                }
                else
                {
                    if (!ismonitor)
                    {
                        _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.UrlErrorMessage),
    targetActive, robotQQ, msgSubType);
                    }
                }

            }
            catch { }
        }


        private static void Run_Encode(string imageCode, string password, string data, string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, string targetPassive, bool auto)
        {
            try
            {
                string url = _API.Api_GuidGetPicLink(imageCode);
                if (url != "")
                {
                    var client = new RestClient(url);
                    var request = new RestRequest("", Method.GET);
                    client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                    request.AddHeader("Referer", "http://qq.com");

                    client.ExecuteAsync(request, (response) =>
                    {

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            try
                            {
                                ImageStegoVlaue iv = new ImageStegoVlaue();
                                iv.type = ImageStegoVlaueType.Text;
                                iv.value = data;

                                Stream Stream_image_out = Run_Encode(response.RawBytes, iv, auto ? password : "秘图密语", password);
                                Image image_out = Image.FromStream(Stream_image_out);
                                string imageguid = _API.Api_UploadPic(image_out, robotQQ, msgType, targetActive);


                                string sha1 = _API.Api_Sha1(image_out);

                                string filepath = @"data\image\Steganography\" + sha1 + ".bk";

                                StegoBackup sb = new StegoBackup();
                                sb.imagecode = imageCode;
                                sb.contect = iv;
                                sb.createdate = DateTime.Now;
                                sb.createdby = msgSrc;
                                sb.guest = new List<string>();
                                sb.read = 0;
                                sb.sha1 = sha1;
                                sb.creater = new List<string>();

                                if (File.Exists(filepath))
                                {
                                    try
                                    {
                                        Backup_locker.AcquireReaderLock(int.MaxValue);
                                        StegoBackup sbtmp = JsonConvert.DeserializeObject<StegoBackup>(File.ReadAllText(filepath));
                                        Backup_locker.ReleaseReaderLock();
                                        if (sbtmp != null)
                                        {
                                            sb = sbtmp;
                                        }
                                    }
                                    catch { }
                                }
                                sb.read += 1;
                                sb.creater.Add(msgSrc);
                                var json = JsonConvert.SerializeObject(sb);
                                Backup_locker.AcquireWriterLock(int.MaxValue);
                                File.WriteAllText(filepath, json);
                                Backup_locker.ReleaseWriterLock();

                                //Cache
                                Stream_image_out.Dispose();
                                image_out.Dispose();

                                _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_encodeMsgOut + imageguid),
                                    targetActive, robotQQ, msgSubType);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_encodeErrMsg, imageCode),
                                    targetActive, robotQQ, msgSubType);
                            }

                        }
                        else
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.UrlErrorMessage2),
targetActive, robotQQ, msgSubType);
                        }
                    });
                }
                else
                {
                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.UrlErrorMessage),
    targetActive, robotQQ, msgSubType);
                }

            }
            catch { }
        }

        private static void Run_Decode(string imageCode, string password, string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, string targetPassive)
        {
            try
            {
                string url = _API.Api_GuidGetPicLink(imageCode);
                if (url != "")
                {
                    var client = new RestClient(url);
                    var request = new RestRequest("", Method.GET);
                    client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                    request.AddHeader("Referer", "http://qq.com");

                    client.ExecuteAsync(request, (response) =>
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ImageStegoVlaue isv = new ImageStegoVlaue();

                            try
                            {
                                using (var memoryStream_image = new MemoryStream(response.RawBytes))
                                {
                                    Image image_out = Image.FromStream(memoryStream_image);
                                    string sha1 = _API.Api_Sha1(image_out);
                                    image_out.Dispose();
                                    string filepath = @"data\image\Steganography\" + sha1 + ".bk";
                                    if (File.Exists(filepath))
                                    {
                                        try
                                        {
                                            Backup_locker.AcquireReaderLock(int.MaxValue);
                                            StegoBackup sb = JsonConvert.DeserializeObject<StegoBackup>(File.ReadAllText(filepath));
                                            Backup_locker.ReleaseReaderLock();
                                            if (sb != null)
                                            {
                                                if (sb.sha1 == sha1)
                                                {
                                                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_decodeMsgOut2 + sb.contect.value),
                                                    targetActive, robotQQ, msgSubType);
                                                    bool exists = sb.guest.Any(item => item == msgSrc);
                                                    if (!exists)
                                                    {
                                                        sb.guest.Add(msgSrc);
                                                    }
                                                    sb.read += 1;
                                                    var json = JsonConvert.SerializeObject(sb);
                                                    Backup_locker.AcquireWriterLock(int.MaxValue);
                                                    File.WriteAllText(filepath, json);
                                                    Backup_locker.ReleaseWriterLock();
                                                }
                                            }
                                        }
                                        catch { }
                                    }
                                }

                                isv = Run_Decode(response.RawBytes, password);
                                if (isv == null)
                                {
                                    BitmapSource img2 = BitmapFrame.Create(new MemoryStream(response.RawBytes));
                                    BitmapMetadata md = (BitmapMetadata)img2.Metadata;
                                    _Steganography sg = new _Steganography();
                                    if (md.Comment != null && md.Comment != "秘图密语")
                                    {
                                        isv = Run_Decode(response.RawBytes, md.Comment);
                                        if (isv == null)
                                        {
                                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_decodeErrMsg1, imageCode),
                                        targetActive, robotQQ, msgSubType);
                                        }
                                        else
                                        {
                                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_decodeMsgOut + isv.value),
                                            targetActive, robotQQ, msgSubType);
                                        }
                                    }
                                    else
                                    {
                                        _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_decodeErrMsg, imageCode),
                                        targetActive, robotQQ, msgSubType);
                                    }
                                }
                                else
                                {
                                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.Msg_decodeMsgOut + isv.value),
                                    targetActive, robotQQ, msgSubType);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.UrlErrorMessage2),
targetActive, robotQQ, msgSubType);
                        }
                    });
                }
                else
                {
                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, HandlerProperty.SProperty.UrlErrorMessage),
    targetActive, robotQQ, msgSubType);
                }

            }
            catch { }
        }

        private class ImageStegoVlaue
        {
            public ImageStegoVlaueType type { get; set; } = ImageStegoVlaueType.Text;
            public string value { get; set; } = "";
        }

        private enum ImageStegoVlaueType : int
        {
            Text = 0,
            Url = 1,
            Image = 2,
            Music = 3,
            Xml = 4,
            Object = 5,
            Zip = 6
        }
        private static ImageMetadata Get_MetaData(byte[] bytes)
        {
            using (var memoryStream_metadata = new MemoryStream(bytes))
            {
                _ImgMetadata da = new _ImgMetadata();
                return da.GetLatLonString(memoryStream_metadata);
            }
        }


        private static Stream Run_Encode(byte[] bytes, ImageStegoVlaue input, string comment = "Steganography", string password = "71C54CE9-A4DE-446A-9201-63BB0E184ADC")
        {
            try
            {
                Image imgin;
                Stream Stream_imgout;

                using (var memoryStream_Steganography = new MemoryStream(bytes))
                {
                    _Steganography sg = new _Steganography();
                    imgin = Image.FromStream(memoryStream_Steganography);
                    Stream_imgout = sg.encode(imgin, JsonConvert.SerializeObject(input), 80, comment, password);
                    return Stream_imgout;
                }
            }
            catch(Exception ex) { Console.WriteLine(ex); }
            return null;
        }

        private static ImageStegoVlaue Run_Decode(byte[] bytes, string password = "")
        {
            try
            {
                using (var memoryStream_Steganography = new MemoryStream(bytes))
                {
                    string outtext = "";
                    _Steganography sg = new _Steganography();

                    sg.decode(out outtext, memoryStream_Steganography, password);
                    return JsonConvert.DeserializeObject<ImageStegoVlaue>(outtext);
                }
            }
            catch { }
            return null;
        }

        private static void MapImage(ImageMetadata imd, string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, int messageid, bool ismonitor)
        {
            string x = imd.GPS_LongitudeD;
            string y = imd.GPS_LatitudeD;
            var client = new RestClient(String.Format("https://restapi.amap.com/v3/assistant/coordinate/convert?key=bafb080da08276e2d4553da21ba9171c&locations={0},{1}&coordsys=gps", x, y));
            var request = new RestRequest("", Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
            request.AddHeader("Referer", "https://lbs.amap.com/api/webservice/guide/api");

            client.ExecuteAsync(request, (response) =>
            {
                string location = string.Format("{0},{1}",x,y);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        location = JsonConvert.DeserializeObject<amap_coordinate>(response.Content).locations;
                    }
                    catch { }

                    client = new RestClient(String.Format("http://restapi.amap.com/v3/geocode/regeo?key=bafb080da08276e2d4553da21ba9171c&location={0}", location));

                    client.ExecuteAsync(request, (response_regeo) =>
                        {
                            var client2 = new RestClient(String.Format("http://restapi.amap.com/v3/staticmap?location={0}&zoom=18&size=250*150&markers=large,,:{0}&scale=2&key=bafb080da08276e2d4553da21ba9171c", location));

                            client2.ExecuteAsync(request, (response_amap) =>
                            {
                                string address = "";
                                amapRootobject tmp = new amapRootobject();
                                tmp = JsonConvert.DeserializeObject<amapRootobject>(response_regeo.Content);
                                if (tmp.status == "1")
                                {
                                    address = tmp.regeocode.formatted_address;
                                }
                                if (address == "")
                                {
                                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(null, HandlerProperty.SProperty.Msg_amap_address_err),
targetActive, robotQQ, msgSubType);
                                    return;
                                }
                                Image amapimage;
                                using (var memoryStream = new MemoryStream(response_amap.RawBytes))
                                {
                                    amapimage = Image.FromStream(memoryStream);
                                }
                                string imageguid = _API.Api_UploadPic(amapimage);
                                amapimage.Dispose();

                                if (HandlerProperty.SProperty.imagetourl)
                                {

                                }

                                if (ismonitor)
                                {


                                    if (HandlerProperty.SProperty.Set_notification_admin)
                                    {
                                        _API.SendMessage(HandlerProperty.SProperty.admin, 1, ReplaceKeyword(null,
$@"{HandlerProperty.SProperty.Keyword} - {HandlerProperty.SProperty.Keyword_monitor}
QQ:{targetActive}
{imd.Exif_Make} {imd.Exif_Model} {imd.Exif_OriginalDateTime}
{imageguid}{location}
{address}"),
HandlerProperty.SProperty.admin, robotQQ, msgSubType);
                                    }

                                    if (HandlerProperty.SProperty.Set_notification_target)
                                    {
                                        _API.SendMessage(msgSrc, 1, ReplaceKeyword(null,
$@"{HandlerProperty.SProperty.Keyword} - {HandlerProperty.SProperty.Keyword_monitor}
{HandlerProperty.SProperty.Msg_notification}
{imd.Exif_Make} {imd.Exif_Model} {imd.Exif_OriginalDateTime}
{imageguid}{location}
{address}"),
msgSrc, robotQQ, msgSubType);
                                    }

                                    if (HandlerProperty.SProperty.Set_notification_delete)
                                    {
                                        if(HandlerProperty.robot == RobotType.CQ)
                                        {
                                            CQAPI.DeleteMessage(HandlerProperty.CQ_AuthCode, messageid);
                                        }
                                    }
                                }
                                else
                                {
                                    _API.SendMessage(msgSrc, msgType, ReplaceKeyword(msgSrc, 
$@"{imageguid}{location}
{imd.Exif_Make} {imd.Exif_Model} {imd.Exif_OriginalDateTime}
{HandlerProperty.SProperty.Msg_amap_address_ok}{address}"),
targetActive, robotQQ, msgSubType);
                                }

                            });

                        });


                }
            });
        }

    }
}
