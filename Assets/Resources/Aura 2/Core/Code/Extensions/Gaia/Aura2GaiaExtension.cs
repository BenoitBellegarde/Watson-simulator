#if GAIA_PRESENT && UNITY_EDITOR

using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEditor;
using Aura2API;
using System.Linq;

namespace Gaia.GX.RaphaelErnaelsten
{
    /// <summary>
    /// Extension to add Aura 2 to Gaia terrains
    /// </summary>
    public class Aura2GaiaExtension
    {
        private static readonly Vector3Int _veryLowQualityResolution = new Vector3Int(40, 23, 16);
        private static readonly Vector3Int _lowQualityResolution = new Vector3Int(80, 45, 32);
        private static readonly Vector3Int _mediumQualityResolution = _lowQualityResolution * 2;
        private static readonly Vector3Int _highQualityResolution = _mediumQualityResolution * 2;
        private static readonly Vector3Int _ultraHighQualityResolution = _highQualityResolution * 2;

        #region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Raphael Ernaelsten (@RaphErnaelsten)";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Aura 2 - Volumetric Fog and Lighting";
        }

        #endregion

        #region Methods exposed by Gaia as buttons must be prefixed with GX_  
        #region Presets
        public static void GX_Presets_Dawn()
        {
            Aura.ApplyPreset(Presets.Dawn);
            SetDefaultCamerasQuality();
        }

        public static void GX_Presets_SunnyDay()
        {
            Aura.ApplyPreset(Presets.SunnyDay);
            SetDefaultCamerasQuality();
        }

        public static void GX_Presets_RainyDay()
        {
            Aura.ApplyPreset(Presets.RainyDay);
            SetDefaultCamerasQuality();
        }

        public static void GX_Presets_Forest()
        {
            Aura.ApplyPreset(Presets.Forest);
            SetDefaultCamerasQuality();
        }

        public static void GX_Presets_Desert()
        {
            Aura.ApplyPreset(Presets.Desert);
            SetDefaultCamerasQuality();
        }

        public static void GX_Presets_SnowyDay()
        {
            Aura.ApplyPreset(Presets.SnowyDay);
            SetDefaultCamerasQuality();
        }
        #endregion

        #region Quality
        public static void GX_Quality_SetVeryLowQuality()
        {
            SetResolutionToCameras(_veryLowQualityResolution);
        }

        public static void GX_Quality_SetLowQuality()
        {
            SetResolutionToCameras(_lowQualityResolution);
        }

        public static void GX_Quality_SetMediumQuality()
        {
            SetResolutionToCameras(_mediumQualityResolution);
        }

        public static void GX_Quality_SetHighQuality()
        {
            SetResolutionToCameras(_highQualityResolution);
        }

        public static void GX_Quality_SetUltraHighQuality()
        {
            SetResolutionToCameras(_ultraHighQualityResolution);
        }
        #endregion

        #region More Information
        public static void GX_MoreInformation_AboutAura() /////////////////////////////////////////////////////
        {
            EditorUtility.DisplayDialog("About Aura 2", "Aura 2 is the sequel of the Unity Awards 2018 winner (BEST ARTISTIC TOOL) Aura, the volumetric lighting solution for Unity.\n\nAura 2 simulates the scattering of the light in the environment and the illumination of micro-particles that are present but invisible to the eye/camera.\n\nThis phoenomenon is commonly known as the \"volumetric fog\".", "COOL !");
        }

        public static void GX_MoreInformation_DiscordServer()
        {
            Application.OpenURL("https://discord.gg/9Upuey2");
        }

        public static void GX_MoreInformation_Twitter()
        {
            Application.OpenURL("https://twitter.com/RaphErnaelsten");
        }
        #endregion

        #region Functions
        private static void SetResolutionToCameras(Vector3Int resolution)
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras();

            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.QualitySettings.SetFrustumGridResolution(resolution);
            }
        }

        private static void SetDefaultCamerasQuality()
        {
            AuraCamera[] auraCameras = Aura.AddAuraToCameras();

            for (int i = 0; i < auraCameras.Length; ++i)
            {
                auraCameras[i].frustumSettings.QualitySettings.occlusionCullingAccuracy = OcclusionCullingAccuracy.Highest; // Because of the trees' leaves
            }

            GX_Quality_SetHighQuality();
        }

        private static AuraLight[] SetupDirectionalLights()
        {
            AuraLight[] auraLights = Aura.AddAuraToDirectionalLights();
            if (auraLights.Length == 0)
            {
                auraLights = new AuraLight[1];
                auraLights[0] = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional).GetComponent<AuraLight>();
            }

            return auraLights;
        }

        private static AuraVolume[] GetVolumes()
        {
            AuraVolume[] auraVolumes = AuraVolume.FindObjectsOfType<AuraVolume>();

            return auraVolumes;
        }

        private static void DisableActiveAuraVolumes(AuraVolume[] auraVolumes)
        {
            for (int i = 0; i < auraVolumes.Length; ++i)
            {
                if (auraVolumes[i].gameObject.activeInHierarchy)
                {
                    auraVolumes[i].gameObject.SetActive(false);
                    Debug.LogWarning("The AuraVolume's gameObject named \"" + auraVolumes[i].gameObject.name + "\" has been disabled to achieve the Preset's goal.", auraVolumes[i]);
                }
            }
        }
        #endregion
        #endregion
    }
}
#endif