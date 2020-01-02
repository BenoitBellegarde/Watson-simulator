
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
    /// Texture2DArray composer used for collecting shadow maps
    /// </summary>
    public class ShadowmapsCollector : Texture2DArrayComposer
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sizeX">The width of the built Texture2DArray</param>
        /// <param name="sizeY">The height of the built Texture2DArray</param>
        public ShadowmapsCollector(int sizeX, int sizeY) : base(sizeX, sizeY, TextureFormat.RGBAFloat, true)
        {
            alwaysGenerateOnUpdate = true;
        }
        #endregion
    }
}
