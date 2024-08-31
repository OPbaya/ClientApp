using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AppsController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] AppsRequest request)
    {
        if (request == null || request.Apps == null)
        {
            return BadRequest("Invalid data.");
        }

        // Log the received apps list to the console
        Console.WriteLine("\n------------SERVER-----------\nReceived apps : \n");
        foreach (var app in request.Apps)
        {
            Console.WriteLine(app);
        }

        // Optionally, return the list back in the response
        return Ok(request.Apps);
    }
}

public class AppsRequest
{
    public List<string> Apps { get; set; }
}
