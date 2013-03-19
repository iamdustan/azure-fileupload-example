using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SampleFileStorage.Models;

namespace SampleFileStorage.Controllers
{
    public class FilesController : ApiController
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobContainer container;

        public FilesController()
        {
            // Retrieve storage account from connection string
            storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // lets create a container full of kitties!!
            container = blobClient.GetContainerReference("mykittys");

            // only need to set permissions once
            if (container.Exists()) return;

            container.CreateIfNotExists();

            // this sets the permissions to public readonly
            container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

        }

        // GET api/files
        public List<FileModel> Get()
        {
            List<FileModel> uris = new List<FileModel>();

            foreach (var blobItem in container.ListBlobs())
            {
                var newBlob = new FileModel();
                newBlob.Uri = blobItem.Uri;
                uris.Add(newBlob);
            }

            // the uri can be put direction into an img tag
            // ex <img src="http://snoddystests.blob.core.windows.net/mykittys/jasonskitty">

            return uris;
        }

        // GET api/files/jasonskitty
        public FileModel Get(string id)
        {
            FileModel fileModel = new FileModel();
            fileModel.Uri = container.GetBlockBlobReference(id).Uri;
            return fileModel;
        }

        // POST api/files
        // we are using post for both posting new and editing a blob
        public HttpResponseMessage Post([FromBody]FileModel blobInfo)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobInfo.BlobName);

            using (var fileStream = System.IO.File.OpenRead(blobInfo.FilePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            return Request.CreateResponse(HttpStatusCode.Created, blobInfo);
        }

        public HttpResponseMessage Delete(string id)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);
            blockBlob.Delete();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}