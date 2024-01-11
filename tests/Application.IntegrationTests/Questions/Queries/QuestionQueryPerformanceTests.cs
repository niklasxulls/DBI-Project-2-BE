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


public class QuestionQueryPerformanceTests : TestBase
{
    private readonly ITestOutputHelper _output;

    public QuestionQueryPerformanceTests(SetupFixture setup, ITestOutputHelper output ) : base(setup)
    {
        _output = output;
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task ShouldTestQueryPerformanceWithoutFilter(int size)
    {
        var testStopwatch = Stopwatch.StartNew();
        var dbStopWatch = Stopwatch.StartNew();

        foreach (var question in questionFaker.Generate(size))
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync(default);
        }

        dbStopWatch.Start();
        await _context.SaveChangesAsync(default);

        dbStopWatch.Stop();
        testStopwatch.Stop();

        var queryStopWatch = Stopwatch.StartNew();

        var questions = await _context.Questions.Take(100).ToListAsync();

        queryStopWatch.Stop();

        _output.WriteLine(string.Format("Querying Test without filter for {0} entries took {1}ms", size, testStopwatch.ElapsedMilliseconds));
        _output.WriteLine(string.Format("Querying {0} entries of {1} took {1}ms", questions.Count, size, queryStopWatch.ElapsedMilliseconds));
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task ShouldTestQueryPerformanceWithFilter(int size)
    {
        var testStopwatch = Stopwatch.StartNew();
        var dbStopWatch = Stopwatch.StartNew();

        foreach (var question in questionFaker.Generate(size))
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync(default);
        }

        dbStopWatch.Start();
        await _context.SaveChangesAsync(default);

        dbStopWatch.Stop();
        testStopwatch.Stop();

        var queryStopWatch = Stopwatch.StartNew();

        var tagIds = _context.Tags.Select(t => t.TagId).Take(2).ToList();
        //var questions = await _context.Questions.Where(a => a.Tags.Any(t => tagIds.Contains(t.TagId))).Take(100).ToListAsync();

        queryStopWatch.Stop();

        _output.WriteLine(string.Format("Querying Test with filter for {0} entries took {1}ms", size, testStopwatch.ElapsedMilliseconds));
        //_output.WriteLine(string.Format("Querying {0} entries of {1} took {1}ms", questions.Count, size, queryStopWatch.ElapsedMilliseconds));
    }
}
