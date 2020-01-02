
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
using UnityEngine.Serialization;

namespace Aura2API
{
    /// <summary>
    /// Base injection settings for the data computation
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Aura Base Settings", menuName = "Aura 2/Base Settings", order = 0)]
    public class AuraBaseSettings : ScriptableObject
    {
        #region Public Members
        /// <summary>
        /// Enables the injection of the base density
        /// </summary>
        public bool useDensity = true;
        /// <summary>
        /// The base density of the environment
        /// </summary>
        public float density = 0.25f;
        /// <summary>
        /// Enables the injection of the base scattering
        /// </summary>
        public bool useScattering = true;
        /// <summary>
        /// The base scattering of the environment
        /// </summary>
        [Range(0, 1)]
        public float scattering = 0.5f;
        /// <summary>
        /// Enables the ambient lighting
        /// </summary>
        public bool useAmbientLighting = true;
        /// <summary>
        /// The strength of the ambient lighting
        /// </summary>
        public float ambientLightingStrength = 1.0f;
        /// <summary>
        /// Enables the injection of the base color
        /// </summary>
        public bool useColor = false;
        /// <summary>
        /// The base color of the environment
        /// </summary>
        [ColorCircularPicker]
        public Color color = Color.cyan * 0.5f;
        /// <summary>
        /// The base color factor of the environment
        /// </summary>
        public float colorStrength = 1.0f;
        /// <summary>
        /// Enables the injection of the tint color
        /// </summary>
        public bool useTint = false;
        /// <summary>
        /// The tint color of the environment
        /// </summary>
        [ColorCircularPicker]
        public Color tint = Color.yellow;
        /// <summary>
        /// The tint color factor of the environment
        /// </summary>
        public float tintStrength = 1.0f;
        /// <summary>
        /// Enables the depth extinction of light
        /// </summary>
        public bool useExtinction = false;
        /// <summary>
        /// The linear light absorbtion of the environment (expressed in decay factor)
        /// </summary>
        public float extinction = 0.75f;
        #endregion
    }
}