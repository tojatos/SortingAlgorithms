using System.ComponentModel;

namespace SortingAlgorithms
{
    public enum SequenceType
    {
        [Description("R")]
        Random,
        [Description("HS")]
        HalfSorted,
        [Description("S")]
        Sorted,
        [Description("RS")]
        ReverseSorted,
    }
}