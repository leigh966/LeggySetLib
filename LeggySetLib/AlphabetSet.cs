using System.Collections;

namespace LeggySetLib
{
    public class AlphabetSet : ISet<char>
    {
        public const int minNumber = (int)'a', maxNumber = (int)'z';
        private ISet<int> binarySet = new BinarySet32(minNumber, maxNumber);
        public AlphabetSet()
        {

        }

        public int Count => binarySet.Count;

        public bool IsReadOnly => binarySet.IsReadOnly;

        public int GetValue(char item)
        {
            return (int)item.ToString().ToLower()[0];
        }

        public bool Add(char item)
        {
            if ((int)item < minNumber || (int)item > maxNumber) throw new NotALetterException(item, "Cannot add chars that aren't letters!");
            int value = GetValue(item);
            return binarySet.Add(value);
        }

        public void Clear()
        {
            binarySet.Clear();
        }

        public bool Contains(char item)
        {
            int value = GetValue(item);
            return binarySet.Contains(value);
        }

        public void CopyTo(char[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<char> other)
        {
            throw new NotImplementedException();
        }




        public bool IsProperSubsetOf(IEnumerable<char> other)
        {
            return IsSubsetOf(other) && Count < other.Count();
        }

        public bool IsProperSupersetOf(IEnumerable<char> other)
        {
            return IsSupersetOf(other) && Count > other.Count();
        }
        public bool Overlaps(IEnumerable<char> other)
        {
            foreach(char letter in other)
            {
                if(Contains(letter)) return true;
            }
            return false;
        }


        public bool IsSubsetOf(IEnumerable<char> other)
        {
            if (Count > other.Count()) return false;
            foreach (char item in ToArray())
            {
                if (!other.Contains(item)) return false;
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<char> other)
        {
            if (other.Count() > Count) return false;
            foreach (char item in other)
            {
                if (!Contains(item)) return false;
            }
            return true;
        }

        public bool Remove(char item)
        {
            int value = GetValue(item);
            return binarySet.Remove(value);
        }

        public bool SetEquals(IEnumerable<char> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<char> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<char> other)
        {
            throw new NotImplementedException();
        }

        void ICollection<char>.Add(char item)
        {
            Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {

            return GetEnumerator();

        }

        private char[] ToArray()
        {

            List<char> list = new List<char>();
            for (int i = minNumber; i <= maxNumber; i++)
            {
                if (Contains((char)i)) list.Add((char)i);
            }
            return list.ToArray();

        }

        public void IntersectWith(IEnumerable<char> other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<char> GetEnumerator()
        {
            foreach (char item in ToArray())
            {
                yield return item;
            }
        }
    }
}
