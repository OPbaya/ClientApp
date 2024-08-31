using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var apps = GetRunningApplications();
        Console.WriteLine("\n------------CLIENT-----------\nSending the following applications to the server :");
        foreach (var app in apps)
        {
            Console.WriteLine(app);
        }

        using (var client = new HttpClient())
        {
            var json = JsonSerializer.Serialize(new { apps });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7047/api/apps", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Server response:");
                Console.WriteLine(responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to send apps. Status code: {response.StatusCode}");
            }
        }
    }

    static List<string> GetRunningApplications()
    {
        var apps = new List<string>();
        foreach (var process in Process.GetProcesses())
        {
            try
            {
                // Filter out system processes and those without a title
                if (!string.IsNullOrEmpty(process.MainWindowTitle) && !process.ProcessName.Contains("System") && !process.ProcessName.Contains("Idle"))
                {
                    apps.Add(process.MainWindowTitle);
                }
            }
            catch
            {
                // Handle exceptions (e.g., access denied for some processes)
            }
        }
        return apps;
    }
}
