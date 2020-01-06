
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
    /// Ordered struct of Levels operation parameters to be sent to the compute shader
    /// </summary>
    public struct LevelsData
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
                    _byteSize += sizeof(float); // levelLowThreshold
                    _byteSize += sizeof(float); // levelHiThreshold
                    _byteSize += sizeof(float); // outputLowValue
                    _byteSize += sizeof(float); // outputHiValue
                    _byteSize += sizeof(float); // contrast
                }

                return _byteSize;
            }
        }
        #endregion
    }
}
