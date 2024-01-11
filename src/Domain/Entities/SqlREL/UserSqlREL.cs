using stackblob.Domain.Entities.SqlREL.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities.SqlREL;

public class UserSqlREL : BaseEntitySqlREL
{
    public UserSqlREL()
    {
        QuestionsCreated = new List<QuestionSqlREL>();
        AnswersCreated = new List<AnswerSqlREL>();
    }

    public int UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Name => Firstname + " " + Lastname;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string? StatusText { get; set; }

    public ICollection<QuestionSqlREL> QuestionsCreated { get; set; }
    public ICollection<AnswerSqlREL> AnswersCreated { get; set; }
}
