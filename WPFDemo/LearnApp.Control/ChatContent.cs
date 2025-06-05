using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Control
{
    public class ChatContent
    {
        //用户
        public string Name { get; set; }
        //类型
        public int Type { get; set; }
        //消息内容
        public object Content { get; set; }
        //日期
        public DateTime DateTime { get; set; }
        //状态
        public int Status { get; set; }
    }
}
