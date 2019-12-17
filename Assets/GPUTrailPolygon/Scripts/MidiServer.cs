using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTrailPolygon
{
    public class MidiServer : MonoBehaviour
    {
        public RibbonTrail audioReactiveTrail;
        public AudioReactiveCrossScale cross;
        public GPUPolygonTrail polygonTrail;

        public float meshaAmpRange;
        public float crossAmpRange;

        void Start()
        {

        }

        void Update()
        {
            audioReactiveTrail.amp = MidiReciever.knobs[0] * meshaAmpRange;
            cross.scale = MidiReciever.knobs[1] * crossAmpRange;
            polygonTrail.blend = MidiReciever.knobs[2];
        }
    }
}