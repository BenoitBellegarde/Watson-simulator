
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

namespace Aura2API
{
    /// <summary>
    /// Base volume injection parameters for density/scattering/color
    /// </summary>
    [Serializable]
    public struct VolumeInjectionCommonParameters
    {
        /// <summary>
        /// Enables the injection
        /// </summary>
        public bool enable;
        /// <summary>
        /// Sets the strength of the injection
        /// </summary>
        public float strength;
        /// <summary>
        /// Applies the Noise Mask
        /// </summary>
        public bool useNoiseMask;
        /// <summary>
        /// Enables levels on the Noise Mask
        /// </summary>
        public bool useNoiseMaskLevels;
        /// <summary>
        /// Levels parameters for the noise mask. Similar to the Levels adjustement tool in Photoshop
        /// </summary>
        public LevelsParameters noiseMaskLevelParameters;
        /// <summary>
        /// Applies the Texture2D Mask
        /// </summary>
        public bool useTexture2DMask;
        /// <summary>
        /// Enables levels on the Texture2D Mask
        /// </summary>
        public bool useTexture2DMaskLevels;
        /// <summary>
        /// Levels parameters for the Texture2D mask. Similar to the Levels adjustement tool in Photoshop
        /// </summary>
        public LevelsParameters texture2DMaskLevelParameters;
        /// <summary>
        /// Applies the Texture3D Mask
        /// </summary>
        public bool useTexture3DMask;
        /// <summary>
        /// Enables levels on the Texture3D Mask
        /// </summary>
        public bool useTexture3DMaskLevels;
        /// <summary>
        /// Levels parameters for the Texture3D mask. Similar to the Levels adjustement tool in Photoshop
        /// </summary>
        public LevelsParameters texture3DMaskLevelParameters;
    }
}
