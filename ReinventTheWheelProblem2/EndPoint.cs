using System;
using System.Collections.Generic;
using System.Text;

namespace ReinventTheWheelProblem2 {
    class EndPoint {
        public string Name { get; set; }

        public Path Path { get; set; }

        /*
        public EndPoint(string name, Path path) {
            Name = name;
            Path = path;
        }
        */

        public void Reset() {
            Path.Tires.Clear();
        }

        public double CalculateCost() {
            return Path.Cost;
        }

        public double PredictCost(int tires) {
            return Path.PredictCost(tires);
        }
    }
}
