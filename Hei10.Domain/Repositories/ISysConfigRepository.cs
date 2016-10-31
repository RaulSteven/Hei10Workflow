using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;

namespace Hei10.Domain.Repositories
{
    public interface ISysConfigRepository : IRepository<SysConfig>
    {
        #region 网站配置
        string WebSiteName { get; set; }
        string WebSiteCopyRight { get; set; }
        string WebSiteICP { get; set; }
        string WebSiteUrl { get; set; }

        string SitemapPath { get; set; }

        //公司名称"
        string CompanyName { get; set; }

        // 公司地址
        string CompanyAddr { get; set; }

        //公司电话"
        string CompanyTel { get; set; }

        /// <summary>
        /// 日志记录路径
        /// </summary>
        string LogFilePath { get; set; }
        #endregion

        #region 文件上传配置
        string UploadPath { get; set; }
        string[] UploadFileTypes { get; set; }
        int UploadFileSizes { get; set; }

        /// <summary>
        /// 水印图片位置
        /// </summary>
        string WaterMarkingPath { get; set; }

        #endregion

        #region 缩略图设置
        string[] ImgSites { get; set; }
        #endregion

        #region 统计
        string StatisticsJS { get; set; }
        #endregion

        #region icon 列表
        string[] Icons { get; set; }

        #endregion

        #region 员工照片

        string[] StaffPic { get; set; }
        #endregion

        #region 首页文章分类配置 
        long CaseClassifyId { get; set; }

        #endregion

        #region 文章搜索分类配置
        string SeachClassifyId { get; set; }
        #endregion
    }
}
