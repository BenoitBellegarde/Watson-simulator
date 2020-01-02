
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
    /// Manages the volumes, collects and packs data and volumetric textures
    /// </summary>
    public class VolumesManager
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="camera">The reference camera, for culling purposes</param>
        /// <param name="frustumSettings">The frustum settings</param>
        public VolumesManager(Camera camera, FrustumSettings frustumSettings)
        {
            _referenceCamera = camera;
            this._frustumSettings = frustumSettings;

            _culler = new ObjectsCuller<AuraVolume>(_referenceCamera, this._frustumSettings);
            for(int i = 0; i < AuraCamera.CommonDataManager.VolumesCommonDataManager.RegisteredVolumesList.Count; ++i)
            {
                _culler.Register(AuraCamera.CommonDataManager.VolumesCommonDataManager.RegisteredVolumesList[i]);
            }

            Camera.onPreRender += Camera_onPreRender;

            AuraCamera.CommonDataManager.VolumesCommonDataManager.OnRegisterVolume += VolumesCommonDataManager_OnRegisterVolume;
            AuraCamera.CommonDataManager.VolumesCommonDataManager.OnUnregisterVolume += VolumesCommonDataManager_OnUnregisterVolume;
        }
        #endregion

        #region Private Members
        /// <summary>
        /// The culler that will tell which volume is visible from the camera
        /// </summary>
        private readonly ObjectsCuller<AuraVolume> _culler;
        /// <summary>
        /// Array of visible volumes
        /// </summary>
        private AuraVolume[] _visibleVolumes;
        /// <summary>
        /// Array of data of the visible volumes
        /// </summary>
        private VolumeData[] _visibleVolumesDataArray;
        /// <summary>
        /// Settings of the frustum
        /// </summary>
        public FrustumSettings _frustumSettings;
        /// <summary>
        /// The reference camera to get the clip space from
        /// </summary>
        private Camera _referenceCamera;
        /// <summary>
        /// One count buffer used to assign to the compute shader when the branch path is unused (for cross platform compatibility)
        /// </summary>
        private ComputeBuffer _emptyBuffer;
        #endregion

        #region Properties
        /// <summary>
        /// One count buffer used to assign to the compute shader when the branch path is unused (for cross platform compatibility)
        /// </summary>
        public ComputeBuffer EmptyBuffer
        {
            get
            {
                if (_emptyBuffer == null)
                {
                    _emptyBuffer = new ComputeBuffer(1, VolumeData.Size);
                }

                return _emptyBuffer;
            }
        }

        /// <summary>
        /// The compute buffer in charge of passing the volumes' data to the compute shaders
        /// </summary>
        public ComputeBuffer Buffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Is one or more volume(s) inside the ranged frustum?
        /// </summary>
        public bool HasVisibleVolumes
        {
            get
            {
                return _culler.HasVisibleObjects;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        ///     Called when any camera is a about to render
        /// </summary>
        private void Camera_onPreRender(Camera camera)
        {
            _culler.Update(camera, _frustumSettings);

            SetupComputeBuffer();

            CollectData();
        }

        /// <summary>
        /// Called when a new volume registers onto the global list
        /// </summary>
        /// <param name="auraVolume">The newly registered volume</param>
        private void VolumesCommonDataManager_OnRegisterVolume(AuraVolume auraVolume)
        {
            _culler.Register(auraVolume);
        }

        /// <summary>
        /// Called when a volume unregisters from the global list
        /// </summary>
        /// <param name="auraVolume">The unregistering volume</param>
        private void VolumesCommonDataManager_OnUnregisterVolume(AuraVolume auraVolume)
        {
            _culler.Unregister(auraVolume);
        }

        /// <summary>
        /// Allocate new compute buffer or null, according to visible objects count from culler
        /// </summary>
        private void SetupComputeBuffer()
        {
            if(Buffer == null || _culler.VisibleObjectsCount != Buffer.count)
            {
                DisposeComputeBuffer();

                if(_culler.HasVisibleObjects)
                {
                    Buffer = new ComputeBuffer(_culler.VisibleObjectsCount, VolumeData.Size);
                    _visibleVolumesDataArray = new VolumeData[_culler.VisibleObjectsCount];
                }
                else
                {
                    Buffer = null;
                }
            }
        }

        /// <summary>
        /// Collects the volumes's data and pack them in the computeBuffer
        /// </summary>
        private void CollectData()
        {
            if (_culler.HasVisibleObjects)
            {
                AuraVolume[] visibleVolumes = _culler.GetVisibleObjects();
                for(int i = 0; i < _culler.VisibleObjectsCount; ++i)
                {
                    _visibleVolumesDataArray[i] = visibleVolumes[i].GetData();
                }

                Buffer.SetData(_visibleVolumesDataArray);
            }
        }

        /// <summary>
        /// Releases the computeBuffer
        /// </summary>
        private void DisposeComputeBuffer()
        {
            if(Buffer != null)
            {
                Buffer.Release();
            }
        }

        /// <summary>
        /// Releases unmanaged objects and unregisters events
        /// </summary>
        public void Dispose()
        {
            DisposeComputeBuffer();

            if (_emptyBuffer != null)
            {
                _emptyBuffer.Release();
                _emptyBuffer = null;
            }

            Camera.onPreRender -= Camera_onPreRender;
            AuraCamera.CommonDataManager.VolumesCommonDataManager.OnRegisterVolume -= VolumesCommonDataManager_OnRegisterVolume;
            AuraCamera.CommonDataManager.VolumesCommonDataManager.OnUnregisterVolume -= VolumesCommonDataManager_OnUnregisterVolume;
        }
        #endregion
    }
}
