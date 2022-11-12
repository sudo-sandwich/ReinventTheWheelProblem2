using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinventTheWheelProblem2 {
    class Path {
        public double BaseGhg { get; set; }
        public double TireGhg { get; set; }
        public int MaxTires { get; set; }

        public List<int> Tires { get; set; }
        public int TotalTires => Tires.Sum();

        /*
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
        */

        public double Cost => PredictCost(TotalTires);

        public Path() {
            Tires = new();
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
