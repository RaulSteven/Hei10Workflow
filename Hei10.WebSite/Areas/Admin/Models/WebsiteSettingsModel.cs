using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hei10.Core.Utilities;
using Hei10.Domain.Repositories;

namespace Hei10.WebSite.Areas.Admin.Models
{
    //基础信息编辑
    public class WebsiteSettingsModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        {
            this.WebSiteName = sysConfigRepository.WebSiteName;
            this.WebSiteUrl = sysConfigRepository.WebSiteUrl;
            this.WebSiteCopyRight = sysConfigRepository.WebSiteCopyRight;
            this.WebSiteICP = sysConfigRepository.WebSiteICP;
            this.CompanyName = sysConfigRepository.CompanyName;
            this.CompanyAddr = sysConfigRepository.CompanyAddr;
            this.CompanyTel = sysConfigRepository.CompanyTel;
            this.SitemapPath = sysConfigRepository.SitemapPath;
            this.LogFilePath = sysConfigRepository.LogFilePath;
        }

        public void Save(ISysConfigRepository sysConfigRepository)
        {
            sysConfigRepository.WebSiteName = this.WebSiteName;
            sysConfigRepository.WebSiteUrl = this.WebSiteUrl;
            sysConfigRepository.WebSiteICP = this.WebSiteICP;
            sysConfigRepository.WebSiteCopyRight = this.WebSiteCopyRight;
            sysConfigRepository.CompanyName = this.CompanyName;
            sysConfigRepository.CompanyAddr = this.CompanyAddr;
            sysConfigRepository.CompanyTel = this.CompanyTel;
            sysConfigRepository.SitemapPath = this.SitemapPath;
            sysConfigRepository.LogFilePath = this.LogFilePath;
        }
        [Display(Name = "网站名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string WebSiteName { get; set; }

        [Display(Name = "网址地址")]
        [Required(ErrorMessage = "请输入{0}")]
        public string WebSiteUrl { get; set; }

        [Display(Name = "网站版权文字")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}的长度不能超过{1}个字")]
        public string WebSiteCopyRight { get; set; }

        [Display(Name = "ICP备案号")]
        [Required(ErrorMessage = "请输入{0}")]
        public string WebSiteICP { get; set; }

        [Display(Name = "公司名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}的长度不能超过{1}个字")]
        public string CompanyName { get; set; }

        [Display(Name = "公司地址")]
        [Required(ErrorMessage = "请输入{0}")]
        public string CompanyAddr { get; set; }

        [Display(Name = "公司电话")]
        [Required(ErrorMessage = "请输入{0}")]
        public string CompanyTel { get; set; }

        [Display(Name = "Sitemap路径")]
        [Required(ErrorMessage = "请输入{0}")]
        public string SitemapPath { get; set; }

        [Display(Name = "日志记录路径")]
        [Required(ErrorMessage = "请输入{0}")]
        public string LogFilePath { get; set; }
    }

    public class TongJiSettingsModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        {
            this.StatisticsJS = sysConfigRepository.StatisticsJS;
        }
        public void Save(ISysConfigRepository sysConfigRepository)
        {
            sysConfigRepository.StatisticsJS = this.StatisticsJS;
        }

        [Display(Name = "统计脚本")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(3000, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string StatisticsJS { get; set; }
    }

    public class UploadModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        {
            this.UploadFileSizes = sysConfigRepository.UploadFileSizes;
            this.UploadFileTypes = sysConfigRepository.UploadFileTypes == null ? "" : string.Join("|", sysConfigRepository.UploadFileTypes);
            this.UploadPath = sysConfigRepository.UploadPath;
            this.ImgSites = sysConfigRepository.ImgSites == null ? "" : string.Join("|", sysConfigRepository.ImgSites);
            this.WaterMarkingPath = sysConfigRepository.WaterMarkingPath;
        }

        public void Save(ISysConfigRepository sysConfigRepository)
        {
            sysConfigRepository.UploadFileSizes = this.UploadFileSizes;
            sysConfigRepository.UploadFileTypes = this.UploadFileTypes.Split('|');
            sysConfigRepository.UploadPath = this.UploadPath;
            sysConfigRepository.ImgSites = this.ImgSites.Split('|');
            sysConfigRepository.WaterMarkingPath = WaterMarkingPath;
        }

        [Display(Name = "上传路径")]
        [Required(ErrorMessage = "请输入{0}")]
        public string UploadPath { get; set; }

        [Display(Name = "上传文件类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public string UploadFileTypes { get; set; }

        [Display(Name = "图片服务器")]
        [Required(ErrorMessage = "请输入{0}")]
        public string ImgSites { get; set; }

        [Display(Name = "上传文件大小限制")]
        [Required(ErrorMessage = "请输入{0}")]
        [RegularExpression(RegexUtility.PositiveInt, ErrorMessage = "{0}输入不正确")]
        public int UploadFileSizes { get; set; }

        [Display(Name = "水印图片路劲")]
        [Required(ErrorMessage = "请输入{0}")]
        public string WaterMarkingPath { get; set; }
    }

    public class IconsModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        {
            this.Icons = sysConfigRepository.Icons == null ? "" : string.Join(",", sysConfigRepository.Icons);
        }

        public void Save(ISysConfigRepository sysConfigRepository)
        {
            sysConfigRepository.Icons = this.Icons.Split(',');
        }

        [Required(ErrorMessage = "请输入Icons")]
        public string Icons { get; set; }
    }
    public class StaffPicModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        {
            this.StaffPic = sysConfigRepository.StaffPic == null ? "" : string.Join("|", sysConfigRepository.StaffPic);
        }

        public void Save(ISysConfigRepository sysConfigRepository)
        {
            sysConfigRepository.StaffPic = this.StaffPic.Split('|');
        }

        [Required(ErrorMessage = "请输入员工图片")]
        public string StaffPic { get; set; }
    }

    public class ArticleClassifySetModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        { 
            CaseClassifyId = sysConfigRepository.CaseClassifyId;
        }

        public void Save(ISysConfigRepository sysConfigRepository)
        { 
            sysConfigRepository.CaseClassifyId = CaseClassifyId;
        } 

        [Display(Name = "案例分类Id")]
        [Required(ErrorMessage = "请输入{0}")]
        [RegularExpression(RegexUtility.PositiveInt, ErrorMessage = "请输入{0}")]
        public long CaseClassifyId { get; set; }
    }

    public class ArticleSearchSetModel
    {
        public void Init(ISysConfigRepository sysConfigRepository)
        {
            SeachClassifyId = sysConfigRepository.SeachClassifyId;
        }

        public void Save(ISysConfigRepository sysConfigRepository)
        {
            sysConfigRepository.SeachClassifyId = SeachClassifyId;
        }

        [Display(Name = "文章搜索分类Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public string SeachClassifyId { get; set; }
    }
}