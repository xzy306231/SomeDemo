using System;

namespace MinIoDemo.Model
{
    public class Result<T>
    {
        public Result(bool isSuccess, string msg)
        {
            this.IsSuccess = isSuccess;
            this.Msg = msg;

        }
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据列表
        /// </summary>
        public Object Data { set; get; }
    }
}
