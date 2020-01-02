
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
    /// Extensions for Texture3D object so we can just invoke functions on them
    /// </summary>
    public static class Texture3DEditorExtensions
    {
        #region Private Members
        /// <summary>
        /// The material used to render the Texture3D
        /// </summary>
        private static Material _previewTexture3DMaterial;
        /// <summary>
        /// The texture that will be used for the background
        /// </summary>
        private static Texture2D _backgroundTexture;
        /// <summary>
        /// The GUIStyle that will be used for the background
        /// </summary>
        private static GUIStyle _backgroundGuiStyle;
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the material used to render the Texture3D
        /// </summary>
        private static Material PreviewTexture3DMaterial
        {
            get
            {
                if (Texture3DEditorExtensions._previewTexture3DMaterial == null)
                {
                    Shader shader = Shader.Find("Hidden/Aura2/DrawTexture3DPreview");
                    if (shader != null)
                    {
                        Texture3DEditorExtensions._previewTexture3DMaterial = new Material(shader);
                    }
                }

                return Texture3DEditorExtensions._previewTexture3DMaterial;
            }
        }

        /// <summary>
        /// Accessor to the static background texture
        /// </summary>
        public static Texture2D BackgroundTexture
        {
            get
            {
                if (Texture3DEditorExtensions._backgroundTexture == null)
                {
                    Texture3DEditorExtensions._backgroundTexture = new Texture2D(1, 1);
                    Texture3DEditorExtensions._backgroundTexture.SetPixel(0, 0, Color.gray * 0.5f);
                    Texture3DEditorExtensions._backgroundTexture.Apply();
                }

                return Texture3DEditorExtensions._backgroundTexture;
            }
        }

        /// <summary>
        /// Accessor to the static background GUIStyle
        /// </summary>
        public static GUIStyle BackgroundGuiStyle
        {
            get
            {
                Texture3DEditorExtensions._backgroundGuiStyle = new GUIStyle();
                Texture3DEditorExtensions._backgroundGuiStyle.active.background = Texture3DEditorExtensions.BackgroundTexture;
                Texture3DEditorExtensions._backgroundGuiStyle.focused.background = Texture3DEditorExtensions.BackgroundTexture;
                Texture3DEditorExtensions._backgroundGuiStyle.hover.background = Texture3DEditorExtensions.BackgroundTexture;
                Texture3DEditorExtensions._backgroundGuiStyle.normal.background = Texture3DEditorExtensions.BackgroundTexture;

                return Texture3DEditorExtensions._backgroundGuiStyle;
            }
        }

        /// <summary>
        /// Sets the parameters to the PreviewRenderUtility and calls the rendering
        /// </summary>
        /// <param name="texture3D">The Texture3D to preview</param>
        /// <param name="angle">The camera angle</param>
        /// <param name="distance">The distance of the camera to the preview cube</param>
        /// <param name="samplingIterations">The amount of slices used to raymarch in the Texture3D</param>
        /// <param name="density">A linear factor to multiply the Texture3D with</param>
        private static void RenderInPreviewRenderUtility(Texture3D texture3D, Vector2 angle, float distance, int samplingIterations, float density)
        {
            Texture3DEditorExtensions.PreviewTexture3DMaterial.SetInt("_SamplingQuality", samplingIterations);
            Texture3DEditorExtensions.PreviewTexture3DMaterial.SetTexture("_MainTex", texture3D);
            Texture3DEditorExtensions.PreviewTexture3DMaterial.SetFloat("_Density", density);

            PreviewRenderUtilityHelpers.Instance.DrawMesh(MeshHelpers.Cube, Matrix4x4.identity, Texture3DEditorExtensions.PreviewTexture3DMaterial, 0);

            PreviewRenderUtilityHelpers.Instance.camera.transform.position = Vector2.zero;
            PreviewRenderUtilityHelpers.Instance.camera.transform.rotation = Quaternion.Euler(new Vector3(-angle.y, -angle.x, 0));
            PreviewRenderUtilityHelpers.Instance.camera.transform.position = PreviewRenderUtilityHelpers.Instance.camera.transform.forward * -distance;
            PreviewRenderUtilityHelpers.Instance.camera.Render();
        }

        /// <summary>
        /// Renders a preview of the Texture3D
        /// </summary>
        /// <param name="texture3D">The Texture3D to preview</param>
        /// <param name="rect">The area where the preview is located</param>
        /// <param name="angle">The camera angle</param>
        /// <param name="distance">The distance of the camera to the preview cube</param>
        /// <param name="samplingIterations">The amount of slices used to raymarch in the Texture3D</param>
        /// <param name="density">A linear factor to multiply the Texture3D with</param>
        /// <returns>A Texture with the preview</returns>
        public static Texture RenderTexture3DPreview(this Texture3D texture3D, Rect rect, Vector2 angle, float distance, int samplingIterations, float density)
        {
            try
            {
                PreviewRenderUtilityHelpers.Instance.BeginPreview(rect, BackgroundGuiStyle);

                RenderInPreviewRenderUtility(texture3D, angle, distance, samplingIterations, density);

                return PreviewRenderUtilityHelpers.Instance.EndPreview();
            }
            catch
            {
                return Aura.ResourcesCollection.texture3DIconTexture;
            }
        }

        /// <summary>
        /// Renders a thumbnail of the Texture3D
        /// </summary>
        /// <param name="texture3D">The Texture3D to preview</param>
        /// <param name="rect">The area where the preview is located</param>
        /// <param name="angle">The camera angle</param>
        /// <param name="distance">The distance of the camera to the preview cube</param>
        /// <param name="samplingIterations">The amount of slices used to raymarch in the Texture3D</param>
        /// <param name="density">A linear factor to multiply the Texture3D with</param>
        /// <returns>A Texture2D with the thumbnail</returns>
        public static Texture2D RenderTexture3DStaticPreview(this Texture3D texture3D, Rect rect, Vector2 angle, float distance, int samplingIterations, float density)
        {
            try
            {
                PreviewRenderUtilityHelpers.Instance.BeginStaticPreview(rect);

                RenderInPreviewRenderUtility(texture3D, angle, distance, samplingIterations, density);

                return PreviewRenderUtilityHelpers.Instance.EndStaticPreview();
            }
            catch
            {
                return Aura.ResourcesCollection.texture3DIconTexture;
            }
        }
        #endregion
    }
}
