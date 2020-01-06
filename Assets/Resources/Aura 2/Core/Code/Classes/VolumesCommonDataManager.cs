
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
using System.Collections.Generic;
using UnityEngine;

namespace Aura2API
{
    /// <summary>
    /// Handles the volumes' registration and maps collection
    /// </summary>
    public class VolumesCommonDataManager
    {
        #region Public members
        /// <summary>
        /// The size of the Texture3D masks
        /// </summary>
        public static readonly int texture3DMaskSize = 16;
        /// <summary>
        /// The size of the Texture2D masks
        /// </summary>
        public static readonly int texture2DMaskSize = 512;
        #endregion

        #region Private members
        /// <summary>
        /// List of registered Aura Volumes
        /// </summary>
        private List<AuraVolume> _registeredVolumesList;
        /// <summary>
        /// List of registered Aura Volumes used as Light Probes Proxy Volume
        /// </summary>
        private List<AuraVolume> _registeredLightProbesProxyVolumesList;
        /// <summary>
        /// The composer that will collect the Texture3D masks and stack them in a Texture3D
        /// </summary>
        private Texture3DAtlasComposer _volumesTexture3DMasksAtlas;
        /// <summary>
        /// The composer that will collect the Texture2D masks and stack them in a Texture2DArray
        /// </summary>
        private Texture2DArrayComposer _volumesTexture2DMasksArray;
        #endregion

        #region Properties
        /// <summary>
        /// Accessor to the list of registered Aura Volumes
        /// </summary>
        public List<AuraVolume> RegisteredVolumesList
        {
            get
            {
                if (_registeredVolumesList == null)
                {
                    _registeredVolumesList = new List<AuraVolume>();
                }

                return _registeredVolumesList;
            }
        }
        /// <summary>
        /// Accessor to the list of registered Aura Volumes used as Light Probes Proxy Volume
        /// </summary>
        public List<AuraVolume> RegisteredLightProbesProxyVolumesList
        {
            get
            {
                if (_registeredLightProbesProxyVolumesList == null)
                {
                    _registeredLightProbesProxyVolumesList = new List<AuraVolume>();
                }

                return _registeredLightProbesProxyVolumesList;
            }
        }

        /// <summary>
        /// Tells if has registered volumes
        /// </summary>
        public bool HasRegisteredVolumes
        {
            get
            {
                return RegisteredVolumesList.Count > 0;
            }
        }

        /// <summary>
        /// Tells if has registered Aura Volumes used as Light Probes Proxy Volume
        /// </summary>
        public bool HasRegisteredLightProbesProxyVolumes
        {
            get
            {
                return RegisteredLightProbesProxyVolumesList.Count > 0;
            }
        }

        /// <summary>
        /// The composer that will collect the Texture2D masks and stack them in a Texture2DArray
        /// </summary>
        private Texture2DArrayComposer Texture2DMasksAtlasComposer
        {
            get
            {
                if (_volumesTexture2DMasksArray == null)
                {
                    _volumesTexture2DMasksArray = new Texture2DArrayComposer(texture2DMaskSize, texture2DMaskSize, TextureFormat.RGBA32, true);
                }

                return _volumesTexture2DMasksArray;
            }
        }

        /// <summary>
        /// Tells if there are any Texture2D mask
        /// </summary>
        public bool HasTexture2DMasks
        {
            get
            {
                return Texture2DMasksAtlasComposer.HasTexture;
            }
        }

        /// <summary>
        /// Accessor to the Texture2DArray containing the Texture2D masks
        /// </summary>
        public Texture2DArray Texture2DMasksAtlas
        {
            get
            {
                return Texture2DMasksAtlasComposer.Texture;
            }
        }

        /// <summary>
        /// The composer that will collect the 3D texture masks and stack them in a Texture3D
        /// </summary>
        private Texture3DAtlasComposer Texture3DMasksAtlasComposer
        {
            get
            {
                if(_volumesTexture3DMasksAtlas == null)
                {
                    _volumesTexture3DMasksAtlas = new Texture3DAtlasComposer(TextureFormat.RGBA32, texture3DMaskSize);
                }

                return _volumesTexture3DMasksAtlas;
            }
        }

        /// <summary>
        /// Tells if there are any Texture3D mask
        /// </summary>
        public bool HasTexture3DMasks
        {
            get
            {
                return Texture3DMasksAtlasComposer.HasVolumeTexture;
            }
        }

        /// <summary>
        /// Accessor to the Texture3D containing the Texture3D masks
        /// </summary>
        public Texture3D Texture3DMasksAtlas
        {
            get
            {
                return Texture3DMasksAtlasComposer.VolumeTexture;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when a volume is being registred
        /// </summary>
        public event Action<AuraVolume> OnRegisterVolume;
        /// <summary>
        /// Event raised when a volume is being unregistred
        /// </summary>
        public event Action<AuraVolume> OnUnregisterVolume;
        #endregion

        #region Functions
        /// <summary>
        /// Registers the Aura Volume to the global manager
        /// </summary>
        /// <param name="auravolume">The Aura Volume to register</param>
        public void RegisterVolume(AuraVolume auraVolume)
        {
            if (!RegisteredVolumesList.Contains(auraVolume))
            {
                RegisteredVolumesList.Add(auraVolume);

                if(auraVolume.useAsLightProbesProxyVolume)
                {
                    RegisteredLightProbesProxyVolumesList.Add(auraVolume);
                }

                if (auraVolume.UsesTexture2DMasking)
                {
                    Texture2DMasksAtlasComposer.AddTexture(auraVolume.texture2DMask.texture);
                    Texture2DMasksAtlasComposer.Generate();
                    SetTexture2DMasksId();
                }

                if (auraVolume.UsesTexture3DMasking)
                {
                    Texture3DMasksAtlasComposer.AddTexture(auraVolume.texture3DMask.texture);
                    Texture3DMasksAtlasComposer.Generate();
                    SetTexture3DMasksId();
                }

                if (OnRegisterVolume != null)
                {
                    OnRegisterVolume(auraVolume);
                }
            }

            auraVolume.OnUninitialize += AuraVolume_OnUninitialize;
        }

        /// <summary>
        /// Function called when the volume is being disabled
        /// </summary>
        private void AuraVolume_OnUninitialize(AuraVolume auraVolume)
        {
            if (RegisteredVolumesList.Contains(auraVolume))
            {
                if (RegisteredLightProbesProxyVolumesList.Contains(auraVolume))
                {
                    RegisteredLightProbesProxyVolumesList.Remove(auraVolume);
                }

                if (OnUnregisterVolume != null)
                {
                    OnUnregisterVolume(auraVolume);
                }

                if (Texture2DMasksAtlasComposer.RemoveTexture(auraVolume.texture2DMask.texture))
                {
                    SetTexture2DMasksId();
                    Texture2DMasksAtlasComposer.Generate();
                }

                if (Texture3DMasksAtlasComposer.RemoveTexture(auraVolume.texture3DMask.texture))
                {
                    SetTexture3DMasksId();
                    Texture3DMasksAtlasComposer.Generate();
                }

                RegisteredVolumesList.Remove(auraVolume);
            }

            auraVolume.OnUninitialize -= AuraVolume_OnUninitialize;
        }

        /// <summary>
        /// Sets the id of the AuraVolumes's Texture2D mask in the atlas
        /// </summary>
        private void SetTexture2DMaskId(AuraVolume auraVolume)
        {
            if (auraVolume.UsesTexture2DMasking)
            {
                auraVolume.texture2DMask.textureIndex = Texture2DMasksAtlasComposer.GetTextureIndex(auraVolume.texture2DMask.texture);
            }
        }

        /// <summary>
        /// Sets the id of each registered AuraVolumes's Texture2D mask in the atlas
        /// </summary>
        private void SetTexture2DMasksId()
        {
            for (int i = 0; i < RegisteredVolumesList.Count; ++i)
            {
                SetTexture2DMaskId(RegisteredVolumesList[i]);
            }
        }

        /// <summary>
        /// Sets the id of the AuraVolumes's Texture3D mask in the atlas
        /// </summary>
        private void SetTexture3DMaskId(AuraVolume auraVolume)
        {
            if (auraVolume.UsesTexture3DMasking)
            {
                auraVolume.texture3DMask.textureIndex = Texture3DMasksAtlasComposer.GetTextureIndex(auraVolume.texture3DMask.texture);
            }
        }

        /// <summary>
        /// Sets the id of each registered AuraVolumes's Texture3D mask in the atlas
        /// </summary>
        private void SetTexture3DMasksId()
        {
            for (int i = 0; i < RegisteredVolumesList.Count; ++i)
            {
                SetTexture3DMaskId(RegisteredVolumesList[i]);
            }
        }
        #endregion
    }
}
