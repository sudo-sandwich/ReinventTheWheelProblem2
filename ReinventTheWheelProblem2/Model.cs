using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinventTheWheelProblem2 {
    class Model {
        public ICollection<StartPoint> StartPoints { get; set; }
        public ICollection<EndPoint> EndPoints { get; set; }

        public void Reset() {
            foreach (StartPoint sp in StartPoints) {
                sp.Reset();
            }

            foreach (EndPoint ep in EndPoints) {
                ep.Reset();
            }
        }

        public double CalculateCost() {
            double cost = 0;

            foreach (StartPoint sp in StartPoints) {
                cost += sp.CalculateCost();
            }

            foreach (EndPoint ep in EndPoints) {
                cost += ep.CalculateCost();
            }

            return cost;
        }
    }
}
