using stackblob.Application.Behaviours;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Users.Commands;
using Castle.DynamicProxy;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using stackblob.Application.Attributes;
using stackblob.Application.UseCases.Auth.Commands.Login;

namespace Application.UnitTests.Behaviors
{
    public class AuthorizationBehavior
    {
        readonly Mock<ICurrentUserService> _moqUserService;
        Mock<LoginUserCommand> _moqRequest;
        readonly AuthorizationBehavior<LoginUserCommand> _sut;

        public AuthorizationBehavior()
        {
            _moqUserService = new Mock<ICurrentUserService>();
            _moqRequest = new Mock<LoginUserCommand>();
            _sut = new AuthorizationBehavior<LoginUserCommand> (_moqUserService.Object);
        }
        [Fact]
        public async Task AccessGrantedBecauseNoAuthIsNeeded()
        {
            await _sut.Process(_moqRequest.Object, default);
        }

        [Fact(Skip ="Cant_add_custom_Attribute_at_runtime")]
        public async Task AccessDeniedBecauseAuthIsNeededButNoUserIsReturned()
        {
            Authorize authAtt = new Authorize();

            Type[] ctorTypes = Type.EmptyTypes;
            var ctor = typeof(Authorize).GetConstructor(ctorTypes);

            // Create an attribute builder.
            object[] attributeArguments = new object[] { };
            CustomAttributeInfo info = new CustomAttributeInfo(ctor, attributeArguments); ;
            // Create the proxy generation options.
            // This is how we tell Castle.DynamicProxy how to create the attribute.
            var proxyOptions = new Castle.DynamicProxy.ProxyGenerationOptions();
            proxyOptions.AdditionalAttributes.Add(info);

            // Create the proxy generator.
            var proxyGenerator = new Castle.DynamicProxy.ProxyGenerator();

            // Create the class proxy.
            var classArguments = new object[] { };
            _moqRequest = (Mock < LoginUserCommand >) proxyGenerator.CreateClassProxy(typeof(Mock<LoginUserCommand>), proxyOptions, classArguments);


            _moqUserService.Setup(x => x.UserId).Returns(() => null);
            await Assert.ThrowsAsync<ForbiddenAccessException>
                (async () => await _sut.Process(_moqRequest.Object, default));
        }

        [Fact]
        public void AccessGrantedBecauseAuthAndUserIsValid()
        {
            Authorize authAtt = new Authorize();
            TypeDescriptor.AddAttributes(_moqRequest.Object.GetType(), authAtt);
            _moqUserService.Setup(x => x.UserId).Returns(() => 1);
        }
    }
}
