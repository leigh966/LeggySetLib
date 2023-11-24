using System.Collections;

namespace LeggySetLib
{
    public class BinarySet64 : BinarySet<ulong>, ISet<int>
    {
        
        public override int NUMBER_OF_BITS => 64;

        #region Initialization

        public BinarySet64(int minimumNumber, int maximumNumber) : base(minimumNumber, maximumNumber) 
        {
            setBin = 0;
        }

        public BinarySet64(int minimumNumber, int maximumNumber, IEnumerable<int> ints) : base(minimumNumber, maximumNumber, ints) 
        {
            setBin = 0;
        }
        #endregion

        #region Private_Methods
        private ulong GetRepresentationOfCollection(IEnumerable<int> collection, bool ignoreException)
        {
            ulong rep = 0;
            foreach (int item in collection)
            {
                if (ignoreException && (item < minNum || item > maxNum))
                {
                    continue;
                }
                rep |= GetRepresentingBinary(item);
            }
            return rep;
        }

        private ulong GetRepresentingBinary(int number)
        {
            if (number > maxNum || number < minNum)
                throw new ArgumentException("Number " + number.ToString() + " not in range " + minNum.ToString() + " - " + maxNum.ToString());
            int position = number - minNum;
            return (ulong)(1 << position);
        }

        private bool ContainsRepresentation(ulong rep)
        {
            return (setBin & rep) != 0;
        }
        #endregion

        #region Public_Properties
        public override int Count
        {
            get
            {
                int count = 0;
                ulong binaryNumber = setBin;
                while (binaryNumber != 0)
                {
                    binaryNumber &= (binaryNumber - 1);
                    count++;
                }
                return count;
            }
        }

        #endregion

        #region Basic_Operations
        public override bool Add(int item)
        {
            ulong rep = GetRepresentingBinary(item);
            bool contains = ContainsRepresentation(rep);
            setBin |= rep;
            return !contains;
        }
        void ICollection<int>.Add(int item)
        {
            Add(item);
        }
        public override void Clear()
        {
            setBin = 0;
        }

        public override bool Contains(int item)
        {
            ulong rep = GetRepresentingBinary(item);
            return ContainsRepresentation(rep);
        }

        public override bool Remove(int item)
        {
            ulong rep = GetRepresentingBinary(item);
            bool contains = ContainsRepresentation(rep);
            setBin &= ~rep;
            return contains;
        }
        #endregion

        #region Enumerable_Operations


        public override void ExceptWith(IEnumerable<int> other)
        {
            ulong rep = GetRepresentationOfCollection(other, true);
            setBin &= ~rep;
        }
        public override void IntersectWith(IEnumerable<int> other)
        {
            ulong rep = GetRepresentationOfCollection(other, true);
            setBin &= rep;
        }


        public override bool Overlaps(IEnumerable<int> other)
        {
            ulong rep = GetRepresentationOfCollection(other, true);
            return ContainsRepresentation(rep);
        }



        public override bool SetEquals(IEnumerable<int> other)
        {
            if (other.Count() != Count) return false;
            try
            {
                ulong rep = GetRepresentationOfCollection(other, false);
                return rep == setBin;
            }
            catch (ArgumentException)
            {
                return false;
            }

        }

        public override void SymmetricExceptWith(IEnumerable<int> other)
        {
            ulong rep = GetRepresentationOfCollection(other, false);
            setBin ^= rep;
        }

        public override void UnionWith(IEnumerable<int> other)
        {

            foreach (var item in other)
            {
                setBin |= GetRepresentingBinary(item);
            }

        }
        #endregion

        #region Get_Enumerator
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
