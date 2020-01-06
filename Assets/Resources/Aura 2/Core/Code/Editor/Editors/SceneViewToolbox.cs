
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
using UnityEditor;

namespace Aura2API
{
    /// <summary>
    /// Autoloading class that will setup the toolbox in the SceneView
    /// </summary>
    [InitializeOnLoad]
    public class SceneViewToolbox
    {
        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        static SceneViewToolbox()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneViewGUI;
            SceneView.duringSceneGui += OnSceneViewGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
            SceneView.onSceneGUIDelegate += OnSceneViewGUI;
#endif
        }
        #endregion

        #region Private members
        /// <summary>
        /// Tells if the resources are initialized
        /// </summary>
        private static bool _isInitialized;
        /// <summary>
        /// The size of the axis cube on the upper right
        /// </summary>
        private const int _axisCubeSize = 128;
        /// <summary>
        /// The fixe margin
        /// </summary>
        private const int _margin = 10;
        /// <summary>
        /// The width of the toolbox
        /// </summary>
        private const int _toolboxWidth = 64;
        /// <summary>
        /// The width of the toolbox toggle display button
        /// </summary>
        private const int _toolboxToggleDisplayButtonWidth = _margin;
        /// <summary>
        /// The current weight of the toolbox apparition effect
        /// </summary>
        private static float _toolboxApparitionWeight = IsVisible ? 1.0f : 0.0f;
        /// <summary>
        /// The current weight of the toolbox expansion effect
        /// </summary>
        private static float _toolboxExpansionWeight = IsExpanded ? 1.0f : 0.0f;
        /// <summary>
        /// The width of the presets window
        /// </summary>
        private const int _presetsWindowWidth = 384-16/*Borders aspect*/+15/*Scrollbar width*/;
        /// <summary>
        /// Tells if presets window should be shown
        /// </summary>
        private static bool _showPresets;
        /// <summary>
        /// The current weight of the presets window apparition effect
        /// </summary>
        private static float _presetsApparitionWeight = 0.0f;
        /// <summary>
        /// The last frame timestamp
        /// </summary>
        private static float _timestamp;
        /// <summary>
        /// The last frame delta time
        /// </summary>
        private static float _deltaTime;
        /// <summary>
        /// The size of the buttons
        /// </summary>
        private const int _buttonWidth = _toolboxWidth / 2;
        /// <summary>
        /// The height of the toolbox
        /// </summary>
        private const int _toolboxHeight = 364;
        /// <summary>
        /// Width of the activation toggle button;
        /// </summary>
        private const int _activationToggleButtonWidth = 136;
        /// <summary>
        /// Height of the activation toggle button;
        /// </summary>
#if UNITY_2019_3_OR_NEWER
        private const int _activationToggleButtonHeight = 20;
#else
        private const int _activationToggleButtonHeight = 17;
#endif
        /// <summary>
        /// The current sceneView
        /// </summary>
        private static SceneView _currentSceneView;
        /// <summary>
        /// The current scene view event
        /// </summary>
        private static Event _sceneViewEvent;
        /// <summary>
        /// Duration after notifications will be removed
        /// </summary>
        private const float _notificationsMaxDuration = 0.75f;
        /// <summary>
        /// Tells if there a notification has been displayed
        /// </summary>
        private static bool _isNotificationDisplayed;
        /// <summary>
        /// Timestamp of the last notification
        /// </summary>
        private static float _notificationsTimestamp;
        /// <summary>
        /// Rect of the toggle display button
        /// </summary>
        private static Rect _displayToggleButtonsRect;
        /// <summary>
        /// Rect of the toggle display button
        /// </summary>
        private static Rect _activationToggleButtonRect;
        /// <summary>
        /// Rect of the toolbox window
        /// </summary>
        private static Rect _toolboxWindowRect;
        /// <summary>
        /// Rect of the presets window
        /// </summary>
        private static Rect _presetsWindowRect;
        /// <summary>
        /// Scroll of the presets window
        /// </summary>
        private static Vector2 _presetsWindowScroll;
        /// <summary>
        /// Content for the SCENE panel
        /// </summary>
        private static GUIContent _scenePanelTitleContent;
        /// <summary>
        /// Content for the apply preset button
        /// </summary>
        private static GUIContent _presetButtonContent;
        /// <summary>
        /// Content for the add Aura to cameras button
        /// </summary>
        private static GUIContent _addAuraToCamerasButtonContent;
        /// <summary>
        /// Content for the add Aura to directionals button
        /// </summary>
        private static GUIContent _addAuraToDirectionalLightsButtonContent;
        /// <summary>
        /// Content for the add Aura to spots button
        /// </summary>
        private static GUIContent _addAuraToSpotLightsButtonContent;
        /// <summary>
        /// Content for the add Aura to points button
        /// </summary>
        private static GUIContent _addAuraToPointLightsButtonContent;
        //private static GUIContent _modifyPanelTitleContent;
        //private static GUIContent _addButtonContent;
        //private static GUIContent _removeButtonContent;
        //private static GUIContent _toggleButtonContent;
        //private static GUIContent _undefinedButtonContent;
        /// <summary>
        /// Content for the CREATE panel
        /// </summary>
        private static GUIContent _createPanelTitleContent;
        /// <summary>
        /// Screenspace pixel offset for labels
        /// </summary>
        private static Vector2 _labelPixelsOffset = new Vector2(25.0f, 0.0f);
        /// <summary>
        /// The space used for the volume creation
        /// </summary>
        private static ToolboxCreationSpace _creationSpace = ToolboxCreationSpace.World;
        /// <summary>
        /// The content of the creation space button
        /// </summary>
        private static GUIContent[] _creationSpaceContent;
        /// <summary>
        /// The content of the focus after creation button
        /// </summary>
        private static GUIContent _focusAfterCreationToggleContent;
        /// <summary>
        /// Enables the focus on the created volume after its creation
        /// </summary>
        private static bool _focusAfterCreation;
        /// <summary>
        /// The volume creation buttons
        /// </summary>
        private static GUIContent[] _creationTypeButtonsContent;
        /// <summary>
        /// Is currently in creation mode
        /// </summary>
        private static bool _isCreating;
        /// <summary>
        /// the tpe of object currently being created
        /// </summary>
        private static ToolboxCreationType _creationType;
        /// <summary>
        /// Has a new object been created
        /// </summary>
        private static bool _isNewObjectCreated;
        /// <summary>
        /// The created new object
        /// </summary>
        private static GameObject _createdObject;
        /// <summary>
        /// The type of volume created
        /// </summary>
        private static VolumeType _volumeType;
        /// <summary>
        /// The type of light created
        /// </summary>
        private static LightType _lightType;
        /// <summary>
        /// The current click count
        /// </summary>
        private static int _clickCount;
        /// <summary>
        /// Temporary normal vector used for creation
        /// </summary>
        private static Vector3 _clickNormal;
        /// <summary>
        /// Temporary position used for creation
        /// </summary>
        private static Vector3 _clickPositionA;
        /// <summary>
        /// Temporary position used for creation
        /// </summary>
        private static Vector3 _clickPositionB;
        /// <summary>
        /// Temporary position used for creation
        /// </summary>
        private static Vector3 _clickPositionC;
        /// <summary>
        /// Temporary matric used for creation
        /// </summary>
        private static Matrix4x4 _matrix;
        /// <summary>
        /// Temporary right direction used for creation
        /// </summary>
        private static Vector3 _rightVector;
        /// <summary>
        /// Temporary up direction used for creation
        /// </summary>
        private static Vector3 _upVector;
        /// <summary>
        /// Temporary forward direction used for creation
        /// </summary>
        private static Vector3 _forwardVector;
        /// <summary>
        /// Temporary position used for creation
        /// </summary>
        private static Vector3 _pointC;
        /// <summary>
        /// Content of the DEBUG panel
        /// </summary>
        private static GUIContent _debugPanelTitleContent;
        /// <summary>
        /// Content of the debug volumetric lighting buffer button
        /// </summary>
        private static GUIContent _debugVolumetricLightingButtonContent;
        /// <summary>
        /// Content of the debug occlusion culling button
        /// </summary>
        private static GUIContent _debugOcclusionCullingButtonContent;
        /// <summary>
        /// Custom control ID for each window;
        /// </summary>
        private static int _controlID = 131071; // random start id to avoid conflicts with other windows
        #endregion

        #region Properties
        /// <summary>
        /// Screen mouse position
        /// </summary>
        private static Vector2 MousePosition
        {
            get
            {
                return _sceneViewEvent.mousePosition;
            }
        }
        /// <summary>
        /// Is the left click pressed
        /// </summary>
        private static bool LeftClick
        {
            get
            {
                return _sceneViewEvent.isMouse && _sceneViewEvent.type == EventType.MouseDown && _sceneViewEvent.button == 0;
            }
        }
        /// <summary>
        /// Is the right click pressed
        /// </summary>
        private static bool RightClick
        {
            get
            {
                return _sceneViewEvent.isMouse && _sceneViewEvent.type == EventType.MouseDown && _sceneViewEvent.button == 1;
            }
        }
        /// <summary>
        /// Is the escape button pressed
        /// </summary>
        private static bool EscapeButton
        {
            get
            {
                return _sceneViewEvent.isKey && _sceneViewEvent.type == EventType.KeyDown && _sceneViewEvent.keyCode == KeyCode.Escape;
            }
        }

        /// <summary>
        /// Tells if the toolbox should be visible
        /// </summary>
        public static bool IsVisible
        {
            get
            {
                return AuraEditorPrefs.DisplayToolbox;
            }
        }

        /// <summary>
        /// Tells if the toolbox should be expanded
        /// </summary>
        public static bool IsExpanded
        {
            get
            {
                return AuraEditorPrefs.ExpandToolbox;
            }
        }

        /// <summary>
        /// The amount of Presets' preview per row
        /// </summary>
        public static int PresetsPreviewsPerRow
        {
            get
            {
                return AuraEditorPrefs.ToolboxPresetsPreviewsPerRow;
            }
            set
            {
                AuraEditorPrefs.ToolboxPresetsPreviewsPerRow = value;
            }
        }

        /// <summary>
        /// Tells if the notifications should be displayed
        /// </summary>
        public static bool DisplayNotifications
        {
            get
            {
                return AuraEditorPrefs.ShowToolboxNotifications;
            }
            set
            {
                AuraEditorPrefs.ShowToolboxNotifications = value;
            }
        }

        /// <summary>
        /// Tells if the toolbox should be animated
        /// </summary>
        public static bool EnableAnimations
        {
            get
            {
                return AuraEditorPrefs.EnableToolboxAnimations;
            }
            set
            {
                AuraEditorPrefs.EnableToolboxAnimations = value;
            }
        }

        private static bool ShouldRepaintSceneView
        {
            get
            {
                return _toolboxExpansionWeight > 0.0f || _toolboxExpansionWeight < 1.0f
                    || _toolboxApparitionWeight > 0.0f || _toolboxApparitionWeight < 1.0f;
            }
        }

        /// <summary>
        /// The toolbox height taking into account the option to display the DEBUG panel
        /// </summary>
        private static int ToolboxHeight
        {
            get
            {
                return _toolboxHeight + (AuraEditorPrefs.DisplayDebugPanelInToolbox ? 85 : 0);
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Called on every scene view update
        /// </summary>
        /// <param name="sceneView">The current update scene view</param>
        private static void OnSceneViewGUI(SceneView sceneView)
        {
            try
            {
                _sceneViewEvent = Event.current;
                _currentSceneView = sceneView;
            
                _deltaTime = Time.realtimeSinceStartup - _timestamp;
                _timestamp = Time.realtimeSinceStartup;

                CheckToRemoveNotification();

                ComputeRects();

                _controlID = 0;

                if (IsVisible || _toolboxApparitionWeight > 0.0f || IsExpanded || _toolboxExpansionWeight > 0.0f)
                {
                    if(!_isInitialized)
                    {
                        Initialize(sceneView);
                    }

            
                    if (_isCreating)
                    {
                        if(_isNewObjectCreated)
                        {
                            Selection.activeGameObject = _createdObject;
                        }
                        else
                        {
                            Selection.activeGameObject = null;
                        }
            
                        if (_sceneViewEvent.type != EventType.Layout)
                        {
                            if(CheckForAbortCreating())
                            {
                                return;
                            }

                            switch (_creationType)
                            {
                                case ToolboxCreationType.Camera:
                                    {
                                        CreateCamera(sceneView, _sceneViewEvent);
                                    }
                                    break;

                                case ToolboxCreationType.Light:
                                    {
                                        CreateLight(sceneView, _sceneViewEvent);
                                    }
                                    break;

                                case ToolboxCreationType.Volume:
                                    {
                                        CreateVolume(sceneView, _sceneViewEvent);
                                    }
                                    break;
                            }
                        }
                    }

                    if (_sceneViewEvent.type == EventType.Layout)
                    {
                        DrawToolbox();
                        DrawPresetsWindow();
                    }
                }

                if (_sceneViewEvent.type == EventType.Layout)
                {
                    DrawActivationToggleButton();
                    DrawDisplayToggleButton();
                }

                if (ShouldRepaintSceneView)
                {
                    sceneView.Repaint();
                }
            }
            catch
            {
#if UNITY_2019_1_OR_NEWER
                SceneView.duringSceneGui -= OnSceneViewGUI;
#else
                SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
#endif
            }
        }

        /// <summary>
        /// Initializes the resources
        /// </summary>
        /// <param name="sceneView">The current update scene view</param>
        private static void Initialize(SceneView sceneView)
        {
            int activationButtonXPosition = 256;
#if UNITY_2019_1_OR_NEWER
            activationButtonXPosition += 32;
#if UNITY_2019_3_OR_NEWER
            activationButtonXPosition += 48;
#endif
#endif
            _activationToggleButtonRect = new Rect(activationButtonXPosition, 0, _activationToggleButtonWidth, _activationToggleButtonHeight);
            _showPresets = false;
            _presetsApparitionWeight = 0.0f;

            _scenePanelTitleContent = new GUIContent("SCENE", "Globally converts objects to AURA with this panel");
            _presetButtonContent = new GUIContent(Aura.ResourcesCollection.displayPresetsButtonTexture, "Display Presets");
            _addAuraToCamerasButtonContent = new GUIContent("Convert Cameras", Aura.ResourcesCollection.cameraIconTexture, "Convert Cameras to Aura");
            _addAuraToDirectionalLightsButtonContent = new GUIContent("Convert Directional Lights", Aura.ResourcesCollection.directionalLightIconTexture, "Convert Directional Lights to Aura");
            _addAuraToSpotLightsButtonContent = new GUIContent("Convert Spot Lights", Aura.ResourcesCollection.spotLightIconTexture, "Convert Spot Lights to Aura");
            _addAuraToPointLightsButtonContent = new GUIContent("Convert Point Lights", Aura.ResourcesCollection.pointLightIconTexture, "Convert Point Lights to Aura");

            //_modifyPanelTitleContent = new GUIContent("MODIFY", "ADD / REMOVE / TOGGLE Aura Components on selected objects with this panel");
            //_addButtonContent = new GUIContent("Add AURA", Aura.AuraResourcesCollection.addIconTexture, "Add AURA Component to selected object");
            //_removeButtonContent = new GUIContent("Remove AURA", Aura.AuraResourcesCollection.removeIconTexture, "Remove AURA Component from selected object");
            //_toggleButtonContent = new GUIContent("Toggle AURA", Aura.AuraResourcesCollection.toggleIconTexture, "Toggle AURA on selected objects");
            //_undefinedButtonContent = new GUIContent("Undefined expected behaviour", Aura.AuraResourcesCollection.questionIconTexture, "Undefined expected behaviour");

            _createPanelTitleContent = new GUIContent("CREATE", "CREATE New Aura Objects with this panel");
            _creationSpace = ToolboxCreationSpace.World;
            string creationSpaceTooltip = "VOLUME CREATION SPACE (Click to change)";
            _creationSpaceContent = new GUIContent[3];
            _creationSpaceContent[0] = new GUIContent("In WorldSpace", Aura.ResourcesCollection.creationSpaceButtonWorldTexture, creationSpaceTooltip);
            _creationSpaceContent[1] = new GUIContent("Along Normal", Aura.ResourcesCollection.creationSpaceButtonNormalTexture, creationSpaceTooltip);
            _creationSpaceContent[2] = new GUIContent("On Topology", Aura.ResourcesCollection.creationSpaceButtonTopologyTexture, creationSpaceTooltip);
            _focusAfterCreationToggleContent = new GUIContent("Toggle Focus", Aura.ResourcesCollection.focusIconTexture, "Focus on Objects after creation");
            _creationTypeButtonsContent = new GUIContent[10];
            _creationTypeButtonsContent[0] = new GUIContent("Global Volume", Aura.ResourcesCollection.creationTypeButtonGlobalTexture, "Create a global Volume");
            _creationTypeButtonsContent[1] = new GUIContent("Layer Volume", Aura.ResourcesCollection.creationTypeButtonLayerTexture, "Create a layer Volume");
            _creationTypeButtonsContent[2] = new GUIContent("Box Volume", Aura.ResourcesCollection.creationTypeButtonBoxTexture, "Create a Volume with the shape of a Box");
            _creationTypeButtonsContent[3] = new GUIContent("Sphere Volume", Aura.ResourcesCollection.creationTypeButtonSphereTexture, "Create a Volume with the shape of a Sphere");
            _creationTypeButtonsContent[4] = new GUIContent("Cylinder Volume", Aura.ResourcesCollection.creationTypeButtonCylinderTexture, "Create a Volume with the shape of a Cylinder");
            _creationTypeButtonsContent[5] = new GUIContent("Cone Volume", Aura.ResourcesCollection.creationTypeButtonConeTexture, "Create a Volume with the shape of a Cone");
            _creationTypeButtonsContent[6] = new GUIContent("Aura Camera", Aura.ResourcesCollection.cameraIconTexture, "Create a Camera with Aura applied");
            _creationTypeButtonsContent[7] = new GUIContent("Aura Directional Light", Aura.ResourcesCollection.directionalLightIconTexture, "Create a Directional Light with Aura applied");
            _creationTypeButtonsContent[8] = new GUIContent("Aura Spot Light", Aura.ResourcesCollection.spotLightIconTexture, "Create a Spot Light with Aura applied");
            _creationTypeButtonsContent[9] = new GUIContent("Aura Point Light", Aura.ResourcesCollection.pointLightIconTexture, "Create a Point Light with Aura applied");

            _debugPanelTitleContent = new GUIContent("DEBUG", "DEBUG view Aura's data with this panel");
            _debugVolumetricLightingButtonContent = new GUIContent("Buffer", "Debug the volumetric lighting buffer");
            _debugOcclusionCullingButtonContent = new GUIContent("Culling", "Debug the occlusion culling");

            sceneView.autoRepaintOnSceneChange = true;

            _isInitialized = true;
        }

        #region GUI
        /// <summary>
        /// Computes the rects of the displayed areas
        /// </summary>
        private static void ComputeRects()
        {   
            Rect toolbarRect = Rect.zero;
            if (AuraEditorPrefs.ToolboxPosition == 0)
            {
                toolbarRect = new Rect(_margin, (int)(_currentSceneView.position.height / 2) - (int)(ToolboxHeight / 2), _toolboxWidth, ToolboxHeight);
            }
            else
            {
                toolbarRect = new Rect(_currentSceneView.position.width - _margin - _toolboxWidth, Mathf.Max(_margin + _axisCubeSize, (int)(_currentSceneView.position.height / 2) - (int)(ToolboxHeight / 2)), _toolboxWidth, ToolboxHeight);
            }

            float targetPos;
            float startingPosition;
            AnimationCurve curve;
            float weightFromCurve;

            targetPos = toolbarRect.y;
            startingPosition = _currentSceneView.position.height;
            if (EnableAnimations)
            {
                _toolboxApparitionWeight += _deltaTime / (IsVisible ? Aura.ResourcesCollection.customWindowsInMotionDuration : Aura.ResourcesCollection.customWindowsOutMotionDuration) * (IsVisible ? 1.0f : -1.0f);
                _toolboxApparitionWeight = Mathf.Clamp01(_toolboxApparitionWeight);
                curve = IsVisible ? Aura.ResourcesCollection.customWindowsInMotionCurve : Aura.ResourcesCollection.customWindowsOutMotionCurve;
                weightFromCurve = curve.Evaluate(_toolboxApparitionWeight);
                toolbarRect.y = Mathf.LerpUnclamped(startingPosition, targetPos, weightFromCurve);
            }
            else
            {
                _toolboxApparitionWeight = IsVisible ? 1.0f : 0.0f;
                toolbarRect.y = IsVisible ? targetPos : startingPosition;
            }

            // Toggle button
            _displayToggleButtonsRect = toolbarRect;
            _displayToggleButtonsRect.width = _toolboxToggleDisplayButtonWidth;
            if (AuraEditorPrefs.ToolboxPosition == 0)
            {
                _displayToggleButtonsRect.x -= _toolboxToggleDisplayButtonWidth;
            }
            else
            {
                _displayToggleButtonsRect.x += _toolboxWidth;
            }

            // Toolbox
            _toolboxWindowRect = toolbarRect;
            targetPos = _toolboxWindowRect.x;
            startingPosition = AuraEditorPrefs.ToolboxPosition == 0 ? -_toolboxWidth - _toolboxToggleDisplayButtonWidth : _currentSceneView.position.width + _toolboxWidth;
            if (EnableAnimations)
            {
                _toolboxExpansionWeight += _deltaTime / (IsExpanded ? Aura.ResourcesCollection.customWindowsInMotionDuration : Aura.ResourcesCollection.customWindowsOutMotionDuration) * (IsExpanded ? 1.0f : -1.0f);
                _toolboxExpansionWeight = Mathf.Clamp01(_toolboxExpansionWeight);
                curve = IsExpanded ? Aura.ResourcesCollection.customWindowsInMotionCurve : Aura.ResourcesCollection.customWindowsOutMotionCurve;
                weightFromCurve = curve.Evaluate(_toolboxExpansionWeight);
                _toolboxWindowRect.x = Mathf.LerpUnclamped(startingPosition, targetPos, weightFromCurve);
            }
            else
            {
                _toolboxExpansionWeight = IsExpanded ? 1.0f : 0.0f;
                _toolboxWindowRect.x = IsExpanded ? targetPos : startingPosition;
            }

            // Presets
            _presetsWindowRect = toolbarRect;
            _presetsWindowRect.x = _presetsWindowRect.x + (AuraEditorPrefs.ToolboxPosition == 0 ? _presetsWindowRect.width : -_presetsWindowWidth);
            _presetsWindowRect.width = _presetsWindowWidth;
            targetPos = _presetsWindowRect.x;
            startingPosition = AuraEditorPrefs.ToolboxPosition == 0 ? -_presetsWindowWidth : _currentSceneView.position.width + _presetsWindowWidth;
            if (EnableAnimations)
            {
                _presetsApparitionWeight += _deltaTime / (_showPresets ? Aura.ResourcesCollection.customWindowsInMotionDuration : Aura.ResourcesCollection.customWindowsOutMotionDuration) * (_showPresets ? 1.0f : -1.0f);
                _presetsApparitionWeight = Mathf.Clamp01(_presetsApparitionWeight);
                curve = _showPresets ? Aura.ResourcesCollection.customWindowsInMotionCurve : Aura.ResourcesCollection.customWindowsOutMotionCurve;
                weightFromCurve = curve.Evaluate(_presetsApparitionWeight * _toolboxExpansionWeight);
                _presetsWindowRect.x = Mathf.LerpUnclamped(startingPosition, targetPos, weightFromCurve);
            }
            else
            {
                _presetsApparitionWeight = _showPresets ? 1.0f : 0.0f;
                _presetsWindowRect.x = _showPresets ? targetPos : startingPosition;
            }

            if (_showPresets)
            {
                Rect cumulatedRects = AuraEditorPrefs.ToolboxPosition == 0 ? new Rect(_displayToggleButtonsRect.x, _displayToggleButtonsRect.y, _displayToggleButtonsRect.width + _toolboxWindowRect.width + _presetsWindowRect.width, _toolboxHeight) : new Rect(_presetsWindowRect.x, _presetsWindowRect.y, _displayToggleButtonsRect.width + _toolboxWindowRect.width + _presetsWindowRect.width, _toolboxHeight);
                if(LeftClick && !cumulatedRects.Contains(MousePosition))
                {
                    _showPresets = !_showPresets;
                }
            }
        }

        /// <summary>
        /// Draws the toolbox bed
        /// </summary>
        /// <param name="sceneView"></param>
        private static void DrawToolbox()
        {
            GUILayout.Window(++_controlID, _toolboxWindowRect, DrawToolboxBody, "", GuiStyles.ButtonNoHoverNoBorder);
        }

        /// <summary>
        /// Draws the toolbox body
        /// </summary>
        /// <param name="controlId"></param>
        private static void DrawToolboxBody(int controlId)
        {
            int pixelsVerticalOffset = 0;
            GUILayout.BeginVertical();

            GUILayout.BeginArea(new Rect(0, 0, _toolboxWidth, ToolboxHeight));
            GUILayout.BeginVertical();

            ////////////////////////// SCENE PANEL

            GUILayout.Label(_scenePanelTitleContent, GuiStyles.ButtonBoldNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(_buttonWidth));
            pixelsVerticalOffset += _buttonWidth;

            EditorGUI.BeginDisabledGroup(_isCreating);

            Rect presetsButtonRect = EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button(_presetButtonContent, _showPresets ? GuiStyles.ButtonPressedImageOnlyNoBorder : GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(_buttonWidth)))
            {
                _showPresets = !_showPresets;
                ShowNotification(new GUIContent((_showPresets ? " Showing" : " Hiding") + " Aura Ambience Presets", Aura.ResourcesCollection.presetUiIconTexture));
                _sceneViewEvent.Use();
            }
            EditorGUILayout.EndHorizontal();
            pixelsVerticalOffset += (int)presetsButtonRect.height;

            GuiHelpers.DrawSeparator(new Color(0, 0, 0, 0), new Rect(0, pixelsVerticalOffset, _toolboxWidth, 0), 4, 4, 2);
            pixelsVerticalOffset += 4;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_addAuraToCamerasButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_toolboxWidth / 2), GUILayout.Height(_buttonWidth)))
            {
                AddAuraToCameras();
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_addAuraToDirectionalLightsButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_toolboxWidth / 2), GUILayout.Height(_buttonWidth)))
            {
                AddAuraToDirectionalLights();
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += _buttonWidth;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_addAuraToSpotLightsButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_toolboxWidth / 2), GUILayout.Height(_buttonWidth)))
            {
                AddAuraToSpotLights();
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_addAuraToPointLightsButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_toolboxWidth / 2), GUILayout.Height(_buttonWidth)))
            {
                AddAuraToPointLights();
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += _buttonWidth;

            GuiHelpers.DrawSeparator(Color.HSVToRGB(0.0f, 0.0f, 0.2f), new Rect(0, pixelsVerticalOffset, _toolboxWidth, 0), 4, 4, 2);
            pixelsVerticalOffset += 4;

            EditorGUI.EndDisabledGroup();

            /*
            ////////////////////////// MODIFY PANEL

            GUILayout.Label(_modifyPanelTitleContent, GuiStyles.ButtonBoldNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(_buttonWidth));
            pixelsVerticalOffset += _buttonWidth;

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(Selection.gameObjects.Length != 1);
            if(Selection.gameObjects.Length != 1)
            {
                GUILayout.Button(_undefinedButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth));
                _sceneViewEvent.Use();
            }
            else
            {
                bool isCamera = Selection.gameObjects[0].GetComponent<Camera>() != null;
                bool hasAura = Selection.gameObjects[0].GetComponent<Aura>() != null;
                bool isLight = Selection.gameObjects[0].GetComponent<Light>() != null;
                bool hasAuraLight = Selection.gameObjects[0].GetComponent<AuraLight>() != null;
                bool hasAuraVolume = Selection.gameObjects[0].GetComponent<AuraVolume>() != null;
                if ((isCamera && !hasAura) || (isLight && !hasAuraLight))
                {
                    if (GUILayout.Button(_addButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
                    {
                        Undo.RecordObject(Selection.gameObjects[0], "Add AURA Component on selection");
                        if (isCamera)
                        {
                            Selection.gameObjects[0].AddComponent<Aura>();
                        }

                        if (isLight)
                        {
                            Selection.gameObjects[0].AddComponent<AuraLight>();
                        }

                        _sceneViewEvent.Use();
                    }
                }
                else if (hasAura || hasAuraLight || hasAuraVolume)
                {
                    if (GUILayout.Button(_removeButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
                    {
                        Undo.RecordObject(Selection.gameObjects[0], "Remove AURA Component on selection");
                        if (hasAura)
                        {
                            Selection.gameObjects[0].GetComponent<Aura>().Destroy();
                        }

                        if (hasAuraLight)
                        {
                            Selection.gameObjects[0].GetComponent<AuraLight>().Destroy();
                        }

                        if (hasAuraVolume)
                        {
                            Selection.gameObjects[0].GetComponent<AuraVolume>().Destroy();
                        }

                        _sceneViewEvent.Use();
                    }
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    GUILayout.Button(_undefinedButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth));
                    EditorGUI.EndDisabledGroup();
                }
            }
            EditorGUI.EndDisabledGroup();

            bool canToggle = false;
            for(int i = 0; i < Selection.gameObjects.Length; ++i)
            {
                if(Selection.gameObjects[i].GetComponent<Aura>() != null || Selection.gameObjects[i].GetComponent<AuraLight>() != null || Selection.gameObjects[i].GetComponent<AuraVolume>() != null)
                {
                    canToggle = true;
                    break;
                }
            }
            EditorGUI.BeginDisabledGroup(!canToggle);
            if (GUILayout.Button(_toggleButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                Undo.RecordObjects(Selection.gameObjects, "Toggle AURA Components on selection");
                for (int i = 0; i < Selection.gameObjects.Length; ++i)
                {
                    Aura auraComponent = Selection.gameObjects[i].GetComponent<Aura>();
                    if (auraComponent != null)
                    {
                        auraComponent.enabled = !auraComponent.enabled;
                    }

                    AuraLight auraLightComponent = Selection.gameObjects[i].GetComponent<AuraLight>();
                    if (auraLightComponent != null)
                    {
                        auraLightComponent.enabled = !auraLightComponent.enabled;
                    }

                    AuraVolume auraVolumeComponent = Selection.gameObjects[i].GetComponent<AuraVolume>();
                    if (auraVolumeComponent != null)
                    {
                        auraVolumeComponent.enabled = !auraVolumeComponent.enabled;
                    }
                }
                
                _sceneViewEvent.Use();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += _buttonWidth;

            GuiHelpers.DrawSeparator(Color.HSVToRGB(0.0f, 0.0f, 0.2f), new Rect(0, pixelsVerticalOffset, _toolboxWidth, 0), 4, 4, 2);
            pixelsVerticalOffset += 4;
            */

            ////////////////////////// CREATE PANEL

            GUILayout.Label(_createPanelTitleContent, GuiStyles.ButtonBoldNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(_buttonWidth));
            pixelsVerticalOffset += _buttonWidth;

            EditorGUI.BeginDisabledGroup(_isCreating);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_creationTypeButtonsContent[6], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingCamera();
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_creationTypeButtonsContent[7], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingLight(LightType.Directional);
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += 32;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_creationTypeButtonsContent[8], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingLight(LightType.Spot);
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_creationTypeButtonsContent[9], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingLight(LightType.Point);
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += 32;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_creationTypeButtonsContent[0], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingVolume(VolumeType.Global);
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_creationTypeButtonsContent[1], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingVolume(VolumeType.Layer);
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += 32;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_creationTypeButtonsContent[2], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingVolume(VolumeType.Box);
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_creationTypeButtonsContent[3], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingVolume(VolumeType.Sphere);
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += 32;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_creationTypeButtonsContent[4], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingVolume(VolumeType.Cylinder);
                _sceneViewEvent.Use();
            }
            if (GUILayout.Button(_creationTypeButtonsContent[5], GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth)))
            {
                StartCreatingVolume(VolumeType.Cone);
                _sceneViewEvent.Use();
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += 32;
            EditorGUI.EndDisabledGroup();

            GuiHelpers.DrawSeparator(new Color(0, 0, 0, 0), new Rect(0, pixelsVerticalOffset, _toolboxWidth, 0), 4, 4, 2);
            pixelsVerticalOffset += 4;

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(_isCreating && _creationType != ToolboxCreationType.Volume);
            EditorGUI.BeginChangeCheck();
            _creationSpace = (ToolboxCreationSpace)EditorGUILayout.Popup((int)_creationSpace, _creationSpaceContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth));
            if (EditorGUI.EndChangeCheck())
            {
                bool tmpIsCreating = _isCreating;
                AbortCreating();
                ShowNotification(new GUIContent(" Creation Space Changed" + (tmpIsCreating ? "\n Creation Aborted" : ""), Aura.ResourcesCollection.logoIconTexture));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginChangeCheck();
            _focusAfterCreation = GUILayout.Toggle(_focusAfterCreation, _focusAfterCreationToggleContent, _focusAfterCreation ? GuiStyles.ButtonPressedImageOnlyNoBorder : GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_buttonWidth), GUILayout.Height(_buttonWidth));
            if (EditorGUI.EndChangeCheck())
            {
                ShowNotification(new GUIContent(" Focus After Creation" + (_focusAfterCreation ? " Enabled" : " Disabled"), Aura.ResourcesCollection.logoIconTexture));
            }
            GUILayout.EndHorizontal();
            pixelsVerticalOffset += 32;

            ////////////////////////// DEBUG PANEL
            if (AuraEditorPrefs.DisplayDebugPanelInToolbox)
            {

                GuiHelpers.DrawSeparator(Color.HSVToRGB(0.0f, 0.0f, 0.2f), new Rect(0, pixelsVerticalOffset, _toolboxWidth, 0), 4, 4, 2);
                pixelsVerticalOffset += 4;

                GUILayout.Label(_debugPanelTitleContent, GuiStyles.ButtonBoldNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(_buttonWidth));
                pixelsVerticalOffset += _buttonWidth;

                GUILayout.BeginVertical();
                bool canDebug = false;
                Camera[] camerasArray = GameObject.FindObjectsOfType<Camera>();
                for (int i = 0; i < camerasArray.Length; ++i)
                {
                    if (camerasArray[i].GetComponent<AuraCamera>() != null && camerasArray[i].GetComponent<AuraCamera>().enabled)
                    {
                        canDebug = true;
                        break;
                    }
                }

                canDebug = canDebug && AuraEditorPrefs.EnableAuraInSceneView;
                EditorGUI.BeginDisabledGroup(!canDebug);
                Aura.ResourcesCollection.editionCameraQualitySettings.displayVolumetricLightingBuffer = GUILayout.Toggle(Aura.ResourcesCollection.editionCameraQualitySettings.displayVolumetricLightingBuffer, _debugVolumetricLightingButtonContent, Aura.ResourcesCollection.editionCameraQualitySettings.displayVolumetricLightingBuffer ? GuiStyles.ButtonPressedNoBorder : GuiStyles.ButtonNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(24));
                Aura.ResourcesCollection.editionCameraQualitySettings.debugOcclusionCulling = GUILayout.Toggle(Aura.ResourcesCollection.editionCameraQualitySettings.debugOcclusionCulling, _debugOcclusionCullingButtonContent, Aura.ResourcesCollection.editionCameraQualitySettings.debugOcclusionCulling ? GuiStyles.ButtonPressedNoBorder : GuiStyles.ButtonNoBorder, GUILayout.Width(_toolboxWidth), GUILayout.Height(24));
                EditorGUI.EndDisabledGroup();
                GUILayout.EndVertical();
                pixelsVerticalOffset += 48;
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the toolbox button bed
        /// </summary>
        private static void DrawActivationToggleButton()
        {
            GUILayout.Window(++_controlID, _activationToggleButtonRect, DrawActivationToggleButtonBody, "", GuiStyles.ButtonNoHoverNoBorder); 
        }

        /// <summary>
        /// Draws the toolbox body
        /// </summary>
        /// <param name="controlId"></param>
        private static void DrawActivationToggleButtonBody(int controlId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginArea(new Rect(0, 0, _activationToggleButtonWidth, _activationToggleButtonHeight));
            GUILayout.BeginHorizontal();
            
            GUIContent buttonContent = new GUIContent(EditorApplication.isCompiling ? "PLEASE WAIT" : ((AuraEditorPrefs.EnableAuraInSceneView ? "Disable" : "Enable") + " Aura Preview"), Aura.ResourcesCollection.logoIconTexture);

            int fontSize = 9;
#if UNITY_2019_1_OR_NEWER
            fontSize = 11;
#endif
            if (GUILayout.Button(buttonContent, new GUIStyle(AuraEditorPrefs.EnableAuraInSceneView ? GuiStyles.ButtonPressedNoBorder : GuiStyles.ButtonNoBorder) { fontSize = fontSize }, GUILayout.Width(_activationToggleButtonWidth), GUILayout.Height(_activationToggleButtonHeight)))
            {
                AuraEditorPrefs.EnableAuraInSceneView = !AuraEditorPrefs.EnableAuraInSceneView;
                _sceneViewEvent.Use();
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the toolbox button bed
        /// </summary>
        private static void DrawDisplayToggleButton()
        {
            GUILayout.Window(++_controlID, _displayToggleButtonsRect, DrawDisplayToggleButtonBody, "", GuiStyles.ButtonNoHoverNoBorder);
        }

        /// <summary>
        /// Draws the toolbox body
        /// </summary>
        /// <param name="controlId"></param>
        private static void DrawDisplayToggleButtonBody(int controlId)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginArea(new Rect(0, 0, _toolboxToggleDisplayButtonWidth, ToolboxHeight));
            GUILayout.BeginVertical();

            int expansionToggleButtonHeight = ToolboxHeight;

            string expansionToggleButtonString = IsExpanded ? "Collapse Aura Toolbox" : "Expand Aura Toolbox";
            Texture2D expansionToggleButtonTexture = IsVisible ? Aura.ResourcesCollection.downIconTexture : Aura.ResourcesCollection.upIconTexture;
            if (AuraEditorPrefs.ToolboxPosition == 0)
            {
                expansionToggleButtonTexture = IsExpanded ? Aura.ResourcesCollection.leftIconTexture : Aura.ResourcesCollection.rightIconTexture;
            }
            else
            {
                expansionToggleButtonTexture = IsExpanded ? Aura.ResourcesCollection.rightIconTexture : Aura.ResourcesCollection.leftIconTexture;
            }
            GUIContent expansionToggleButtonContent = new GUIContent(expansionToggleButtonString, expansionToggleButtonTexture, expansionToggleButtonString);
            if (GUILayout.Button(expansionToggleButtonContent, GuiStyles.ButtonImageOnlyNoBorder, GUILayout.Width(_toolboxToggleDisplayButtonWidth), GUILayout.Height(expansionToggleButtonHeight)))
            {
                Expand(!IsExpanded);
                _sceneViewEvent.Use();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws the presets window bed
        /// </summary>
        /// <param name="sceneView"></param>
        private static void DrawPresetsWindow()
        {
            GUILayout.Window(++_controlID, _presetsWindowRect, DrawPresetsWindowBody, "", GuiStyles.ButtonNoHoverNoBorder);
        }

        /// <summary>
        /// Draws the presets window
        /// </summary>
        /// <param name="controlId"></param>
        private static void DrawPresetsWindowBody(int controlId)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginArea(new Rect(0, 0, _presetsWindowWidth, ToolboxHeight));
            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder, GUILayout.Width(_presetsWindowWidth));
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Ambience</b>", Aura.ResourcesCollection.presetUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
            EditorGUILayout.EndHorizontal();
            /*Rect rect = */EditorGUILayout.BeginHorizontal();
            _presetsWindowScroll = GUILayout.BeginScrollView(_presetsWindowScroll, false, true, GUILayout.Width(_presetsWindowWidth)/*, GUILayout.Height(ToolboxHeight - rect.y - 1)*/);

            int selectedPresetId = -1;
            if(PresetsPreviewsPerRow == 1)
            {
                for (int i = 0; i < Aura.ResourcesCollection.presetsButtonsTextures.Length; ++i)
                {
                    if (GUILayout.Button(new GUIContent(Aura.ResourcesCollection.presetsButtonsTextures[i]), GuiStyles.ButtonImageOnly, GUILayout.Width(368), GUILayout.Height(192)))
                    {
                        selectedPresetId = i;
                        _sceneViewEvent.Use();
                    }
                }
            }
            else
            {
                selectedPresetId = GUILayout.SelectionGrid(selectedPresetId, Aura.ResourcesCollection.presetsButtonsTextures, 2, GuiStyles.ButtonImageOnly, GUILayout.Width(368), GUILayout.Height(293));
            }

            if (selectedPresetId > -1)
            {
                Presets selectedPreset = (Presets)selectedPresetId;
                Aura.ApplyPreset(selectedPreset);
                ShowNotification( new GUIContent(" Applying \"" + selectedPreset + "\" Ambience Preset", Aura.ResourcesCollection.presetUiIconTexture));
                _showPresets = false;
            }

            GUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
                GUILayout.EndArea();
            GUILayout.EndVertical();
        }
        #endregion

        #region Add
        /// <summary>
        /// Adds the Aura component to all the Cameras
        /// </summary>
        public static void AddAuraToCameras()
        {
            ShowNotification(new GUIContent(" Adding Aura to Cameras", Aura.ResourcesCollection.cameraUiIconTexture));
            Aura.AddAuraToCameras();
        }

        /// <summary>
        /// Adds the Aura component to all the Directional Lights
        /// </summary>
        public static void AddAuraToDirectionalLights()
        {
            ShowNotification(new GUIContent(" Adding Aura to Directional Lights", Aura.ResourcesCollection.lightUiIconTexture));
            Aura.AddAuraToDirectionalLights();
        }

        /// <summary>
        /// Adds the Aura component to all the Spot Lights
        /// </summary>
        public static void AddAuraToSpotLights()
        {
            ShowNotification(new GUIContent(" Adding Aura to Spot Lights", Aura.ResourcesCollection.lightUiIconTexture));
            Aura.AddAuraToSpotLights();
        }

        /// <summary>
        /// Adds the Aura component to all the Point Lights
        /// </summary>
        public static void AddAuraToPointLights()
        {
            ShowNotification(new GUIContent(" Adding Aura to Point Lights", Aura.ResourcesCollection.lightUiIconTexture));
            Aura.AddAuraToPointLights();
        }
        #endregion

        #region Create

        #region Camera
        /// <summary>
        /// Toggles to camera creation mode
        /// </summary>
        private static void StartCreatingCamera()
        {
            AbortCreating();
            _creationType = ToolboxCreationType.Camera;
            ShowNotification(new GUIContent(" Creating Camera", Aura.ResourcesCollection.cameraUiIconTexture));
            _isCreating = true;
        }

        /// <summary>
        /// Creates the camera
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateCamera(SceneView sceneView, Event sceneViewEvent)
        {
            if (!sceneViewEvent.alt && LeftClick)
            {
                ++_clickCount;
                sceneViewEvent.Use();

                if (_clickCount == 1)
                {
                    _createdObject = AuraCamera.CreateGameObject("Aura Camera");
                    _isNewObjectCreated = true;
                    return;
                }
            }

            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                if (_createdObject != null)
                {
                    bool hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);

                    if (hitsTarget)
                    {
                        DrawTarget(sceneView, _clickPositionB);

                        _createdObject.transform.position = _clickPositionA;
                        _createdObject.transform.LookAt(_clickPositionB);
                        _createdObject.transform.localScale = Vector3.one;
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }
        #endregion

        #region Light
        /// <summary>
        /// Toggles to light creation mode
        /// </summary>
        private static void StartCreatingLight(LightType lightType)
        {
            AbortCreating();
            _creationType = ToolboxCreationType.Light;
            _lightType = lightType;
            ShowNotification(new GUIContent(" Creating " + _lightType + " Light", Aura.ResourcesCollection.lightUiIconTexture));
            _isCreating = true;
        }

        /// <summary>
        /// Creates the light
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateLight(SceneView sceneView, Event sceneViewEvent)
        {
            if (!sceneViewEvent.alt && LeftClick)
            {
                ++_clickCount;
                sceneViewEvent.Use();

                switch (_lightType)
                {
                    case LightType.Directional:
                        {
                            if (_clickCount == 1)
                            {
                                _createdObject = AuraLight.CreateGameObject("Aura Directional Light", LightType.Directional);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case LightType.Spot:
                        {
                            if (_clickCount == 2)
                            {
                                _createdObject = AuraLight.CreateGameObject("Aura Spot Light", LightType.Spot);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case LightType.Point:
                        {
                            if (_clickCount == 1)
                            {
                                _createdObject = AuraLight.CreateGameObject("Aura Point Light", LightType.Point);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;
                }
            }

            switch (_lightType)
            {
                case LightType.Directional:
                    {
                        CreateDirectionalLight(sceneView, sceneViewEvent);
                    }
                    break;

                case LightType.Spot:
                    {
                        CreateSpotLight(sceneView, sceneViewEvent);
                    }
                    break;

                case LightType.Point:
                    {
                        CreatePointLight(sceneView, sceneViewEvent);
                    }
                    break;
            }
        }

        /// <summary>
        /// Creates a directional light
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateDirectionalLight(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                if (_createdObject != null)
                {
                    _forwardVector = (_clickPositionB - _clickPositionA).normalized;
                    bool hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);

                    if (hitsTarget)
                    {
                        DrawTarget(sceneView, _clickPositionB);
                        Quaternion rotation = Quaternion.LookRotation(_forwardVector);
                        _createdObject.transform.position = _clickPositionA;
                        _createdObject.transform.rotation = rotation;
                        _createdObject.transform.localScale = Vector3.one;
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }

        /// <summary>
        /// Creates a spot light
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateSpotLight(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                bool hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);

                if (hitsTarget)
                {
                    DrawTarget(sceneView, _clickPositionB);
                    CustomGizmo.DrawLineSegment(_clickPositionA, _clickPositionB, CustomGizmo.color, CustomGizmo.pixelWidth);
                    sceneView.Repaint();
                }
            }
            else if (_clickCount == 2)
            {
                if (_createdObject != null)
                {
                    Vector3 backToFront = _clickPositionB - _clickPositionA;
                    _forwardVector = backToFront.normalized;
                    Plane intersectionPlane = new Plane(_forwardVector, _clickPositionB);

                    bool hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionC);

                    if (hitsTarget)
                    {
                        float length = backToFront.magnitude;
                        float width = Vector3.Distance(_clickPositionB, _clickPositionC);
                        Quaternion rotation = Quaternion.LookRotation(_forwardVector);
                        _createdObject.transform.position = _clickPositionA;
                        _createdObject.transform.rotation = rotation;
                        _createdObject.transform.localScale = Vector3.one;
                        _createdObject.GetComponent<Light>().range = length;
                        _createdObject.GetComponent<Light>().spotAngle = Mathf.Atan(width / length * 2.0f) * Mathf.Rad2Deg;
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }

        /// <summary>
        /// Creates a point light
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreatePointLight(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                if (_createdObject != null)
                {
                    Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + Vector3.up, _clickPositionA + sceneView.camera.transform.right);
                    bool hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionB);

                    if (hitsTarget)
                    {
                        float width = Vector3.Distance(_clickPositionA, _clickPositionB);
                        _createdObject.transform.position = _clickPositionA;
                        _createdObject.transform.rotation = Quaternion.identity;
                        _createdObject.transform.localScale = Vector3.one;
                        _createdObject.GetComponent<Light>().range = width;
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }
        #endregion

        #region Volume
        /// <summary>
        /// Toggles to volume creation mode
        /// </summary>
        private static void StartCreatingVolume(VolumeType volumeType)
        {
            AbortCreating();
            _creationType = ToolboxCreationType.Volume;
            _volumeType = volumeType;
            ShowNotification(new GUIContent(" Creating "+ _volumeType + " Volume", Aura.ResourcesCollection.volumeUiIconTexture));
            _isCreating = true;

        }

        /// <summary>
        /// Creates the volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateVolume(SceneView sceneView, Event sceneViewEvent)
        {
            if (!sceneViewEvent.alt && LeftClick)
            {
                ++_clickCount;
                sceneViewEvent.Use();
                
                switch (_volumeType)
                {
                    case VolumeType.Global:
                        {
                            if (_clickCount == 1)
                            {
                                _createdObject = AuraVolume.CreateGameObject("Aura Global Volume", VolumeType.Global);
                                _createdObject.transform.position = _clickPositionA;
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case VolumeType.Layer:
                        {
                            if (_clickCount == 1)
                            {
                                _createdObject = AuraVolume.CreateGameObject("Aura Layer Volume", VolumeType.Layer);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case VolumeType.Box:
                        {
                            if (_clickCount == 3)
                            {
                                _createdObject = AuraVolume.CreateGameObject("Aura Box Volume", VolumeType.Box);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case VolumeType.Sphere:
                        {
                            if (_clickCount == 2)
                            {
                                _createdObject = AuraVolume.CreateGameObject("Aura Sphere Volume", VolumeType.Sphere);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case VolumeType.Cylinder:
                        {
                            if (_clickCount == 2)
                            {
                                _createdObject = AuraVolume.CreateGameObject("Aura Cylinder Volume", VolumeType.Cylinder);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;

                    case VolumeType.Cone:
                        {
                            if (_clickCount == 2)
                            {
                                _createdObject = AuraVolume.CreateGameObject("Aura Cone Volume", VolumeType.Cone);
                                _isNewObjectCreated = true;
                                return;
                            }
                        }
                        break;
                }
            }

            switch (_volumeType)
            {
                case VolumeType.Global:
                    {
                        CreateGlobalVolume(sceneView, sceneViewEvent);
                    }
                    break;

                case VolumeType.Layer:
                    {
                        CreateLayerVolume(sceneView, sceneViewEvent);
                    }
                    break;

                case VolumeType.Box:
                    {
                        CreateBoxVolume(sceneView, sceneViewEvent);
                    }
                    break;

                case VolumeType.Sphere:
                    {
                        CreateSphereVolume(sceneView, sceneViewEvent);
                    }
                    break;

                case VolumeType.Cylinder:
                    {
                        CreateCylinderVolume(sceneView, sceneViewEvent);
                    }
                    break;

                case VolumeType.Cone:
                    {
                        CreateConeVolume(sceneView, sceneViewEvent);
                    }
                    break;
            }
        }

        /// <summary>
        /// Creates a global volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateGlobalVolume(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                if (TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionA, ref _clickNormal))
                {
                    DrawTarget(sceneView, _clickPositionA);
                    if(_creationSpace != ToolboxCreationSpace.Topology)
                    {
                        Vector3 forward = -Vector3.Cross(_clickNormal, Vector3.Cross(_clickNormal, sceneView.camera.transform.forward).normalized).normalized;
                        _matrix = Matrix4x4.TRS(_clickPositionA, _creationSpace == ToolboxCreationSpace.World ? Quaternion.identity : Quaternion.LookRotation(forward, _clickNormal), Vector3.one);
                        DrawGrid(_matrix);
                    }
                    sceneView.Repaint();
                }
            }
            else
            {
                if(_createdObject != null)
                {
                    FrameCreatedObjectAndStopEditing(sceneView);
                }
            }
        }

        /// <summary>
        /// Creates a layer volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateLayerVolume(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                if(_createdObject != null)
                {
                    bool hitsTarget = false;
                    if (_creationSpace == ToolboxCreationSpace.Topology)
                    {
                        hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);
                    }
                    else
                    {
                        Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + _upVector, _clickPositionA + sceneView.camera.transform.right);
                        hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionB);
                        DrawGrid(_matrix);
                    }

                    if (hitsTarget)
                    {
                        if (_creationSpace == ToolboxCreationSpace.Topology)
                        {
                            DrawTarget(sceneView, _clickPositionB);
                            _upVector = (_clickPositionB - _clickPositionA).normalized;
                            _forwardVector = -Vector3.Cross(_upVector, Vector3.Cross(_upVector, sceneView.camera.transform.forward).normalized).normalized;
                        }

                        float distance = Vector3.Distance(_clickPositionA, _clickPositionB) * Vector3.Dot((_clickPositionB - _clickPositionA).normalized, _upVector);
                        _createdObject.transform.position = _clickPositionA;
                        _createdObject.transform.rotation = Quaternion.LookRotation(_forwardVector, _upVector);
                        _createdObject.transform.localScale = new Vector3(1.0f, distance, 1.0f);
                        CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Height = " + distance, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }

        /// <summary>
        /// Creates a box volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateBoxVolume(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                bool hitsTarget = false;
                if (_creationSpace == ToolboxCreationSpace.Topology)
                {
                    hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);
                }
                else
                {
                    Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + _rightVector, _clickPositionA + _forwardVector);
                    hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionB);
                    DrawGrid(Matrix4x4.TRS(_clickPositionA, Quaternion.LookRotation((_clickPositionB - _clickPositionA).normalized, _upVector), Vector3.one));
                }

                if (hitsTarget)
                {
                    DrawTarget(sceneView, _clickPositionB);
                    CustomGizmo.DrawLineSegment(_clickPositionA, _clickPositionB, CustomGizmo.color, CustomGizmo.pixelWidth);
                    CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Length = " + Vector3.Distance(_clickPositionA, _clickPositionB), CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                    sceneView.Repaint();
                }
            }
            else if (_clickCount == 2)
            {
                _rightVector = (_clickPositionB - _clickPositionA).normalized;
                _forwardVector = Vector3.Cross(_rightVector, _upVector).normalized;
                _matrix = Matrix4x4.TRS(_clickPositionA, Quaternion.LookRotation(_forwardVector, _upVector), Vector3.one);

                bool hitsTarget = false;
                if (_creationSpace == ToolboxCreationSpace.Topology)
                {
                    hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionC, ref _clickNormal);
                }
                else
                {
                    DrawGrid(_matrix);
                    Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + _rightVector, _clickPositionA + _forwardVector);
                    hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionC);
                }

                if (hitsTarget)
                {
                    DrawTarget(sceneView, _clickPositionC);
                    Vector3 projectedPoint = MathHelpers.ProjectPointOnLine(_clickPositionA, _rightVector, _clickPositionC); // HandleUtility.ProjectPointLine(_clickPositionC, _clickPositionA, _clickPositionB); Doesn't work outside boundary points

                    if (_creationSpace == ToolboxCreationSpace.Topology)
                    {
                        _forwardVector = (_clickPositionC - projectedPoint).normalized;
                        _upVector = -Vector3.Cross(_rightVector, _forwardVector);
                    }

                    float distance = Vector3.Distance(projectedPoint, _clickPositionC) * Vector3.Dot((_clickPositionC - projectedPoint).normalized, _forwardVector);
                    _pointC = _clickPositionA + _forwardVector * distance;
                    Vector3 center = (_clickPositionB + _pointC) * 0.5f;
                    Matrix4x4 matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(_forwardVector, _upVector), new Vector3(Vector3.Distance(_clickPositionA, _clickPositionB), 1.0f, distance));
                    CustomGizmo.DrawSquare(matrix, CustomGizmo.color, CustomGizmo.pixelWidth);
                    CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Length = " + Vector3.Distance(_clickPositionA, _clickPositionB) + "\nWidth = " + distance, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                    sceneView.Repaint();
                }
            }
            else if (_clickCount == 3)
            {
                Vector3 baseCenter = (_clickPositionB + _pointC) * 0.5f;
                Plane intersectionPlane = new Plane(baseCenter, baseCenter + _upVector, baseCenter + sceneView.camera.transform.right);
                Vector3 intersectionPoint = Vector3.zero;
                if (TargetOnPlane(sceneViewEvent, intersectionPlane, ref intersectionPoint) && _createdObject != null)
                {
                    if (_creationSpace != ToolboxCreationSpace.Topology)
                    {
                        DrawGrid(_matrix);
                    }
                    Vector3 projectedPoint = MathHelpers.ProjectPointOnLine(baseCenter, _upVector, intersectionPoint);
                    float height = Vector3.Distance(projectedPoint, baseCenter);
                    Vector3 center = (baseCenter + projectedPoint) * 0.5f;
                    _createdObject.transform.position = center;
                    _createdObject.transform.rotation = Quaternion.LookRotation(_forwardVector, _upVector);
                    _createdObject.transform.localScale = new Vector3(Vector3.Distance(_clickPositionA, _clickPositionB), height, Vector3.Distance(_pointC, _clickPositionA));
                    CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Length = " + Vector3.Distance(_clickPositionA, _clickPositionB) + "\nWidth = " + Vector3.Distance(_clickPositionA, _pointC) + "\nHeight = " + height, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                    sceneView.Repaint();
                }
            }
            else
            {
                
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }

        /// <summary>
        /// Creates a sphere volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateSphereVolume(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                bool hitsTarget = false;
                if (_creationSpace == ToolboxCreationSpace.Topology)
                {
                    hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);
                }
                else
                {
                    Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + _upVector, _clickPositionA + sceneView.camera.transform.right);
                    hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionB);
                }

                if (hitsTarget)
                {
                    if (_creationSpace != ToolboxCreationSpace.Topology)
                    {
                        _clickPositionB = MathHelpers.ProjectPointOnLine(_clickPositionA, _upVector, _clickPositionB);
                        DrawGrid(_matrix);
                    }

                    DrawTarget(sceneView, _clickPositionB);
                    float height = Vector3.Distance(_clickPositionB, _clickPositionA);
                    CustomGizmo.DrawLineSegment(_clickPositionA, _clickPositionB, CustomGizmo.color, CustomGizmo.pixelWidth);
                    CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Height = " + height, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                    sceneView.Repaint();
                }
            }
            else if (_clickCount == 2)
            {
                if(_createdObject != null)
                {
                    Vector3 baseToTop = _clickPositionB - _clickPositionA;
                    _upVector = baseToTop.normalized;
                    Vector3 center = (_clickPositionA + _clickPositionB) * 0.5f;

                    bool hitsTarget = false;
                    if (_creationSpace == ToolboxCreationSpace.Topology)
                    {
                        hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionC, ref _clickNormal);
                    }
                    else
                    {
                        Plane intersectionPlane = new Plane(center, center + _upVector, center + sceneView.camera.transform.right);
                        hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionC);
                    }

                    if (hitsTarget)
                    {
                        float distance = 0;
                        if (_creationSpace == ToolboxCreationSpace.Topology)
                        {
                            DrawTarget(sceneView, _clickPositionC);
                            Plane intersectionPlane = new Plane(_upVector, center);
                            _clickPositionC = intersectionPlane.ClosestPointOnPlane(_clickPositionC);
                            distance = Vector3.Distance(center, _clickPositionC);
                            _forwardVector = (_clickPositionC - center).normalized;
                        }
                        else
                        {
                            DrawGrid(_matrix);
                            Vector3 projectedPoint = MathHelpers.ProjectPointOnLine(_clickPositionA, _upVector, _clickPositionC);
                            Vector3 centerToBorder = _clickPositionC - projectedPoint;
                            distance = centerToBorder.magnitude;
                            _forwardVector = centerToBorder.normalized;
                        }

                        _createdObject.transform.position = center;
                        _createdObject.transform.rotation = Quaternion.LookRotation(_forwardVector, _upVector);
                        _createdObject.transform.localScale = new Vector3(distance * 2.0f, baseToTop.magnitude, distance * 2.0f);
                        CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Height = " + Vector3.Distance(_clickPositionA, _clickPositionB) + "\nRadius = " + distance, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }

        /// <summary>
        /// Creates a cylinder volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateCylinderVolume(SceneView sceneView, Event sceneViewEvent)
        {
            CreateSphereVolume(sceneView, sceneViewEvent); // The very same creation process except that the created volume is of type Cylinder
        }

        /// <summary>
        /// Creates a cone volume
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void CreateConeVolume(SceneView sceneView, Event sceneViewEvent)
        {
            if (_clickCount == 0)
            {
                SetFirstPoint(sceneView, sceneViewEvent);
            }
            else if (_clickCount == 1)
            {
                bool hitsTarget = false;
                if (_creationSpace == ToolboxCreationSpace.Topology)
                {
                    hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionB, ref _clickNormal);
                }
                else
                {
                    Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + _upVector, _clickPositionA + sceneView.camera.transform.right);
                    hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionB);
                }

                if (hitsTarget)
                {
                    if (_creationSpace != ToolboxCreationSpace.Topology)
                    {
                        _clickPositionB = MathHelpers.ProjectPointOnLine(_clickPositionA, _upVector, _clickPositionB);
                        DrawGrid(_matrix);
                    }

                    DrawTarget(sceneView, _clickPositionB);
                    float height = Vector3.Distance(_clickPositionB, _clickPositionA);
                    CustomGizmo.DrawLineSegment(_clickPositionA, _clickPositionB, CustomGizmo.color, CustomGizmo.pixelWidth);
                    CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Length = " + height, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                    sceneView.Repaint();
                }
            }
            else if (_clickCount == 2)
            {
                if (_createdObject != null)
                {
                    Vector3 backToFront = _clickPositionB - _clickPositionA;
                    _forwardVector = backToFront.normalized;

                    bool hitsTarget = false;
                    if (_creationSpace == ToolboxCreationSpace.Topology)
                    {
                        hitsTarget = TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionC, ref _clickNormal);
                    }
                    else
                    {
                        Plane intersectionPlane = new Plane(_clickPositionA, _clickPositionA + _forwardVector, _clickPositionA + sceneView.camera.transform.right);
                        hitsTarget = TargetOnPlane(sceneViewEvent, intersectionPlane, ref _clickPositionC);
                    }

                    if (hitsTarget)
                    {
                        float distance = 0;
                        if (_creationSpace == ToolboxCreationSpace.Topology)
                        {
                            DrawTarget(sceneView, _clickPositionC);
                            Plane intersectionPlane = new Plane(_forwardVector, _clickPositionA);
                            _clickPositionC = intersectionPlane.ClosestPointOnPlane(_clickPositionC);
                            distance = Vector3.Distance(_clickPositionA, _clickPositionC);
                            _rightVector = (_clickPositionC - _clickPositionA).normalized;
                        }
                        else
                        {
                            Vector3 projectedPoint = MathHelpers.ProjectPointOnLine(_clickPositionA, _forwardVector, _clickPositionC);
                            Vector3 centerToBorder = _clickPositionC - projectedPoint;
                            distance = centerToBorder.magnitude;
                            _rightVector = centerToBorder.normalized;
                        }
                        _upVector = Vector3.Cross(_rightVector, _forwardVector);

                        if (_creationSpace != ToolboxCreationSpace.Topology)
                        {
                            _matrix = Matrix4x4.TRS(_clickPositionA, _creationSpace == ToolboxCreationSpace.World ? Quaternion.identity : Quaternion.LookRotation(-Vector3.Cross(_clickNormal, Vector3.Cross(_clickNormal, sceneView.camera.transform.forward).normalized).normalized, _forwardVector), Vector3.one);
                            DrawGrid(_matrix);
                        }

                        _createdObject.transform.position = _clickPositionA;
                        _createdObject.transform.rotation = Quaternion.LookRotation(_forwardVector, _upVector);
                        _createdObject.transform.localScale = new Vector3(distance * 2.0f, distance * 2.0f, backToFront.magnitude);
                        CustomGizmo.DrawLabelNextToMouse(sceneViewEvent, "Length = " + Vector3.Distance(_clickPositionA, _clickPositionB) + "\nRadius = " + distance, CustomGizmo.color, _labelPixelsOffset, GuiStyles.LabelBackground);
                        sceneView.Repaint();
                    }
                }
            }
            else
            {
                FrameCreatedObjectAndStopEditing(sceneView);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Tells if creation must be cancelled
        /// </summary>
        /// <returns></returns>
        private static bool CheckForAbortCreating()
        {
            if (RightClick || EscapeButton)
            {
                AbortCreating();
                _sceneViewEvent.Use();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Cancels creation
        /// </summary>
        private static void AbortCreating()
        {
            ShowNotification(new GUIContent(" Creation Aborted", Aura.ResourcesCollection.logoIconTexture));
            if (_isNewObjectCreated)
            {
                _createdObject.Destroy();
            }
            StopCreating();
        }

        /// <summary>
        /// Resets all creation data
        /// </summary>
        private static void StopCreating()
        {
            _clickCount = 0;
            _isCreating = false;
            _createdObject = null;
            _isNewObjectCreated = false;
        }

        /// <summary>
        /// Frame the created object
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        private static void FrameCreatedObjectAndStopEditing(SceneView sceneView)
        {
            if(_creationType == ToolboxCreationType.Camera)
            {
                ShowNotification(new GUIContent(" Camera Created", Aura.ResourcesCollection.cameraUiIconTexture));
            }
            else if(_creationType == ToolboxCreationType.Light)
            {
                ShowNotification(new GUIContent(" " + _lightType + " Light Created", Aura.ResourcesCollection.lightUiIconTexture));
            }
            else if(_creationType == ToolboxCreationType.Volume)
            {
                ShowNotification(new GUIContent(" " + _volumeType + " Volume Created", Aura.ResourcesCollection.volumeUiIconTexture));
            }

            if (_focusAfterCreation)
            {
                Selection.activeGameObject = _createdObject;
                sceneView.FrameSelected(true);
            }

            StopCreating();
        }

        /// <summary>
        /// Draw the target in the scene
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="targetPosition">The position of the target</param>
        private static void DrawTarget(SceneView sceneView, Vector3 targetPosition)
        {
            CustomGizmo.DrawCross(Matrix4x4.TRS(targetPosition, Quaternion.identity, Vector3.one), CustomGizmo.color, CustomGizmo.pixelWidth);
        }

        /// <summary>
        /// Draw a grid
        /// </summary>
        /// <param name="matrix">The matrix of the grid</param>
        private static void DrawGrid(Matrix4x4 matrix)
        {
            CustomGizmo.DrawGrid(matrix, 3, 3, 2, Color.grey * new Color(1.0f, 1.0f, 1.0f, 0.75f), CustomGizmo.pixelWidth, Color.grey * new Color(1.0f, 1.0f, 1.0f, 0.5f), CustomGizmo.pixelWidth * 0.5f);
        }

        /// <summary>
        /// Sets the starting point of the creation mode
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        private static void SetFirstPoint(SceneView sceneView, Event sceneViewEvent)
        {
            if (TargetOnScene(sceneView, sceneViewEvent, ref _clickPositionA, ref _clickNormal))
            {
                DrawTarget(sceneView, _clickPositionA);
                if(_creationType == ToolboxCreationType.Volume)
                {
                    if (_creationSpace == ToolboxCreationSpace.Normal)
                    {
                        Vector3 forward = -Vector3.Cross(_clickNormal, Vector3.Cross(_clickNormal, sceneView.camera.transform.forward).normalized).normalized;
                        _matrix = Matrix4x4.TRS(_clickPositionA, Quaternion.LookRotation(forward, _clickNormal), Vector3.one);
                        _rightVector = -Vector3.Cross(forward, _clickNormal);
                        _upVector = _clickNormal;
                        _forwardVector = forward;
                        DrawGrid(_matrix);
                    }
                    else if (_creationSpace == ToolboxCreationSpace.World)
                    {
                        _matrix = Matrix4x4.TRS(_clickPositionA, Quaternion.identity, Vector3.one);
                        _rightVector = Vector3.right;
                        _upVector = Vector3.up;
                        _forwardVector = Vector3.forward;
                        DrawGrid(_matrix);
                    }
                }

                sceneView.Repaint();
            }
        }

        /// <summary>
        /// Targets the scene under the mouse
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        /// <param name="sceneViewEvent">The current scene view event</param>
        /// <param name="targetPosition">The output scene position under the mouse</param>
        /// <param name="targetNormal">The output scene normal under the mouse</param>
        /// <returns>False if fails to target scene under the mouse</returns>
        private static bool TargetOnScene(SceneView sceneView, Event sceneViewEvent, ref Vector3 targetPosition, ref Vector3 targetNormal)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(sceneViewEvent.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                targetNormal = hit.normal;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Targets a plane under the mouse
        /// </summary>
        /// <param name="sceneViewEvent">The current scene view event</param>
        /// <param name="targetPosition">The output position under the mouse</param>
        /// <returns></returns>
        private static bool TargetOnPlane(Event sceneViewEvent, Plane plane, ref Vector3 targetPosition)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(sceneViewEvent.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                targetPosition = ray.origin + ray.direction * distance;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Displays a notification overlaying the sceneView
        /// </summary>
        /// <param name="content">The content of the notification</param>
        private static void ShowNotification(GUIContent content)
        {
            if(DisplayNotifications)
            {
                _currentSceneView.ShowNotification(content);
                _isNotificationDisplayed = true;
                _notificationsTimestamp = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// Removes the notification after _notificationsMaxDuration
        /// </summary>
        private static void CheckToRemoveNotification()
        {
            if(_isNotificationDisplayed && ((_timestamp - _notificationsTimestamp) > _notificationsMaxDuration))
            {
                _currentSceneView.RemoveNotification();
                _isNotificationDisplayed = false;
            }
        }

        /// <summary>
        /// Sets the toolbox visibility
        /// </summary>
        /// <param name="visibility">Shows/hides the toolbox</param>
        public static void Display(bool visibility)
        {
            AuraEditorPrefs.DisplayToolbox = visibility;
            ShowNotification(new GUIContent((visibility ? " Showing" : " Hiding") + " Toolbox", Aura.ResourcesCollection.logoIconTexture));
        }

        /// <summary>
        /// Collapses/expands the toolbox
        /// </summary>
        /// <param name="visibility">Collapses/expands the toolbox</param>
        public static void Expand(bool visibility)
        {
            AuraEditorPrefs.ExpandToolbox = visibility;
            ShowNotification(new GUIContent((visibility ? " Expanding" : " Collapsing") + " Toolbox", Aura.ResourcesCollection.logoIconTexture));
        }
        #endregion
        #endregion        
        #endregion
    }
}