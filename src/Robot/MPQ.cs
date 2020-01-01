using RestSharp;
using Steganography;
using Steganography.Extension;
using Steganography.Property;
using Steganography.Robot.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImgMetadata;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace Steganography
{
    public class MPQ
    {
        static frmSet fs = new frmSet();
        static frmAbout fa = new frmAbout();

        //Disable
        [DllExport("Message", CallingConvention = CallingConvention.StdCall)]
        public static Int32 Message(string robotQQ, Int32 msgType, string msgRaw, string cookies, string SessionKey, string ClinetKey)
        {
            return 1;
        }

        [DllExport("info", CallingConvention = CallingConvention.StdCall)]
        public static string info()
        {
            HandlerProperty.robot = RobotType.MPQ;
            Config.LoadProperty();
            return "秘图密语";
        }

        /// <summary>
        /// Event定义
        ///<para>会话事件返回值定义:</para>
        ///<para>0 继续链表 1 执行完毕且继续链表  2执行完毕 阻断链表</para>
        ///<para>特别注意:</para>
        ///<para>在特殊事件(需要同意或拒绝的事件)中 返回值1 = 默认确定\同意  2=默认取消\拒绝</para>
        ///<para>因此 插件未处理或不需要的信息请返回0以免造成默认允许添加好友或入群</para>
        ///<para>会话信息事件</para>
        ///<para>1 好友信息;2 群信息;3 讨论组信息;4 临时会话信息</para>
        /// </summary>
        /// <param name="robotQQ"></param>
        /// <param name="msgType"></param>
        /// <param name="msgSubType"></param>
        /// <param name="msgSrc"></param>
        /// <param name="targetActive"></param>
        /// <param name="targetPassive"></param>
        /// <param name="msgContent"></param>
        /// <param name="msgRaw"></param>
        /// <param name="mPointer"></param>
        /// <returns>0</returns>
        [DllExport("EventFun", CallingConvention = CallingConvention.StdCall)]
        public static int EventFun(string robotQQ, Int32 msgType, Int32 msgSubType, string msgSrc, string targetActive, string targetPassive, string msgContent, string msgRaw, IntPtr mPointer)
        {
            Stego.Run(robotQQ, msgType, msgSubType, msgSrc, targetActive, targetPassive, msgContent, 0);

            return 0;
        }


    [DllExport("about", CallingConvention = CallingConvention.StdCall)]
        public static void about()
        {
            if (!fa.Visible)
            {
                fa.Show();
            }
            else
            {
                try
                {
                    fa.Activate();
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            MPQMessageAPI.Api_OutPut("启动关于页");
        }

        [DllExport("set", CallingConvention = CallingConvention.StdCall)]
        public static void set()
        {
            if (!fs.Visible)
            {
                fs.Show();
            }
            else
            {
                try
                {
                    fs.Activate();
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            MPQMessageAPI.Api_OutPut("启动设置页");
        }

        [DllExport("end", CallingConvention = CallingConvention.StdCall)]
        public static Int32 end()
        {
            return 0;
        }

    }
}
