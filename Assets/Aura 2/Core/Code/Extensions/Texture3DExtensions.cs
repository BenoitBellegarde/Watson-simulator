
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
    public static class Texture3DExtensions
    {
        #region Private Members
        /// <summary>
        /// Temporary object to avoid allocation
        /// </summary>
        private static Vector3 _tmpVector3 = new Vector3();
        #endregion

        #region Functions
        /// <summary>
        /// Returns the size of the Texture3D in a Vector3
        /// </summary>
        /// <returns>A Vector3 containing the width, height and depth of the Texture3D</returns>
        public static Vector3 GetSize(this Texture3D texture)
        {
            _tmpVector3.x = texture.width;
            _tmpVector3.y = texture.height;
            _tmpVector3.z = texture.depth;

            return _tmpVector3;
        }
        #endregion
    }
}
