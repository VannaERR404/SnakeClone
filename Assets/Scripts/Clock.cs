using System;

using UnityEngine;

namespace Clock {
    [Serializable]
    public struct Clock {
        public float interval;
        [HideInInspector] public float time;
        
        public Action onTrigger;


        public Clock(float interval) {
            this.interval = interval;
            time = 0;
            onTrigger = null;
        }

        public void Update(float deltaTime) {
            time += deltaTime;
            while (time >= interval) {
                time -= interval;
                onTrigger?.Invoke();
            }
        }
    }
}