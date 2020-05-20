using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using FluentValidationTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FluentValidation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new Person());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Person person)
        {
            return await new ValueTask<IActionResult>(Ok());
        }

        [HttpPost("without-validation")]
        public async Task<IActionResult> PostWithoutValidation([FromBody][CustomizeValidator(Skip = true)] Person person)
        {
            return await new ValueTask<IActionResult>(Ok());
        }

        [HttpPost("with-ruleset")]
        public async Task<IActionResult> PostWithRuleSet([FromBody][CustomizeValidator(RuleSet = "default,newPerson")] Person person)
        {
            return await new ValueTask<IActionResult>(Ok());
        }

        [HttpPost("with-manual-validation")]
        public async Task<IActionResult> PostWithManualValidation([FromBody][CustomizeValidator(Skip = true)] Person person)
        {
            var validator = new PersonValidator();
            var result = await validator.ValidateAsync(person);

            if(result.IsValid == false)
            {
                result.AddToModelState(ModelState, null);
                return ValidationProblem(); //BadRequest(ModelState); <-- this doesn't returns the same result as the middleware
            }

            return await new ValueTask<IActionResult>(Ok());
        }

        [HttpPost("with-manual-validation-and-throw")]
        public async Task<IActionResult> PostWithManualValidation2([FromBody][CustomizeValidator(Skip = true)] Person person)
        {
            var validator = new PersonValidator();
            await validator.ValidateAndThrowAsync(person);

            return await new ValueTask<IActionResult>(Ok());
        }

        [HttpPost("with-model-state")]
        public async Task<IActionResult> PostWithModelState([FromBody][CustomizeValidator(Skip = true)] Person person)
        {
            ModelState.AddModelError(nameof(person), "error1");
            ModelState.AddModelError(nameof(person.Id), "error2");

            return ValidationProblem();
        }
    }
}
