using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraSize : MonoBehaviour {

    public SpriteRenderer Background;

    private void Awake()
    {
        if (Background == null)
        {
            Debug.LogWarning("Background is null");
            return;
        }

        setCameraSize();
    }

    void setCameraSize()
    {
        Vector2 bgWorldSize = Background.size;
        float bgAspect = bgWorldSize.x / bgWorldSize.y;

        if (bgAspect < Camera.main.aspect)
        {
            Camera.main.orthographicSize = bgWorldSize.y / 2f;
        }
        else
        {
            float cameraDesireHeight = bgWorldSize.x / Camera.main.aspect;
            Camera.main.orthographicSize = cameraDesireHeight / 2f;
        }
    }
}
