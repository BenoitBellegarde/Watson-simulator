
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

namespace Aura2API
{
    /// <summary>
    /// Texture3D mask to be used in a AuraVolume
    /// </summary>
    [Serializable]
    public struct Texture3DMask
    {
        #region Public Members
        /// <summary>
        /// Enables the texture mask
        /// </summary>
        public bool enable;
        /// <summary>
        /// The source Texture3D
        /// </summary>
        [Texture3DPreview]
        public Texture3D texture;
        /// <summary>
        /// Index of the texture inside the atlas
        /// </summary>
        public int textureIndex;
        /// <summary>
        /// Allows to set base position, rotation and scale
        /// </summary>
        public TransformParameters transform;
        #endregion

        #region Functions
        /// <summary>
        /// Sets default values
        /// </summary>
        public void SetDefaultValues()
        {
            transform.space = Space.Self;
            transform.position = Vector3.zero;
            transform.rotation = Vector3.zero;
            transform.scale = Vector3.one;

            textureIndex = -1;
        }
        #endregion
    }
}
