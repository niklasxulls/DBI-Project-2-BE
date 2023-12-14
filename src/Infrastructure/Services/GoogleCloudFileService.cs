using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Attachments;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Other;
using stackblob.Infrastructure.Util;

namespace stackblob.Infrastructure.Services;

public class GoogleCloudFileService : IFileService
{
    private readonly static IReadOnlyList<string> ImageEndings = new List<string>() { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif",
                        ".pjpeg", ".pjb", ".png", ".svg", ".webp", ".bmp", ".ico", ".cur", ".tif", ".tiff"};

    private readonly GoogleCredential _googleCredential;
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;



    public GoogleCloudFileService(IOptions<FileSettings> fileSettings)
    {
        _googleCredential = GoogleCredential.FromFile(fileSettings.Value.CredentialPath);
        _storageClient = StorageClient.Create(_googleCredential);
        _bucketName = fileSettings.Value.Bucket;
    }

    public async Task<ICollection<Attachment>> UploadFilesTo(AttachmentType destination, CancellationToken cancellationToken = default, params IFormFile[] attachments)
    {
        List<Attachment> mappedAttachments = new();

        foreach (var attachment in attachments)
        {
            var mappedAttachment = FileUtil.AttachmentsFromFormFiles(destination, attachment).First();


            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await attachment.CopyToAsync(memoryStream);
                    var objc = await _storageClient.UploadObjectAsync(_bucketName, mappedAttachment.RelativePath, null, memoryStream, cancellationToken: cancellationToken);

                    //var dataObject = await storageClient.UploadObjectAsync(bucketName, fileNameForStorage, null, memoryStream);
                    //return dataObject.MediaLink;
                }
            } catch (Exception ex)
            {
                var x = ex;
            }



            mappedAttachments.Add(mappedAttachment);
        }


        return mappedAttachments;
    }

    public async Task<ICollection<int>> RemoveAttachments(CancellationToken cancellationToken, params Attachment[] attachments)
    {
        var removedAttachments = new List<int>();

        if(attachments?.Any() ?? false)
        {
            foreach(var attachment in attachments)
            {
                await _storageClient.DeleteObjectAsync(_bucketName, attachment.RelativePath, cancellationToken: cancellationToken);
                removedAttachments.Add(attachment.AttachmentId);
            }
        }

        return removedAttachments;
    }

    public async Task<bool> AttachmentExists(Attachment a, CancellationToken cancellationToken)
    {
        return await _storageClient.GetObjectAsync(_bucketName, a.RelativePath, cancellationToken: cancellationToken) != null;

    }

    public async Task ClearAll()
    {
        var attachments = _storageClient.ListObjects(_bucketName);
        foreach (var obj in attachments)
        {
            await _storageClient.DeleteObjectAsync(obj);
        }
    }

  

    //public async Task<string> SaveUserProfilePicture(int userId, IFormFile data)
    //{
    //    var blobClient = GetUserProfilePictureClient(userId);

    //    await blobClient.UploadAsync(GetFileContent(data), new BlobHttpHeaders { ContentType = data.ContentType });

    //    return "users/" + blobClient.Name;
    //}

    //public async Task<string> SaveUserBannerPicture(int userId, IFormFile data)
    //{
    //    var blobClient = GetUserBannerPictureClient(userId);

    //    await blobClient.UploadAsync(GetFileContent(data), new BlobHttpHeaders { ContentType = data.ContentType });

    //    return "users/" + blobClient.Name;
    //}

    //public async Task RemoveUserProfilePicture(int userId)
    //{
    //    var blobClient = GetUserProfilePictureClient(userId);

    //    await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
    //}

    //public async Task RemoveUserBannerPicture(int userId)
    //{
    //    var blobClient = GetUserBannerPictureClient(userId);

    //    await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
    //}

    //public async Task RemoveAttachments(int messageId)
    //{
    //    var blobClient = GetAttachmentClient(messageId);

    //    await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
    //}

    //public async Task RemoveAttachments(int messageId, List<string> names)
    //{
    //    foreach(var attachmentName in names)
    //    {
    //        var blobClient = GetAttachmentClient(messageId, attachmentName);

    //        await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
    //    }
    //}

    //public async Task<List<Attachment>> SaveAttachments(int messageId, List<IFormFile> data)
    //{
    //    var containerClient = _blobServiceClient.GetBlobContainerClient("messages");
    //    var basePath = messageId.ToString();
    //    List<Attachment> res = new List<Attachment>();

    //    foreach(var file in data)
    //    {
    //        var path = $"{basePath}/{file.FileName}";
    //        var blobClient = containerClient.GetBlobClient(path);
    //        await blobClient.UploadAsync(GetFileContent(file), new BlobHttpHeaders { ContentType = file.ContentType });
    //        res.Add(new Attachment() { Name = file.FileName, RelativePath = "messages/" + path, TypeId = GetAttachmentType(file.Name), Size = file.Length });
    //    }

    //    return res;
    //}

    //private AttachmentType GetAttachmentType(string filename)
    //{
    //    //var ext = Path.GetExtension(filename);
    //    //if (ImageEndings.Contains(ext))
    //    //    return AttachmentType;

    //    return AttachmentType.AnswerAttachment;
    //}

    //private MemoryStream GetFileContent(IFormFile file)
    //{
    //    var ms = new MemoryStream();
    //        file.CopyTo(ms);
    //    ms.Position = 0;
    //    return ms;
    //}


    //private BlobClient GetUserProfilePictureClient(int userId)
    //{
    //    var containerClient = _blobServiceClient.GetBlobContainerClient("users");
    //    var path = $"{userId}/$profilePicture.png";
    //    var blobClient = containerClient.GetBlobClient(path);
    //    return blobClient;
    //}

    //private BlobClient GetUserBannerPictureClient(int userId)
    //{
    //    var containerClient = _blobServiceClient.GetBlobContainerClient("users");
    //    var path = $"{userId}/bannerPicture.png";
    //    var blobClient = containerClient.GetBlobClient(path);
    //    return blobClient;
    //}

    //private BlobClient GetAttachmentClient(int messageId, string attachmentName = null)
    //{
    //    var containerClient = _blobServiceClient.GetBlobContainerClient("messages");
    //    var basePath = messageId.ToString();
    //    var path = basePath;
    //    if(!string.IsNullOrEmpty(attachmentName))
    //    {
    //        path = $"{basePath}/{attachmentName}";
    //    }
    //    return containerClient.GetBlobClient(path);
    //}

    //public string GetBaseUrl()
    //{
    //    var containerClient = _blobServiceClient.GetBlobContainerClient("");
    //    return containerClient.Uri.AbsoluteUri;
    //}

    //public string ExtentUrl(string url)
    //{
    //    return GetBaseUrl().TrimEnd('/') + "/" + url;
    //}


}
