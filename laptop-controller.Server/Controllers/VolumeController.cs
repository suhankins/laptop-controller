using AudioSwitcher.AudioApi.CoreAudio;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Versioning;

namespace laptop_controller.Server.Controllers
{

    [SupportedOSPlatform("windows")]
    [ApiController]
    [Route("[controller]")]
    public class VolumeController : ControllerBase
    {
        private readonly ILogger<VolumeController> _logger;

        public VolumeController(ILogger<VolumeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetVolume")]
        public IActionResult Get()
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            return Ok(defaultPlaybackDevice.Volume);
        }


        [HttpPost(Name = "SetVolume")]
        public IActionResult Set(byte volume)
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = volume;
            return Ok();
        }
    }
}
