using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable {

    void OnBeginDrag();
    void OnDrag(Vector2 inputPoint);
    void OnEndDrag();

}
