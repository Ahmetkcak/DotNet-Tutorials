using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        BackgroundJob.Enqueue(() => BackgroundJobService.Test());
        return Ok("Job is running in background");
    }
}

public class BackgroundJobService
{
    public static void Test()
    {
        Console.WriteLine("Running Hangfire ", DateTime.Now);
    }
}
