using UnityEngine;
using UnityEngine.Events;

public class EventHolder : MonoBehaviour
{
    public UnityEvent _event;

    public void RunEvent()
    {
        _event?.Invoke();
    }

}
