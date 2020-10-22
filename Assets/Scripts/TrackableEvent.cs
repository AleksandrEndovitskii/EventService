    using System;

    [Serializable]
    public class TrackableEvent
    {
        public string type;
        public string data;

        public TrackableEvent(string type, string data)
        {
            this.type = type;
            this.data = data;
        }
    }
