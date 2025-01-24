using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Versioning;

namespace laptop_controller.Server.Controllers
{

    [SupportedOSPlatform("windows")]
    [ApiController]
    [Route("[controller]")]
    public class VolumeController : ControllerBase
    {
        private static readonly CoreAudioController coreAudioController = new CoreAudioController();
        private readonly ILogger<VolumeController> _logger;

        public VolumeController(ILogger<VolumeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetVolume")]
        public IActionResult Get()
        {
            return Ok(coreAudioController.GetDefaultDevice(DeviceType.Playback, Role.Multimedia).Volume);
        }


        [HttpPost(Name = "SetVolume")]
        async public Task<IActionResult> Set(byte volume)
        {
            await coreAudioController.GetDefaultDevice(DeviceType.Playback, Role.Multimedia).SetVolumeAsync(volume);
            return Ok();
        }
    }
}
