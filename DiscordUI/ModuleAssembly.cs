using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordUI;
public static class ModuleAssembly
{
    public static Assembly Get() => Assembly.GetExecutingAssembly();
}
