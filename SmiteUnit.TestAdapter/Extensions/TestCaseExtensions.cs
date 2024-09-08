// Original File: https://github.com/microsoft/testfx/blob/db6a0951020b53fb2fec74972a1dec86592bdebb/src/Adapter/MSTest.TestAdapter/Extensions/TestCaseExtensions.cs
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project below for full license information.
// https://github.com/microsoft/testfx?tab=MIT-1-ov-file

using Microsoft.TestPlatform.AdapterUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;

namespace SmiteUnit.TestAdapter.Extensions;

/// <summary>
/// Extension Methods for TestCase Class.
/// </summary>
internal static class TestCaseExtensions
{
	internal static readonly TestProperty ManagedTypeProperty = TestProperty.Register(
		id: ManagedNameConstants.ManagedTypePropertyId,
		label: ManagedNameConstants.ManagedTypeLabel,
		category: string.Empty,
		description: string.Empty,
		valueType: typeof(string),
		validateValueCallback: o => !string.IsNullOrWhiteSpace(o as string),
		attributes: TestPropertyAttributes.Hidden,
		owner: typeof(TestCase));

	internal static readonly TestProperty ManagedMethodProperty = TestProperty.Register(
		id: ManagedNameConstants.ManagedMethodPropertyId,
		label: ManagedNameConstants.ManagedMethodLabel,
		category: string.Empty,
		description: string.Empty,
		valueType: typeof(string),
		validateValueCallback: o => !string.IsNullOrWhiteSpace(o as string),
		attributes: TestPropertyAttributes.Hidden,
		owner: typeof(TestCase));

	internal static readonly TestProperty HierarchyProperty = TestProperty.Register(
		id: HierarchyConstants.HierarchyPropertyId,
		label: HierarchyConstants.HierarchyLabel,
		category: string.Empty,
		description: string.Empty,
		valueType: typeof(string[]),
		validateValueCallback: null,
		attributes: TestPropertyAttributes.Immutable,
		owner: typeof(TestCase));

	/// <summary>
	/// The test name.
	/// </summary>
	/// <param name="testCase"> The test case. </param>
	/// <param name="testClassName"> The test case's class name. </param>
	/// <returns> The test name, without the class name, if provided. </returns>
	internal static string GetTestName(this TestCase testCase, string? testClassName)
	{
		string fullyQualifiedName = testCase.FullyQualifiedName;

		// Not using Replace because there can be multiple instances of that string.
		string name = fullyQualifiedName.StartsWith($"{testClassName}.", StringComparison.Ordinal)
			? fullyQualifiedName.Remove(0, $"{testClassName}.".Length)
			: fullyQualifiedName;

		return name;
	}

	internal static string? GetManagedType(this TestCase testCase) => testCase.GetPropertyValue<string>(ManagedTypeProperty, null);

	internal static void SetManagedType(this TestCase testCase, string value) => testCase.SetPropertyValue(ManagedTypeProperty, value);

	internal static string? GetManagedMethod(this TestCase testCase) => testCase.GetPropertyValue<string>(ManagedMethodProperty, null);

	internal static void SetManagedMethod(this TestCase testCase, string value) => testCase.SetPropertyValue(ManagedMethodProperty, value);

	internal static bool ContainsManagedMethodAndType(this TestCase testCase) => !string.IsNullOrWhiteSpace(testCase.GetManagedMethod()) && !string.IsNullOrWhiteSpace(testCase.GetManagedType());

	internal static string[]? GetHierarchy(this TestCase testCase) => testCase.GetPropertyValue<string[]>(HierarchyProperty, null);

	internal static void SetHierarchy(this TestCase testCase, params string?[] value) => testCase.SetPropertyValue(HierarchyProperty, value);
}
