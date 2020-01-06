
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
    /// Collection of extension functions for Matrix4x4 objects
    /// </summary>
    public static class Matrix4X4Extensions
    {
        #region Functions
        /// <summary>
        /// Converts the matrix to the MatrixFloats format
        /// </summary>
        /// <returns>The converted MatrixFloats</returns>
        public static MatrixFloats ToAuraMatrixFloats(this Matrix4x4 matrix)
        {
            return MatrixFloats.ToMatrixFloats(matrix);
        }

        /// <summary>
        /// Converts the matrix to an array of floats
        /// </summary>
        /// <param name="floatsArrayToFill">Pre allocated floats array to fill</param>
        /// <returns>The array of floats</returns>
        public static void ToFloatArray(this Matrix4x4 matrix, ref float[] floatsArrayToFill)
        {
            floatsArrayToFill[ 0 ] = matrix[ 0, 0 ];
            floatsArrayToFill[ 1 ] = matrix[ 1, 0 ];
            floatsArrayToFill[ 2 ] = matrix[ 2, 0 ];
            floatsArrayToFill[ 3 ] = matrix[ 3, 0 ];
            floatsArrayToFill[ 4 ] = matrix[ 0, 1 ];
            floatsArrayToFill[ 5 ] = matrix[ 1, 1 ];
            floatsArrayToFill[ 6 ] = matrix[ 2, 1 ];
            floatsArrayToFill[ 7 ] = matrix[ 3, 1 ];
            floatsArrayToFill[ 8 ] = matrix[ 0, 2 ];
            floatsArrayToFill[ 9 ] = matrix[ 1, 2 ];
            floatsArrayToFill[ 10 ] = matrix[ 2, 2 ];
            floatsArrayToFill[ 11 ] = matrix[ 3, 2 ];
            floatsArrayToFill[ 12 ] = matrix[ 0, 3 ];
            floatsArrayToFill[ 13 ] = matrix[ 1, 3 ];
            floatsArrayToFill[ 14 ] = matrix[ 2, 3 ];
            floatsArrayToFill[ 15 ] = matrix[ 3, 3 ];
        }
        #endregion
    }
}
