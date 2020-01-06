
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
using UnityEngine.Rendering;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Component to assign on a GameObject with a Light component if you want this Light to be taken into account in Aura
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Light))]
    [AddComponentMenu("Aura 2/Aura Light", 1)]
    [ExecuteInEditMode]
    public class AuraLight : CullableObject
    {
        #region Public Members
        /// <summary>
        /// Strength of the volumetric light contribution
        /// </summary>
        public float strength = 1.0f; // See Reset() for default value
        /// <summary>
        /// Allows to ignore the scattering of the light
        /// </summary>
        public BooleanChoice useScattering = BooleanChoice.Default;
        /// <summary>
        /// Allows to ignore local scattering and refers to the "overridingScattering" property instead
        /// </summary>
        public bool overrideScattering;
        /// <summary>
        /// Overriding scattering if "overrideScattering" is checked
        /// </summary>
        [Range(0.0f,1.0f)]
        public float overridingScattering = 0.5f;
        /// <summary>
        /// Biases the scattering factor in order to have more control for each light
        /// </summary>
        public float scatteringBias = 0.0f;
        /// <summary>
        /// Enables the color temperature tint.
        /// Unity currently gives no way to know if the color temperature mode is enabled on the light, therefore it has to be manually enabled.
        /// </summary>
        public bool useColorTemperatureTint = false;
        /// <summary>
        /// Allows to ignore the light's color and refers to the "overridingColor" property instead
        /// </summary>
        public bool overrideColor;
        /// <summary>
        /// Overriding color if "overrideColor" is checked
        /// </summary>
        [ColorCircularPicker]
        public Color overridingColor = Color.white;
        /// <summary>
        /// Allows to use light's shadows attenuation
        /// </summary>
        public bool useShadow = true;
        /// <summary>
        /// The light's shadow map
        /// </summary>
        public RenderTexture shadowMapRenderTexture;
        /// <summary>
        /// Allows to use light's cookie attenuation
        /// </summary>
        public bool useCookie = true;
        /// <summary>
        /// The light's cookie map
        /// </summary>
        public RenderTexture cookieMapRenderTexture;
        /// <summary>
        /// Allows to ignore the directional light's color and refers to the \"overridingColor\" property instead
        /// </summary>
        public bool enableOutOfPhaseColor;
        /// <summary>
        /// Strength of out-of-phase color
        /// </summary>
        public float outOfPhaseColorStrength = 0.1f;
        /// <summary>
        /// Out-of-phase color
        /// </summary>
        [ColorCircularPicker]
        public Color outOfPhaseColor = Color.cyan;
        /// <summary>
        /// The directional light's shadow data texture
        /// </summary>
        public RenderTexture shadowDataRenderTexture;
        /// <summary>
        /// Custom distance falloff start (Spot/Point Lights only)
        /// </summary>
        public float customDistanceFalloffThreshold = 0.5f;
        /// <summary>
        /// Custom distance falloff power (Spot/Point Lights only)
        /// </summary>
        public float customDistanceFalloffPower = 2.0f;
        /// <summary>
        /// Custom cookie distance falloff start (Spot/Point Lights only)
        /// </summary>
        public float customCookieDistanceFalloffStartThreshold = 0.1f;
        /// <summary>
        /// Custom cookie distance falloff end (Spot/Point Lights only)
        /// </summary>
        public float customCookieDistanceFalloffEndThreshold = 0.25f;
        /// <summary>
        /// Custom cookie distance falloff power (Spot/Point Lights only)
        /// </summary>
        public float customCookieDistanceFalloffPower = 2.0f;
        /// <summary>
        /// Custom angular falloff start (Spot Lights only)
        /// </summary>
        public float customAngularFalloffThreshold = 0.8f;
        /// <summary>
        /// Custom angular falloff power (Spot Lights only)
        /// </summary>
        public float customAngularFalloffPower = 2.0f;
        #endregion

        #region Private Members
        /// <summary>
        /// Shader used to copy the directional light's cascaded shadows projection data
        /// </summary>
        private Shader _storeDirectionalShadowDataShader;
        /// <summary>
        /// Shader used to store the directional/spot light's cookie map
        /// </summary>
        private Shader _storeDirectionalSpotCookieMapShader;
        /// <summary>
        /// Shader used to store the point light's shadow map (renders the TextureCUBE into a Texture2D)
        /// </summary>
        private Shader _storePointLightShadowMapShader;
        /// <summary>
        /// Mesh used to store the point light's shadow map (renders the TextureCUBE into a Texture2D). TODO : FIND A WAY TO MAKE IT WORK WITHOUT THIS WORKAROUND
        /// </summary>
        private Mesh _storePointLightShadowMapMesh;
        /// <summary>
        /// Shader used to store the point light's cookie map (renders the TextureCUBE into a Texture2D)
        /// </summary>
        private Shader _storePointLightCookieMapShader;
        /// <summary>
        /// Tells if the component is initialized
        /// </summary>
        private bool _isInitialized;
        /// <summary>
        /// The attached light component
        /// </summary>
        private Light _lightComponent;
        /// <summary>
        /// Used for checking changes of light's type
        /// </summary>
        private LightType _previousLightType;
        /// <summary>
        /// Used for checking changes in "useShadow"
        /// </summary>
        private bool _previousUseShadow;
        /// <summary>
        /// The index of the light's shadow map in the composed Texture2DArray that is sent to the compute shader
        /// </summary>
        private int _shadowMapIndex;
        /// <summary>
        /// The command buffer used to copy the shadow map at a specific stage of the light's processing
        /// </summary>
        private CommandBuffer _copyShadowmapCommandBuffer;
        /// <summary>
        /// Used for checking changes in "useCookie"
        /// </summary>
        private bool _previousUseCookie;
        /// <summary>
        /// The index of the light's cookie map in the composed Texture2DArray that is sent to the compute shader
        /// </summary>
        private int _cookieMapIndex;
        /// <summary>
        /// Used for checking changes in the light's cookie texture
        /// </summary>
        private Texture2D _previousCookieTexture;
        /// <summary>
        /// Material used to copy the directional light's cascaded shadows projection data
        /// </summary>
        private Material _storeShadowDataMaterial;
        /// <summary>
        /// The command buffer used to copy the directional light's shadow data at a specific stage of the light's processing
        /// </summary>
        private CommandBuffer _storeShadowDataCommandBuffer;
        /// <summary>
        /// Material used to store the point light's shadow map (renders the TextureCUBE into a Texture2D)
        /// </summary>
        private Material _storePointLightShadowMapMaterial;
        /// <summary>
        /// Material used to store the point light's cookie map (renders the TextureCUBE into a Texture2D)
        /// </summary>
        private Material _storeCookieMapMaterial;
        /// <summary>
        /// The collected data of the directional light's the will be packed into a common compute buffer by the lights manager and sent to the compute shader
        /// </summary>
        private DirectionalLightParameters _directionalLightParameters;
        /// <summary>
        /// The collected data of the spot light's the will be packed into a common compute buffer by the lights manager and sent to the compute shader
        /// </summary>
        private SpotLightParameters _spotLightParameters;
        /// <summary>
        /// The collected data of the point light's the will be packed into a common compute buffer by the lights manager and sent to the compute shader
        /// </summary>
        private PointLightParameters _pointLightParameters;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the attached light's type
        /// </summary>
        public Light LightComponent
        {
            get
            {
                if(_isInitialized)
                {
                    return _lightComponent;
                }
                else
                {
                    return GetComponent<Light>();
                }
            }
        }

        /// <summary>
        /// Returns the attached light's type
        /// </summary>
        public LightType Type
        {
            get
            {
                return LightComponent.type;
            }
        }

        /// <summary>
        /// Tells if it should cast shadows
        /// </summary>
        public bool CastsShadows
        {
            get
            {
                return LightComponent.shadows != LightShadows.None && useShadow;
            }
        }

        /// <summary>
        /// Tells if it should cast cookie
        /// </summary>
        public bool CastsCookie
        {
            get
            {
                return LightComponent.cookie != null && useCookie;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the light is being disabled
        /// </summary>
        public event Action<AuraLight,LightType> OnUninitialize;
        #endregion

        #region Monobehaviour functions
        private void Reset()
        {
            if (GetComponent<Light>().type == LightType.Directional) // Due to long distance accumulation, directional lights are perceived as overbright
            {
                strength *= 0.25f;
            }
        }

        private void OnEnable()
        {
            if(!Aura.IsCompatible)
            {
                enabled = false;
                return;
            }

            Initialize();
        }

        private void OnDisable()
        {
            Uninitialize();
        }

        private void Update()
        {
            if(_previousUseShadow != CastsShadows || _previousUseCookie != CastsCookie || _previousLightType != Type)
            {
                Reinitialize();
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Function run when the OnPreCullEvent is raised on the Aura main component
        /// </summary>
        private void Camera_onPreCull(Camera camera)
        {
            if(this == null)
            {
                Camera.onPreCull -= Camera_onPreCull;
                return;
            }

            if(CastsShadows)
            {
                switch(Type)
                {
                    case LightType.Point :
                        {
                            _copyShadowmapCommandBuffer.Clear();
                        }
                        break;
                }
            }

            UpdateBoundingSphere();
        }

        /// <summary>
        ///     Called when any camera is a about to render
        /// </summary>
        private void Camera_onPreRender(Camera camera)
        {
            if(this == null)
            {
                Camera.onPreRender -= Camera_onPreRender;
                return;
            }

            if(CastsCookie)
            {
                CopyCookieMap();
            }

            PackParameters(camera);
        }

        /// <summary>
        /// Called when the amount of cascades is changed
        /// </summary>
        private void LightsCommonDataManager_OnCascadesCountChanged()
        {
            Reinitialize();
        }

        /// <summary>
        /// Initializes the component (command buffers, registrations, events, managed members ...)
        /// </summary>
        private void Initialize()
        {
            InitializeResources();

            _lightComponent = GetComponent<Light>();
            _previousLightType = Type;

            if (CastsShadows)
            {
                Vector2Int shadowMapSize = new Vector2Int(0, 0);
                switch(Type)
                {
                    case LightType.Directional :
                        {
                            shadowMapSize = DirectionalLightsManager.ShadowMapSize;
                        }
                        break;

                    case LightType.Spot :
                        {
                            shadowMapSize = SpotLightsManager.shadowMapSize;
                        }
                        break;

                    case LightType.Point :
                        {
                            shadowMapSize = PointLightsManager.shadowMapSize;
                        }
                        break;
                }

                shadowMapRenderTexture = new RenderTexture(shadowMapSize.x, shadowMapSize.y, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                shadowMapRenderTexture.name = gameObject.name + " : Shadow Map Render Texture";
                shadowMapRenderTexture.Create();
                RenderTargetIdentifier shadowMapRenderTextureIdentifier = BuiltinRenderTextureType.CurrentActive;
                _copyShadowmapCommandBuffer = new CommandBuffer();
                _copyShadowmapCommandBuffer.name = "Aura 2 : Store shadowmap";
                _copyShadowmapCommandBuffer.SetShadowSamplingMode(shadowMapRenderTextureIdentifier, ShadowSamplingMode.RawDepth);
                _copyShadowmapCommandBuffer.Blit(shadowMapRenderTextureIdentifier, new RenderTargetIdentifier(shadowMapRenderTexture));
                LightComponent.AddCommandBuffer(LightEvent.AfterShadowMap, _copyShadowmapCommandBuffer);

                switch (Type)
                {
                    case LightType.Point :
                        {
                            _storePointLightShadowMapMaterial = new Material(_storePointLightShadowMapShader);
                        }
                        break;

                    case LightType.Directional :
                        {
                            if(shadowDataRenderTexture == null)
                            {
                                shadowDataRenderTexture = new RenderTexture(32, 1, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                                shadowDataRenderTexture.name = gameObject.name + " : Shadow Data Render Texture";
                            }

                            _storeShadowDataCommandBuffer = new CommandBuffer();
                            _storeShadowDataCommandBuffer.name = "Aura 2 : Store directional shadow data";

                            _storeShadowDataMaterial = new Material(_storeDirectionalShadowDataShader);

                            LightComponent.AddCommandBuffer(LightEvent.BeforeScreenspaceMask, _storeShadowDataCommandBuffer);

                            _storeShadowDataCommandBuffer.Blit(null, new RenderTargetIdentifier(shadowDataRenderTexture), _storeShadowDataMaterial);

                            AuraCamera.CommonDataManager.LightsCommonDataManager.OnCascadesCountChanged += LightsCommonDataManager_OnCascadesCountChanged;
                        }
                        break;
                }
            }

            _previousUseShadow = CastsShadows;

            if(CastsCookie)
            {
                Vector2Int cookieMapSize = Vector2Int.zero;
                switch(Type)
                {
                    case LightType.Directional :
                        {
                            cookieMapSize = DirectionalLightsManager.cookieMapSize;
                        }
                        break;

                    case LightType.Spot :
                        {
                            cookieMapSize = SpotLightsManager.cookieMapSize;
                        }
                        break;

                    case LightType.Point :
                        {
                            cookieMapSize = PointLightsManager.cookieMapSize;
                        }
                        break;
                }

                cookieMapRenderTexture = new RenderTexture(cookieMapSize.x, cookieMapSize.y, 0, RenderTextureFormat.R8);
                cookieMapRenderTexture.name = gameObject.name + " : Cookie Map Render Texture";

                switch (Type)
                {
                    case LightType.Point :
                        {
                            _storeCookieMapMaterial = new Material(_storePointLightCookieMapShader);
                        }
                        break;

                    default :
                        {
                            _storeCookieMapMaterial = new Material(_storeDirectionalSpotCookieMapShader);
                        }
                        break;
                }
            }

            _previousUseCookie = CastsCookie;

            AuraCamera.CommonDataManager.LightsCommonDataManager.RegisterLight(this);

            Camera.onPreCull += Camera_onPreCull;
            Camera.onPreRender += Camera_onPreRender;

            _isInitialized = true;
        }

        /// <summary>
        /// Uninitializes the component (command buffers, registrations, events, managed members ...)
        /// </summary>
        private void Uninitialize()
        {
            if (_isInitialized)
            {
                if (OnUninitialize != null)
                {
                    OnUninitialize(this, _previousLightType);
                }

                if (_previousUseShadow)
                {
                    if (_previousLightType == LightType.Directional)
                    {
                        LightComponent.RemoveCommandBuffer(LightEvent.BeforeScreenspaceMask, _storeShadowDataCommandBuffer);

                        _storeShadowDataCommandBuffer.Clear();
                        _storeShadowDataCommandBuffer.Release();
                        _storeShadowDataCommandBuffer = null;

                        shadowDataRenderTexture.Release();
                        shadowDataRenderTexture = null;

                        AuraCamera.CommonDataManager.LightsCommonDataManager.OnCascadesCountChanged -= LightsCommonDataManager_OnCascadesCountChanged;
                    }

                    LightComponent.RemoveCommandBuffer(LightEvent.AfterShadowMap, _copyShadowmapCommandBuffer);

                    _copyShadowmapCommandBuffer.Clear();
                    _copyShadowmapCommandBuffer.Release();
                    _copyShadowmapCommandBuffer = null;

                    shadowMapRenderTexture.Release();
                    shadowMapRenderTexture = null;
                }

                Camera.onPreCull -= Camera_onPreCull;
                Camera.onPreRender -= Camera_onPreRender;

                _isInitialized = false;
            }
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
        /// Initialize the needed resources
        /// </summary>
        private void InitializeResources()
        {
            _storeDirectionalShadowDataShader = Aura.ResourcesCollection.storeDirectionalShadowDataShader;
            _storeDirectionalSpotCookieMapShader = Aura.ResourcesCollection.storeDirectionalSpotCookieMapShader;
            _storePointLightShadowMapShader = Aura.ResourcesCollection.storePointShadowMapShader;
            _storePointLightShadowMapMesh = Aura.ResourcesCollection.storePointShadowMapMesh;
            _storePointLightCookieMapShader = Aura.ResourcesCollection.storePointCookieMapShader;
        }

        /// <summary>
        /// Collects the light's data so it will be packed into a common compute buffer by the lights manager and sent to the compute shader
        /// </summary>
        /// <param name="camera"></param>
        private void PackParameters(Camera camera)
        {
            Vector4 colorTemperature = (LightHelpers.IsColorTemperatureAvailable && useColorTemperatureTint) ? (Vector4)Mathf.CorrelatedColorTemperatureToRGB(LightComponent.colorTemperature) : Vector4.one;
            Vector4 color = (overrideColor ? overridingColor : LightComponent.color * colorTemperature) * LightComponent.intensity * strength;
            int useDefaultScattering = useScattering == BooleanChoice.Default ? 1 : 0;
            float scatteringOverride = useScattering == BooleanChoice.False ? -2 : (overrideScattering ? 1.0f - overridingScattering : -1);

            switch(Type)
            {
                case LightType.Directional :
                    {
                        _directionalLightParameters.color = color;
                        _directionalLightParameters.useDefaultScattering = useDefaultScattering;
                        _directionalLightParameters.scatteringOverride = scatteringOverride;
                        _directionalLightParameters.lightPosition = LightComponent.transform.position;
                        _directionalLightParameters.lightDirection = LightComponent.transform.forward;
                        Matrix4x4 lightToWorldMatrix = Matrix4x4.TRS(LightComponent.transform.position, LightComponent.transform.rotation, Vector3.one);
                        _directionalLightParameters.worldToLightMatrix = MatrixFloats.ToMatrixFloats(lightToWorldMatrix.inverse);
                        _directionalLightParameters.lightToWorldMatrix = MatrixFloats.ToMatrixFloats(lightToWorldMatrix);
                        _directionalLightParameters.shadowMapIndex = CastsShadows ? _shadowMapIndex : -1;

                        _directionalLightParameters.cookieMapIndex = -1;
                        if(CastsCookie)
                        {
                            _directionalLightParameters.cookieMapIndex = _cookieMapIndex;
                            _directionalLightParameters.cookieParameters.x = LightComponent.cookieSize;
                            _directionalLightParameters.cookieParameters.y = LightComponent.cookie.wrapMode == TextureWrapMode.Repeat ? 0 : 1;
                        }

                        _directionalLightParameters.enableOutOfPhaseColor = enableOutOfPhaseColor ? 1 : 0;
                        _directionalLightParameters.outOfPhaseColor = (Vector4)outOfPhaseColor * outOfPhaseColorStrength;
                    }
                    break;

                case LightType.Spot :
                    {
                        _spotLightParameters.color = color;
                        _spotLightParameters.useDefaultScattering = useDefaultScattering;
                        _spotLightParameters.scatteringOverride = scatteringOverride;
                        _spotLightParameters.lightPosition = LightComponent.transform.position;
                        _spotLightParameters.lightDirection = LightComponent.transform.forward;
                        _spotLightParameters.lightRange = LightComponent.range;
                        _spotLightParameters.lightCosHalfAngle = Mathf.Cos(LightComponent.spotAngle * 0.5f * Mathf.Deg2Rad);
                        _spotLightParameters.angularFalloffParameters = new Vector2(customAngularFalloffThreshold, customAngularFalloffPower);
                        _spotLightParameters.distanceFalloffParameters = new Vector2(Mathf.Min(customDistanceFalloffThreshold, 0.999999f), customDistanceFalloffPower);

                        _spotLightParameters.shadowMapIndex = -1;
                        if(CastsShadows)
                        {
                            Matrix4x4 worldToLight = Matrix4x4.TRS(LightComponent.transform.position, LightComponent.transform.rotation, Vector3.one).inverse;
                            Matrix4x4 proj = Matrix4x4.Perspective(LightComponent.spotAngle, 1, LightComponent.shadowNearPlane, LightComponent.range);
                            Matrix4x4 clip = Matrix4x4.TRS(Vector3.one * 0.5f, Quaternion.identity, Vector3.one * 0.5f);
                            Matrix4x4 m = clip * proj;
                            m[0, 2] *= -1;
                            m[1, 2] *= -1;
                            m[2, 2] *= -1;
                            m[3, 2] *= -1;
                            Matrix4x4 worldToShadow = m * worldToLight;
                            _spotLightParameters.worldToShadowMatrix = MatrixFloats.ToMatrixFloats(worldToShadow);

                            _spotLightParameters.shadowMapIndex = _shadowMapIndex;
                            _spotLightParameters.shadowStrength = 1.0f - LightComponent.shadowStrength;
                        }

                        _spotLightParameters.cookieMapIndex = -1;
                        if(CastsCookie)
                        {
                            _spotLightParameters.cookieMapIndex = _cookieMapIndex;
                            _spotLightParameters.cookieParameters.x = customCookieDistanceFalloffStartThreshold;
                            _spotLightParameters.cookieParameters.y = customCookieDistanceFalloffEndThreshold;
                            _spotLightParameters.cookieParameters.z = customCookieDistanceFalloffPower;
                        }
                    }
                    break;

                case LightType.Point :
                    {
                        _pointLightParameters.color = color;
                        _pointLightParameters.useDefaultScattering = useDefaultScattering;
                        _pointLightParameters.scatteringOverride = scatteringOverride;
                        _pointLightParameters.lightPosition = LightComponent.transform.position;
                        _pointLightParameters.lightRange = LightComponent.range;
                        _pointLightParameters.distanceFalloffParameters = new Vector2(Mathf.Min(customDistanceFalloffThreshold, 0.999999f), customDistanceFalloffPower);

                        _pointLightParameters.shadowMapIndex = -1;
                        if(CastsShadows)
                        {
                            Matrix4x4 worldMatrix = Matrix4x4.TRS(camera.transform.position, transform.rotation, Vector3.one * camera.nearClipPlane * 2);
                            _storePointLightShadowMapMaterial.SetMatrix("_WorldViewProj", GL.GetGPUProjectionMatrix(camera.projectionMatrix, true) * camera.worldToCameraMatrix * worldMatrix);
                            _copyShadowmapCommandBuffer.SetGlobalTexture("_ShadowMapTexture", BuiltinRenderTextureType.CurrentActive);
                            _copyShadowmapCommandBuffer.SetRenderTarget(shadowMapRenderTexture);
                            _copyShadowmapCommandBuffer.DrawMesh(_storePointLightShadowMapMesh, worldMatrix, _storePointLightShadowMapMaterial, 0);

                            Matrix4x4 worldToShadow = Matrix4x4.TRS(LightComponent.transform.position, LightComponent.transform.rotation, Vector3.one * LightComponent.range).inverse;
                            _pointLightParameters.worldToShadowMatrix = MatrixFloats.ToMatrixFloats(worldToShadow);
                            #if UNITY_2017_3_OR_NEWER
                            _pointLightParameters.lightProjectionParameters = new Vector2(LightComponent.range / (LightComponent.shadowNearPlane - LightComponent.range), (LightComponent.shadowNearPlane * LightComponent.range) / (LightComponent.shadowNearPlane - LightComponent.range)); // From UnityShaderVariables.cginc:114
                            #endif

                            _pointLightParameters.shadowMapIndex = _shadowMapIndex;
                            _pointLightParameters.shadowStrength = 1.0f - LightComponent.shadowStrength;
                        }

                        _pointLightParameters.cookieMapIndex = -1;
                        if(CastsCookie)
                        {
                            _pointLightParameters.cookieMapIndex = _cookieMapIndex;
                            _pointLightParameters.cookieParameters.x = customCookieDistanceFalloffStartThreshold;
                            _pointLightParameters.cookieParameters.y = customCookieDistanceFalloffEndThreshold;
                            _pointLightParameters.cookieParameters.z = customCookieDistanceFalloffPower;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns the collected data for the directional light
        /// </summary>
        /// <returns>The packed data</returns>
        public DirectionalLightParameters GetDirectionnalParameters()
        {
            return _directionalLightParameters;
        }

        /// <summary>
        /// Returns the collected data for the spot light
        /// </summary>
        /// <returns>The packed data</returns>
        public SpotLightParameters GetSpotParameters()
        {
            return _spotLightParameters;
        }

        /// <summary>
        /// Returns the collected data for the point light
        /// </summary>
        /// <returns>The packed data</returns>
        public PointLightParameters GetPointParameters()
        {
            return _pointLightParameters;
        }

        /// <summary>
        /// Modifies the index of the shadow map in the composed Texture2DArray that is sent to the compute shader 
        /// </summary>
        /// <param name="index">The new index</param>
        public void SetShadowMapIndex(int index)
        {
            _shadowMapIndex = index;
        }

        /// <summary>
        /// Modifies the index of the cookie map in the composed Texture2DArray that is sent to the compute shader 
        /// </summary>
        /// <param name="index">The new index</param>
        public void SetCookieMapIndex(int index)
        {
            _cookieMapIndex = index;
        }

        /// <summary>
        /// Compute the bounding sphere for the ObjectsCuller
        /// </summary>
        private void UpdateBoundingSphere()
        {
            float radius = float.MaxValue;

            switch(Type)
            {
                case LightType.Point :
                    {
                        radius = LightComponent.range;
                    }
                    break;

                case LightType.Spot :
                    {
                        // TODO : MORE ACCURATE BOUNDING SPHERE
                        radius = LightComponent.range;
                    }
                    break;
            }

            UpdateBoundingSphere(transform.position, radius);
        }

        /// <summary>
        /// Copies the light's cookie map into the "cookieMapRenderTexture"
        /// </summary>
        private void CopyCookieMap()
        {
            switch(Type)
            {
                case LightType.Point :
                    {
                        _storeCookieMapMaterial.SetMatrix("_InverseWorldMatrix", LightComponent.transform.worldToLocalMatrix);
                    }
                    break;
            }

            Graphics.Blit(LightComponent.cookie, cookieMapRenderTexture, _storeCookieMapMaterial);
        }
        #endregion

        #region GameObject constructor
        /// <summary>
        /// Method allowing to quickly build GameObjects with the component assigned
        /// </summary>
        /// <param name="name">The name of the new GameObject</param>
        /// <param name="type">The desired light's type</param>
        /// <returns>The created AuraLight gameObject</returns>
        public static GameObject CreateGameObject(string name, LightType type)
        {
            GameObject newGameObject = new GameObject(name);

            newGameObject.AddComponent<Light>();
            newGameObject.GetComponent<Light>().type = type;
            newGameObject.GetComponent<Light>().shadows = LightShadows.Soft;
            newGameObject.AddComponent<AuraLight>();

            return newGameObject;
        }

#if UNITY_EDITOR 
        /// <summary>
        /// Method allowing to add a MenuItem to quickly build GameObjects with the component assigned
        /// </summary>
        /// <param name="menuCommand">Stuff that Unity automatically fills with some editor's contextual infos</param>
        /// <param name="name">The name of the new GameObject</param>
        /// <param name="type">The desired light's type</param>
        /// <returns>The created AuraLight gameObject</returns>
        private static GameObject CreateGameObject(MenuCommand menuCommand, string name, LightType type)
        {
            GameObject newGameObject = CreateGameObject(name, type);

            float offset = 1.5f;
            if(SceneView.lastActiveSceneView != null)
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
        /// Creates a new Aura Directional Light
        /// </summary>
        /// <param name="menuCommand">Stuff that Unity automatically fills with some editor's contextual infos</param>
        [MenuItem("GameObject/Aura 2/Light/Directional", false, 1)]
        private static void CreateDirectionalGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Directional Light", LightType.Directional);
        }

        /// <summary>
        /// Creates a new Aura Spot Light
        /// </summary>
        /// <param name="menuCommand">Stuff that Unity automatically fills with some editor's contextual infos</param>
        [MenuItem("GameObject/Aura 2/Light/Spot", false, 2)]
        private static void CreateSpotGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Spot Light", LightType.Spot);
        }

        /// <summary>
        /// Creates a new Aura Point Light
        /// </summary>
        /// <param name="menuCommand">Stuff that Unity automatically fills with some editor's contextual infos</param>
        [MenuItem("GameObject/Aura 2/Light/Point", false, 3)]
        private static void CreatePointGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Point Light", LightType.Point);
        }
#endif
        #endregion
    }

    #region Gizmo
    #if UNITY_EDITOR 
    /// <summary>
    /// Allows to draw custom gizmos for AuraVolume objects
    /// </summary>
    public class AuraLightGizmoDrawer
    {
        /// <summary>
        /// Draws a custom gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="gizmoType">Gizmo state</param>
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NonSelected | GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
        static void DrawGizmoForAuraLight(AuraLight component, GizmoType gizmoType)
        {
            if (!AuraEditorPrefs.DisplayGizmosOnLights)
            {
                return;
            }

            bool isFaded = (int)gizmoType == (int)GizmoType.NonSelected || (int)gizmoType == (int)GizmoType.NotInSelectionHierarchy || (int)gizmoType == (int)GizmoType.NonSelected + (int)GizmoType.NotInSelectionHierarchy;

            if(isFaded && !AuraEditorPrefs.DisplayGizmosWhenUnselected || !isFaded && !AuraEditorPrefs.DisplayGizmosWhenSelected)
            {
                return;
            }

            float opacity = isFaded ? 0.15f : 1.0f;

            DrawGizmo(component, opacity);
        }

        /// <summary>
        /// Draws the gizmo
        /// </summary>
        /// <param name="component">The target component</param>
        /// <param name="opacity">The gizmo opacity</param>
        private static void DrawGizmo(AuraLight component, float opacity)
        {
            Color color = CustomGizmo.color;
            color.a = CustomGizmo.color.a * opacity;

            switch (component.Type)
            {
                case LightType.Directional:
                    {
                        float size = HandleUtility.GetHandleSize(component.transform.position);
                        const int stepAmount = 8;
                        const float width = 1.0f;
                        const float length = 3.0f;
                        CustomGizmo.DrawCircle(Matrix4x4.TRS(component.transform.position, component.transform.rotation * Quaternion.AngleAxis(90, Vector3.right), Vector3.one * size * width), color, CustomGizmo.pixelWidth);
                        for(int i = 0; i < stepAmount; ++i)
                        {
                            float ratio = (float)i / (float)stepAmount * 2.0f * Mathf.PI;
                            Vector3 localStartPosition = new Vector3(Mathf.Sin(ratio), Mathf.Cos(ratio), 0) * size * width * 0.5f;
                            Vector3 transformedStartPosition = component.transform.localToWorldMatrix.MultiplyPoint(localStartPosition);
                            CustomGizmo.DrawLineSegment(transformedStartPosition, transformedStartPosition + component.transform.forward * size * length, color, CustomGizmo.pixelWidth);
                        }
                    }
                    break;

                case LightType.Spot:
                    {
                        float angleToWidth = Mathf.Tan(component.GetComponent<Light>().spotAngle * Mathf.Deg2Rad * 0.5f) * 2.0f * component.GetComponent<Light>().range;
                        CustomGizmo.DrawCone(Matrix4x4.TRS(component.transform.position, component.transform.rotation, new Vector3(angleToWidth, angleToWidth, component.GetComponent<Light>().range)), color, CustomGizmo.pixelWidth);
                    }
                    break;

                case LightType.Point:
                    {
                        CustomGizmo.DrawSphere(Matrix4x4.TRS(component.transform.position, component.transform.rotation, Vector3.one * component.GetComponent<Light>().range * 2.0f), color, CustomGizmo.pixelWidth);
                    }
                    break;
            }
        }
    }
    #endif
    #endregion
}
