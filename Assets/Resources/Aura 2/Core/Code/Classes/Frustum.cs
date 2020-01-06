
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
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace Aura2API
{
    /// <summary>
    /// Ranged camera frustum inside which the volumetric data will be computed
    /// </summary>
    public class Frustum
    {
        #region Private Members
        /// <summary>
        /// Settings of the frustum
        /// </summary>
        private FrustumSettings _frustumSettings;
        /// <summary>
        /// Settings of the frustum
        /// </summary>
        private FrustumSettingsToId _frustumSettingsToId;
        /// <summary>
        /// Compute Shader in charge of computing the maximum depth for occlusion culling
        /// </summary>
        private ComputeShader _computeMaximumDepthComputeShader;
        /// <summary>
        /// Shader in charge of filtering and formatting the maximum depth
        /// </summary>
        private Shader _processOcclusionMapShader;
        /// <summary>
        /// Material used for filtering and formatting the maximum depth
        /// </summary>
        private Material _processOcclusionMapMaterial;
        /// <summary>
        /// Compute Shader in charge of ruling out the invisible cells of the volumetric grid
        /// </summary>
        public ComputeShader _computeVisibleCellsComputeShader;
        /// <summary>
        /// Compute Shader in charge of computing the data contribution inside the frustum
        /// </summary>
        private ComputeShader _computeDataComputeShader;
        /// <summary>
        /// The compute shaders thead size in X
        /// </summary>
        private uint _threadSizeX;
        /// <summary>
        /// The compute shaders thead size in Y
        /// </summary>
        private uint _threadSizeY;
        /// <summary>
        /// The compute shaders thead size in Z
        /// </summary>
        private uint _threadSizeZ;
        /// <summary>
        /// Compute buffers used for indirect dispatching
        /// </summary>
        private ComputeBuffer[] _dispatchBuffers;
        /// <summary>
        /// Compute Shader in charge of accumulating the data
        /// </summary>
        private ComputeShader _computeAccumulationComputeShader;
        /// <summary>
        /// The far clip of the volume
        /// </summary>
        private float _farClip;
        /// <summary>
        /// Contains near and far clips of the camera
        /// </summary>
        private Vector4 _cameraRanges;
        /// <summary>
        /// Contains near and far clips of the volume
        /// </summary>
        private Vector4 _frustumRanges;
        /// <summary>
        /// Data for depthmap linearization
        /// </summary>
        private Vector4 _zParameters;
        /// <summary>
        /// A vector containing the resolution of the buffers
        /// </summary>
        private Vector4 _bufferResolutionVector;
        /// <summary>
        /// A vector containing the size of the texels of the Texture3D buffers
        /// </summary>
        private Vector4 _bufferTexelSizeVector;
        /// <summary>
        /// The reference camera to get the clip space from
        /// </summary>
        private Camera _cameraComponent;
        /// <summary>
        /// The aura component this frustum in called from
        /// </summary>
        private AuraCamera _auraComponent;
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
        /// <summary>
        /// Previous frame's world to clip matrix for reprojection
        /// </summary>
        private Matrix4x4 _previousWorldToClipMatrix;
        /// <summary>
        /// Pre-allocated floats array for previous frame's world to clip matrix
        /// </summary>
        private float[] _previousWorldToClipMatrixFloats;
        /// <summary>
        /// Previous frame's secondary world to clip matrix for reprojection
        /// </summary>
        private Matrix4x4 _previousSecondaryWorldToClipMatrix;
        /// <summary>
        /// Pre-allocated floats array for previous frame's secondary world to clip matrix
        /// </summary>
        private float[] _previousSecondaryWorldToClipMatrixFloats;
        /// <summary>
        /// The buffers used to compute the volumetric lighting
        /// </summary>
        private TextureBuffers _workingBuffers;
        /// <summary>
        /// The volumetric grid resolution
        /// </summary>
        private Vector3Int _frustumGridResolution;
        /// <summary>
        /// The previous occlusion culling activation state
        /// </summary>
        private bool _previousOcclusionCullingState;
        /// <summary>
        /// The light probes buffers resolution
        /// </summary>
        private Vector4 _lightProbesCoefficientsTextureSizeVector;
        /// <summary>
        /// The light probes Texture3D buffer half texel size
        /// </summary>
        private Vector4 _lightProbesCoefficientsTextureHalfTexelSize;
        /// <summary>
        /// The compute shader used to render the light probes lighting into the Texture3D buffer
        /// </summary>
        private ComputeShader _renderLightProbesTextureComputeShader;
        /// <summary>
        /// The previous light probes activation state
        /// </summary>
        private bool _previousLightProbesState;
        /// <summary>
        /// The compute shader used to apply the denoising filter
        /// </summary>
        private ComputeShader _applyMedianFilterComputeShader;
        /// <summary>
        /// The compute shader used to apply the blur filter
        /// </summary>
        private ComputeShader _applyBlurFilterComputeShader;
        /// <summary>
        /// Frustum's corners positions
        /// </summary>
        private float[] _frustumCornersWorldPositionArray;
        /// <summary>
        /// Secondary frustum's corners positions
        /// </summary>
        private float[] _secondaryFrustumCornersWorldPositionArray;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Frustum(FrustumSettings frustumSettings, Camera camera, AuraCamera auraComponent)
        {
            _frustumSettings = frustumSettings;
            _frustumSettings.OnFrustumQualityChanged += _frustumSettings_OnFrustumQualityChanged;

            _cameraComponent = camera;
            _auraComponent = auraComponent;
            _volumesManager = new VolumesManager(_cameraComponent, _frustumSettings);
            _spotLightsManager = new SpotLightsManager(_cameraComponent, _frustumSettings);
            _pointLightsManager = new PointLightsManager(_cameraComponent, _frustumSettings);
            _frustumSettingsToId = new FrustumSettingsToId(_frustumSettings, _auraComponent, _volumesManager, _spotLightsManager, _pointLightsManager);

            InitializeResources();

            InitializeComputeBuffers();

            SetFrustumGridResolution(_frustumSettings.QualitySettings.frustumGridResolution);

            _previousOcclusionCullingState = _frustumSettingsToId.HasFlags(FrustumParameters.EnableOcclusionCulling);
            _previousLightProbesState = _frustumSettingsToId.HasFlags(FrustumParameters.EnableLightProbes);

            _cameraRanges = new Vector4();
            _frustumRanges = new Vector4();
            _zParameters = new Vector4();
            _frustumCornersWorldPositionArray = new float[32];
            _secondaryFrustumCornersWorldPositionArray = new float[32];
            _previousWorldToClipMatrix = new Matrix4x4();
            _previousWorldToClipMatrixFloats = new float[16];
            _previousSecondaryWorldToClipMatrix = new Matrix4x4();
            _previousSecondaryWorldToClipMatrixFloats = new float[16];
        }
        #endregion

        #region Properties
        /// <summary>
        /// The volumetric grid resolution
        /// </summary>
        private Vector3Int FrustumGridResolution
        {
            get
            {
                //#if UNITY_EDITOR && AURA_IN_SCENEVIEW
                //if (_cameraComponent.IsSceneViewCamera())
                //{
                //    return SceneViewVisualization.FrustumGridResolution;
                //}
                //#endif

                return _frustumGridResolution;
            }
        }

        /// <summary>
        /// A vector containing the size of the texels of the Texture3D buffers
        /// </summary>
        private Vector4 BufferTexelSizeVector
        {
            get
            {
                //#if UNITY_EDITOR && AURA_IN_SCENEVIEW
                //if (_cameraComponent.IsSceneViewCamera())
                //{
                //    return SceneViewVisualization.BufferTexelSizeVector;
                //}
                //#endif

                return _bufferTexelSizeVector;
            }
        }

        /// <summary>
        /// A vector containing the resolution of the buffers
        /// </summary>
        private Vector4 BufferResolutionVector
        {
            get
            {
                //#if UNITY_EDITOR && AURA_IN_SCENEVIEW
                //if (_cameraComponent.IsSceneViewCamera())
                //{
                //    return SceneViewVisualization.BufferResolutionVector;
                //}
                //#endif

                return _bufferResolutionVector;
            }
        }

        /// <summary>
        /// The buffers used to compute the volumetric lighting
        /// </summary>
        private TextureBuffers WorkingBuffers
        {
            get
            {
                //#if UNITY_EDITOR && AURA_IN_SCENEVIEW
                //if (_cameraComponent.IsSceneViewCamera())
                //{
                //    return SceneViewVisualization.TextureBuffers;
                //}
                //#endif

                if (_workingBuffers == null)
                {
                    _workingBuffers = new TextureBuffers();
                }

                return _workingBuffers;
            }
        }

        /// <summary>
        /// Accessor to the volumetric data buffer (containing the lighting (RGB) and the density (A))
        /// </summary>
        private SwappableRenderTexture DataVolumeTexture
        {
            get
            {
                return WorkingBuffers.DataVolumeTexture;
            }
        }

        /// <summary>
        /// Accessor to the volumetric accumulated buffer (containing the accumulated lighting)
        /// </summary>
        private RenderTexture FogVolumeTexture
        {
            get
            {
                return WorkingBuffers.FogVolumeTexture;
            }
        }

        /// <summary>
        /// Accessor to the buffer containing the maximum depth
        /// </summary>
        private SwappableRenderTexture OcclusionTexture
        {
            get
            {
                return WorkingBuffers.OcclusionTexture;
            }
        }

        /// <summary>
        /// Accessor to the buffer containing the maximum depth
        /// </summary>
        private SwappableRenderTexture SliceTexture
        {
            get
            {
                return WorkingBuffers.SliceTexture;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Called when the frustum quality settings are changed
        /// </summary>
        private void _frustumSettings_OnFrustumQualityChanged()
        {
            SetFrustumGridResolution(_frustumSettings.QualitySettings.frustumGridResolution);
        }

        /// <summary>
        /// Initialize the needed resources
        /// </summary>
        private void InitializeResources()
        {
            _computeMaximumDepthComputeShader = Aura.ResourcesCollection.computeMaximumDepthComputeShader;
            _computeVisibleCellsComputeShader = Aura.ResourcesCollection.computeVisibleCellsComputeShader;
            _processOcclusionMapShader = Aura.ResourcesCollection.processOcclusionMapShader;
            _computeDataComputeShader = Aura.ResourcesCollection.computeDataComputeShader;
            _computeAccumulationComputeShader = Aura.ResourcesCollection.computeAccumulationComputeShader;
            _renderLightProbesTextureComputeShader = Aura.ResourcesCollection.renderLightProbesTextureComputeShader;
            _applyMedianFilterComputeShader = Aura.ResourcesCollection.applyDenoisingFilterComputeShader;
            _applyBlurFilterComputeShader = Aura.ResourcesCollection.applyBlurFilterComputeShader;
        }

        /// <summary>
        /// Computes the volumetric data
        /// </summary>
        public void ComputeData()
        {
            #region Shaders Keywords
            if (_frustumSettings.QualitySettings.displayVolumetricLightingBuffer)
            {
                Shader.EnableKeyword("AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY");
            }
            else
            {
                Shader.DisableKeyword("AURA_DISPLAY_VOLUMETRIC_LIGHTING_ONLY");
            }

            if (_frustumSettings.QualitySettings.enableDithering)
            {
                Shader.EnableKeyword("AURA_USE_DITHERING");
            }
            else
            {
                Shader.DisableKeyword("AURA_USE_DITHERING");
            }

            if (_frustumSettings.QualitySettings.texture3DFiltering == Texture3DFiltering.Cubic)
            {
                Shader.EnableKeyword("AURA_USE_CUBIC_FILTERING");
            }
            else
            {
                Shader.DisableKeyword("AURA_USE_CUBIC_FILTERING");
            }
            #endregion

            #region Variables
            _computeMaximumDepthComputeShader.SetVector("Aura_BufferResolution", BufferResolutionVector);
            _computeDataComputeShader.SetVector("Aura_BufferResolution", BufferResolutionVector);
            _computeAccumulationComputeShader.SetVector("Aura_BufferResolution", BufferResolutionVector);
            _computeVisibleCellsComputeShader.SetVector("Aura_BufferResolution", BufferResolutionVector);
            _computeMaximumDepthComputeShader.SetVector("Aura_BufferTexelSize", BufferTexelSizeVector);
            _computeMaximumDepthComputeShader.SetVector("Aura_BufferTexelSize", BufferTexelSizeVector);
            _computeVisibleCellsComputeShader.SetVector("Aura_BufferTexelSize", BufferTexelSizeVector);
            _computeDataComputeShader.SetVector("Aura_BufferTexelSize", BufferTexelSizeVector);
            _computeAccumulationComputeShader.SetVector("Aura_BufferTexelSize", BufferTexelSizeVector);
            float depthBiasCoefficient = Mathf.Max(0.001f, _frustumSettings.QualitySettings.depthBiasCoefficient);
            float depthBiasReciproqualCoefficient = 1.0f / depthBiasCoefficient;
            _computeDataComputeShader.SetFloat("Aura_DepthBiasCoefficient", depthBiasCoefficient);
            _computeAccumulationComputeShader.SetFloat("Aura_DepthBiasCoefficient", depthBiasCoefficient);
            _computeVisibleCellsComputeShader.SetFloat("Aura_DepthBiasCoefficient", depthBiasCoefficient);
            _computeDataComputeShader.SetFloat("Aura_DepthBiasReciproqualCoefficient", depthBiasReciproqualCoefficient);
            _computeVisibleCellsComputeShader.SetInt("_frameID", _auraComponent.FrameId);
            _cameraRanges.x = _cameraComponent.nearClipPlane;
            _cameraRanges.y = _cameraComponent.farClipPlane;
            _frustumRanges.x = 0.0000000000000001f /*Set close to 0 but need non-zero for stereo projection matrices*/;
            _frustumRanges.y = Mathf.Min(_cameraComponent.farClipPlane, _frustumSettings.QualitySettings.farClipPlaneDistance);
            _zParameters.x = -1.0f + _cameraComponent.farClipPlane / _cameraComponent.nearClipPlane;
            _zParameters.y = 1.0f;
            _zParameters.z = _zParameters.x / _cameraComponent.farClipPlane;
            _zParameters.w = _zParameters.y / _cameraComponent.farClipPlane;
            Color baseColor = _frustumSettings.BaseSettings.useColor ? (_frustumSettings.BaseSettings.color * _frustumSettings.BaseSettings.colorStrength) : Color.black;
            float baseDensity = _frustumSettings.BaseSettings.useDensity ? _frustumSettings.BaseSettings.density : 0.0f;
            float extinction = _frustumSettings.BaseSettings.useExtinction ? _frustumSettings.BaseSettings.extinction : 1.0f;
            int currentKernelIndex = _frustumSettingsToId.GetKernelId(_cameraComponent);
            bool isOrthographic = _cameraComponent.orthographic;
            #endregion

            #region Shaders Globals
            Shader.SetGlobalColor("Aura_BaseColor", baseColor);
            Shader.SetGlobalFloat("Aura_BaseDensity", baseDensity);
            Shader.SetGlobalFloat("Aura_Extinction", extinction);
            Shader.SetGlobalVector("Aura_BufferResolution", BufferResolutionVector);
            Shader.SetGlobalVector("Aura_BufferTexelSize", BufferTexelSizeVector);
            Shader.SetGlobalTexture("Aura_VolumetricLightingTexture", FogVolumeTexture);
            Shader.SetGlobalFloat("Aura_DepthBiasCoefficient", depthBiasCoefficient);
            Shader.SetGlobalFloat("Aura_DepthBiasReciproqualCoefficient", depthBiasReciproqualCoefficient);
            Shader.SetGlobalVector("Aura_FrustumRanges", _frustumRanges);
            #endregion

            Graphics.ClearRandomWriteTargets();

            _frustumSettingsToId.ComputeFlags();

            bool useReprojection = _frustumSettingsToId.HasFlags(FrustumParameters.EnableTemporalReprojection);
            _computeVisibleCellsComputeShader.SetBool("useReprojection", useReprojection);

            #region Occlusion culling
            _dispatchBuffers[5].SetCounterValue(0);
            bool useOcclusion = _frustumSettingsToId.HasFlags(FrustumParameters.EnableOcclusionCulling);
            if (useOcclusion)
            {
                Profiler.BeginSample("Aura 2 : Compute occlusion data");

                Profiler.BeginSample("Aura 2 : Compute maxima");
                _computeMaximumDepthComputeShader.SetTextureFromGlobal((int)_frustumSettings.QualitySettings.occlusionCullingAccuracy, "depthTexture", "_CameraDepthTexture");
                _computeMaximumDepthComputeShader.SetVector("cameraRanges", _frustumRanges);
                _computeMaximumDepthComputeShader.SetVector("zParameters", isOrthographic ? _cameraRanges : _zParameters);
                _computeMaximumDepthComputeShader.SetBool("isOrthographic", isOrthographic);
                _computeMaximumDepthComputeShader.SetTexture((int)_frustumSettings.QualitySettings.occlusionCullingAccuracy, "occlusionTexture", OcclusionTexture.WriteBuffer);
                _computeMaximumDepthComputeShader.DispatchIndirect((int)_frustumSettings.QualitySettings.occlusionCullingAccuracy, _dispatchBuffers[2]); // TODO : Use parallel reduction (http://diaryofagraphicsprogrammer.blogspot.com/2014/03/compute-shader-optimizations-for-amd.html && https://developer.download.nvidia.com/compute/cuda/1.1-Beta/x86_website/projects/reduction/doc/reduction.pdf) to get as close as the target size then resize to perfect size
                OcclusionTexture.Swap();
                if (_processOcclusionMapMaterial == null)
                {
                    _processOcclusionMapMaterial = new Material(_processOcclusionMapShader);
                }
                _processOcclusionMapMaterial.SetVector("bufferResolution", BufferResolutionVector);
                Graphics.Blit(OcclusionTexture.ReadBuffer, OcclusionTexture.WriteBuffer, _processOcclusionMapMaterial);
                OcclusionTexture.Swap();
                Profiler.EndSample();

                Profiler.BeginSample("Aura 2 : Compute visibility");
                _computeVisibleCellsComputeShader.SetTexture(1, "occlusionTexture", OcclusionTexture.ReadBuffer);
                _computeVisibleCellsComputeShader.SetBuffer(1, "appendedCellsBuffer", _dispatchBuffers[5]);
                _computeVisibleCellsComputeShader.SetTexture(1, "maximumSliceAmountTexture", SliceTexture.WriteBuffer);
                _computeVisibleCellsComputeShader.DispatchIndirect(1, _dispatchBuffers[3]);
                ComputeBuffer.CopyCount(_dispatchBuffers[5], _dispatchBuffers[0], 0);
                SliceTexture.Swap();

                _computeVisibleCellsComputeShader.SetBuffer(2, "visibleCellsAmountBuffer", _dispatchBuffers[0]);
                _computeVisibleCellsComputeShader.SetBuffer(2, "sizeBuffer", _dispatchBuffers[1]);
                _computeVisibleCellsComputeShader.DispatchIndirect(2, _dispatchBuffers[4]);
                Profiler.EndSample();

                Profiler.EndSample();
            }
            else
            {
                if (_previousOcclusionCullingState)
                {
                    WorkingBuffers.ReleaseOcclusionTextureBuffer();
                    WorkingBuffers.ReleaseSliceTextureBuffer();
                    ComputeDispatchSizes();
                }

                _computeVisibleCellsComputeShader.SetTexture(0, "occlusionTexture", Aura.ResourcesCollection.dummyTexture);
                _computeVisibleCellsComputeShader.SetTexture(0, "maximumSliceAmountTexture", Aura.ResourcesCollection.DummyTextureUAV);
                _computeVisibleCellsComputeShader.SetBuffer(0, "appendedCellsBuffer", _dispatchBuffers[5]);
                _computeVisibleCellsComputeShader.DispatchIndirect(0, _dispatchBuffers[3]);

                //uint[] tst = new uint[3];
                //_dispatchBuffers[1].GetData(tst);
                //Debug.Log(tst[0] + ", " + tst[1] + ", " + tst[2]);
                //_dispatchBuffers[0].GetData(tst);
                //Debug.Log(tst[0] + ", " + tst[1] + ", " + tst[2]);
            }
            #endregion

            #region Compute contributions
            Profiler.BeginSample("Aura 2 : Compute volumetric lighting and density");

            DataVolumeTexture.Swap();

            DataVolumeTexture.WriteBuffer.Clear(Vector4.zero);
            FogVolumeTexture.Clear(Vector4.zero);

            _computeDataComputeShader.SetTexture(currentKernelIndex, "lightingTexture", DataVolumeTexture.WriteBuffer);
            _computeDataComputeShader.SetTexture(currentKernelIndex, "previousFrameLightingVolumeTexture", DataVolumeTexture.ReadBuffer);
            _computeDataComputeShader.SetFloat("time", AuraCamera.Time);
            _computeDataComputeShader.SetVector("cameraPosition", _cameraComponent.transform.position.AsVector4(1.0f));
            _computeDataComputeShader.SetVector("cameraRanges", _frustumRanges);
            _computeDataComputeShader.SetFloat("baseDensity", baseDensity);
            _computeDataComputeShader.SetBool("useScattering", _frustumSettings.BaseSettings.useScattering);
            _computeDataComputeShader.SetFloat("baseScattering", 1.0f - _frustumSettings.BaseSettings.scattering);
            _computeDataComputeShader.SetVector("baseTint", _frustumSettings.BaseSettings.useTint ? (_frustumSettings.BaseSettings.tint * _frustumSettings.BaseSettings.tintStrength) : Color.white);
            _computeDataComputeShader.SetVector("baseColor", baseColor);

            #region Frustum Corners
            if (_cameraComponent.GetCameraStereoMode() == StereoMode.SinglePass)
            {
                _cameraComponent.GetFrustumCorners(Camera.MonoOrStereoscopicEye.Left, _frustumRanges.x, _frustumRanges.y, ref _frustumCornersWorldPositionArray);
                _cameraComponent.GetFrustumCorners(Camera.MonoOrStereoscopicEye.Right, _frustumRanges.x, _frustumRanges.y, ref _secondaryFrustumCornersWorldPositionArray);
            }
            else
            {
                _cameraComponent.GetFrustumCorners(Camera.MonoOrStereoscopicEye.Mono, _frustumRanges.x, _frustumRanges.y, ref _frustumCornersWorldPositionArray);
            }
            _computeDataComputeShader.SetFloats("frustumCornersWorldPositionArray", _frustumCornersWorldPositionArray);
            _computeDataComputeShader.SetFloats("secondaryFrustumCornersWorldPositionArray", _secondaryFrustumCornersWorldPositionArray);
            _computeAccumulationComputeShader.SetFloats("frustumCornersWorldPositionArray", _frustumCornersWorldPositionArray);
            _computeAccumulationComputeShader.SetFloats("secondaryFrustumCornersWorldPositionArray", _secondaryFrustumCornersWorldPositionArray);
            #endregion

            #region Temporal Reprojection
            _computeDataComputeShader.SetBool("useReprojection", useReprojection);
            _computeDataComputeShader.SetFloat("temporalReprojectionFactor", _frustumSettings.QualitySettings.temporalReprojectionFactor);
            _computeDataComputeShader.SetFloats("previousFrameWorldToClipMatrix", _previousWorldToClipMatrixFloats);
            _computeDataComputeShader.SetFloats("previousFrameSecondaryWorldToClipMatrix", _previousSecondaryWorldToClipMatrixFloats);
            _computeDataComputeShader.SetTexture(currentKernelIndex, "previousMaximumSliceAmountTexture", SliceTexture.WriteBuffer);
            _computeDataComputeShader.SetVector("cameraRanges", _frustumRanges);
            #endregion

            #region Volumes Injection
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableVolumes))
            {
                _computeDataComputeShader.SetBool("useVolumes", true);
                _computeDataComputeShader.SetInt("volumeCount", _volumesManager.Buffer.count);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "volumeDataBuffer", _volumesManager.Buffer);
            }
            else
            {
                _computeDataComputeShader.SetBool("useVolumes", false);
                _computeDataComputeShader.SetInt("volumeCount", 0);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "volumeDataBuffer", _volumesManager.EmptyBuffer);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableVolumesTexture2DMask))
            {
                _computeDataComputeShader.SetBool("useTexture2DMasks", true);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "texture2DMaskAtlasTexture", AuraCamera.CommonDataManager.VolumesCommonDataManager.Texture2DMasksAtlas);
            }
            else
            {
                _computeDataComputeShader.SetBool("useTexture2DMasks", false);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "texture2DMaskAtlasTexture", Aura.ResourcesCollection.dummyTextureArray);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableVolumesTexture3DMask))
            {
                _computeDataComputeShader.SetBool("useTexture3DMasks", true);
                _computeDataComputeShader.SetVector("texture3DMaskAtlasTextureSize", AuraCamera.CommonDataManager.VolumesCommonDataManager.Texture3DMasksAtlas.GetSize().AsVector4(0.0f));
                _computeDataComputeShader.SetTexture(currentKernelIndex, "texture3DMaskAtlasTexture", AuraCamera.CommonDataManager.VolumesCommonDataManager.Texture3DMasksAtlas);
            }
            else
            {
                _computeDataComputeShader.SetBool("useTexture3DMasks", false);
                _computeDataComputeShader.SetVector("texture3DMaskAtlasTextureSize", Vector4.zero);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "texture3DMaskAtlasTexture", Aura.ResourcesCollection.dummyTexture3D);
            }

            _computeDataComputeShader.SetBool("useVolumesNoise", _frustumSettingsToId.HasFlags(FrustumParameters.EnableVolumesNoiseMask));
            #endregion

            #region Ambient Lighting
            _computeDataComputeShader.SetBool("useAmbientLighting", _frustumSettingsToId.HasFlags(FrustumParameters.EnableAmbientLighting));
            _computeDataComputeShader.SetInt("ambientMode", (int)RenderSettings.ambientMode);
            _computeDataComputeShader.SetVector("ambientColorBottom", RenderSettings.ambientGroundColor);
            _computeDataComputeShader.SetVector("ambientColorHorizon", RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Trilight ? RenderSettings.ambientEquatorColor : RenderSettings.ambientLight);
            _computeDataComputeShader.SetVector("ambientColorTop", RenderSettings.ambientSkyColor);
            _computeDataComputeShader.SetVector("ambientShAr", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.firstBandCoefficients.shAr);
            _computeDataComputeShader.SetVector("ambientShAb", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.firstBandCoefficients.shAb);
            _computeDataComputeShader.SetVector("ambientShAg", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.firstBandCoefficients.shAg);
            _computeDataComputeShader.SetVector("ambientShBr", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.shBr);
            _computeDataComputeShader.SetVector("ambientShBg", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.shBg);
            _computeDataComputeShader.SetVector("ambientShBb", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.shBb);
            _computeDataComputeShader.SetVector("ambientShC", AuraCamera.CommonDataManager.AmbientLightingCommonDataManager.Coefficients.shC);
            _computeDataComputeShader.SetFloat("ambientLightingFactor", _frustumSettings.BaseSettings.ambientLightingStrength * AmbientLightingCommonDataManager.GlobalStrength);
            #endregion

            #region Light Probes
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableLightProbes))
            {
                Profiler.BeginSample("Aura 2 : Light Probes");
                _lightProbesCoefficientsTextureSizeVector = ((Vector3)WorkingBuffers.LightProbesCoefficientsTextureResolution).AsVector4(1.0f);
                _lightProbesCoefficientsTextureHalfTexelSize = _lightProbesCoefficientsTextureSizeVector.GetReciproqual() * 0.5f;
                // Would be more efficient to be able to call LightProbes.GetInterpolatedProbe()-like function directly on GPU
                // Would be worth also to test about stealing the LightProxyVolumes' generated Texture3D, store them in an atlas and sample after in the compute shader, but will be expensive as well and requires LightProxyVolumes
                for (int i = 0; i < WorkingBuffers.LightProbesCoefficientsTextureResolution.x; ++i)
                {
                    float xRatio = (float)i / (_lightProbesCoefficientsTextureSizeVector.x - 1.0f);
                    for (int j = 0; j < WorkingBuffers.LightProbesCoefficientsTextureResolution.y; ++j)
                    {
                        float yRatio = (float)j / (_lightProbesCoefficientsTextureSizeVector.y - 1.0f);
                        for (int k = 0; k < WorkingBuffers.LightProbesCoefficientsTextureResolution.z; ++k)
                        {
                            float zRatio = (float)(k + 1.0f) / _lightProbesCoefficientsTextureSizeVector.z;
                            zRatio = 1.0f - Mathf.Pow(1.0f - zRatio, _frustumSettings.QualitySettings.depthBiasCoefficient);
                            int index = k * WorkingBuffers.LightProbesCoefficientsTextureResolution.x * WorkingBuffers.LightProbesCoefficientsTextureResolution.y + j * WorkingBuffers.LightProbesCoefficientsTextureResolution.x + i;
                            SphericalHarmonicsL2 sphericalHarmonics;
                            Vector3 probeWorldPosition = _cameraComponent.ViewportToWorldPoint(new Vector3(xRatio, yRatio, zRatio * _frustumSettings.QualitySettings.farClipPlaneDistance));

                            bool sampleProbe = false;
                            for (int l = 0; l < AuraCamera.CommonDataManager.VolumesCommonDataManager.RegisteredLightProbesProxyVolumesList.Count; ++l)
                            {
                                if (!Mathf.Approximately(AuraCamera.CommonDataManager.VolumesCommonDataManager.RegisteredLightProbesProxyVolumesList[l].lightProbesMultiplier, 0.0f) && AuraCamera.CommonDataManager.VolumesCommonDataManager.RegisteredLightProbesProxyVolumesList[l].Bounds.Contains(probeWorldPosition))
                                {
                                    sampleProbe = true;
                                    break;
                                }
                            }

                            if (sampleProbe)
                            {
                                LightProbes.GetInterpolatedProbe(probeWorldPosition, null, out sphericalHarmonics);
                                WorkingBuffers.LightProbesCoefficients[index] = sphericalHarmonics.RepackFirstBandForShaders();
                            }
                        }
                    }
                }

                WorkingBuffers.LightProbesCoefficientsComputeBuffer.SetData(WorkingBuffers.LightProbesCoefficients);
                _renderLightProbesTextureComputeShader.SetTexture(0, "lightProbesCoefficientsTexture", WorkingBuffers.LightProbesCoefficientsTexture);
                _renderLightProbesTextureComputeShader.SetVector("lightProbesCoefficientsTextureSize", _lightProbesCoefficientsTextureSizeVector);
                _renderLightProbesTextureComputeShader.SetBuffer(0, "lightProbesCoefficientsBuffer", WorkingBuffers.LightProbesCoefficientsComputeBuffer);
                // Directly generate light probes color in the small texture buffer instead for massive performance improvement, will lose the local light phasing but it's too subtle anyway
                _renderLightProbesTextureComputeShader.Dispatch(0, WorkingBuffers.LightProbesCoefficientsTextureResolution.x, WorkingBuffers.LightProbesCoefficientsTextureResolution.y, WorkingBuffers.LightProbesCoefficientsTextureResolution.z); //////////////////////////////////////////////// DIRECTEMENT GENERER UNE TEXTURE AVEC LES COULEURS????????
                _computeDataComputeShader.SetBool("useLightProbes", true);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "lightProbesCoefficientsTexture", WorkingBuffers.LightProbesCoefficientsTexture);
                _computeDataComputeShader.SetVector("lightProbesCoefficientsTextureHalfTexelSize", _lightProbesCoefficientsTextureHalfTexelSize);
                Profiler.EndSample();
            }
            else
            {
                if (_previousLightProbesState)
                {
                    WorkingBuffers.ReleaseAllLightProbesBuffers();
                }

                _computeDataComputeShader.SetBool("useLightProbes", false);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "lightProbesCoefficientsTexture", Aura.ResourcesCollection.dummyTexture3D);
                _computeDataComputeShader.SetVector("lightProbesCoefficientsTextureHalfTexelSize", Vector4.one);
            }
            #endregion

            _computeDataComputeShader.SetBool("useLightsCookies", _frustumSettingsToId.HasFlags(FrustumParameters.EnableLightsCookies));

            #region Directional lights
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableDirectionalLights))
            {
                _computeDataComputeShader.SetBool("useDirectionalLights", true);
                _computeDataComputeShader.SetInt("directionalLightCount", AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalLightsManager.DataBuffer.count);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "directionalLightDataBuffer", AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalLightsManager.DataBuffer);
            }
            else
            {
                _computeDataComputeShader.SetBool("useDirectionalLights", false);
                _computeDataComputeShader.SetInt("directionalLightCount", 0);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "directionalLightDataBuffer", AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalLightsManager.EmptyBuffer);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableDirectionalLightsShadows))
            {
                _computeDataComputeShader.SetBool("useDirectionalLightsShadows", true);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "directionalShadowMapsArray", AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalShadowMapsArray);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "directionalShadowDataArray", AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalShadowDataArray);
            }
            else
            {
                _computeDataComputeShader.SetBool("useDirectionalLightsShadows", false);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "directionalShadowMapsArray", Aura.ResourcesCollection.dummyTextureArray);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "directionalShadowDataArray", Aura.ResourcesCollection.dummyTextureArray);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableLightsCookies) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasDirectionalCookieCasters)
            {
                _computeDataComputeShader.SetTexture(currentKernelIndex, "directionalCookieMapsArray", AuraCamera.CommonDataManager.LightsCommonDataManager.DirectionalCookieMapsArray);
            }
            else
            {
                _computeDataComputeShader.SetTexture(currentKernelIndex, "directionalCookieMapsArray", Aura.ResourcesCollection.dummyTextureArray);
            }
            #endregion

            #region Spot lights
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableSpotLights))
            {
                _computeDataComputeShader.SetBool("useSpotLights", true);
                _computeDataComputeShader.SetInt("spotLightCount", _spotLightsManager.Buffer.count);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "spotLightDataBuffer", _spotLightsManager.Buffer);
            }
            else
            {
                _computeDataComputeShader.SetBool("useSpotLights", false);
                _computeDataComputeShader.SetInt("spotLightCount", 0);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "spotLightDataBuffer", _spotLightsManager.EmptyBuffer);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableSpotLightsShadows))
            {
                _computeDataComputeShader.SetBool("useSpotLightsShadows", true);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "spotShadowMapsArray", AuraCamera.CommonDataManager.LightsCommonDataManager.SpotShadowMapsArray);
            }
            else
            {
                _computeDataComputeShader.SetBool("useSpotLightsShadows", false);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "spotShadowMapsArray", Aura.ResourcesCollection.dummyTextureArray);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableLightsCookies) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasSpotCookieCasters)
            {
                _computeDataComputeShader.SetTexture(currentKernelIndex, "spotCookieMapsArray", AuraCamera.CommonDataManager.LightsCommonDataManager.SpotCookieMapsArray);
            }
            else
            {
                _computeDataComputeShader.SetTexture(currentKernelIndex, "spotCookieMapsArray", Aura.ResourcesCollection.dummyTextureArray);
            }
            #endregion

            #region Point lights
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnablePointLights))
            {
                _computeDataComputeShader.SetBool("usePointLights", true);
                _computeDataComputeShader.SetInt("pointLightCount", _pointLightsManager.Buffer.count);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "pointLightDataBuffer", _pointLightsManager.Buffer);
            }
            else
            {
                _computeDataComputeShader.SetBool("usePointLights", false);
                _computeDataComputeShader.SetInt("pointLightCount", 0);
                _computeDataComputeShader.SetBuffer(currentKernelIndex, "pointLightDataBuffer", _pointLightsManager.EmptyBuffer);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnablePointLightsShadows))
            {
                _computeDataComputeShader.SetBool("usePointLightsShadows", true);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "pointShadowMapsArray", AuraCamera.CommonDataManager.LightsCommonDataManager.PointShadowMapsArray);
            }
            else
            {
                _computeDataComputeShader.SetBool("usePointLightsShadows", false);
                _computeDataComputeShader.SetTexture(currentKernelIndex, "pointShadowMapsArray", Aura.ResourcesCollection.dummyTextureArray);
            }

            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableLightsCookies) && AuraCamera.CommonDataManager.LightsCommonDataManager.HasPointCookieCasters)
            {
                _computeDataComputeShader.SetTexture(currentKernelIndex, "pointCookieMapsArray", AuraCamera.CommonDataManager.LightsCommonDataManager.PointCookieMapsArray);
            }
            else
            {
                _computeDataComputeShader.SetTexture(currentKernelIndex, "pointCookieMapsArray", Aura.ResourcesCollection.dummyTextureArray);
            }
            #endregion

            #region Compute
            _computeDataComputeShader.SetBuffer(currentKernelIndex, "visibleCellsAmountBuffer", _dispatchBuffers[0]);
            _computeDataComputeShader.SetBuffer(currentKernelIndex, "dispatchSizeBuffer", _dispatchBuffers[1]);
            _computeDataComputeShader.SetBuffer(currentKernelIndex, "visibleCellsBuffer", _dispatchBuffers[5]);
            _computeDataComputeShader.SetFloat("densityFactor", 1.0f / 16.0f); // make human scale
            _computeDataComputeShader.DispatchIndirect(currentKernelIndex, _dispatchBuffers[1]);

            Profiler.EndSample();
            #endregion

            #region Denoising Filter
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableDenoisingFilter))
            {
                Profiler.BeginSample("Aura 2 : Apply 3D Denoising Filter");
                DataVolumeTexture.Swap();
                currentKernelIndex = (int)_frustumSettings.QualitySettings.EXPERIMENTAL_denoisingFilterRange;
                _applyMedianFilterComputeShader.SetBuffer(currentKernelIndex, "visibleCellsAmountBuffer", _dispatchBuffers[0]);
                _applyMedianFilterComputeShader.SetBuffer(currentKernelIndex, "dispatchSizeBuffer", _dispatchBuffers[1]);
                _applyMedianFilterComputeShader.SetBuffer(currentKernelIndex, "visibleCellsBuffer", _dispatchBuffers[5]);
                _applyMedianFilterComputeShader.SetVector("Aura_BufferResolution", BufferResolutionVector);
                _applyMedianFilterComputeShader.SetTexture(currentKernelIndex, "sourceTexture", DataVolumeTexture.ReadBuffer);
                _applyMedianFilterComputeShader.SetTexture(currentKernelIndex, "destinationTexture", DataVolumeTexture.WriteBuffer);
                _applyMedianFilterComputeShader.DispatchIndirect(currentKernelIndex, _dispatchBuffers[1]);
                Profiler.EndSample();
            }
            #endregion

            #region Blur Filter
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableBlurFilter))
            {
                Profiler.BeginSample("Aura 2 : Apply 3D Blur Filter");
                DataVolumeTexture.Swap();
                currentKernelIndex = (int)_frustumSettings.QualitySettings.EXPERIMENTAL_blurFilterType * 3 + (int)_frustumSettings.QualitySettings.EXPERIMENTAL_blurFilterRange;
                _applyBlurFilterComputeShader.SetBuffer(currentKernelIndex, "visibleCellsAmountBuffer", _dispatchBuffers[0]);
                _applyBlurFilterComputeShader.SetBuffer(currentKernelIndex, "dispatchSizeBuffer", _dispatchBuffers[1]);
                _applyBlurFilterComputeShader.SetBuffer(currentKernelIndex, "visibleCellsBuffer", _dispatchBuffers[5]);
                _applyBlurFilterComputeShader.SetVector("Aura_BufferResolution", BufferResolutionVector);
                _applyBlurFilterComputeShader.SetVector("Aura_BufferTexelSize", BufferTexelSizeVector);
                _applyBlurFilterComputeShader.SetTexture(currentKernelIndex, "sourceTexture", DataVolumeTexture.ReadBuffer);
                _applyBlurFilterComputeShader.SetTexture(currentKernelIndex, "destinationTexture", DataVolumeTexture.WriteBuffer);
                _applyBlurFilterComputeShader.SetFloat("gaussianDeviation", _frustumSettings.QualitySettings.EXPERIMENTAL_blurFilterGaussianDeviation);
                _applyBlurFilterComputeShader.DispatchIndirect(currentKernelIndex, _dispatchBuffers[1]);
                Profiler.EndSample();
            }
            #endregion

            Shader.SetGlobalTexture("Aura_VolumetricDataTexture", DataVolumeTexture.WriteBuffer);
            #endregion

            #region Accumulate fog texture
            Profiler.BeginSample("Aura 2 : Compute accumulated contributions");
            currentKernelIndex = (_cameraComponent.GetCameraStereoMode() == StereoMode.SinglePass ? 1 : 0) + (_frustumSettings.QualitySettings.debugOcclusionCulling ? 2 : 0) + (useOcclusion ? 4 : 0);
            _computeAccumulationComputeShader.SetTexture(currentKernelIndex, "maximumSliceAmountTexture", useOcclusion ? SliceTexture.ReadBuffer : (Texture)Aura.ResourcesCollection.dummyTexture);
            _computeAccumulationComputeShader.SetVector("cameraPosition", _cameraComponent.transform.position);
            _computeAccumulationComputeShader.SetTexture(currentKernelIndex, "lightingTexture", DataVolumeTexture.WriteBuffer);
            _computeAccumulationComputeShader.SetFloat("extinction", extinction);
            _computeAccumulationComputeShader.SetTexture(currentKernelIndex, "fogVolumeTexture", FogVolumeTexture);
            _computeAccumulationComputeShader.DispatchIndirect(currentKernelIndex, _dispatchBuffers[3]);
            Profiler.EndSample();
            #endregion

            #region Storing Previous Data For Next Frame
            if (_frustumSettingsToId.HasFlags(FrustumParameters.EnableTemporalReprojection))
            {
                if (_cameraComponent.GetCameraStereoMode() == StereoMode.SinglePass)
                {
                    _cameraComponent.ResetStereoProjectionMatrices();
                    _cameraComponent.GetWorldToClipMatrix(Camera.MonoOrStereoscopicEye.Left, _frustumRanges.x, _frustumRanges.y, ref _previousWorldToClipMatrix);
                    _cameraComponent.GetWorldToClipMatrix(Camera.MonoOrStereoscopicEye.Right, _frustumRanges.x, _frustumRanges.y, ref _previousSecondaryWorldToClipMatrix);
                    _previousSecondaryWorldToClipMatrix.ToFloatArray(ref _previousSecondaryWorldToClipMatrixFloats);
                }
                else
                {
                    _cameraComponent.GetWorldToClipMatrix(Camera.MonoOrStereoscopicEye.Mono, _frustumRanges.x, _frustumRanges.y, ref _previousWorldToClipMatrix);
                }

                _previousWorldToClipMatrix.ToFloatArray(ref _previousWorldToClipMatrixFloats);
            }

            _previousOcclusionCullingState = _frustumSettingsToId.HasFlags(FrustumParameters.EnableOcclusionCulling);
            _previousLightProbesState = _frustumSettingsToId.HasFlags(FrustumParameters.EnableLightProbes);
            #endregion
        }

        /// <summary>
        /// Disposes the managed members
        /// </summary>
        public void Dispose()
        {
            ReleaseComputeBuffers();
            WorkingBuffers.ReleaseAllBuffers();
            DisposeManagers();
            _frustumSettings.OnFrustumQualityChanged -= _frustumSettings_OnFrustumQualityChanged;
        }

        /// <summary>
        /// Disposes the managers
        /// </summary>
        private void DisposeManagers()
        {
            _volumesManager.Dispose();
            _volumesManager = null;
            _spotLightsManager.Dispose();
            _spotLightsManager = null;
            _pointLightsManager.Dispose();
            _pointLightsManager = null;
        }

        /// <summary>
        /// Sets a new frustum resolution
        /// </summary>
        /// <param name="frustumGridResolution">The desired resolution</param>
        public void SetFrustumGridResolution(Vector3Int frustumGridResolution)
        {
            _computeDataComputeShader.GetKernelThreadGroupSizes(0, out _threadSizeX, out _threadSizeY, out _threadSizeZ);

            frustumGridResolution.x = frustumGridResolution.x.SnapMin((int)_threadSizeX);
            frustumGridResolution.y = frustumGridResolution.y.SnapMin((int)_threadSizeY);
            frustumGridResolution.z = frustumGridResolution.z.SnapMin((int)_threadSizeZ);

            _frustumSettings.QualitySettings.frustumGridResolution = frustumGridResolution;
            _frustumGridResolution = _frustumSettings.QualitySettings.GetFrustumGridResolution(_cameraComponent);
            _bufferResolutionVector = ((Vector3)_frustumGridResolution).AsVector4(1.0f);
            _bufferTexelSizeVector = _bufferResolutionVector.GetReciproqual();

            WorkingBuffers.VolumetricBuffersResolution = _frustumGridResolution;

            ComputeDispatchSizes();

            if (_dispatchBuffers[5] != null)
            {
                ReleaseComputeBuffer(5);
            }
            _dispatchBuffers[5] = new ComputeBuffer(FrustumGridResolution.x * FrustumGridResolution.y * FrustumGridResolution.z, VisibleCellData.Size, ComputeBufferType.Append);
        }

        /// <summary>
        /// Computes the dispatch size of the compute shaders
        /// </summary>
        private void ComputeDispatchSizes()
        {
            Vector3Int frustumGridResolution = FrustumGridResolution;

            uint[] dispatchSizeBuffer = new uint[3];
            dispatchSizeBuffer[0] = (uint)(frustumGridResolution.x / (int)_threadSizeX);
            dispatchSizeBuffer[1] = (uint)(frustumGridResolution.y / (int)_threadSizeY);
            dispatchSizeBuffer[2] = (uint)(frustumGridResolution.z / (int)_threadSizeZ);
            _dispatchBuffers[1].SetData(dispatchSizeBuffer);

            dispatchSizeBuffer[2] = 1;
            _dispatchBuffers[3].SetData(dispatchSizeBuffer);

            dispatchSizeBuffer[0] = (uint)frustumGridResolution.x;
            dispatchSizeBuffer[1] = (uint)frustumGridResolution.y;
            _dispatchBuffers[2].SetData(dispatchSizeBuffer);

            dispatchSizeBuffer = new uint[1];
            dispatchSizeBuffer[0] = (uint)(frustumGridResolution.x * frustumGridResolution.y * frustumGridResolution.z);
            _dispatchBuffers[0].SetData(dispatchSizeBuffer);
        }

        /// <summary>
        /// Initializes the buffers used by the compute shaders
        /// </summary>
        private void InitializeComputeBuffers()
        {
            _dispatchBuffers = new ComputeBuffer[6];

            _dispatchBuffers[0] = new ComputeBuffer(1, sizeof(uint), ComputeBufferType.Raw);                // The amount of visible cells
            _dispatchBuffers[1] = new ComputeBuffer(3, sizeof(uint), ComputeBufferType.IndirectArguments);  // The variable amount of dispatched groups for computing lighting/density data
            _dispatchBuffers[2] = new ComputeBuffer(3, sizeof(uint), ComputeBufferType.IndirectArguments);  // The amount of dispatched groups for computing occlusion data
            _dispatchBuffers[3] = new ComputeBuffer(3, sizeof(uint), ComputeBufferType.IndirectArguments);  // The amount of dispatched groups for computing visibility/accumulation data

            _dispatchBuffers[4] = new ComputeBuffer(3, sizeof(uint), ComputeBufferType.IndirectArguments);  // Unique dispatched group
            uint[] sizeBuffer = new uint[3];
            sizeBuffer[0] = 1;
            sizeBuffer[1] = 1;
            sizeBuffer[2] = 1;
            _dispatchBuffers[4].SetData(sizeBuffer);

            // _dispatchBuffers[5] -> The reserved buffer for the visible cells
        }

        /// <summary>
        /// Releases the given buffer
        /// </summary>
        /// <param name="index">The index of the buffer to release</param>
        private void ReleaseComputeBuffer(int index)
        {
            _dispatchBuffers[index].Dispose();
            _dispatchBuffers[index] = null;
        }

        /// <summary>
        /// Releases the buffers used by the compute shaders
        /// </summary>
        private void ReleaseComputeBuffers()
        {
            for (int i = 0; i < _dispatchBuffers.Length; ++i)
            {
                ReleaseComputeBuffer(i);
            }
        }
        #endregion
    }
}
