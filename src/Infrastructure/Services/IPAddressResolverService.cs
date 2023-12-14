using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using stackblob.Application.Interfaces.Services;
using stackblob.Domain.Settings;
using stackblob.Domain.ValueObjects;
using stackblob.Infrastructure.Other;
using static System.Net.WebRequestMethods;

namespace stackblob.Infrastructure.Services;
public class IPAddressResolverService : IIPAddressResolverService
{
    private readonly HttpClient _httpClient;
    private readonly IPAddressResolverSettings _ipAddressResolverSettings;
    private const string RESOLVER_ENDPOINT = "https://api.apilayer.com/ip_to_location/{ip}";
    public IPAddressResolverService(HttpClient httpClient, IOptions<IPAddressResolverSettings> ipAddressResolverSettings)
    {
        _httpClient = httpClient;
        _ipAddressResolverSettings = ipAddressResolverSettings.Value;
    }
    public async Task<IPAddressLocation> ResolveLocationDetailsByIPAddress(IpAddress ipAddress)
    {
        _httpClient.DefaultRequestHeaders.Add("apikey", _ipAddressResolverSettings.ApiKey);

        var res = await _httpClient.GetAsync(RESOLVER_ENDPOINT.Replace("{ip}", ipAddress.ToString()));
        var content = await res.Content.ReadAsStringAsync();

        var options = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };



        var location = JsonConvert.DeserializeObject<IPAddressLocation>(content, options);
        //var y = TimeZoneInfo.FromSerializedString(location.Timezones.First());

        return location;
    }
}
