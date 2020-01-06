
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
    /// Ordered struct of noise parameters to be sent to the compute shader
    /// </summary>
    public struct VolumeDynamicNoiseData
    {
        #region Public Members
        /// <summary>
        /// Enables the dynamic noise computation
        /// </summary>
        public int enable;
        /// <summary>
        /// The tranform of the noise
        /// </summary>
        public MatrixFloats transform;
        /// <summary>
        /// The speed of the noise mutation
        /// </summary>
        public float speed;
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
                if(_byteSize == 0)
                {
                    _byteSize += sizeof(int);        // enable
                    _byteSize += MatrixFloats.Size;  // transform
                    _byteSize += sizeof(float);      // speed
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
