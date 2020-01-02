
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
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Quality settings for Aura2 computation
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Aura Quality Settings", menuName = "Aura 2/Quality Settings", order = 1)]
    public class AuraQualitySettings : ScriptableObject
    {
        #region Public Members
        /// <summary>
        /// Shows only the volumetric lighting
        /// </summary>
        public bool displayVolumetricLightingBuffer;
        /// <summary>
        /// The resolution of the frustum grid
        /// </summary>
        public Vector3Int frustumGridResolution = new Vector3Int(160, 90, 128);
        /// <summary>
        /// Enables the automatic horizontal resizing of the frustum grid resolution when the camera is running in stereo mode (halved size for MultiPass, doubled size for SinglePass)
        /// </summary>
        public bool enableAutomaticStereoResizing = true;
        /// <summary>
        /// The furthest distance where the data will be computed
        /// </summary>
        public float farClipPlaneDistance = 128.0f;
        /// <summary>
        /// Focus the depth slices near or far from the camera
        /// </summary>
        public float depthBiasCoefficient = 0.35f;
        /// <summary>
        /// Enables the computation of volumes
        /// </summary>
        public bool enableVolumes = true;
        /// <summary>
        /// Enables the computation of Texture2D masks
        /// </summary>
        public bool enableVolumesTexture2DMask = true;
        /// <summary>
        /// Enables the computation of Texture3D masks
        /// </summary>
        public bool enableVolumesTexture3DMask = true;
        /// <summary>
        /// Enables the computation of noise mask
        /// </summary>
        public bool enableVolumesNoiseMask = true;
        /// <summary>
        /// Enables the computation of ambient lighting
        /// </summary>
        public bool enableAmbientLighting = true;
        /// <summary>
        /// Enables the computation of directional lights
        /// </summary>
        public bool enableDirectionalLights = true;
        /// <summary>
        /// Enables the computation of directional lights' shadow
        /// </summary>
        public bool enableDirectionalLightsShadows = true;
        /// <summary>
        /// Enables the computation of spot lights
        /// </summary>
        public bool enableSpotLights = true;
        /// <summary>
        /// Enables the computation of spot lights' shadow
        /// </summary>
        public bool enableSpotLightsShadows = true;
        /// <summary>
        /// Enables the computation of point lights
        /// </summary>
        public bool enablePointLights = true;
        /// <summary>
        /// Enables the computation of point lights' shadow
        /// </summary>
        public bool enablePointLightsShadows = true;
        /// <summary>
        /// Enables the computation of lights' cookie
        /// </summary>
        public bool enableLightsCookies = true;
        /// <summary>
        /// Enables blue noise dithering to smooth color banding (might need to be smoothed)
        /// </summary>
        public bool enableDithering = true;
        /// <summary>
        /// The sampling filter used for Texture3D
        /// </summary>
        public Texture3DFiltering texture3DFiltering = Texture3DFiltering.Cubic;
        /// <summary>
        /// Enables the denoising filter on the data texture3D
        /// </summary>
        public bool EXPERIMENTAL_enableDenoisingFilter = false;
        /// <summary>
        /// The range of the denoising filter on the data texture3D
        /// </summary>
        public DenoisingFilterRange EXPERIMENTAL_denoisingFilterRange = DenoisingFilterRange.OneNeighbour;
        /// <summary>
        /// Enables the blur filter on the data texture3D
        /// </summary>
        public bool EXPERIMENTAL_enableBlurFilter = false;
        /// <summary>
        /// The range of the blur filter on the data texture3D
        /// </summary>
        public BlurFilterRange EXPERIMENTAL_blurFilterRange = BlurFilterRange.OneNeighbour;
        /// <summary>
        /// The type of the blur filter on the data texture3D
        /// </summary>
        public BlurFilterType EXPERIMENTAL_blurFilterType = BlurFilterType.Box;
        /// <summary>
        /// The deviation used to compute the gaussian curve factor
        /// </summary>
        public float EXPERIMENTAL_blurFilterGaussianDeviation = 0.0025f;
        /// <summary>
        /// Enables temporal reprojection
        /// </summary>
        public bool enableTemporalReprojection = true;
        /// <summary>
        /// Amount of reprojection with the previous frame
        /// </summary>
        [Range(0, 1)]
        public float temporalReprojectionFactor = 0.95f;
        /// <summary>
        /// Enables depth occlusion culling
        /// </summary>
        public bool enableOcclusionCulling = true;
        /// <summary>
        /// Highlights the occluded cells for debug
        /// </summary>
        public bool debugOcclusionCulling = false;
        /// <summary>
        /// Accuracy of the occlusion computation
        /// </summary>
        public OcclusionCullingAccuracy occlusionCullingAccuracy = OcclusionCullingAccuracy.Lowest;
        /// <summary>
        /// Enables the computation of light probes
        /// </summary>
        public bool enableLightProbes = false;
        ///// <summary>
        ///// The resolution of the grid used to store the light probes in the frustum
        ///// </summary>
        //public Vector3Int lightProbesProxyGridResolution = new Vector3Int(16, 9, 16);
        #endregion

        #region Function
        /// <summary>
        /// Returns the frustum grid resolution, taking into account the camera stereo mode
        /// </summary>
        /// <param name="camera">The reference camera to look for stereo mode</param>
        /// <returns>The frustum grid resolution, resized according to the stereo mode of the camera</returns>
        public Vector3Int GetFrustumGridResolution(Camera camera)
        {
            StereoMode cameraStereoMode = camera.GetCameraStereoMode();
            Vector3Int resolution = frustumGridResolution;
            if (enableAutomaticStereoResizing)
            {
                if (cameraStereoMode == StereoMode.MultiPass)
                {
                    resolution.x /= 2;
                }
                else if (cameraStereoMode == StereoMode.SinglePass)
                {
                    resolution.x *= 2;
                }
            }

            return resolution;
        }

        /// <summary>
        /// Sets a new resolution for the frustum grid and tries to apply it on all the Aura components that has the current qualitySettings on
        /// </summary>
        /// <param name="resolution">The new resolution to set</param>
        public void SetFrustumGridResolution(Vector3Int resolution)
        {
            this.frustumGridResolution = resolution;

            AuraCamera[] auraComponents = FindObjectsOfType<AuraCamera>();
            for (int i = 0; i < auraComponents.Length; ++i)
            {
                if (auraComponents[i].frustumSettings.QualitySettings == this)
                {
                    auraComponents[i].SetFrustumGridResolution(this.frustumGridResolution);
                }
            }
        }

        ///// <summary>
        ///// Sets a new resolution for the light probes proxy volume grid and tries to apply it on all the Aura components that has the current qualitySettings on
        ///// </summary>
        ///// <param name="resolution">The new resolution to set</param>
        //public void SetLightProbesProxyGridResolution(Vector3Int resolution)
        //{
        //    this.lightProbesProxyGridResolution = resolution;
        //
        //    AuraCamera[] auraComponents = FindObjectsOfType<AuraCamera>();
        //    for (int i = 0; i < auraComponents.Length; ++i)
        //    {
        //        if (auraComponents[i].frustumSettings.QualitySettings == this)
        //        {
        //            auraComponents[i].SetLightProbesProxyGridResolution(this.lightProbesProxyGridResolution);
        //        }
        //    }
        //}
        #endregion
    }
}