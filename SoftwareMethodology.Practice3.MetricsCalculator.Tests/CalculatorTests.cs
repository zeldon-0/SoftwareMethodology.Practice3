

using FluentAssertions;
using SoftwareMethodology.Practice3.MetricsCalculator.Core;
using Xunit;

namespace SoftwareMethodology.Practice3.MetricsCalculator.Tests;
public class CalculatorTests
{
    private readonly Calculator _calculator;

    public CalculatorTests()
    {
        _calculator = new Calculator();
    }

    [Theory]
    [MemberData(nameof(CalculateClassDepthOfInheritanceData))]
    public void CalculateClassDepthOfInheritance_should_return_depth_of_queried_class(
        string className,
        int expectedDepth)
    {
        // Act
        var actualDepth = _calculator.CalculateClassDepthOfInheritance(className);

        // Assert
        actualDepth.Should().Be(expectedDepth);
    }

    [Fact]
    public void CalculateClassDepthOfInheritance_should_throw_exception_for_missing_class()
    {
        // Arrange
        var missingClassName = "SomeClassThatDoesNotExist";

        // Act
        var act = () => _calculator.CalculateClassDepthOfInheritance(missingClassName);

        // Assert
        act.Should().Throw<Exception>().WithMessage("No such class in the library!");
    }

    [Fact]
    public void CalculateLibraryDepthOfInheritance_should_return_max_class_depth_in_library()
    {
        // Arrange
        var expectedDepth = 3;

        // Act
        var actualDepth = _calculator.CalculateLibraryDepthOfInheritance();

        // Assert
        actualDepth.Should().Be(expectedDepth);
    }

    [Theory]
    [MemberData(nameof(CalculateClassNumberOfChildrenData))]
    public void CalculateClassNumberOfChildren_should_return_number_of_child_classes(
        string className,
        int expectedNumberOfChildren)
    {
        // Act
        var actualNumberOfChildren = _calculator.CalculateClassNumberOfChildren(className);

        // Assert
        actualNumberOfChildren.Should().Be(expectedNumberOfChildren);
    }

    [Theory]
    [MemberData(nameof(CalculateClassMethodInheritanceData))]
    public void CalculateClassMethodInheritance_should_return_ratio_of_inherited_methods_to_ones_defined_in_class(
        string className,
        double expectedMethodInheritanceFactor)
    {
        // Act
        var actualMethodInheritanceFactor = _calculator.CalculateClassMethodInheritance(className);

        // Assert
        actualMethodInheritanceFactor.Should().Be(expectedMethodInheritanceFactor);
    }

    [Fact]
    public void CalculateLibraryMethodInheritance_should_return_ratio_of_inherited_methods_to_ones_defined_in_entire_project()
    {
        // Arrange
        var expectedMethodInheritanceFactor = (double)7 / 15;

        // Act
        var actualMethodInheritanceFactor = _calculator.CalculateLibraryMethodInheritance();

        // Assert
        actualMethodInheritanceFactor.Should().Be(expectedMethodInheritanceFactor);
    }

    [Theory]
    [MemberData(nameof(CalculateClassMethodHidingFactorData))]
    public void CalculateClassMethodHidingFactor_should_return_ratio_of_hidden_methods_in_class(
        string className,
        double expectedMethodHidingFactor)
    {
        // Act
        var actualMethodHidingFactor = _calculator.CalculateClassMethodHidingFactor(className);

        // Assert
        actualMethodHidingFactor.Should().BeApproximately(expectedMethodHidingFactor, 1e-5);
    }

    [Fact]
    public void CalculateLibraryMethodHidingFactor_should_return_ratio_of_hidden_methods_in_library()
    {
        // Arrange
        var expectedMethodHidingFactor = (double)1 / 3;

        // Act
        var actualMethodHidingFactor = _calculator.CalculateLibraryMethodHidingFactor();

        // Assert
        actualMethodHidingFactor.Should().BeApproximately(expectedMethodHidingFactor, 1e-5);
    }

    [Theory]
    [MemberData(nameof(CalculateClassAttributeInheritanceData))]
    public void CalculateClassAttributeInheritance_ratio_of_hidden_properties_in_library(
        string className,
        double expectedAttributeInheritanceFactor)
    {
        // Act
        var actualAttributeInheritanceFactor = _calculator.CalculateClassAttributeInheritance(className);

        // Assert
        actualAttributeInheritanceFactor.Should().Be(expectedAttributeInheritanceFactor);
    }

    [Fact]
    public void CalculateLibraryPropertyInheritance_should_return_ratio_of_inherited_properties_to_ones_defined_in_entire_project()
    {
        // Arrange
        var expectedPropertyInheritanceFactor = (double)7 / 15;

        // Act
        var actualPropertyInheritanceFactor = _calculator.CalculateLibraryAttributeInheritance();

        // Assert
        actualPropertyInheritanceFactor.Should().Be(expectedPropertyInheritanceFactor);
    }

    [Theory]
    [MemberData(nameof(CalculateClassPropertyHidingFactorData))]
    public void CalculateClassPropertyHidingFactor_should_return_ratio_of_hidden_properties_in_class(
        string className,
        double expectedPropertyHidingFactor)
    {
        // Act
        var actualPropertyHidingFactor = _calculator.CalculateClassPropertyHidingFactor(className);

        // Assert
        actualPropertyHidingFactor.Should().BeApproximately(expectedPropertyHidingFactor, 1e-5);
    }

    [Fact]
    public void CalculateLibraryPropertyHidingFactor_should_return_ratio_of_hidden_properties_in_library()
    {
        // Arrange
        var expectedPropertyHidingFactor = (double)1 / 3;

        // Act
        var actualPropertyHidingFactor = _calculator.CalculateLibraryPropertyHidingFactor();

        // Assert
        actualPropertyHidingFactor.Should().BeApproximately(expectedPropertyHidingFactor, 1e-5);
    }

    [Theory]
    [MemberData(nameof(CalculateClassPolymorphismFactorData))]
    public void CalculateClassPolymorphismFactor_should_return_ratio_of_overriden_to_potential_overrides_in_class(
        string className,
        double expectedPolymorphismFactor)
    {
        // Act
        var actualPolymorphismFactor = _calculator.CalculateClassPolymorphismFactor(className);

        // Assert
        actualPolymorphismFactor.Should().BeApproximately(expectedPolymorphismFactor, 1e-5);
    }


    [Fact]
    public void CalculateLibraryPolymorphismFactor_should_return_ratio_of_overriden_to_potential_overrides_in_library()
    {
        // Arrange
        var expectedPropertyHidingFactor = (double) 1 / 8;

        // Act
        var actualPropertyHidingFactor = _calculator.CalculateLibraryPolymorphismFactor();

        // Assert
        actualPropertyHidingFactor.Should().BeApproximately(expectedPropertyHidingFactor, 1e-5);
    }

    public static IEnumerable<object[]> CalculateClassDepthOfInheritanceData()
    {
        yield return new object[] { "ClassA", 1 };
        yield return new object[] { "ClassB", 2 };
        yield return new object[] { "ClassC", 3 };
    }

    public static IEnumerable<object[]> CalculateClassNumberOfChildrenData()
    {
        yield return new object[] { "ClassA", 1 };
        yield return new object[] { "ClassB", 2 };
        yield return new object[] { "ClassC", 0 };
    }

    public static IEnumerable<object[]> CalculateClassMethodInheritanceData()
    {
        yield return new object[] { "ClassA", 0 };
        yield return new object[] { "ClassB", (double)1 / 3 };
        yield return new object[] { "ClassC", (double)3 / 4 };
        yield return new object[] { "ClassD", (double)3 / 5 };
    }

    public static IEnumerable<object[]> CalculateClassMethodHidingFactorData()
    {
        yield return new object[] { "ClassA", (double)1 / 3 };
        yield return new object[] { "ClassB", (double)1 / 3 };
        yield return new object[] { "ClassC", (double)1 / 4 };
        yield return new object[] { "ClassD", (double)2 / 5 };
    }

    public static IEnumerable<object[]> CalculateClassAttributeInheritanceData()
    {
        yield return new object[] { "ClassA", 0 };
        yield return new object[] { "ClassB", (double)1 / 3 };
        yield return new object[] { "ClassC", (double)3 / 4 };
        yield return new object[] { "ClassD", (double)3 / 5 };
    }

    public static IEnumerable<object[]> CalculateClassPropertyHidingFactorData()
    {
        yield return new object[] { "ClassA", (double)1 / 3 };
        yield return new object[] { "ClassB", (double)1 / 3 };
        yield return new object[] { "ClassC", (double)1 / 4 };
        yield return new object[] { "ClassD", (double)2 / 5 };
    }
    public static IEnumerable<object[]> CalculateClassPolymorphismFactorData()
    {
        yield return new object[] { "ClassA", 0 };
        yield return new object[] { "ClassB", (double)1 / 2 };
        yield return new object[] { "ClassC", (double)0 };
        yield return new object[] { "ClassD", (double)0 };
    }
}
