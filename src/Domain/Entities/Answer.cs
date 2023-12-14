using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class Answer : BaseEntityUserTracking
{
    public Answer()
    {
    }
    public int AnswerId { get; set; }
    public Question Question { get; set; } = null!;
    public int QuestionId { get; set; }
    public Question? CorrectAnswerQuestion { get; set; }
    public int? CorrectAnswerQuestionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
