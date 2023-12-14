using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Application.Models.DTOs.Auth;
public class RegisterUserDto
{
    public string Salt { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
}
