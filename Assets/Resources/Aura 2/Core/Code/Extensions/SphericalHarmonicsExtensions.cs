
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
using UnityEngine.Rendering;

namespace Aura2API
{
    /// <summary>
    /// Static class containing extension for spherical harmonics
    /// </summary>
    public static class SphericalHarmonicsExtensions
    {
        /// <summary>
        /// Repacks the first band of the spherical harmonics coefficients used by probes to be used in shaders
        /// </summary>
        /// <param name="rawCoefficients">The input spherical harmonic coefficients</param>
        /// <returns>The repacked first band coefficients, ready to be used in shaders</returns>
        public static SphericalHarmonicsFirstBandCoefficients RepackFirstBandForShaders(this SphericalHarmonicsL2 rawCoefficients)
        {
            // https://docs.unity3d.com/Manual/LightProbes-TechnicalInformation.html -> http://www.ppsloan.org/publications/StupidSH36.pdf -> Appendix A10 : "Shader/CPU code for Irradiance Environment Maps"

            SphericalHarmonicsFirstBandCoefficients repackedCoefficients = new SphericalHarmonicsFirstBandCoefficients();
            repackedCoefficients.shAr.x = rawCoefficients[0, 3];
            repackedCoefficients.shAr.y = rawCoefficients[0, 1];
            repackedCoefficients.shAr.z = rawCoefficients[0, 2];
            repackedCoefficients.shAr.w = rawCoefficients[0, 0] - rawCoefficients[0, 6];
            repackedCoefficients.shAg.x = rawCoefficients[1, 3];
            repackedCoefficients.shAg.y = rawCoefficients[1, 1];
            repackedCoefficients.shAg.z = rawCoefficients[1, 2];
            repackedCoefficients.shAg.w = rawCoefficients[1, 0] - rawCoefficients[1, 6];
            repackedCoefficients.shAb.x = rawCoefficients[2, 3];
            repackedCoefficients.shAb.y = rawCoefficients[2, 1];
            repackedCoefficients.shAb.z = rawCoefficients[2, 2];
            repackedCoefficients.shAb.w = rawCoefficients[2, 0] - rawCoefficients[2, 6];

            return repackedCoefficients;
        }
        /// <summary>
        /// Repacks all of the spherical harmonics coefficients used by probes to be used in shaders
        /// </summary>
        /// <param name="rawCoefficients">The input spherical harmonic coefficients</param>
        /// <returns>The repacked coefficients, ready to be used in shaders</returns>
        public static SphericalHarmonicsCoefficients RepackForShaders(this SphericalHarmonicsL2 rawCoefficients)
        {
            // https://docs.unity3d.com/Manual/LightProbes-TechnicalInformation.html -> http://www.ppsloan.org/publications/StupidSH36.pdf -> Appendix A10 : "Shader/CPU code for Irradiance Environment Maps"

            SphericalHarmonicsCoefficients repackedCoefficients = new SphericalHarmonicsCoefficients();
            repackedCoefficients.firstBandCoefficients = rawCoefficients.RepackFirstBandForShaders();

            repackedCoefficients.shBr.x = rawCoefficients[0, 4];
            repackedCoefficients.shBr.y = rawCoefficients[0, 5];
            repackedCoefficients.shBr.z = rawCoefficients[0, 6] * 3.0f;
            repackedCoefficients.shBr.w = rawCoefficients[0, 7];
            repackedCoefficients.shBg.x = rawCoefficients[1, 4];
            repackedCoefficients.shBg.y = rawCoefficients[1, 5];
            repackedCoefficients.shBg.z = rawCoefficients[1, 6] * 3.0f;
            repackedCoefficients.shBg.w = rawCoefficients[1, 7];
            repackedCoefficients.shBb.x = rawCoefficients[2, 4];
            repackedCoefficients.shBb.y = rawCoefficients[2, 5];
            repackedCoefficients.shBb.z = rawCoefficients[2, 6] * 3.0f;
            repackedCoefficients.shBb.w = rawCoefficients[2, 7];

            repackedCoefficients.shC.x = rawCoefficients[0, 8];
            repackedCoefficients.shC.y = rawCoefficients[1, 8];
            repackedCoefficients.shC.z = rawCoefficients[2, 8];
            repackedCoefficients.shC.w = 1.0f;

            return repackedCoefficients;
        }
    }

}
