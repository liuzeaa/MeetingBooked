using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingModel
{
    public enum SysStatus
    {
        正常 = 0,
        删除 = 1,
        归档 = 2
    }

    public enum Hot
    {
        热点 = 1,
        普通 = 0
    }

    public enum NewsType
    {
        通知公告 = 0,
        学校新闻 = 1,
        媒体报道 = 2,
        招聘信息 = 3
    }

    public enum AdvertisType
    {
        联系我们 = 0,
        网站简介 = 1,
        友情链接 = 2,
        学校简介 = 3,
        校长寄语 = 4,
        学校历史 = 6,
        招生信息 = 7,
        就业分配 = 8,
        教学环境 = 9,
        校园文化 = 10,
        鉴定培训 = 11,
        职业培训 = 12,
        网上报名 = 13,
        学校特色 = 14,
        荣誉资质 = 15,
        明星学员 = 16,
        联系学校 = 17
    }

    public enum PushTime
    {
        推送一次 = 0,
        每周推送一次 = 1
    }

    public enum AutoNotice
    {
        待批改作业 = 0,
        待批试卷 = 1,
        调查问卷 = 2,
        资源审核 = 3,
        学生报名 = 4,
        学生考试 = 5,
        学生任务 = 6,
        邮件消息 = 7,
        系统消息 = 8,
        发布作业 = 9,
        邮箱验证 = 10,
        找回密码=11
    }
    public enum MessageStatus
    {
        未读 = 0,
        已读 = 1
    }
    public enum isSend
    {
        未发送 = 0,
        已发送 = 1
    }
    public enum Display
    {
        显示 = 0,
        隐藏 = 1
    }
    public enum BeforeAfter
    {
        前台展示 = 0,
        后台展示 = 1,
        前后台展示 = 2
    }
    public enum RecordType
    {
        课程 = 0,
        知识点 = 1,
        资源 = 2,
        新闻通知 = 3,
        知识库 = 4,
        登录 = 5
    }

    public enum RequestUserType
    {
        内部学员 = 0,
        外部学员 = 1
    }

    public enum FusionChartType
    {
        None = 0,
        /// <summary>
        /// 2D柱状图
        /// </summary>
        MSColumn2D = 1,

        /// <summary>
        /// 仪表盘
        /// </summary>
        AngularGauge = 2,

        /// <summary>
        /// 2D饼图
        /// </summary>
        Pie2D = 3,

        /// <summary>
        /// 漏斗图
        /// </summary>
        Funnel = 4,

        /// <summary>
        /// 气泡图
        /// </summary>
        Bubble = 5,

        /// <summary>
        /// 趋势线图
        /// </summary>
        Line = 6,
        ///<summary>
        ///环形图
        ///</summary>
        Doughnut2D = 7,
        ///<summary>
        ///多Y轴图
        ///</summary>
        MSCombiDY2D = 8,
        /// <summary>
        /// 两线图
        /// </summary>
        Line2 = 9,
        /// <summary>
        /// 3D饼图
        /// </summary>
        Pie3D = 10,
    }

    public enum EnteredStatus
    {
        已申请 = 0,
        已审批 = 1
    }

    public enum MessageTiming
    {
        立即发送 = 0,
        定时发送 = 1
    }
    public enum VisitUserType
    {
        游客 = 0,
        普通用户 = 1
    }

    public enum enumTimeInterval 
    {
        今日=0,
        三天前=1,
        一周之内=2,
        一个月之内=3
    }

    public enum isPush 
    {
        发布=1,
        未发布=0
    }

    public enum isRegImg 
    {
        校园快讯=53
    }

    public enum ModelTemplete 
    {
        普通展示=0,
        Word展示=1
    }

}
