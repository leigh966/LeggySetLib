# LeggySetLib

## What?
A .NET Framework C# class library providing a 32 bit binary set and 64 bit binary set. These sets make use of a single binary number to store information on which numbers are contained within them based on bits flipped and a defined range of numbers that the set can hold.

## Why?
Since many of the operations on this set are actually just basic binary arithmatic, these operations are exeptionally fast (provably much faster than the HashSet provided by C#)! The sets are not just fast either; since the representation depends on flipping bits within a single int(32 bit) or long(64 bit), the size of the set is consistent no matter how many values it holds making it more memory efficient than almost any other data type for storing large numbers of entries. 

## Use
### Instantiation
```c#
ISet<int> numbersSet = new BinarySet32(33,64) // Setting 33 as our minimum number and 64 as our maximum number - this range must include 32 values or less
```
```c#
ISet<int> numbersSet = new BinarySet64(1,64) // Setting 1 as our minimum number and 64 as our maximum number - this range must include 64 values or less
```
