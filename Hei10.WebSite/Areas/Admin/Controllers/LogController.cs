using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hei10.Domain.Enums;
using Hei10.Domain.Repositories;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.WebSite.Areas.Admin.Models;
using log4net;
using log4net.Appender;
using PagedList;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class LogController : AdminController
    {
        public ISysOperationLogRepository SysOperationLog { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }

        [ValidatePage]
        public ActionResult Index(string src, string srcId, string tit, string uname, TableSource? cat, OperationType? type, DateTime? minTime, DateTime? maxTime, string orderField, string orderDirection, int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.pageCurrent = pageCurrent;
            ViewBag.pageSize = pageSize;
            ViewBag.tit = tit;
            ViewBag.src = src;
            ViewBag.srcId = srcId;
            ViewBag.uname = uname;
            ViewBag.minTime = minTime;
            ViewBag.maxTime = maxTime;
            var list = SysOperationLog.GetPagedList(pageCurrent, pageSize, orderField, orderDirection, src, srcId, cat, type, tit, uname, minTime, maxTime);
            return View(list);
        }

        [ValidatePage]
        public ActionResult LogList(string q, int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.pageCurrent = pageCurrent;
            ViewBag.pageSize = pageSize;
            DirectoryInfo d = new DirectoryInfo(SysConfigRepository.LogFilePath);
            var query = d.GetFiles("*.*").Where(file => file.Name.ToLower().EndsWith(".txt"));
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(l => l.Name.Contains(q));
            }
            var list = query
                .Select(m => new LogModel
                {
                    Name = m.Name,
                    Length = m.Length,
                    CreatDateTime = m.CreationTime,
                    LastWriteTime = m.LastWriteTime,
                    FullName = m.FullName
                })
                .OrderByDescending(m=>m.LastWriteTime)
                .ToPagedList(pageCurrent, pageSize);
            return View(list);
        }

        [ValidateButton(Buttons = SysButton.Download, ActionName = "LogList")]
        public ActionResult DownFile(string filePath, string name)//相对路径及完整文件名（有后缀）
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        [ValidateButton(Buttons = SysButton.Browse, ActionName = "LogList")]
        public FileStreamResult ReadFile(string filepath)
        {
            FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            return File(stream, "text/plain");
        }
    }
}