using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public enum Status
    {
        Idle,
        MoveIn,
        Print,
        MoveOut
    }

    public enum Axis
    {
        X, Y, Z
    }
}
