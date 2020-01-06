
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
    /// Culls candidate objects according to the ranged camera frustum and puts an array of visible objects at disposal
    /// </summary>
    /// <typeparam name="T">The type of objects that the culler will have to handle. Must inherit from CullableObject</typeparam>
    internal class ObjectsCuller<T> where T : CullableObject
    {
        #region Constructor
        public ObjectsCuller(Camera referenceCamera, FrustumSettings settings)
        {
            _registredObjectsList = new List<T>();
            _frustumCuller = new FrustumCuller<T>();
        }
        #endregion

        #region Private Members
        /// <summary>
        /// List of canditate objects to cull
        /// </summary>
        private readonly List<T> _registredObjectsList;
        /// <summary>
        /// List of visible objects after culling
        /// </summary>
        private T[] _visibleObjectsArray;
        /// <summary>
        /// Array of the bounding spheres to send to the culling group
        /// </summary>
        private BoundingSphere[] _boundingSpheres;
        /// <summary>
        /// Array with the indices of the visible bounding spheres. Indices of bounding spheres and registred objects are directly linked
        /// </summary>
        private int[] _visibleObjectsIndices;
        /// <summary>
        /// Settings of the frustum
        /// </summary>
        private FrustumSettings _frustumSettings;
        /// <summary>
        /// The effective culler
        /// </summary>
        private FrustumCuller<T> _frustumCuller;
        #endregion

        #region Functions
        /// <summary>
        /// Tells if the culler has candidate objects
        /// </summary>
        public bool HasRegistredObjects
        {
            get
            {
                return _registredObjectsList.Count > 0;
            }
        }

        /// <summary>
        /// Tells if there are visible objects
        /// </summary>
        public bool HasVisibleObjects
        {
            get
            {
                return VisibleObjectsCount > 0;
            }
        }

        /// <summary>
        /// Tells the amount of visible objects
        /// </summary>
        public int VisibleObjectsCount
        {
            get
            {
                return _visibleObjectsArray != null ? _visibleObjectsArray.Length : 0;
            }
        }

        /// <summary>
        /// Sets the culling group parameters
        /// </summary>
        private void SetupCullingGroup()
        {
            _boundingSpheres = new BoundingSphere[_registredObjectsList.Count];
        }

        /// <summary>
        /// Registers a new candidate object
        /// </summary>
        /// <param name="candidate">The candidate object to be culled</param>
        public void Register(T candidate)
        {
            if(!_registredObjectsList.Contains(candidate))
            {
                _registredObjectsList.Add(candidate);
                SetupCullingGroup();
            }
        }

        /// <summary>
        /// Unregisters the candiadate object
        /// </summary>
        /// <param name="volume"></param>
        public void Unregister(T volume)
        {
            if(_registredObjectsList.Contains(volume))
            {
                _registredObjectsList.Remove(volume);
                SetupCullingGroup();
            }
        }

        /// <summary>
        /// Updates the culler.
        /// </summary>
        public void Update(Camera referenceCamera, FrustumSettings settings)
        {
            if (HasRegistredObjects)
            {
                for(int i = 0; i < _registredObjectsList.Count; ++i)
                {
                    _boundingSpheres[i] = _registredObjectsList[i].BoundingSphere;
                }

                _visibleObjectsArray = _frustumCuller.GetVisibleObjects(referenceCamera, referenceCamera.nearClipPlane, settings.QualitySettings.farClipPlaneDistance, _registredObjectsList);
            }
            else
            {
                _visibleObjectsArray = null;
            }
        }

        /// <summary>
        /// Retreives the array of visible objects
        /// </summary>
        /// <returns></returns>
        public T[] GetVisibleObjects()
        {
            return _visibleObjectsArray;
        }
        #endregion
    }
}
