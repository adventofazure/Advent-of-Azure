using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace day7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Day7Controller : ControllerBase
    {
        private readonly ILogger<Day7Controller> _logger;

        public Day7Controller(ILogger<Day7Controller> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Solve(Input input)
        {
            if (input.input == null) {
                return "Day7. No input given.";
            }
            return "Part 1: " + Day7Solver.SolvePart1(input.input) + "\n" +
                "Part 2: " + Day7Solver.SolvePart2(input.input);                
        }
    }
}
