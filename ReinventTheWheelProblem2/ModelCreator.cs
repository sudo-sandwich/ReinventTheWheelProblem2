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
        private record Capacity(string Location, int MaxTireCapacity);
        private record Distances(string DcName, string Destination, int Distance);
        private record SecondLeg(string Location, double BaseGhg, double TireGhg);
        private record TotalTires(string Location, int Tires);
        private record Vehicles(string DcName, string VehicleName, string VehicleType, double BaseGhgPerMile, double TireGhgPerMile, int MaxTireCapacity);

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
            IEnumerable<SecondLeg> secondLeg = secondLegCsv.GetRecords<SecondLeg>();
            IEnumerable<TotalTires> totalTires = totalTiresCsv.GetRecords<TotalTires>();
            IEnumerable<Vehicles> vehicles = vehiclesCsv.GetRecords<Vehicles>();

            Dictionary<string, StartPoint> startPoints = new();
            Dictionary<string, EndPoint> endPoints = new();

            foreach (TotalTires tt in totalTires) {
                startPoints.Add(tt.Location, new StartPoint() {
                    Name = tt.Location,
                    Tires = tt.Tires
                });
            }

            foreach (Capacity c in capacities) {
                endPoints.Add(c.Location, new EndPoint() { 
                    Name = c.Location,
                    Path = new Path() { MaxTires = c.MaxTireCapacity }
                });
            }

            foreach (StartPoint sp in startPoints.Values) {
                foreach (EndPoint ep in endPoints.Values) {

                }
            }

            return null;
        }
    }
}
