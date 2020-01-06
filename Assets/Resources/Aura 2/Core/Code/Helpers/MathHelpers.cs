
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
    /// Collection of function/variables related to maths
    /// </summary>
    public static class MathHelpers
    {
        #region Functions
        /// <summary>
        /// Projects a point orthogonally on a line (MathHelpers.ProjectPointOnLine() doesn't work when outside of the boundary points)
        /// </summary>
        /// <param name="linePoint">Any point on the line</param>
        /// <param name="direction">The direction of the line</param>
        /// <param name="pointToProject">The point to project</param>
        /// <returns>The position of the projected point on the line</returns>
        public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 direction, Vector3 pointToProject)
        {
            return linePoint + Vector3.Dot(pointToProject - linePoint, direction) * direction;
        }
        #endregion
    }
}
