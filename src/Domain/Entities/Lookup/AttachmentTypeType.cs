using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.Lookup;
public class AttachmentTypeType : BaseEntityEnum
{
    public AttachmentTypeType()
    {
        Attachments = new List<Attachment>();
    }

    public AttachmentType AttachmentTypeId { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
}
