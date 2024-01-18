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
public class GetQuestionUsersQuery : IRequest<ICollection<QuestionUserMongoFE>>
{
}

public class GetQuestionUsersQueryHandler : IRequestHandler<GetQuestionUsersQuery, ICollection<QuestionUserMongoFE>>
{
    private readonly IMapper mapper;

    public GetQuestionUsersQueryHandler( IMapper mapper)
    {
			this.mapper = mapper;
    }

    public async Task<ICollection<QuestionUserMongoFE>> Handle(GetQuestionUsersQuery request, CancellationToken cancellationToken)
    {
        var client = new MongoClient(GlobalUtil.ConnectionString);
        var db = client.GetDatabase(GlobalUtil.MongoDbName);

        var collection = db.GetCollection<QuestionMongoFE>("QUESTION_FE_PROD");

        // Getting distinct tag names
        var distinctUserNames = await collection.Distinct<string>("CreatedBy.Name", Builders<QuestionMongoFE>.Filter.Empty).ToListAsync();
        var distinctUserEmails = await collection.Distinct<string>("CreatedBy.Email", Builders<QuestionMongoFE>.Filter.Empty).ToListAsync();
        var distinctUserIds = await collection.Distinct<ObjectId>("CreatedBy._id", Builders<QuestionMongoFE>.Filter.Empty).ToListAsync();

        var users = new List<QuestionUserMongoFE>();

        for(int i = 0; i  < distinctUserNames.Count; i++)
        {
            users.Add(new()
            {
                _id = distinctUserIds.ElementAt(i),
                Name = distinctUserNames.ElementAt(i),
                Email = distinctUserEmails.ElementAt(i),
            });
        }

        return users;
    }
}


