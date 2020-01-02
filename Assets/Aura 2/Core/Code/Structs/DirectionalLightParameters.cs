
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
    /// Struct containing parameters of a directional AuraLight
    /// </summary>
    public struct DirectionalLightParameters
    {
        #region Public Members
        public Vector3 color;
        public int useDefaultScattering;
        public float scatteringOverride;
        public Vector3 lightPosition;
        public Vector3 lightDirection;
        public MatrixFloats worldToLightMatrix;
        public MatrixFloats lightToWorldMatrix;
        public int shadowMapIndex;
        public int cookieMapIndex;
        public Vector2 cookieParameters;
        public int enableOutOfPhaseColor;
        public Vector3 outOfPhaseColor;
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
                    _byteSize += sizeof(float) * 3;      //color
                    _byteSize += sizeof(int);            //useDefaultScattering
                    _byteSize += sizeof(float);          //scatteringBias
                    _byteSize += sizeof(float) * 3;      //lightPosition
                    _byteSize += sizeof(float) * 3;      //lightDirection
                    _byteSize += MatrixFloats.Size;      //worldToLightMatrix
                    _byteSize += MatrixFloats.Size;      //lightToWorldMatrix
                    _byteSize += sizeof(int);            //shadowMapIndex
                    _byteSize += sizeof(int);            //cookieMapIndex
                    _byteSize += sizeof(float) * 2;      //cookieParameters
                    _byteSize += sizeof(int);            //enableOutOfPhaseColor
                    _byteSize += sizeof(float) * 3;      //outOfPhaseColor
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
