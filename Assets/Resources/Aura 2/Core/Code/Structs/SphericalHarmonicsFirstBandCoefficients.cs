
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
    /// Struct containing spherical harmonics coefficients to use probes in shaders
    /// </summary>
    public struct SphericalHarmonicsFirstBandCoefficients
    {
        #region Public Members
        public Vector4 shAr;
        public Vector4 shAg;
        public Vector4 shAb;
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
                    _byteSize += sizeof(float) * 4; //shAr
                    _byteSize += sizeof(float) * 4; //shAb
                    _byteSize += sizeof(float) * 4; //shAg
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
