using System;
using System.Collections.Generic;
using System.Text;

namespace LearnApp.Shared.BaseBusinessDto
{
    public class FdJsonResult<T>
    {
        public int Code { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
