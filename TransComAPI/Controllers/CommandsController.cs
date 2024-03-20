using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransComAPI.Models;
using TransComAPI.Services;

namespace TransComAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly SerialService _serialService;

        public CommandsController(SerialService serialService)
        {
            _serialService = serialService;
        }

        [HttpPost("sendCommandWithTextResponse")]
        public async Task<IActionResult> SendCommandWithTextResponse([FromBody] CommandRequest commandRequest)
        {
            if (commandRequest == null || string.IsNullOrWhiteSpace(commandRequest.Command))
            {
                return BadRequest("Invalid command request.");
            }

            var commandResponse = await _serialService.SendCommandForTextResponseAsync(commandRequest);

            if (commandResponse.Success)
            {
                return Ok(commandResponse);
            }
            else
            {
                return Problem(commandResponse.ErrorMessage);
            }
        }

        [HttpPost("sendCommandWithBinaryResponse")]
        public async Task<IActionResult> SendCommandWithBinaryResponse([FromBody] CommandRequest commandRequest)
        {
            if (commandRequest == null || string.IsNullOrWhiteSpace(commandRequest.Command))
            {
                return BadRequest("Invalid command request.");
            }

            var commandResponse = await _serialService.SendCommandForBinaryResponseAsync(commandRequest);

            if (commandResponse.Success)
            {
                return Ok(commandResponse);
            }
            else
            {
                return Problem(commandResponse.ErrorMessage);
            }
        }

        [HttpGet("autoDetectAndOpen")]
        public async Task<IActionResult> AutoDetectAndOpenTranComPort()
        {
            var result = await _serialService.AutoDetectAndOpenTranComPortAsync();

            if (result.IsDetected)
            {
                return Ok(new { Success = true, PortName = result.PortName, PingResponse = result.PingResponse });
            }
            else
            {
                return NotFound("TRAN device not found.");
            }
        }

        [HttpGet("status")]
        public IActionResult GetConnectionStatus()
        {
            var isOpen = _serialService.IsOpen();
            return Ok(new { PortOpen = isOpen });
        }

        [HttpPost("close")]
        public IActionResult CloseConnection()
        {
            _serialService.CloseConnection();
            return Ok(new { Message = "Connection closed successfully." });
        }
    }
}
