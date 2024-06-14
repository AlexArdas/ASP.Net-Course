using CST.Common.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CST.Common.Tests;

public class IListExtensionsTests
{
	[Fact]
	public async Task  RunParallel_ShouldReturnCompletedTasks_WhenListOfTasksContainsGoodTasks()
	{
		// Arrange
		var tasks = new List<Func<Task<int>>>();
		Enumerable.Range(1, 6).ForEach(_ => tasks.Add((Func<Task<int>>)(GoodTask)));
		Enumerable.Range(1, 5).ForEach(_ => tasks.Add((Func<Task<int>>)BadTask));
		var expectedCompletedTasks = 6;

		// Act
		var executedTasks = await tasks.RunParallel();

		// Assert
		expectedCompletedTasks.Should().Be(executedTasks.Count);
	}

	[Fact]
	public async Task  RunParallel_ShouldReturnEmptyListOfCompletedTasks_WhenListOfTasksContainsOnlyBadTasks()
	{
		// Arrange
		var tasks = new List<Func<Task<int>>>();
		Enumerable.Range(1, 5).ForEach(_ => tasks.Add((Func<Task<int>>)BadTask));
		var expectedCompletedTasks = 0;

		// Act
		var executedTasks = await tasks.RunParallel();

		// Assert
		expectedCompletedTasks.Should().Be(executedTasks.Count);
	}

	private Task<int> GoodTask() => Task.FromResult(2);
	private Task<int> BadTask() => throw new InvalidOperationException();
}