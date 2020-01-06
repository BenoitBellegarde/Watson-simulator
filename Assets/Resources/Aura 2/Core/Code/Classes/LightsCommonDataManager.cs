
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
using System.Collections.Generic;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Handles the lights' registration and maps collection
    /// </summary>
    public class LightsCommonDataManager
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LightsCommonDataManager()
        {
            _previousCascadesCount = QualitySettings.shadowCascades;
        }
        #endregion

        #region Private members
        /// <summary>
        /// List of registered directional Aura Lights
        /// </summary>
        private List<AuraLight> _registeredDirectionalLightsList;
        /// <summary>
        /// Handles the collection and packing of the directional Aura lights' data
        /// </summary>
        private DirectionalLightsManager _directionalLightsManager;
        /// <summary>
        /// The composer that will collect the shadow maps and stack them in a Texture2DArray
        /// </summary>
        private ShadowmapsCollector _directionalLightsShadowMapsCollector;
        /// <summary>
        /// The composer that will collect the shadow data maps and stack them in a Texture2DArray
        /// </summary>
        private DirectionalShadowDataCollector _directionalLightsShadowDataCollector;
        /// <summary>
        /// The composer that will collect the cookie maps and stack them in a Texture2DArray
        /// </summary>
        private Texture2DArrayComposer _directionalLightsCookieMapsCollector;
        /// <summary>
        /// Used for checking changes in the number of shadow cascades
        /// </summary>
        private static int _previousCascadesCount;
        /// <summary>
        /// List of registered spot Aura Lights
        /// </summary>
        private List<AuraLight> _registeredSpotLightsList;
        /// <summary>
        /// The composer that will collect the shadow maps and stack them in a Texture2DArray
        /// </summary>
        private ShadowmapsCollector _spotLightsShadowMapsCollector;
        /// <summary>
        /// The composer that will collect the cookie maps and stack them in a Texture2DArray
        /// </summary>
        private Texture2DArrayComposer _spotLightsCookieMapsCollector;
        /// <summary>
        /// List of registered point Aura Lights
        /// </summary>
        private List<AuraLight> _registeredPointLightsList;
        /// <summary>
        /// The composer that will collect the shadow maps and stack them in a Texture2DArray
        /// </summary>
        private ShadowmapsCollector _pointLightsShadowMapsCollector;
        /// <summary>
        /// The composer that will collect the cookie maps and stack them in a Texture2DArray
        /// </summary>
        private Texture2DArrayComposer _pointLightsCookieMapsCollector;
        #endregion

        #region Properties
        /// <summary>
        /// Accessor to the list of registered directional Aura Lights
        /// </summary>
        public List<AuraLight> RegisteredDirectionalLightsList
        {
            get
            {
                if (_registeredDirectionalLightsList == null)
                {
                    _registeredDirectionalLightsList = new List<AuraLight>();
                }

                return _registeredDirectionalLightsList;
            }
        }

        /// <summary>
        /// Handles the collection and packing of the directional Aura lights' data
        /// </summary>
        public DirectionalLightsManager DirectionalLightsManager
        {
            get
            {
                if (_directionalLightsManager == null)
                {
                    _directionalLightsManager = new DirectionalLightsManager();
                }

                return _directionalLightsManager;
            }
        }

        /// <summary>
        /// Tells if has registered directional lights
        /// </summary>
        public bool HasRegisteredDirectionalLights
        {
            get
            {
                return RegisteredDirectionalLightsList.Count > 0;
            }
        }

        /// <summary>
        /// Tells if there are any shadow caster
        /// </summary>
        public bool HasDirectionalShadowCasters
        {
            get
            {
                return _directionalLightsShadowMapsCollector != null ? _directionalLightsShadowMapsCollector.HasTexture : false;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked shadow maps
        /// </summary>
        public Texture2DArray DirectionalShadowMapsArray
        {
            get
            {
                return _directionalLightsShadowMapsCollector.Texture;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked shadow data maps
        /// </summary>
        public Texture2DArray DirectionalShadowDataArray
        {
            get
            {
                return _directionalLightsShadowDataCollector.Texture;
            }
        }

        /// <summary>
        /// Tells if there are any cookie caster
        /// </summary>
        public bool HasDirectionalCookieCasters
        {
            get
            {
                return _directionalLightsCookieMapsCollector != null ? _directionalLightsCookieMapsCollector.HasTexture : false;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked cookie maps
        /// </summary>
        public Texture2DArray DirectionalCookieMapsArray
        {
            get
            {
                return _directionalLightsCookieMapsCollector.Texture;
            }
        }

        /// <summary>
        /// Accessor to the list of registered spot Aura Lights
        /// </summary>
        public List<AuraLight> RegisteredSpotLightsList
        {
            get
            {
                if (_registeredSpotLightsList == null)
                {
                    _registeredSpotLightsList = new List<AuraLight>();
                }

                return _registeredSpotLightsList;
            }
        }

        /// <summary>
        /// Tells if has registered spot lights
        /// </summary>
        public bool HasRegisteredSpotLights
        {
            get
            {
                return RegisteredSpotLightsList.Count > 0;
            }
        }

        /// <summary>
        /// Tells if there are any shadow caster
        /// </summary>
        public bool HasSpotShadowCasters
        {
            get
            {
                return _spotLightsShadowMapsCollector != null ? _spotLightsShadowMapsCollector.HasTexture : false;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked shadow maps
        /// </summary>
        public Texture2DArray SpotShadowMapsArray
        {
            get
            {
                return _spotLightsShadowMapsCollector.Texture;
            }
        }

        /// <summary>
        /// Tells if there are any cookie caster
        /// </summary>
        public bool HasSpotCookieCasters
        {
            get
            {
                return _spotLightsCookieMapsCollector != null ? _spotLightsCookieMapsCollector.HasTexture : false;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked cookie maps
        /// </summary>
        public Texture2DArray SpotCookieMapsArray
        {
            get
            {
                return _spotLightsCookieMapsCollector.Texture;
            }
        }

        /// <summary>
        /// Accessor to the list of registered point Aura Lights
        /// </summary>
        public List<AuraLight> RegisteredPointLightsList
        {
            get
            {
                if (_registeredPointLightsList == null)
                {
                    _registeredPointLightsList = new List<AuraLight>();
                }

                return _registeredPointLightsList;
            }
        }

        /// <summary>
        /// Tells if has registered point lights
        /// </summary>
        public bool HasRegisteredPointLights
        {
            get
            {
                return RegisteredPointLightsList.Count > 0;
            }
        }

        /// <summary>
        /// Tells if there are any shadow caster
        /// </summary>
        public bool HasPointShadowCasters
        {
            get
            {
                return _pointLightsShadowMapsCollector != null ? _pointLightsShadowMapsCollector.HasTexture : false;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked shadow maps
        /// </summary>
        public Texture2DArray PointShadowMapsArray
        {
            get
            {
                return _pointLightsShadowMapsCollector.Texture;
            }
        }

        /// <summary>
        /// Tells if there are any cookie caster
        /// </summary>
        public bool HasPointCookieCasters
        {
            get
            {
                return _pointLightsCookieMapsCollector != null ? _pointLightsCookieMapsCollector.HasTexture : false;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the stacked cookie maps
        /// </summary>
        public Texture2DArray PointCookieMapsArray
        {
            get
            {
                return _pointLightsCookieMapsCollector.Texture;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the number of shadow cascades changes
        /// </summary>
        public event Action OnCascadesCountChanged;
        /// <summary>
        /// Event raised when a spot light is being registred
        /// </summary>
        public event Action<AuraLight> OnRegisterSpotLight;
        /// <summary>
        /// Event raised when a spot light is being unregistred
        /// </summary>
        public event Action<AuraLight> OnUnregisterSpotLight;
        /// <summary>
        /// Event raised when a point light is being registred
        /// </summary>
        public event Action<AuraLight> OnRegisterPointLight;
        /// <summary>
        /// Event raised when a point light is being unregistred
        /// </summary>
        public event Action<AuraLight> OnUnregisterPointLight;
        #endregion

        #region Functions
        /// <summary>
        /// Releases all the managed members
        /// </summary>
        public void Dispose()
        {
            ReleaseLightsManagers();
        }

        /// <summary>
        /// Releases the lights managers
        /// </summary>
        private void ReleaseLightsManagers()
        {
            if (_directionalLightsManager != null)
            {
                DirectionalLightsManager.Dispose();
            }
        }
        
        /// <summary>
        /// Updates all the data
        /// </summary>
        public void Update()
        {
            if (_previousCascadesCount != QualitySettings.shadowCascades)
            {
                if (_directionalLightsShadowMapsCollector != null)
                {
                    _directionalLightsShadowMapsCollector.Resize(DirectionalLightsManager.ShadowMapSize.x, DirectionalLightsManager.ShadowMapSize.y);
                }

                if (OnCascadesCountChanged != null)
                {
                    OnCascadesCountChanged();
                }

                _previousCascadesCount = QualitySettings.shadowCascades;
            }
            
            GenerateLightsMaps();
            
            DirectionalLightsManager.Update();
        }

        /// <summary>
        /// Registers the Aura light to the global manager
        /// </summary>
        /// <param name="auraLight">The Aura Light to register</param>
        public void RegisterLight(AuraLight auraLight)
        {
            switch (auraLight.Type)
            {
                case LightType.Directional:
                    {
                        if (!RegisteredDirectionalLightsList.Contains(auraLight))
                        {
                            RegisteredDirectionalLightsList.Add(auraLight);

                            if (auraLight.CastsShadows)
                            {
                                if (_directionalLightsShadowMapsCollector == null)
                                {
                                    _directionalLightsShadowMapsCollector = new ShadowmapsCollector(DirectionalLightsManager.ShadowMapSize.x, DirectionalLightsManager.ShadowMapSize.y);
                                }

                                _directionalLightsShadowMapsCollector.AddTexture(auraLight.shadowMapRenderTexture);
                                SetDirectionalShadowMapsId();

                                if (_directionalLightsShadowDataCollector == null)
                                {
                                    _directionalLightsShadowDataCollector = new DirectionalShadowDataCollector();
                                }
                                _directionalLightsShadowDataCollector.AddTexture(auraLight.shadowDataRenderTexture);
                            }


                            if (auraLight.CastsCookie)
                            {
                                if (_directionalLightsCookieMapsCollector == null)
                                {
                                    _directionalLightsCookieMapsCollector = new Texture2DArrayComposer(DirectionalLightsManager.cookieMapSize.x, DirectionalLightsManager.cookieMapSize.y, TextureFormat.R8, true);
                                }
                                _directionalLightsCookieMapsCollector.AddTexture(auraLight.cookieMapRenderTexture);
                                SetDirectionalCookieMapsId();
                            }
                        }
                    }
                    break;

                case LightType.Spot:
                    {
                        if (!RegisteredSpotLightsList.Contains(auraLight))
                        {
                            RegisteredSpotLightsList.Add(auraLight);

                            if (auraLight.CastsShadows)
                            {
                                if (_spotLightsShadowMapsCollector == null)
                                {
                                    _spotLightsShadowMapsCollector = new ShadowmapsCollector(SpotLightsManager.shadowMapSize.x, SpotLightsManager.shadowMapSize.y);
                                }
                                _spotLightsShadowMapsCollector.AddTexture(auraLight.shadowMapRenderTexture);
                                SetSpotShadowMapsId();
                            }

                            if (auraLight.CastsCookie)
                            {
                                if (_spotLightsCookieMapsCollector == null)
                                {
                                    _spotLightsCookieMapsCollector = new Texture2DArrayComposer(SpotLightsManager.cookieMapSize.x, SpotLightsManager.cookieMapSize.y, TextureFormat.R8, true);
                                }
                                _spotLightsCookieMapsCollector.AddTexture(auraLight.cookieMapRenderTexture);
                                SetSpotCookieMapsId();
                            }

                            if (OnRegisterSpotLight != null)
                            {
                                OnRegisterSpotLight(auraLight);
                            }
                        }
                    }
                    break;

                case LightType.Point:
                    {
                        if (!RegisteredPointLightsList.Contains(auraLight))
                        {
                            RegisteredPointLightsList.Add(auraLight);

                            if (auraLight.CastsShadows)
                            {
                                if (_pointLightsShadowMapsCollector == null)
                                {
                                    _pointLightsShadowMapsCollector = new ShadowmapsCollector(PointLightsManager.shadowMapSize.x, PointLightsManager.shadowMapSize.y);
                                }
                                _pointLightsShadowMapsCollector.AddTexture(auraLight.shadowMapRenderTexture);
                                SetPointShadowMapsId();
                            }

                            if (auraLight.CastsCookie)
                            {
                                if (_pointLightsCookieMapsCollector == null)
                                {
                                    _pointLightsCookieMapsCollector = new Texture2DArrayComposer(PointLightsManager.cookieMapSize.x, PointLightsManager.cookieMapSize.y, TextureFormat.R8, true);
                                    _pointLightsCookieMapsCollector.alwaysGenerateOnUpdate = true;
                                }
                                _pointLightsCookieMapsCollector.AddTexture(auraLight.cookieMapRenderTexture);
                                SetPointCookieMapsId();
                            }

                            if (OnRegisterPointLight != null)
                            {
                                OnRegisterPointLight(auraLight);
                            }
                        }
                    }
                    break;
            }

            auraLight.OnUninitialize += AuraLight_OnUninitialize;
        }

        /// <summary>
        /// Function called when the light is being disabled
        /// </summary>
        private void AuraLight_OnUninitialize(AuraLight auraLight, LightType typeBeforeUninitialize)
        {
            switch (typeBeforeUninitialize)
            {
                case LightType.Directional:
                    {
                        if (RegisteredDirectionalLightsList.Contains(auraLight))
                        {
                            if (_directionalLightsShadowMapsCollector != null && _directionalLightsShadowMapsCollector.RemoveTexture(auraLight.shadowMapRenderTexture))
                            {
                                _directionalLightsShadowDataCollector.RemoveTexture(auraLight.shadowDataRenderTexture);
                                SetDirectionalShadowMapsId();
                            }

                            if (_directionalLightsCookieMapsCollector != null && _directionalLightsCookieMapsCollector.RemoveTexture(auraLight.cookieMapRenderTexture))
                            {
                                SetDirectionalCookieMapsId();
                            }

                            RegisteredDirectionalLightsList.Remove(auraLight);
                        }
                    }
                    break;

                case LightType.Spot:
                    {
                        if (RegisteredSpotLightsList.Contains(auraLight))
                        {
                            if (_spotLightsShadowMapsCollector != null && _spotLightsShadowMapsCollector.RemoveTexture(auraLight.shadowMapRenderTexture))
                            {
                                SetSpotShadowMapsId();
                            }

                            if (_spotLightsCookieMapsCollector != null && _spotLightsCookieMapsCollector.RemoveTexture(auraLight.cookieMapRenderTexture))
                            {
                                SetSpotCookieMapsId();
                            }

                            RegisteredSpotLightsList.Remove(auraLight);

                            if (OnUnregisterSpotLight != null)
                            {
                                OnUnregisterSpotLight(auraLight);
                            }
                        }
                    }
                    break;

                case LightType.Point:
                    {
                        if (RegisteredPointLightsList.Contains(auraLight))
                        {
                            if (_pointLightsShadowMapsCollector != null && _pointLightsShadowMapsCollector.RemoveTexture(auraLight.shadowMapRenderTexture))
                            {
                                SetPointShadowMapsId();
                            }

                            if (_pointLightsCookieMapsCollector != null && _pointLightsCookieMapsCollector.RemoveTexture(auraLight.cookieMapRenderTexture))
                            {
                                SetPointCookieMapsId();
                            }

                            RegisteredPointLightsList.Remove(auraLight);

                            if (OnUnregisterPointLight != null)
                            {
                                OnUnregisterPointLight(auraLight);
                            }
                        }
                    }
                    break;
            }

            auraLight.OnUninitialize -= AuraLight_OnUninitialize;
        }

        /// <summary>
        /// Sets the id of the Aura light's shadowmap in the texture2DArray
        /// </summary>
        private void SetDirectionalShadowMapId(AuraLight auraLight)
        {
            if (auraLight.CastsShadows)
            {
                auraLight.SetShadowMapIndex(_directionalLightsShadowMapsCollector.GetTextureIndex(auraLight.shadowMapRenderTexture));
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's shadowmap in the texture2DArray
        /// </summary>
        private void SetDirectionalShadowMapsId()
        {
            for (int i = 0; i < RegisteredDirectionalLightsList.Count; ++i)
            {
                SetDirectionalShadowMapId(RegisteredDirectionalLightsList[i]);
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's cookiemap in the texture2DArray
        /// </summary>
        private void SetDirectionalCookieMapId(AuraLight auraLight)
        {
            if (auraLight.CastsCookie)
            {
                auraLight.SetCookieMapIndex(_directionalLightsCookieMapsCollector.GetTextureIndex(auraLight.cookieMapRenderTexture));
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's cookiemap in the texture2DArray
        /// </summary>
        private void SetDirectionalCookieMapsId()
        {
            for (int i = 0; i < RegisteredDirectionalLightsList.Count; ++i)
            {
                SetDirectionalCookieMapId(RegisteredDirectionalLightsList[i]);
            }
        }

        /// <summary>
        /// Sets the id of the Aura light's shadowmap in the texture2DArray
        /// </summary>
        private void SetSpotShadowMapId(AuraLight auraLight)
        {
            if (auraLight.CastsShadows)
            {
                auraLight.SetShadowMapIndex(_spotLightsShadowMapsCollector.GetTextureIndex(auraLight.shadowMapRenderTexture));
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's shadowmap in the texture2DArray
        /// </summary>
        private void SetSpotShadowMapsId()
        {
            for (int i = 0; i < RegisteredSpotLightsList.Count; ++i)
            {
                SetSpotShadowMapId(RegisteredSpotLightsList[i]);
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's cookiemap in the texture2DArray
        /// </summary>
        private void SetSpotCookieMapId(AuraLight auraLight)
        {
            if (auraLight.CastsCookie)
            {
                auraLight.SetCookieMapIndex(_spotLightsCookieMapsCollector.GetTextureIndex(auraLight.cookieMapRenderTexture));
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's cookiemap in the texture2DArray
        /// </summary>
        private void SetSpotCookieMapsId()
        {
            for (int i = 0; i < RegisteredSpotLightsList.Count; ++i)
            {
                SetSpotCookieMapId(RegisteredSpotLightsList[i]);
            }
        }

        /// <summary>
        /// Sets the id of the Aura light's shadowmap in the texture2DArray
        /// </summary>
        private void SetPointShadowMapId(AuraLight auraLight)
        {
            if (auraLight.CastsShadows)
            {
                auraLight.SetShadowMapIndex(_pointLightsShadowMapsCollector.GetTextureIndex(auraLight.shadowMapRenderTexture));
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's shadowmap in the texture2DArray
        /// </summary>
        private void SetPointShadowMapsId()
        {
            for (int i = 0; i < RegisteredPointLightsList.Count; ++i)
            {
                SetPointShadowMapId(RegisteredPointLightsList[i]);
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's cookiemap in the texture2DArray
        /// </summary>
        private void SetPointCookieMapId(AuraLight auraLight)
        {
            if (auraLight.CastsCookie)
            {
                auraLight.SetCookieMapIndex(_pointLightsCookieMapsCollector.GetTextureIndex(auraLight.cookieMapRenderTexture));
            }
        }

        /// <summary>
        /// Sets the id of each registered Aura light's cookiemap in the texture2DArray
        /// </summary>
        private void SetPointCookieMapsId()
        {
            for (int i = 0; i < RegisteredPointLightsList.Count; ++i)
            {
                SetPointCookieMapId(RegisteredPointLightsList[i]);
            }
        }

        /// <summary>
        /// Generates the Aura lights' shadow maps and cookie maps
        /// </summary>
        private void GenerateLightsMaps()
        {
            if (_directionalLightsShadowMapsCollector != null)
            {
                _directionalLightsShadowMapsCollector.Generate();
                _directionalLightsShadowDataCollector.Generate();
            }

            if (_directionalLightsCookieMapsCollector != null)
            {
                _directionalLightsCookieMapsCollector.Generate();
            }

            if (_spotLightsShadowMapsCollector != null)
            {
                _spotLightsShadowMapsCollector.Generate();
            }

            if (_spotLightsCookieMapsCollector != null)
            {
                _spotLightsCookieMapsCollector.Generate();
            }

            if (_pointLightsShadowMapsCollector != null)
            {
                _pointLightsShadowMapsCollector.Generate();
            }

            if (_pointLightsCookieMapsCollector != null)
            {
                _pointLightsCookieMapsCollector.Generate();
            }
        }
        #endregion
    }
}
