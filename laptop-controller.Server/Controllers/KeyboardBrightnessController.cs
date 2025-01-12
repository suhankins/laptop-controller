using AudioSwitcher.AudioApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace laptop_controller.Server.Controllers
{

    [SupportedOSPlatform("windows")]
    [ApiController]
    [Route("[controller]")]
    public class KeyboardBrightnessController : ControllerBase
    {
        private readonly ILogger<KeyboardBrightnessController> _logger;
        private readonly IntPtr _handle;

        public KeyboardBrightnessController(ILogger<KeyboardBrightnessController> logger)
        {
            _logger = logger;
            _handle = CreateFile(
                @"\\.\\ATKACPI",
                0x80000000 | 0x40000000, // GENERIC_READ | GENERIC_WRITE
                1 | 2, // FILE_SHARE_READ | FILE_SHARE_WRITE
                IntPtr.Zero,
                3, // OPEN_EXISTING
                0x80, // FILE_ATTRIBUTE_NORMAL
                IntPtr.Zero
            );
        }


        [HttpPost(Name = "SetKeyboardBrightness")]
        public IActionResult Set(uint brightness)
        {
            if (brightness > 3)
            {
                brightness = 3;
            }
            const uint DEVS = 0x53564544;
            const uint TUF_KB_BRIGHTNESS = 0x00050021;
            const uint CONTROL_CODE = 0x0022240C;

            byte[] acpiBuf = new byte[16];
            byte[] outBuffer = new byte[16];

            BitConverter.GetBytes(DEVS).CopyTo(acpiBuf, 0);
            BitConverter.GetBytes(8).CopyTo(acpiBuf, 4);
            BitConverter.GetBytes(TUF_KB_BRIGHTNESS).CopyTo(acpiBuf, 8);
            BitConverter.GetBytes(0x80 | (brightness & 0x7F)).CopyTo(acpiBuf, 12);

            uint lpBytesReturned = 0;
            DeviceIoControl(
                _handle,
                CONTROL_CODE,
                acpiBuf,
                (uint)acpiBuf.Length,
                outBuffer,
                (uint)outBuffer.Length,
                ref lpBytesReturned,
                IntPtr.Zero
            );

            int result = BitConverter.ToInt32(outBuffer, 0);

            _logger.Log(LogLevel.Information, "SetKeyboardBrightness: {}", result);

            return Ok();
        }

        ~KeyboardBrightnessController() {
            CloseHandle(_handle);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            byte[] lpInBuffer,
            uint nInBufferSize,
            byte[] lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}
