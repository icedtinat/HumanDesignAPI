using System;

namespace HumanDesignClean
{
    public record ChartDTO(
        string Type,
        string Profile,
        int[] GateNumbers,
        string[] ActiveChannels,
        string[] DefinedCenters
    );
}
