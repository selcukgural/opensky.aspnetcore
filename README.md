## OpenSky.AspNetCore 

This package provides an extension method for adding the `OpenSkyService` to the service collection in an ASP.NET Core project.

Also, you can use [OpenSky.Core](https://github.com/selcukgural/opensky.core) package for your console applications or other types of projects.

### Configuration

Create a configuration file or set environment variables for the API key and other settings.

### Installation

.NET CLI:

```bash
  dotnet add package OpenSky.AspNetCore
```

Package Manager:

```bash
  Install-Package OpenSky.AspNetCore
```

### Usage

#### Example Usage for Dependency Injection

Here is an example of how to configure and use the `OpenSkyService` with dependency injection in an ASP.NET Core project:

1. Configure the service in `Startup.cs` or `Program.cs`:

```csharp
using OpenSky.AspNetCore.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOssClient(e =>
{
    e.ApiKey = "your-api-key";
});
```

2. Inject and use the service in your controllers or services:

```csharp
    using Microsoft.AspNetCore.Mvc;
    using OpenSky.Core.Service;

    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IOpenSkyService _openSkyService;

        public WeatherController(IOpenSkyService openSkyService)
        {
            _openSkyService = openSkyService;
        }

        [HttpGet("{location}")]
        public async Task<IActionResult> GetWeather(string location)
        {
            var response = await _openSkyService.SearchAsync(location);

            if (response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Error.Message);
        }
    }
```

This example demonstrates how to configure the `OpenSkyService` using dependency injection and how to use it in an ASP.NET Core controller to search for weather information.