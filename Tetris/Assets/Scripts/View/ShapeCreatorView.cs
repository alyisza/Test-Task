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

        /// <summary>
        /// Create shapes at PointsArray
        /// </summary>
        public void CreateShapes()
        {
            if (ShapeContainer.childCount != 0)
                return;

            for(int i = 0; i < PointsArray.Length; i++)
            {
                int shapeIndex = UnityEngine.Random.Range(0, ShapePrefabs.Length);
                GameObject shape = Instantiate(ShapePrefabs[shapeIndex], PointsArray[i].position,
                    Quaternion.identity, ShapeContainer);
                Vector3 positionOffset = shape.transform.Find("Pivot").transform.position
                    - shape.transform.position;
                shape.transform.position -= positionOffset;
                shape.AddComponent<ShapeView>();
            }
        }
        /// <summary>
        /// Destroy all previous shapes
        /// </summary>
        public void ClearShapes()
        {
            for(int i = ShapeContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(ShapeContainer.GetChild(i).gameObject);
            }
        }
        
    }
}