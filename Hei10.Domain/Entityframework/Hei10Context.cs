using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Models;
using Hei10.Domain.Entityframework.ModelConfigurations;
using Hei10.Domain.Models.Workflow;

namespace Hei10.Domain.Entityframework
{
    public class Hei10Context : DbContext
    {
        public Hei10Context() : base("name=Hei10Context") { }

        #region 用户
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public virtual DbSet<User2Apartment> User2Apartment { get; set; }
        public virtual DbSet<User2Role> User2Role { get; set; }
        public virtual DbSet<UserRole2Apartment> UserRole2Apartment { get; set; }
        public virtual DbSet<UserRole2Menu> UserRole2Menu { get; set; }
        public virtual DbSet<UserRole2Filter> UserRole2Filter { get; set; }

        #endregion

        #region 系统
        public virtual DbSet<SysApartment> SysApartment { get; set; }
        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<SysOperationLog> SysOperationLog { get; set; }
        public virtual DbSet<SysConfig> SysConfig { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        #endregion

        #region 文章

        public virtual DbSet<ArticleClassify> ArticleClassify { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        #endregion

        #region 广告
        public virtual DbSet<AdPosition> AdPosition { get; set; }
        public virtual DbSet<Advert> Advert { get; set; }

        #endregion

        #region 前台菜单
        public virtual DbSet<FrontMenu> FrontMenu { get; set; }

        #endregion
        #region 招聘工作
        public virtual DbSet<RecruitJob> RecruitJob { get; set; }

        #endregion
        #region 咨询

        public virtual DbSet<Consult> Consult { get; set; }
        public virtual DbSet<ConsultClassify> ConsultClassify { get; set; }
        #endregion
        #region 合作伙伴
        public virtual DbSet<Partner> Partner { get; set; }

        #endregion
        #region 工作流
        public virtual DbSet<WfProcess> WfProcess { get; set; }
        public virtual DbSet<WfActivityInstance> WfActivityInstance { get; set; }
        public virtual DbSet<WfProcessInstance> WfProcessInstance { get; set; }
        #endregion

        #region 请假
        public virtual DbSet<LeaveInfo> LeaveInfoes { get; set; }
        #endregion

        #region 盖章
        public virtual DbSet<SealInfo> SealInfo { get; set; }
        #endregion

        #region 会议室
        public virtual DbSet<MeetingRoomInfo> MeetingRoomInfo { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
               .Configurations
               .Add(new WfActivityInstanceConfiguration())
               .Add(new WfProcessInstanceConfiguration());
        }
    }
}
