
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
    /// Ordered struct of Vector4 representing a matrix to be sent to the compute shader
    /// </summary>
    public struct MatrixFloats
    {
        #region Public Members
        /// <summary>
        /// Matric column A
        /// </summary>
        public Vector4 a;
        /// <summary>
        /// Matric column B
        /// </summary>
        public Vector4 b;
        /// <summary>
        /// Matric column C
        /// </summary>
        public Vector4 c;
        /// <summary>
        /// Matric column D
        /// </summary>
        public Vector4 d;
        #endregion

        #region Private Members
        /// <summary>
        /// The bytes size of the struct
        /// </summary>
        private static int _byteSize = 0;
        /// <summary>
        /// Temporary object to avoid allocation
        /// </summary>
        private static MatrixFloats _tmpMatrixFloats;
        #endregion

        #region Properties
        /// <summary>
        /// The bytes size of the struct
        /// </summary>
        public static int Size
        {
            get
            {
                if (_byteSize == 0)
                {
                    _byteSize = sizeof(float) * 16;
                }

                return _byteSize;
            }
        }

        /// <summary>
        /// Converts a Matrix4x4 to MatrixFloats format
        /// </summary>
        /// <param name="matrix">The matrix to be converted</param>
        /// <returns>The matrix converted into the MatrixFloats format</returns>
        public static MatrixFloats ToMatrixFloats(Matrix4x4 matrix)
        {
            _tmpMatrixFloats.a = matrix.GetColumn(0);
            _tmpMatrixFloats.b = matrix.GetColumn(1);
            _tmpMatrixFloats.c = matrix.GetColumn(2);
            _tmpMatrixFloats.d = matrix.GetColumn(3);

            return _tmpMatrixFloats;
        }
        #endregion
    }
}
