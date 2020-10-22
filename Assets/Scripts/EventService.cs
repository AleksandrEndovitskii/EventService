using System.Collections;
using UnityEngine;

public class EventService : MonoBehaviour
{
    [SerializeField]
    private int _cooldownBeforeSend;

    private TrackableEventsJsonObject _trackableEventsJsonObject;

    private Coroutine _eventsSendingCoroutine;

    private void Start()
    {
        Initialize(10);
    }
    private void OnDestroy()
    {
        UnInitialize();
    }

    public void TrackEvent(string type, string data)
    {
        var trackableEvent = new TrackableEvent(type, data);
        _trackableEventsJsonObject.events.Add(trackableEvent);

        Debug.Log($"Track event with type({type}) and data({data})");
    }

    private void Initialize(int cooldownBeforeSend)
    {
        _trackableEventsJsonObject = new TrackableEventsJsonObject();

        _cooldownBeforeSend = cooldownBeforeSend;

        _eventsSendingCoroutine = StartCoroutine(EventsSendingCoroutine(_cooldownBeforeSend));
    }
    private void UnInitialize()
    {
        StopCoroutine(_eventsSendingCoroutine);
    }

    private IEnumerator EventsSendingCoroutine(int cooldownBeforeSend)
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownBeforeSend);

            if (_trackableEventsJsonObject.events.Count == 0)
            {
                continue;
            }

            Debug.Log($"Events({_trackableEventsJsonObject.events.Count}) sent");

            _trackableEventsJsonObject = new TrackableEventsJsonObject();
        }
    }
}
