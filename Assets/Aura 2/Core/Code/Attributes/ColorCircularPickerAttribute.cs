
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
    /// Custom attribute for enabling circular picker on Color properties
    /// </summary>
    public class ColorCircularPickerAttribute : PropertyAttribute
    {
        #region Members
        /// <summary>
        /// Enables the display of the name of the property
        /// </summary>
        public readonly bool showLabel;
        #endregion

        #region Constructor
        /// <summary>
        /// Enables circular picker on Color properties
        /// </summary>
        /// <param name="showLabel">Enables the display of the name of the property</param>
        public ColorCircularPickerAttribute(bool showLabel = false)
        {
            this.showLabel = showLabel;
        }
        #endregion
    }
}
