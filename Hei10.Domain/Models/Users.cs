﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Hei10.Domain.Models
{
    public partial class Users : AggregateRoot
    {
        [DisplayName("登录名称")]
        [MaxLength(50)]
        [Required]
        public virtual string LoginName
        {
            get;
            set;
        }

        [DisplayName("密码")]
        [MaxLength(250)]
        [Required]
        public virtual string Password
        {
            get;
            set;
        }

        [DisplayName("密码盐")]
        [MaxLength(250)]
        [Required]
        public virtual string PasswordSalt
        {
            get;
            set;
        }

        [DisplayName("真实姓名")]
        [MaxLength(50)]
        [Required]
        public virtual string RealName
        {
            get;
            set;
        }

        [DisplayName("备注")]
        [MaxLength(250)]
        public virtual string Remark
        {
            get;
            set;
        }

        [DisplayName("登录次数")]
        public virtual int LoginCount
        {
            get;
            set;
        }

        [DisplayName("头像")]
        [MaxLength(250)]
        public virtual string HeadImg { get; set; }

        [DisplayName("性别，0:保密;1:男;2:女")]
        public virtual Gender Gender { get; set; }
    }

}