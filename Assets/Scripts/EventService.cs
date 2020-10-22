using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class EventService : MonoBehaviour
{
    [SerializeField]
    private int _cooldownBeforeSend = 3;
    [SerializeField]
    private string serverUrl = "http://localhost:3000/..."; //TODO: add correct server ulr here
    [SerializeField]
    private string _playerPrefsNameForTrackableEventsJson = "_trackableEventsJson";

    private TrackableEventsJsonObject _trackableEventsJsonObject;
    private string _trackableEventsJson;

    private Coroutine _eventsSendingCoroutine;

    private long _responseCodeOFSuccessfulPost = 200; //OK

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
        var trackableEvent = new TrackableEvent(type, data);
        _trackableEventsJsonObject.events.Add(trackableEvent);

        Debug.Log($"Track event with type({type}) and data({data})");

        _trackableEventsJson = JsonUtility.ToJson(_trackableEventsJsonObject);
        //save json to prefs in case of app crash
        PlayerPrefs.SetString(_playerPrefsNameForTrackableEventsJson, _trackableEventsJson);
    }

    private void Initialize()
    {
        _trackableEventsJsonObject = new TrackableEventsJsonObject();

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

            StartCoroutine(Post(serverUrl, _trackableEventsJson, OnSuccess, OnFail));
        }
    }

    private void OnSuccess(UnityWebRequest unityWebRequest)
    {
        Debug.Log($"Events({_trackableEventsJsonObject.events.Count}) sent");

        //clear data about sent trackable events
        _trackableEventsJsonObject = new TrackableEventsJsonObject();
        PlayerPrefs.SetString(_playerPrefsNameForTrackableEventsJson, "");
    }

    private void OnFail(UnityWebRequest unityWebRequest)
    {

    }

    //https://forum.unity.com/threads/posting-json-through-unitywebrequest.476254/
    private IEnumerator Post(string url, string bodyJsonString,
        Action<UnityWebRequest> onSuccess = null, Action<UnityWebRequest> onFail = null)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        Debug.Log("Request response code: " + request.responseCode);

        if (request.isNetworkError ||
            request.isHttpError ||
            request.responseCode != _responseCodeOFSuccessfulPost)
        {
            Debug.Log("Data sending failed with error: " + request.error);

            onFail?.Invoke(request);
        }
        else
        {
            Debug.Log("Data sending completed");

            onSuccess?.Invoke(request);
        }
    }
}
