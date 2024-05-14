using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Web.Utils;

namespace Proyecta.Web.Controllers;

[ApiController]
[Route("info")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ApiInformationController : ControllerBase
{
    [HttpGet("server")]
    public async Task<IActionResult> GetServerInfo()
    {
        var result = await GetServerInfoAsync();

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<object>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
            Data = result
        });
    }

    [HttpGet("system")]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> GetSystemInfo()
    {
        var result = (from DictionaryEntry entry in Environment.GetEnvironmentVariables()
            let key = entry.Key.ToString()
            let value = entry.Value.ToString()
            select new KeyValuePair<string, string>(key, value)).ToList();

        return StatusCode(StatusCodes.Status200OK, new ApiResponse<object>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
            Data = result
        });
    }

    private async Task<List<KeyValuePair<string, string>>> GetServerInfoAsync()
    {
        var hostName = System.Net.Dns.GetHostName();

        var apiVersion = Assembly.GetExecutingAssembly().GetName().Version;

        // Memory
        var gcMemoryInfo = GC.GetGCMemoryInfo();
        var installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;

        // IP
        var ipArray = await System.Net.Dns.GetHostAddressesAsync(hostName);
        var ipList = ipArray.Select(ipAddress => ipAddress.ToString()).ToList();

        var serverInfo = new List<KeyValuePair<string, string>>()
        {
            new("ApiVersion", apiVersion != null ? apiVersion.ToString() : "Undefined"),
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