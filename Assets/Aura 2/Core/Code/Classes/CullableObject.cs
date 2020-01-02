
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
    /// <summary>
    /// Class to inherit from when using an ObjectCuller
    /// </summary>
    public abstract class CullableObject : MonoBehaviour
    {
        #region Private Members
        /// <summary>
        /// The bounding sphere used to cull with the camera
        /// </summary>
        private BoundingSphere _boundingSphere;
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the bounding sphere used to cull with the camera
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get
            {
                return _boundingSphere;
            }
        }

        /// <summary>
        /// Updates the bounding sphere used to cull with the camera
        /// </summary>
        /// <param name="position">The new postition</param>
        /// <param name="radius">The new radius</param>
        public void UpdateBoundingSphere(Vector3 position, float radius)
        {
            _boundingSphere.position = position;
            _boundingSphere.radius = radius;
        }

        /// <summary>
        /// Updates the bounding sphere used to cull with the camera
        /// </summary>
        /// <param name="boundingSphere">The reference bounding sphere</param>
        public void UpdateBoundingSphere(BoundingSphere boundingSphere)
        {
            _boundingSphere = boundingSphere;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frustumPlanes"></param>
        /// <returns></returns>
        public bool CheckFrustumOverlap(Plane[] frustumPlanes)
        {
            for (int i = 0; i < frustumPlanes.Length; ++i)
            {
                float signedDistanceFromPlane = frustumPlanes[i].GetDistanceToPoint(BoundingSphere.position);
                if (signedDistanceFromPlane < -BoundingSphere.radius) // early returns false if the bounding sphere is behind the plane more than radius (means that it cannot overlap anymore)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
