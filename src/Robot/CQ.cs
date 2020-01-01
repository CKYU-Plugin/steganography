using Steganography.Property;
using Steganography.Robot.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Steganography
{
    public class CQ
    {
        static string _ApiVer = "9";
        static string _AppID = "link.toroko.steganography";
        static int _AuthCode = 0;
        static frmSet fs = new frmSet();
        static frmAbout fa = new frmAbout();

        [DllExport("AppInfo", CallingConvention = CallingConvention.StdCall)]
        public static string appInfo()
        {
            return string.Concat(_ApiVer, ",", _AppID);
        }

        [DllExport("Initialize", CallingConvention = CallingConvention.StdCall)]
        public static Int32 Initialize(int AuthCode)
        {
            HandlerProperty.robot = RobotType.CQ;
            HandlerProperty.CQ_AuthCode = AuthCode;
            Config.LoadProperty();
            _AuthCode = HandlerProperty.CQ_AuthCode;
            return 0;
        }

        [DllExport("_eventStartup", CallingConvention = CallingConvention.StdCall)]
        public static Int32 AppStartup()
        {
            return 0;
        }

        [DllExport("_eventExit", CallingConvention = CallingConvention.StdCall)]
        public static Int32 AppExit()
        {
            return 0;
        }

        [DllExport("_eventEnable", CallingConvention = CallingConvention.StdCall)]
        public static Int32 AppEnable()
        {
            return 0;
        }

        [DllExport("_eventDisable", CallingConvention = CallingConvention.StdCall)]
        public static Int32 AppDisable()
        {
            return 0;
        }

        [DllExport("_eventPrivateMsg", CallingConvention = CallingConvention.StdCall)]
        public static Int32 PrivateMessage(int subType, int msgId, long fromQQ, string msg, int font)
        {
            Stego.Run(CQAPI.GetLoginQQ(HandlerProperty.CQ_AuthCode).ToString(), 1, subType, fromQQ.ToString(), fromQQ.ToString(), fromQQ.ToString(), msg, msgId);
            return 0;
        }

        [DllExport("_eventGroupMsg", CallingConvention = CallingConvention.StdCall)]
        public static Int32 GroupMessage(int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, string msg, int font)
        {
            Stego.Run(CQAPI.GetLoginQQ(HandlerProperty.CQ_AuthCode).ToString(), 2, subType, fromQQ.ToString(), fromGroup.ToString(), fromQQ.ToString(), msg, msgId);
            return 0;
        }

        [DllExport("_eventDiscussMsg", CallingConvention = CallingConvention.StdCall)]
        public static Int32 DiscussMessage(int subType, int sendTime, long fromDiscuss, long fromQQ, string msg, int font)
        {
            return 0;
        }

        [DllExport("_eventGroupUpload", CallingConvention = CallingConvention.StdCall)]
        public static Int32 GroupUpload(int subType, int sendTime, long fromGroup, long fromQQ, string file)
        {
            // 处理群文件上传事件。
            //  CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{1}你上传了一个文件：{2}", CQ.ProxyType, CQ.CQCode_At(fromQQ), file));
            return 0;
        }

        [DllExport("_eventSystem_GroupAdmin", CallingConvention = CallingConvention.StdCall)]
        public static Int32 GroupAdmin(int subType, int sendTime, long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-管理员变动。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]{2}({1})被{3}管理员权限。", CQ.ProxyType, beingOperateQQ, CQ.GetQQName(beingOperateQQ), subType == 1 ? "取消了" : "设置为"));
            return 0;
        }

        [DllExport("_eventSystem_GroupMemberDecrease", CallingConvention = CallingConvention.StdCall)]
        public static Int32 GroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员减少。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]群员{2}({1}){3}", CQ.ProxyType, beingOperateQQ, CQ.GetQQName(beingOperateQQ), subType == 1 ? "退群。" : String.Format("被{0}({1})踢除。", CQ.GetQQName(fromQQ), fromQQ)));
            return 0;
        }

        [DllExport("_eventSystem_GroupMemberIncrease", CallingConvention = CallingConvention.StdCall)]
        public static Int32 GroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员增加。
            //CQ.SendGroupMessage(fromGroup, String.Format("[{0}]群里来了新人{2}({1})，管理员{3}({4}){5}", CQ.ProxyType, beingOperateQQ, CQ.GetQQName(beingOperateQQ), CQ.GetQQName(fromQQ), fromQQ, subType == 1 ? "同意。" : "邀请。"));
            return 0;
        }

        [DllExport("_eventFriend_Add", CallingConvention = CallingConvention.StdCall)]
        public static Int32 FriendAdded(int subType, int sendTime, long fromQQ)
        {
            // 处理好友事件-好友已添加。
            // CQ.SendPrivateMessage(fromQQ, String.Format("[{0}]你好，我的朋友！", CQ.ProxyType));
            return 0;
        }

        [DllExport("_eventRequest_AddFriend", CallingConvention = CallingConvention.StdCall)]
        public static Int32 RequestAddFriend(int subType, int sendTime, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-好友添加。
            // CQ.SetFriendAddRequest(responseFlag, CQReactType.Allow, "新来的朋友");
            return 0;
        }

        [DllExport("_eventRequest_AddGroup", CallingConvention = CallingConvention.StdCall)]
        public static Int32 RequestAddGroup(int subType, int sendTime, long fromGroup, long fromQQ, string msg, string responseFlag)
        {
            // 处理请求-群添加。
            // CQ.SetGroupAddRequest(responseFlag, CQRequestType.GroupAdd, CQReactType.Allow, "新群友");
            return 0;
        }

        [DllExport("_menuA", CallingConvention = CallingConvention.Cdecl)]
        public static Int32 _menuA()
        {
            return 0;
        }

        [DllExport("_menuB", CallingConvention = CallingConvention.Cdecl)]
        public static Int32 _menuB()
        {
            return 0;
        }

        [DllExport("_set", CallingConvention = CallingConvention.Cdecl)]
        public static Int32 set()
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
        //  CQAPI.AddLog(_AuthCode, 1, 10, "启动设置页");
            return 0;
        }

        [DllExport("_about", CallingConvention = CallingConvention.Cdecl)]
        public static Int32 about()
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
          //  CQAPI.AddLog(_AuthCode, 1, 10, "启动关于页");
            return 0;
        }
    }
}
