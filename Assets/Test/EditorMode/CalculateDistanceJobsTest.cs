using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.TestTools;
using YeUtility;

public class CalculateDistanceJobsTest
{
    // A Test behaves as an ordinary method
    public void CalculateDistanceJobsTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    [TestCase(2, 2, ExpectedResult = (IEnumerator)null)]
    [TestCase(3, 3, ExpectedResult = (IEnumerator)null)]
    [TestCase(4, 4, ExpectedResult = (IEnumerator)null)]
    [TestCase(5, 5, ExpectedResult = (IEnumerator)null)]
    public IEnumerator CalculateDistanceJobsTestWithEnumeratorPasses(int size = 3, int length = 5)
    {
        var sourceList = new List<Vector3>();

        for (int i = 0; i < size; i++)
        {
            sourceList.Add(new Vector3(i*length,0,0));
        }
        
        var jobs = new CalculateDistanceJobs();
        jobs.Setup(sourceList);
        var jobHandler = jobs.Schedule(size, 64);
        
        yield return null;
    
        jobHandler.Complete();
        
        var rv = jobs.Result;
        Assert.That(size*size, Is.EqualTo(rv.Length));

        for (var i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j) continue;
                var index = i * size + j;
                Assert.That(rv[index], Is.EqualTo(Mathf.Abs(i-j)*length));
            }
        }
        jobs.Dispose();
    }
}
