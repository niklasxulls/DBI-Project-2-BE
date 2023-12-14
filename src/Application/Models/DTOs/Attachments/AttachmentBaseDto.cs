using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Models.DTOs.Attachments;
public abstract class AttachmentBaseDto
{
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; }
}
