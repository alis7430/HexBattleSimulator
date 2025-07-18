using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly float LayCastDist = 100f;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    [SerializeField]
    private float _speed = 1f;
    private float wait_run_ratio = 0f;

    private PlayerState _state = PlayerState.Idle;
    private Vector3 _destPos = Vector3.zero;

    private Animator animator = null;

    private void Awake()
    {
        animator = this.transform.GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe event using Managers.Input
        Managers.Input.OnMouseEvent -= OnMouseClicked;
        Managers.Input.OnMouseEvent += OnMouseClicked;
    }

    private void Update()
    {
        switch(_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            default:
                break;
        }
    }

    private void UpdateMoving()
    {
            Vector3 dir = _destPos - transform.position;

            if (dir.magnitude < 0.0001f)
            {
                _state = PlayerState.Idle;
            }
            else
            {
                float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
                transform.position += dir.normalized * moveDist;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
                //transform.LookAt(_destPos);
            }
            animator.SetFloat("speed", _speed);
    }

    private void UpdateIdle()
    {
        animator.SetFloat("speed", 0);
    }
    
    private void OnMouseClicked(MouseEventArgs args)
    {
        if (args.EventType != MouseEventType.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * LayCastDist, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayCastDist, LayerMask.GetMask("Floor")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}
