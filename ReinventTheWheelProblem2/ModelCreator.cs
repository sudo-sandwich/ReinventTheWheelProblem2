using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinventTheWheelProblem2 {
    class ModelCreator {
        private record Capacity(string location, int max_tire_capacity);
        private record Distances(string dc_name, string destination, int distance);
        private record SecondLeg(string location, double base_ghg, double additional_ghg_per_tire);
        private record TotalTires(string dc_name, int tires);
        private record Vehicle(string dc_name, string vehicle_name, string vehicle_type, double base_ghg_per_mile, double extra_ghg_per_tire_per_mile, int max_tire_capacity);

        public static Model Create(string capacityPath, string distancesPath, string secondLegPath, string totalTiresPath, string vehiclesPath) {
            using StreamReader capacityStream = File.OpenText(capacityPath);
            using StreamReader distancesStream = File.OpenText(distancesPath);
            using StreamReader secondLegStream = File.OpenText(secondLegPath);
            using StreamReader totalTiresStream = File.OpenText(totalTiresPath);
            using StreamReader vehiclesStream = File.OpenText(vehiclesPath);
            using CsvReader capacityCsv = new CsvReader(capacityStream, CultureInfo.InvariantCulture);
            using CsvReader distancesCsv = new CsvReader(distancesStream, CultureInfo.InvariantCulture);
            using CsvReader secondLegCsv = new CsvReader(secondLegStream, CultureInfo.InvariantCulture);
            using CsvReader totalTiresCsv = new CsvReader(totalTiresStream, CultureInfo.InvariantCulture);
            using CsvReader vehiclesCsv = new CsvReader(vehiclesStream, CultureInfo.InvariantCulture);
            IEnumerable<Capacity> capacities = capacityCsv.GetRecords<Capacity>();
            IEnumerable<Distances> distances = distancesCsv.GetRecords<Distances>();
            IEnumerable<SecondLeg> secondLegs = secondLegCsv.GetRecords<SecondLeg>();
            IEnumerable<TotalTires> totalTires = totalTiresCsv.GetRecords<TotalTires>();
            IEnumerable<Vehicle> vehicles = vehiclesCsv.GetRecords<Vehicle>();

            Dictionary<string, StartPoint> startPoints = new();
            Dictionary<string, EndPoint> endPoints = new();

            foreach (TotalTires tt in totalTires) {
                startPoints.Add(tt.dc_name, new StartPoint() {
                    Name = tt.dc_name,
                    Tires = tt.tires
                });
            }

            foreach (Capacity c in capacities) {
                endPoints.Add(c.location, new EndPoint() { 
                    Name = c.location,
                    Path = new Path() { MaxTires = c.max_tire_capacity }
                });
            }

            foreach (SecondLeg sl in secondLegs) {
                if (startPoints.Keys.Contains(sl.location)) {
                    startPoints[sl.location].DefaultPath = new Path() {
                        BaseGhg = sl.base_ghg,
                        TireGhg = sl.additional_ghg_per_tire,
                        MaxTires = int.MaxValue
                    };
                } else if (endPoints.Keys.Contains(sl.location)) {
                    endPoints[sl.location].Path.BaseGhg = sl.base_ghg;
                    endPoints[sl.location].Path.TireGhg = sl.additional_ghg_per_tire;
                }
            }

            double EvBaseGhg = -1;
            double EvTireGhg = -1;
            int EvMaxTires = -1;
            double DvBaseGhg = -1;
            double DvTireGhg = -1;
            int DvMaxTires = -1;

            foreach (Vehicle v in vehicles) {
                if (v.vehicle_type == "EV") {
                    if (EvBaseGhg == -1) {
                        EvBaseGhg = v.base_ghg_per_mile;
                        EvTireGhg = v.extra_ghg_per_tire_per_mile;
                        EvMaxTires = v.max_tire_capacity;
                    }

                    startPoints[v.dc_name].EvNames.Add(v.vehicle_name);
                }
                if (v.vehicle_type == "DIESEL") {
                    if (DvBaseGhg == -1) {
                        DvBaseGhg = v.base_ghg_per_mile;
                        DvTireGhg = v.extra_ghg_per_tire_per_mile;
                        DvMaxTires = v.max_tire_capacity;
                    }

                    startPoints[v.dc_name].DvNames.Add(v.vehicle_name);
                }
            }

            foreach (Distances d in distances) {
                startPoints[d.dc_name].ElectricVehicle.Add(endPoints[d.destination], new Path() {
                    BaseGhg = EvBaseGhg * d.distance,
                    TireGhg = EvTireGhg * d.distance,
                    MaxTires = EvMaxTires
                });
                startPoints[d.dc_name].DieselVehicle.Add(endPoints[d.destination], new Path() {
                    BaseGhg = DvBaseGhg * d.distance,
                    TireGhg = DvTireGhg * d.distance,
                    MaxTires = DvMaxTires
                });
            }

            Model model = new() {
                StartPoints = startPoints.Values,
                EndPoints = endPoints.Values
            };

            return model;
        }
    }
}
