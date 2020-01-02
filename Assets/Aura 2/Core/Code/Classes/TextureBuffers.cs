
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
using UnityEngine.Rendering;

namespace Aura2API
{
    /// <summary>
    /// Collection of texture buffers which contain the computed volumetric data
    /// </summary>
    public class TextureBuffers
    {
        #region Private Members
        /// <summary>
        /// The resolution of the volumetric buffers
        /// </summary>
        private Vector3Int _volumetricBuffersResolution;
        /// <summary>
        /// The Texture3D buffer containing the lighting/density data
        /// </summary>
        private SwappableRenderTexture _dataVolumeTexture;
        /// <summary>
        /// The Texture3D buffer containing the accumulated volumetric fog
        /// </summary>
        private RenderTexture _fogVolumeTexture;
        /// <summary>
        /// The Texture2D buffer containing the minimum depth per cell
        /// </summary>
        private SwappableRenderTexture _occlusionTexture;
        /// <summary>
        /// The Texture2D buffer containing the maximum amount of slices per cell
        /// </summary>
        private SwappableRenderTexture _sliceTexture;
        /// <summary>
        /// The resolution of the light probes lighting buffer
        /// </summary>
        private static readonly Vector3Int _lightProbesCoefficientsTextureResolution = new Vector3Int(32, 18, 16); // Temporary STATIC READONLY
        /// <summary>
        /// The Texture3D buffer containing the light probes lighting data
        /// </summary>
        private RenderTexture _lightProbesCoefficientsTexture;
        /// <summary>
        /// The light probes coefficients
        /// </summary>
        private SphericalHarmonicsFirstBandCoefficients[] _lightProbesCoefficients;
        /// <summary>
        /// The compute buffer containing the light probes coefficients
        /// </summary>
        private ComputeBuffer _lightProbesCoefficientsBuffer;
        #endregion

        #region Constructor
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public Buffers()
        //{
        //}
        #endregion

        #region Properties
        /// <summary>
        /// The resolution of the volumetric buffers
        /// </summary>
        public Vector3Int VolumetricBuffersResolution
        {
            get
            {
                return _volumetricBuffersResolution;
            }
            set
            {
                ReleaseVolumeTextureBuffers();
                ReleaseOcclusionTextureBuffer();
                ReleaseSliceTextureBuffer();

                _volumetricBuffersResolution = value;
            }
        }

        /// <summary>
        /// Tells if the resolution of the volumetric buffers is set
        /// </summary>
        public bool VolumetricBuffersResolutionIsValid
        {
            get
            {
                return VolumetricBuffersResolution != Vector3Int.zero;
            }
        }

        /// <summary>
        /// The resolution of the light probes buffers
        /// </summary>
        public Vector3Int LightProbesCoefficientsTextureResolution
        {
            get
            {
                return _lightProbesCoefficientsTextureResolution;
            }
            //set
            //{
            //    ReleaseLightProbesCoefficientsTextureBuffer();
            //
            //    __lightProbesCoefficientsTextureResolution = value;
            //}
        }

        /// <summary>
        /// Tells if the resolution of the light probes buffers is set
        /// </summary>
        public bool LightProbesCoefficientsTextureResolutionIsValid
        {
            get
            {
                return LightProbesCoefficientsTextureResolution != Vector3Int.zero;
            }
        }

        /// <summary>
        /// Accessor to the volumetric data buffer (containing the lighting (RGB) and the density (A))
        /// </summary>
        public SwappableRenderTexture DataVolumeTexture
        {
            get
            {
                if(!VolumetricBuffersResolutionIsValid)
                {
                    Debug.LogError("Error while creating DataVolumeTextures buffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_dataVolumeTexture == null)
                {
                    _dataVolumeTexture = new SwappableRenderTexture(VolumetricBuffersResolution.x, VolumetricBuffersResolution.y, VolumetricBuffersResolution.z, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, TextureWrapMode.Clamp, FilterMode.Bilinear);
                }

                return _dataVolumeTexture;
            }
        }

        /// <summary>
        /// Accessor to the volumetric accumulated buffer (containing the accumulated lighting)
        /// </summary>
        public RenderTexture FogVolumeTexture
        {
            get
            {
                if (!VolumetricBuffersResolutionIsValid)
                {
                    Debug.LogError("Error while creating FogVolumeTexture buffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_fogVolumeTexture == null)
                {
                    _fogVolumeTexture = new RenderTexture(VolumetricBuffersResolution.x, VolumetricBuffersResolution.y, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
                    _fogVolumeTexture.dimension = TextureDimension.Tex3D;
                    _fogVolumeTexture.volumeDepth = VolumetricBuffersResolution.z;
                    _fogVolumeTexture.wrapMode = TextureWrapMode.Clamp;
                    _fogVolumeTexture.filterMode = FilterMode.Bilinear;
                    _fogVolumeTexture.enableRandomWrite = true;
                    _fogVolumeTexture.Create();
                }

                return _fogVolumeTexture;
            }
        }

        /// <summary>
        /// Accessor to the buffer containing the maximum depth
        /// </summary>
        public SwappableRenderTexture OcclusionTexture
        {
            get
            {
                if (!VolumetricBuffersResolutionIsValid)
                {
                    Debug.LogError("Error while creating OcclusionTexture buffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_occlusionTexture == null)
                {
                    _occlusionTexture = new SwappableRenderTexture(VolumetricBuffersResolution.x, VolumetricBuffersResolution.y, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear, TextureWrapMode.Clamp, FilterMode.Point);
                }

                return _occlusionTexture;
            }
        }

        /// <summary>
        /// The Texture2D buffer containing the maximum amount of slices per cell
        /// </summary>
        public SwappableRenderTexture SliceTexture
        {
            get
            {
                if (!VolumetricBuffersResolutionIsValid)
                {
                    Debug.LogError("Error while creating OcclusionTexture buffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_sliceTexture == null)
                {
                    _sliceTexture = new SwappableRenderTexture(VolumetricBuffersResolution.x, VolumetricBuffersResolution.y, RenderTextureFormat.RInt, RenderTextureReadWrite.Linear, TextureWrapMode.Clamp, FilterMode.Point); // Lack of R32UINT
                }

                return _sliceTexture;
            }
        }

        /// <summary>
        /// Accessor to the volumetric light probes coefficients texture
        /// </summary>
        public RenderTexture LightProbesCoefficientsTexture
        {
            get
            {
                if (!LightProbesCoefficientsTextureResolutionIsValid)
                {
                    Debug.LogError("Error while creating LightProbesCoefficientsTexture buffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_lightProbesCoefficientsTexture == null)
                {
                    _lightProbesCoefficientsTexture = new RenderTexture(LightProbesCoefficientsTextureResolution.x * 3, LightProbesCoefficientsTextureResolution.y, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
                    _lightProbesCoefficientsTexture.dimension = TextureDimension.Tex3D;
                    _lightProbesCoefficientsTexture.volumeDepth = LightProbesCoefficientsTextureResolution.z;
                    _lightProbesCoefficientsTexture.wrapMode = TextureWrapMode.Clamp;
                    _lightProbesCoefficientsTexture.filterMode = FilterMode.Bilinear;
                    _lightProbesCoefficientsTexture.enableRandomWrite = true;
                    _lightProbesCoefficientsTexture.Create();
                }

                return _lightProbesCoefficientsTexture;
            }
        }

        /// <summary>
        /// Accessor to the light probes coefficients array
        /// </summary>
        public SphericalHarmonicsFirstBandCoefficients[] LightProbesCoefficients
        {
            get
            {
                if (!LightProbesCoefficientsTextureResolutionIsValid)
                {
                    Debug.LogError("Error while creating LightProbesCoefficients array buffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_lightProbesCoefficients == null)
                {
                    _lightProbesCoefficients = new SphericalHarmonicsFirstBandCoefficients[LightProbesCoefficientsTextureResolution.x * LightProbesCoefficientsTextureResolution.y * LightProbesCoefficientsTextureResolution.z];
                }

                return _lightProbesCoefficients;
            }
        }

        /// <summary>
        /// Accessor to the light probes coefficients compute buffer
        /// </summary>
        public ComputeBuffer LightProbesCoefficientsComputeBuffer
        {
            get
            {
                if (!LightProbesCoefficientsTextureResolutionIsValid)
                {
                    Debug.LogError("Error while creating LightProbesCoefficientsBuffer computeBuffer in \"" + this + "\". The resolution parameter seems not set.");
                    return null;
                }

                if (_lightProbesCoefficientsBuffer == null)
                {
                    _lightProbesCoefficientsBuffer = new ComputeBuffer(LightProbesCoefficients.Length, SphericalHarmonicsFirstBandCoefficients.Size);
                }

                return _lightProbesCoefficientsBuffer;
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Releases all texture buffers
        /// </summary>
        public void ReleaseAllBuffers()
        {
            ReleaseVolumeTextureBuffers();
            ReleaseOcclusionTextureBuffer();
            ReleaseSliceTextureBuffer();
            ReleaseAllLightProbesBuffers();
        }

        /// <summary>
        /// Releases the volume texture buffers
        /// </summary>
        public void ReleaseVolumeTextureBuffers()
        {
            if (_dataVolumeTexture != null)
            {
                _dataVolumeTexture.Release();
                _dataVolumeTexture = null;
            }

            if (_fogVolumeTexture != null)
            {
                _fogVolumeTexture.Release();
                _fogVolumeTexture.Destroy();
            }
        }

        /// <summary>
        /// Releases the occlusion texture buffer
        /// </summary>
        public void ReleaseOcclusionTextureBuffer()
        {
            if (_occlusionTexture != null)
            {
                _occlusionTexture.Release();
                _occlusionTexture = null;
            }
        }

        /// <summary>
        /// Releases the occlusion texture buffer
        /// </summary>
        public void ReleaseSliceTextureBuffer()
        {
            if (_sliceTexture != null)
            {
                _sliceTexture.Release();
                _sliceTexture = null;
            }
        }

        /// <summary>
        /// Releases all light probes coefficients buffers
        /// </summary>
        public void ReleaseAllLightProbesBuffers()
        {
            ReleaseLightProbesCoefficientsTextureBuffer();
            ReleaseLightProbesCoefficientsBuffer();
        }

        /// <summary>
        /// Releases the light probes coefficients texture buffer
        /// </summary>
        public void ReleaseLightProbesCoefficientsTextureBuffer()
        {
            if (_lightProbesCoefficientsTexture != null)
            {
                _lightProbesCoefficientsTexture.Release();
                _lightProbesCoefficientsTexture.Destroy();
            }
        }

        /// <summary>
        /// Releases the light probes coefficients texture buffer
        /// </summary>
        public void ReleaseLightProbesCoefficientsBuffer()
        {
            if (_lightProbesCoefficientsBuffer != null)
            {
                _lightProbesCoefficientsBuffer.Release();
                _lightProbesCoefficientsBuffer = null;
            }
        }
        #endregion
    }
}
