
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    public class AuraResourcesCollection : ScriptableObject
    {
        #region Public Members
        [Header("Compute Shaders")]
        /// <summary>
        /// Compute Shader in charge of computing the maximum depth for occlusion culling
        /// </summary>
        public ComputeShader computeMaximumDepthComputeShader;
        /// <summary>
        /// Compute Shader in charge of ruling out the invisible cells of the volumetric grid
        /// </summary>
        public ComputeShader computeVisibleCellsComputeShader;
        /// <summary>
        /// Compute Shader in charge of computing the data contribution inside the frustum
        /// </summary>
        public ComputeShader computeDataComputeShader;
        /// <summary>
        /// Compute Shader in charge of accumulating the data
        /// </summary>
        public ComputeShader computeAccumulationComputeShader;
        /// <summary>
        /// Compute Shader in charge of rendering the light probes' coefficients into the 3D texture buffer
        /// </summary>
        public ComputeShader renderLightProbesTextureComputeShader;
        /// <summary>
        /// Compute Shader in charge of applying the denoising filter
        /// </summary>
        public ComputeShader applyDenoisingFilterComputeShader;
        /// <summary>
        /// Compute Shader in charge of applying the blur filter
        /// </summary>
        public ComputeShader applyBlurFilterComputeShader;

        [Header("Shaders")]
        /// <summary>
        /// Shader in charge of filtering and formatting the maximum depth
        /// </summary>
        public Shader processOcclusionMapShader;
        /// <summary>
        /// Shader used for applying the volumetric lighting as Post Process
        /// </summary>
        public Shader postProcessShader;
        /// <summary>
        /// Shader used to copy the directional light's cascaded shadows projection data
        /// </summary>
        public Shader storeDirectionalShadowDataShader;
        /// <summary>
        /// Shader used to store the directional/spot light's cookie map
        /// </summary>
        public Shader storeDirectionalSpotCookieMapShader;
        /// <summary>
        /// Shader used to store the point light's shadow map (renders the TextureCUBE into a Texture2D)
        /// </summary>
        public Shader storePointShadowMapShader;
        /// <summary>
        /// Shader used to store the point light's cookie map (renders the TextureCUBE into a Texture2D)
        /// </summary>
        public Shader storePointCookieMapShader;
        /// <summary>
        /// Lit shader used to make sprites work with the Aura system
        /// </summary>
        public Shader spriteLitShader;
        /// <summary>
        /// Unlit shader used to make sprites work with the Aura system
        /// </summary>
        public Shader spriteUnlitShader;

        [Header("Textures")]
        /// <summary>
        /// Texture containing blue noise for dithering volumetric lighting
        /// </summary>
        public Texture2DArray blueNoiseTextureArray;
        /// <summary>
        /// Dummy Texture2D with 2x2 white pixels
        /// </summary>
        public Texture2D dummyTexture;
        /// <summary>
        /// Dummy RenderTexture with 2x2 black R8 pixels and UAV/enableRandomWrite flag set
        /// </summary>
        public RenderTexture _dummyTextureUAV;
        public RenderTexture DummyTextureUAV
        {
            get
            {
                if(_dummyTextureUAV == null)
                {
                    _dummyTextureUAV = new RenderTexture(2, 2, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
                    _dummyTextureUAV.enableRandomWrite = true;
                    _dummyTextureUAV.Create();
                }

                return _dummyTextureUAV;
            }
        }
        /// <summary>
        /// Dummy Texture2DArray with 2x2x1 white pixels
        /// </summary>
        public Texture2DArray dummyTextureArray;
        /// <summary>
        /// Dummy Texture3D with 2x2x2 white pixels
        /// </summary>
        public Texture3D dummyTexture3D;
        /// <summary>
        /// Default Sprite texture
        /// </summary>
        public Sprite defaultSprite;

        [Header("Meshes")]
        /// <summary>
        /// Mesh used to store the point light's shadow map
        /// </summary>
        public Mesh storePointShadowMapMesh;

#if UNITY_EDITOR
        [Header("Editor Floats")]
        public float customWindowsInMotionDuration = 1.0f;
        public float customWindowsOutMotionDuration = 1.0f;
        [Header("Editor Curves")]
        public AnimationCurve customWindowsInMotionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve customWindowsOutMotionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [Header("Editor Textures")]
        public Texture2D logoTexture;
        public Texture2D injectionIconTexture;
        public Texture2D settingsIconTexture;
        public Texture2D gridIconTexture;
        public Texture2D listIconTexture;
        public Texture2D debugIconTexture;
        public Texture2D optionsIconTexture;
        public Texture2D experimentalIconTexture;
        public Texture2D additionalSettingsIconTexture;
        public Texture2D lightIconTexture;
        public Texture2D shapeIconTexture;
        public Texture2D shapeMiniIconTexture;
        public Texture2D texture3DIconTexture;
        public Texture2D textureIconTexture;
        public Texture2D noiseIconTexture;
        public Texture2D densityIconTexture;
        public Texture2D colorIconTexture;
        public Texture2D tintIconTexture;
        public Texture2D scatteringIconTexture;
        public Texture2D extinctionIconTexture;
        public Texture2D illuminationColorIconTexture;
        public Texture2D illuminationIconTexture;
        public Texture2D lightProbesIconTexture;
        public Texture2D boostIconTexture;
        public Texture2D logoIconTexture;
        public Texture2D baseSettingsPresetIconTexture;
        public Texture2D qualitySettingsPresetIconTexture;
        public Texture2D saveIconTexture;
        public Texture2D loadIconTexture;
        public Texture2D cameraUiIconTexture;
        public Texture2D cameraIconTexture;
        public Texture2D cameraMiniIconTexture;
        public Texture2D directionalLightIconTexture;
        public Texture2D directionalLightMiniIconTexture;
        public Texture2D spotLightIconTexture;
        public Texture2D spotLightMiniIconTexture;
        public Texture2D pointLightIconTexture;
        public Texture2D pointLightMiniIconTexture;
        public Texture2D displayPresetsButtonTexture;
        public Texture2D presetUiIconTexture;
        public Texture2D addIconTexture;
        public Texture2D removeIconTexture;
        public Texture2D toggleIconTexture;
        public Texture2D questionIconTexture;
        public Texture2D warningIconTexture;
        public Texture2D lightUiIconTexture;
        public Texture2D volumeUiIconTexture;
        public Texture2D creationSpaceButtonTopologyTexture;
        public Texture2D creationSpaceButtonNormalTexture;
        public Texture2D creationSpaceButtonWorldTexture;
        public Texture2D focusIconTexture;
        public Texture2D creationTypeButtonGlobalTexture;
        public Texture2D creationTypeButtonLayerTexture;
        public Texture2D creationTypeButtonBoxTexture;
        public Texture2D creationTypeButtonSphereTexture;
        public Texture2D creationTypeButtonCylinderTexture;
        public Texture2D creationTypeButtonConeTexture;
        public Texture2D toggleOnHorizontalIconTexture;
        public Texture2D toggleOffHorizontalIconTexture;
        public Texture2D toggleOnVerticalIconTexture;
        public Texture2D toggleOffVerticalIconTexture;
        public Texture2D upIconTexture;
        public Texture2D downIconTexture;
        public Texture2D leftIconTexture;
        public Texture2D rightIconTexture;
        public Texture2D spriteUiIconTexture;
        public Texture2D spriteIconTexture;
        public Texture2D spriteMiniIconTexture;
        [Header("Presets buttons textures")]
        public Texture2D[] presetsButtonsTextures;
        [Header("Styles Textures")]
        public Texture2D backgroundStylesTexture;
        public Texture2D buttonUpStylesTexture;
        public Texture2D buttonHoverStylesTexture;
        public Texture2D buttonDownStylesTexture;
        public Texture2D checkerUpStylesTexture;
        public Texture2D checkerHoverStylesTexture;
        public Texture2D checkerDownStylesTexture;
        public Texture2D checkerCheckedStylesTexture;
        [Header("Edition Settings")]
        public AuraQualitySettings editionCameraQualitySettings;
        [Header("Splash Screens")]
        public Texture2D[] mainIntroductionScreenTextures;
        public Texture2D[] cameraIntroductionScreenTextures;
#endif
        #endregion
    }
}