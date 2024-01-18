using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using stackblob.Application.UseCases.Questions.Queries.GetQuestions;
using stackblob.Application.UseCases.Questions.Queries.GetQuestionTags;
using stackblob.Domain.Entities.MongoFE;
using stackblob.Rest.Controllers;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Rest.Controllers;

[Route("api/questions")]
public class QuestionController : ApiControllerBase
{

    [HttpGet, Route("")]
    public async Task<ActionResult<ICollection<QuestionMongoFE>>> GetQuestionsQuery([FromQuery] GetQuestionsQuery query)
    {
        var questions = await Mediator.Send(query);

        return questions.ToList();
    }

    [HttpGet, Route("tags")]
    public async Task<ActionResult<ICollection<QuestionTagMongoFE>>> GetQuestionTags([FromQuery] GetQuestionTagsQuery query)
    {
        var questions = await Mediator.Send(query);

        return questions.ToList();
    }

    [HttpGet, Route("users")]
    public async Task<ActionResult<ICollection<QuestionUserMongoFE>>> GetQuestionUsers([FromQuery] GetQuestionUsersQuery query)
    {
        var questions = await Mediator.Send(query);

        return questions.ToList();
    }
}
