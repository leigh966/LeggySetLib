
namespace TestLeggySetLib
{
    [TestClass]
    public class TestAlphabetSet
    {
        [DataTestMethod]
        [DataRow('a', 'A')]
        [DataRow('z', 'Z')]
        [DataRow('A', 'a')]
        [DataRow('Z', 'z')]
        public void TestAddedValuePresent(char toAdd, char toContain)
        {
            ISet<char> charset = new AlphabetSet();
            Assert.IsFalse(charset.Contains(toContain));
            charset.Add(toAdd);
            Assert.IsTrue(charset.Contains(toContain));
        }


        [DataTestMethod]
        [DataRow('@')]
        [DataRow((char)60)]
        [DataRow('[')]
        [DataRow('{')]
        public void TestAddBadValue(char badChar)
        {
            ISet<char> charset = new AlphabetSet();
            Assert.ThrowsException<NotALetterException>(()=>charset.Add(badChar));
        }
    }
}
