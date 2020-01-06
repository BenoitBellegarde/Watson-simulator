
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

using System.Collections.Generic;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Culls candidate objects according to the camera frustum
    /// </summary>
    /// <typeparam name="T">The type of objects that the culler will have to handle. Must inherit from CullableObject</typeparam>
    internal class FrustumCuller<T> where T : CullableObject
    {
        #region Functions
        /// <summary>
        /// Computes the objects that are, even partially, visible from a camera
        /// </summary>
        /// <param name="camera">The target camera</param>
        /// <param name="nearClipPlaneDistance">The near clip distance to test against</param>
        /// <param name="farClipPlaneDistance">The far clip distance to test against</param>
        /// <param name="candidateObjects">The array of objects to test</param>
        /// <returns>A list of visible objects</returns>
        public T[] GetVisibleObjects(Camera camera, float nearClipPlane, float farClipPlane, T[] candidateObjects)
        {
            Plane[] frustumPlanes = camera.GetFrustumPlanes(nearClipPlane, farClipPlane);
            List<T> visibleObjectsList = new List<T>();

            for(int i = 0; i < candidateObjects.Length; ++i)
            {
                if(candidateObjects[i].CheckFrustumOverlap(frustumPlanes))
                {
                    visibleObjectsList.Add(candidateObjects[i]);
                }
            }

            return visibleObjectsList.ToArray();
        }
        /// <summary>
        /// Computes the objects that are, even partially, visible from a camera
        /// </summary>
        /// <param name="camera">The target camera</param>
        /// <param name="nearClipPlaneDistance">The near clip distance to test against</param>
        /// <param name="farClipPlaneDistance">The far clip distance to test against</param>
        /// <param name="candidateObjects">The list of objects to test</param>
        /// <returns>A list of visible objects</returns>
        public T[] GetVisibleObjects(Camera camera, float nearClipPlane, float farClipPlane, List<T> candidateObjects)
        {
            return GetVisibleObjects(camera, nearClipPlane, farClipPlane, candidateObjects.ToArray());
        }
        #endregion
    }
}
