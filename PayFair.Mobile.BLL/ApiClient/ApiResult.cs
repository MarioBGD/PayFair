using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.Mobile.BLL.ApiClient
{
    public class ApiResult<T>
    {
        public System.Net.HttpStatusCode StatusCode { get; set; } = System.Net.HttpStatusCode.BadRequest;

        public bool IsOk => (StatusCode == System.Net.HttpStatusCode.OK);
        public T ResultContent { get; set; }
        //public bool IsResultContent => ResultContent == default(T);

        public string Message { get; set; }
    }
}
