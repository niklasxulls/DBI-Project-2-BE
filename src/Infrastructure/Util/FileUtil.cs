using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace stackblob.Infrastructure.Util;
public static class FileUtil
{
    public static ICollection<Attachment> AttachmentsFromFormFiles(AttachmentType type, params IFormFile[] formFiles)
    {
        return formFiles.Select(f => new Attachment()
        {
            Name = f.Name ?? f.FileName,
            TypeId = type,
            RelativePath = BuildFileName(type, f.FileName),
            Size = (int)f.Length,
        }).ToList();
    }

    public static string GetFolderByAttachmentType(AttachmentType t)
    {
        switch (t)
        {
            case AttachmentType.QuestionAttachment: return "question";
            case AttachmentType.AnswerAttachment: return "answer";
            case AttachmentType.ProfileAvatar: return "profile-avatar";
            case AttachmentType.ProfileBanner: return "profile-banner";
            case AttachmentType.UpcomingQuestionAttachment: return "upcoming-question-attachment";
            case AttachmentType.UpComingAnswerAttachment: return "upcoming-answer-attachment";
            default: return "other";
        }
    }

    public static string BuildFileName(AttachmentType t, string filename)
    {
        return $"{GetFolderByAttachmentType(t)}/{GuidUtil.NewGuid()}{Path.GetExtension(filename)}";
    }
}
