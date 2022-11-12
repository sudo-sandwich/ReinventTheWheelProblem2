using System;
using System.Collections.Generic;
using System.Text;

namespace ReinventTheWheelProblem2 {
    class StartPoint {
        public string Name { get; set; }

        public int Tires { get; set; }

        public Path DefaultPath { get; set; }

        public HashSet<string> EvNames { get; set; }
        public HashSet<string> DvNames { get; set; }

        public Dictionary<EndPoint, Path> ElectricVehicle { get; set; }
        public Dictionary<EndPoint, Path> DieselVehicle { get; set; }

        public int CurrentTires { get; set; }
        public int EvInUse { get; set; }
        public int DvInUse { get; set; }

        public StartPoint() {
            EvNames = new();
            DvNames = new();

            ElectricVehicle = new();
            DieselVehicle = new();

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
            foreach (Path path in ElectricVehicle.Values) {
                path.Tires = 0;
            }

            foreach (Path path in DieselVehicle.Values) {
                path.Tires = 0;
            }

            CurrentTires = Tires;
            EvInUse = 0;
            DvInUse = 0;
        }

        public void UseEv(EndPoint ep, int tires) {
            if (ElectricVehicle[ep].Tires != 0) {
                throw new Exception("Path already in use.");
            }
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

            ElectricVehicle[ep].Tires = tires;
            ep.Path.Tires += tires;
            CurrentTires -= tires;
            EvInUse++;
        }

        public void UseDv(EndPoint ep, int tires) {
            if (DieselVehicle[ep].Tires != 0) {
                throw new Exception("Path already in use.");
            }
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

            DieselVehicle[ep].Tires = tires;
            ep.Path.Tires += tires;
            CurrentTires -= tires;
            DvInUse++;
        }

        public double CalculateCost() {
            double cost = 0;

            foreach (Path path in ElectricVehicle.Values) {
                cost += path.Cost;
            }

            foreach (Path path in DieselVehicle.Values) {
                cost += path.Cost;
            }

            DefaultPath.Tires = CurrentTires;
            cost += DefaultPath.Cost;

            return cost;
        }
    }
}
