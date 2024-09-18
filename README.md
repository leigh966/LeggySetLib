# LeggySetLib

This library provides NET Framework C# class libraries for a 32 bit binary set and 64 bit binary set. The basic principle is that, instead of storing the numbers that we add to the set, we keep an index of a known range of numbers where each bit represents whether it is present in the set.

Since many of the operations on this set are actually just basic binary arithmetic, these operations are exceptionally fast (provably much faster than the HashSet provided by C#)! The sets are not just fast either; since the representation depends on flipping bits within a single int(32 bit) or long(64 bit), the size of the set is consistent no matter how many values it holds making it more memory efficient than almost any other data type for storing large numbers of entries. 

## Use
### Instantiation
If you need to store numbers in a range of 32 or less, you can use BinarySet32 like this:
```c#
ISet<int> numbersSet = new BinarySet32(33,64) // Setting 33 as our minimum number and 64 as our maximum number - this range must include 32 values or less
```
or if you need a range bigger than 32 but <= 64, you can use BinarySet64 like this:
```c#
ISet<int> numbersSet = new BinarySet64(1,64) // Setting 1 as our minimum number and 64 as our maximum number - this range must include 64 values or less
```
