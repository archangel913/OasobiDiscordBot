using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Modules;
public class ModuleBase : InteractionModuleBase
{
    public ModuleBase(IServiceProvider services) 
    {
        this.Services = services;
    }

    protected IServiceProvider Services { get; }
}
