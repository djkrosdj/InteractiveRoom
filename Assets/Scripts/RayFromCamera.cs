using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс выпускающий луч из камеры на точке где курсор мыши и возвращающий значения объекта с которым сталкнулся.
/// </summary>
public class RayFromCamera : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public RaycastHit? Ray(float rayLength)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, rayLength))
        {
            return hitInfo;
        }

        return null;
    }
}
