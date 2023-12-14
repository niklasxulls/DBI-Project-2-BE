using System.Runtime.Serialization;
using AutoMapper;
using stackblob.Application.Mapping;
using stackblob.Application.Models.DTOs.Answers;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.Models.DTOs.Users;
using stackblob.Domain.Entities;
using Xunit;

namespace stackblob.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }


    [Theory]
    [InlineData(typeof(User), typeof(UserDto))]
    [InlineData(typeof(Question), typeof(QuestionReadDto))]
    [InlineData(typeof(Answer), typeof(AnswerReadDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}
