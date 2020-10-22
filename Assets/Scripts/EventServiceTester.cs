using System.Collections;
using UnityEngine;

public class EventServiceTester : MonoBehaviour
{
    [SerializeField]
    private EventService _eventService;
    [SerializeField]
    private int _testTrackEventsCount = 3;
    [SerializeField]
    private int _cooldownBeforeTrackEventSecondsCount = 1;

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
        _testTrackEventCoroutine = StartCoroutine(TestTrackEventCoroutine());
    }
    private void UnInitialize()
    {
        StopCoroutine(_testTrackEventCoroutine);
    }

    private IEnumerator TestTrackEventCoroutine()
    {
        for (var i = 0; i < _testTrackEventsCount; i++)
        {
            yield return new WaitForSeconds(_cooldownBeforeTrackEventSecondsCount);

            _eventService.TrackEvent("testType", "testData");
        }
    }
}
