
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

using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Ordered struct of volume parameters to be sent to the compute shader
    /// </summary>
    public struct VolumeData
    {
        #region Public Members
        /// <summary>
        /// Transform of the volume
        /// </summary>
        public MatrixFloats transform;
        /// <summary>
        /// Id of the shape of the volume
        /// </summary>
        public int shape;
        /// <summary>
        /// Exponent the fading gradient will be raised to
        /// </summary>
        public float falloffExponent;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local X axis
        /// </summary>
        public float xPositiveFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local X axis
        /// </summary>
        public float xNegativeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Y axis
        /// </summary>
        public float yPositiveFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Y axis
        /// </summary>
        public float yNegativeFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the positive local Z axis
        /// </summary>
        public float zPositiveFade;
        /// <summary>
        /// Normalized size of the fading on the borders, on the negative local Z axis
        /// </summary>
        public float zNegativeFade;
        /// <summary>
        /// Use the volume to show light probes lighting
        /// </summary>
        public int useAsLightProbesProxyVolume;
        /// <summary>
        /// Light probes lighting multiplier
        /// </summary>
        public float lightProbesMultiplier;
        /// <summary>
        /// Texture2D mask parameters
        /// </summary>
        public TextureMaskData texture2DMaskData;
        /// <summary>
        /// Texture3D mask parameters
        /// </summary>
        public TextureMaskData texture3DMaskData;
        /// <summary>
        /// Noise parameters
        /// </summary>
        public VolumeDynamicNoiseData noiseData;
        /// <summary>
        /// Enables density injection
        /// </summary>
        public int injectDensity;
        /// <summary>
        /// Density injection strength
        /// </summary>
        public float densityValue;
        /// <summary>
        /// Density Texture2D mask levels parameters
        /// </summary>
        public LevelsData densityTexture2DMaskLevelsParameters;
        /// <summary>
        /// Density Texture3D mask levels parameters
        /// </summary>
        public LevelsData densityTexture3DMaskLevelsParameters;
        /// <summary>
        /// Density noise mask levels parameters
        /// </summary>
        public LevelsData densityNoiseLevelsParameters;
        /// <summary>
        /// Enables scattering injection
        /// </summary>
        public int injectScattering;
        /// <summary>
        /// Scattering injection strength
        /// </summary>
        public float scatteringValue;
        /// <summary>
        /// Scattering Texture2D mask levels parameters
        /// </summary>
        public LevelsData scatteringTexture2DMaskLevelsParameters;
        /// <summary>
        /// Scattering Texture3D mask levels parameters
        /// </summary>
        public LevelsData scatteringTexture3DMaskLevelsParameters;
        /// <summary>
        /// Scattering noise mask levels parameters
        /// </summary>
        public LevelsData scatteringNoiseLevelsParameters;
        /// <summary>
        /// Enables color injection
        /// </summary>
        public int injectColor;
        /// <summary>
        /// Color value * injection strength
        /// </summary>
        public Vector3 colorValue;
        /// <summary>
        /// Color Texture2D mask levels parameters
        /// </summary>
        public LevelsData colorTexture2DMaskLevelsParameters;
        /// <summary>
        /// Color Texture3D mask levels parameters
        /// </summary>
        public LevelsData colorTexture3DMaskLevelsParameters;
        /// <summary>
        /// Color noise mask levels parameters
        /// </summary>
        public LevelsData colorNoiseLevelsParameters;
        /// <summary>
        /// Enables tint injection
        /// </summary>
        public int injectTint;
        /// <summary>
        /// Tint value
        /// </summary>
        public Vector3 tintColor;
        /// <summary>
        /// Tint Texture2D mask levels parameters
        /// </summary>
        public LevelsData tintTexture2DMaskLevelsParameters;
        /// <summary>
        /// Tint Texture3D mask levels parameters
        /// </summary>
        public LevelsData tintTexture3DMaskLevelsParameters;
        /// <summary>
        /// Tint noise mask levels parameters
        /// </summary>
        public LevelsData tintNoiseLevelsParameters;
        /// <summary>
        /// Enables ambient lighting injection
        /// </summary>
        public int injectAmbient;
        /// <summary>
        /// Ambient Lighting injection strength
        /// </summary>
        public float ambientLightingValue;
        /// <summary>
        /// Ambient Texture2D mask levels parameters
        /// </summary>
        public LevelsData ambientTexture2DMaskLevelsParameters;
        /// <summary>
        /// Ambient Texture3D mask levels parameters
        /// </summary>
        public LevelsData ambientTexture3DMaskLevelsParameters;
        /// <summary>
        /// Ambient noise mask levels parameters
        /// </summary>
        public LevelsData ambientNoiseLevelsParameters;
        /// <summary>
        /// Enables boost injection
        /// </summary>
        public int injectBoost;
        /// <summary>
        /// Boost injection strength
        /// </summary>
        public float boostValue;
        /// <summary>
        /// Boost Texture2D mask levels parameters
        /// </summary>
        public LevelsData boostTexture2DMaskLevelsParameters;
        /// <summary>
        /// Boost Texture3D mask levels parameters
        /// </summary>
        public LevelsData boostTexture3DMaskLevelsParameters;
        /// <summary>
        /// Boost noise mask levels parameters
        /// </summary>
        public LevelsData boostNoiseLevelsParameters;
        #endregion

        #region Private Members
        /// <summary>
        /// The bytes size of the struct
        /// </summary>
        private static int _byteSize = 0;
        #endregion

        #region Properties
        /// <summary>
        /// The bytes size of the struct
        /// </summary>
        public static int Size
        {
            get
            {
                if (_byteSize == 0)
                {
                    _byteSize += MatrixFloats.Size;              // transform
                    _byteSize += sizeof(int);                    // type
                    _byteSize += sizeof(float);                  // falloffExponent
                    _byteSize += sizeof(float);                  // xPositiveFade
                    _byteSize += sizeof(float);                  // xNegativeFade
                    _byteSize += sizeof(float);                  // yPositiveFade
                    _byteSize += sizeof(float);                  // yNegativeFade
                    _byteSize += sizeof(float);                  // zPositiveFade
                    _byteSize += sizeof(float);                  // zNegativeFade
                    _byteSize += sizeof(int);                    // useAsLightProbesProxyVolume
                    _byteSize += sizeof(float);                  // lightProbesMultiplier
                    _byteSize += TextureMaskData.Size;           // texture2DMaskData
                    _byteSize += TextureMaskData.Size;           // texture3DMaskData
                    _byteSize += VolumeDynamicNoiseData.Size;    // noiseData
                    _byteSize += sizeof(int);                    // injectDensity
                    _byteSize += sizeof(float);                  // densityValue
                    _byteSize += LevelsData.Size;                // densityTexture2DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // densityTexture3DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // densityNoiseLevelsParameters
                    _byteSize += sizeof(int);                    // injectScattering
                    _byteSize += sizeof(float);                  // scatteringValue
                    _byteSize += LevelsData.Size;                // scatteringTexture2DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // scatteringTexture3DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // scatteringNoiseLevelsParameters
                    _byteSize += sizeof(int);                    // injectColor
                    _byteSize += sizeof(float) * 3;              // colorValue
                    _byteSize += LevelsData.Size;                // colorTexture2DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // colorTexture3DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // colorNoiseLevelsParameters
                    _byteSize += sizeof(int);                    // injectTint
                    _byteSize += sizeof(float) * 3;              // tintColor
                    _byteSize += LevelsData.Size;                // tintTexture2DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // tintTexture3DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // tintNoiseLevelsParameters
                    _byteSize += sizeof(int);                    // injectAmbient
                    _byteSize += sizeof(float);                  // ambientLightingValue
                    _byteSize += LevelsData.Size;                // ambientTexture2DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // ambientTexture3DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // ambientNoiseLevelsParameters
                    _byteSize += sizeof(int);                    // injectBoost
                    _byteSize += sizeof(float);                  // boostValue
                    _byteSize += LevelsData.Size;                // boostTexture2DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // boostTexture3DMaskLevelsParameters
                    _byteSize += LevelsData.Size;                // boostNoiseLevelsParameters
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
