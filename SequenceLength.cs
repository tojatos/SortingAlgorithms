using System.ComponentModel;

namespace SortingAlgorithms
{
    public enum SequenceLength
    {
        [Description("HT")]
        HundredThousand = 100000,
        [Description("FHT")]
        FiveHundredThousand = 500000,
        [Description("M")]
        Million = 1000000,
        [Description("TM")]
        TwoMillion = 2000000,
    }
}