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

        public void EnableBlocks(List<Index> indexes)
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

        public void ClearGrid()
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
    }
}
