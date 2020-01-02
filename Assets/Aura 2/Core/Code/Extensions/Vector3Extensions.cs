
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
    /// Static class containing extension for Vector3 type
    /// </summary>
    public static class Vector3Extensions
    {
        #region Private Members
        /// <summary>
        /// Temporary object to avoid allocation
        /// </summary>
        private static float[] _tmpFloatArray = new float[3];
        /// <summary>
        /// Temporary object to avoid allocation
        /// </summary>
        private static Vector4 _tmpVector4 = new Vector4();
        #endregion

        #region Functions
        /// <summary>
        /// Formats a Vector3 into a array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] AsFloatArray(this Vector3 vector)
        {
            _tmpFloatArray[0] = vector.x;
            _tmpFloatArray[1] = vector.y;
            _tmpFloatArray[2] = vector.z;

            return _tmpFloatArray;
        }

        /// <summary>
        /// Formats an array of Vector3 into a array of floats
        /// </summary>
        /// <returns>The array of floats</returns>
        public static float[] AsFloatArray(this Vector3[] vector)
        {
            float[] floatArray = new float[vector.Length * 3];

            for(int i = 0; i < vector.Length; ++i)
            {
                floatArray[i * 3] = vector[i].x;
                floatArray[i * 3 + 1] = vector[i].y;
                floatArray[i * 3 + 2] = vector[i].z;
            }

            return floatArray;
        }

        /// <summary>
        /// Non-Alloc casts the Vector3 into a Vector4 allowing to specify the fourth value
        /// </summary>
        /// <param name="fourthValue">The value stored in W</param>
        /// <returns>The new Vector4</returns>
        public static Vector4 AsVector4(this Vector3 vector, float fourthValue)
        {
            _tmpVector4.x = vector.x;
            _tmpVector4.y = vector.y;
            _tmpVector4.z = vector.z;
            _tmpVector4.w = fourthValue;

            return _tmpVector4;
        }
        #endregion
    }
}
