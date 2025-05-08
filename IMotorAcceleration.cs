using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public interface IMotorAcceleration
    {
        public void AccelerateMotor(double acc);
        public void DeccelerateMotor(double acc);
    }
}
