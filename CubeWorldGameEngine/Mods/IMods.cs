using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeWorldGameEngine.Mods
{
    public interface IMods
    {
        string Name { get; }
        Version Version { get; }
        Helpers.ModsTypeEnum.ModsTypes ModType { get; }
    }
}
