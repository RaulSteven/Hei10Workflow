using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Domain.Models
{
    public partial class Attachment
    {
        private string _fileSizeStr;
        [NotMapped]
        public string FileSizeStr
        {
            get
            {
                if (string.IsNullOrEmpty(_fileSizeStr))
                {
                    if (FileSize > 1000000)
                    {
                        _fileSizeStr = Math.Round((decimal)FileSize / (1024 * 1024), 2) + "M";
                    }
                    else
                    {
                        _fileSizeStr = Math.Round((decimal)FileSize / 1024, 2) + "KB";
                    }
                }
                return _fileSizeStr;
            }
        }
    }
}
