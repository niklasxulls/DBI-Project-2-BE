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
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Domain.Entities;
using stackblob.Domain.Entities.MongoFE;
using stackblob.Domain.Entities.MongoREL;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Questions.Read;

public class QuestionMongoFEReadPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionMongoFEReadPerformanceTests(SetupFixture setup, ITestOutputHelper output ) : base(setup)
    {
        _output = output;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Read_Performance_Without_Filter(int size)
    {
        // create questions (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME).InsertMany(questionsMongoFE);


        var dbStopWatch = Stopwatch.StartNew();

        var questions = await _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME).FindAsync(default);

        dbStopWatch.Stop();

        _output.WriteLine(string.Format("Querying {0} entries in MongoDB without filter took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Read_Performance_With_Filter(int size)
    {
        // create questions (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME).InsertMany(questionsMongoFE);

        // gather use
        var firstUser = questionUserPoolMongoFE.First();

        var dbStopWatch = Stopwatch.StartNew();

        var questions = await _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME)
                                      .Find(a => a.CreatedBy.UserId == firstUser.UserId)
                                      .ToListAsync();

        dbStopWatch.Stop();

        _output.WriteLine(string.Format("Querying {0} entries in MongoDB with filter took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }


    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Read_Performance_With_Filter_And_Projection(int size)
    {
        // create questions (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME).InsertMany(questionsMongoFE);

        // gather use
        var firstUser = questionUserPoolMongoFE.First();

        var dbStopWatch = Stopwatch.StartNew();


        var questions = await _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME)
                                      .Find(a => a.CreatedBy.UserId == firstUser.UserId)
                                      .Project(a => new QuestionMongoFE()
                                        {
                                            Title = a.Title,
                                            Description = a.Description
                                        })
                                        .ToListAsync(default);


        dbStopWatch.Stop();

        _output.WriteLine(string.Format("Querying {0} entries in MongoDB with filter and projection took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Read_Performance_With_Filter_And_Projection_And_Sort(int size)
    {
        // create questions (prepare)
        var questionsMongoFE = questionFakerMongoFE.Generate(size);
        _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONFE_COLLECTION_NAME).InsertMany(questionsMongoFE);

        // gather use
        var firstUser = questionUserPoolMongoFE.First();

        var dbStopWatch = Stopwatch.StartNew();


        var questions = await _mongoDB.GetCollection<QuestionMongoFE>(QUESTIONREL_COLLECTION_NAME)
                                      .Find(a => a.CreatedBy.UserId == firstUser.UserId)
                                      .Project(a => new QuestionMongoFE()
                                      {
                                          Title = a.Title,
                                          Description = a.Description
                                      })
                                      .SortByDescending(a => a.CreatedAt)
                                      .ToListAsync(default);


        dbStopWatch.Stop();

        _output.WriteLine(string.Format("Querying {0} entries in MongoDB with filter and projection took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }
}
