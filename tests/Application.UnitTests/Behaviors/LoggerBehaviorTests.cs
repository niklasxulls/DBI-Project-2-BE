using MediatR;
using Moq;
using stackblob.Application.Behaviours;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Questions.Queries.Search;
using Xunit;

namespace stackblob.Application.UnitTests.Common.Behaviours;

public class LoggerBehaviorTests
{
    private readonly Mock<ILoggerService<IRequest<GetQuestionsQuery>>> _logger ;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly LoggingBehavior<IRequest<GetQuestionsQuery>, GetQuestionsQuery> _sut;
    public LoggerBehaviorTests()
    {
        _logger = new Mock<ILoggerService<IRequest<GetQuestionsQuery>>>();
        _currentUserService = new Mock<ICurrentUserService>();
        _sut = new LoggingBehavior<IRequest<GetQuestionsQuery>, GetQuestionsQuery>(_logger.Object);

    }

    [Fact]
    public async Task ShouldLog()
    {
        _currentUserService.Setup(x => x.UserId).Returns(1);


        await _sut.Handle(Mock.Of<IRequest<GetQuestionsQuery>>(), new CancellationToken(), Mock.Of<RequestHandlerDelegate<GetQuestionsQuery>>());

        _logger.Verify(i => i.Log(It.IsAny<string>(), It.IsAny<LoggingType>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ShouldLogError()
    {
        _currentUserService.Setup(x => x.UserId).Returns(1);

        try
        {
            await _sut.Handle(Mock.Of<IRequest<GetQuestionsQuery>>(), new CancellationToken(),() => { return null!; });
        } catch(Exception ex)
        {

        }

        _logger.Verify(i => i.Log(It.IsAny<string>(), LoggingType.Error), Times.AtLeastOnce);
    }
}
