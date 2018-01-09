using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GridView : EventView
    {
        public int size = 10;
        public GameObject blockPrefab;
        GameObject[,] blockArray;

        public void Init()
        {
            blockArray = new GameObject[size, size];
        }

        #region Show and Colour Block Methods
        /// <summary>
        /// Create or show blocks at the shape position
        /// </summary>
        /// <param name="worldPositions"></param>
        /// <param name="color"></param>
        public void ShowBlocks(List<Vector3> worldPositions, Color color)
        {
            List<Index> indexList = ConvertWorldPosToIndex(worldPositions);
            EnableBlocks(indexList);
            ColourBlocks(indexList, color);
        }

        private void EnableBlocks(List<Index> indexes)
        {
            indexes.ForEach(index =>EnableBlock(index));
        }

        private void ColourBlocks(List<Index> indexes, Color color)
        {
            indexes.ForEach(index =>SetBlockColor(index, color));
        }

        private void EnableBlock(Index index)
        {
            GameObject block = getBlock(index);
            if (block == null)
                block = InstantiateBlock(index);

            block.SetActive(true);
            blockArray[index.x, index.y] = block;
        }

        private GameObject InstantiateBlock(Index index)
        {
            Vector3 position = GetBlockWorldPosition(index);
            return Instantiate(blockPrefab, position, Quaternion.identity, transform);
        }

        private void SetBlockColor(Index index, Color color)
        {
            GameObject block = getBlock(index);
            if (block == null)
            {
                Debug.LogWarning("Block for set color not found. " +
                    "Index x: " + index.x + " y: " + index.y);
                return;
            }
            block.GetComponent<SpriteRenderer>().color = color;
        }
        #endregion

        /// <summary>
        /// Get block world position that calculate form grid BoxCollider2D
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector3 GetBlockWorldPosition(Index index)
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            float gridWorldSize = collider.size.x;
            Vector2 startPoint = transform.position - gridWorldSize / 2f * Vector3.right
                - gridWorldSize / 2f * Vector3.up;
            float blockScale = gridWorldSize / size;
            float xPos = startPoint.x + (index.x + .5f) * blockScale;
            float yPos = startPoint.y + (index.y + .5f) * blockScale;
            Vector3 position = new Vector3(xPos, yPos, 0);

            return position;
        }

        private GameObject getBlock(Index index)
        {
            return blockArray[index.x, index.y];
        }

        public void UpdateGrid(bool[,] boolArray)
        {
            if (boolArray.GetLength(0) != size || boolArray.GetLength(1) != size)
            {
                Debug.LogError("The size of bool view of grid not match gameobject view");
                return;
            }
            for(int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (blockArray[x, y] != null)
                        blockArray[x, y].SetActive(boolArray[x, y]);
                }
            }
        }

        public void Reset()
        {
            for(int x = 0; x < size; x++)
            {
                for(int y = 0; y < size; y++)
                {
                    if (blockArray[x, y] != null)
                        blockArray[x, y].SetActive(false);
                }
            }
        }

        public List<Index> ConvertWorldPosToIndex(List<Vector3> worldPosList)
        {
            List<Index> indexPosition = new List<Index>();
            worldPosList.ForEach(
                pos=>indexPosition.Add(new Index(GetXGridPosition(pos.x), 
                GetYGridPosition(pos.y))));
            return indexPosition;
        }

        private int GetXGridPosition(float xWorldPos)
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            float gridWorldSize = collider.size.x;
            float startPoint = transform.position.x - gridWorldSize / 2f; 
            float blockScale = gridWorldSize / size;
            List<float> blockPositions = new List<float>();
            for (int i = 0; i < size; i++)
                blockPositions.Add(startPoint + (i + .5f) * blockScale);
            int indexX = 0;
            float minDistance = Mathf.Abs(xWorldPos - blockPositions[0]);
            for(int x = 1; x < size; x++)
            {
                if(Mathf.Abs(xWorldPos - blockPositions[x]) < minDistance)
                {
                    indexX = x;
                    minDistance = Mathf.Abs(xWorldPos - blockPositions[x]);
                }
            }
            return indexX;
        }

        private int GetYGridPosition(float yWorldPos)
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            float gridWorldSize = collider.size.y;
            float startPoint = transform.position.y - gridWorldSize / 2f;
            float blockScale = gridWorldSize / size;
            List<float> blockPositions = new List<float>();
            for (int i = 0; i < size; i++)
                blockPositions.Add(startPoint + (i + .5f) * blockScale);
            int indexY = 0;
            float minDistance = Mathf.Abs(yWorldPos - blockPositions[0]);
            for (int y = 1; y < size; y++)
            {
                if (Mathf.Abs(yWorldPos - blockPositions[y]) < minDistance)
                {
                    indexY = y;
                    minDistance = Mathf.Abs(yWorldPos - blockPositions[y]);
                }
            }
            return indexY;
        }

    }
}
