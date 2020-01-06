
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

namespace Aura2API
{
    /// <summary>
    /// Ordered struct of texture mask parameters to be sent to the compute shader
    /// </summary>
    public struct TextureMaskData
    {
        #region Public Members
        /// <summary>
        /// The tranform of the texture
        /// </summary>
        public MatrixFloats transform;
        /// <summary>
        /// The index of the texture in the composed volumetric texture. The "enable" parameter is included in this as it is set to -1 if enable == false
        /// </summary>
        public int index;
        #endregion

        #region Private Members
        /// <summary>
        /// The bytes size of the struct
        /// </summary>
        private static int _byteSize = 0;
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
                    _byteSize += MatrixFloats.Size;  // transform
                    _byteSize += sizeof(int);        // index
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
