using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Hei10.Domain.Repositories;
using Hei10.Web.Framework.Controllers;
using System.Threading.Tasks;
using Hei10.Domain.Models;
using Hei10.WebSite.Areas.Admin.Models;
using AutoMapper;
using Hei10.Core.Extensions;
using Hei10.Domain.ViewModels;
using Hei10.Domain.Enums;
using Newtonsoft.Json;
using Hei10.Domain.Services;
using Hei10.Core.Utilities;
using Hei10.Domain.Entityframework;
using Hei10.Web.Framework.Security;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class UserRoleController : AdminController
    {
        public IUserRoleRepository UserRoleRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        public ISysMenuRepository SysMenuRepository { get; set; }

        public IUserRole2MenuRepository UserRole2MenuRepository { get; set; }

        public ISysMenuSvc SysMenuSvc { get; set; }

        public IUserRoleSvc UserRoleSvc { get; set; }

        public IUserRole2FilterRepository UserRole2FilterRepository { get; set; }

        [ValidatePage]
        public ActionResult Index(int pageSize = 30, int pageCurrent = 1)
        {
            var list = UserRoleRepository.GetList(pageSize, pageCurrent);
            return PartialView(list);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        public async Task<ActionResult> Edit(long id = 0)
        {
            var model = new UserRoleModel();
            if (id == 0)
            {
                return PartialView(model);
            }
            var role = await UserRoleRepository.GetEnableByIdAsync(id);
            if (role == null)
            {
                var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            Mapper.Map(role, model);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRoleModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            UserRole role = null;
            var operationType = OperationType.Update;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                role = await UserRoleRepository.GetEnableByIdAsync(model.Id);
                if (role == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                role = new UserRole();
            }
            Mapper.Map(model, role);
            role.CommonStatus = CommonStatus.Enabled;
            await UserRoleRepository.SaveAsync(role);
            await LogRepository.Insert(TableSource.UserRole, operationType, "", role.Id);
            result.Data = role;
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Delete, ActionName = "Index")]
        public async Task<ActionResult> Delete(long id)
        {
            await UserRoleRepository.Delete(id);
            var jsonModel = new JsonModel();
            jsonModel.message = "删除成功！";
            jsonModel.closeCurrent = false;
            return Json(jsonModel);
        }

        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetMenus(long id)
        {
            ViewBag.RoleId = id;
            var data = await SysMenuSvc.GetJsonListAsync(id);
            return PartialView("SetMenus", JsonConvert.SerializeObject(data));
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetMenus(long id, string menuIds)
        {
            var result = new JsonModel();
            await UserRoleSvc.SaveListAsync(id, menuIds);
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetButtons(long roleId)
        {
            ViewBag.RoleId = roleId;
            var model = await SysMenuSvc.GetRole2MenuListAsync(roleId);
            return PartialView(model);
        }

        /// <param name="btnIds">39_16,40_1,40_2</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetButtons(long roleId, string btnIds)
        {
            var result = new JsonModel();
            if (string.IsNullOrEmpty(btnIds))
            {
                result.statusCode = 300;
                result.message = "请选择按钮！";
                return Json(result);
            }
            await UserRole2MenuRepository.SaveRole2MenuButtons(roleId, btnIds);
            UserRoleSvc.ClearRoleUserCache(roleId);
            result.message = "保存成功！";
            return Json(result);
        }



        /// <summary>
        /// 设置角色的数据权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public ActionResult SetFilterRule(long roleId)
        {
            //Source、FilterGroup
            var list =  UserRoleSvc.GetFilterList(roleId);
            ViewBag.roleId = roleId;
            return PartialView(list);
        }
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> DeleteFilterRule(long id)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (id < 1)
            {
                return Json(result);
            }
            var model = await UserRole2FilterRepository.GetUnDeleteByIdAsync(id);
            if (model == null)
            {
                return Json(result);
            }
            await UserRole2FilterRepository.Delete(model);
            UserRoleSvc.ClearRoleUserCache(model.RoleId);
            await LogRepository.Insert(TableSource.UserRole2Filter, OperationType.Delete, "删除角色数据规则" + model.Name, id);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> SetFilterRule(long id, string filterGroup,string name,string source,long roleId)
        {
            var result = new JsonModel();
            var exist = await UserRole2FilterRepository.ExistSourceAsync(id, source);
            if (exist)
            {
                result.statusCode = 300;
                result.message = $"已存在资源为{source}的角色数据规则！";
                return Json(result);
            }
            UserRole2Filter userFilter = null;
            if (id > 0)
            {
                userFilter = await UserRole2FilterRepository.GetByIdAsync(id);
                if (userFilter == null)
                {
                    result.statusCode = 300;
                    result.message = $"找不到Id为{id}的角色数据规则关联数据！";
                    return Json(result);
                }
            }
            else
            {
                userFilter = DbFactory.CreateUserRole2Filter();
            }
            userFilter.RoleId = roleId;
            userFilter.Name = name;
            userFilter.Source = source;
            userFilter.FilterGroups = filterGroup;
            await UserRole2FilterRepository.SaveAsync(userFilter);
            UserRoleSvc.ClearRoleUserCache(userFilter.RoleId);
            result.message = "保存成功！";
            var propModelList = GetPropertyList(source);
            userFilter.SourceProperties = propModelList;
            result.Data = userFilter;
            return Json(result);
        }

        [HttpPost]
        [ValidateButton(Buttons = SysButton.Grant, ActionName = "Index")]
        public async Task<ActionResult> GetCurrResources(FilterCurrent curr)
        {
            var list = await UserRoleSvc.GetResListAsync(curr);
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetSource(string source)
        {
            var propModelList = GetPropertyList(source);
            return Json(propModelList);
        }
       
        private List<PropertyModel> GetPropertyList(string source)
        {
            var type = Assembly.Load("Hei10.Domain").GetType("Hei10.Domain.Models." + source);
            if (type == null)
            {
                return null;
            }
            var propModelList = new List<PropertyModel>();
            var propertyInfoList = type.GetProperties();
            foreach (var property in propertyInfoList)
            {
                var proModel = new PropertyModel()
                {
                    Name = property.Name,
                    TypeName = property.PropertyType.IsEnum ? "Enum" : property.PropertyType.Name,
                };
                var nameAttr = property.GetCustomAttribute(typeof(DisplayNameAttribute));
                if (nameAttr == null)
                {
                    proModel.DisplayName = property.Name;
                }
                else
                {
                    proModel.DisplayName = ((DisplayNameAttribute)nameAttr).DisplayName;
                }
                proModel.DisplayName = $"{proModel.DisplayName}({proModel.TypeName})";
                propModelList.Add(proModel);
            }
            foreach (var curr in FilterCurrent.CurrentDeptId.GetSList())
            {
                propModelList.Add(new PropertyModel()
                {
                    Name = curr.Value,
                    DisplayName = curr.Text,
                    TypeName = "Enum"
                });
            }
            return propModelList;
        }
    }
}