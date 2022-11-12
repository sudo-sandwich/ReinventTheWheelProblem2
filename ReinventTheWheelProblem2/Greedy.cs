using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinventTheWheelProblem2 {
    class Greedy {
        public static ICollection<ModelCsv.Output> Run(Model model) {
            double minCost = double.MaxValue;
            ICollection<ModelCsv.Output> currentResult = null;

            // all permutations of start points
            IEnumerable<IEnumerable<StartPoint>> spPermutations = GetPermutations<StartPoint>(model.StartPoints, model.StartPoints.Count);

            foreach (IEnumerable<StartPoint> spPermutation in spPermutations) {
                foreach (StartPoint sp in spPermutation) {
                    // cost of leaving everything at the start point
                    double defaultCost = sp.PredictCost(sp.InitialTires);

                    // cost of shipping everything out of the start point
                    double shipCost = 0;
                     
                    // loop while we still have tires in the location and vehicles available
                    while (sp.CurrentTires > 0 && (sp.EvInUse < sp.EvNames.Count || sp.DvInUse < sp.DvNames.Count)) {
                        //Console.WriteLine($"Current sp: {sp.Name}, current tires: {sp.CurrentTires}, EvInUse: {sp.EvInUse}/{sp.EvNames.Count}, DvInUse: {sp.DvInUse}/{sp.DvNames.Count}");
                        // find the cheapest way to ship tires
                        ShipData shipEv = EmptyShipData;
                        ShipData shipDv = EmptyShipData;
                        // get min ship data, but skip if we have no vehicles of a certain type available
                        if (sp.EvInUse < sp.EvNames.Count) {
                            shipEv = GetMinShipCost(sp.EvPaths, sp.CurrentTires);
                        }
                        if (sp.DvInUse < sp.DvNames.Count) {
                            shipDv = GetMinShipCost(sp.DvPaths, sp.CurrentTires);
                        }

                        // use whichever is cheaper
                        if (shipEv.AvgTireCost < shipDv.AvgTireCost) {
                            double originalCost = shipEv.Ep.CalculateCost();
                            sp.UseEv(shipEv.Ep, shipEv.NumTires);
                            double newCost = shipEv.Ep.CalculateCost() + sp.EvPaths[shipEv.Ep].Cost;
                            shipCost += newCost - originalCost;
                        } else {
                            double originalCost = shipDv.Ep.CalculateCost();
                            sp.UseDv(shipDv.Ep, shipDv.NumTires);
                            double newCost = shipDv.Ep.CalculateCost() + sp.DvPaths[shipDv.Ep].Cost;
                            shipCost += newCost - originalCost;
                        }
                    }

                    // hopefully we never hit this line
                    if (defaultCost < shipCost) {
                        Console.WriteLine("default cost is lower than ship cost");
                    }
                }

                double totalCost = model.CalculateCost();
                if (totalCost < minCost) {
                    Console.WriteLine($"Found new minCost. old: {minCost}, new: {totalCost}");
                    minCost = totalCost;
                    currentResult = ModelCsv.GetResult(model);
                }
                model.Reset();
            }

            return currentResult;
        }

        private record ShipData(EndPoint Ep, int NumTires, double AvgTireCost);
        private static readonly ShipData EmptyShipData = new(null, -1, double.MaxValue);

        private static ShipData GetMinShipCost(Dictionary<EndPoint, Path> paths, int maxTires) {
            ShipData minShip = EmptyShipData;

            foreach (EndPoint currentEp in paths.Keys) {
                // skip if the end point is already full
                if (currentEp.Path.TotalTires == currentEp.Path.MaxTires) {
                    continue;
                }

                // smallest value of max vehicle load, tires at the start location, and room left at the end point
                int numTires = Math.Min(paths[currentEp].MaxTires, Math.Min(maxTires, currentEp.Path.MaxTires - currentEp.Path.TotalTires));
                double epCostDifference = currentEp.PredictCost(currentEp.Path.TotalTires + numTires) - currentEp.CalculateCost();
                double vehicleCostDifference = paths[currentEp].PredictCost(numTires) - paths[currentEp].Cost;
                double totalCost = epCostDifference + vehicleCostDifference;
                double avgCost = totalCost / numTires;

                if (avgCost < minShip.AvgTireCost) {
                    minShip = new(currentEp, numTires, avgCost);
                }
            }

            return minShip;
        }

        // yoink
        // https://stackoverflow.com/a/10630026
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length) {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
