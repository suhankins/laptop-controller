using Microsoft.AspNetCore.Mvc;
using System.Management;
using System.Runtime.Versioning;

namespace laptop_controller.Server.Controllers
{

    [SupportedOSPlatform("windows")]
    [ApiController]
    [Route("[controller]")]
    public class ScreenBrightnessController : ControllerBase
    {
        private readonly ILogger<ScreenBrightnessController> _logger;

        public ScreenBrightnessController(ILogger<ScreenBrightnessController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetScreenBrightness")]
        public IActionResult Get()
        {
            ManagementClass mclass = new ManagementClass("WmiMonitorBrightness");
            mclass.Scope = new ManagementScope(@"\\.\root\wmi");
            ManagementObjectCollection instances = mclass.GetInstances();

            byte? currentBrightness = null;

            foreach (ManagementObject instance in instances)
            {
                currentBrightness = (byte)instance.GetPropertyValue("CurrentBrightness");
                instance.Dispose();
            }
            mclass.Dispose();
            instances.Dispose();

            if (currentBrightness == null)
            {
                return StatusCode(500);
            }
            return Ok(currentBrightness);
        }


        [HttpPost(Name = "SetScreenBrightness")]
        public IActionResult SetBrightness(byte brightness)
        {
            if (brightness < 0)
            {
                brightness = 0;
            }
            else if (brightness > 100)
            {
                brightness = 100;
            }
            ManagementClass mclass = new ManagementClass("WmiMonitorBrightnessMethods");
            mclass.Scope = new ManagementScope(@"\\.\root\wmi");
            ManagementObjectCollection instances = mclass.GetInstances();

            foreach (ManagementObject instance in instances)
            {
                uint timeout = 0;
                object[] args = [timeout, brightness];
                instance.InvokeMethod("WmiSetBrightness", args);
                instance.Dispose();
            }
            mclass.Dispose();
            instances.Dispose();

            return Ok(0);
        }
    }
}
