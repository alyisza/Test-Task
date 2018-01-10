using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InputManager : MonoBehaviour
    {
        IClickable clickable;

#if UNITY_ANDROID || UNITY_IOS
        void Start()
        {
            Input.multiTouchEnabled = false;
        } 
#endif

        void Update()
        {

#if UNITY_STANDALONE || UNITY_EDITOR
            CheckMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
            CheckTouchInput();
#endif
        }

        void CheckMouseInput()
        {
            //begin drag
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CheckClickPosition(clickWorldPosition);
            }
            //Drag
            if (clickable != null)
                clickable.OnDrag(Input.mousePosition);
            //End drag
            if (Input.GetMouseButtonUp(0) && clickable != null)
            {
                clickable.OnEndDrag();
                clickable = null;
            }
        }

        void CheckTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                //begin drag
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 clickWorldPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    CheckClickPosition(clickWorldPosition);
                }
                //Drag
                if (touch.phase == TouchPhase.Moved && clickable != null)
                    clickable.OnDrag(Input.mousePosition);
                //End drag
                if (touch.phase == TouchPhase.Ended && clickable != null)
                {
                    clickable.OnEndDrag();
                    clickable = null;
                }
            }
        }

        void CheckClickPosition(Vector2 clickWorldPosition)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(clickWorldPosition, Vector2.zero);
            if (hit.collider == null)
                return;
            clickable = hit.collider.gameObject.GetComponent<IClickable>();
            if (clickable == null)
                return;
            clickable.OnBeginDrag();
        }
    }
}