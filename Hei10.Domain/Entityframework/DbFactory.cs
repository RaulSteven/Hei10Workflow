using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Models;

namespace Hei10.Domain.Entityframework
{
    public class DbFactory : IDbFactory,IDisposable
    {
        public Hei10Context Context
        {
            get;
            set;
        }

        public DbFactory()
        {
            Context = new Hei10Context();
        }

        public void Dispose()
        {
            if (Context == null)
            {
                return;
            }
            Context.Dispose();
            Context = null;
        }

        public Task<int> SaveChangesAsync()
        {
            return this.Context.SaveChangesAsync();
        }
        public static AdPosition CreateAdPosition()
        {
            var AdPosition = new AdPosition
            {
                CommonStatus = CommonStatus.Enabled
            };
            return AdPosition;
        }
        public static Advert CreateAdvert(long adPosId, AdPosKey code, string adPosName)
        {
            var advert = new Advert
            {
                AdPosId = adPosId,
                Code = code,
                StartTime = DateTime.Now,
                AdvertStatus = AdvertStatus.Draft,
                AdType = AdvertType.Img,
                AdPosName = adPosName,
                CommonStatus = CommonStatus.Enabled
            };
            return advert;
        }

        public static RecruitJob CreateRecruitJob()
        {
            var model = new RecruitJob
            {
                CommonStatus = CommonStatus.Enabled
            };
            return model;
        }

        public static UserRole2Filter CreateUserRole2Filter()
        {
            var model = new UserRole2Filter
            {
                CommonStatus = CommonStatus.Enabled
            };
            return model;
        }
    }


}
