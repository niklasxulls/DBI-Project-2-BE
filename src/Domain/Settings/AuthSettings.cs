using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Settings
{
    public class AuthSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        //time offset between Issuer (smth like Auth0) and 
        //Audience (our API) -> maybe due to high traffic
        //issuer needs longer to anwer which can result in
        //that the key should have been valid but because
        //of the overhead it is not valid anymore
        //because of ClockSew a tolerance of x seconds is added
        //in order to sustain the authentification
        public TimeSpan ClockSkew { get; set; }
        public int Expires { get; set; }
        public int RefreshTokenExpires { get; set; }

    }
}
