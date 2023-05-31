using System.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text.Json;
using Proyecta.Web.Helpers;

namespace Proyecta.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var hostName = System.Net.Dns.GetHostName();

        // Memory
        var gcMemoryInfo = GC.GetGCMemoryInfo();
        var installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes;

        // IP
        var ipArray = await System.Net.Dns.GetHostAddressesAsync(hostName);
        var ipList = ipArray.Select(ipAddress => ipAddress.ToString()).ToList();

        var info = new
        {
            dotnetVersion = RuntimeInformation.FrameworkDescription,
            operatingSystem = RuntimeInformation.OSDescription,
            runtimeIdentifier = RuntimeInformation.RuntimeIdentifier,
            processorArchitecture = RuntimeInformation.OSArchitecture.ToString(),
            cpuCores = Environment.ProcessorCount,
            containerized = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is not null,
            user = Environment.UserName,
            memory = $"{installedMemory} ({GeneralHelper.MemoryInBestUnit(installedMemory)})",
            hostName,
            ipList,
        };

        return Ok(info);
    }
}