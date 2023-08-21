using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T>
{
  T[] heapItems;
  private int Count;

  public int Length {
    get {
      return Count;
    } 
  }

  public Heap(int maxSize){
    heapItems = new T[maxSize];
    Count = 0;
  }

  public void Add(T item){
    //Adding an item puts it at the very bottom of the tree/array/ SO 
    item.HeapIndex = Count;
    heapItems[Count] = item;
    Sort(item);
    Count++;
  }

  public void Update(T item){
    if(!Contains(item)) return;
    Sort(item);
    // Have to update the index Sort(item);
  }

  public T getTop(){
    if(Count == 0) return default;
    T top = heapItems[0];
    T replace = heapItems[Count-1];
    Swap(top, replace);
    heapItems[Count-1] = default;
    Count--;
    while(true){
      int LChildNum = replace.HeapIndex*2+1;
      int RChildNum = replace.HeapIndex*2+2;
      int swapIndex = 0;
      if(LChildNum < Count){
        swapIndex = LChildNum;
          if(RChildNum < Count){
            swapIndex = heapItems[RChildNum].CompareTo(heapItems[LChildNum]) > 0 
                        ? RChildNum : LChildNum;
          }
          if(replace.CompareTo(heapItems[swapIndex]) > -1){
            return top;
          }

        Swap(replace, heapItems[swapIndex]);

      } else {
        return top;
      }
    }
  }

  //why is this here?? why is this needed?
  public bool Contains (T item) {
    return Equals(heapItems[item.HeapIndex], item);
  }

  private void Sort(T item){
    T itemToSort = item;
    int parentIndex = (item.HeapIndex-1)/2;

    while(true){
      if(item.CompareTo(heapItems[parentIndex]) > 0){
        Swap(item, heapItems[parentIndex]);
        parentIndex = (parentIndex-1)/2;
      } else {
        return;
      }
    }
  }

  private void Swap(T item1, T item2){
    int index1 = item1.HeapIndex;
    int index2 = item2.HeapIndex;
    T swapItem1 = heapItems[index1];

    heapItems[index1] = heapItems[index2];
    heapItems[index2] = swapItem1;

    heapItems[index1].HeapIndex = index1;
    heapItems[index2].HeapIndex = index2;
  }


}

public interface IHeapItem<T>:IComparable<T>{
    int HeapIndex {
        get;
        set;
    }

}
