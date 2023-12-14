using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class Attachment : BaseEntity
{
    public Attachment()
    {
    }
    public int AttachmentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public AttachmentType TypeId { get; set; }
    public AttachmentTypeType Type { get; set; } = null!;   
    public long Size { get; set; }

    public Answer? Answer { get; set; }
    public int? AnswerId { get; set; }
    public Question? Question { get; set; }
    public int? QuestionId { get; set; }
    public User? UserProfilePicture { get; set; }
    public int? UserProfilePictureId { get; set; }
    public int? UserBannerPictureId { get; set; }

    public User? UserBannerPicture { get; set; }

}
