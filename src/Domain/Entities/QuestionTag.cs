using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities;

public class QuestionTag
{
    public string QuestionId { get; set; }
    public Question Question { get; set; }
    public string TagId { get; set; }
    public Tag Tag { get; set; }
}
