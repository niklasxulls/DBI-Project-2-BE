using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Application.Mapping;

namespace stackblob.Application.Models.DTOs.Tags;
public class TagReadDto : IMapFrom<Tag>
{
    public int TagId { get; set; }
    public string Name { get; set; } = string.Empty;
}
