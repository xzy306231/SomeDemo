﻿using System.IO;

namespace MinIoDemo.Model
{
    public class UploadFileArgs
    {
        public string BucketName { get; set; }
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
        public string ContentType { get; set; }
    }
}
