
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
    /// Component allowing the volume injection of density/scattering/light
    /// </summary>
    [AddComponentMenu("Aura 2/Aura Volume", 2)]
    [ExecuteInEditMode]
    [Serializable]
    public class AuraVolume : CullableObject
    {
        #region Public members
        /// <summary>
        /// Shape of the volume and its fading parameters"  
        /// </summary>
        [SerializeField]
        public VolumeInjectionShape volumeShape;
        /// <summary>
        /// Enables the volume to show the light probes lighting
        /// </summary>
        public bool useAsLightProbesProxyVolume = false;
        /// <summary>
        /// Multiplies the light probes lighting
        /// </summary>
        public float lightProbesMultiplier = 1;
        /// <summary>
        ///     Texture2D mask (RGB for color, A for density and scattering)
        /// </summary>
        public Texture2DMask texture2DMask;
        /// <summary>
        ///     Texture3D mask (RGB for color, A for density and scattering)
        /// </summary>
        public Texture3DMask texture3DMask;
        /// <summary>
        ///     Volumetric noise mask
        /// </summary>
        public DynamicNoiseParameters noiseMask;
        /// <summary>
        /// Density injection parameters
        /// </summary>
        public VolumeInjectionCommonParameters densityInjection;
        /// <summary>
        ///     Scattering injection parameters
        /// </summary>
        public VolumeInjectionCommonParameters scatteringInjection;
        /// <summary>
        ///     Light injection parameters
        /// </summary>
        public VolumeInjectionColorParameters lightInjection;
        /// <summary>
        ///     Tint injection parameters
        /// </summary>
        public VolumeInjectionColorParameters tintInjection;
        /// <summary>
        ///     Ambient lighting injection parameters
        /// </summary>
        public VolumeInjectionCommonParameters ambientInjection;
        /// <summary>
        /// Boost injection parameters
        /// </summary>
        public VolumeInjectionCommonParameters boostInjection;
        #endregion

        #region Private members
        /// <summary>
        ///     Packed data to be sent to the compute shader
        /// </summary>
        private VolumeData _volumeData;
        /// <summary>
        ///     Tells if the component is succesfully initialized
        /// </summary>
        private bool _isInitialized;
        /// <summary>
        /// The bounds of the volume
        /// </summary>
        private Bounds _bounds;
        /// <summary>
        /// Previous light probes proxy activation state
        /// </summary>
        private bool _previousUseAsLightProbesProxyVolume;
        /// <summary>
        ///     Previous Texture2D mask activation state for Texture2D mask activation state changed event
        /// </summary>
        private bool _previousTexture2DMaskUsage;
        /// <summary>
        ///     Previous Texture2D mask reference for Texture2D mask reference changed event
        /// </summary>
        private Texture2D _previousTexture2DMask;
        /// <summary>
        ///     Previous Texture3D mask activation state for Texture3D mask activation state changed event
        /// </summary>
        private bool _previousTexture3DMaskUsage;
        /// <summary>
        ///     Previous Texture3D mask reference for Texture3D mask reference changed event
        /// </summary>
        private Texture3D _previousTexture3DMask;
        #endregion

        #region Properties
        /// <summary>
        /// The bounds of the current volume
        /// </summary>
        public Bounds Bounds
        {
            get
            {
                if (transform.hasChanged)
                {
                    _bounds = new Bounds(BoundingSphere.position, Vector3.zero);

                    switch (volumeShape.shape)
                    {
                        default:
                            {
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f)));
                            }
                            break;

                        case VolumeType.Global:
                            {
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f) * 2.0f));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f) * 2.0f));
                            }
                            break;

                        case VolumeType.Layer:
                            {
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.0f, 0.0f, 0.0f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.0f, 1.0f, 0.0f)));
                            }
                            break;

                        case VolumeType.Cone:
                            {
                                _bounds.Encapsulate(transform.position);
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(Vector3.forward));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, 1.0f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, 1.0f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, 0.5f, 1.0f)));
                                _bounds.Encapsulate(transform.localToWorldMatrix.MultiplyPoint(new Vector3(0.5f, -0.5f, 1.0f)));
                            }
                            break;

                    }

                    transform.hasChanged = false;
                }

                return _bounds;
            }
        }

        /// <summary>
        /// Tells if it should use Texture2D masking
        /// </summary>
        public bool UsesTexture2DMasking
        {
            get
            {
                return texture2DMask.enable && texture2DMask.texture != null ;
            }
        }
        /// <summary>
        /// Tells if it should compute Texture2D masking
        /// </summary>
        public bool ShoukdComputeTexture2DMasking
        {
            get
            {
                return UsesTexture2DMasking
                    && ((densityInjection.enable && densityInjection.useTexture2DMask)
                        || (scatteringInjection.enable && scatteringInjection.useTexture2DMask)
                        || (lightInjection.injectionParameters.enable && lightInjection.injectionParameters.useTexture2DMask)
                        || (tintInjection.injectionParameters.enable && tintInjection.injectionParameters.useTexture2DMask));
            }
        }

        /// <summary>
        /// Tells if it should use Texture3D masking
        /// </summary>
        public bool UsesTexture3DMasking
        {
            get
            {
                return texture3DMask.enable && texture3DMask.texture != null;
            }
        }
        /// <summary>
        /// Tells if it should compute Texture3D masking
        /// </summary>
        public bool ShouldComputeTexture3DMasking
        {
            get
            {
                return UsesTexture3DMasking
                    && ((densityInjection.enable && densityInjection.useTexture3DMask)
                        || (scatteringInjection.enable && scatteringInjection.useTexture3DMask)
                        || (lightInjection.injectionParameters.enable && lightInjection.injectionParameters.useTexture3DMask)
                        || (tintInjection.injectionParameters.enable && tintInjection.injectionParameters.useTexture3DMask));
            }
        }

        /// <summary>
        /// Tells if it should compute Noise
        /// </summary>
        public bool ShouldComputeNoise
        {
            get
            {
                return noiseMask.enable
                    && ((densityInjection.enable && densityInjection.useNoiseMask)
                        || (scatteringInjection.enable && scatteringInjection.useNoiseMask)
                        || (lightInjection.injectionParameters.enable && lightInjection.injectionParameters.useNoiseMask)
                        || (tintInjection.injectionParameters.enable && tintInjection.injectionParameters.useNoiseMask));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the volume is being uninitialized
        /// </summary>
        public event Action<AuraVolume> OnUninitialize;
        #endregion

        #region Monobehaviour functions
        private void OnEnable()
        {
            if (!Aura.IsCompatible)
            {
                enabled = false;
                return;
            }

            Initialize();
        }

        private void Update()
        {
            if (useAsLightProbesProxyVolume != _previousUseAsLightProbesProxyVolume ||
                UsesTexture2DMasking != _previousTexture2DMaskUsage || texture2DMask.texture != _previousTexture2DMask ||
                UsesTexture3DMasking != _previousTexture3DMaskUsage || texture3DMask.texture != _previousTexture3DMask)
            {
                Reinitialize();
            }
        }

        private void OnDisable()
        {
            if (_isInitialized)
            {
                Uninitialize();
            }
        }

        private void Reset()
        {
            SetDefaultValues(this);
        }
        #endregion

        #region Functions
        /// <summary>
        ///     Initalize the component
        /// </summary>
        private void Initialize()
        {
            AuraCamera.CommonDataManager.VolumesCommonDataManager.RegisterVolume(this);

            Camera.onPreCull += Camera_onPreCull;
            Camera.onPreRender += Camera_onPreRender;

            _volumeData = new VolumeData();

            _previousUseAsLightProbesProxyVolume = useAsLightProbesProxyVolume;
            _previousTexture2DMaskUsage = UsesTexture2DMasking;
            _previousTexture2DMask = texture2DMask.texture;
            _previousTexture3DMaskUsage = UsesTexture3DMasking;
            _previousTexture3DMask = texture3DMask.texture;

            _isInitialized = true;
        }

        /// <summary>
        ///     Uninitialize the component
        /// </summary>
        private void Uninitialize()
        {
            if (OnUninitialize != null)
            {
                OnUninitialize(this);
            }

            Camera.onPreCull -= Camera_onPreCull;
            Camera.onPreRender -= Camera_onPreRender;

            _isInitialized = false;
        }

        /// <summary>
        /// Uninitializes the component then re-initializes it
        /// </summary>
        private void Reinitialize()
        {
            Uninitialize();
            Initialize();
        }

        /// <summary>
        ///     Called when Aura raises OnPreCullEvent
        /// </summary>
        private void Camera_onPreCull(Camera camera)
        {
            if (this == null)
            {
                Camera.onPreCull -= Camera_onPreCull;
                return;
            }

            UpdateBoundingSphere();
        }

        /// <summary>
        ///     Called when any camera is a about to render
        /// </summary>
        private void Camera_onPreRender(Camera camera)
        {
            if (this == null)
            {
                Camera.onPreRender -= Camera_onPreRender;
                return;
            }

            PackData();
        }

        /// <summary>
        ///     Packs the data
        /// </summary>
        private void PackData()
        {
            _volumeData.transform = MatrixFloats.ToMatrixFloats(transform.worldToLocalMatrix);
            _volumeData.shape = (int)volumeShape.shape;
            _volumeData.falloffExponent = volumeShape.fading.falloffExponent;

            switch (volumeShape.shape)
            {
                case VolumeType.Box:
                    {
                        _volumeData.xPositiveFade = volumeShape.fading.xPositiveCubeFade;
                        _volumeData.xNegativeFade = volumeShape.fading.xNegativeCubeFade;
                        _volumeData.yPositiveFade = volumeShape.fading.yPositiveCubeFade;
                        _volumeData.yNegativeFade = volumeShape.fading.yNegativeCubeFade;
                        _volumeData.zPositiveFade = volumeShape.fading.zPositiveCubeFade;
                        _volumeData.zNegativeFade = volumeShape.fading.zNegativeCubeFade;
                    }
                    break;

                case VolumeType.Cone:
                    {
                        _volumeData.xPositiveFade = volumeShape.fading.angularConeFade;
                        _volumeData.zPositiveFade = volumeShape.fading.distanceConeFade;
                    }
                    break;

                case VolumeType.Cylinder:
                    {
                        _volumeData.xPositiveFade = volumeShape.fading.widthCylinderFade;
                        _volumeData.yPositiveFade = volumeShape.fading.yPositiveCylinderFade;
                        _volumeData.yNegativeFade = volumeShape.fading.yNegativeCylinderFade;
                    }
                    break;

                case VolumeType.Sphere:
                    {
                        _volumeData.xPositiveFade = volumeShape.fading.distanceSphereFade;
                    }
                    break;
            }
            _volumeData.useAsLightProbesProxyVolume = useAsLightProbesProxyVolume ? 1 : 0;
            _volumeData.lightProbesMultiplier = lightProbesMultiplier * Mathf.PI;

            if (ShoukdComputeTexture2DMasking)
            {
                Matrix4x4 localMatrix = texture2DMask.transform.Matrix.inverse;
                _volumeData.texture2DMaskData.transform = MatrixFloats.ToMatrixFloats(noiseMask.transform.space == Space.Self ? localMatrix * transform.worldToLocalMatrix : localMatrix);
            }
            _volumeData.texture2DMaskData.index = texture2DMask.textureIndex;

            if (ShouldComputeTexture3DMasking)
            {
                Matrix4x4 localMatrix = texture3DMask.transform.Matrix.inverse;
                _volumeData.texture3DMaskData.transform = MatrixFloats.ToMatrixFloats(noiseMask.transform.space == Space.Self ? localMatrix * transform.worldToLocalMatrix : localMatrix);
            }
            _volumeData.texture3DMaskData.index = texture3DMask.textureIndex;

            _volumeData.noiseData.enable = ShouldComputeNoise ? 1 : 0;
            if (ShouldComputeNoise)
            {
                Matrix4x4 localMatrix = noiseMask.transform.Matrix.inverse;
                _volumeData.noiseData.transform = MatrixFloats.ToMatrixFloats(noiseMask.transform.space == Space.Self ? localMatrix * transform.worldToLocalMatrix : localMatrix);
                _volumeData.noiseData.speed = noiseMask.speed;
            }

            _volumeData.injectDensity = densityInjection.enable ? 1 : 0;
            _volumeData.densityValue = densityInjection.strength;
            _volumeData.densityNoiseLevelsParameters = densityInjection.useNoiseMask ? (densityInjection.useNoiseMaskLevels ? densityInjection.noiseMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.densityTexture2DMaskLevelsParameters = densityInjection.useTexture2DMask ? (densityInjection.useTexture2DMaskLevels ? densityInjection.texture2DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.densityTexture3DMaskLevelsParameters = densityInjection.useTexture3DMask ? (densityInjection.useTexture3DMaskLevels ? densityInjection.texture3DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;

            _volumeData.injectScattering = scatteringInjection.enable ? 1 : 0;
            _volumeData.scatteringValue = scatteringInjection.strength;
            _volumeData.scatteringNoiseLevelsParameters = scatteringInjection.useNoiseMask ? (scatteringInjection.useNoiseMaskLevels ? scatteringInjection.noiseMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.scatteringTexture2DMaskLevelsParameters = scatteringInjection.useTexture2DMask ? (scatteringInjection.useTexture2DMaskLevels ? scatteringInjection.texture2DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.scatteringTexture3DMaskLevelsParameters = scatteringInjection.useTexture3DMask ? (scatteringInjection.useTexture3DMaskLevels ? scatteringInjection.texture3DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;

            _volumeData.injectColor = lightInjection.injectionParameters.enable ? 1 : 0;
            _volumeData.colorValue = (Vector4)(lightInjection.color * lightInjection.injectionParameters.strength);
            _volumeData.colorNoiseLevelsParameters = lightInjection.injectionParameters.useNoiseMask ? (lightInjection.injectionParameters.useNoiseMaskLevels ? lightInjection.injectionParameters.noiseMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.colorTexture2DMaskLevelsParameters = lightInjection.injectionParameters.useTexture2DMask ? (lightInjection.injectionParameters.useTexture2DMaskLevels ? lightInjection.injectionParameters.texture2DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.colorTexture3DMaskLevelsParameters = lightInjection.injectionParameters.useTexture3DMask ? (lightInjection.injectionParameters.useTexture3DMaskLevels ? lightInjection.injectionParameters.texture3DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;

            _volumeData.injectTint = tintInjection.injectionParameters.enable ? 1 : 0;
            _volumeData.tintColor = (Vector4)(tintInjection.color * tintInjection.injectionParameters.strength);
            _volumeData.tintNoiseLevelsParameters = tintInjection.injectionParameters.useNoiseMask ? (tintInjection.injectionParameters.useNoiseMaskLevels ? tintInjection.injectionParameters.noiseMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.tintTexture2DMaskLevelsParameters = tintInjection.injectionParameters.useTexture2DMask ? (tintInjection.injectionParameters.useTexture2DMaskLevels ? tintInjection.injectionParameters.texture2DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.tintTexture3DMaskLevelsParameters = tintInjection.injectionParameters.useTexture3DMask ? (tintInjection.injectionParameters.useTexture3DMaskLevels ? tintInjection.injectionParameters.texture3DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;

            _volumeData.injectAmbient = ambientInjection.enable ? 1 : 0;
            _volumeData.ambientLightingValue = ambientInjection.strength;
            _volumeData.ambientNoiseLevelsParameters = ambientInjection.useNoiseMask ? (ambientInjection.useNoiseMaskLevels ? ambientInjection.noiseMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.ambientTexture2DMaskLevelsParameters = ambientInjection.useTexture2DMask ? (ambientInjection.useTexture2DMaskLevels ? ambientInjection.texture2DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.ambientTexture3DMaskLevelsParameters = ambientInjection.useTexture3DMask ? (ambientInjection.useTexture3DMaskLevels ? ambientInjection.texture3DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;

            _volumeData.injectBoost = boostInjection.enable ? 1 : 0;
            _volumeData.boostValue = boostInjection.strength;
            _volumeData.boostNoiseLevelsParameters = boostInjection.useNoiseMask ? (boostInjection.useNoiseMaskLevels ? boostInjection.noiseMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.boostTexture2DMaskLevelsParameters = boostInjection.useTexture2DMask ? (boostInjection.useTexture2DMaskLevels ? boostInjection.texture2DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
            _volumeData.boostTexture3DMaskLevelsParameters = boostInjection.useTexture3DMask ? (boostInjection.useTexture3DMaskLevels ? boostInjection.texture3DMaskLevelParameters.Data : LevelsParameters.Default.Data) : LevelsParameters.One.Data;
        }

        /// <summary>
        ///     Retrieves the packed data of the volume
        /// </summary>
        /// <returns>The packed data of the volume</returns>
        public VolumeData GetData()
        {
            return _volumeData;
        }

        /// <summary>
        ///     Computes the sphere radius englobing the scaled normalized cube
        /// </summary>
        /// <returns></returns>
        private float GetRadiusFromScale()
        {
            return transform.localScale.magnitude;
        }

        /// <summary>
        ///     Updates the bounding sphere data
        /// </summary>
        private void UpdateBoundingSphere()
        {
            Vector3 position = transform.position;
            float radius = float.MaxValue;

            switch (volumeShape.shape)
            {
                case VolumeType.Box:
                    {
                        radius = GetRadiusFromScale() * 0.5f;
                    }
                    break;

                case VolumeType.Sphere:
                    {
                        radius = Mathf.Max(Mathf.Abs(transform.localScale.x), Mathf.Max(Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z))) * 0.5f;
                    }
                    break;

                case VolumeType.Cylinder:
                    {
                        radius = GetRadiusFromScale() * 0.5f;
                    }
                    break;

                case VolumeType.Cone: // Could be better
                    {
                        position += transform.forward * transform.localScale.z * 0.5f;
                        radius = GetRadiusFromScale() * 0.5f;
                    }
                    break;
            }

            UpdateBoundingSphere(position, radius);
        }

        /// <summary>
        /// Set default values
        /// </summary>
        /// <param name="auraVolume"></param>
        private static void SetDefaultValues(AuraVolume auraVolume)
        {
            auraVolume.volumeShape.fading.falloffExponent = 3.0f;
            auraVolume.volumeShape.fading.xPositiveCubeFade = 0.25f;
            auraVolume.volumeShape.fading.xNegativeCubeFade = 0.25f;
            auraVolume.volumeShape.fading.yPositiveCubeFade = 0.25f;
            auraVolume.volumeShape.fading.yNegativeCubeFade = 0.25f;
            auraVolume.volumeShape.fading.zPositiveCubeFade = 0.25f;
            auraVolume.volumeShape.fading.zNegativeCubeFade = 0.25f;
            auraVolume.volumeShape.fading.angularConeFade = 0.5f;
            auraVolume.volumeShape.fading.distanceConeFade = 0.5f;
            auraVolume.volumeShape.fading.widthCylinderFade = 0.5f;
            auraVolume.volumeShape.fading.yNegativeCylinderFade = 0.25f;
            auraVolume.volumeShape.fading.yPositiveCylinderFade = 0.25f;
            auraVolume.volumeShape.fading.distanceSphereFade = 0.5f;
            auraVolume.texture2DMask.SetDefaultValues();
            auraVolume.texture3DMask.SetDefaultValues();
            auraVolume.noiseMask.speed = 0.125f;
            auraVolume.noiseMask.transform.scale = Vector3.one * 5.0f;
            auraVolume.densityInjection.useNoiseMask = true;
            auraVolume.densityInjection.useNoiseMaskLevels = true;
            auraVolume.densityInjection.noiseMaskLevelParameters.SetDefaultValues();
            auraVolume.densityInjection.noiseMaskLevelParameters.contrast = 5.0f;
            auraVolume.densityInjection.useTexture2DMask = true;
            auraVolume.densityInjection.texture2DMaskLevelParameters.SetDefaultValues();
            auraVolume.densityInjection.useTexture3DMask = true;
            auraVolume.densityInjection.texture3DMaskLevelParameters.SetDefaultValues();
            auraVolume.densityInjection.enable = true; // To have something visible when a volume is added
            auraVolume.densityInjection.strength = 5.0f; // To have something visible when a volume is added
            auraVolume.scatteringInjection.useNoiseMask = true;
            auraVolume.scatteringInjection.useNoiseMaskLevels = true;
            auraVolume.scatteringInjection.noiseMaskLevelParameters.SetDefaultValues();
            auraVolume.scatteringInjection.noiseMaskLevelParameters.contrast = 3.0f;
            auraVolume.scatteringInjection.noiseMaskLevelParameters.outputLowValue = -1.0f;
            auraVolume.scatteringInjection.useTexture2DMask = true;
            auraVolume.scatteringInjection.texture2DMaskLevelParameters.SetDefaultValues();
            auraVolume.scatteringInjection.texture2DMaskLevelParameters.outputLowValue = -1.0f;
            auraVolume.scatteringInjection.useTexture3DMask = true;
            auraVolume.scatteringInjection.texture3DMaskLevelParameters.SetDefaultValues();
            auraVolume.scatteringInjection.texture3DMaskLevelParameters.outputLowValue = -1.0f;
            auraVolume.scatteringInjection.strength = 0.25f;
            auraVolume.lightInjection.injectionParameters.useNoiseMask = true;
            auraVolume.lightInjection.injectionParameters.useNoiseMaskLevels = true;
            auraVolume.lightInjection.injectionParameters.noiseMaskLevelParameters.SetDefaultValues();
            auraVolume.lightInjection.injectionParameters.noiseMaskLevelParameters.contrast = 5.0f;
            auraVolume.lightInjection.injectionParameters.useTexture2DMask = true;
            auraVolume.lightInjection.injectionParameters.texture2DMaskLevelParameters.SetDefaultValues();
            auraVolume.lightInjection.injectionParameters.useTexture3DMask = true;
            auraVolume.lightInjection.injectionParameters.texture3DMaskLevelParameters.SetDefaultValues();
            auraVolume.lightInjection.injectionParameters.strength = 1;
            auraVolume.lightInjection.color = Color.white;
            auraVolume.tintInjection.injectionParameters.useNoiseMask = true;
            auraVolume.tintInjection.injectionParameters.useNoiseMaskLevels = true;
            auraVolume.tintInjection.injectionParameters.noiseMaskLevelParameters.SetDefaultValues();
            auraVolume.tintInjection.injectionParameters.noiseMaskLevelParameters.contrast = 5.0f;
            auraVolume.tintInjection.injectionParameters.useTexture2DMask = true;
            auraVolume.tintInjection.injectionParameters.texture2DMaskLevelParameters.SetDefaultValues();
            auraVolume.tintInjection.injectionParameters.useTexture3DMask = true;
            auraVolume.tintInjection.injectionParameters.texture3DMaskLevelParameters.SetDefaultValues();
            auraVolume.tintInjection.injectionParameters.strength = 1;
            auraVolume.tintInjection.color = Color.white;
            auraVolume.ambientInjection.strength = 1;
            auraVolume.ambientInjection.useNoiseMask = true;
            auraVolume.ambientInjection.useNoiseMaskLevels = true;
            auraVolume.ambientInjection.noiseMaskLevelParameters.SetDefaultValues();
            auraVolume.ambientInjection.noiseMaskLevelParameters.contrast = 5.0f;
            auraVolume.ambientInjection.useTexture2DMask = true;
            auraVolume.ambientInjection.texture2DMaskLevelParameters.SetDefaultValues();
            auraVolume.ambientInjection.useTexture3DMask = true;
            auraVolume.ambientInjection.texture3DMaskLevelParameters.SetDefaultValues();
            auraVolume.boostInjection.strength = 1;
            auraVolume.boostInjection.useNoiseMask = true;
            auraVolume.boostInjection.useNoiseMaskLevels = true;
            auraVolume.boostInjection.noiseMaskLevelParameters.SetDefaultValues();
            auraVolume.boostInjection.noiseMaskLevelParameters.contrast = 5.0f;
            auraVolume.boostInjection.useTexture2DMask = true;
            auraVolume.boostInjection.texture2DMaskLevelParameters.SetDefaultValues();
            auraVolume.boostInjection.useTexture3DMask = true;
            auraVolume.boostInjection.texture3DMaskLevelParameters.SetDefaultValues();
        }
        #endregion

        #region GameObject constructor
        /// <summary>
        /// Generic method for crating a GameObject with a AuraVolume component assigned
        /// </summary>
        /// <param name="name">Name of the created GameObject</param>
        /// <param name="shape">Desired volume shape</param>
        /// <returns>The created AuraVolume gameObject</returns>
        public static GameObject CreateGameObject(string name, VolumeType shape)
        {
            GameObject newGameObject = new GameObject(name);
            newGameObject.transform.localScale = Vector3.one * 3.0f;
            AuraVolume auraVolume = newGameObject.AddComponent<AuraVolume>();
            auraVolume.volumeShape.shape = shape;
            SetDefaultValues(auraVolume);

            return newGameObject;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Generic method for crating a GameObject with a AuraVolume component assigned
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        /// <param name="name">Name of the created GameObject</param>
        /// <param name="shape">Desired volume shape</param>
        /// <param name="selectAndFocus">Selects and focus on the newly created volume</param>
        /// <returns>The created AuraVolume gameObject</returns>
        private static GameObject CreateGameObject(MenuCommand menuCommand, string name, VolumeType shape)
        {
            GameObject newGameObject = CreateGameObject(name, shape);

            float offset = 0;
            switch (shape)
            {
                case VolumeType.Box:
                case VolumeType.Cone:
                case VolumeType.Cylinder:
                case VolumeType.Sphere:
                    {
                        offset = newGameObject.transform.lossyScale.y * 0.5f;
                    }
                    break;
            }

            if (SceneView.lastActiveSceneView != null)
            {
                newGameObject.transform.position = SceneView.lastActiveSceneView.camera.GetSpawnPosition() + Vector3.up * offset;
            }
            
            GameObjectUtility.SetParentAndAlign(newGameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(newGameObject, "Create " + newGameObject.name);
            Selection.activeObject = newGameObject;
            SceneView.FrameLastActiveSceneViewWithLock();

            return newGameObject;
        }

        /// <summary>
        /// Creates a "global" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Volume/Global", false, 4)]
        private static void CreateGlobalGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Global Volume", VolumeType.Global);
        }

        /// <summary>
        /// Creates a "Layer" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Volume/Layer", false, 5)]
        private static void CreateLayerGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Layer Volume", VolumeType.Layer);
        }

        /// <summary>
        /// Creates a "box" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Volume/Box", false, 6)]
        private static void CreateBoxGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Box Volume", VolumeType.Box);
        }

        /// <summary>
        /// Creates a "sphere" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Volume/Sphere", false, 7)]
        private static void CreateSphereGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Sphere Volume", VolumeType.Sphere);
        }

        /// <summary>
        /// Creates a "cylinder" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Volume/Cylinder", false, 8)]
        private static void CreateCylinderGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Cylinder Volume", VolumeType.Cylinder);
        }

        /// <summary>
        /// Creates a "cone" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Volume/Cone", false, 9)]
        private static void CreateConeGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Cone Volume", VolumeType.Cone);
        }
#endif
        #endregion
    }

    #region Gizmo
#if UNITY_EDITOR 
    /// <summary>
    /// Allows to draw custom gizmos for AuraVolume objects
    /// </summary>
    public class AuraVolumeGizmoDrawer
    {
        /// <summary>
        /// Draws a custom gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="gizmoType">Gizmo state</param>
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NonSelected | GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
        static void DrawGizmoForAuraVolume(AuraVolume component, GizmoType gizmoType)
        {
            if(!AuraEditorPrefs.DisplayGizmosOnVolumes)
            {
                return;
            }

            bool isFaded = (int)gizmoType == (int)GizmoType.NonSelected || (int)gizmoType == (int)GizmoType.NotInSelectionHierarchy || (int)gizmoType == (int)GizmoType.NonSelected + (int)GizmoType.NotInSelectionHierarchy;

            if(isFaded && !AuraEditorPrefs.DisplayGizmosWhenUnselected || !isFaded && !AuraEditorPrefs.DisplayGizmosWhenSelected)
            {
                return;
            }
            
            float opacity = isFaded ? 0.5f : 1.0f;

            DrawGizmo(component, opacity);
        }

        /// <summary>
        /// Draws the gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="opacity">The gizmo opacity</param>
        private static void DrawGizmo(AuraVolume component, float opacity)
        {
            switch (component.volumeShape.shape)
            {
                case VolumeType.Global:
                    {
                        DrawGlobal(component, opacity);
                    }
                    break;

                case VolumeType.Layer:
                    {
                        DrawLayer(component, opacity);
                    }
                    break;

                case VolumeType.Box:
                    {
                        DrawBox(component, opacity);
                    }
                    break;

                case VolumeType.Sphere:
                    {
                        DrawSphere(component, opacity);
                    }
                    break;

                case VolumeType.Cylinder:
                    {
                        DrawCylinder(component, opacity);
                    }
                    break;

                case VolumeType.Cone:
                    {
                        DrawCone(component, opacity);
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws a "Global" gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="alpha">The base opacity</param>
        private static void DrawGlobal(AuraVolume component, float alpha)
        {
            Color color = CustomGizmo.color;
            color.a = CustomGizmo.color.a * alpha;
            const int circlesAmount = 10;
            const float maxWidth = 5.0f;
            for (int i = 1; i <= circlesAmount; ++i)
            {
                float ratio = (float)i / (float)circlesAmount;
                float curvedRatio = Mathf.Pow(ratio, 2.5f);
                CustomGizmo.DrawCircle(component.transform.localToWorldMatrix, Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * maxWidth * curvedRatio), color, CustomGizmo.pixelWidth);
            }
        }

        /// <summary>
        /// Draws a "Layer" gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="alpha">The base opacity</param>
        private static void DrawLayer(AuraVolume component, float alpha)
        {
            Color color = CustomGizmo.color;
            color.a *= alpha;

            int count = 15;
            for (int i = 0; i < count; ++i)
            {
                float ratio = 1.0f - ((float)i / (float)count);
                float scaleFactor = 1.0f; // Mathf.Lerp(0.5f, 10.0f, ratio);
                //ratio = 1 - Mathf.Pow(ratio, component.volumeShape.fading.falloffExponent);
                CustomGizmo.DrawSquare(component.transform.localToWorldMatrix, Matrix4x4.TRS(Vector3.up * ratio, Quaternion.identity, Vector3.one * scaleFactor), color, CustomGizmo.pixelWidth);
            }

        }

        /// <summary>
        /// Draws a "Box" gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="alpha">The base opacity</param>
        private static void DrawBox(AuraVolume component, float alpha)
        {
            float thickness = CustomGizmo.pixelWidth;
            Color color = CustomGizmo.color;
            color.a *= alpha;
            CustomGizmo.DrawCube(component.transform.localToWorldMatrix, color, thickness);

            float xPos = (1.0f - component.volumeShape.fading.xPositiveCubeFade) * 2 - 1;
            float xNeg = component.volumeShape.fading.xNegativeCubeFade * 2 - 1;
            float yPos = (1.0f - component.volumeShape.fading.yPositiveCubeFade) * 2 - 1;
            float yNeg = component.volumeShape.fading.yNegativeCubeFade * 2 - 1;
            float zPos = (1.0f - component.volumeShape.fading.zPositiveCubeFade) * 2 - 1;
            float zNeg = component.volumeShape.fading.zNegativeCubeFade * 2 - 1;

            Vector3 customPointA = new Vector3(xNeg, yPos, zNeg) * 0.5f;
            Vector3 customPointB = new Vector3(xPos, yPos, zNeg) * 0.5f;
            Vector3 customPointC = new Vector3(xPos, yNeg, zNeg) * 0.5f;
            Vector3 customPointD = new Vector3(xNeg, yNeg, zNeg) * 0.5f;
            Vector3 customPointE = new Vector3(xNeg, yPos, zPos) * 0.5f;
            Vector3 customPointF = new Vector3(xPos, yPos, zPos) * 0.5f;
            Vector3 customPointG = new Vector3(xPos, yNeg, zPos) * 0.5f;
            Vector3 customPointH = new Vector3(xNeg, yNeg, zPos) * 0.5f;
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerA, new Vector3(xNeg, yPos, zNeg) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerB, new Vector3(xPos, yPos, zNeg) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerC, new Vector3(xPos, yNeg, zNeg) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerD, new Vector3(xNeg, yNeg, zNeg) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerE, new Vector3(xNeg, yPos, zPos) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerF, new Vector3(xPos, yPos, zPos) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerG, new Vector3(xPos, yNeg, zPos) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(CustomGizmo.cubeCornerH, new Vector3(xNeg, yNeg, zPos) * 0.5f, component.transform.localToWorldMatrix, color, thickness);

            CustomGizmo.DrawLineSegment(customPointA, customPointB, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointB, customPointC, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointC, customPointD, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointD, customPointA, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointE, customPointF, component.transform.localToWorldMatrix, color, thickness);

            CustomGizmo.DrawLineSegment(customPointF, customPointG, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointG, customPointH, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointH, customPointE, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointE, customPointF, component.transform.localToWorldMatrix, color, thickness);

            CustomGizmo.DrawLineSegment(customPointA, customPointE, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointB, customPointF, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointC, customPointG, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(customPointD, customPointH, component.transform.localToWorldMatrix, color, thickness);
        }

        /// <summary>
        /// Draws a "Sphere" gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="alpha">The base opacity</param>
        private static void DrawSphere(AuraVolume component, float alpha)
        {
            float thickness = CustomGizmo.pixelWidth;
            Color color = CustomGizmo.color;
            color.a *= alpha;
            CustomGizmo.DrawSphere(component.transform.localToWorldMatrix, color, CustomGizmo.pixelWidth);

            float x = 1.0f - component.volumeShape.fading.distanceSphereFade;
            CustomGizmo.DrawLineSegment(Vector3.up * 0.5f, Vector3.up * x * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(Vector3.down * 0.5f, Vector3.down * x * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(Vector3.left * 0.5f, Vector3.left * x * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(Vector3.right * 0.5f, Vector3.right * x * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(Vector3.back * 0.5f, Vector3.back * x * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(Vector3.forward * 0.5f, Vector3.forward * x * 0.5f, component.transform.localToWorldMatrix, color, thickness);

            CustomGizmo.DrawSphere(component.transform.localToWorldMatrix, Vector3.zero, Quaternion.identity, Vector3.one * x, color, thickness);
        }

        /// <summary>
        /// Draws a "Cylinder" gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="alpha">The base opacity</param>
        private static void DrawCylinder(AuraVolume component, float alpha)
        {
            float thickness = CustomGizmo.pixelWidth;
            Color color = CustomGizmo.color;
            color.a *= alpha;
            CustomGizmo.DrawCylinder(component.transform.localToWorldMatrix, color, thickness);

            float x = 1.0f - component.volumeShape.fading.widthCylinderFade;
            float yPos = (1.0f - component.volumeShape.fading.yPositiveCylinderFade) * 2 - 1;
            float yNeg = component.volumeShape.fading.yNegativeCylinderFade * 2 - 1;
            CustomGizmo.DrawLineSegment(new Vector3(0.5f, 0.5f, 0), new Vector3(x, yPos, 0) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(-0.5f, 0.5f, 0), new Vector3(-x, yPos, 0) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.5f, -0.5f, 0), new Vector3(x, yNeg, 0) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(-0.5f, -0.5f, 0), new Vector3(-x, yNeg, 0) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.0f, 0.5f, 0.5f), new Vector3(0, yPos, x) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.0f, 0.5f, -0.5f), new Vector3(0, yPos, -x) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.0f, -0.5f, 0.5f), new Vector3(0, yNeg, x) * 0.5f, component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.0f, -0.5f, -0.5f), new Vector3(0, yNeg, -x) * 0.5f, component.transform.localToWorldMatrix, color, thickness);

            CustomGizmo.DrawCylinder(component.transform.localToWorldMatrix, (Vector3.up * ((1.0f - component.volumeShape.fading.yPositiveCylinderFade) + component.volumeShape.fading.yNegativeCylinderFade) * 0.5f) - Vector3.up * 0.5f, Quaternion.identity, new Vector3(x, 1.0f - component.volumeShape.fading.yPositiveCylinderFade - component.volumeShape.fading.yNegativeCylinderFade, x), color, thickness);
        }

        /// <summary>
        /// Draws a "Cone" gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="alpha">The base opacity</param>
        private static void DrawCone(AuraVolume component, float alpha)
        {
            float thickness = CustomGizmo.pixelWidth;
            Color color = CustomGizmo.color;
            color.a *= alpha;
            CustomGizmo.DrawCone(component.transform.localToWorldMatrix, color, thickness);

            float z = 1.0f - component.volumeShape.fading.distanceConeFade;
            float xy = Mathf.Lerp(0, 1.0f - component.volumeShape.fading.angularConeFade, z);
            CustomGizmo.DrawLineSegment(new Vector3(0.0f, 0.5f, 1), new Vector3(0, xy * 0.5f, z), component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.0f, -0.5f, 1), new Vector3(0, -xy * 0.5f, z), component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(0.5f, 0.0f, 1), new Vector3(xy * 0.5f, 0.0f, z), component.transform.localToWorldMatrix, color, thickness);
            CustomGizmo.DrawLineSegment(new Vector3(-0.5f, 0.0f, 1), new Vector3(-xy * 0.5f, 0.0f, z), component.transform.localToWorldMatrix, color, thickness);

            CustomGizmo.DrawCone(component.transform.localToWorldMatrix, Vector3.zero, Quaternion.identity, new Vector3(xy, xy, z), color, thickness);
        }
    }
#endif
    #endregion
}
