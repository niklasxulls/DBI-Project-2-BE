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


public class QuestionInsertPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionInsertPerformanceTests(SetupFixture setup, ITestOutputHelper output ) : base(setup)
    {
        _output = output;
    }

    [SkipIfSQL]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task Should_Test_Insert_Performance(int size)
    {
        var testStopwatch = Stopwatch.StartNew();
        var dbStopWatch = Stopwatch.StartNew();

        foreach (var question in questionFakerMongoREL.Generate(size))
        {
            _context.QuestionsMongoREL.Add(question);
            await _context.SaveChangesAsync(default);
        }

        dbStopWatch.Start();
        await _context.SaveChangesAsync(default);

        dbStopWatch.Stop();
        testStopwatch.Stop();

        _output.WriteLine(string.Format("Inserting Test for {0} entries took {1}ms", size, testStopwatch.ElapsedMilliseconds));
        _output.WriteLine(string.Format("Inserting {0} entries in DB took {1}ms", size, dbStopWatch.ElapsedMilliseconds));

    }
}
