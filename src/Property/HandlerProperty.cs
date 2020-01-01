using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Property
{
    public class HandlerProperty
    {
        public static SetProperty SProperty = new SetProperty();
        public static RobotType robot = RobotType.MPQ;
        public static int CQ_AuthCode = 0;
    }

    public enum RobotType : int
    {
        Test = -1,
        CQ = 0,
        MPQ = 1,
        Amanda = 2,
        IRQQ = 3,
        Huajing = 4,
        QY = 5
    }

    public  class SetProperty
    {
        public string admin { get; set; } = "";
        public int version { get; set; } = 0;
        public bool imagetourl = false;

        public bool isActiveResponseHeader { get; set; } = false;
        public bool isActiveResponseFooter { get; set; } = false;
        public bool isAtMe { get; set; } = false;

        public string Keyword { get; set; } = "秘图密语";
        public string Keyword_end { get; set; } = "关秘图密语";
        public string Keyword_encode { get; set; } = "加密";
        public string Keyword_decode { get; set; } = "解密";
        public string Keyword_metadata { get; set; } = "查坐标";
        public string Keyword_set { get; set; } = "设定";
        public string Keyword_refresh { get; set; } = "刷新时间";
        public string Keyword_quota { get; set; } = "限额";
        public string Keyword_sensitiveword { get; set; } = "敏感词筛查";
        public string Keyword_monitor { get; set; } = "监控与通知";
        public string Keyword_notification_admin { get; set; } = "通知主人";
        public string Keyword_notification_target { get; set; } = "通知目标";
        public string Keyword_notification_delete { get; set; } = "自动撒消";
        public string Keyword_statistics { get; set; } = "统计";

        public string Msg_metadataMsg { get; set; } = "请发送图片進行查询";
        public string Msg_metadataErrMsg { get; set; } = "查坐标失败";

        public string Msg_encodeMsg { get; set; } = "请发送图片進行加密";
        public string Msg_encodeMsg2 { get; set; } = "请发送需要加密在图片里的內容";
        public string Msg_encodeMsg3 { get; set; } = "请设定密码";
        public string Msg_encodeMsg4 { get; set; } = "是否支持备份解密(是/否)";
        public string Msg_encodeMsg5 { get; set; } = "正在尝试加密图片…";
        public string Msg_encodeMsgOut { get; set; } = "这是已加密图片:\n";
        public string Msg_encodeErrMsg { get; set; } = "加密图片失败";

        public string Msg_decodeMsg { get; set; } = "请发送图片進行解密";
        public string Msg_decodeMsg2 { get; set; } = "请提供解密密码";
        public string Msg_decodeMsg3 { get; set; } = "正在尝试解密图片…";
        public string Msg_decodeMsgOut { get; set; } = "这是图片加密內容:\n";
        public string Msg_decodeMsgOut2 { get; set; } = "找到备份,这是备份的內容:\n";

        public string Msg_statisticsErrMsg { get; set; } = "找到到该图片的{统计}资料";

        public string Msg_decodeErrMsg { get; set; } =
@"解密图片失败,这可能是没有加密的图片/密码错误";
        public string Msg_decodeErrMsg1 { get; set; } =
@"提供的密码解密失败,尝试以备份进行解密失败";

        public string Msg_over { get; set; } = "超出使用限额,请{刷新时间G}分钟后再试";
        public string Msg_start { get; set; } =
@"{秘图密语}
-------------
{加密} 加密图片
{解密} 解密图片
{查坐标} 查图片坐标
{关秘图密语} 关闭{秘图密语}
{统计} {统计}资料
{设定} 设定{秘图密语}";
        public string Msg_set { get; set; } =
@"
{秘图密语}-{设定}
-------------
{刷新时间} 设定刷新时间(现在{刷新时间V}分钟)
{限额}  设定限额(现在{限额V}次)
{敏感词筛查} {!敏感词筛查S}敏感词筛查
{监控} {!监控S}监控通知
{通知主人} {!通知主人S}通知主人
{通知目标} {!通知目标S}通知目标
{撒消} {!撒消S}自動撒消
";
        public string Msg_setErrMsg { get; set; } =
@"
你不是主人({主人})
";

        public string Msg_monitor_onoff = "已{监控S}监控通知";
        public string Msg_notification_admin_onoff = "已{通知主人S}通知主人";
        public string Msg_notification_target_onoff = "已{通知目标S}通知目标";
        public string Msg_notification_delete_onoff = "已{撒消S}自動撒消";

        public string Msg_startfirst { get; set; } = "请先开启{秘图密语}";
        public string Msg_end { get; set; } = "已关闭{秘图密语}";

        public string Msg_refresh { get; set; } = "请设定刷新时间:(数字)分钟";
        public string Msg_quota { get; set; } = "请设定限额(数字)";

        public string Msg_refresh_ok { get; set; } = "已设定刷新时间:{刷新时间V}分钟";
        public string Msg_quota_ok { get; set; } = "已设定限额:{限额V}";
        public string Msg_sensitivewords_ok { get; set; } = "已{敏感词筛查S}{敏感词筛查}";
        public string Msg_monitor_ok { get; set; } = "已{监控与通知S}{监控与通知}";
        public string Msg_amap_address_ok { get; set; } = "地址:";
        public string Msg_amap_address_err { get; set; } = "找不到地址";

        public string Msg_notification { get; set; } = "你发出的图片包含隐私信息;";
        public int Set_Quota { get; set; } = 10;
        public int Set_Refresh { get; set; } = 10;
        public bool Set_sensitiveword { get; set; } = true;
        public bool Set_monitor { get; set; } = true;
        public bool Set_notification_admin { get; set; } = true;
        public bool Set_notification_target { get; set; } = true;
        public bool Set_notification_delete { get; set; } = true;

        public string Response { get; set; }= @"";
        public string ResponseErrorMessage { get; set; } = "查询失败";
        public string UrlErrorMessage { get; set; } = "图片获取失败";
        public string UrlErrorMessage2 { get; set; } = "下载图片失败";


    }
}
