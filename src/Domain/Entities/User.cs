using stackblob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        RefreshTokens = new List<RefreshToken>();
        Socials = new List<UserSocialType>();
        LoginLocations = new List<LoginLocation>();
        Answers = new List<Answer>();
        Question = new List<Question>();
        QuestionsCreated = new List<Question>();
        AnswerVotes = new List<Vote>();
        QuestionVotes = new List<Vote>();
        QuestionComments =  new List<Comment>();
        AnswerComments = new List<Comment>();
        EmailVerficiations = new List<UserEmailVerification>();
    }

    public int UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Name => Firstname + " " + Lastname;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public int? ProfilePictureId { get; set; }
    public Attachment? ProfilePicture { get; set; }
    public int? BannerId { get; set; }
    public Attachment? Banner { get; set; }
    public string? StatusText { get; set; }
    private decimal? _reputation { get; set; }
    public decimal Reputation
    {
        get
        {
            if(_reputation == null)
            {
                _reputation = VoteUtil.CalculateUserReputation(this);
            }
            return (decimal) _reputation;
        }
    }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<UserSocialType> Socials { get; set; }
    public ICollection<LoginLocation> LoginLocations { get; set; }
    public ICollection<Question> Question { get; set; }
    public ICollection<Question> QuestionsCreated { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<Comment> AnswerComments { get; set; }
    public ICollection<Vote> AnswerVotes { get; set; }
    public ICollection<Vote> QuestionVotes { get; set; }
    public ICollection<Comment> QuestionComments { get; set; }
    public ICollection<UserEmailVerification> EmailVerficiations { get; set; }
}
