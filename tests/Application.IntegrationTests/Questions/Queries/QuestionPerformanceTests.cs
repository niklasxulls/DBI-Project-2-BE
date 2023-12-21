using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Domain.Entities;
using Xunit;

namespace stackblob.stackblob.Application.IntegrationTests.Questions.Queries;


public class QuestionPerformanceTests : TestBase
{
    public QuestionPerformanceTests(SetupFixture setup) : base(setup)
    {
    }



    [Theory]
    [InlineData(1000)]
    [InlineData(10000)]
    [InlineData(100000)]
    [InlineData(1000000)]
    public async Task ShouldTestPerformance(int size)
    {
    }
}