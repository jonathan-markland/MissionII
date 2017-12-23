using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public struct GameTimeSpan
    {
        public double Milliseconds;
        public static GameTimeSpan operator-(GameTimeSpan lhs, GameTimeSpan rhs)
        {
            return new GameTimeSpan { Milliseconds = lhs.Milliseconds - rhs.Milliseconds };
        }
    }
}
