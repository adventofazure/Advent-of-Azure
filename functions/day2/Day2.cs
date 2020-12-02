using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AdventOfAzure.Function
{
    public class PasswordSpec {
        public int FirstParameter { get; set; }

        public int SecondParameter { get; set; }

        public char Letter { get; set; }

        public string Password { get; set; }
    }

    public enum Part
    {
        One = 1,
        Two = 2
    }

    public static class Day2
    {
        public static PasswordSpec ParseSpec(string passwordSpec) {
            var parts = passwordSpec.Split(' ');
            if (parts.Length != 3) {
                return null;
            }
            var hyphenIndex = parts[0].IndexOf('-');
            if (hyphenIndex == -1) {
                return null;
            }
            return new PasswordSpec() {
                FirstParameter = Int32.Parse(parts[0].Substring(0, hyphenIndex)),
                SecondParameter = Int32.Parse(parts[0].Substring(hyphenIndex+1)),
                Letter = parts[1][0],
                Password = parts[2]
            };
        }

        public static bool validPasswordPart1(ILogger log, PasswordSpec passwordSpec) {
            var occurrences = 0;
            for (var i = 0; i < passwordSpec.Password.Length; i++) {
                if (passwordSpec.Password[i] == passwordSpec.Letter) {
                    occurrences++;
                }
            }
            return occurrences >= passwordSpec.FirstParameter && occurrences <= passwordSpec.SecondParameter;
        }

        public static bool validPasswordPart2(ILogger logger, PasswordSpec passwordSpec) {
            return passwordSpec.Password[passwordSpec.FirstParameter-1] == passwordSpec.Letter
                ^ passwordSpec.Password[passwordSpec.SecondParameter-1] == passwordSpec.Letter;
        }

        public static int Solve(ILogger log, string input, Part part) {
            log.LogInformation("Solving part " + part);
            var splittedInput = input.Split(',');
            var validPasswordCount = 0;
            foreach (var password in splittedInput)
            {
                var spec = ParseSpec(password);
                if ((part == Part.One && validPasswordPart1(log, spec))
                   || (part == Part.Two && validPasswordPart2(log, spec))) {
                    validPasswordCount++;
                }
            }
            return validPasswordCount;
        }

        [FunctionName("Day2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string input = req.Query["input"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            input = input ?? data?.input;

            string responseMessage = string.IsNullOrEmpty(input)
                ? "Day 2. No input given."
                : "Part 1: " + Solve(log, input, Part.One) + "\n"
                + "Part 2: " + Solve(log, input, Part.Two);

            return new OkObjectResult(responseMessage);
        }
    }
}
