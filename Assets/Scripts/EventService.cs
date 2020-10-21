using UnityEngine;

public class EventService : MonoBehaviour
{
    private void Start()
    {
        Initialize();
    }
    private void OnDestroy()
    {
        UnInitialize();
    }

    public void TrackEvent(string type, string data)
    {

    }

    private void Initialize()
    {

    }
    private void UnInitialize()
    {

    }
}
