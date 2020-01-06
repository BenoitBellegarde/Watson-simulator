
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
    /// Autoloading class that will show introduction splash screens
    /// </summary>
    [InitializeOnLoad]
    public class IntroductionScreens
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        static IntroductionScreens()
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
        /// Tells if resources are initialized
        /// </summary>
        private static bool _isInitialized;
        /// <summary>
        /// The scene view event
        /// </summary>
        private static Event _sceneViewEvent;
        /// <summary>
        /// The size of the screens
        /// </summary>
        private static readonly Vector2Int _size = new Vector2Int(720, 480);
        /// <summary>
        /// The fixe margin
        /// </summary>
        private static readonly int _margin = 24;
        /// <summary>
        /// The current displayed screen id of the main introduction
        /// </summary>
        private static int _mainIntroductionScreenId = 0;
        /// <summary>
        /// The current displayed screen id of the camera introduction
        /// </summary>
        private static int _cameraIntroductionScreenId = 0;
        /// <summary>
        /// The current displayed screen id of the light introduction
        /// </summary>
        private static int _lightIntroductionScreenId = 0;
        /// <summary>
        /// The current displayed screen id of the volume introduction
        /// </summary>
        private static int _volumeIntroductionScreenId = 0;
        #endregion

        #region Properties
        /// <summary>
        /// Tells if the main screen should be displayed
        /// </summary>
        private static bool ShowMainIntroductionScreen
        {
            get
            {
                return _mainIntroductionScreenId > 0 || AuraEditorPrefs.DisplayMainIntroductionScreen;
            }
        }

        /// <summary>
        /// Tells if the camera screen should be displayed
        /// </summary>
        private static bool ShowCameraIntroductionScreen
        {
            get
            {
                return (_cameraIntroductionScreenId > 0 || AuraEditorPrefs.DisplayCameraIntroductionScreen);
            }
        }

        /// <summary>
        /// Tells if the light screen should be displayed
        /// </summary>
        private static bool ShowLightIntroductionScreen
        {
            get
            {
                return (_lightIntroductionScreenId > 0 || AuraEditorPrefs.DisplayLightIntroductionScreen);
            }
        }

        /// <summary>
        /// Tells if the volume screen should be displayed
        /// </summary>
        private static bool ShowVolumeIntroductionScreen
        {
            get
            {
                return (_volumeIntroductionScreenId > 0 || AuraEditorPrefs.DisplayVolumeIntroductionScreen);
            }
        }

        /// <summary>
        /// Tells if any screen should be displayed
        /// </summary>
        private static bool ShowAnyIntroductionScreen
        {
            get
            {
                return ShowMainIntroductionScreen || ShowCameraIntroductionScreen || ShowLightIntroductionScreen || ShowVolumeIntroductionScreen;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Called on every scene view update
        /// </summary>
        /// <param name="sceneView">The current scene view</param>
        private static void OnSceneViewGUI(SceneView sceneView)
        {
            if (ShowAnyIntroductionScreen)
            {
                if (!_isInitialized)
                {
                    Initialize(sceneView);
                }

                _sceneViewEvent = Event.current;

                if (_sceneViewEvent.type == EventType.Layout)
                {
                    DrawGUI(sceneView);
                }
            }
        }

        /// <summary>
        /// Initialize resources
        /// </summary>
        /// <param name="sceneView">the current scene view</param>
        private static void Initialize(SceneView sceneView)
        {
            _isInitialized = true;
        }

        #region GUI
        /// <summary>
        /// Draws the whole gui
        /// </summary>
        /// <param name="sceneView"></param>
        private static void DrawGUI(SceneView sceneView)
        {
            Handles.BeginGUI();

            Rect spashScreenRect = Rect.zero;
            spashScreenRect = new Rect((int)(sceneView.position.width / 2) - (int)(_size.x / 2), (int)(sceneView.position.height / 2) - (int)(_size.y / 2), _size.x, _size.y);
            spashScreenRect = GUILayout.Window(0, spashScreenRect, DrawIntroductionScreens, "", GuiStyles.Background); 

            Handles.EndGUI();
        }

        /// <summary>
        /// Draws the screens
        /// </summary>
        /// <param name="controlId"></param>
        private static void DrawIntroductionScreens(int controlId)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginArea(new Rect(0, 0, _size.x, _size.y));
            GUILayout.BeginVertical();

            if(ShowMainIntroductionScreen)
            {
                if(_mainIntroductionScreenId == 0)
                {
                    _mainIntroductionScreenId = 1;
                }
                
                DrawMainIntroductionScreen();

                AuraEditorPrefs.DisplayMainIntroductionScreen = false;
            }
            else if (ShowCameraIntroductionScreen)
            {
                if (_cameraIntroductionScreenId == 0)
                {
                    _cameraIntroductionScreenId = 1;
                }

                DrawCameraIntroductionScreen();

                AuraEditorPrefs.DisplayCameraIntroductionScreen = false;
            }
            else if (ShowLightIntroductionScreen)
            {
                if (_lightIntroductionScreenId == 0)
                {
                    _lightIntroductionScreenId = 1;
                }

                DrawLightIntroductionScreen();

                AuraEditorPrefs.DisplayLightIntroductionScreen = false;
            }
            else if (ShowVolumeIntroductionScreen)
            {
                if (_volumeIntroductionScreenId == 0)
                {
                    _volumeIntroductionScreenId = 1;
                }

                DrawVolumeIntroductionScreen();

                AuraEditorPrefs.DisplayVolumeIntroductionScreen = false;
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUILayout.EndVertical();
        }
        
        /// <summary>
        /// Draws the main screen
        /// </summary>
        private static void DrawMainIntroductionScreen()
        {
            Rect drawnArea = new Rect(_margin, _margin, _size.x - _margin * 2, _size.y);

            //GUIStyle areaStyle = GuiStyles.ButtonNoHoverNoBorder;
            GUIStyle areaStyle = GuiStyles.Label;

            if (_mainIntroductionScreenId == 1)
            {
                drawnArea.y += 16;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.mainIntroductionScreenTextures[0]);
                GUILayout.Label(new GUIContent("Thank you for buying Aura 2"), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 20 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += 196;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Label(new GUIContent("Please read the provided documentation."), new GUIStyle(GuiStyles.LabelBold) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("It explains the philosophy behind Aura 2 and contains detailed explanations about all options, components and parameters included in Aura 2."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("Click Next to continue."), new GUIStyle(GuiStyles.LabelBold) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else if (_mainIntroductionScreenId == 2)
            {
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.logoTexture);
                GUILayout.Label(new GUIContent(" Aura Components"), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 17 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.logoTexture.height + 36;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Label(new GUIContent("Aura 2 operates using 3 types of Aura components applied to game objects : "), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += 28;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal(areaStyle);
                GUILayout.BeginHorizontal(GUILayout.Width(Aura.ResourcesCollection.cameraUiIconTexture.width));
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.cameraUiIconTexture);
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                GUILayout.BeginVertical();
                GUILayout.Space(10);
                GUILayout.Label(new GUIContent("<b>Aura Camera</b> components are in charge of collecting all the contributing data, processing the Aura system computation and displaying the volumetric lighting."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                drawnArea.y += 64;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal(areaStyle);
                GUILayout.BeginHorizontal(GUILayout.Width(Aura.ResourcesCollection.lightUiIconTexture.width));
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.lightUiIconTexture);
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                GUILayout.BeginVertical();
                GUILayout.Space(10);
                GUILayout.Label(new GUIContent("<b>Aura Light</b> components are in charge of collecting the light's data/parameters and providing them to the Aura system."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                drawnArea.y += 64;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal(areaStyle);
                GUILayout.BeginHorizontal(GUILayout.Width(Aura.ResourcesCollection.volumeUiIconTexture.width));
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.volumeUiIconTexture);
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                GUILayout.BeginVertical();
                GUILayout.Space(10);
                GUILayout.Label(new GUIContent("<b>Aura Volume</b> components are in charge of creating an injection volume, collecting its data/parameters and providing them to the Aura system."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();

                drawnArea.y += 64;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Label(new GUIContent("Aura components can be added/created using the <b>\"Add Component\"</b> button, the <b>\"Add Aura\"</b> button (on Cameras and Lights), the <b>\"GameObject -> Aura 2\"</b> menu or the Edition <b>Toolbox</b> integrated in the SceneView."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else if (_mainIntroductionScreenId == 3)
            {
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.logoTexture);
                GUILayout.Label(new GUIContent(" Edition Toolbox"), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 17 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.logoTexture.height + 36;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal(areaStyle);
                GUILayout.BeginHorizontal(GUILayout.Width(drawnArea.width / 5));
                GUILayout.Label(Aura.ResourcesCollection.mainIntroductionScreenTextures[1], GuiStyles.EmptyMiddleAligned);
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Space(24);
                GUILayout.Label(new GUIContent("Aura 2 introduces an <b>Edition Toolbox</b> that contains shortcut buttons."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("These shortcuts allow to easily and quickly add or edit volumetric lighting into your scenes."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("The <b>SCENE</b> panel contains buttons to quickly add Aura components on the scene's game objects.\nIt also contains a <b>Display Presets</b> button that will let you choose and apply amongst several <b>Aura Ambience</b> presets."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("The <b>CREATE</b> panel contains buttons to quickly create Aura objects in the scene."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
            else if (_mainIntroductionScreenId == 4)
            {
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.logoTexture);
                GUILayout.Label(new GUIContent(" Aura Ambience Presets"), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 17 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.logoTexture.height + 62;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal(areaStyle);
                GUILayout.BeginHorizontal(GUILayout.Width(drawnArea.width / 2.5f));
                GUILayout.Label(Aura.ResourcesCollection.mainIntroductionScreenTextures[4], GuiStyles.EmptyMiddleAligned);
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Space(24);
                GUILayout.Label(new GUIContent("The <b>Display Presets</b> button located in the <b>Toolbox</b> will open a panel containing several pre-established <b>Aura Ambiance</b> presets."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("In one click, you will be able to apply the desired <b>Aura Ambience</b> preset to your whole scene, giving you the perfect kickstart to integrate volumetric lighting into your project."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
            else if (_mainIntroductionScreenId == 5)
            {
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.logoTexture);
                GUILayout.Label(new GUIContent(" Aura Menu"), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 17 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.logoTexture.height + 48;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Label(new GUIContent("The <b>Aura 2 menu</b> can be found in \"Window -> Aura 2\"."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                GUILayout.Label(Aura.ResourcesCollection.mainIntroductionScreenTextures[2], GuiStyles.EmptyMiddleAligned);
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("This menu contains access to the <b>Aura Edition Settings</b> as well as other useful tools such as the <b>Texture3D Tool</b>."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            else if (_mainIntroductionScreenId == 6)
            {
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginVertical(areaStyle);
                GuiHelpers.DrawHeader(Aura.ResourcesCollection.logoTexture);
                GUILayout.Label(new GUIContent(" Aura Edition Settings"), new GUIStyle(GuiStyles.LabelBoldCenteredBig) { fontSize = 17 });
                GUILayout.EndVertical();
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.logoTexture.height + 36;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal(areaStyle);
                GUILayout.BeginHorizontal(GUILayout.Width(drawnArea.width / 2));
                GUILayout.Label(Aura.ResourcesCollection.mainIntroductionScreenTextures[3], GuiStyles.EmptyMiddleAligned);
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical(areaStyle);
                GUILayout.Space(56);
                GUILayout.Label(new GUIContent("The <b>Aura Edition Settings</b> window contains settings related to the edition environment."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("Settings such as <b>SceneView Toolbox</b> parameters and <b>SceneView Visualization</b> quality settings can be found there."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            Rect buttonsArea = new Rect(_size.x - _margin - 200, _size.y - _margin - 32, 64, 32);
            if (_mainIntroductionScreenId < 6)
            {
                GUILayout.BeginArea(buttonsArea);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Close", GuiStyles.Button, GUILayout.Width(64), GUILayout.Height(32)))
                {
                    _mainIntroductionScreenId = 0;
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            buttonsArea.x += 64 + 8;
            buttonsArea.width = 128;
            buttonsArea.y -= 8;
            buttonsArea.height += 8;
            GUILayout.BeginArea(buttonsArea);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_mainIntroductionScreenId == 6 ? "Close" : "Next", new GUIStyle(GuiStyles.ButtonBold) { fontSize = 17 }, GUILayout.Width(128), GUILayout.Height(40)))
            {
                ++_mainIntroductionScreenId;
                _mainIntroductionScreenId %= 7; 
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            
            if(_mainIntroductionScreenId > 1)
            {
                buttonsArea.x = _margin;
                GUILayout.BeginArea(buttonsArea);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Previous", new GUIStyle(GuiStyles.ButtonBold) { fontSize = 17 }, GUILayout.Width(128), GUILayout.Height(40)))
                {
                    --_mainIntroductionScreenId;
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }

        /// <summary>
        /// Draws the camera screen
        /// </summary>
        private static void DrawCameraIntroductionScreen()
        {
            Rect drawnArea = new Rect(_margin, _margin, _size.x - _margin * 2, _size.y);

            //GUIStyle areaStyle = GuiStyles.ButtonNoHoverNoBorder;
            GUIStyle areaStyle = GuiStyles.Label;

            if (_cameraIntroductionScreenId == 1)
            {
                GUILayout.BeginArea(drawnArea);
                EditorGUILayout.LabelField(new GUIContent(" Aura <b>Camera</b>", Aura.ResourcesCollection.cameraUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.cameraUiIconTexture.height + 16;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("<b>Aura Camera</b> objects are responsible of two main tasks."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(Aura.ResourcesCollection.cameraIntroductionScreenTextures[0], GuiStyles.EmptyMiddleAligned);
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.cameraIntroductionScreenTextures[0].height + 32;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("The first function is to establish a foundation for the volumetric lighting, using a <b>Aura Base Settings</b> preset file.", Aura.ResourcesCollection.baseSettingsPresetIconTexture), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();

                drawnArea.y += 64;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("The second responsibility is to setup the quality of the volumetric lighting computation, using a <b>Aura Quality Settings</b> preset file.", Aura.ResourcesCollection.qualitySettingsPresetIconTexture), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("Using those two collections of parameters, the <b>Aura Camera</b> component will then gather all the data from the contributing Aura components, process and display the volumetric lighting computation."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();
            }

            drawnArea.y = _size.y - _margin - 30;
            GUILayout.BeginArea(drawnArea);
            GUILayout.Label(new GUIContent("Please refer to the Documentation for more detailed information."), new GUIStyle(GuiStyles.LabelBig));
            GUILayout.EndArea();

            Rect buttonsArea = new Rect(_size.x - _margin - 200, _size.y - _margin - 32, 64, 32);
            buttonsArea.x += 64 + 8;
            buttonsArea.width = 128;
            buttonsArea.y -= 8;
            buttonsArea.height += 8;
            GUILayout.BeginArea(buttonsArea);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close", new GUIStyle(GuiStyles.ButtonBold) { fontSize = 17 }, GUILayout.Width(128), GUILayout.Height(40)))
            {
                _cameraIntroductionScreenId = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        /// <summary>
        /// Draws the light screen
        /// </summary>
        private static void DrawLightIntroductionScreen()
        {
            Rect drawnArea = new Rect(_margin, _margin, _size.x - _margin * 2, _size.y);

            //GUIStyle areaStyle = GuiStyles.ButtonNoHoverNoBorder;
            GUIStyle areaStyle = GuiStyles.Label;

            if (_lightIntroductionScreenId == 1)
            {
                GUILayout.BeginArea(drawnArea);
                EditorGUILayout.LabelField(new GUIContent(" Aura <b>Light</b>", Aura.ResourcesCollection.lightUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.cameraUiIconTexture.height + 16;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("<b>Aura Light</b> objects are responsible of collecting the data related to the Light and feeding them to the Aura system."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                EditorGUILayout.Separator();
                GUILayout.Label(new GUIContent("<b>Aura Light</b> components are divided in two panels."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();

                drawnArea.y += 104;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal();
                GUILayout.Label(Aura.ResourcesCollection.illuminationIconTexture, GuiStyles.EmptyMiddleAligned, GUILayout.Width(32), GUILayout.Height(32));
                GUILayout.BeginVertical();
                GUILayout.Space(4);
                GUILayout.Label("<b>Common Parameters</b> panel", new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.Label(new GUIContent("This panel contains settings that are identical to all types of lights."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();

                drawnArea.y += 96;
                GUILayout.BeginArea(drawnArea);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Directional", Aura.ResourcesCollection.directionalLightIconTexture), new GUIStyle(GuiStyles.LabelBold) { fontSize = 15 }, GUILayout.Width(134));
                GUILayout.Label(new GUIContent("Spot", Aura.ResourcesCollection.spotLightIconTexture), new GUIStyle(GuiStyles.LabelBold) { fontSize = 15 }, GUILayout.Width(86));
                GUILayout.Label(new GUIContent("Point ", Aura.ResourcesCollection.pointLightIconTexture), new GUIStyle(GuiStyles.LabelBold) { fontSize = 15 }, GUILayout.Width(90));
                GUILayout.BeginVertical();
                GUILayout.Space(7);
                GUILayout.Label("<b>Parameters</b> panels", new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.Label(new GUIContent("These panels contain settings that are unique to the type of the selected light."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();
            }

            drawnArea.y = _size.y - _margin - 30;
            GUILayout.BeginArea(drawnArea);
            GUILayout.Label(new GUIContent("Please refer to the Documentation for more detailed information."), new GUIStyle(GuiStyles.LabelBig));
            GUILayout.EndArea();

            Rect buttonsArea = new Rect(_size.x - _margin - 200, _size.y - _margin - 32, 64, 32);
            buttonsArea.x += 64 + 8;
            buttonsArea.width = 128;
            buttonsArea.y -= 8;
            buttonsArea.height += 8;
            GUILayout.BeginArea(buttonsArea);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close", new GUIStyle(GuiStyles.ButtonBold) { fontSize = 17 }, GUILayout.Width(128), GUILayout.Height(40)))
            {
                _lightIntroductionScreenId = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        /// <summary>
        /// Draws the volume screen
        /// </summary>
        private static void DrawVolumeIntroductionScreen()
        {
            Rect drawnArea = new Rect(_margin, _margin, _size.x - _margin * 2, _size.y);

            //GUIStyle areaStyle = GuiStyles.ButtonNoHoverNoBorder;
            GUIStyle areaStyle = GuiStyles.Label;

            if (_volumeIntroductionScreenId == 1)
            {
                GUILayout.BeginArea(drawnArea);
                EditorGUILayout.LabelField(new GUIContent(" Aura <b>Volume</b>", Aura.ResourcesCollection.volumeUiIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
                GUILayout.EndArea();

                drawnArea.y += Aura.ResourcesCollection.cameraUiIconTexture.height + 16;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("<b>Aura Volume</b> objects are responsible of injecting data inside the volume."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.Label(new GUIContent("Three types of data can be injected inside the volume : fog <b>Density</b>, pure <b>Light</b> and lighting <b>Scattering</b> factor."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();

                drawnArea.y += 104;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("<b>Volume Mask</b> panel", Aura.ResourcesCollection.shapeIconTexture), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.Label(new GUIContent("This panel contains the parameters used to setup the injection mask. This mask will be used to mask out the <b>Injected Data</b>."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();

                drawnArea.y += 96;
                GUILayout.BeginArea(drawnArea);
                GUILayout.Label(new GUIContent("<b>Inject Data</b> panel", Aura.ResourcesCollection.injectionIconTexture), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.Label(new GUIContent("This panel contains the data you will be able to inject and their parameters. These data will be masked out by the <b>Volume Mask</b>."), new GUIStyle(GuiStyles.Label) { fontSize = 15 });
                GUILayout.EndArea();
            }

            drawnArea.y = _size.y - _margin - 30;
            GUILayout.BeginArea(drawnArea);
            GUILayout.Label(new GUIContent("Please refer to the Documentation for more detailed information."), new GUIStyle(GuiStyles.LabelBig));
            GUILayout.EndArea();

            Rect buttonsArea = new Rect(_size.x - _margin - 200, _size.y - _margin - 32, 64, 32);
            buttonsArea.x += 64 + 8;
            buttonsArea.width = 128;
            buttonsArea.y -= 8;
            buttonsArea.height += 8;
            GUILayout.BeginArea(buttonsArea);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close", new GUIStyle(GuiStyles.ButtonBold) { fontSize = 17 }, GUILayout.Width(128), GUILayout.Height(40)))
            {
                _volumeIntroductionScreenId = 0;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        #endregion
        #endregion
    }
}
