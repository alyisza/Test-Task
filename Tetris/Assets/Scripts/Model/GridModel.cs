using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridModel : IGrid {

    private int _size;
    public int Size
    {
        get
        {
            return _size;
        }
    }

    private bool[,] _gridArray;
    public bool[,] GridArray
    {
        get
        {
            return _gridArray;
        }
    }

    public GridModel()
    {

    }

    public GridModel(int size)
    {
        Reset(size);
    }

    public void AddShape(List<Index> indexList)
    {
        indexList.ForEach(index => _gridArray[index.x, index.y] = true);
    }

    public bool CheckAvailabilityForShape(bool[,] shapeInBoolArray)
    {
        for(int x =0; x < Size - shapeInBoolArray.GetLength(0); x++)
        {
            for(int y = 0; y < Size - shapeInBoolArray.GetLength(1); y++)
            {
                if (CheckAvailabilityInGridPart(shapeInBoolArray, new Index(x, y)))
                    return true;
            }
        }
        return false;
    }

    private bool CheckAvailabilityInGridPart(bool[,] shapeInBoolArray, Index startIndex)
    {
        if (startIndex.x + shapeInBoolArray.GetLength(0) - 1 > Size 
            || startIndex.y + shapeInBoolArray.GetLength(1) - 1 > Size)
            return false; 

        for(int x = 0; x < shapeInBoolArray.GetLength(0); x++)
        {
            for(int y = 0; y < shapeInBoolArray.GetLength(1); y++)
            {
                //if in part of grid array required cell is occupied, this part is unavailable
                if (shapeInBoolArray[x, y] && GridArray[x + startIndex.x, y + startIndex.y])
                    return false;
            }
        }
        return true;
    }

    public void DeleteColumn(int columnId)
    {
        for (int y = 0; y < Size; y++)
            _gridArray[columnId, y] = false;
    }

    public void DeleteRow(int rowId)
    {
        for (int x = 0; x < Size; x++)
            _gridArray[x, rowId] = false;
    }

    public void Reset(int size)
    {
        this._size = size;
        _gridArray = new bool[size, size];
    }
    /// <summary>
    /// return count of destroyed
    /// </summary>
    /// <returns></returns>
    public int CheckRowsAndColumns()
    {
        int count = 0;
        for(int y = 0; y < Size; y++)
        {
            bool fill = true;
            for(int x = 0; x < Size; x++)
            {
                if (GridArray[x, y] == false)
                {
                    fill = false;
                    break;
                }
            }
            if (fill)
            {
                DeleteRow(y);
                count++;
            }
        }

        for (int x = 0; x < Size; x++)
        {
            bool fill = true;
            for (int y = 0; y < Size; y++)
            {
                if (GridArray[x, y] == false)
                {
                    fill = false;
                    break;
                }
            }
            if (fill)
            {
                DeleteColumn(x);
                count++;
            }
        }
        return count;
    }
}
