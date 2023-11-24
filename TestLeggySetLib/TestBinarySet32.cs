using System.Diagnostics;

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
        public void TestNumberInRangeAdded()
        {
            ISet<int> mySet = new BinarySet32(1, 32);
            Assert.IsFalse(mySet.Contains(14));
            mySet.Add(14);
            Assert.IsTrue(mySet.Contains(14));
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

        [DataTestMethod]
        [DataRow(new int[2] { 1, 2 }, new int[2] { 1, 2 }, false)]
        [DataRow(new int[3] { 1, 2, 3 }, new int[2] { 1, 2 }, true)]
        [DataRow(new int[2] { 1, 2 }, new int[2] { 1, 4 }, false)]
        [DataRow(new int[2] { 1, 2 }, new int[3] { 1, 2, 3 }, false)]
        [DataRow(new int[2] { 1, 2 }, new int[3] { 1, 2, 33 }, false)]
        [DataRow(new int[2] { 1, 2 }, new int[3] { 1, 2, 99 }, false)]
        public void TestIsProperSuperset(int[] arr1, int[] arr2, bool expected)
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            foreach (var item in arr1)
            {
                binSet.Add(item);
            }
            ISet<int> hashSet = new HashSet<int>();
            foreach (var item in arr2)
            {
                hashSet.Add(item);
            }
            Assert.AreEqual(expected, binSet.IsProperSupersetOf(hashSet));
        }

        [DataTestMethod]
        [DataRow(new int[2] {1,2}, new int[2] { 1, 2 }, true)]
        [DataRow(new int[3] { 1, 2, 3 }, new int[2] { 1, 2 }, true)]
        [DataRow(new int[2] { 1, 2 }, new int[2] { 1, 4 }, false)]
        [DataRow(new int[2] { 1, 2 }, new int[3] { 1, 2, 3 }, false)]
        [DataRow(new int[2] { 1, 2 }, new int[3] { 1, 2, 33 }, false)]
        [DataRow(new int[2] { 1, 2 }, new int[3] { 1, 2, 99 }, false)]
        public void TestIsSuperset(int[] arr1, int[] arr2, bool expected) 
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            foreach(var item in arr1)
            {
                binSet.Add(item);
            }
            ISet<int> hashSet = new HashSet<int>();
            foreach (var item in arr2)
            {
                hashSet.Add(item);
            }
            Assert.AreEqual(expected, binSet.IsSupersetOf(hashSet));
        }

        [TestMethod]
        public void TestOverlapsTrue()
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            ISet<int> ints = new HashSet<int>();
            binSet.Add(1);
            binSet.Add(2);
            ints.Add(2);
            ints.Add(3);
            Assert.IsTrue(binSet.Overlaps(ints));
        }

        [TestMethod]
        public void TestOverlapsFalse()
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            ISet<int> ints = new HashSet<int>();
            binSet.Add(1);
            binSet.Add(2);
            ints.Add(4);
            ints.Add(3);
            Assert.IsFalse(binSet.Overlaps(ints));
        }

        [TestMethod]
        public void TestSetEquals()
        {
            ISet<int> binSet = new BinarySet32(1, 32);
            ISet<int> ints = new HashSet<int>();
            Assert.IsTrue(binSet.SetEquals(ints));
            binSet.Add(10);
            Assert.IsFalse(binSet.SetEquals(ints));
            ints.Add(10);
            Assert.IsTrue(binSet.SetEquals(ints));
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


        [TestMethod]
        public void TestSymmetricExceptWith()
        {
            ISet<int> binSet = new BinarySet32(1, 3) { 1, 2 };
            ISet<int> ints = new HashSet<int> { 2, 3 };
            Assert.IsTrue(binSet.Contains(1));
            Assert.IsTrue(binSet.Contains(2));
            Assert.IsFalse(binSet.Contains(3));
            Assert.AreEqual(2, binSet.Count);
            binSet.SymmetricExceptWith(ints);
            Assert.IsTrue(binSet.Contains(1));
            Assert.IsTrue(binSet.Contains(3));
            Assert.IsFalse(binSet.Contains(2));
            Assert.AreEqual(2, binSet.Count);
        }

        [DataTestMethod]
        [DataRow(33, 1, 32)]
        [DataRow(0, 1, 32)]
        public void TestAddOutOfRange(int number, int minNumber, int maxNumber)
        {
            ISet<int> binSet = new BinarySet32(minNumber, maxNumber);
            Assert.ThrowsException<ArgumentException>(() => binSet.Add(number));
        }
    }
}