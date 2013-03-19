using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleFileStorage.Models
{
    public class FileModel
    {
        public virtual string BlobName { get; set; }
        public virtual string FilePath { get; set; }
        public virtual Uri Uri { get; set; }
    }
}