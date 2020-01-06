
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

using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Collection of useful functions
    /// </summary>
    public static class AuraUtility
    {
        #region Functions
#if UNITY_EDITOR
        /// <summary>
        /// Gets the define symbols as a full string
        /// </summary>
        /// <param name="target">The platform target</param>
        /// <returns>The define symbols as a full string</returns>
        public static string GetTargetDefineSymbols(BuildTargetGroup target)
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        }
        /// <summary>
        /// Gets the define symbols as a full string for the current platform target
        /// </summary>
        /// <returns>The define symbols as a full string</returns>
        public static string GetTargetDefineSymbols()
        {
            return GetTargetDefineSymbols(EditorUserBuildSettings.selectedBuildTargetGroup);
        }

        /// <summary>
        /// Gets the define symbols strings in an array
        /// </summary>
        /// <param name="target">The platform target</param>
        /// <returns>The define symbols strings in an array</returns>
        public static string[] GetTargetDefineSymbolsSplitted(BuildTargetGroup target)
        {
            return GetTargetDefineSymbols(target).Split(';');
        }
        /// <summary>
        /// Gets the define symbols strings in an array for the current platform target
        /// </summary>
        /// <returns>The define symbols strings in an array</returns>
        public static string[] GetTargetDefineSymbolsSplitted()
        {
            return GetTargetDefineSymbolsSplitted(EditorUserBuildSettings.selectedBuildTargetGroup);
        }

        /// <summary>
        /// Gets if the specified symbol string is present in the defined symbols
        /// </summary>
        /// <param name="symbol">The queried symbol string</param>
        /// <param name="target">The platform target</param>
        /// <returns>True if found, false otherwise</returns>
        public static bool GetIfSymbolIsDefined(string symbol, BuildTargetGroup target)
        {
            string[] currentSymbols = GetTargetDefineSymbolsSplitted(target);
            for (int i = 0; i < currentSymbols.Length; ++i)
            {
                if (currentSymbols[i] == symbol)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Gets if the specified symbol string is present in the defined symbols for the current platform target
        /// </summary>
        /// <param name="symbol">The queried symbol string</param>
        /// <returns>True if found, false otherwise</returns>
        public static bool GetIfSymbolIsDefined(string symbol)
        {
            return GetIfSymbolIsDefined(symbol, EditorUserBuildSettings.selectedBuildTargetGroup);
        }

        /// <summary>
        /// Adds a new define symbol
        /// </summary>
        /// <param name="symbol">The new symbol string to add</param>
        /// <param name="target">The platform target</param>
        public static void AddDefineSymbol(string symbol, BuildTargetGroup target)
        {
            if (!GetIfSymbolIsDefined(symbol, target))
            {
                string currentSymbols = GetTargetDefineSymbols(target);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, currentSymbols + ";" + symbol);
            }
        }
        /// <summary>
        /// Adds a new define symbol to the current platform target
        /// </summary>
        /// <param name="symbol">The new symbol string to add</param>
        public static void AddDefineSymbol(string symbol)
        {
            AddDefineSymbol(symbol, EditorUserBuildSettings.selectedBuildTargetGroup);
        }

        /// <summary>
        /// Removes a define symbol
        /// </summary>
        /// <param name="symbol">The new symbol string to remove</param>
        /// <param name="target">The platform target</param>
        public static void RemoveDefineSymbol(string symbol, BuildTargetGroup target)
        {
            if (GetIfSymbolIsDefined(symbol, target))
            {
                string[] currentSymbols = GetTargetDefineSymbolsSplitted(target);
                string newSymbols = "";
                for (int i = 0; i < currentSymbols.Length; ++i)
                {
                    if (currentSymbols[i] != symbol)
                    {
                        newSymbols += currentSymbols[i] + ";";
                    }
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, newSymbols);
            }
        }
        /// <summary>
        /// Removes a define symbol from the current platform target
        /// </summary>
        /// <param name="symbol">The new symbol string to remove</param>
        public static void RemoveDefineSymbol(string symbol)
        {
            RemoveDefineSymbol(symbol, EditorUserBuildSettings.selectedBuildTargetGroup);
        }
#endif
        #endregion
    }
}