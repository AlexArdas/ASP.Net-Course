namespace CST.Common.Extensions;

public static class IEnumerableExtensions
{
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) =>
		enumerable is null || !enumerable.Any();

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		if (enumerable is null || action is null)
		{
			return;
		}

		foreach (var item in enumerable)
		{
			action(item);
		}
	}
}