using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using BenchmarkDotNet.Attributes;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using NuGet.Packaging;
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Domain.Entities;
using stackblob.Domain.Entities.MongoFE;
using stackblob.Domain.Entities.MongoREL;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Questions.ReadWithIndex;

public class QuestionMongoFEReadIndexPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionMongoFEReadIndexPerformanceTests(SetupFixture setup, ITestOutputHelper output ) : base(setup)
    {
        _output = output;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Read_Performance_Without_Index(int size)
    {
        // create questions (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        var collection = _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_WITH_INDEX_COLLECTION_NAME);

        var questionMongFEBatches = questionsMongoFE.SplitIntoBatches(1000);

        foreach (var badge in questionMongFEBatches)
        {
            await collection.InsertManyAsync(badge);
        }

        // gather use
        var firstUser = questionUserPoolMongoFE.First();

        long accumulatedRunTime = 0;
        int runs = 10;

        for (int i = 0; i < runs; i++)
        {
            await collection.DeleteManyAsync(a => true);

            var questionsNew = questionFakerMongoFE.Generate(size);

            var batches = questionsNew.SplitIntoBatches(1000);

            foreach (var badge in batches)
            {
                await collection.InsertManyAsync(questionsNew);
            }


            var dbStopWatch = Stopwatch.StartNew();

            var questions = await _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME)
                                          .Find(a => a.CreatedBy.UserId == firstUser.UserId)
                                          .ToListAsync();

            dbStopWatch.Stop();

            accumulatedRunTime += dbStopWatch.ElapsedMilliseconds;
        }

        _output.WriteLine(string.Format("Querying {0} entries in MongoDB without index took {1}ms", size, accumulatedRunTime / runs));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Read_Performance_With_Index(int size)
    {
        // create questions (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        var collection = _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_WITH_INDEX_COLLECTION_NAME);

        var questionMongFEBatches = questionsMongoFE.SplitIntoBatches(10000);

        foreach (var badge in questionMongFEBatches)
        {
            await collection.InsertManyAsync(badge);
        }

        // create index
        // Define the index (e.g., an ascending index on the "fieldName" field)
        var indexKeys = Builders<QuestionMongoFE>.IndexKeys.Ascending("CreatedBy.UserId");

        // Create the index
        var indexModel = new CreateIndexModel<QuestionMongoFE>(indexKeys);
        collection.Indexes.CreateOne(indexModel);

        // assume it takes that long to create the index
        Thread.Sleep(Math.Max(size, 5000));

        // gather use
        var firstUser = questionUserPoolMongoFE.First();
        
        long accumulatedRunTime = 0;
        int runs = 10;

        for(int i = 0; i < runs; i++)
        {
            await collection.DeleteManyAsync(a => true);

            var questionsNew = questionFakerMongoFE.Generate(size);
            var batches = questionsNew.SplitIntoBatches(5000);

            foreach(var badge in batches)
            {
                await collection.InsertManyAsync(questionsNew);
            }


            var dbStopWatch = Stopwatch.StartNew();

            var questions = await _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_WITH_INDEX_COLLECTION_NAME)
                                          .Find(a => a.CreatedBy.UserId == firstUser.UserId)
                                          .ToListAsync();

            dbStopWatch.Stop();

            accumulatedRunTime += dbStopWatch.ElapsedMilliseconds;
        }

        _output.WriteLine(string.Format("Querying {0} entries in MongoDB with index took {1}ms", size, accumulatedRunTime / runs));
    }
}

public static class ListExtensions
{
    public static IEnumerable<List<T>> SplitIntoBatches<T>(this List<T> source, int batchSize)
    {
        for (int i = 0; i < source.Count; i += batchSize)
        {
            yield return source.GetRange(i, Math.Min(batchSize, source.Count - i));
        }
    }
}
