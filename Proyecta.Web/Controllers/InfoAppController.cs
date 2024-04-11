using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Proyecta.Core.Results;
using Proyecta.Web.Utils;

namespace Proyecta.Web.Controllers;

[ApiController]
[Route("info-app")]
[Authorize(Roles = "System,Administrator")]
public class InfoAppController : ControllerBase
{

}