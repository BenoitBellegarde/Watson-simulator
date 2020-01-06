
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
    /// Collection of extension functions for Object objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Destroys an Object derived object
        /// </summary>
        /// <param name="objectToDelete">The object to delete</param>
        public static void Destroy(this Object objectToDelete)
        {
            if (objectToDelete != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Object.Destroy(objectToDelete);
                }
                else
                {
                    Object.DestroyImmediate(objectToDelete);
                }
#else
                Object.Destroy(objectToDelete);
#endif
            }
        }
    }
}
