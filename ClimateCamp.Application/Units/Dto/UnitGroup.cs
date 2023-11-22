using System.Collections.Generic;

namespace ClimateCamp.Application
{
    public class UnitGroup
    {
        public string label { get; set; }
        public int value { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string label { get; set; }
        public int value { get; set; }
    }
}
