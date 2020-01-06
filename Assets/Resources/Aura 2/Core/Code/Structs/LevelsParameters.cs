
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
    /// Collection of parameters for Levels adjustement. Similar to the same tool in Photoshop.
    /// </summary>
    [Serializable]
    public struct LevelsParameters
    {
        #region Public Members
        /// <summary>
        /// Offsets the bottom values (similar to Levels in Photoshop)
        /// </summary>
        public float levelLowThreshold;
        /// <summary>
        /// Offsets the top values (similar to Levels in Photoshop)
        /// </summary>
        public float levelHiThreshold;
        /// <summary>
        /// Output value of the bottom threshold (similar to Levels in Photoshop, except that it is unclamped here)
        /// </summary>
        public float outputLowValue;
        /// <summary>
        /// Output value of the top threshold (similar to Levels in Photoshop, except that it is unclamped here)
        /// </summary>
        public float outputHiValue;
        /// <summary>
        /// Contrast intensity
        /// </summary>
        public float contrast;
        /// <summary>
        /// Tells if output values should be clamped between 0 and 1;
        /// </summary>
        public bool saturateOutputValues;
        #endregion

        #region Private Members
        /// <summary>
        /// Packed data to be sent to the compute shader
        /// </summary>
        private LevelsData _packedData;
        #endregion

        #region Properties
        /// <summary>
        /// Packs the data and returns them
        /// </summary>
        public LevelsData Data
        {
            get
            {
                _packedData.levelLowThreshold = levelLowThreshold;
                _packedData.levelHiThreshold = levelHiThreshold;
                _packedData.outputLowValue = outputLowValue;
                _packedData.outputHiValue = outputHiValue;
                _packedData.contrast = contrast;

                return _packedData;
            }
        }

        /// <summary>
        /// Returns a LevelsParameters with default values
        /// </summary>
        public static LevelsParameters Default
        {
            get
            {
                LevelsParameters levels = new LevelsParameters(); // TODO : ONLT MAKE IT ONCE AT FIRST ACCESS
                levels.levelLowThreshold = 0;
                levels.levelHiThreshold = 1;
                levels.outputLowValue = 0;
                levels.outputHiValue = 1;
                levels.contrast = 1;

                return levels;
            }
        }

        /// <summary>
        /// Returns a LevelsParameters with all values to One
        /// </summary>
        public static LevelsParameters One
        {
            get
            {
                LevelsParameters levels = new LevelsParameters();
                levels.levelLowThreshold = 1;
                levels.levelHiThreshold = 1;
                levels.outputLowValue = 1;
                levels.outputHiValue = 1;
                levels.contrast = 1;

                return levels;
            }
        }

        /// <summary>
        /// Returns a LevelsParameters with all values to Zero
        /// </summary>
        public static LevelsParameters Zero
        {
            get
            {
                LevelsParameters levels = new LevelsParameters();
                levels.levelLowThreshold = 0;
                levels.levelHiThreshold = 0;
                levels.outputLowValue = 0;
                levels.outputHiValue = 0;
                levels.contrast = 0;

                return levels;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Set default values
        /// </summary>
        public void SetDefaultValues()
        {
            this = Default;
        }
        #endregion
    }
}
