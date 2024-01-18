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
using NuGet.Packaging;
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Domain.Entities;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Questions.Update;


public class QuestionSqlRELUpdatePerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionSqlRELUpdatePerformanceTests(SetupFixture setup, ITestOutputHelper output) : base(setup)
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
        foreach (var question in questionFakerSqlREL.Generate(size))
        {
            _sqlContext.QuestionsSqlREL.Add(question);
        }
        await _sqlContext.SaveChangesAsync(default);

        var dbStopWatch = new Stopwatch();

        // gather entries
        var questions = await _sqlContext.QuestionsSqlREL.ToListAsync();

        foreach(var question in questions)
        {
            question.Title = "UPDATED TITLE";
        }

        // update in db
        dbStopWatch.Start();

        await _sqlContext.SaveChangesAsync(default);

        dbStopWatch.Stop();

        // log
        _output.WriteLine(string.Format("Updating {0} entries in SqlServerDB took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }
}
