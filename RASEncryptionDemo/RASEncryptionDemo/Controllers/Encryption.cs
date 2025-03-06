using Microsoft.AspNetCore.Mvc;
using RASEncryptionDemo.Service;

namespace RASEncryptionDemo.Controllers
{
    [Route("api/encryption")]
    [ApiController]
    public class EncryptionController:ControllerBase
    {
        private readonly RSAService _rsaService;

        public EncryptionController()
        {
            _rsaService = new RSAService();
        }

        [HttpPost("encrypt")]
        public IActionResult Encrypt([FromBody] string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return BadRequest("Input cannot be empty");

            string encryptedData = _rsaService.Encrypt(plaintext);
            return Ok(new { EncryptedText = encryptedData });
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt([FromBody] string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return BadRequest("Input cannot be empty");

            string decryptedData = _rsaService.Decrypt(encryptedText);
            return Ok(new { DecryptedText = decryptedData });
        }
    }
}
