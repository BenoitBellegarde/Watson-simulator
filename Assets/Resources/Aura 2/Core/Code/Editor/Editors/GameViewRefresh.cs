
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
    /// Autoloading class that will make the game view refresh when focussed
    /// </summary>
    [InitializeOnLoad]
    public class GameViewRefresh
    {
        #region Constructor
        static GameViewRefresh()
        {
            Camera.onPostRender += OnCameraPostRender;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Called after every render
        /// </summary>
        /// <param name="camera">The camera tha just finished rendering</param>
        private static void OnCameraPostRender(Camera camera)
        {
            EditorWindow window = EditorWindow.focusedWindow;
            bool isGameViewFocused = window != null && window.GetType().ToString() == "UnityEditor.GameView";
            if (!Application.isPlaying && isGameViewFocused)
            {
                window.Repaint();
            }
        }
        #endregion
    }
}
