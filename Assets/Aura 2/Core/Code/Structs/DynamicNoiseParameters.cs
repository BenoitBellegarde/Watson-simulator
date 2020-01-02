
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

namespace Aura2API
{
    /// <summary>
    /// Collection of parameters for 4D dynamic noise mask to be used in a volume
    /// </summary>
    [Serializable]
    public struct DynamicNoiseParameters
    {
        /// <summary>
        /// Enables the dynamic noise computation
        /// </summary>
        public bool enable;
        /// <summary>
        /// The speed of the mutation
        /// </summary>
        public float speed;
        /// <summary>
        /// Allows to set base position, rotation and scale and animate them
        /// </summary>
        public TransformParameters transform;
    }
}
