
/***************************************************************************
*                                                                          *
*  Copyright (c) Raphaël Ernaelsten (@RaphErnaelsten)                      *
*  All Rights Reserved.                                                    *
*                                                                          *
*  NOTICE: Aura 2 is a commercial project.                                 * 
*  All information contained herein is, and remains the property of        *
*  Raphaël Ernaelsten.                                                     *
*  The intellectual and technical concepts contained herein are            *
*  proprietary to Raphaël Ernaelsten and are protected by copyright laws.  *
*  Dissemination of this information or reproduction of this material      *
*  is strictly forbidden.                                                  *
*                                                                          *
***************************************************************************/

using UnityEngine;

namespace Aura2API
{
    public class SinRotationOffset : MonoBehaviour
    {
        private Quaternion _initialRotation;
        public float sinAmplitude = 15;
        public Vector3 sinDirection = Vector3.up;
        public float sinOffset;
        public float sinSpeed = 1;
        public Space space = Space.Self;

        private void Start()
        {
            _initialRotation = space == Space.Self ? transform.localRotation : transform.rotation;
        }

        private void Update()
        {
            float angle = sinAmplitude * Mathf.Sin(Time.time * sinSpeed + sinOffset);
            Quaternion rotationOffset = Quaternion.AngleAxis(angle, sinDirection);

            if (space == Space.Self)
            {
                transform.localRotation = _initialRotation * rotationOffset;
            }
            else
            {
                transform.rotation = _initialRotation * rotationOffset;
            }
        }
    }
}
