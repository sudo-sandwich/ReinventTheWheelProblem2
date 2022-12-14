using System;
using System.Collections.Generic;
using System.Text;

namespace ReinventTheWheelProblem2 {
    class StartPoint {
        public string Name { get; set; }

        public int InitialTires { get; set; }

        public Path DefaultPath { get; set; }

        public HashSet<string> EvNames { get; set; }
        public HashSet<string> DvNames { get; set; }

        public Dictionary<EndPoint, Path> EvPaths { get; set; }
        public Dictionary<EndPoint, Path> DvPaths { get; set; }

        public int CurrentTires { get; set; }
        public int EvInUse { get; set; }
        public int DvInUse { get; set; }

        public StartPoint() {
            EvNames = new();
            DvNames = new();

            EvPaths = new();
            DvPaths = new();

            EvInUse = 0;
            DvInUse = 0;
        }

        /*
        public StartPoint(string name, Path defaultPath, string[] evNames, string[] dvNames, Dictionary<EndPoint, Path> electricVehicle, Dictionary<EndPoint, Path> dieselVehicle) {
            Name = name;
            DefaultPath = defaultPath;
            EvNames = evNames;
            DvNames = dvNames;
            ElectricVehicle = electricVehicle;
            DieselVehicle = dieselVehicle;

            EvInUse = 0;
            DvInUse = 0;
        }
        */

        public void Reset() {
            foreach (Path path in EvPaths.Values) {
                path.Tires.Clear();
            }

            foreach (Path path in DvPaths.Values) {
                path.Tires.Clear();
            }

            CurrentTires = InitialTires;
            DefaultPath.Tires.Clear();
            EvInUse = 0;
            DvInUse = 0;
        }

        public void UseEv(EndPoint ep, int tires) {
            //Console.WriteLine($"UseEv: {ep.Name}, {tires}");
            /*
            if (EvPaths[ep].Tires != 0) {
                throw new Exception("Path already in use.");
            }
            */
            /*
            if (ep.Path.Tires + tires > ep.MaxTires) {
                throw new Exception($"End point does not enough room for {tires} tires. Current: {ep.Path.Tires}, Max: {ep.MaxTires}");
            }
            */
            if (EvInUse >= EvNames.Count) {
                throw new Exception("No more electric vehicles.");
            }
            if (tires > CurrentTires) {
                throw new Exception("Not enough tires.");
            }

            EvPaths[ep].Tires.Add(tires);
            ep.Path.Tires.Add(tires);
            CurrentTires -= tires;
            EvInUse++;
        }

        public void UseDv(EndPoint ep, int tires) {
            /*
            if (DvPaths[ep].Tires != 0) {
                throw new Exception("Path already in use.");
            }
            */
            /*
            if (ep.Path.Tires + tires > ep.MaxTires) {
                throw new Exception($"End point does not enough room for {tires} tires. Current: {ep.Path.Tires}, Max: {ep.MaxTires}");
            }
            */
            if (DvInUse >= DvNames.Count) {
                throw new Exception("No more diesel vehicles.");
            }
            if (tires > CurrentTires) {
                throw new Exception("Not enough tires.");
            }

            DvPaths[ep].Tires.Add(tires);
            ep.Path.Tires.Add(tires);
            CurrentTires -= tires;
            DvInUse++;
        }

        public double CalculateCost() {
            double cost = 0;

            foreach (Path path in EvPaths.Values) {
                cost += path.Cost;
            }

            foreach (Path path in DvPaths.Values) {
                cost += path.Cost;
            }

            DefaultPath.Tires.Add(CurrentTires);
            cost += DefaultPath.Cost;

            return cost;
        }

        public double PredictCost(int tires) {
            return DefaultPath.PredictCost(tires);
        }
    }
}
