
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
    /// Static class containing extension for Vector4 type
    /// </summary>
    public static class Vector4Extensions
    {
        #region Private Members
        /// <summary>
        /// Temporary object to avoid allocation
        /// </summary>
        private static float[] _tmpFloatArray = new float[4];
        /// <summary>
        /// Temporary object to avoid allocation
        /// </summary>
        private static Vector4 _tmpVector4 = new Vector4();
        #endregion

        #region Functions
        /// <summary>
        /// Formats a Vector4 into a array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] AsFloatArray(this Vector4 vector)
        {
            _tmpFloatArray[0] = vector.x;
            _tmpFloatArray[1] = vector.y;
            _tmpFloatArray[2] = vector.z;
            _tmpFloatArray[3] = vector.w;

            return _tmpFloatArray;
        }

        /// <summary>
        /// Formats an array of Vector4 into a array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] AsFloatArray(this Vector4[] vector)
        {
            float[] floatArray = new float[vector.Length * 4];

            for(int i = 0; i < vector.Length; ++i)
            {
                floatArray[i * 4] = vector[i].x;
                floatArray[i * 4 + 1] = vector[i].y;
                floatArray[i * 4 + 2] = vector[i].z;
                floatArray[i * 4 + 3] = vector[i].w;
            }

            return floatArray;
        }

        /// <summary>
        /// Computes the reciproqual vector
        /// </summary>
        /// <returns>1.0f / vector</returns>
        public static Vector4 GetReciproqual(this Vector4 vector)
        {
            _tmpVector4.x = 1.0f / vector.x;
            _tmpVector4.y = 1.0f / vector.y;
            _tmpVector4.z = 1.0f / vector.z;
            _tmpVector4.w = 1.0f / vector.w;

            return _tmpVector4;
        }
        #endregion
    }
}
