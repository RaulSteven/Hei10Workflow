using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Repositories;
namespace Hei10.Domain.Services
{
    public class SysApartmenSvc : ISysApartmenSvc
    {
        public ISysApartmentRepository SysApartRepository { get; set; }
        public IUser2ApartmentRepository User2ApartRepository { get; set; }


        public async Task<List<SysApartJsonModel>> GetJsonListAsync(long userId)
        {
            var apartList = await SysApartRepository.GetJsonList();

            var apartIdList = await User2ApartRepository.GetApartIdListAsync(userId);

            SetJsonChecked(apartIdList, apartList);

            return apartList;
        }

        private void SetJsonChecked(List<long> apartIdList, List<SysApartJsonModel> apartList)
        {
            if (apartList == null
                || apartList.Count == 0
                || apartIdList == null
                || apartIdList.Count == 0)
            {
                return;
            }
            foreach (var apart in apartList)
            {
                apart.@checked = apartIdList.Any(m => m == apart.id).ToString().ToLower();
                SetJsonChecked(apartIdList, apart.children);
            }
        }
    }
}
