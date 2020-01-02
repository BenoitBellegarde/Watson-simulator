
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Aura2API
{
    /// <summary>
    /// Handles the ambient lighting data
    /// </summary>
    public class AmbientLightingCommonDataManager
    {
        #region Private members
        /// <summary>
        /// Packed ambient probe's spherical harmonics coefficients
        /// </summary>
        private SphericalHarmonicsCoefficients _coefficients = new SphericalHarmonicsCoefficients();
        #endregion

        #region Properties
        /// <summary>
        /// Ambient probe's spherical harmonics coefficients packed to be sent to the compute shader
        /// </summary>
        public SphericalHarmonicsCoefficients Coefficients
        {
            get
            {
                return _coefficients;
            }
        }

        /// <summary>
        /// Returns the global strength of the ambient lighting
        /// </summary>
        public static float GlobalStrength
        {
            get
            {
                return RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Skybox ? Mathf.PI * RenderSettings.ambientIntensity : 1.0f;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Updates the spherical harmonics coefficients
        /// </summary>
        public void Update()
        {
            _coefficients = RenderSettings.ambientProbe.RepackForShaders();
        }
        #endregion
    }
}
