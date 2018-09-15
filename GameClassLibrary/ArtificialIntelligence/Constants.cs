using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Constants
    {
        public const int SingleMindedMoveDurationCycles = 50;
        public const int SingleMindedFiringCyclesAndMask = 31;
        public const int FiringAttractorFiringCyclesAndMask = 7;
        public const int WanderingMineMoveDurationCycles = 20;
        public const int SingleMindedFiringProbabilityPercent = 20;
        public const int AttractorFiringProbabilityPercent = 40;
        public const int SingleMindedSpeedDivisor = 2;
        public const int FiringAttractorSpeedDivisor = 3;
        public const int WanderingMineSpeedDivisor = 4;
        public const int SwoopMovementCycles = 2;
    }
}
