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

namespace Questions.Insert;


public class QuestionSqlRELInsertPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionSqlRELInsertPerformanceTests(SetupFixture setup, ITestOutputHelper output) : base(setup)
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

        foreach (var question in questionFakerSqlREL.Generate(size))
        {
            _sqlContext.QuestionsSqlREL.Add(question);
        }

        testStopwatch.Stop();

        // add to db
        dbStopWatch.Start();

        await _sqlContext.SaveChangesAsync(default);

        dbStopWatch.Stop();

        // log
        _output.WriteLine(string.Format("Creating Test records for {0} entries took {1}ms", size, testStopwatch.ElapsedMilliseconds));
        _output.WriteLine(string.Format("Inserting {0} entries in SqlServerDB took {1}ms", size, dbStopWatch.ElapsedMilliseconds));
    }
}
