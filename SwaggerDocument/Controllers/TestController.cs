using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwaggerDocument.Models;
using SwaggerDocument.OperationProcessor.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerDocument.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// PostJsonResponseValue
        /// </summary>
        /// <remarks>
        /// PostJsonResponseValue
        /// </remarks>
        /// <param name="inputModel"></param>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [OperationHeaderToken]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostJsonResponseValue([FromBody] PostJsonResponseValueInputModel inputModel)
        {
            var result = await Task.FromResult("Hello");
            return Ok(result);
        }
        /// <summary>
        /// PostJsonResponseJson
        /// </summary>
        /// <remarks>
        /// PostJsonResponseJson
        /// </remarks>
        /// <param name="inputModel"></param>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PostJsonResponseJsonOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostJsonResponseJson([FromBody] PostJsonResponseJsonInputModel inputModel)
        {
            var result = await Task.FromResult(new PostJsonResponseJsonOutputModel()
            {
                Name = "aaa",
                Age = 7,
                BornDate = DateTime.Now,
                Address = "....."
            });
            return Ok(result);
        }
        /// <summary>
        /// PostJsonDownload
        /// </summary>
        /// <remarks>
        /// PostJsonDownload
        /// </remarks>
        /// <param name="inputModel"></param>
        /// <response code="200">OK</response>
        /// <response code="400">BadRequest</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/download", "text/plain")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostJsonDownload([FromQuery] PostJsonDownloadInputModel inputModel)
        {
            Response.ContentType = "application/download";
            Response.Headers.Add("content-disposition", "attachment; filename=file.txt");
            await Response.WriteAsync("abcd", Encoding.UTF8);
            return Ok();
        }
    }
}
