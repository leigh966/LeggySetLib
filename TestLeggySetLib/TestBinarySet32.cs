using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace TestLeggySetLib
{
    [TestClass]
    public class TestBinarySet32
    {
        [DataTestMethod]
        [DataRow(1,0)]
        [DataRow(1, 33)]
        [DataRow(0, 32)]
        public void TestInitBadRange(int min, int max)
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ISet<int> mySet = new BinarySet32(min,max);
            });

        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(2, 33)]
        [DataRow(1, 32)]
        public void TestInitGoodRange(int min, int max)
        {
            ISet<int> mySet = new BinarySet32(1, 32);

        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(0, 0, 0)]
        [DataRow(2, 2, 33)]
        [DataRow(33, 2, 33)]
        [DataRow(1, 1, 32)]
        [DataRow(32, 1, 32)]
        [DataRow(14, 1, 32)]
        public void TestAddNumberInRange(int number, int minNum, int maxNum)
        {
            ISet<int> mySet = new BinarySet32(minNum, maxNum);
            Assert.IsFalse(mySet.Contains(number));
            Assert.IsTrue(mySet.Add(number));
            Assert.IsTrue(mySet.Contains(number));
        }

        private void AddNumbers(ISet<int> ints, int lowest, int highest)
        {
            for (int i = lowest; i <= highest; i++)
            {
                Assert.IsFalse(ints.Contains(i));
                ints.Add(i);
                Assert.IsTrue(ints.Contains(i));
            }
        }


        [TestMethod]
        public void TestIsFasterThanHashSet()
        {
            Stopwatch hashStopWatch = new Stopwatch();
            Stopwatch binStopWatch = new Stopwatch();

            // run lots of times to negate the advantage afforded by caching to sets run last
            for (int i = 0; i < 100; i++)
            {
                binStopWatch.Start();
                ISet<int> binSet = new BinarySet32(1, 32);
                AddNumbers(binSet, 1, 32);
                binStopWatch.Stop();

                hashStopWatch.Start();
                ISet<int> hashSet = new HashSet<int>();
                AddNumbers(hashSet, 1, 32);
                hashStopWatch.Stop();
            }



            string message = String.Format("HashSet took {0} microseconds. BinarySet32 took {1} microseconds.",
                hashStopWatch.Elapsed.TotalMicroseconds, binStopWatch.Elapsed.TotalMicroseconds);
            Assert.IsTrue(hashStopWatch.Elapsed.TotalMicroseconds > binStopWatch.Elapsed.TotalMicroseconds, message);
        }

        [TestMethod]
        public void TestCountAccurate()
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            Assert.AreEqual(0, binSet.Count);
            binSet.Add(5);
            Assert.AreEqual(1, binSet.Count);
            binSet.Add(21);
            Assert.AreEqual(2, binSet.Count);
        }


        [TestMethod]
        public void TestCorrectAddedNumberRemoved()
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            AddNumbers(binSet, 1, 32);
            Assert.IsTrue(binSet.Remove(5));
            for (int i = 1; i <= 32; i++)
            {
                if (i == 5)
                {
                    Assert.IsFalse(binSet.Contains(i));
                    continue;
                }
                Assert.IsTrue(binSet.Contains(i));
            }
        }

        private int[] GetIntArrFromArrString(string arrString)
        {
            IList<int> list = new List<int>();
            string[] arr1 = arrString.Split(',');
            foreach (var item in arr1)
            {
                list.Add(int.Parse(item));
            }
            return list.ToArray();
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2", false)]
        [DataRow("1,2,3", "1,2", true)]
        [DataRow("1,2", "1,4", false)]
        [DataRow("1,2", "1,2,3", false)]
        [DataRow("1,2", "1,2,33", false)]
        [DataRow("1,2","1,2,99", false)]
        public void TestIsProperSuperset(string arr1String, string arr2String, bool expected)
        {
            ISet<int> binSet = new BinarySet32(1,32);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> hashSet = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.AreEqual(expected, binSet.IsProperSupersetOf(hashSet));
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2", true)]
        [DataRow("1,2,3", "1,2", true)]
        [DataRow("1,2", "1,4", false)]
        [DataRow("1,2", "1,2,3", false)]
        [DataRow("1,2", "1,2,33", false)]
        [DataRow("1,2", "1,2,99", false)]
        public void TestIsSuperset(string arr1String, string arr2String, bool expected) 
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> hashSet = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.AreEqual(expected, binSet.IsSupersetOf(hashSet));
        }


        [DataTestMethod]
        [DataRow("1,2", "1,2", true)]
        [DataRow("1,2", "3,2", true)]
        [DataRow("1,2", "3,4", false)]
        [DataRow("1,2", "0,3", false)]
        [DataRow("1,2", "0,2", true)]
        public void TestOverlaps(string arr1String, string arr2String, bool expected)
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> hashSet = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.AreEqual(expected, binSet.Overlaps(hashSet));
        }


        [DataTestMethod]
        [DataRow("1,2", "1,2", true)]
        [DataRow("1,2", "1,2,3", false)]
        [DataRow("1,2,3", "1,2", false)]
        [DataRow("1", "1,200", false)]
        [DataRow("1,2", "1,200", false)]
        public void TestSetEquals(string arr1String, string arr2String, bool expected)
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> hashSet = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.AreEqual(expected, binSet.SetEquals(hashSet));

        }

        [TestMethod]
        public void TestUnionWith()
        {
            ISet<int> binSet = new BinarySet32(1, 3) { 1, 2 };
            ISet<int> ints = new HashSet<int>{2,3};
            Assert.IsTrue(binSet.Contains(1));
            Assert.IsTrue(binSet.Contains(2));
            Assert.IsFalse(binSet.Contains(3));
            Assert.AreEqual(2, binSet.Count);
            binSet.UnionWith(ints);
            Assert.IsTrue(binSet.Contains(1));
            Assert.IsTrue(binSet.Contains(2));
            Assert.IsTrue(binSet.Contains(3));
            Assert.AreEqual(3, binSet.Count);

        }


        [DataTestMethod]
        [DataRow("1,2", "1,2", "")]
        [DataRow("1,2", "3,4", "1,2,3,4")]
        [DataRow("1,2", "3,2", "1,3")]
        public void TestSymmetricExceptWith(string arr1String, string arr2String, string expectedArrString)
        {
            ISet<int> binSet = new BinarySet32(1, 10);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> ints = new HashSet<int>(GetIntArrFromArrString(arr2String));
            binSet.SymmetricExceptWith(ints);
            if (expectedArrString == "")
            {
                Assert.AreEqual(0, binSet.Count);
                return;
            }
            int[] answers = GetIntArrFromArrString(expectedArrString);
            Assert.IsTrue(binSet.SetEquals(answers));
            
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2,200")] // should error
        [DataRow("1,2", "1,200")] // should error
        [DataRow("1,2", "1,2,11")] // should error
        [DataRow("1,2", "1,11")] // should error
        public void TestSymmetricExceptWithErrors(string arr1String, string arr2String)
        {
            ISet<int> binSet = new BinarySet32(1, 10);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> ints = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.ThrowsException<ArgumentException>(() => binSet.SymmetricExceptWith(ints));

        }

        [DataTestMethod]
        [DataRow(33, 1, 32)]
        [DataRow(0, 1, 32)]
        public void TestAddOutOfRange(int number, int minNumber, int maxNumber)
        {
            ISet<int> binSet = new BinarySet32(minNumber, maxNumber);
            Assert.ThrowsException<ArgumentException>(() => binSet.Add(number));
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2", "1,2")]
        [DataRow("1,2", "1,3", "1")]
        [DataRow("1,2", "3,4", "")]
        public void TestIntersectWith(string arr1String, string arr2String, string expectedArrString)
        {
            ISet<int> binSet = new BinarySet32(1, 10);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> ints = new HashSet<int>(GetIntArrFromArrString(arr2String));
            binSet.IntersectWith(ints);
            if (expectedArrString == "")
            {
                Assert.AreEqual(0, binSet.Count);
                return;
            }
            int[] answers = GetIntArrFromArrString(expectedArrString);
            Assert.IsTrue(binSet.SetEquals(answers));
        }


        [DataTestMethod]
        [DataRow("1,2", "1,2", "")]
        [DataRow("1,2", "1,3", "2")]
        [DataRow("1,2", "3,4", "1,2")]
        public void TestExceptWith(string arr1String, string arr2String, string expectedArrString)
        {
            ISet<int> binSet = new BinarySet32(1, 10);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> ints = new HashSet<int>(GetIntArrFromArrString(arr2String));
            binSet.ExceptWith(ints);
            if (expectedArrString == "")
            {
                Assert.AreEqual(0, binSet.Count);
                return;
            }
            int[] answers = GetIntArrFromArrString(expectedArrString);
            Assert.IsTrue(binSet.SetEquals(answers));
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2,0,0",1, "1,1,2,0")]
        [DataRow("1,2", "1,2,0,0", 0, "1,2,0,0")]
        [DataRow("1,2", "1,2,0,0", 2, "1,2,1,2")]
        public void TestCopyTo(string setArrString, string arrString, int index, string expectedArrString)
        {
            ISet<int> binSet = new BinarySet32(1, 10);
            binSet.UnionWith(GetIntArrFromArrString(setArrString));
            var arr = GetIntArrFromArrString(arrString);
            var expectedArr = GetIntArrFromArrString(expectedArrString);
            binSet.CopyTo(arr, index);
            for (int i = 0; i < expectedArr.Length; i++)
            {
                Assert.AreEqual(expectedArr[i], arr[i]);
            }
        }

        [TestMethod]
        public void TestEnumerator()
        {
            ISet<int> binSet = new BinarySet32(1, 10);
            binSet.UnionWith(GetIntArrFromArrString("1,2,3,4,5"));
            int count = 1;
            foreach (int i in binSet)
            {
                Assert.AreEqual(count, i);
                count++;
            }
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2", true)]
        [DataRow( "1,2", "1,2,3", true)]
        [DataRow( "1,4", "1,2", false)]
        [DataRow( "1,2,3", "1,2", false)]
        [DataRow("1,2", "1,2,33", true)]
        [DataRow("1,2", "1,2,99", true)]
        public void TestIsSubsetOf(string arr1String, string arr2String, bool expected)
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> hashSet = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.AreEqual(expected, binSet.IsSubsetOf(hashSet));
        }

        [DataTestMethod]
        [DataRow("1,2", "1,2", false)]
        [DataRow("1,2", "1,2,3", true)]
        [DataRow("1,4", "1,2", false)]
        [DataRow("1,2,3", "1,2", false)]
        [DataRow("1,2", "1,2,33", true)]
        [DataRow("1,2", "1,2,99", true)]
        public void TestIsProperSubsetOf(string arr1String, string arr2String, bool expected)
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            binSet.UnionWith(GetIntArrFromArrString(arr1String));
            ISet<int> hashSet = new HashSet<int>(GetIntArrFromArrString(arr2String));
            Assert.AreEqual(expected, binSet.IsProperSubsetOf(hashSet));
        }

    }
}