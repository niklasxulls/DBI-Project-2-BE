using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.Settings;
using stackblob.Domain.Entities.MongoFE;
using MongoDB.Driver;
using MongoDB.Bson;

namespace stackblob.Application.UseCases.Questions.Queries.GetQuestions;
public class GetQuestionsQuery : IRequest<ICollection<QuestionMongoFE>>
{
    public string? SearchTerm { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTill { get; set; }
    public ICollection<string>? TagIds { get; set; }
    public ICollection<string>? UserIds { get; set; }
}

public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, ICollection<QuestionMongoFE>>
{
    private readonly IMapper _mapper;

    public GetQuestionsQueryHandler(IMapper mapper)
    {
		_mapper = mapper;
    }

    public async Task<ICollection<QuestionMongoFE>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var client = new MongoClient(GlobalUtil.ConnectionString);
        var db = client.GetDatabase(GlobalUtil.MongoDbName);

        var collection = db.GetCollection<QuestionMongoFE>("QUESTION_FE_PROD");


        var filters = new List<FilterDefinition<QuestionMongoFE>>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var searchTermFilter = Builders<QuestionMongoFE>.Filter.Regex(nameof(QuestionMongoFE.Title), new BsonRegularExpression(request.SearchTerm, "i")) |
                                   Builders<QuestionMongoFE>.Filter.Regex(nameof(QuestionMongoFE.Description), new BsonRegularExpression(request.SearchTerm, "i"));

            filters.Add(searchTermFilter);
        }

        if (request.CreatedAtFrom.HasValue && request.CreatedAtTill.HasValue)
        {
            var dateFilter = Builders<QuestionMongoFE>.Filter.Gte(question => question.CreatedAt, request.CreatedAtFrom.Value) &
                             Builders<QuestionMongoFE>.Filter.Lte(question => question.CreatedAt, request.CreatedAtTill.Value);
            filters.Add(dateFilter);
        }

        if (request.TagIds != null && request.TagIds.Any())
        {
            var tagIdsBsonArray = new BsonArray(request.TagIds.Select(id => new ObjectId(id)));
            var tagIdsFilter = new BsonDocument("Tags._id", new BsonDocument("$in", tagIdsBsonArray));
            filters.Add(tagIdsFilter);
        }

        if (request.UserIds != null && request.UserIds.Any())
        {
            var userIdsBsonArray = new BsonArray(request.UserIds.Select(id => new ObjectId(id)));
            var userIdsFilter = new BsonDocument("CreatedBy._id", new BsonDocument("$in", userIdsBsonArray));
            filters.Add(userIdsFilter);
        }

        if(filters.Count < 1)
        {
            var results = await collection.Find(a => true).ToListAsync();
        return results;

        }
        else
        {
            var combinedFilter = Builders<QuestionMongoFE>.Filter.And(filters);

            var results = await collection.Find(combinedFilter).ToListAsync();
        return results;
        }

    }
}