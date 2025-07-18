using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handle Mouse(Touch), Keyboard Input
/// </summary>
public class InputManager
{
    public Action OnKeyEvent = null;
    public Action<MouseEventArgs> OnMouseEvent = null;

    // --- Configurable parameters ---
    private readonly float _longPressThreshold = 0.3f;
    private readonly float _dragThreshold = 5f; // pixel distance

    // --- Internal state ---
    private bool _isPressed = false;
    private bool _isDragging = false;
    private float _pressStartTime;
    private Vector3 _pressStartPos;
    private Vector3 _lastPos;

    public void OnUpdate()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        OnUpdateKeyAction();
        OnUpdateMouseAction();
    }

    public void Clear()
    {
        OnKeyEvent = null;
        OnMouseEvent = null;
    }

    private void OnUpdateKeyAction()
    {
        if (Input.anyKey)
            OnKeyEvent?.Invoke();
    }

    private void OnUpdateMouseAction()
    {
        Vector3 currentPos = Input.mousePosition;

        // 마우스 버튼 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            HandleMousePress(currentPos);
        }

        // 길게 누름과 드래그 판정
        if (_isPressed)
        {
            HandleMouseHold(currentPos);

            if (Input.GetMouseButtonUp(0))
                HandleMouseRelease(currentPos);
        }
    }

    // ---------------------------
    // Press
    // ---------------------------
    private void HandleMousePress(Vector3 pos)
    {
        _isPressed = true;
        _isDragging = false;
        _pressStartTime = Time.time;
        _pressStartPos = pos;
        _lastPos = pos;

        FireEvent(MouseEventType.Press, pos);
    }

    // ---------------------------
    // Hold (LongPress & Drag)
    // ---------------------------
    private void HandleMouseHold(Vector3 pos)
    {
        float pressDuration = Time.time - _pressStartTime;
        float distance = Vector3.Distance(_pressStartPos, pos);

        // LongPress
        if (!_isDragging && pressDuration > _longPressThreshold && distance < _dragThreshold)
        {
            FireEvent(MouseEventType.LongPress, pos, Vector3.zero, pressDuration);
        }

        // DragStart
        if (!_isDragging && distance >= _dragThreshold)
        {
            _isDragging = true;
            FireEvent(MouseEventType.DragStart, pos, Vector3.zero, pressDuration);
        }

        // Dragging
        if (_isDragging)
        {
            Vector3 delta = pos - _lastPos;
            FireEvent(MouseEventType.Drag, pos, delta, pressDuration);
        }

        _lastPos = pos;
    }

    // ---------------------------
    // Release
    // ---------------------------
    private void HandleMouseRelease(Vector3 pos)
    {
        float pressDuration = Time.time - _pressStartTime;

        if (_isDragging)
        {
            FireEvent(MouseEventType.DragEnd, pos, Vector3.zero, pressDuration);
        }
        else
        {
            FireEvent(MouseEventType.Click, pos, Vector3.zero, pressDuration);
        }

        _isPressed = false;
        _isDragging = false;
    }

    // ---------------------------
    // Event Dispatcher
    // ---------------------------
    private void FireEvent(MouseEventType type, Vector3 pos, Vector3? delta = null, float duration = 0f)
    {
        var args = new MouseEventArgs(type, pos, delta ?? Vector3.zero, duration);
        OnMouseEvent?.Invoke(args);
    }
}
