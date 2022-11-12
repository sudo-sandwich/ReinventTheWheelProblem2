using System;
using System.Collections.Generic;
using System.Text;

namespace ReinventTheWheelProblem2 {
    class Path {
        public double BaseGhg { get; set; }
        public double TireGhg { get; set; }
        public int MaxTires { get; set; }

        private int _tires;
        public int Tires { 
            get {
                return _tires;
            }
            set {
                if (value > MaxTires) {
                    throw new Exception("Value over MaxTires.");
                }
                _tires = value;
            }
        }

        public double Cost => PredictCost(Tires);

        public Path() {
            Tires = 0;
        }

        /*
        public Path(double baseGhg, double tireGhg, int maxTires) {
            BaseGhg = baseGhg;
            TireGhg = tireGhg;
            MaxTires = maxTires;

            Tires = 0;
        }
        */

        public double PredictCost(int tires) {
            return tires == 0 ? 0 : BaseGhg + TireGhg * tires;
        }
    }
}
