using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventServiceTester : MonoBehaviour
{
    [SerializeField]
    private EventService _eventService;
    [SerializeField]
    private int _cooldownBeforeTrackEventSecondsCount = 1;

    private List<TrackableEvent> _testTrackEvents = new List<TrackableEvent>();

    private Coroutine _testTrackEventCoroutine;

    private void Start()
    {
        Initialize();
    }
    private void OnDestroy()
    {
        UnInitialize();
    }

    private void Initialize()
    {
        _testTrackEvents.Add(new TrackableEvent("levelStart", "level:3"));
        _testTrackEvents.Add(new TrackableEvent("rewardReceive", "2"));
        _testTrackEvents.Add(new TrackableEvent("coinSpend", "1"));

        _testTrackEventCoroutine = StartCoroutine(TestTrackEventCoroutine());
    }
    private void UnInitialize()
    {
        StopCoroutine(_testTrackEventCoroutine);
    }

    private IEnumerator TestTrackEventCoroutine()
    {
        foreach (var trackableEvent in _testTrackEvents)
        {
            yield return new WaitForSeconds(_cooldownBeforeTrackEventSecondsCount);

            _eventService.TrackEvent(trackableEvent.type, trackableEvent.data);
        }
    }
}
