using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Interfaces.Services;

public interface IMailService
{
    Task<bool> SendEmailVerification(User user, Guid verificationGuid);
}
