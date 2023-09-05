using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class AbbrevationUtilityTest
{
    // A Test behaves as an ordinary method
    [Test]
    [TestCase(999, "999")]
    [TestCase(1000, "1K")]
    [TestCase(999999, "999K")]
    [TestCase(1100000, "1M")]
    [TestCase(588888888, "588M")]
    [TestCase(1588888888, "1B")]
    public void AbbrevationUtilityTestSimplePasses(float input, string result)
    {
        // Use the Assert class to test conditions
        var rv = YeUtility.AbbrevationUtility.AbbreviateNumber(input);
        Assert.That(result, Is.EqualTo(rv));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator AbbrevationUtilityTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
