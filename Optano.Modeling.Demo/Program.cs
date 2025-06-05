using OPTANO.Modeling.Optimization.Enums;
using OPTANO.Modeling.Optimization;
using OPTANO.Modeling.Optimization.Solver.Gurobi1000;
using OPTANO.Modeling.Common;
using OPTANO.Modeling.Optimization.Configuration;

namespace Optano.Modeling.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Sample1();
           // Sample2();
            Sample3();
            Console.ReadKey();
        }

        private static void Sample3()
        {
            // create example Nodes
            INode pb = new Node("Paderborn", 50);
            INode ny = new Node("New York", -100);
            INode b = new Node("Beijing", 50);
            INode sp = new Node("São Paulo", 50);
            INode sf = new Node("San Francisco", -50);
            // assign these nodes to a list of INodes
            var nodes = new List<INode> { pb, ny, b, sp, sf };

            // create example Edges
            IEdge one = new Edge(ny, pb, null, 3, 6100);
            IEdge two = new Edge(b, ny, null, 2, 11000);
            IEdge three = new Edge(sp, b, 75, 1, 17600);
            IEdge four = new Edge(sf, sp, null, 4, 10500);
            IEdge five = new Edge(sp, pb, null, 5, 9900);
            IEdge six = new Edge(pb, b, 50, 1, 7600);

            // assign these edges to a list of IEdges
            var edges = new List<IEdge> { one, two, three, four, five, six };

            // Use long names for easier debugging/model understanding.
            var config = new Configuration();
            config.NameHandling = NameHandlingStyle.UniqueLongNames;
            config.ComputeRemovedVariables = true;
            using (var scope = new ModelScope(config))
            {
                // create a model, based on given data and the model scope
                var designModel = new NetworkDesignModel(nodes, edges);

                // Get a solver instance, change your solver
                using (var solver = new GurobiSolver())
                {
                    // solve the model
                    var solution = solver.Solve(designModel.Model);

                    // import the results back into the model 
                    designModel.Model.VariableCollections.ForEach(vc => vc.SetVariableValues(solution.VariableValues));

                    // print objective and variable decisions
                    Console.WriteLine($"{solution.ObjectiveValues.Single()}");
                    designModel.x.Variables.ForEach(x => Console.WriteLine($"{x.ToString().PadRight(36)}: {x.Value}"));
                    designModel.y.Variables.ForEach(y => Console.WriteLine($"{y.ToString().PadRight(36)}: {y.Value}"));

                    designModel.Model.VariableStatistics.WriteCSV(AppDomain.CurrentDomain.BaseDirectory);
                    Console.ReadLine();
                }
            }
        }
        private static void Sample2()
        {
            using (var scope = new ModelScope())
            {
                var model = new Model();
                var x = new Variable("x");
                var y = new Variable("y");
                model.AddConstraint(x + y >= 120);
                model.AddObjective(new Objective(2 * x + 3 * y));

                using (var solver = new GurobiSolver())
                {
                    var solution = solver.Solve(model);
                }
            }
        }
        private static void Sample1()
        {
            double A = 32;
            double F = 68;
            double M = 45;
            double G = 89;
            double V = 301;

            double Sum = A + F - M - G + V;

            // 创建优化模型
            Model model = new Model();

            // 添加变量，这里参数不明白的可以转到定义中去看一下官方注释
            var X = new Variable("X", 23, double.PositiveInfinity, VariableType.Continuous); //A
            var Y = new Variable("Y", 52, 124, VariableType.Continuous);//B
            model.AddVariable(X);
            model.AddVariable(Y);

            // 添加目标函数，目标函数什么意思，猜一下，就是需要被算出最优解的函数，在我们的需求中是要使 X 最大，那么我们的求解目标就是找到 X 的最优解
            Objective objective = new Objective(X);
            //Maximize：最大，如果需求是 X 的最小呢？应该引用什么参数？Minimize：最小
            objective.Sense = ObjectiveSense.Maximize;

            //将目标函数添加进模型中
            model.AddObjective(objective);

            // 添加约束条件
            model.AddConstraint(X + Y == Sum);
            model.AddConstraint(Y >= 52);
            model.AddConstraint(Y <= 124);
            model.AddConstraint(X >= 23);

            // 求解线性规划问题
            var solver = new GurobiSolver(); // 使用Gurobi作为求解器，也可以选择其他求解器
            var solution = solver.Solve(model);

            // 输出解决方案
            Console.WriteLine($"优化状态: {solution.Status}");
            Console.WriteLine($"X 的最优解: {solution.GetVariableValue("A")}");
            Console.WriteLine($"Y 的最优解: {solution.GetVariableValue("B")}");
            // 在 model 中变量名是从 A 起始的，按照向 model 中添加变量的顺序依次从 A 往后面排
        }
    }
}
