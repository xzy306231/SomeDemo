using Google.OrTools.Init;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;

namespace OR.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GanttData();
            Console.ReadKey();
        }

        private static void GanttData()
        {
            var allJobs =
           new[] {
                new[] {
                    // job0
                    new { machine = 0, duration = 3 }, // task0
                    new { machine = 1, duration = 2 }, // task1
                    new { machine = 2, duration = 2 }, // task2
                }
                    .ToList(),
                new[] {
                    // job1
                    new { machine = 0, duration = 2 }, // task0
                    new { machine = 2, duration = 1 }, // task1
                    new { machine = 1, duration = 4 }, // task2
                }
                    .ToList(),
                new[] {
                    // job2
                    new { machine = 1, duration = 4 }, // task0
                    new { machine = 2, duration = 3 }, // task1
                }
                    .ToList(),
           }
               .ToList();

            int numMachines = 0;
            foreach (var job in allJobs)
            {
                foreach (var task in job)
                {
                    numMachines = Math.Max(numMachines, 1 + task.machine);
                }
            }
            int[] allMachines = Enumerable.Range(0, numMachines).ToArray();

            // Computes horizon dynamically as the sum of all durations.
            int horizon = 0;
            foreach (var job in allJobs)
            {
                foreach (var task in job)
                {
                    horizon += task.duration;
                }
            }

            // Creates the model.
            CpModel model = new CpModel();

            Dictionary<Tuple<int, int>, Tuple<IntVar, IntVar, IntervalVar>> allTasks =
                new Dictionary<Tuple<int, int>, Tuple<IntVar, IntVar, IntervalVar>>(); // (start, end, duration)
            Dictionary<int, List<IntervalVar>> machineToIntervals = new Dictionary<int, List<IntervalVar>>();
            for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
            {
                var job = allJobs[jobID];
                for (int taskID = 0; taskID < job.Count(); ++taskID)
                {
                    var task = job[taskID];
                    String suffix = $"_{jobID}_{taskID}";
                    IntVar start = model.NewIntVar(0, horizon, "start" + suffix);
                    IntVar end = model.NewIntVar(0, horizon, "end" + suffix);
                    IntervalVar interval = model.NewIntervalVar(start, task.duration, end, "interval" + suffix);
                    var key = Tuple.Create(jobID, taskID);
                    allTasks[key] = Tuple.Create(start, end, interval);
                    if (!machineToIntervals.ContainsKey(task.machine))
                    {
                        machineToIntervals.Add(task.machine, new List<IntervalVar>());
                    }
                    machineToIntervals[task.machine].Add(interval);
                }
            }

            // Create and add disjunctive constraints.
            foreach (int machine in allMachines)
            {
                model.AddNoOverlap(machineToIntervals[machine]);
            }

            // Precedences inside a job.
            for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
            {
                var job = allJobs[jobID];
                for (int taskID = 0; taskID < job.Count() - 1; ++taskID)
                {
                    var key = Tuple.Create(jobID, taskID);
                    var nextKey = Tuple.Create(jobID, taskID + 1);
                    model.Add(allTasks[nextKey].Item1 >= allTasks[key].Item2);
                }
            }

            // Makespan objective.
            IntVar objVar = model.NewIntVar(0, horizon, "makespan");

            List<IntVar> ends = new List<IntVar>();
            for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
            {
                var job = allJobs[jobID];
                var key = Tuple.Create(jobID, job.Count() - 1);
                ends.Add(allTasks[key].Item2);
            }
            model.AddMaxEquality(objVar, ends);
            model.Minimize(objVar);

            // Solve
            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);
            Console.WriteLine($"Solve status: {status}");

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                Console.WriteLine("Solution:");

                Dictionary<int, List<AssignedTask>> assignedJobs = new Dictionary<int, List<AssignedTask>>();
                for (int jobID = 0; jobID < allJobs.Count(); ++jobID)
                {
                    var job = allJobs[jobID];
                    for (int taskID = 0; taskID < job.Count(); ++taskID)
                    {
                        var task = job[taskID];
                        var key = Tuple.Create(jobID, taskID);
                        int start = (int)solver.Value(allTasks[key].Item1);
                        if (!assignedJobs.ContainsKey(task.machine))
                        {
                            assignedJobs.Add(task.machine, new List<AssignedTask>());
                        }
                        assignedJobs[task.machine].Add(new AssignedTask(jobID, taskID, start, task.duration));
                    }
                }

                // Create per machine output lines.
                String output = "";
                foreach (int machine in allMachines)
                {
                    // Sort by starting time.
                    assignedJobs[machine].Sort();
                    String solLineTasks = $"Machine {machine}: ";
                    String solLine = "           ";

                    foreach (var assignedTask in assignedJobs[machine])
                    {
                        String name = $"job_{assignedTask.jobID}_task_{assignedTask.taskID}";
                        // Add spaces to output to align columns.
                        solLineTasks += $"{name,-15}";

                        String solTmp = $"[{assignedTask.start},{assignedTask.start + assignedTask.duration}]";
                        // Add spaces to output to align columns.
                        solLine += $"{solTmp,-15}";
                    }
                    output += solLineTasks + "\n";
                    output += solLine + "\n";
                }
                // Finally print the solution found.
                Console.WriteLine($"Optimal Schedule Length: {solver.ObjectiveValue}");
                Console.WriteLine($"\n{output}");
            }
            else
            {
                Console.WriteLine("No solution found.");
            }

            Console.WriteLine("Statistics");
            Console.WriteLine($"  conflicts: {solver.NumConflicts()}");
            Console.WriteLine($"  branches : {solver.NumBranches()}");
            Console.WriteLine($"  wall time: {solver.WallTime()}s");
        }

        private void Process()
        {
            // ==================== 问题参数设置 ====================

            // 储罐信息
            var tanks = new[]
            {
                new { Id = 0, Name = "Tank-A", LiquidType = "阿曼", Capacity = 1000, CurrentLevel = 800 },
                new { Id = 1, Name = "Tank-B", LiquidType = "沙中", Capacity = 1500, CurrentLevel = 1200 },
                new { Id = 2, Name = "Tank-C", LiquidType = "沙中", Capacity = 800, CurrentLevel = 600 },
                new { Id = 3, Name = "Tank-D", LiquidType = "阿曼", Capacity = 1200, CurrentLevel = 900 },
                new { Id = 3, Name = "Tank-E", LiquidType = "阿曼", Capacity = 1200, CurrentLevel = 900 },
                new { Id = 3, Name = "Tank-F", LiquidType = "阿曼", Capacity = 1200, CurrentLevel = 900 }
            };

            // 加工设备信息
            var equipments = new[]
            {
                new { Id = 0, Name = "1#常减压", RequiredLiquid = "阿曼", BatchSize = 200, ProcessingTime = 120 },
                new { Id = 1, Name = "2#常减压", RequiredLiquid = "沙中", BatchSize = 250, ProcessingTime = 150 },
              //  new { Id = 2, Name = "Mixer-1", RequiredLiquid = "Chemical-C", BatchSize = 180, ProcessingTime = 100 },
              //  new { Id = 3, Name = "Distillator-1", RequiredLiquid = "Chemical-A", BatchSize = 220, ProcessingTime = 180 }
            };

            // 管线信息
            var pipelines = new[]
            {
                new { Id = 0, Name = "Main-Pipeline", Capacity = 1 } // 1表示同一时间只能有一个输送任务
            };

            // 时间参数（分钟）
            int horizon = 480; // 8小时工作制
            int transferTimePerUnit = 1; // 每单位液体输送时间（分钟）

            // ==================== 创建模型 ====================
            CpModel model = new CpModel();

            // ==================== 创建变量 ====================

            // 定义输送任务
            var transferTasks = new List<TransferTask>();
            int taskId = 0;

            // 为每个可能的储罐-设备组合创建任务（仅当液体类型匹配时）
            foreach (var tank in tanks)
            {
                foreach (var equipment in equipments)
                {
                    if (tank.LiquidType == equipment.RequiredLiquid &&
                        tank.CurrentLevel >= equipment.BatchSize)
                    {
                        transferTasks.Add(new TransferTask
                        {
                            Id = taskId++,
                            TankId = tank.Id,
                            EquipmentId = equipment.Id,
                            Volume = equipment.BatchSize,
                            TransferTime = equipment.BatchSize * transferTimePerUnit,
                            SetupTime = 15 // 管线切换准备时间
                        });
                    }
                }
            }

            Console.WriteLine($"创建了 {transferTasks.Count} 个输送任务");

            // 任务变量
            Dictionary<int, IntVar> startVars = new Dictionary<int, IntVar>();
            Dictionary<int, IntVar> endVars = new Dictionary<int, IntVar>();
            Dictionary<int, IntervalVar> intervalVars = new Dictionary<int, IntervalVar>();
            Dictionary<int, BoolVar> activeVars = new Dictionary<int, BoolVar>();

            foreach (var task in transferTasks)
            {
                // 开始时间变量 [0, horizon]
                startVars[task.Id] = model.NewIntVar(0, horizon, $"start_{task.Id}");

                // 结束时间变量
                endVars[task.Id] = model.NewIntVar(0, horizon, $"end_{task.Id}");

                // 任务是否被调度
                activeVars[task.Id] = model.NewBoolVar($"active_{task.Id}");

                // 持续时间 = 输送时间 + 准备时间（如果任务被激活）
                var duration = task.TransferTime + task.SetupTime;

                // 创建区间变量
                intervalVars[task.Id] = model.NewOptionalIntervalVar(
                    startVars[task.Id],
                    duration,
                    endVars[task.Id],
                    activeVars[task.Id],
                    $"interval_{task.Id}"
                );

                // 约束：如果任务被激活，结束时间 = 开始时间 + 持续时间
                model.Add(endVars[task.Id] == startVars[task.Id] + duration)
                    .OnlyEnforceIf(activeVars[task.Id]);

                // 如果任务不被激活，开始和结束时间设为0
                model.Add(startVars[task.Id] == 0).OnlyEnforceIf(activeVars[task.Id].Not());
                model.Add(endVars[task.Id] == 0).OnlyEnforceIf(activeVars[task.Id].Not());
            }

            // ==================== 添加约束 ====================

            // 约束1: 同一管线不能同时处理多个任务（无重叠约束）
            List<IntervalVar> pipelineIntervals = new List<IntervalVar>();
            foreach (var task in transferTasks)
            {
                pipelineIntervals.Add(intervalVars[task.Id]);
            }
            model.AddNoOverlap(pipelineIntervals);

            // 约束2: 同一储罐不能同时进行多个输送任务
            foreach (var tank in tanks)
            {
                var tankTasks = transferTasks.Where(t => t.TankId == tank.Id).ToList();
                if (tankTasks.Count > 1)
                {
                    List<IntervalVar> tankIntervals = new List<IntervalVar>();
                    foreach (var task in tankTasks)
                    {
                        tankIntervals.Add(intervalVars[task.Id]);
                    }
                    model.AddNoOverlap(tankIntervals);
                }
            }

            // 约束3: 同一设备不能同时接收多个输送任务
            foreach (var equipment in equipments)
            {
                var equipmentTasks = transferTasks.Where(t => t.EquipmentId == equipment.Id).ToList();
                if (equipmentTasks.Count > 1)
                {
                    List<IntervalVar> equipmentIntervals = new List<IntervalVar>();
                    foreach (var task in equipmentTasks)
                    {
                        equipmentIntervals.Add(intervalVars[task.Id]);
                    }
                    model.AddNoOverlap(equipmentIntervals);
                }
            }

            // 约束4: 储罐容量约束（输送后不能为负）
            foreach (var tank in tanks)
            {
                var tankTasks = transferTasks.Where(t => t.TankId == tank.Id).ToList();
                if (tankTasks.Count > 0)
                {
                    // 计算该储罐的总输出量
                    List<Google.OrTools.Sat.LinearExpr> tankTransfers = new List<Google.OrTools.Sat.LinearExpr>();
                    foreach (var task in tankTasks)
                    {
                        tankTransfers.Add(activeVars[task.Id] * task.Volume);
                    }
                    model.Add(Google.OrTools.Sat.LinearExpr.Sum(tankTransfers) <= tank.CurrentLevel);
                }
            }

            // 约束5: 设备处理约束（考虑设备处理时间）
            foreach (var equipment in equipments)
            {
                var equipmentTasks = transferTasks.Where(t => t.EquipmentId == equipment.Id).ToList();
                foreach (var task in equipmentTasks)
                {
                    // 输送完成后，设备需要处理时间
                    var processingEnd = model.NewIntVar(0, horizon, $"process_end_{task.Id}");
                    model.Add(processingEnd == endVars[task.Id] + equipment.ProcessingTime)
                        .OnlyEnforceIf(activeVars[task.Id]);
                }
            }

            // ==================== 设置目标函数 ====================

            // 目标1: 最小化总完成时间（makespan）
            IntVar makespan = model.NewIntVar(0, horizon, "makespan");
            foreach (var task in transferTasks)
            {
                model.Add(makespan >= endVars[task.Id]);
            }

            // 目标2: 最大化完成的任务数量
            IntVar numCompletedTasks = model.NewIntVar(0, transferTasks.Count, "num_completed_tasks");
            model.Add(numCompletedTasks == Google.OrTools.Sat.LinearExpr.Sum(activeVars.Values));

            // 多目标：主要最小化完成时间，次要最大化完成任务数
            // 通过加权组合
            int makespanWeight = 10;
            int tasksWeight = 1;

            Google.OrTools.Sat.LinearExpr objective = makespan * makespanWeight - numCompletedTasks * tasksWeight;
            model.Minimize(objective);

            // ==================== 求解 ====================

            CpSolver solver = new CpSolver();
            solver.StringParameters = "num_search_workers:8, max_time_in_seconds:30, log_search_progress: true";

            Console.WriteLine("开始求解...");
            CpSolverStatus status = solver.Solve(model);

            // ==================== 输出结果 ====================

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                Console.WriteLine("\n=== 储罐液体输送调度方案 ===");
                Console.WriteLine($"求解状态: {status}");
                Console.WriteLine($"总完成时间: {solver.Value(makespan)} 分钟");
                Console.WriteLine($"完成的任务数: {solver.Value(numCompletedTasks)} / {transferTasks.Count}");
                Console.WriteLine($"目标函数值: {solver.ObjectiveValue}");
                Console.WriteLine();

                // 按时间顺序输出任务调度
                //var scheduledTasks = transferTasks
                //    .Where(t => solver.Value(activeVars[t.Id]) == 1)
                //    .OrderBy(t => solver.Value(startVars[t.Id]))
                //    .ToList();

                var scheduledTasks = transferTasks
                    //   .Where(t => solver.Value(activeVars[t.Id]) == 1)
                    .OrderBy(t => solver.Value(startVars[t.Id]))
                    .ToList();

                Console.WriteLine("调度计划（按时间顺序）:");
                Console.WriteLine("时间\t\t储罐\t\t→\t设备\t\t体积\t持续时间");
                Console.WriteLine(new string('-', 80));

                foreach (var task in scheduledTasks)
                {
                    var tank = tanks.First(t => t.Id == task.TankId);
                    var equipment = equipments.First(e => e.Id == task.EquipmentId);
                    var start = solver.Value(startVars[task.Id]);
                    var end = solver.Value(endVars[task.Id]);

                    Console.WriteLine($"{start:000}-{end:000}\t{tank.Name}\t→\t{equipment.Name}\t{task.Volume}\t{task.TransferTime + task.SetupTime}min");
                }

                // 输出资源利用率
                Console.WriteLine("\n=== 资源利用率分析 ===");

                // 管线利用率
                var totalPipelineTime = scheduledTasks.Sum(t => solver.Value(endVars[t.Id]) - solver.Value(startVars[t.Id]));
                double pipelineUtilization = (double)totalPipelineTime / solver.Value(makespan) * 100;
                Console.WriteLine($"管线利用率: {pipelineUtilization:F1}%");

                // 储罐使用情况
                Console.WriteLine("\n储罐使用情况:");
                foreach (var tank in tanks)
                {
                    var tankTasks = scheduledTasks.Where(t => t.TankId == tank.Id).ToList();
                    int totalTransfer = tankTasks.Sum(t => t.Volume);
                    double utilization = (double)totalTransfer / tank.CurrentLevel * 100;

                    Console.WriteLine($"{tank.Name}: {totalTransfer}/{tank.CurrentLevel} ({utilization:F1}%) - {tankTasks.Count}个任务");
                }

                // 设备接收情况
                Console.WriteLine("\n设备接收情况:");
                foreach (var equipment in equipments)
                {
                    var equipmentTasks = scheduledTasks.Where(t => t.EquipmentId == equipment.Id).ToList();
                    Console.WriteLine($"{equipment.Name}: {equipmentTasks.Count}个输送任务");
                }
            }
            else
            {
                Console.WriteLine("未找到可行解");
            }

            // 输出求解统计
            Console.WriteLine("\n=== 求解统计 ===");
            Console.WriteLine($"求解时间: {solver.WallTime():F2}秒");
            Console.WriteLine($"目标值边界: {solver.BestObjectiveBound}");
            Console.WriteLine($"冲突次数: {solver.NumConflicts()}");
            Console.WriteLine($"分支次数: {solver.NumBranches()}");

            Console.ReadKey();
        }
    }

    public class AssignedTask : IComparable
    {
        public int jobID;
        public int taskID;
        public int start;
        public int duration;

        public AssignedTask(int jobID, int taskID, int start, int duration)
        {
            this.jobID = jobID;
            this.taskID = taskID;
            this.start = start;
            this.duration = duration;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            AssignedTask otherTask = obj as AssignedTask;
            if (otherTask != null)
            {
                if (this.start != otherTask.start)
                    return this.start.CompareTo(otherTask.start);
                else
                    return this.duration.CompareTo(otherTask.duration);
            }
            else
                throw new ArgumentException("Object is not a Temperature");
        }
    }

    public class TransferTask
    {
        public int Id { get; set; }
        public int TankId { get; set; }
        public int EquipmentId { get; set; }
        public int Volume { get; set; }
        public int TransferTime { get; set; }
        public int SetupTime { get; set; }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Google.OrTools.Sat;

//namespace UniversalTankScheduling
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            // ==================== 问题参数设置 ====================

//            // 储罐信息
//            var tanks = new[]
//            {
//                new { Id = 0, Name = "Tank-A", LiquidType = "Chemical-A", Capacity = 1000, CurrentLevel = 800, MinLevel = 100 },
//                new { Id = 1, Name = "Tank-B", LiquidType = "Chemical-B", Capacity = 1500, CurrentLevel = 1200, MinLevel = 150 },
//                new { Id = 2, Name = "Tank-C", LiquidType = "Chemical-A", Capacity = 800, CurrentLevel = 600, MinLevel = 80 },
//                new { Id = 3, Name = "Tank-D", LiquidType = "Chemical-C", Capacity = 1200, CurrentLevel = 900, MinLevel = 120 },
//                new { Id = 4, Name = "Tank-E", LiquidType = "Chemical-D", Capacity = 900, CurrentLevel = 700, MinLevel = 90 }
//            };

//            // 加工设备信息 - 现在可以处理多种液体
//            var equipments = new[]
//            {
//                new {
//                    Id = 0,
//                    Name = "Reactor-1",
//                    CompatibleLiquids = new[] { "Chemical-A", "Chemical-B" }, // 可处理A和B
//                    BatchSize = 200,
//                    ProcessingTime = 120,
//                    ChangeoverTime = 30 // 切换液体类型的清洗时间
//                },
//                new {
//                    Id = 1,
//                    Name = "Reactor-2",
//                    CompatibleLiquids = new[] { "Chemical-B", "Chemical-C", "Chemical-D" },
//                    BatchSize = 250,
//                    ProcessingTime = 150,
//                    ChangeoverTime = 25
//                },
//                new {
//                    Id = 2,
//                    Name = "Mixer-1",
//                    CompatibleLiquids = new[] { "Chemical-A", "Chemical-C", "Chemical-D" },
//                    BatchSize = 180,
//                    ProcessingTime = 100,
//                    ChangeoverTime = 20
//                },
//                new {
//                    Id = 3,
//                    Name = "Distillator-1",
//                    CompatibleLiquids = new[] { "Chemical-A", "Chemical-B", "Chemical-C", "Chemical-D" },
//                    BatchSize = 220,
//                    ProcessingTime = 180,
//                    ChangeoverTime = 40
//                }
//            };

//            // 管线信息
//            var pipelines = new[]
//            {
//                new { Id = 0, Name = "Main-Pipeline-1", Capacity = 1 },
//                new { Id = 1, Name = "Main-Pipeline-2", Capacity = 1 }
//            };

//            // 时间参数（分钟）
//            int horizon = 480; // 8小时工作制
//            int transferTimePerUnit = 1; // 每单位液体输送时间（分钟）

//            // ==================== 创建模型 ====================
//            CpModel model = new CpModel();

//            // ==================== 创建变量 ====================

//            // 定义所有可能的输送任务（只要设备和液体兼容）
//            var transferTasks = new List<TransferTask>();
//            int taskId = 0;

//            foreach (var tank in tanks)
//            {
//                foreach (var equipment in equipments)
//                {
//                    // 检查设备是否兼容该液体类型
//                    if (equipment.CompatibleLiquids.Contains(tank.LiquidType) &&
//                        tank.CurrentLevel >= equipment.BatchSize)
//                    {
//                        transferTasks.Add(new TransferTask
//                        {
//                            Id = taskId++,
//                            TankId = tank.Id,
//                            EquipmentId = equipment.Id,
//                            LiquidType = tank.LiquidType,
//                            Volume = equipment.BatchSize,
//                            TransferTime = equipment.BatchSize * transferTimePerUnit,
//                            SetupTime = 15 // 基础准备时间
//                        });
//                    }
//                }
//            }

//            Console.WriteLine($"创建了 {transferTasks.Count} 个可能的输送任务");

//            // 任务变量
//            Dictionary<int, IntVar> startVars = new Dictionary<int, IntVar>();
//            Dictionary<int, IntVar> endVars = new Dictionary<int, IntVar>();
//            Dictionary<int, IntervalVar> intervalVars = new Dictionary<int, IntervalVar>();
//            Dictionary<int, BoolVar> activeVars = new Dictionary<int, BoolVar>();

//            // 设备液体类型切换变量
//            Dictionary<(int, int, string), BoolVar> equipmentLiquidVars =
//                new Dictionary<(int, int, string), BoolVar>();

//            foreach (var task in transferTasks)
//            {
//                // 开始时间变量 [0, horizon]
//                startVars[task.Id] = model.NewIntVar(0, horizon, $"start_{task.Id}");

//                // 结束时间变量
//                endVars[task.Id] = model.NewIntVar(0, horizon, $"end_{task.Id}");

//                // 任务是否被调度
//                activeVars[task.Id] = model.NewBoolVar($"active_{task.Id}");

//                // 持续时间 = 输送时间 + 准备时间
//                var duration = task.TransferTime + task.SetupTime;

//                // 创建区间变量
//                intervalVars[task.Id] = model.NewOptionalIntervalVar(
//                    startVars[task.Id],
//                    duration,
//                    endVars[task.Id],
//                    activeVars[task.Id],
//                    $"interval_{task.Id}"
//                );

//                // 约束：如果任务被激活，结束时间 = 开始时间 + 持续时间
//                model.Add(endVars[task.Id] == startVars[task.Id] + duration)
//                    .OnlyEnforceIf(activeVars[task.Id]);

//                // 如果任务不被激活，开始和结束时间设为0
//                model.Add(startVars[task.Id] == 0).OnlyEnforceIf(activeVars[task.Id].Not());
//                model.Add(endVars[task.Id] == 0).OnlyEnforceIf(activeVars[task.Id].Not());

//                // 创建设备液体类型变量
//                var equipment = equipments.First(e => e.Id == task.EquipmentId);
//                equipmentLiquidVars[(task.EquipmentId, task.Id, task.LiquidType)] =
//                    model.NewBoolVar($"equip_{task.EquipmentId}_task_{task.Id}_liquid_{task.LiquidType}");
//            }

//            // ==================== 添加约束 ====================

//            // 约束1: 同一管线不能同时处理多个任务
//            // 分配任务到管线（简单的负载均衡）
//            foreach (var pipeline in pipelines)
//            {
//                // 选择一部分任务分配给这个管线（简化分配）
//                var pipelineTasks = transferTasks.Where(t => t.Id % pipelines.Length == pipeline.Id).ToList();
//                List<IntervalVar> pipelineIntervals = new List<IntervalVar>();

//                foreach (var task in pipelineTasks)
//                {
//                    pipelineIntervals.Add(intervalVars[task.Id]);
//                }

//                if (pipelineIntervals.Count > 1)
//                {
//                    model.AddNoOverlap(pipelineIntervals);
//                }
//            }

//            // 约束2: 同一储罐不能同时进行多个输送任务
//            foreach (var tank in tanks)
//            {
//                var tankTasks = transferTasks.Where(t => t.TankId == tank.Id).ToList();
//                if (tankTasks.Count > 1)
//                {
//                    List<IntervalVar> tankIntervals = new List<IntervalVar>();
//                    foreach (var task in tankTasks)
//                    {
//                        tankIntervals.Add(intervalVars[task.Id]);
//                    }
//                    model.AddNoOverlap(tankIntervals);
//                }
//            }

//            // 约束3: 同一设备不能同时接收多个输送任务
//            foreach (var equipment in equipments)
//            {
//                var equipmentTasks = transferTasks.Where(t => t.EquipmentId == equipment.Id).ToList();
//                if (equipmentTasks.Count > 1)
//                {
//                    List<IntervalVar> equipmentIntervals = new List<IntervalVar>();
//                    foreach (var task in equipmentTasks)
//                    {
//                        equipmentIntervals.Add(intervalVars[task.Id]);
//                    }
//                    model.AddNoOverlap(equipmentIntervals);
//                }
//            }

//            // 约束4: 储罐容量约束（输送后不能低于最低液位）
//            foreach (var tank in tanks)
//            {
//                var tankTasks = transferTasks.Where(t => t.TankId == tank.Id).ToList();
//                if (tankTasks.Count > 0)
//                {
//                    List<LinearExpr> tankTransfers = new List<LinearExpr>();
//                    foreach (var task in tankTasks)
//                    {
//                        tankTransfers.Add(activeVars[task.Id] * task.Volume);
//                    }
//                    model.Add(LinearExpr.Sum(tankTransfers) <= tank.CurrentLevel - tank.MinLevel);
//                }
//            }

//            // 约束5: 设备液体类型切换约束
//            foreach (var equipment in equipments)
//            {
//                var equipmentTasks = transferTasks.Where(t => t.EquipmentId == equipment.Id)
//                                                 .OrderBy(t => t.Id).ToList();

//                for (int i = 0; i < equipmentTasks.Count - 1; i++)
//                {
//                    var task1 = equipmentTasks[i];
//                    var task2 = equipmentTasks[i + 1];

//                    // 如果两个任务都被激活且液体类型不同，需要添加切换时间
//                    var differentLiquid = model.NewBoolVar($"diff_liquid_{task1.Id}_{task2.Id}");
//                    model.Add(task1.LiquidType != task2.LiquidType).OnlyEnforceIf(differentLiquid);
//                    model.Add(task1.LiquidType == task2.LiquidType).OnlyEnforceIf(differentLiquid.Not());

//                    var equipmentObj = equipments.First(e => e.Id == equipment.Id);
//                    var changeoverNeeded = model.NewBoolVar($"changeover_{task1.Id}_{task2.Id}");
//                    model.AddBoolAnd(new[] { activeVars[task1.Id], activeVars[task2.Id], differentLiquid })
//                         .OnlyEnforceIf(changeoverNeeded);
//                    model.AddBoolOr(new[] { activeVars[task1.Id].Not(), activeVars[task2.Id].Not(), differentLiquid.Not() })
//                         .OnlyEnforceIf(changeoverNeeded.Not());

//                    // 如果需要切换，任务2的开始时间 >= 任务1的结束时间 + 切换时间
//                    model.Add(startVars[task2.Id] >= endVars[task1.Id] + equipmentObj.ChangeoverTime)
//                         .OnlyEnforceIf(changeoverNeeded);
//                }
//            }

//            // 约束6: 优先使用即将达到最低液位的储罐
//            foreach (var tank in tanks)
//            {
//                var tankTasks = transferTasks.Where(t => t.TankId == tank.Id).ToList();
//                if (tankTasks.Count > 0)
//                {
//                    // 计算储罐的安全余量
//                    double safetyMargin = (double)(tank.CurrentLevel - tank.MinLevel) / tank.Capacity;

//                    // 为低安全余量的储罐任务添加优先级（较早开始）
//                    if (safetyMargin < 0.3) // 安全余量低于30%
//                    {
//                        foreach (var task in tankTasks)
//                        {
//                            // 鼓励这些任务尽早开始（通过目标函数实现）
//                            // 这里我们会在目标函数中处理
//                        }
//                    }
//                }
//            }

//            // ==================== 设置目标函数 ====================

//            // 目标1: 最小化总完成时间（makespan）
//            IntVar makespan = model.NewIntVar(0, horizon, "makespan");
//            foreach (var task in transferTasks)
//            {
//                model.Add(makespan >= endVars[task.Id]);
//            }

//            // 目标2: 最大化完成的任务数量
//            IntVar numCompletedTasks = model.NewIntVar(0, transferTasks.Count, "num_completed_tasks");
//            model.Add(numCompletedTasks == LinearExpr.Sum(activeVars.Values));

//            // 目标3: 最小化设备切换次数（减少清洗时间）
//            LinearExpr totalChangeoverTime = LinearExpr.Constant(0);
//            foreach (var equipment in equipments)
//            {
//                var equipmentTasks = transferTasks.Where(t => t.EquipmentId == equipment.Id)
//                                                 .OrderBy(t => t.Id).ToList();

//                for (int i = 0; i < equipmentTasks.Count - 1; i++)
//                {
//                    var task1 = equipmentTasks[i];
//                    var task2 = equipmentTasks[i + 1];

//                    var differentLiquid = model.NewBoolVar($"obj_diff_liquid_{task1.Id}_{task2.Id}");
//                    model.Add(task1.LiquidType != task2.LiquidType).OnlyEnforceIf(differentLiquid);
//                    model.Add(task1.LiquidType == task2.LiquidType).OnlyEnforceIf(differentLiquid.Not());

//                    var changeoverOccurs = model.NewBoolVar($"obj_changeover_{task1.Id}_{task2.Id}");
//                    model.AddBoolAnd(new[] { activeVars[task1.Id], activeVars[task2.Id], differentLiquid })
//                         .OnlyEnforceIf(changeoverOccurs);

//                    totalChangeoverTime = LinearExpr.Sum(new[] {
//                        totalChangeoverTime,
//                        changeoverOccurs * equipments.First(e => e.Id == equipment.Id).ChangeoverTime
//                    });
//                }
//            }

//            // 目标4: 平衡储罐使用（避免某些储罐过度使用）
//            LinearExpr tankBalancePenalty = LinearExpr.Constant(0);
//            foreach (var tank in tanks)
//            {
//                var tankTasks = transferTasks.Where(t => t.TankId == tank.Id).ToList();
//                if (tankTasks.Count > 0)
//                {
//                    LinearExpr tankUsage = LinearExpr.Constant(0);
//                    foreach (var task in tankTasks)
//                    {
//                        tankUsage = LinearExpr.Sum(new[] { tankUsage, activeVars[task.Id] * task.Volume });
//                    }

//                    // 理想使用量（按容量比例）
//                    double idealRatio = (double)tank.Capacity / tanks.Sum(t => t.Capacity);
//                    int idealUsage = (int)(idealRatio * transferTasks.Sum(t => t.Volume) / 2); // 简化计算

//                    // 使用量与理想值的偏差（绝对值）
//                    var usageDeviation = model.NewIntVar(0, horizon, $"deviation_tank_{tank.Id}");
//                    model.AddAbsEquality(usageDeviation, tankUsage - idealUsage);
//                    tankBalancePenalty = LinearExpr.Sum(new[] { tankBalancePenalty, usageDeviation });
//                }
//            }

//            // 多目标加权组合
//            int makespanWeight = 10;
//            int tasksWeight = 5;
//            int changeoverWeight = 3;
//            int balanceWeight = 2;

//            LinearExpr objective = LinearExpr.Sum(new[] {
//                makespan * makespanWeight,
//                LinearExpr.ScalProd(new[] { numCompletedTasks }, new[] { -tasksWeight }), // 负号因为我们要最大化
//                totalChangeoverTime * changeoverWeight,
//                tankBalancePenalty * balanceWeight
//            });

//            model.Minimize(objective);

//            // ==================== 求解 ====================

//            CpSolver solver = new CpSolver();
//            solver.StringParameters = "num_search_workers:8, max_time_in_seconds:60, log_search_progress: true";

//            Console.WriteLine("开始求解...");
//            CpSolverStatus status = solver.Solve(model);

//            // ==================== 输出结果 ====================

//            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
//            {
//                Console.WriteLine("\n=== 通用储罐液体输送调度方案 ===");
//                Console.WriteLine($"求解状态: {status}");
//                Console.WriteLine($"总完成时间: {solver.Value(makespan)} 分钟");
//                Console.WriteLine($"完成的任务数: {solver.Value(numCompletedTasks)} / {transferTasks.Count}");
//                Console.WriteLine($"目标函数值: {solver.ObjectiveValue}");
//                Console.WriteLine();

//                // 按设备分组输出调度
//                foreach (var equipment in equipments.OrderBy(e => e.Id))
//                {
//                    Console.WriteLine($"\n{equipment.Name} 调度:");
//                    var equipmentTasks = transferTasks
//                        .Where(t => t.EquipmentId == equipment.Id && solver.Value(activeVars[t.Id]) == 1)
//                        .OrderBy(t => solver.Value(startVars[t.Id]))
//                        .ToList();

//                    if (equipmentTasks.Count == 0)
//                    {
//                        Console.WriteLine("  无调度任务");
//                        continue;
//                    }

//                    Console.WriteLine("  时间\t\t储罐\t\t液体类型\t体积\t持续时间");
//                    Console.WriteLine("  " + new string('-', 65));

//                    foreach (var task in equipmentTasks)
//                    {
//                        var tank = tanks.First(t => t.Id == task.TankId);
//                        var start = solver.Value(startVars[task.Id]);
//                        var end = solver.Value(endVars[task.Id]);

//                        Console.WriteLine($"  {start:000}-{end:000}\t{tank.Name}\t{task.LiquidType}\t\t{task.Volume}\t{end - start}min");
//                    }
//                }

//                // 输出管线使用情况
//                Console.WriteLine("\n=== 管线使用情况 ===");
//                foreach (var pipeline in pipelines)
//                {
//                    var pipelineTasks = transferTasks
//                        .Where(t => t.Id % pipelines.Length == pipeline.Id && solver.Value(activeVars[t.Id]) == 1)
//                        .ToList();

//                    long totalTime = pipelineTasks.Sum(t => solver.Value(endVars[t.Id]) - solver.Value(startVars[t.Id]));
//                    double utilization = (double)totalTime / solver.Value(makespan) * 100;

//                    Console.WriteLine($"{pipeline.Name}: {pipelineTasks.Count}个任务, " +
//                                    $"利用率: {utilization:F1}%");
//                }

//                // 输出储罐状态变化
//                Console.WriteLine("\n=== 储罐状态变化 ===");
//                Console.WriteLine("储罐\t\t初始液位\t使用量\t最终液位\t剩余安全余量");
//                Console.WriteLine(new string('-', 80));

//                foreach (var tank in tanks)
//                {
//                    var tankTasks = transferTasks
//                        .Where(t => t.TankId == tank.Id && solver.Value(activeVars[t.Id]) == 1)
//                        .ToList();

//                    int totalUsed = tankTasks.Sum(t => t.Volume);
//                    int finalLevel = tank.CurrentLevel - totalUsed;
//                    double safetyMargin = (double)(finalLevel - tank.MinLevel) / tank.Capacity * 100;

//                    Console.WriteLine($"{tank.Name}\t{tank.CurrentLevel}\t\t{totalUsed}\t{finalLevel}\t\t{safetyMargin:F1}%");
//                }

//                // 输出设备切换统计
//                Console.WriteLine("\n=== 设备切换统计 ===");
//                foreach (var equipment in equipments)
//                {
//                    var equipmentTasks = transferTasks
//                        .Where(t => t.EquipmentId == equipment.Id && solver.Value(activeVars[t.Id]) == 1)
//                        .OrderBy(t => solver.Value(startVars[t.Id]))
//                        .ToList();

//                    int changeovers = 0;
//                    for (int i = 0; i < equipmentTasks.Count - 1; i++)
//                    {
//                        if (equipmentTasks[i].LiquidType != equipmentTasks[i + 1].LiquidType)
//                        {
//                            changeovers++;
//                        }
//                    }

//                    Console.WriteLine($"{equipment.Name}: {changeovers}次液体类型切换");
//                }
//            }
//            else
//            {
//                Console.WriteLine("未找到可行解");
//            }

//            // 输出求解统计
//            Console.WriteLine("\n=== 求解统计 ===");
//            Console.WriteLine($"求解时间: {solver.WallTime():F2}秒");
//            Console.WriteLine($"目标值边界: {solver.BestObjectiveBound}");
//            Console.WriteLine($"冲突次数: {solver.NumConflicts()}");
//            Console.WriteLine($"分支次数: {solver.NumBranches()}");
//        }

//        // 输送任务定义
//        class TransferTask
//        {
//            public int Id { get; set; }
//            public int TankId { get; set; }
//            public int EquipmentId { get; set; }
//            public string LiquidType { get; set; }
//            public int Volume { get; set; }
//            public int TransferTime { get; set; }
//            public int SetupTime { get; set; }
//        }
//    }
//}