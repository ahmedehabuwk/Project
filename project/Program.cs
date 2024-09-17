using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    internal class Program
    {
        class Process
        {
            public string Name { get; set; }
            public int ArrivalTime { get; set; }
            public int BurstTime { get; set; }
            public int RemainingTime { get; set; }
            public int WaitingTime { get; set; }
            public int TurnaroundTime { get; set; }
            public int ResponseTime { get; set; } = -1;

            public Process(string name, int arrivalTime, int burstTime)
            {
                Name = name;
                ArrivalTime = arrivalTime;
                BurstTime = burstTime;
                RemainingTime = burstTime;
            }
        }

        class Program2
        {
            static void RoundRobin(List<Process> processes, int quantum)
            {
                int time = 0;
                Queue<Process> queue = new Queue<Process>();
                List<(string, int)> ganttChart = new List<(string, int)>();

                foreach (var p in processes)
                {
                    queue.Enqueue(p);
                }

                while (queue.Count > 0)
                {
                    Process process = queue.Dequeue();

                    if (process.ResponseTime == -1)
                        process.ResponseTime = time;

                    int executionTime = Math.Min(process.RemainingTime, quantum);
                    process.RemainingTime -= executionTime;
                    time += executionTime;

                    ganttChart.Add((process.Name, time));

                    if (process.RemainingTime > 0)
                    {
                        queue.Enqueue(process);
                    }
                    else
                    {
                        process.TurnaroundTime = time - process.ArrivalTime;
                    }
                }

                foreach (var p in processes)
                {
                    p.WaitingTime = p.TurnaroundTime - p.BurstTime;
                }

                PrintStatistics(processes);
            }

            static void PrintStatistics(List<Process> processes)
            {
                Console.WriteLine($"{"Process",-10}{"Waiting Time",-15}{"Turnaround Time",-20}{"Response Time",-15}");
                double totalWaitingTime = 0, totalTurnaroundTime = 0, totalResponseTime = 0;

                foreach (var p in processes)
                {
                    totalWaitingTime += p.WaitingTime;
                    totalTurnaroundTime += p.TurnaroundTime;
                    totalResponseTime += p.ResponseTime;
                    Console.WriteLine($"{p.Name,-10}{p.WaitingTime,-15}{p.TurnaroundTime,-20}{p.ResponseTime,-15}");
                }

                Console.WriteLine($"\nAverage Waiting Time: {totalWaitingTime / processes.Count}");
                Console.WriteLine($"Average Turnaround Time: {totalTurnaroundTime / processes.Count}");
                Console.WriteLine($"Average Response Time: {totalResponseTime / processes.Count}");
            }

            static void Main()
            {
                List<Process> processes = new List<Process>
                {
                    new Process("P1", 0, 8),
                    new Process("P2", 1, 4),
                    new Process("P3", 2, 9),
                    new Process("P4", 3, 5)
                };

                int quantum = 4; 
                RoundRobin(processes, quantum);
                Console.ReadLine();
            }
        }

    }
}