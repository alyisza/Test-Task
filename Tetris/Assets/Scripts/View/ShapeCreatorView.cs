using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class ShapeCreatorView : EventView
    {
        public Transform[] PointsArray;
        public GameObject[] ShapePrefabs;
        public Transform ShapeContainer;

        [Header("Shape values")]
        public LayerMask GridLayer;
        public LayerMask BlockLayer;
        public Vector3 StartSize = Vector3.one;
        public Vector3 OnBoardSize = new Vector3(1.38f, 1.38f, 1f);

        private float[] rotation = new float[] { 0f, 90f, 180f, 270f };

        /// <summary>
        /// Create shapes at PointsArray
        /// </summary>
        public void UpdateShapes()
        {
            for (int i = 0; i < ShapeContainer.childCount; i++)
            {
                if (ShapeContainer.GetChild(i).gameObject.activeInHierarchy)
                    return;
            }
            ClearShapes();
            CreateShapes();
        }

        public void Reset()
        {
            ClearShapes();
            CreateShapes();
        }

        private void CreateShapes()
        {
            for (int i = 0; i < PointsArray.Length; i++)
            {
                Vector3 position = PointsArray[i].position;
                CreateShape(position);
            }
        }

        private void CreateShape(Vector3 position)
        {
            int shapeIndex = UnityEngine.Random.Range(0, ShapePrefabs.Length);

            GameObject shape = Instantiate(ShapePrefabs[shapeIndex], position,
                Quaternion.identity, ShapeContainer);

            Transform pivot = shape.transform.Find("Pivot");
            Vector3 positionOffset = pivot.position - shape.transform.position;

            float rotationAngle = rotation[UnityEngine.Random.Range(0, rotation.Length)];

            shape.transform.RotateAround(pivot.position, Vector3.forward, rotationAngle);

            shape.transform.position -= positionOffset;
            shape.AddComponent<ShapeView>();
            shape.GetComponent<ShapeView>().SetData(StartSize, OnBoardSize,
                GridLayer, BlockLayer);
        }

        /// <summary>
        /// Destroy all previous shapes
        /// </summary>
        private void ClearShapes()
        {
            for(int i = ShapeContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(ShapeContainer.GetChild(i).gameObject);
            }
        }
        
        public List<ShapeView> GetShapes()
        {
            List<ShapeView> list = new List<ShapeView>();
            for (int i = 0; i < ShapeContainer.childCount; i++)
            {
                GameObject shape = ShapeContainer.GetChild(i).gameObject;
                if (shape.activeInHierarchy)
                    list.Add(shape.GetComponent<ShapeView>());
            }
            return list;
        }
    }
}