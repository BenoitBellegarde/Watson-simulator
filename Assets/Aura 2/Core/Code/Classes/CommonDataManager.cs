
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
    /// Handles the lights' and volumes' commond data managers
    /// </summary>
    public class CommonDataManager
    {
        #region Private members
        /// <summary>
        /// The manager that handles the lights' registration and maps collection
        /// </summary>
        private LightsCommonDataManager _lightsCommonDataManager;
        /// <summary>
        /// The manager that handles the volumes' registration and masks collection
        /// </summary>
        private VolumesCommonDataManager _volumesCommonDataManager;
        /// <summary>
        /// The manager that handles the ambient lighting data
        /// </summary>
        private AmbientLightingCommonDataManager _ambientLightingCommonDataManager;
        #endregion

        #region Properties
        /// <summary>
        /// The manager that handles the lights' registration, maps collection and culling
        /// </summary>
        public LightsCommonDataManager LightsCommonDataManager
        {
            get
            {
                if (_lightsCommonDataManager == null)
                {
                    _lightsCommonDataManager = new LightsCommonDataManager();
                }

                return _lightsCommonDataManager;
            }
        }
        /// <summary>
        /// The manager that handles the lights' registration, maps collection and culling
        /// </summary>
        public VolumesCommonDataManager VolumesCommonDataManager
        {
            get
            {
                if (_volumesCommonDataManager == null)
                {
                    _volumesCommonDataManager = new VolumesCommonDataManager();
                }

                return _volumesCommonDataManager;
            }
        }
        /// <summary>
        /// The manager that handles the ambient lighting data
        /// </summary>
        public AmbientLightingCommonDataManager AmbientLightingCommonDataManager
        {
            get
            {
                if (_ambientLightingCommonDataManager == null)
                {
                    _ambientLightingCommonDataManager = new AmbientLightingCommonDataManager();
                }

                return _ambientLightingCommonDataManager;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Releases all the managed members
        /// </summary>
        public void Dispose()
        {
            LightsCommonDataManager.Dispose();
        }

        /// <summary>
        /// Updates all the data
        /// </summary>
        public void UpdateData()
        {
            if (_lightsCommonDataManager != null)
            {
                LightsCommonDataManager.Update();
            }

            if (_ambientLightingCommonDataManager != null)
            {
                AmbientLightingCommonDataManager.Update();
            }
        }
        #endregion
    }
}
