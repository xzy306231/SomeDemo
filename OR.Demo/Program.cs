using Google.OrTools.Init;
using Google.OrTools.LinearSolver;

namespace OR.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Google.OrTools version: " + OrToolsVersion.VersionString());

            // Create the linear solver with the GLOP backend.
            Solver solver = Solver.CreateSolver("GLOP");
            if (solver is null)
            {
                Console.WriteLine("Could not create solver GLOP");
                return;
            }

            // Create the variables x and y.
            Variable x = solver.MakeNumVar(0.0, 1.0, "x");
            Variable y = solver.MakeNumVar(0.0, 2.0, "y");

            Console.WriteLine("Number of variables = " + solver.NumVariables());

            // Create a linear constraint, x + y <= 2.
            Constraint constraint = solver.MakeConstraint(double.NegativeInfinity, 2.0, "constraint");
            constraint.SetCoefficient(x, 1);
            constraint.SetCoefficient(y, 1);

            Console.WriteLine("Number of constraints = " + solver.NumConstraints());

            // Create the objective function, 3 * x + y.
            Objective objective = solver.Objective();
            objective.SetCoefficient(x, 3);
            objective.SetCoefficient(y, 1);
            objective.SetMaximization();

            Console.WriteLine("Solving with " + solver.SolverVersion());
            Solver.ResultStatus resultStatus = solver.Solve();

            Console.WriteLine("Status: " + resultStatus);
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                Console.WriteLine("The problem does not have an optimal solution!");
                if (resultStatus == Solver.ResultStatus.FEASIBLE)
                {
                    Console.WriteLine("A potentially suboptimal solution was found");
                }
                else
                {
                    Console.WriteLine("The solver could not solve the problem.");
                    return;
                }
            }

            Console.WriteLine("Solution:");
            Console.WriteLine("Objective value = " + solver.Objective().Value());
            Console.WriteLine("x = " + x.SolutionValue());
            Console.WriteLine("y = " + y.SolutionValue());

            Console.WriteLine("Advanced usage:");
            Console.WriteLine("Problem solved in " + solver.WallTime() + " milliseconds");
            Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");

            Console.ReadKey();
        }
    }
}
