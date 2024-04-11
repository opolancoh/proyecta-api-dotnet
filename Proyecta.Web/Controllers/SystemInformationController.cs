using System.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Proyecta.Core.Results;
using Proyecta.Web.Utils;

namespace Proyecta.Web.Controllers;

[ApiController]
[Route("system-info")]
[Authorize(Roles = "System")]
public class SystemInformationController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var envVarsInfo = (from DictionaryEntry entry in Environment.GetEnvironmentVariables()
            let key = entry.Key.ToString()
            let value = entry.Value.ToString()
            select new KeyValuePair<string, string>(key, value)).ToList();

        var serverInfo = await GetServerInfo();

        return StatusCode(StatusCodes.Status200OK, new ApplicationResult
        {
            Status = StatusCodes.Status200OK,
            D = new { envVarsInfo, serverInfo }
        });
    }

    public async Task<List<KeyValuePair<string, string>>> GetServerInfo()
    {
        var hostName = System.Net.Dns.GetHostName();

        // Memory
        var gcMemoryInfo = GC.GetGCMemoryInfo();
        var installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;

        // IP
        var ipArray = await System.Net.Dns.GetHostAddressesAsync(hostName);
        var ipList = ipArray.Select(ipAddress => ipAddress.ToString()).ToList();

        var serverInfo = new List<KeyValuePair<string, string>>()
        {
            new("DotnetVersion", RuntimeInformation.FrameworkDescription),
            new("DotnetEnvironment",
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "undefined"),
            new("OperatingSystem", RuntimeInformation.OSDescription),
            new("RuntimeIdentifier", RuntimeInformation.RuntimeIdentifier),
            new("ProcessorArchitecture", RuntimeInformation.OSArchitecture.ToString()),
            new("CpuCores", Environment.ProcessorCount.ToString()),
            new(
                "Containerized",
                (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is not null).ToString()
            ),
            new("User", Environment.UserName),
            new("Memory",
                $"Installed Memory {GeneralHelper.MemoryInBestUnit(installedMemory)}"),
            new("HostName", hostName),
            new("IpList", string.Join(", ", ipList))
        };

        return serverInfo;
    }
}