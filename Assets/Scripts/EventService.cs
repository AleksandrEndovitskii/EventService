using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventService : MonoBehaviour
{
    [SerializeField]
    private int _cooldownBeforeSend;

    private List<KeyValuePair<string, string>> _trackEvents = new List<KeyValuePair<string, string>>();

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
        _trackEvents.Add(new KeyValuePair<string, string>(type, data));

        Debug.Log($"Track event with type({type}) and data({data})");
    }

    private void Initialize(int cooldownBeforeSend)
    {
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

            if (_trackEvents.Count == 0)
            {
                continue;
            }

            Debug.Log($"Events({_trackEvents.Count}) sent");

            _trackEvents.Clear();
        }
    }
}
