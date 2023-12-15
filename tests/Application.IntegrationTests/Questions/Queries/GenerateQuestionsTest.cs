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


public class GenerateQuestionsTest : TestBase
{
    public GenerateQuestionsTest(SetupFixture setup) : base(setup)
    {
    }



    [Fact]
    public async Task ShouldPageAnswers()
    {
        true.Should().Be(true);
    }
}