using System.Diagnostics;

namespace TestLeggySetLib
{
    [TestClass]
    public class TestBinarySet32
    {
        [TestMethod]
        public void TestInitRangeTooBig()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ISet<int> mySet = new BinarySet32(0, 32);
            });
            
        }

        [TestMethod]
        public void TestInitRangeTooSmall()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                ISet<int> mySet = new BinarySet32(1,0);
            });

        }

        [TestMethod]
        public void TestInitGoodRange()
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
            for(int i = lowest; i <= highest; i++)
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
            for(int i = 0; i < 100; i++)
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
    }
}