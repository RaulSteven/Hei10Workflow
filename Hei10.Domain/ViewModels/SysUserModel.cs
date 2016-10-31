using Hei10.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.ViewModels;

namespace Hei10.Domain.ViewModels
{
    [Serializable]
    public class SysUserModel
    {
        #region properties
        public long UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string RealName { get; set; }

        public string HeadImg { get; set; }

        public string GId { get; set; }

        public List<long> RoleIdList { get; set; }

        public List<UserMenuModel> MenuList { get; set; }

        public UserMenuModel CurrentMenu { get; set; }

        public List<long> SysApartIdList { get; set; }

        public List<UserRole2Filter> UserRoleFilterList { get; set; }

        #endregion

        public SysUserModel(Users user,List<long> roleIdList,List<UserMenuModel> menuList,List<long> apartIdList,List<UserRole2Filter> userRoleFilterList)
        {
            UserId = user.Id;
            UserName = user.LoginName;
            HeadImg = user.HeadImg;
            GId = user.GId;
            RoleIdList = roleIdList;
            MenuList = menuList;
            SysApartIdList = apartIdList;
            RealName = user.RealName;
            UserRoleFilterList = userRoleFilterList;
        }

        public SysUserModel()
        {

        }

        public void FindCurrentMenu(string targetUrl)
        {
            if (string.IsNullOrEmpty(targetUrl))
            {
                return;
            }
            targetUrl = targetUrl.ToLower();
            //sysMenu的Url保存的时候自动转为小写
            CurrentMenu = MenuList.FirstOrDefault(m => !string.IsNullOrEmpty(m.Url)
                && m.Url.Contains(targetUrl));
        }
    }
}
