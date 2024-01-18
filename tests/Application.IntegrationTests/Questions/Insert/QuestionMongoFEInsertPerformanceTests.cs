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

namespace Questions.Insert;


public class QuestionMongoFEInsertPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionMongoFEInsertPerformanceTests(SetupFixture setup, ITestOutputHelper output) : base(setup)
    {
        _output = output;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Insert_Performance(int size)
    {
        // create records
        var testStopwatch = Stopwatch.StartNew();
        var dbStopWatch = new Stopwatch();

        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        var questionMongFEBatches = questionsMongoFE.SplitIntoBatches(1000);
        var collection = _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME);


        testStopwatch.Stop();

        // add to db
        dbStopWatch.Start();

        foreach (var badge in questionMongFEBatches)
        {
            await collection.InsertManyAsync(badge);
        }

        dbStopWatch.Stop();

        // log
        _output.WriteLine(string.Format("Creating Test records for {0} entries took {1}ms", size, testStopwatch.ElapsedMilliseconds));
        _output.WriteLine(string.Format("Inserting {0} entries in MongoDB took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }
}
