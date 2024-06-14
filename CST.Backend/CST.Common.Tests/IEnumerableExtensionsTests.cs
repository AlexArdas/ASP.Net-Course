using CST.Common.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CST.Common.Tests;

public class IEnumerableExtensionsTests
{
	[Fact]
	public void IsNullOrEmpty_ShouldReturnTrue_WhenEnumerableIsNull()
	{
		// Arrange
		IEnumerable<int> enumerable = null;

		// Act
		var result = enumerable.IsNullOrEmpty();

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void IsNullOrEmpty_ShouldReturnTrue_WhenEnumerateIsEmpty()
	{
		// Arrange
		var enumerable = Array.Empty<int>();

		// Act
		var result = enumerable.IsNullOrEmpty();

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void IsNullOrEmpty_ShouldReturnFalse_WhenEnumerableHasValues()
	{
		// Arrange
		var enumerable = new[] {1, 2, 3};

		// Act
		var result = enumerable.IsNullOrEmpty();

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void ForEach_ShouldNotEnumerateEnumerable_WhenEnumerableIsNull()
	{
		// Arrange
		var expectedEnumeratedItems = 0;
		var currentEnumeratedItems = 0;
		IEnumerable<int> enumerable = null;

		// Act
		enumerable.ForEach(_ => currentEnumeratedItems++);

		// Assert
		expectedEnumeratedItems.Should().Be(currentEnumeratedItems);
	}

	[Fact]
	public void ForEach_ShouldNotEnumerateEnumerable_WhenEnumerableIsEmpty()
	{
		// Arrange
		var expectedEnumeratedItems = 0;
		var currentEnumeratedItems = 0;
		var enumerable = new int[] {};

		// Act
		enumerable.ForEach(_ => currentEnumeratedItems++);

		// Assert
		expectedEnumeratedItems.Should().Be(currentEnumeratedItems);
	}

	[Fact]
	public void ForEach_ShouldEnumerateEnumerable_WhenEnumerableHasValues()
	{
		// Arrange
		var expectedEnumeratedItems = 10;
		var currentEnumeratedItems = 0;

		// Act
		Enumerable.Range(0, 10).ForEach(_ => currentEnumeratedItems++);

		// Assert
		expectedEnumeratedItems.Should().Be(currentEnumeratedItems);
	}
}