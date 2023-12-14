using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using stackblob.Application.Models;

namespace stackblob.Application.Interfaces.Services;

public interface IFileService
{
    Task<bool> AttachmentExists(Attachment a, CancellationToken cancellationToken);
    Task ClearAll();
    Task<ICollection<int>> RemoveAttachments(CancellationToken cancellationToken = default, params Attachment[] attachments);

    Task<ICollection<Attachment>> UploadFilesTo(AttachmentType destination, CancellationToken cancellationToken = default, params IFormFile[] attachments);
}
