
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
using UnityEngine.Profiling;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Main component to assign on a GameObject with a Camera component
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Aura 2/Aura Camera", 0)]
    [ExecuteInEditMode]
    #if AURA_IN_SCENEVIEW
    [ImageEffectAllowedInSceneView]
    #endif
    public class AuraCamera : MonoBehaviour
    {
        #region Public members
        /// <summary>
        /// Settings of the frustum
        /// </summary>
        public FrustumSettings frustumSettings;
        ///// <summary>
        ///// Enabled the volumetric lighting to be applied as a Post Process 
        ///// </summary>
        //public bool applyAsPostProcess = true;
        #endregion

        #region Private members
        /// <summary>
        /// Shader used for applying the volumetric lighting as Post Process
        /// </summary>
        private Shader _postProcessShader;
        /// <summary>
        /// Texture containing blue noise for dithering volumetric lighting
        /// </summary>
        private Texture2DArray _blueNoiseTexturesArray;
        /// <summary>
        /// The frustum inside which the data will be computed
        /// </summary>
        public Frustum _frustum;
        /// <summary>
        /// The camera component which this current component is attached to
        /// </summary>
        private Camera _cameraComponent;
        /// <summary>
        /// The material used for applying the volumetric lights as Post Process
        /// </summary>
        private Material _postProcessMaterial;
        #if UNITY_EDITOR 
        /// <summary>
        /// Custom time used during edition
        /// </summary>
        private static double _editorTime;
        /// <summary>
        /// Custom delta time used during edition
        /// </summary>
        private static float _editorDeltaTime;
        #endif
        /// <summary>
        /// The manager in charge of registering Aura lights and collecting their common (unculled) data
        /// </summary>
        private static CommonDataManager _commonDataManager;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Camera component
        /// </summary>
        public Camera CameraComponent
        {
            get
            {
                if (_cameraComponent == null)
                {
                    _cameraComponent = GetComponent<Camera>();
                }

                return _cameraComponent;
            }
        }

        /// <summary>
        /// The manager in charge of registering Aura lights and collecting their common (unculled) data
        /// </summary>
        public static CommonDataManager CommonDataManager
        {
            get
            {
                if (_commonDataManager == null)
                {
                    _commonDataManager = new CommonDataManager();
                }

                return _commonDataManager;
            }
        }

        /// <summary>
        /// The frame count since the begining
        /// </summary>
        public int FrameId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the elapsed Time 
        /// </summary>
        public static float Time
        {
            get
            {
#if !UNITY_EDITOR
                return UnityEngine.Time.time;
#else
                return (float)_editorTime;
#endif
            }
        }

        /// <summary>
        /// Gets the current Delta Time 
        /// </summary>
        public static float DeltaTtime
        {
            get
            {
                #if !UNITY_EDITOR
                return UnityEngine.Time.deltaTime;
                #else
                return _editorDeltaTime;
                #endif
            }
        }

        /// <summary>
        /// Tells if the component has succesfully been initialized
        /// </summary>
        public bool IsInitialized
        {
            get;
            private set;
        }
        #endregion

        #region Monobehavious functions
        private void OnEnable()
        {
            if (!Aura.IsCompatible)
            {
                enabled = false;
                return;
            }
            
            Initialize();
        }

        private void OnDisable()
        {
            try // Weird managed resource error when building or loading a scene
            {
                Uninitialize();
            }
            catch
            {
            }
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            try // Weird managed resource error when building or loading a scene
            {
                if (IsInitialized)
                {
                    #if UNITY_EDITOR
                    UpdateEditorTimeData();
                    #endif
                
                    CommonDataManager.UpdateData();

                    Shader.SetGlobalInt("_frameID", FrameId);

                    _frustum.ComputeData();

                    ++FrameId;
                //}
                //
                //if (IsInitialized && applyAsPostProcess)
                //{
                    Profiler.BeginSample("Aura 2 : Apply as post process");
                    Graphics.Blit(src, dest, _postProcessMaterial);
                    Profiler.EndSample();
                }
                else
                {
                    Graphics.CopyTexture(src, dest);
                }
            }
            catch
            {
                Graphics.CopyTexture(src, dest);
            }

            GL.Flush();
        }
        #endregion

        #region Functions
#if UNITY_EDITOR 
        /// <summary>
        /// Updates the custom Time and DeltaTime and repaints the GameView if focused
        /// </summary>
        private void UpdateEditorTimeData()
        {
            double currentTime = EditorApplication.timeSinceStartup;
            _editorDeltaTime = (float)(currentTime - Time);
            _editorTime = currentTime;
            //editorTime += 1.0f/60.0f; //For recorder fixed framerate
        }
#endif

        /// <summary>
        /// Sets a new frustum grid resolution
        /// </summary>
        /// <param name="resolution">The new resolution</param>
        public void SetFrustumGridResolution(Vector3Int resolution)
        {
            _frustum.SetFrustumGridResolution(resolution);
        }

        /// <summary>
        /// Sets a new grid resolution for light probes proxy
        /// </summary>
        /// <param name="resolution">The new resolution</param>
        public void SetLightProbesProxyGridResolution(Vector3Int resolution)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize the Aura component and its variables
        /// </summary>
        private void Initialize()
        {
            InitializeResources();
            
            CameraComponent.depthTextureMode |= DepthTextureMode.Depth;

            if(frustumSettings == null)
            {
                frustumSettings = new FrustumSettings();
            }

            _frustum = new Frustum(frustumSettings, _cameraComponent, this);

            _postProcessMaterial = new Material(_postProcessShader);
            Shader.SetGlobalTexture("_blueNoiseTexturesArray", _blueNoiseTexturesArray);

            Shader.EnableKeyword("AURA");

            IsInitialized = true;
        }

        /// <summary>
        /// Uninitialize the Aura component and dispose its managed variables
        /// </summary>
        private void Uninitialize()
        {
            if (IsInitialized)
            {
                Shader.DisableKeyword("AURA");

                _frustum.Dispose();
                _frustum = null;

                CommonDataManager.Dispose();

                IsInitialized = false;
            }
        }

        /// <summary>
        /// Initialize the needed resources
        /// </summary>
        private void InitializeResources()
        {
            _postProcessShader = Aura.ResourcesCollection.postProcessShader;
            _blueNoiseTexturesArray = Aura.ResourcesCollection.blueNoiseTextureArray;
        }
        #endregion

        #region GameObject constructor
        /// <summary>
        /// Method allowing to quickly build GameObjects with the component assigned
        /// </summary>
        /// <param name="name">The name of the new GameObject</param>
        /// <returns>The created AuraCamera gameObject</returns>
        public static GameObject CreateGameObject(string name)
        {
            GameObject newGameObject = new GameObject(name);
            newGameObject.AddComponent<Camera>();
            newGameObject.AddComponent<AuraCamera>();

            return newGameObject;
        }

#if UNITY_EDITOR

        /// <summary>
        /// Method allowing to add a MenuItem to quickly build GameObjects with the component assigned
        /// </summary>
        /// <param name="menuCommand">Stuff that Unity automatically fills with some editor's contextual infos</param>
        /// <param name="name">The name of the new GameObject</param>
        /// <returns>The created AuraCamera gameObject</returns>
        private static GameObject CreateGameObject(MenuCommand menuCommand, string name)
        {
            GameObject newGameObject = CreateGameObject(name);

            float offset = 1.5f;
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
        /// Creates a new Aura Camera
        /// </summary>
        /// <param name="menuCommand">Stuff that Unity automatically fills with some editor's contextual infos</param>
        [MenuItem("GameObject/Aura 2/Camera", false, 0)]
        private static void CreateDirectionalGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Camera");
        }
        #endif
        #endregion
    }
    
    #region Gizmo
    #if UNITY_EDITOR
    /// <summary>
    /// Custom Gizmo drawer for AuraCamera component
    /// </summary>
    public class AuraCameraGizmoDrawer
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NonSelected | GizmoType.NotInSelectionHierarchy | GizmoType.Selected)]
        static void DrawGizmoForAuraCamera(AuraCamera component, GizmoType gizmoType)
        {
            if (!AuraEditorPrefs.DisplayGizmosOnCameras)
            {
                return;
            }

            bool isFaded = (int)gizmoType == (int)GizmoType.NonSelected || (int)gizmoType == (int)GizmoType.NotInSelectionHierarchy || (int)gizmoType == (int)GizmoType.NonSelected + (int)GizmoType.NotInSelectionHierarchy;

            if(isFaded && !AuraEditorPrefs.DisplayGizmosWhenUnselected || !isFaded && !AuraEditorPrefs.DisplayGizmosWhenSelected)
            {
                return;
            }

            float opacity = isFaded ? 0.5f : 1.0f;

            Camera camera = component.GetComponent<Camera>();
            Matrix4x4 tmp = Gizmos.matrix;
            Gizmos.matrix = camera.transform.localToWorldMatrix;
            Gizmos.color = CustomGizmo.color * new Color(1.0f, 1.0f, 1.0f, opacity);
            Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, component.frustumSettings.qualitySettings.farClipPlaneDistance, 0, camera.aspect);
            Gizmos.matrix = tmp;
            
            if( !isFaded && AuraEditorPrefs.DisplayCameraSlicesInEdition)
            {
                for( int i = 1; i < component.frustumSettings.qualitySettings.frustumGridResolution.z; ++i)
                {
                    float sliceRatio = (float)i / component.frustumSettings.qualitySettings.frustumGridResolution.z;
                    sliceRatio = 1.0f - Mathf.Pow(1.0f - sliceRatio, component.frustumSettings.qualitySettings.depthBiasCoefficient);
                    float distance = sliceRatio * component.frustumSettings.qualitySettings.farClipPlaneDistance;
                    Vector2 frustumSize = camera.GetFrustumSizeAtDistance(distance);
                    Matrix4x4 matrix = Matrix4x4.TRS(component.transform.position + component.transform.forward * distance, component.transform.rotation * Quaternion.Euler(90,0,0), new Vector3(frustumSize.x, 1, frustumSize.y));
                    CustomGizmo.DrawSquare(matrix, CustomGizmo.color * new Color(1.0f, 1.0f, 1.0f, 0.5f), CustomGizmo.pixelWidth);
                }
            }
        }
    }
    #endif
    #endregion
}
