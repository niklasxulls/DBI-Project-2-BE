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
using Questions.ReadWithIndex;
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Domain.Entities;
using stackblob.Domain.Entities.MongoFE;
using stackblob.Domain.Settings;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Questions.Update;


public class QuestionMongoFEUpdatePerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionMongoFEUpdatePerformanceTests(SetupFixture setup, ITestOutputHelper output) : base(setup)
    {
        _output = output;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Update_Performance(int size)
    {
        // create records (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        var questionMongFEBatches = questionsMongoFE.SplitIntoBatches(1000);
        var collection = _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME);

        foreach (var badge in questionMongFEBatches)
        {
            await collection.InsertManyAsync(badge);
        }

        var dbStopWatch = new Stopwatch();

        // build query
        var update = Builders<QuestionMongoFE>.Update.Set($"{nameof(QuestionMongoFE.Title)}", "UPDATED TITLE");

        // update
        dbStopWatch.Start();

        // Perform the update operation
        var result = await collection.UpdateManyAsync(t => true, update);

        dbStopWatch.Stop();

        // log
        _output.WriteLine(string.Format("Updating {0} entries in MongoDB took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }
}
