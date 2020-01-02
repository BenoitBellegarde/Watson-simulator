
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

using UnityEditor;

namespace Aura2API
{
    [InitializeOnLoad]
    public class DefineAuraSymbols
    {
        #region Public Members
        /// <summary>
        /// The define symbol string to tell Aura is present in the project
        /// </summary>
        public const string symbolString = "AURA_IN_PROJECT";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        static DefineAuraSymbols()
        {
            EditorApplication.update += OnEditorApplicationUpdate;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Called on every editor update
        /// </summary>
        private static void OnEditorApplicationUpdate()
        {
            #if !AURA_IN_PROJECT
            AuraUtility.AddDefineSymbol(symbolString);
            #endif
        }
        #endregion
    }
}