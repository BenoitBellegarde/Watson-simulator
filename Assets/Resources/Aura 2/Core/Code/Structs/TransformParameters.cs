
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

using System;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Used for transforming reference position for textures/noise inside volumes
    /// </summary>
    [Serializable]
    public struct TransformParameters
    {
        #region Public Members
        /// <summary>
        /// Referential to use for transformations and animations
        /// </summary>
        public Space space;
        /// <summary>
        /// Position of the volume in the selected referencial
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// Rotation of the volume in the selected referencial
        /// </summary>
        public Vector3 rotation;
        /// <summary>
        /// Scale of the volume in the selected referencial
        /// </summary>
        public Vector3 scale;
        #endregion

        #region Functions
        /// <summary>
        /// The resulting transformation matrix
        /// </summary>
        public Matrix4x4 Matrix
        {
            get
            {
                return Matrix4x4.TRS(position, Quaternion.Euler(rotation), scale);
            }
        }
        #endregion
    }
}
