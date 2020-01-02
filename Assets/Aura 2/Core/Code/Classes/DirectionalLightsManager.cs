
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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Manager that handles directional AuraLights
    /// </summary>
    public class DirectionalLightsManager
    {
        #region Public Members
        /// <summary>
        /// The cookie map size
        /// </summary>
        public static readonly Vector2Int cookieMapSize = new Vector2Int(256, 256); // TODO : EXPOSE AS DYNAMIC PARAMETER
        #endregion

        #region Private Members
        /// <summary>
        /// The shadow map size for one cascade (AKA no cascade)
        /// </summary>
        private static readonly Vector2Int _shadowMapSizeOneCascade = new Vector2Int(256, 256); // TODO : EXPOSE AS DYNAMIC PARAMETER
        /// <summary>
        /// The shadow map size for two cascades (based on one cascade size)
        /// </summary>
        private static readonly Vector2Int _shadowMapSizeTwoCascades = new Vector2Int(DirectionalLightsManager._shadowMapSizeOneCascade.x * 2, DirectionalLightsManager._shadowMapSizeOneCascade.y);
        /// <summary>
        /// The shadow map size for four cascades (based on one cascade size)
        /// </summary>
        private static readonly Vector2Int _shadowMapSizeFourCascades = new Vector2Int(DirectionalLightsManager._shadowMapSizeOneCascade.x * 2, DirectionalLightsManager._shadowMapSizeOneCascade.y * 2);
        /// <summary>
        /// The collected packed data
        /// </summary>
        private DirectionalLightParameters[] _parameters;
        /// <summary>
        /// One count buffer used to assign to the compute shader when the branch path is unused (for cross platform compatibility)
        /// </summary>
        private ComputeBuffer _emptyBuffer;
        #endregion

        #region Properties
        /// <summary>
        /// Accessor to the shadow map size (depending on the shadow cascade count)
        /// </summary>
        public static Vector2Int ShadowMapSize
        {
            get
            {
                switch (QualitySettings.shadowCascades)
                {
                    case 4 :
                        return DirectionalLightsManager._shadowMapSizeFourCascades;
                    case 2 :
                        return DirectionalLightsManager._shadowMapSizeTwoCascades;
                    default :
                        return DirectionalLightsManager._shadowMapSizeOneCascade;
                }
            }
        }
        
        /// <summary>
        /// Accessor to the candidate lights
        /// </summary>
        public List<AuraLight> CandidateLights
        {
            get
            {
                return AuraCamera.CommonDataManager.LightsCommonDataManager.RegisteredDirectionalLightsList;
            }
        }

        /// <summary>
        /// Tells how many lights are candidate
        /// </summary>
        public int CandidateLightsCount
        {
            get
            {
                return CandidateLights.Count;
            }
        }

        /// <summary>
        /// Tells if has candidate lights
        /// </summary>
        public bool HasCandidateLights
        {
            get
            {
                return CandidateLightsCount > 0;
            }
        }

        /// <summary>
        /// One count buffer used to assign to the compute shader when the branch path is unused (for cross platform compatibility)
        /// </summary>
        public ComputeBuffer EmptyBuffer
        {
            get
            {
                if (_emptyBuffer == null)
                {
                    _emptyBuffer = new ComputeBuffer(1, DirectionalLightParameters.Size);
                }

                return _emptyBuffer;
            }
        }

        /// <summary>
        /// Accessor to the compute buffer containing the packed data of the visible lights
        /// </summary>
        public ComputeBuffer DataBuffer
        {
            get;
            private set;
        }

        /// <summary>
        /// Disposes the members
        /// </summary>
        public void Dispose()
        {
            ReleaseBuffer();

            if (_emptyBuffer != null)
            {
                _emptyBuffer.Release();
                _emptyBuffer = null;
            }
        }

        /// <summary>
        /// Setup the data buffers containing the packed data of the visible lights
        /// </summary>
        private void SetupBuffers()
        {
            if (DataBuffer == null || DataBuffer.count != CandidateLightsCount)
            {
                ReleaseBuffer();
                SetupBuffer();
            }

            if (_parameters == null || _parameters.Length != CandidateLightsCount)
            {
                SetupParametersArray();
            }
        }
        
        /// <summary>
        /// Initializes the parameters array containing the packed data of the visible lights
        /// </summary>
        private void SetupParametersArray()
        {
            _parameters = new DirectionalLightParameters[CandidateLightsCount];
        }

        /// <summary>
        /// Releases the compute buffer containing the packed data of the visible lights
        /// </summary>
        private void ReleaseBuffer()
        {
            if(DataBuffer != null)
            {
                DataBuffer.Release();
                DataBuffer = null;
            }
        }

        /// <summary>
        /// Initializes the compute buffer containing the packed data of the visible lights
        /// </summary>
        private void SetupBuffer()
        {
            if(HasCandidateLights)
            {
                DataBuffer = new ComputeBuffer(CandidateLightsCount, DirectionalLightParameters.Size);
            }
        }

        /// <summary>
        /// Updates the manager (collects shadows/cookies/data and packs them)
        /// </summary>
        public void Update()
        {
            SetupBuffers();

            if (HasCandidateLights)
            {
                for (int i = 0; i < CandidateLightsCount; ++i)
                {
                    _parameters[i] = CandidateLights[i].GetDirectionnalParameters();
                }

                DataBuffer.SetData(_parameters);
            }
        }
        #endregion
    }
}
