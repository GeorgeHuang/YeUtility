using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TestTools;
using YeUtility;

public class CircleSplitTest
{
    // A Test behaves as an ordinary method
    [Test]
    [TestCase(3,8,360/3)]
    public void CircleSplitTestSimplePasses(int number, float radius, float rvAngle)
    {
        var rootGo = new GameObject("root");
        var circleSplit = rootGo.AddComponent<YeCircleSplit>();
        circleSplit.number = number;
        circleSplit.radius = radius;
        var rvGoList = circleSplit.Apply();
        Assert.That(rvGoList.Count, Is.EqualTo(number));
        int index = 0;
        foreach (var go in rvGoList)
        {
            var dis = Vector3.Distance(go.transform.position, rootGo.transform.position);
            const float delta = 0.00001f;
            Assert.AreEqual(dis, radius, delta);
            var nextIndex = (index + 1) % rvGoList.Count;
            var angle = Vector3.Angle(rvGoList[index].transform.right, rvGoList[nextIndex].transform.right);
            Assert.AreEqual(angle, rvAngle, delta);
            index++;
        }
    }

}
