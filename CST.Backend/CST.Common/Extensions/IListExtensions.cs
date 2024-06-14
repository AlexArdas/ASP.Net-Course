using System.Collections.Concurrent;

namespace CST.Common.Extensions;

public static class IListExtensions
{
	public static async Task<IReadOnlyCollection<T>> RunParallel<T>(
		this IList<Func<Task<T>>> tasksList,
		int parallelTasksCount = 5,
		Action<string> logAction = null)
	{
		var bag = new ConcurrentBag<T>();

		var wrappedTasks = tasksList
			.Select(task => (Func<Task>) (async () =>
			{
				var executedTask = await task.Invoke();
				bag.Add(executedTask);
			}))
			.ToList();

		await wrappedTasks.RunParallel(parallelTasksCount, logAction);
		return bag.ToList();
	}

	public static async Task RunParallel(
		this IList<Func<Task>> tasksList,
		int parallelTasksCount = 5,
		Action<string> logAction = null)
	{
		if (tasksList is null)
		{
			return;
		}

		var semaphore = new SemaphoreSlim(parallelTasksCount);

		var totalTasksCount = tasksList.Count;
		var completedTasksCount = 0;
		var failedTasksCount = 0;

		var tasks = tasksList
			.Select(TaskWrapper)
			.ToArray();

		await Task.WhenAll(tasks);

		if (failedTasksCount > 0)
			logAction?.Invoke(
				$"Finished {totalTasksCount} total tasks: {completedTasksCount} tasks completed, {failedTasksCount} tasks failed.");

		Task TaskWrapper(Func<Task> task)
		{
			return Task.Run(async () =>
			{
				await semaphore.WaitAsync();
				try
				{
					await task.Invoke();
					Interlocked.Increment(ref completedTasksCount);
				}
				catch (Exception ex)
				{
					Interlocked.Increment(ref failedTasksCount);
					logAction?.Invoke(ex.Message);
					logAction?.Invoke(ex.StackTrace);
				}
				finally
				{
					semaphore.Release();
				}
			});
		}
	}
}