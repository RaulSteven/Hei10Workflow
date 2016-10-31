namespace Hei10.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StevenInitDb1031 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdPositions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Code = c.Int(nullable: false),
                        ViewName = c.String(nullable: false, maxLength: 255),
                        Descript = c.String(),
                        ImgUrl = c.String(maxLength: 255),
                        Size = c.String(maxLength: 50),
                        Sort = c.Int(nullable: false),
                        LinkUrl = c.String(maxLength: 255),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        AdPosId = c.Long(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                        AdGroup = c.Short(nullable: false),
                        Seat = c.Short(nullable: false),
                        AdvertStatus = c.Int(nullable: false),
                        AdType = c.Int(nullable: false),
                        Target = c.Int(nullable: false),
                        MetaContent = c.String(),
                        LinkUrl = c.String(nullable: false, maxLength: 255),
                        Size = c.String(maxLength: 50),
                        TextContent = c.String(maxLength: 255),
                        ImgUrl = c.String(maxLength: 500),
                        SortIndex = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClassifyId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        TitleSub = c.String(maxLength: 255),
                        Author = c.String(maxLength: 50),
                        Brief = c.String(maxLength: 500),
                        ArticleContent = c.String(nullable: false),
                        ArticleDateTime = c.DateTime(nullable: false),
                        Tags = c.String(maxLength: 255),
                        Source = c.String(maxLength: 200),
                        SourceLink = c.String(maxLength: 255),
                        SortIndex = c.Int(nullable: false),
                        Pic = c.String(maxLength: 255),
                        ViewCount = c.Int(nullable: false),
                        PartialViewCode = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleClassifies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 200),
                        SortIndex = c.Int(nullable: false),
                        ChildrenCount = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        TreePath = c.String(nullable: false, maxLength: 255),
                        PartialViewCode = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FileSize = c.Long(nullable: false),
                        FileExt = c.String(nullable: false, maxLength: 20),
                        FilePath = c.String(nullable: false, maxLength: 255),
                        SortIndex = c.Int(nullable: false),
                        Descript = c.String(maxLength: 255),
                        Source = c.Int(nullable: false),
                        SourceId = c.Long(nullable: false),
                        ViewCount = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Consults",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClassifyId = c.Long(nullable: false),
                        Title = c.String(maxLength: 500),
                        Contents = c.String(),
                        UserName = c.String(maxLength: 250),
                        Phone = c.String(maxLength: 250),
                        Email = c.String(maxLength: 500),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsultClassifies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(maxLength: 500),
                        Contents = c.String(maxLength: 1000),
                        Sort = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FrontMenus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Pid = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Remark = c.String(maxLength: 250),
                        Url = c.String(maxLength: 250),
                        IconSmall = c.String(maxLength: 250),
                        IconMiddle = c.String(maxLength: 250),
                        IconLarge = c.String(maxLength: 250),
                        Sort = c.Int(nullable: false),
                        TreePath = c.String(maxLength: 100),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeaveInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Days = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        LeaveType = c.String(nullable: false, maxLength: 10),
                        Remark = c.String(maxLength: 250),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MeetingRoomInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        UsedTime = c.DateTime(nullable: false),
                        ApplyTime = c.DateTime(nullable: false),
                        ApplyName = c.String(nullable: false, maxLength: 20),
                        Remark = c.String(maxLength: 250),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Url = c.String(nullable: false, maxLength: 250),
                        LogoUrl = c.String(maxLength: 250),
                        Sort = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecruitJobs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        WorkDuty = c.String(),
                        Requirement = c.String(),
                        Sort = c.Int(nullable: false),
                        Contents = c.String(),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SealInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        ApplyTime = c.DateTime(nullable: false),
                        ApplyUserName = c.String(maxLength: 250),
                        FileExplain = c.String(maxLength: 1000),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysApartments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Pid = c.Long(nullable: false),
                        Sort = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 250),
                        TreePath = c.String(maxLength: 100),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConKey = c.String(nullable: false, maxLength: 50),
                        ConValue = c.String(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysMenus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Pid = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Remark = c.String(maxLength: 250),
                        Url = c.String(maxLength: 250),
                        Icon = c.String(maxLength: 20),
                        Sort = c.Int(nullable: false),
                        Source = c.String(maxLength: 20),
                        TreePath = c.String(maxLength: 100),
                        Buttons = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SysOperationLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LogCat = c.Int(nullable: false),
                        LogType = c.Int(nullable: false),
                        LogTitle = c.String(nullable: false, maxLength: 200),
                        LogDesc = c.String(),
                        DataSource = c.String(maxLength: 20),
                        DataSouceId = c.String(maxLength: 50),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User2Apartment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ApartmentId = c.Long(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User2Role",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Remark = c.String(maxLength: 250),
                        Sort = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole2Apartment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        ApartmentId = c.Long(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole2Filter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                        Source = c.String(maxLength: 20),
                        FilterGroups = c.String(),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole2Menu",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        MenuId = c.Long(nullable: false),
                        DisallowField = c.String(maxLength: 250),
                        FilterGroups = c.String(),
                        Buttons = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LoginName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 250),
                        PasswordSalt = c.String(nullable: false, maxLength: 250),
                        RealName = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 250),
                        LoginCount = c.Int(nullable: false),
                        HeadImg = c.String(maxLength: 250),
                        Gender = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WfActivityInstances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcessInstanceId = c.Long(nullable: false),
                        ProcessId = c.Long(nullable: false),
                        ProcessName = c.String(),
                        PreActInstanceId = c.Long(nullable: false),
                        ActivityGuid = c.String(nullable: false, maxLength: 60),
                        ActivityName = c.String(nullable: false, maxLength: 50),
                        ActivityType = c.Int(nullable: false),
                        ActivityState = c.Int(nullable: false),
                        BackActivityInstanceId = c.Long(),
                        AssignToUserIds = c.String(),
                        AssignToUserNames = c.String(),
                        DealUserId = c.Long(nullable: false),
                        DealUserName = c.String(),
                        DealTime = c.DateTime(),
                        Comment = c.String(maxLength: 250),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WfProcesses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 250),
                        ProcessContent = c.String(),
                        TableSource = c.Int(nullable: false),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WfProcessInstances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WfProcessId = c.Long(nullable: false),
                        ProcessName = c.String(nullable: false, maxLength: 50),
                        ProcessState = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        CurrActivityName = c.String(maxLength: 50),
                        TableSource = c.Int(nullable: false),
                        SourceId = c.Long(nullable: false),
                        Conditions = c.String(maxLength: 250),
                        CommonStatus = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        CreateUserName = c.String(maxLength: 50),
                        CreateUserId = c.Long(nullable: false),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateUserId = c.Long(nullable: false),
                        CreateIP = c.String(maxLength: 50),
                        UpdateIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WfProcessInstances");
            DropTable("dbo.WfProcesses");
            DropTable("dbo.WfActivityInstances");
            DropTable("dbo.Users");
            DropTable("dbo.UserRole2Menu");
            DropTable("dbo.UserRole2Filter");
            DropTable("dbo.UserRole2Apartment");
            DropTable("dbo.UserRoles");
            DropTable("dbo.User2Role");
            DropTable("dbo.User2Apartment");
            DropTable("dbo.SysOperationLogs");
            DropTable("dbo.SysMenus");
            DropTable("dbo.SysConfigs");
            DropTable("dbo.SysApartments");
            DropTable("dbo.SealInfoes");
            DropTable("dbo.RecruitJobs");
            DropTable("dbo.Partners");
            DropTable("dbo.MeetingRoomInfoes");
            DropTable("dbo.LeaveInfoes");
            DropTable("dbo.FrontMenus");
            DropTable("dbo.ConsultClassifies");
            DropTable("dbo.Consults");
            DropTable("dbo.Attachments");
            DropTable("dbo.ArticleClassifies");
            DropTable("dbo.Articles");
            DropTable("dbo.Adverts");
            DropTable("dbo.AdPositions");
        }
    }
}
