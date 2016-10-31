using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Enums
{
    public enum TableSource
    {
        [Description("默认")]
        None = 0,
        [Description("用户")]
        Users = 1,
        [Description("配置")]
        SysConfigures = 2,
        [Description("部门")]
        SysApartments = 3,
        [Description("按钮")]
        SysButtons = 4,
        [Description("附件")]
        Attachments = 5,
        [Description("菜单")]
        SysMenus = 6,
        [Description("文章分类")]
        ArticleClassifies = 7,
        [Description("角色")]
        UserRole = 8,
        [Description("文章")]
        Articles = 9,
        [Description("工作流按钮")]
        WorkFlowButton = 10,
        [Description("广告位")]
        AdPositions = 11,
        [Description("广告")]
        Adverts = 12,
        [Description("数据库连接")]
        DbConnection = 13,
        [Description("前台菜单")]
        FrontMenu = 14,
        [Description("招聘职位")]
        RecruitJob = 15,
        [Description("咨询")]
        Consults = 16,
        [Description("合作伙伴")]
        Partner = 17,
        [Description("咨询分类")]
        ConsultClassifies = 18,
        [Description("请假信息")]
        LeaveInfo = 19,
        [Description("角色数据规则")]
        UserRole2Filter = 20,
        [Description("盖章申请")]
        SealInfo = 21,
        [Description("会议室使用申请")]
        MeetingRoomInfo = 22,
    }
}
