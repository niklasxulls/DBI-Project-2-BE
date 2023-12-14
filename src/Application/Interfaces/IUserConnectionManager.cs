using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Interfaces;

public interface IUserConnectionManager
{
    void KeepUserConnection(string userId, string connectionId);
    void RemoveUserConnection(string connectionId);
    string GetUserConnections(string userId);
}
