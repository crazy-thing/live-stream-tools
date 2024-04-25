using LiveStreamTools.Models;


namespace LiveStreamTools.Util
{
    public class TaskHandler
    {
        public static void ShowAllTasks()
        {
            lock (Program.lockObject)
            {
                foreach (var kvp in Program.tasks)
                {
                    Console.WriteLine($"Task: \"{kvp.Key}\"   Type: {kvp.Value.TaskType}   Status: {(kvp.Value.CancellationTokenSource.IsCancellationRequested ? "Canceled" : "Running")}  ");
                }
            }
        }

        public static void StopTask(string taskName)
        {
            lock (Program.lockObject)
            {
                if (taskName.Equals("all"))
                {
                    foreach (var task in Program.tasks)
                    {
                        task.Value.CancellationTokenSource.Cancel();
                    }
                    Program.tasks.Clear();
                    Program.nameToIds.Clear();
                    Console.WriteLine("Stopped all tasks");
                    return;
                }

                if (Program.tasks.TryGetValue(taskName, out TaskInfo taskInfo))
                {
                    taskInfo.CancellationTokenSource.Cancel();
                    Program.tasks.Remove(taskName);
                    Program.nameToIds.Remove(taskName);
                    Console.WriteLine($"Task {taskName} stopped.");
                }
                else
                {
                    Console.WriteLine($"Task {taskName} not found.");
                }            
            }
        }
    }   
}