
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
    /// Struct containing parameters of a point AuraLight
    /// </summary>
    public struct PointLightParameters
    {
        #region Public Members
        public Vector3 color;
        public int useDefaultScattering;
        public float scatteringOverride;
        public Vector3 lightPosition;
        public float lightRange;
        public Vector2 distanceFalloffParameters;
        public MatrixFloats worldToShadowMatrix;
        #if UNITY_2017_3_OR_NEWER
        public Vector2 lightProjectionParameters;
        #endif
        public int shadowMapIndex;
        public float shadowStrength;
        public int cookieMapIndex;
        public Vector3 cookieParameters;
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
                    _byteSize += sizeof(float);          //lightRange
                    _byteSize += sizeof(float) * 2;      //distanceFalloffParameters
                    _byteSize += MatrixFloats.Size;      //worldToLightMatrix
                    #if UNITY_2017_3_OR_NEWER
                    _byteSize += sizeof(float) * 2;      //lightProjectionParameters
                    #endif
                    _byteSize += sizeof(int);            //shadowMapIndex
                    _byteSize += sizeof(float);          //shadowStrength
                    _byteSize += sizeof(int);            //cookieMapIndex
                    _byteSize += sizeof(float) * 3;      //cookieParameters
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
