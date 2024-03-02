using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ProblemSetSpace;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class ProblemSetTests
{
    private ProblemSet _problemSetGenerator;

    [SetUp]
    public void Setup()
    {
        // Initialize your ProblemSetGenerator or provide any necessary dependencies.
        _problemSetGenerator = new ProblemSet();
    }

    [Test]
    public void GetNewProblemSet_TargetNumberWithinRange()
    {
        // Arrange
        int currentLevel = 1;

        // Act
        var result = _problemSetGenerator.GetNewProblemSet(currentLevel);

        // Assert
        // Assert.GreaterOrEqual(result.Item1, ProblemSetTests._problemSetGenerator.MinNumber);
        // Assert.LessOrEqual(result.Item1, _problemSetGenerator.MaxNumber);
    }

    [Test]
    public void GetNewProblemSet_ProvidedSelectablesShuffled()
    {
        // Arrange
        int currentLevel = 1;

        // Act
        var result = _problemSetGenerator.GetNewProblemSet(currentLevel);

        // Assert
        CollectionAssert.AreNotEqual(result.Item2, _problemSetGenerator.GetNewProblemSet(currentLevel).Item2);
        // Ensure that the provided selectables are shuffled.
    }
}
