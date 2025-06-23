using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_CAMERASTATS : MonoBehaviour
{
    [System.Serializable]
    public struct CameraStats
    {
        public float hozFollowSpd;
        public float vertFollowSpd;

        public float rotateSpd;
        public float rotateSmoothness;
        public float yRotDivisor;

        public float minYRot;
        public float maxYRot;

        public float fastFallSpd;
        public float fastFallActivation;
        public float downSlopeSpd;
        public float jumpSpd;

        public float rotAssistStr;
        public float rotAssistSmooth;

        public float moveOffsetAmount;
        public float moveOffsetSpd;
        public float slopeRotSpd;

        public float camDesiredDistance;
    }

    public CameraStats CST;
}
