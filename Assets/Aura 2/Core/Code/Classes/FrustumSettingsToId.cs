
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

namespace Aura2API
{
    /// <summary>
    /// Collects all the settings and compute the flag Id to be used as kernel id of the computeDataComputeShader
    /// </summary>
    public class FrustumSettingsToId
    {
        #region Private members
        /// <summary>
        /// The AuraCamera component this frustum in called from
        /// </summary>
        private AuraCamera _auraComponent;
        /// <summary>
        /// Bitmask collecting the different parameters and giving a unique int for each combinaison
        /// </summary>
        private FrustumParameters _frustumParameters;
        /// <summary>
        /// Settings of the frustum
        /// </summary>
        private FrustumSettings _frustumSettings;
        /// <summary>
        /// Handles the culling and the packing of the data of the visible volumes
        /// </summary>
        private VolumesManager _volumesManager;
        /// <summary>
        /// Handles the culling and the packing of the data of the visible spot lights
        /// </summary>
        private SpotLightsManager _spotLightsManager;
        /// <summary>
        /// Handles the culling and the packing of the data of the visible point lights
        /// </summary>
        private PointLightsManager _pointLightsManager;
        #endregion

        #region Constructor
        public FrustumSettingsToId(FrustumSettings settings, AuraCamera auraComponent, VolumesManager volumesManager, SpotLightsManager spotLightsManager, PointLightsManager pointLightsManager)
        {
            _frustumSettings = settings;
            _auraComponent = auraComponent;
            _volumesManager = volumesManager;
            _spotLightsManager = spotLightsManager;
            _pointLightsManager = pointLightsManager;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Returns a unique int relative to the combinaison of the different parameters
        /// </summary>
        /// <param name="recomputeFlags">Forces the Id recomputation. Default : false</param>
        /// <returns>A unique int relative to the combinaison of the different parameters</returns>
        // Deactivated while waiting for Unity to better handle compute shader variants/compilation
        //public int GetId(bool recomputeFlags = false)
        //{
        //    if (recomputeFlags)
        //    {
        //        ComputeFlags();
        //    }
        //
        //    return (int)_frustumParameters;
        //}

        /// <summary>
        /// Returns the kernel Id that should be used for compute shader
        /// </summary>
        /// <returns>The kernel Id</returns>
        public int GetKernelId()
        {
            switch (QualitySettings.shadowCascades)
            {
                case 1:
                    {
                        return 0;
                    }

                case 2:
                    {
                        return 1;
                    }

                case 4:
                default:
                    {
                        return 2;
                    }
            }
        }
        /// <summary>
        /// Returns the kernel Id that should be used for compute shader, checks for single pass stereo as well
        /// </summary>
        /// <returns>The kernel Id</returns>
        public int GetKernelId(Camera camera)
        {
            int id = GetKernelId();
            id += camera.GetCameraStereoMode() == StereoMode.SinglePass ? 3 : 0;

            return (camera.orthographic ? 0 : id)/*Switch to 1 cascade directional mode when orthographic*/ + (HasFlags(FrustumParameters.EnableOcclusionCulling) ? 6 : 0);
        }

        /// <summary>
        /// Recomputes the bitmask of the options
        /// </summary>
        public void ComputeFlags()
        {
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableOcclusionCulling, _frustumSettings.QualitySettings.enableOcclusionCulling);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableTemporalReprojection, _frustumSettings.QualitySettings.enableTemporalReprojection && _auraComponent.FrameId > 1 && !Mathf.Approximately(_frustumSettings.QualitySettings.temporalReprojectionFactor, 0.0f));

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableVolumes, _frustumSettings.QualitySettings.enableVolumes && _volumesManager.HasVisibleVolumes);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableVolumesNoiseMask, _frustumSettings.QualitySettings.enableVolumesNoiseMask && _frustumParameters.HasFlags(FrustumParameters.EnableVolumes));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableVolumesTexture2DMask, _frustumSettings.QualitySettings.enableVolumesTexture2DMask && _frustumParameters.HasFlags(FrustumParameters.EnableVolumes) && AuraCamera.CommonDataManager.VolumesCommonDataManager.HasTexture2DMasks);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableVolumesTexture3DMask, _frustumSettings.QualitySettings.enableVolumesTexture3DMask && _frustumParameters.HasFlags(FrustumParameters.EnableVolumes) && AuraCamera.CommonDataManager.VolumesCommonDataManager.HasTexture3DMasks);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableAmbientLighting, _frustumSettings.BaseSettings.useAmbientLighting && _frustumSettings.QualitySettings.enableAmbientLighting && !Mathf.Approximately(_frustumSettings.BaseSettings.ambientLightingStrength, 0.0f));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableLightProbes, _frustumSettings.QualitySettings.enableLightProbes && AuraCamera.CommonDataManager.VolumesCommonDataManager.HasRegisteredLightProbesProxyVolumes && LightProbeProxyVolume.isFeatureSupported && LightmapSettings.lightProbes != null && LightmapSettings.lightProbes.count > 0);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableDirectionalLights, _frustumSettings.QualitySettings.enableDirectionalLights && AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalLightsManager.HasCandidateLights);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableDirectionalLightsShadows, _frustumSettings.QualitySettings.enableDirectionalLightsShadows && _frustumParameters.HasFlags(FrustumParameters.EnableDirectionalLights) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasDirectionalShadowCasters);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.DirectionalLightsShadowsOneCascade, QualitySettings.shadowCascades == 1 && _frustumParameters.HasFlags(FrustumParameters.EnableDirectionalLights));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.DirectionalLightsShadowsTwoCascades, QualitySettings.shadowCascades == 2 && _frustumParameters.HasFlags(FrustumParameters.EnableDirectionalLights));
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.DirectionalLightsShadowsFourCascades, QualitySettings.shadowCascades == 4 && _frustumParameters.HasFlags(FrustumParameters.EnableDirectionalLights));

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableSpotLights, _frustumSettings.QualitySettings.enableSpotLights && _spotLightsManager.HasVisibleLights);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableSpotLightsShadows, _frustumSettings.QualitySettings.enableSpotLightsShadows && _frustumParameters.HasFlags(FrustumParameters.EnableSpotLights) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasSpotShadowCasters);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnablePointLights, _frustumSettings.QualitySettings.enablePointLights && _pointLightsManager.HasVisibleLights);
            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnablePointLightsShadows, _frustumSettings.QualitySettings.enablePointLightsShadows && _frustumParameters.HasFlags(FrustumParameters.EnablePointLights) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasPointShadowCasters);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableLightsCookies, _frustumSettings.QualitySettings.enableLightsCookies && (_frustumParameters.HasFlags(FrustumParameters.EnablePointLights) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasPointCookieCasters || _frustumParameters.HasFlags(FrustumParameters.EnableSpotLights) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasSpotCookieCasters || _frustumParameters.HasFlags(FrustumParameters.EnableDirectionalLights) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasDirectionalCookieCasters));

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableDenoisingFilter, _frustumSettings.QualitySettings.EXPERIMENTAL_enableDenoisingFilter);

            _frustumParameters = _frustumParameters.ReplaceFlags(FrustumParameters.EnableBlurFilter, _frustumSettings.QualitySettings.EXPERIMENTAL_enableBlurFilter);
        }

        /// <summary>
        /// Tells if the specified flags combinaision is found inside the parameters bitmask
        /// </summary>
        /// <param name="flags">A combinaison of flags</param>
        /// <returns>Is the flags combinaison found in the parameters bitmask</returns>
        public bool HasFlags(FrustumParameters flags)
        {
            return _frustumParameters.HasFlags(flags);
        }
        #endregion
    }
}
