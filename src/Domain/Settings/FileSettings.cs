using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stackblob.Domain.Settings;
public class FileSettings
{
    public string CredentialPath { get; set; } = string.Empty;
    public string Bucket { get; set; } = string.Empty; 

    public static string PublicFileBaseUrl { get; set;} = string.Empty;
}
