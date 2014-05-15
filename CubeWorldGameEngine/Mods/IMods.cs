using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CubeWorldGameEngine.Helpers;

namespace CubeWorldGameEngine.Mods
{
    public interface IMods
    {
        string Name { get; }
        Version Version { get; }
        ModsInfo.ModsTypes ModType { get; }

        void Init();

    }
}
