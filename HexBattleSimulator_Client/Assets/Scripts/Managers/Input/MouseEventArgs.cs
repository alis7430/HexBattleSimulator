using UnityEngine;
public enum MouseEventType
{
    Press,
    Click,
    LongPress,
    DragStart,
    Drag,
    DragEnd,
}

public class MouseEventArgs
{
    public MouseEventType EventType;
    public Vector3 ScreenPosition;
    public Vector3 Delta;
    public float PressDuration;

    public MouseEventArgs(MouseEventType type, Vector3 pos, Vector3 delta, float duration)
    {
        EventType = type;
        ScreenPosition = pos;
        Delta = delta;
        PressDuration = duration;
    }
}