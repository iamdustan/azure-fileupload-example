azure-fileupload-example
========================

Example tutorial can be found here:
- http://www.windowsazure.com/en-us/develop/net/how-to-guides/blob-storage-v17/

NuGet Packages Required:
- Windows Azure Storage

Other Requirements:
- You need to set up a Windows Azure Storage account in the Management Portal
- You will then need the account name and access keys for the web.config file

What this does:
- It creates a container called "mykittys" and sets the permissions to public readonly
- GET will send back a list of all the blobs in the container
- GET(blobName) will send back a specific blob
- POST, PUT will insert or update a specific blob ex {"BlobName":"mykitty":"FilePath":"[filepath]"}
- DELETE(blobName) will delete a specific blob
