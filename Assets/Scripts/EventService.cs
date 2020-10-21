using System.Collections;
using UnityEngine;

public class EventService : MonoBehaviour
{
    [SerializeField]
    private int _cooldownBeforeSend;

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

            Debug.Log("Events sent");
        }
    }
}
