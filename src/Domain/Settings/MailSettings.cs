using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Settings;

public class MailSettings
{
    public string From { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
    public string AppPassword { get; set; } = string.Empty;
}
