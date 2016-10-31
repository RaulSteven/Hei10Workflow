using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JR.Domain.ViewModels;
using JR.Domain.Models;

namespace JR.WebSite.Models
{
    public class HomeModel
    {
        public ArticleClassifyListModel LeftArticleClassify { get; set; }
        public ArticleClassifyListModel CentreArticleClassify { get; set; }
        public ArticleClassifyListModel RightArticleClassify { get; set; }

        public List<Partner> PartnerList { get; set; }
    }
}