using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace stackblob.Domain.Entities;

//public class MongoQuestion : BaseEntityUserTracking
//{
//    public MongoQuestion()
//    {
//        Tags = new List<string>();
//        Answers = new List<MongoAnswer>();
//    }
//    public string QuestionId { get; set; }

//    public string Title { get; set; } = string.Empty;
//    public string Description { get; set; } = string.Empty;

//    public ICollection<string> Tags { get; set; }
//    public ICollection<MongoAnswer> Answers { get; set; }
//}

//public class MongoAnswer : BaseEntityUserTracking
//{
//    public string AnswerId { get; set; }
//    public string Title { get; set; } = string.Empty;
//    public string Description { get; set; } = string.Empty;
//}