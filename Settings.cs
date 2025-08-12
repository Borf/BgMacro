using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgMacro;
public class Settings
{
    public string className { get; set; }
    public string windowTitle { get; set; }
    public int interval { get; set; } = 60;
    public string key { get; set; } = "VK_TAB";
    public bool autoStart { get; set; } = false;
}
