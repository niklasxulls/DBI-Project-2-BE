using MongoDB.Bson;
using stackblob.Domain.Entities.MongoFE;
using stackblob.Domain.Entities.MongoREL;
using stackblob.Domain.Enums;
using stackblob.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Infrastructure.Persistence;

public static class StackblobDbContextSeed
{
    public static async Task SeedSampleData(StackblobDbContext context)
    {

        //var questionX = new QuestionMongoREL();
        //context.QuestionsMongoREL.Add(questionX);

        //var question = new QuestionMongoFE();
        //context.QuestionsMongoFE.Add(question);

        //await context.SaveChangesAsync(default);

    }
}
