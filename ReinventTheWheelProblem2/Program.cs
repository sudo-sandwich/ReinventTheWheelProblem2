using System;

namespace ReinventTheWheelProblem2 {
    class Program {
        static void Main(string[] args) {
            Model model = ModelCsv.Create(args[0], args[1], args[2], args[3], args[4]);
            ModelCsv.Export(Greedy.Run(model), System.IO.Path.Combine(args[5], "greedy.csv"));
        }
    }
}
