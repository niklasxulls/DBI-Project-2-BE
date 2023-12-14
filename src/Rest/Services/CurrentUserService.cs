using MongoDB.Bson;
using stackblob.Application.Interfaces.Services;
using stackblob.Domain.Settings;
using stackblob.Domain.ValueObjects;
using System.Security.Claims;

namespace stackblob.Rest.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ObjectId? _userId;
        public ObjectId? UserId { 
            get {
                if(_userId == null)
                {
                    var claimVal = _httpContextAccessor.HttpContext?.User?.FindFirstValue(AuthClaimSettings.USERID_CLAIM_NAME);
                    _userId =  claimVal == null ? null : ObjectId.Empty;
                }
                return _userId;
            } 
        }

        // Reminder: forward X-FORWARD Header when it comes to deployment, elsewise the ip address will the one of the reverse proxy in this case localhost
        public IpAddress IpAddress { 
            get
            {
                var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
                return IpAddress.From(ip?.ToString() ?? "");
            }
                
        }

        private bool? _isVerified;
        public bool IsVerified
        {
            get
            {
                if (_isVerified == null)
                {
                    var claimVal = _httpContextAccessor.HttpContext?.User?.FindFirstValue(AuthClaimSettings.USERISVERIFIED_CLAIM_NAME);
                    _isVerified = claimVal == null ? false : bool.Parse(claimVal);
                }
                return _isVerified.GetValueOrDefault();
            }
        }

        //public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(GlobalConfig.USERNAME_CLAIM_NAME);

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
    }
}
