using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{

    /// <summary>
    /// Y=0 평면 기준으로 화면 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public static Vector3 ScreenToWorld(Camera cam, Vector3 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        Plane ground = new Plane(Vector3.up, Vector3.zero); // y=0 plane

        if (ground.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }

    /// <summary>
    /// tag가 일치하는 오브젝트를 기준으로 화면 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="screenPos"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public static Vector3 ScreenToWorldWithTag(Camera cam, Vector3 screenPos, string tagName)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag(tagName))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }
}
