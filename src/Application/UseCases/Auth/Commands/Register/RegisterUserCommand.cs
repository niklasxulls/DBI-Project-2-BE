using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Mapping;
using stackblob.Application.Models;
using AutoMapper;
using stackblob.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using stackblob.Application.UseCases.Users.Commands;
using stackblob.Application.UseCases.Auth.Commands.Login;
using stackblob.Application.UseCases.Auth.Commands.RequestEmailVerification;
using stackblob.Application.Models.DTOs.Auth;
using stackblob.Application.Models.DTOs.Users;

namespace stackblob.Application.UseCases.Auth.Commands.Register;

public class RegisterUserCommand : IRequest<UserAuthDto>, IMapFrom<User>
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    //in case of the selection of a provided avatar or by using the api for displaying the avatar, the ProfilePicture is null
    //and the ProfilePictureUrl not
    public IFormFile? ProfilePicture { get; set; }
    public IFormFile? Banner { get; set; }
    public ICollection<SocialDto>? Socials { get; set; }

    public void Mapping(Profile p)
    {
        p.CreateMap<RegisterUserCommand, User>()
            .ForMember(c => c.Socials, o => o.MapFrom((reg, user, i, context) =>
            {
                if (reg.Socials == null)
                {
                    return null;
                }
                return reg.Socials.Select(s => context.Mapper.Map<UserSocialType>(s));
            }))
            .ForMember(d => d.ProfilePicture, o => o.Ignore())
            .ForMember(d => d.Banner, o => o.Ignore())
            ;
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserAuthDto>
{
    private readonly IAuthService _auth;
    private readonly IMapper _mapper;
    private readonly IStackblobDbContext _context;
    private readonly IFileService _fileService;
    private readonly IMailService _mailService;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler(IAuthService auth, IMapper mapper, IStackblobDbContext context, IFileService fileService, IMailService mailService)
    {
        _auth = auth;
        _mapper = mapper;
        _context = context;
        _fileService = fileService;
        _mailService = mailService;
    }
    public async Task<UserAuthDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);

        var credentials = _auth.GenerateCredentials(request);

        user.Password = credentials.HashedPassword;
        user.Salt = credentials.Salt;

        _context.Users.Add(user);


        //var hasToSave = false;
        if (request.ProfilePicture != null)
        {
            var profilePicture = await _fileService.UploadFilesTo(AttachmentType.ProfileAvatar, cancellationToken, request.ProfilePicture!);
            user.ProfilePicture = profilePicture.First();
            //user.ProfilePicture = profilePicture;
        }
        if (request.Banner != null)
        {
            var bannerPicture = await _fileService.UploadFilesTo(AttachmentType.ProfileBanner, cancellationToken, request.ProfilePicture!);
            user.Banner = bannerPicture.First();
            //var bannerPicture = await _fileService.SaveUserBannerPicture(user.UserId, request.Banner);
            //user.Banner = bannerPicture;
        }
        await _context.SaveChangesAsync(cancellationToken);

        var verification = new UserEmailVerification()
        {
            ExpiresAt = DateTimeUtil.Now().AddDays(1),
            UserId = user.UserId ,
        };

        _context.UserEmailVerifications.Add(verification);


        await _mailService.SendEmailVerification(user, verification.UserEmailVerificationAccess);


        await _context.SaveChangesAsync(cancellationToken);

        //await _mediator.Send(new LoginUserCommand() { });

        //await _mediator.Send(new RequestEmailVerificationCommand() { });



        return _mapper.Map<UserAuthDto>(user);
        //return await _auth.Authenticate(loginUserCmd, cancellationToken);
    }

}
