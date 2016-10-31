using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hei10.Core.Cache;
using Hei10.Core.Utilities;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using log4net;
using Hei10.Domain.Entityframework;

namespace Hei10.Domain.Repositories
{
    public class SysConfigRepository : Repository<SysConfig>, ISysConfigRepository
    {
        public ICacheManager Cache { get; set; }

        public SysConfigRepository(IDbFactory factory,ICacheManager cache)
        {
            Factory = factory;
            Cache = cache;
        }

        #region constraints

        public const string CacheKey = "JR.Config.";

        public const string ConfigPatternByKey = CacheKey + "ByCode.{0}";

        public const int ConfigCacheTimeOut = 600;

        #endregion

        public Task<SysConfig> GetByKeyAsync(string key)
        {
            return QueryUnDelete()
                .FirstOrDefaultAsync(c => c.ConKey == key);
        }

        private T GetConfig<T>(MethodBase method)
        {
            try
            {
                var configKey = method.Name;
                if (configKey.StartsWith("get_"))
                {
                    configKey = configKey.Substring(4).Trim();
                }
                else
                {
                    throw new Exception("GetConfig 方法只能在get属性中调用！");
                }
                var cacheKey = string.Format(ConfigPatternByKey, configKey);
                var result = Cache.Get<T>(cacheKey, CacheKey, () =>
                {
                    var config = GetByKeyAsync(configKey).Result;
                    if (config == null)
                    {
                        throw new NullReferenceException(string.Format("找不到{0}的配置！", configKey));
                    }
                    var cacheResult = XmlUtility.XmlDeserialize<T>(config.ConValue);
                    return cacheResult;
                });
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        private void SetConfig<T>(MethodBase method, T value)
        {
            try
            {
                if (null == value)
                {
                    throw new ArgumentNullException("SetConfig 设置的值不能为空！");
                }
                var configKey = method.Name;
                if (configKey.StartsWith("set_"))
                {
                    configKey = configKey.Substring(4).Trim();
                }
                else
                {
                    throw new Exception("SetConfig 方法只能在set属性中调用！");
                }
                var config = GetByKeyAsync(configKey).Result;
                if (config == null)
                {
                    config = new SysConfig()
                    {
                        ConKey = configKey,
                        CreateTime = DateTime.Now,
                        CreateUserId = User.UserInfo.UserId,
                        CreateUserName = User.UserInfo.UserName
                    };
                    base.Add(config);
                }
                config.UpdateTime = DateTime.Now;
                config.UpdateIP = (new IPUtility()).GetIPAndCity();
                config.ConValue = XmlUtility.XmlSerialize(value);
                SaveChangesAsync().Wait();
                var cacheKey = string.Format(ConfigPatternByKey, configKey);
                Cache.Remove(cacheKey, CacheKey);
                Cache.Remove(cacheKey + "Id", CacheKey);
            }
            catch (Exception ex)
            {
                var log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                log.ErrorFormat("Set Config error:{0}", ex);
            }
        }
        
        #region 网站配置

        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebSiteName
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        /// <summary>
        /// 网站CopyRight
        /// </summary>
        public string WebSiteCopyRight
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        public string WebSiteICP
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }


        public string WebSiteUrl
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        public string SitemapPath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string CompanyAddr
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string CompanyTel
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        /// <summary>
        /// 日志记录路径
        /// </summary>
        public string LogFilePath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }
        #endregion

        #region 文件上传配置
        public string UploadPath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        public string[] UploadFileTypes
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        public int UploadFileSizes
        {
            get
            {
                return GetConfig<int>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        public string WaterMarkingPath
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        #endregion

        #region 缩略图设置
        
        public string[] ImgSites
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        #endregion

        #region 统计
        public string StatisticsJS
        {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }
        #endregion

        #region icon 列表

        public string[] Icons
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        #endregion

        #region 员工照片

        public string[] StaffPic
        {
            get
            {
                return GetConfig<string[]>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }

        #endregion

        #region 首页文章分类配置 
        public long CaseClassifyId
        {
            get
            {
                return GetConfig<long>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }
        #endregion

        #region 文章搜索分类配置
        public string SeachClassifyId {
            get
            {
                return GetConfig<string>(MethodBase.GetCurrentMethod());
            }
            set
            {
                SetConfig(MethodBase.GetCurrentMethod(), value);
            }
        }
        #endregion
    }
}
