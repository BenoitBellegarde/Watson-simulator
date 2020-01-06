
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
    /// Collection of extension functions for FrustumParametersEnum objects
    /// </summary>
    public static class FrustumParametersEnumExtensions
    {
        /// <summary>
        /// Tells if specified flags are checked
        /// </summary>
        /// <param name="comparisonFlags">The flags to check</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static bool HasFlags(this FrustumParameters referenceFlags, FrustumParameters comparisonFlags)
        {
            return (referenceFlags & comparisonFlags) == comparisonFlags;
        }

        /// <summary>
        /// Sets specified flags on
        /// </summary>
        /// <param name="addedFlags">The flags to set on</param>
        /// <returns></returns>
        public static FrustumParameters SetFlags(this FrustumParameters referenceFlags, FrustumParameters addedFlags)
        {
            return referenceFlags | addedFlags;
        }

        /// <summary>
        /// Sets specified flags off
        /// </summary>
        /// <param name="removedFlags">The flags to set off</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static FrustumParameters RemoveFlags(this FrustumParameters referenceFlags, FrustumParameters removedFlags)
        {
            return referenceFlags & ~removedFlags;
        }

        /// <summary>
        /// Toggles the specified flags
        /// </summary>
        /// <param name="togglingFlags">the flags to toggle</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static FrustumParameters ToggleFlags(this FrustumParameters referenceFlags, FrustumParameters togglingFlags)
        {
            return referenceFlags ^ togglingFlags;
        }

        /// <summary>
        /// Forces the specified flags to on/off
        /// </summary>
        /// <param name="replacingFlags">The flags to replace</param>
        /// <param name="value">The forced value</param>
        /// <returns>The modified FrustumParametersEnum</returns>
        public static FrustumParameters ReplaceFlags(this FrustumParameters referenceFlags, FrustumParameters replacingFlags, bool value)
        {
            FrustumParameters newFlags = referenceFlags;

            if(value && !referenceFlags.HasFlags(replacingFlags) || !value && referenceFlags.HasFlags(replacingFlags))
            {
                newFlags = referenceFlags.ToggleFlags(replacingFlags);
            }

            return newFlags;
        }
    }
}
