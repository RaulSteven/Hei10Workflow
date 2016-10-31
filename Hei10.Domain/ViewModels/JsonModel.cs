using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hei10.Domain.ViewModels
{
    public class JsonModel
    {
        public JsonModel()
        {
            statusCode = 200;
            closeCurrent = true;
        }

        public int statusCode { get; set; }
        public string message { get; set; }
        public bool closeCurrent { get; set; }
        public List<ErorrModel> Error { get; set; }

        public object Data { get; set; }

        public int gridNumber { get; set; }

        public void GetError(ModelStateDictionary modelState)
        {
            Error = modelState.Where(d => d.Value.Errors.Any())
                              .Select(d => new ErorrModel { Key = d.Key, Value = d.Value.Errors.Any() ? d.Value.Errors.FirstOrDefault().ErrorMessage : "" })
                              .ToList();
            if (!Error.Any())
            {
                return;
            }
            statusCode = 300;
            closeCurrent = false;
        }
    }

    public class ErorrModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
