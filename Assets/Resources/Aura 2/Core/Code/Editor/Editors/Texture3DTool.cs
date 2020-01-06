
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

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Tool for generating Texture3D out of a Texture2D containing slices
    /// </summary>
    public class Texture3DTool : EditorWindow
    {
        #region Private Members
        /// <summary>
        /// The Texture2D used as source
        /// </summary>
        private Texture2D _sourceTexture;
        /// <summary>
        /// The reference size used to cut the source texture and build the Texture3D
        /// </summary>
        private int _referenceSize;
        /// <summary>
        /// The order of reading the slices in the source texture
        /// </summary>
        private Texture2DReadingOrder _readingOrder;
        /// <summary>
        /// The path of the source texture asset
        /// </summary>
        private string _sourceTexturePath;
        /// <summary>
        /// Position of the scroll
        /// </summary>
        private Vector2 _scrollPosition;
        #endregion

        #region Properties
        /// <summary>
        /// Tells if a source texture was provided
        /// </summary>
        private bool HasSourceTexture
        {
            get
            {
                return _sourceTexture != null;
            }
        }

        /// <summary>
        /// Tells the amount of horizontal tiles based on the reference size
        /// </summary>
        public int HorizontalTilesCount
        {
            get
            {
                return HasSourceTexture ? _sourceTexture.width / _referenceSize : 0;
            }
        }

        /// <summary>
        /// Tells the amount of vertical tiles based on the reference size
        /// </summary>
        public int VerticalTilesCount
        {
            get
            {
                return HasSourceTexture ? _sourceTexture.height / _referenceSize : 0;
            }
        }

        /// <summary>
        /// Tells the total amount of tiles based on the reference size
        /// </summary>
        public int TotalTilesCount
        {
            get
            {
                return HasSourceTexture ? HorizontalTilesCount * VerticalTilesCount : 0;
            }
        }

        /// <summary>
        /// tells if the parameters are valid for generating the Texture3D
        /// </summary>
        public bool AreParametersValid
        {
            get
            {
                return HasSourceTexture && /*referenceSize >= 16 &&*/ TotalTilesCount == _referenceSize;
            }
        }
        #endregion

        #region Overriden base class functions (https://docs.unity3d.com/ScriptReference/EditorWindow.html)
        [MenuItem("Window/Aura 2/Texture3D Tool", priority = 50)]
        private static void Init()
        {
            Texture3DTool window = (Texture3DTool)EditorWindow.GetWindow(typeof(Texture3DTool));
            window.titleContent.text = "Texture3D Tool";
            window.Show();
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            EditorGUILayout.BeginVertical(GuiStyles.Background);
            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical(GuiStyles.ButtonNoHover);
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal(GuiStyles.BackgroundNoBorder);
            EditorGUILayout.LabelField(new GUIContent(" Aura <b>Texture3D Tool</b>", Aura.ResourcesCollection.logoIconTexture), new GUIStyle(GuiStyles.LabelCenteredBig) { fontSize = 24 });
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical(GuiStyles.Background);
            GUILayout.Label(new GUIContent(" Create Texture3D", Aura.ResourcesCollection.texture3DIconTexture, "Create a composite Texture3D from a Texture2D"), GuiStyles.LabelBoldCenteredBig);
            EditorGUILayout.Separator();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Source Texture2D", "Source texture used to build the Texture3D"), GuiStyles.Label);
            _sourceTexture = (Texture2D)EditorGUILayout.ObjectField(_sourceTexture, typeof(Texture2D), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if(_sourceTexture != null)
            {
                _sourceTexturePath = AssetDatabase.GetAssetPath(_sourceTexture);
                TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(_sourceTexturePath);

                if(textureImporter.isReadable)
                {
                    if (textureImporter.textureCompression == TextureImporterCompression.Uncompressed)
                    {
                        EditorGUILayout.Separator();
                    
                        _referenceSize = EditorGUILayout.DelayedIntField(new GUIContent("Reference Size", "Target size of the 3D texture"), _referenceSize);
                        _referenceSize = Mathf.Max(_referenceSize, 2);
                        _referenceSize = Mathf.ClosestPowerOfTwo(_referenceSize);

                        EditorGUILayout.Separator();

                        GuiHelpers.DrawHelpBox("Horizontal tiles count : " + HorizontalTilesCount + "\nVertical tiles count : " + VerticalTilesCount + "\n\nTotal amount of tiles : " + TotalTilesCount, HelpBoxType.Question);

                        EditorGUILayout.Separator();

                        if(AreParametersValid)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label(new GUIContent("Reading Order", "The order of which the tiles should be assembled to create the 3D texture (should we assemble line by line or column by column?)"), GuiStyles.Label);
                            _readingOrder = (Texture2DReadingOrder)EditorGUILayout.EnumPopup(_readingOrder);
                            EditorGUILayout.EndHorizontal();
                        
                            EditorGUILayout.Separator();
                        
                            if(GUILayout.Button(new GUIContent("Generate", "Generates a new Texture3D from the source texture"), GuiStyles.ButtonBigBold))
                            {
                                GenerateVolumetricTexture(_sourceTexture);
                            }
                        }
                        else
                        {
                            GuiHelpers.DrawHelpBox("The total amount of tiles should be equal to the reference tile size.\nIn this case : " + _referenceSize, HelpBoxType.Warning);
                        }
                    }
                    else
                    {
                        GuiHelpers.DrawHelpBox("The source Texture2D asset should not use texture compression", HelpBoxType.Warning);
                        EditorGUILayout.Separator();
                        if (GUILayout.Button(new GUIContent("Disable texture compression"), GuiStyles.ButtonBigBold))
                        {
                            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                            textureImporter.SaveAndReimport();
                        }
                    }
                }
                else
                {
                    GuiHelpers.DrawHelpBox("The source Texture2D asset should be marked as \"READABLE\"", HelpBoxType.Warning);
                    EditorGUILayout.Separator();
                    if(GUILayout.Button(new GUIContent("Mark As \"READABLE\""), GuiStyles.ButtonBigBold))
                    {
                        textureImporter.isReadable = true;
                        textureImporter.SaveAndReimport();
                    }
                }

                EditorGUILayout.Separator();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Generates the Texture3D
        /// </summary>
        /// <param name="sourceTexture">The source texture asset</param>
        /// <returns>True if parameters were valid, false otherwise</returns>
        public bool GenerateVolumetricTexture(Texture2D sourceTexture)
        {
            if(AreParametersValid)
            {
                Texture3D volumetricTexture = new Texture3D(_referenceSize, _referenceSize, _referenceSize, sourceTexture.format, false);
                volumetricTexture.wrapMode = sourceTexture.wrapMode;
                volumetricTexture.wrapModeU = sourceTexture.wrapModeU;
                volumetricTexture.wrapModeV = sourceTexture.wrapModeV;
                volumetricTexture.wrapModeW = sourceTexture.wrapModeW;
                volumetricTexture.filterMode = sourceTexture.filterMode;
                volumetricTexture.mipMapBias = 0;
                volumetricTexture.anisoLevel = 0;

                Color[] colorArray = new Color[0];
                switch(_readingOrder)
                {
                    case Texture2DReadingOrder.RowMajor :
                        {
                            for(int i = 0; i < HorizontalTilesCount; ++i)
                            {
                                for(int j = 0; j < VerticalTilesCount; ++j)
                                {
                                    colorArray = colorArray.Append(sourceTexture.GetPixels(i * _referenceSize, j * _referenceSize, _referenceSize, _referenceSize));
                                }
                            }
                        }

                        break;

                    case Texture2DReadingOrder.ColumnMajor :
                        {
                            for(int i = VerticalTilesCount - 1; i >= 0; --i)
                            {
                                for(int j = 0; j < HorizontalTilesCount; ++j)
                                {
                                    colorArray = colorArray.Append(sourceTexture.GetPixels(j * _referenceSize, i * _referenceSize, _referenceSize, _referenceSize));
                                }
                            }
                        }

                        break;
                }

                volumetricTexture.SetPixels(colorArray);
                volumetricTexture.Apply();

                AssetDatabase.CreateAsset(volumetricTexture, Directory.GetParent(AssetDatabase.GetAssetPath(sourceTexture)) + "\\" + sourceTexture.name + "_Texture3D.asset");

                return true;
            }

            return false;
        }
        #endregion
    }
}
