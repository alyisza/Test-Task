using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IGrid {

    int Size { get; }
    bool [,] GridArray { get; }

    void Reset(int size);
    void DeleteRow(int rowId);
    void DeleteColumn(int columnId);
    void AddShape(List<Index> indexList);
    bool CheckAvailabilityForShape(bool[,] shapeInBoolArray);
}
