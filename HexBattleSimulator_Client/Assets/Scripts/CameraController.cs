using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Define.CameraMode _mode = Define.CameraMode.QuaterView;
    [SerializeField] Vector3 _delta = new Vector3(0f, 6f, -5f);
    [SerializeField] GameObject _target = null;

    private void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuaterView)
        {
            FollowTarget();
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }

    private void FollowTarget()
    {
        if (_target != null)
        {
            RaycastHit hit;
            Vector3 targetPos = _target.transform.position + new Vector3(0, 0.5f, 0);
            if (Physics.Raycast(targetPos, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - targetPos).magnitude * 0.8f;
                transform.position = targetPos + _delta.normalized * dist;
            }
            else
            {
                transform.position = targetPos + _delta;
                transform.LookAt(targetPos);
            }
        }
    }
}
