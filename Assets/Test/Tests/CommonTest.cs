using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using YeUtility;

namespace Test.Tests
{
    public class CommonTest
    {
        private const float Delta = 0.001f;

        // A Test behaves as an ordinary method
        [Test]
        [TestCase(1, 2, 4)]
        public void RemapTest(float input, float to1, float to2)
        {
            var rv = input.Remap(0, to1, 0, to2: to2);
            Assert.AreEqual(rv, input*to2/to1, Delta);
        }

        [Test]
        [TestCase("Meader")]
        public void StringByteTest(string input)
        {
            var bytes = input.ToByte();
            var result = bytes.ToSting();
            Assert.AreEqual(input, result);
        }
    
        [Test]
        [TestCase("MeaderG")]
        public void EncryptTest(string input)
        {
            var encode = Common.Encrypt(input);
            var result = Common.Decrypt(encode);
            Assert.AreEqual(input, result);
        }

        [Test]
        [TestCase(50, 3, 1,0,0)]
        [TestCase(180, 30, 0.5f,0.5f,0)]
        public void CreateFanVectors(float angle, int number, float x, float y, float z)
        {
            var centerDir = new Vector3(x, y, z);
            var rvDirs = Common.CreateFanVectors(angle, number, centerDir);
            Assert.That(number, Is.EqualTo(rvDirs.Count));
            var preAngle = angle / (number - 1);
            for (var index = 0; index < number - 1; index++)
            {
                var nextIndex = (index + 1) % rvDirs.Count;
                var rvAngle = Vector3.Angle(rvDirs[index], rvDirs[nextIndex]);
                Assert.AreEqual(preAngle, rvAngle, Delta);
            }

            var temp = Vector3.Angle(centerDir, rvDirs[0]);
            Assert.AreEqual(temp, angle*0.5f, Delta);
        }

        [Test]
        [TestCase(5, 4, "0005")]
        [TestCase(123, 5, "00123")]
        public void INTToStrTest(int input, int digits, string result)
        {
            Assert.AreEqual(input.INTToStr(digits), result);
        }

        [Test]
        public void ObjectToStringTest()
        {
            var sourceObj = new TestToStringClass();
            sourceObj.Init();
            var temp = Common.ToString(sourceObj);
            var resultObj = new TestToStringClass();
            Common.FromString(resultObj,temp);
            Assert.IsTrue(sourceObj.Compare(resultObj));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        public IEnumerator CommonTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        internal class TestToStringClass
        {
            private enum MyEnum
            {
                e1,
                e2,
                e3,
            }

            private MyEnum _enums;
            private string _string;
            private Dictionary<string, int> _intDict = new();
            private Dictionary<string, string> _strDict = new();
            private int[] _ints;
            private float[] _floats;
            private List<string> _strs = new();

            public void Init()
            {
                _enums = MyEnum.e3;
                _string = "TestAbc1234";
                _intDict["abc"] = 1;
                _intDict["bc"] = 2;
                _intDict["ac"] = 3;

                _strDict["123"] = "a";
                _strDict["23"] = "b";
                _strDict["12"] = "c";


                _ints = new[] { 6, 5, 4, 3 };
                _floats = new[] { 3f, 5f, 6f, 7f };

                _strs.Add("bcxz");
                _strs.Add("1234");
                _strs.Add("cgc");
            }

            public bool Compare(TestToStringClass other)
            {
                if (_enums != other._enums)
                    return false;
                if (_string != other._string)
                    return false;
                if (!_intDict.SequenceEqual(other._intDict))
                    return false;
                if (!_strDict.SequenceEqual(other._strDict))
                    return false;
                if (!_ints.SequenceEqual(other._ints))
                    return false;
                if (!_floats.SequenceEqual(other._floats))
                    return false;
                return _strs.SequenceEqual(other._strs);
            }
        }
    }
}
