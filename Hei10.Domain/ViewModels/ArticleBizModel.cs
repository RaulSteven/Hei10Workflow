using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Models;
using PagedList;

namespace Hei10.Domain.ViewModels
{
    public class ArticleBizModel
    {
        public Article Article { get; set; }

        public string ArticleClassifyName { get; set; }
    }

    public class ArticleClassifyListModel
    {
        public ArticleClassify ArticleClassify { get; set; }
        public List<Article> Article { get; set; }
    }

    public class ArticleSearchListModel
    {
        public long ArticleClassifyId { get; set; }
        public IPagedList<Article> PagedList { get; set; }
    }
}
