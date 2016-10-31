using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hei10.Domain.Models;
using AutoMapper;
using Hei10.Domain.ViewModels;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.WebSite.Models;

namespace Hei10.WebSite
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SysApartment, SysApartModel>();
                cfg.CreateMap<SysApartModel, SysApartment>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateIP, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateTime, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserId, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserName, opt => opt.Ignore());

                cfg.CreateMap<Users, AdminModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                cfg.CreateMap<AdminModel, Users>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

                cfg.CreateMap<ArticleClassify, ArticleClassifyModel>(); 
                cfg.CreateMap<ArticleClassifyModel, ArticleClassify>();

                cfg.CreateMap<Article, ArticleModel>();
                cfg.CreateMap<ArticleModel, Article>();

                cfg.CreateMap<SysMenu, SysMenuModel>();
                cfg.CreateMap<SysMenuModel, SysMenu>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateIP, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateTime, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserId, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserName, opt => opt.Ignore());

                cfg.CreateMap<UserRole, UserRoleModel>();
                cfg.CreateMap<UserRoleModel, UserRole>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateIP, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateTime, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserId, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserName, opt => opt.Ignore());
                
                cfg.CreateMap<FrontMenu, FrontMenuModel>();
                cfg.CreateMap<FrontMenuModel, FrontMenu>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateIP, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateTime, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserId, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserName, opt => opt.Ignore());

                cfg.CreateMap<Partner, PartnerModel>();
                cfg.CreateMap<PartnerModel, Partner>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateIP, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateTime, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserId, opt => opt.Ignore())
                   .ForMember(dest => dest.CreateUserName, opt => opt.Ignore());

                cfg.CreateMap<ConsultClassify, ConsultClassifyModel>();
                cfg.CreateMap<ConsultClassifyModel, ConsultClassify>();

                cfg.CreateMap<Consult, ConsultModel>();
                cfg.CreateMap<ConsultModel, Consult>();

                cfg.CreateMap<LeaveInfo, LeaveInfoModel>();
                cfg.CreateMap<LeaveInfoModel, LeaveInfo>()
                   .ForMember(dest=>dest.Id,opt=>opt.Ignore());

                cfg.CreateMap<SealInfo, SealInfoModel>();
                cfg.CreateMap<SealInfoModel, SealInfo>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore());

                cfg.CreateMap<MeetingRoomInfo, MeetingRoomInfoModel>();
                cfg.CreateMap<MeetingRoomInfoModel, MeetingRoomInfo>()
                   .ForMember(dest => dest.Id, opt => opt.Ignore());
            });
        }
    }
}