using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Models.ResponseData
{
    public class ResponseData
    {
        public bool State { get; set; }
        public string Message { get; set; }
        public object Paginator { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Code { get; set; }


        public ResponseData()
        {

        }
        public ResponseData(bool state)
        {
            this.State = state;
        }
        public ResponseData(bool state, string msg, object data)
        {
            this.State = state;
            this.Data = data;
            this.Message = msg;
        }

        public ResponseData(bool state, string msg)
        {
            this.State = state;
            this.Message = msg;
        }

        public ResponseData(bool state, string msg, object paging, object data)
        {
            this.State = state;
            this.Data = data;
            this.Message = msg;
            this.Paginator = paging;
        }
    }
}