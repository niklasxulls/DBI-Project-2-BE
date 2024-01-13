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

namespace stackblob.stackblob.Application.IntegrationTests.Questions.Queries;


public class QuestionSqlRELInsertPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionSqlRELInsertPerformanceTests(SetupFixture setup, ITestOutputHelper output ) : base(setup)
    {
        _output = output;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Insert_Performance(int size)
    {
        var testStopwatch = Stopwatch.StartNew();
        var dbStopWatch = Stopwatch.StartNew();

        foreach (var question in questionFakerSqlREL.Generate(size))
        {
            _sqlContext.QuestionsSqlREL.Add(question);
            await _mongoContext.SaveChangesAsync(default);
        }

        dbStopWatch.Start();
        await _sqlContext.SaveChangesAsync(default);

        dbStopWatch.Stop();
        testStopwatch.Stop();

        _output.WriteLine(string.Format("Inserting Test for {0} entries took {1}ms", size, testStopwatch.ElapsedMilliseconds));
        _output.WriteLine(string.Format("Inserting {0} entries in DB took {1}ms", size, dbStopWatch.ElapsedMilliseconds));

    }
}
