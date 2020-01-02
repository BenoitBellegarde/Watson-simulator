
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

#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace Aura2API
{
    [InitializeOnLoad]
    public class SceneViewVisualization
    {
        #region Public Members
        /// <summary>
        /// The define symbol string for enablin aura visualization in the SceneView
        /// </summary>
        public const string symbolString = "AURA_IN_SCENEVIEW";
        #endregion

        #region Private Members
        //private static Buffers _textureBuffers;
        #endregion

        #region Properties
        //public static Vector3Int FrustumGridResolution
        //{
        //    get
        //    {
        //        return EditionSettings.EditionCameraQualitySettings.frustumGridResolution;
        //    }
        //}
        //
        //public static Vector4 BufferResolutionVector
        //{
        //    get
        //    {
        //        return new Vector4(FrustumGridResolution.x, FrustumGridResolution.y, FrustumGridResolution.z, 1.0f);
        //    }
        //}
        //
        //public static Vector4 BufferTexelSizeVector
        //{
        //    get
        //    {
        //        return new Vector4(1.0f / FrustumGridResolution.x, 1.0f / FrustumGridResolution.y, 1.0f / FrustumGridResolution.z, 1.0f);
        //    }
        //}
        //
        //public static Buffers TextureBuffers
        //{
        //    get
        //    {
        //        if(_textureBuffers == null)
        //        {
        //            _textureBuffers = new Buffers();
        //            _textureBuffers.resolution = FrustumGridResolution;
        //        }
        //
        //        return _textureBuffers;
        //    }
        //}
        //
        //public static SwappableRenderTexture DataVolumeTexture
        //{
        //    get
        //    {
        //        return TextureBuffers.DataVolumeTexture;
        //    }
        //}
        //
        //public static RenderTexture FogVolumeTexture
        //{
        //    get
        //    {
        //        return TextureBuffers.FogVolumeTexture;
        //    }
        //}
        //
        //public static SwappableRenderTexture OcclusionTexture
        //{
        //    get
        //    {
        //        return TextureBuffers.OcclusionTexture;
        //    }
        //}
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        static SceneViewVisualization()
        {
            EditorApplication.update += OnEditorApplicationUpdate;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Called on every editor update
        /// </summary>
        private static void OnEditorApplicationUpdate()
        {
            if (AuraEditorPrefs.EnableAuraInSceneView)
            {
                EnableAuraInSceneView();
            }
            else
            {
                DisableAuraInSceneView();
            }
        }
        
        /// <summary>
        /// Tells if Aura SceneView Visualization is enabled
        /// </summary>
        public static bool AuraIsEnabledInSceneView
        {
            get
            {
                #if AURA_IN_SCENEVIEW
                return true;
                #else
                return false;
                #endif
                //return AuraUtility.GetIfSymbolIsDefined(symbolString);
            }
        }

        /// <summary>
        /// Enabled Aura Visualization in SceneView
        /// </summary>
        public static void EnableAuraInSceneView()
        {
            if(!AuraIsEnabledInSceneView)
            {
                AuraUtility.AddDefineSymbol(symbolString);
            }
        }

        /// <summary>
        /// Disable Aura Visualization in SceneView
        /// </summary>
        public static void DisableAuraInSceneView()
        {
            //        TextureBuffers.ReleaseAllBuffers();
            if(AuraIsEnabledInSceneView)
            {
                AuraUtility.RemoveDefineSymbol(symbolString);
            }
        }

        //public static void SnapFrustumGridResolution()
        //{
        //    EditionSettings.EditionCameraQualitySettings.frustumGridResolution.x = Mathf.Max(64, EditionSettings.EditionCameraQualitySettings.frustumGridResolution.x.Snap(8));
        //    EditionSettings.EditionCameraQualitySettings.frustumGridResolution.y = Mathf.Max(32, EditionSettings.EditionCameraQualitySettings.frustumGridResolution.y.Snap(8));
        //    EditionSettings.EditionCameraQualitySettings.frustumGridResolution.z = Mathf.Max(32, EditionSettings.EditionCameraQualitySettings.frustumGridResolution.z.Snap(8));
        //}
        //
        //public static void SetSceneViewVisualizationFrustumGridResolution(Vector3Int newResolution)
        //{
        //    EditionSettings.EditionCameraQualitySettings.frustumGridResolution = newResolution;
        //    SnapFrustumGridResolution();
        //
        //    TextureBuffers.resolution = FrustumGridResolution;
        //    TextureBuffers.ReleaseAllBuffers();
        //}
        #endregion
    }
}
#endif