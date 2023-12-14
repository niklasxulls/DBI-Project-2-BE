using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Models.DTOs.Comments;
public abstract class CommentBaseDto
{
    public string Description { get; set; } = string.Empty;
}
