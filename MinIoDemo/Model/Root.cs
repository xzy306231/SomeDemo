using System.Collections.Generic;

namespace MinIoDemo.Model
{

    public class ValueKind
    {

    }

    public class UserIdentity
    {
        /// <summary>
        /// 
        /// </summary>
        public string principalId { get; set; }
    }

    public class @object
    {
        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string eTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string contentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object userMetadata { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sequencer { get; set; }
    }

    public class S3
    {
        /// <summary>
        /// 
        /// </summary>
        public string s3SchemaVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string configurationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object bucket { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public @object @object { get; set; }
    }

    public class Source
    {
        /// <summary>
        /// 
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string port { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userAgent { get; set; }
    }

    public class RecordsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string eventVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string eventSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string awsRegion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string eventTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string eventName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserIdentity userIdentity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object requestParameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object responseElements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public S3 s3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Source source { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// demo/工程图纸/001/bb.txt
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RecordsItem> Records { get; set; }
    }



}