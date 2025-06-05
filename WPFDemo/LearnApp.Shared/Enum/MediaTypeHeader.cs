using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LearnApp.Shared.Enum
{
    public enum MediaTypeHeader
    {
        /// <summary>
        /// 原生form表单
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        FormUrlencoded = 0,
        /// <summary>
        /// 文件
        /// </summary>
        [Description("multipart/form-data")]
        FormData = 1,
        /// <summary>
        /// JSON格式
        /// </summary>
        [Description("application/json")]
        Json = 2,
        /// <summary>
        /// XML格式
        /// </summary>
        [Description("application/xml")]
        Xml = 3
    }
}
