using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeWorldGameEngine.Helpers
{
    [Serializable]
    public class BaseBlocksList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<BaseBlocksInfo> BaseBlocks { get; set; }

        public BaseBlocksList()
        {
            ID = 0;
            Name = "";
            BaseBlocks = new List<BaseBlocksInfo>();
        }
    }
}
