using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using strange.extensions.mediation.impl;
using strange.extensions.mediation.api;

namespace Game
{
    public class ShapeView : EventView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private const string blockNameInHierarchy = "Block";
        private Vector3 startSize;
        private Vector3 dragSize;
        private Vector3 startDragPosition;
        private LayerMask gridLayer;
        private LayerMask blockLayer;

        [Inject]
        public PutShapeSignal putShapeSignal { get; set; }
        /// <summary>
        /// Call signal notifies that a shape is added at the board
        /// </summary>
        void PutOnBoard()
        {
            gameObject.SetActive(false);
            putShapeSignal.Dispatch(this);
        }

        public void SetData(Vector3 startSize, Vector3 onBoardSize,
            LayerMask gridLayer, LayerMask boardBlockLayer)
        {
            this.startSize = startSize;
            this.dragSize = onBoardSize;
            this.gridLayer = gridLayer;
            this.blockLayer = boardBlockLayer;
        }


        public Color GetShapeColor()
        {
            Color color = transform.Find(blockNameInHierarchy).
                GetComponent<SpriteRenderer>().color;
            return color;
        }

        #region IBeginDragHandler implementation 
        public void OnBeginDrag(PointerEventData eventData)
        {
            startDragPosition = transform.position;
            transform.localScale = dragSize;
        }
        #endregion

        #region IDragHandler implementation
        public void OnDrag(PointerEventData eventData)
        {
            Vector3 mousePosToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosToWorldPos.z = 0;
            transform.position = mousePosToWorldPos;
        }
        #endregion

        #region IEndDragHandler implementation
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsValidShapePosition())
            {
                transform.position = startDragPosition;
                transform.localScale = startSize;
            }
            else
            {
                PutOnBoard();
            }
        }
        #endregion

        #region Check Valid Position 
        bool IsValidShapePosition()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name != blockNameInHierarchy) continue;
                if (!IsValidTransformPosition(child))
                    return false;
            }
            return true;
        }

        bool IsValidTransformPosition(Transform transf)
        {
            int raycastLayer = gridLayer | blockLayer;
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transf.position, Vector2.zero, distance: 3f, layerMask: raycastLayer);

            if (hit.collider == null)
                return false;
            return (1 << hit.collider.gameObject.layer == gridLayer);
        }
        #endregion

        /// <summary>
    /// Shape in bool array: occuipied cell - true, empty - false;
    /// </summary>
    /// <returns></returns>
        public bool[,] GetShapeInBoolArrray()
        {
            //во избежание ошибок из-за погрешностей
            float errorDistance = .1f;
            List<Vector3> positions = GetLastWorldPosition();

            //calculate column count
            positions = positions.OrderBy(pos => pos.x).ToList();
            List<float> xPositions = new List<float>();
            xPositions.Add(positions[0].x);
            for (int x = 1; x < positions.Count; x++)
            {
                float xPos = positions[x].x;
                float lastAddedX = xPositions[xPositions.Count - 1];
                if (Mathf.Abs(xPos - lastAddedX) > errorDistance)
                    xPositions.Add(xPos);
            }

            //calculate row count
            positions = positions.OrderBy(pos => pos.y).ToList();
            List<float> yPositions = new List<float>();
            yPositions.Add(positions[0].y);
            for (int y = 1; y < positions.Count; y++)
            {
                float yPos = positions[y].y;
                float lastAddedY = yPositions[yPositions.Count - 1];
                if (Mathf.Abs(yPos - lastAddedY) > errorDistance)
                    yPositions.Add(yPos);
            }

            int xCount = xPositions.Count;
            int yCount = yPositions.Count;
            bool[,] boolArray = new bool[xCount, yCount];

            positions.ForEach(pos =>
            {
                int nearestX = GetNearlyIndex(pos.x, xPositions);
                int nearestY = GetNearlyIndex(pos.y, yPositions);
                boolArray[nearestX, nearestY] = true;
            });

            return boolArray;
        }
        /// <summary>
        /// Get nearly index to value from listOfValues
        /// </summary>
        /// <param name="value"></param>
        /// <param name="listOfValues"></param>
        /// <returns></returns>
        private static int GetNearlyIndex(float value, List<float> listOfValues)
        {
            int index = 0;
            float minDistance = value - listOfValues[0];
            for (int i = 0; i < listOfValues.Count; i++)
            {
                if (Mathf.Abs(value - listOfValues[i]) < minDistance)
                    index = i;
                minDistance = Mathf.Abs(value - listOfValues[i]);
            }
            return index;
        }

        /// <summary>
        /// Last position of cells before set shape at grid.
        /// For creating grid cells at grid.
        /// </summary>
        /// <returns></returns>
        public List<Vector3> GetLastWorldPosition()
        {
            List<Vector3> positionsList = new List<Vector3>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name != blockNameInHierarchy) continue;
                positionsList.Add(child.position);
            }
            return positionsList;
        }
    }
}