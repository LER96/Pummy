using UnityEngine;

namespace FIMSpace.Generating
{
    [AddComponentMenu("FImpossible Creations/Hidden/Object Stamp Stigma (Hidden)", 1)]
    public class OStampStigma : MonoBehaviour
    {
        public OStamperSet ReferenceSet;
        [HideInInspector] public ObjectStampEmitterBase Emitter;
        public ObjectStamperEmittedInfo EmitInfo;
    }
}