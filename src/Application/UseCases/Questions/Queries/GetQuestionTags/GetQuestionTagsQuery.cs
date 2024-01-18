using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stackblob.Domain.Entities.MongoFE;
using MongoDB.Driver;
using stackblob.Domain.Settings;
using MongoDB.Bson;

namespace stackblob.Application.UseCases.Questions.Queries.GetQuestionTags;
public class GetQuestionTagsQuery : IRequest<ICollection<QuestionTagMongoFE>>
{
}

public class GetQuestionTagsQueryHandler : IRequestHandler<GetQuestionTagsQuery, ICollection<QuestionTagMongoFE>>
{
    private readonly IMapper mapper;

    public GetQuestionTagsQueryHandler( IMapper mapper)
    {
			this.mapper = mapper;
    }

    public async Task<ICollection<QuestionTagMongoFE>> Handle(GetQuestionTagsQuery request, CancellationToken cancellationToken)
    {
        var client = new MongoClient(GlobalUtil.ConnectionString);
        var db = client.GetDatabase(GlobalUtil.MongoDbName);

        var collection = db.GetCollection<QuestionMongoFE>("QUESTION_FE");

        // Getting distinct tag names
        var distinctTagNames = await collection.Distinct<string>("Tags.Name", Builders<QuestionMongoFE>.Filter.Empty).ToListAsync();

        // Getting distinct tag IDs
        var distinctTagIds = await collection.Distinct<ObjectId>("Tags.TagId", Builders<QuestionMongoFE>.Filter.Empty).ToListAsync();

        var tags = new List<QuestionTagMongoFE>();

        for(int i = 0; i  < distinctTagNames.Count; i++)
        {
            tags.Add(new()
            {
                Name = distinctTagNames.ElementAt(i),
                TagId = distinctTagIds.ElementAt(i)
            });
        }

        return tags;
    }
}


