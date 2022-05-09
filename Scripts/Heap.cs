using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];

    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;//one less node, decrease size
        items[0] = items[currentItemCount];//tackes last leaf and puts it at root
        items[0].heapIndex = 0;
        SortDown(items[0]);// pushes value to correct place

        return firstItem; //return removed item
    }
    public void updateItem(T item)
    {
        SortUp(item);//we only inc priority so dont need sort down
    }
    public int Count//returns item count, need this to fetch the data out of the heap class
    {
        get
        {
            return currentItemCount;
        }
    }
    public bool Contains(T item)//checks if there is a corresponding item in the items heap
    {
        return Equals(items[item.heapIndex], item);
    }
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRight = item.heapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)//if there any children in index bounds
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)//if there is a right child and its smaller swap
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)//swap withleft/right child
                {
                    Swap(item, items[swapIndex]);
                }
                else // it is in the right place, and children are both larger
                {
                    return;
                }

            }
            else//there are no children so end loop
            {
                return;
            }

        }
    }
    void SortUp(T item)// this inserts a value to the tree and orders it to the top
    {
        int parentIndex = (item.heapIndex-1)/2;
        while (true)
        {
            T parent = items[parentIndex];
            if (item.CompareTo(parent) > 0)
            {
                Swap(parent, item);
            }
            else
            {
                break;
            }
            parentIndex = (item.heapIndex - 1) / 2;
        }

    }

    void Swap( T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int temp = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = temp;

    }
}
public interface IHeapItem<T> : IComparable<T>//interface means that no finction can be defined here. the information is moved through between scripts here
    //COmparable decentralizes the CompareTo function definition to where this class is defined
    //this allows the user to define the data type that is compared in compare to
    //this makes the code applicable to any data type (tree of anything, we define it)
{

    int heapIndex
    {
        get;
        set;
    }
}