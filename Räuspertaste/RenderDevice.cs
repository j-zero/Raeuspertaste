using CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Räuspertaste
{
    public class RenderDevice
    {
        public readonly string Name;
        public readonly MMDevice Device;

        public RenderDevice(MMDevice device)
        {
            Device = device;
            Name = device.Description + " (" + device.FriendlyName + ")";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
