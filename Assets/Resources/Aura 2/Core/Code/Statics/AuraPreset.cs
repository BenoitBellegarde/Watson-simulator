
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
using UnityEngine.Rendering;

namespace Aura2API
{
    /// <summary>
    /// Collection of functions related to presets
    /// </summary>
    public static class AuraPreset
    {
        #region Private Members
        public const string _presetVolumesName = "Aura Preset Volume";
        #endregion

        #region Functions
        /// <summary>
        /// Deletes the volumes created by applying presets
        /// </summary>
        private static void DeletePresetVolumes()
        {
            AuraVolume[] volumes = Aura.GetAuraVolumes();
            for (int i = 0; i < volumes.Length; ++i)
            {
                if (volumes[i].name == _presetVolumesName)
                {
                    volumes[i].gameObject.Destroy();
                }
            }
        }

        /// <summary>
        /// Applies a preset to the Aura system
        /// </summary>
        /// <param name="preset">The desired preset</param>
        public static void ApplyPreset(Presets preset)
        {
            switch(preset)
            {
                case Presets.Dawn:
                    {
                        ApplyDawnPreset();
                    }
                    break;

                case Presets.SunnyDay:
                    {
                        ApplySunnyDayPreset();
                    }
                    break;

                case Presets.RainyDay:
                    {
                        ApplyRainyDayPreset();
                    }
                    break;

                case Presets.Forest:
                    {
                        ApplyForestPreset();
                    }
                    break;

                case Presets.Desert:
                    {
                        ApplyDesertPreset();
                    }
                    break;

                case Presets.SnowyDay:
                    {
                        ApplySnowyDayPreset();
                    }
                    break;

                default:
                    {
                        ApplySunnyDayPreset();
                    }
                    break;
            }
        }

        #region Presets
        /// <summary>
        /// Applies the "Dawn" preset
        /// </summary>
        public static void ApplyDawnPreset()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras(1);
            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.BaseSettings.useDensity = true;
                auraCameras[i].frustumSettings.BaseSettings.density = 0.25f;
                auraCameras[i].frustumSettings.BaseSettings.useScattering = true;
                auraCameras[i].frustumSettings.BaseSettings.scattering = 0.5f;
                auraCameras[i].frustumSettings.BaseSettings.useAmbientLighting = true;
                auraCameras[i].frustumSettings.BaseSettings.ambientLightingStrength = 1.0f;
            }

            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            Color tmp;
            ColorUtility.TryParseHtmlString("#203140", out tmp);
            RenderSettings.ambientSkyColor = tmp;
            ColorUtility.TryParseHtmlString("#402F20", out tmp);
            RenderSettings.ambientEquatorColor = tmp;
            ColorUtility.TryParseHtmlString("#241C1C", out tmp);
            RenderSettings.ambientGroundColor = tmp;

            AuraLight[] directionalLights = Aura.AddAuraToDirectionalLights(1);
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 10.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.0777f, 0.92f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.0f;

                directionalLights[i].strength = 1.0f;
                directionalLights[i].enableOutOfPhaseColor = false;
                directionalLights[i].outOfPhaseColor = Color.HSVToRGB(0.025f, 0.6f, 1);
                directionalLights[i].outOfPhaseColorStrength = 0.25f;
            }

            DeletePresetVolumes();

            AuraVolume globalVolume = AuraVolume.CreateGameObject(_presetVolumesName, VolumeType.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.transform.space = Space.World;
            globalVolume.noiseMask.transform.scale = Vector3.one * 5.0f;
            globalVolume.densityInjection.enable = true;
            globalVolume.densityInjection.strength = 0.1f;
            globalVolume.densityInjection.noiseMaskLevelParameters.contrast = 5.0f;
            globalVolume.lightInjection.injectionParameters.enable = false;
            globalVolume.scatteringInjection.enable = true;
            globalVolume.scatteringInjection.strength = 0.25f;
        }

        /// <summary>
        /// Applies the "SunnyDay" preset
        /// </summary>
        public static void ApplySunnyDayPreset()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras(1);
            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.BaseSettings.useDensity = true;
                auraCameras[i].frustumSettings.BaseSettings.density = 0.05f;
                auraCameras[i].frustumSettings.BaseSettings.useScattering = true;
                auraCameras[i].frustumSettings.BaseSettings.scattering = 0.5f;
                auraCameras[i].frustumSettings.BaseSettings.useAmbientLighting = true;
                auraCameras[i].frustumSettings.BaseSettings.ambientLightingStrength = 1.0f;
            }

            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            Color tmp;
            ColorUtility.TryParseHtmlString("#406381", out tmp);
            RenderSettings.ambientSkyColor = tmp;
            ColorUtility.TryParseHtmlString("#402F20", out tmp);
            RenderSettings.ambientEquatorColor = tmp;
            ColorUtility.TryParseHtmlString("#241C1C", out tmp);
            RenderSettings.ambientGroundColor = tmp;

            AuraLight[] directionalLights = Aura.AddAuraToDirectionalLights(1);
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                ColorUtility.TryParseHtmlString("#FFD0A6", out tmp);
                directionalLights[i].GetComponent<Light>().color = tmp;
                directionalLights[i].GetComponent<Light>().intensity = 1.4f;

                directionalLights[i].strength = 1.0f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            DeletePresetVolumes();
        }

        /// <summary>
        /// Applies the "RainyDay" preset
        /// </summary>
        public static void ApplyRainyDayPreset()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras(1);
            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.BaseSettings.useDensity = true;
                auraCameras[i].frustumSettings.BaseSettings.density = 0.5f;
                auraCameras[i].frustumSettings.BaseSettings.useScattering = true;
                auraCameras[i].frustumSettings.BaseSettings.scattering = 1.0f;
                auraCameras[i].frustumSettings.BaseSettings.useAmbientLighting = true;
                auraCameras[i].frustumSettings.BaseSettings.ambientLightingStrength = 1.25f;
            }

            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            Color tmp;
            ColorUtility.TryParseHtmlString("#406381", out tmp);
            RenderSettings.ambientSkyColor = tmp;
            ColorUtility.TryParseHtmlString("#2C4459", out tmp);
            RenderSettings.ambientEquatorColor = tmp;
            ColorUtility.TryParseHtmlString("#131D26", out tmp);
            RenderSettings.ambientGroundColor = tmp;

            AuraLight[] directionalLights = Aura.AddAuraToDirectionalLights(1);
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.27f, 0.15f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 0.8f;

                directionalLights[i].strength = 0.5f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            DeletePresetVolumes();

            AuraVolume globalVolume = AuraVolume.CreateGameObject(_presetVolumesName, VolumeType.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.speed = 0.15f;
            globalVolume.noiseMask.transform.space = Space.World;
            globalVolume.noiseMask.transform.scale = Vector3.one * 3.0f;
            globalVolume.densityInjection.enable = true;
            globalVolume.densityInjection.strength = 0.2f;
            globalVolume.densityInjection.noiseMaskLevelParameters.contrast = 15.0f;
            globalVolume.densityInjection.noiseMaskLevelParameters.outputLowValue = 0.0f;
            globalVolume.densityInjection.noiseMaskLevelParameters.outputHiValue = -1.0f;
            globalVolume.lightInjection.injectionParameters.enable = false;
            globalVolume.scatteringInjection.enable = false;
        }

        /// <summary>
        /// Applies the "Forest" preset
        /// </summary>
        public static void ApplyForestPreset()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras(1);
            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.BaseSettings.useDensity = true;
                auraCameras[i].frustumSettings.BaseSettings.density = 0.3f;
                auraCameras[i].frustumSettings.BaseSettings.useScattering = true;
                auraCameras[i].frustumSettings.BaseSettings.scattering = 0.75f;
                auraCameras[i].frustumSettings.BaseSettings.useAmbientLighting = true;
                auraCameras[i].frustumSettings.BaseSettings.ambientLightingStrength = 1.0f;
            }

            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            Color tmp;
            ColorUtility.TryParseHtmlString("#406381", out tmp);
            RenderSettings.ambientSkyColor = tmp;
            ColorUtility.TryParseHtmlString("#37402C", out tmp);
            RenderSettings.ambientEquatorColor = tmp;
            ColorUtility.TryParseHtmlString("#212420", out tmp);
            RenderSettings.ambientGroundColor = tmp;

            AuraLight[] directionalLights = Aura.AddAuraToDirectionalLights(1);
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                directionalLights[i].GetComponent<Light>().color = Color.HSVToRGB(0.12f, 0.35f, 1.0f);
                directionalLights[i].GetComponent<Light>().intensity = 1.0f;

                directionalLights[i].strength = 0.5f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            DeletePresetVolumes();

            AuraVolume globalVolume = AuraVolume.CreateGameObject(_presetVolumesName, VolumeType.Global).GetComponent<AuraVolume>();
            globalVolume.noiseMask.enable = true;
            globalVolume.noiseMask.speed = 0.15f;
            globalVolume.noiseMask.transform.space = Space.World;
            globalVolume.noiseMask.transform.scale = Vector3.one * 3.0f;
            globalVolume.densityInjection.enable = true;
            globalVolume.densityInjection.strength = 0.1f;
            globalVolume.densityInjection.noiseMaskLevelParameters.contrast = 15.0f;
            globalVolume.densityInjection.noiseMaskLevelParameters.outputLowValue = 0.0f;
            globalVolume.densityInjection.noiseMaskLevelParameters.outputHiValue = -1.0f;
            globalVolume.lightInjection.injectionParameters.enable = false;
            globalVolume.scatteringInjection.enable = false;
        }

        /// <summary>
        /// Applies the "Desert" preset
        /// </summary>
        public static void ApplyDesertPreset()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras(1);
            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.BaseSettings.useDensity = true;
                auraCameras[i].frustumSettings.BaseSettings.density = 0.5f;
                auraCameras[i].frustumSettings.BaseSettings.useScattering = true;
                auraCameras[i].frustumSettings.BaseSettings.scattering = 0.5f;
                auraCameras[i].frustumSettings.BaseSettings.useAmbientLighting = true;
                auraCameras[i].frustumSettings.BaseSettings.ambientLightingStrength = 2.0f;
            }

            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            Color tmp;
            ColorUtility.TryParseHtmlString("#817251", out tmp);
            RenderSettings.ambientSkyColor = tmp;
            ColorUtility.TryParseHtmlString("#403A2C", out tmp);
            RenderSettings.ambientEquatorColor = tmp;
            ColorUtility.TryParseHtmlString("#242320", out tmp);
            RenderSettings.ambientGroundColor = tmp;

            AuraLight[] directionalLights = Aura.AddAuraToDirectionalLights(1);
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                ColorUtility.TryParseHtmlString("#FFD780", out tmp);
                directionalLights[i].GetComponent<Light>().color = tmp;
                directionalLights[i].GetComponent<Light>().intensity = 1.4f;

                directionalLights[i].strength = 0.25f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            DeletePresetVolumes();
        }

        /// <summary>
        /// Applies the "SnowyDay" preset
        /// </summary>
        public static void ApplySnowyDayPreset()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras(1);
            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.BaseSettings.useDensity = true;
                auraCameras[i].frustumSettings.BaseSettings.density = 0.25f;
                auraCameras[i].frustumSettings.BaseSettings.useScattering = true;
                auraCameras[i].frustumSettings.BaseSettings.scattering = 1.0f;
                auraCameras[i].frustumSettings.BaseSettings.useAmbientLighting = true;
                auraCameras[i].frustumSettings.BaseSettings.ambientLightingStrength = 3.0f;
            }

            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            Color tmp;
            ColorUtility.TryParseHtmlString("#70818C", out tmp);
            RenderSettings.ambientSkyColor = tmp;
            ColorUtility.TryParseHtmlString("#546069", out tmp);
            RenderSettings.ambientEquatorColor = tmp;
            ColorUtility.TryParseHtmlString("#384046", out tmp);
            RenderSettings.ambientGroundColor = tmp;

            AuraLight[] directionalLights = Aura.AddAuraToDirectionalLights(1);
            for (int i = 0; i < directionalLights.Length; ++i)
            {
                Vector3 tmpRotation = directionalLights[i].transform.rotation.eulerAngles;
                tmpRotation.x = 50.0f;
                directionalLights[i].transform.rotation = Quaternion.Euler(tmpRotation);

                ColorUtility.TryParseHtmlString("#CAE8FF", out tmp);
                directionalLights[i].GetComponent<Light>().color = tmp;
                directionalLights[i].GetComponent<Light>().intensity = 1.4f;

                directionalLights[i].strength = 0.25f;
                directionalLights[i].enableOutOfPhaseColor = false;
            }

            DeletePresetVolumes();
        }
        #endregion
        #endregion
    }
}