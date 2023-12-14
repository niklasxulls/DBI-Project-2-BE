using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Util;

public static class VoteUtil
{
    public static readonly decimal QuestionCreatedUpVoteReputationWeight = 10;
    public static readonly decimal QuestionCreatedDownVoteReputationWeight = -5;

    public static readonly decimal AnswerCreatedUpVoteReputationWeight = 20;
    public static readonly decimal AnswerCreatedDownVoteReputationWeight = -10;

    public static readonly decimal QuestionsCreatedReputationWeight = 0.5m;
    public static readonly decimal AnswersCreatedReputationWeight = 0.2m;
    public static readonly decimal CommentsCreatedReputationWeight = 0.01m;

    public static decimal CalculateUserReputation(User u)
    {
        decimal reputation = 0.0m;

        reputation += u.QuestionsCreated.Sum(x => QuestionsCreatedReputationWeight);
        reputation += u.Answers.Sum(x => AnswersCreatedReputationWeight);
        reputation += u.AnswerComments.Union(u.QuestionComments).Sum(x => CommentsCreatedReputationWeight);

        return reputation;
    }

    public static decimal CalculateQuestionPopularity(Question q)
    {
        decimal popularity = 0.0m;

        return popularity;
    }

    //public static Expression<Func<Question, decimal>> CalculateQuestionPopularityL(Question q) {

    //    ParameterExpression questionParameter = Expression.Parameter(typeof(Question));


    //    decimal popularity = 0.0m;

    //    popularity += q.QuestionVotes.Select(v => v.IsUpVote ? QuestionCreatedUpVoteReputationWeight : QuestionCreatedDownVoteReputationWeight).Sum();
    //    popularity += q.Answers.SelectMany(a => a.AnswerVotes).Select(v => v.IsUpVote ? AnswerCreatedUpVoteReputationWeight : AnswerCreatedDownVoteReputationWeight).Sum();

    //    return popularity;

    //}
}
