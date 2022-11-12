using System;

namespace ReinventTheWheelProblem2 {
    class Program {
        static void Main(string[] args) {
            Model model = ModelCreator.Create(args[0], args[1], args[2], args[3], args[4]);
        }
    }
}
