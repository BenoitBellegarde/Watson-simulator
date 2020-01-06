
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
    /// Double buffering render texture
    /// </summary>
    public class SwappableRenderTexture
    {
        #region Private Members
        /// <summary>
        /// The two swapped render textures
        /// </summary>
        private RenderTexture[] _buffers;
        /// <summary>
        /// The ID of the read texture
        /// </summary>
        private int _readId = 0;
        /// <summary>
        /// The ID of the write texture
        /// </summary>
        private int _writeId = 1;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for a 3D swappable RenderTexture
        /// </summary>
        /// <param name="width">The width of the textures</param>
        /// <param name="height">The height of the textures</param>
        /// <param name="depth">The depth of the textures</param>
        /// <param name="format">The format of the textures</param>
        /// <param name="sRgbSampling">The color space of the textures</param>
        /// <param name="wrapMode">The wrap mode of the textures</param>
        /// <param name="filterMode">The filter mode of the textures</param>
        public SwappableRenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite sRgbSampling, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            _buffers = new RenderTexture[2];
            _buffers[0] = CreateRenderTexture(width, height, depth, format, sRgbSampling, wrapMode, filterMode);
            _buffers[1] = CreateRenderTexture(width, height, depth, format, sRgbSampling, wrapMode, filterMode);
        }

        /// <summary>
        /// Constructor for a 2D swappable RenderTexture
        /// </summary>
        /// <param name="width">The width of the textures</param>
        /// <param name="height">The height of the textures</param>
        /// <param name="format">The format of the textures</param>
        /// <param name="sRgbSampling">The color space of the textures</param>
        /// <param name="wrapMode">The wrap mode of the textures</param>
        /// <param name="filterMode">The filter mode of the textures</param>
        public SwappableRenderTexture(int width, int height, RenderTextureFormat format, RenderTextureReadWrite sRgbSampling, TextureWrapMode wrapMode, FilterMode filterMode) : this(width, height, -1, format, sRgbSampling, wrapMode, filterMode)
        {
        }
        #endregion

        #region Functions
        /// <summary>
        /// Accessor to the read texture
        /// </summary>
        public RenderTexture ReadBuffer
        {
            get
            {
                return _buffers[_readId];
            }
        }

        /// <summary>
        /// Accessor to the write texture
        /// </summary>
        public RenderTexture WriteBuffer
        {
            get
            {
                return _buffers[_writeId];
            }
        }

        /// <summary>
        /// Creates a formated RenderTexture
        /// </summary>
        /// <param name="width">The width of the texture</param>
        /// <param name="height">The height of the texture</param>
        /// <param name="depth">The depth of the texture</param>
        /// <param name="format">The format of the texture</param>
        /// <param name="sRgbSampling">The color space of the texture</param>
        /// <param name="wrapMode">The wrap mode of the texture</param>
        /// <param name="filterMode">The filter mode of the texture</param>
        private RenderTexture CreateRenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite sRgbSampling, TextureWrapMode wrapMode, FilterMode filterMode)
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 0, format, sRgbSampling);
            if(depth > 0)
            {
                renderTexture.dimension = TextureDimension.Tex3D;
                renderTexture.volumeDepth = depth;
            }
            renderTexture.wrapMode = wrapMode;
            renderTexture.filterMode = filterMode;
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();

            return renderTexture;
        }

        /// <summary>
        /// Swaps the textures
        /// </summary>
        public void Swap()
        {
            int tmp = _readId;
            _readId = _writeId;
            _writeId = tmp;
        }

        /// <summary>
        /// Releases the textures
        /// </summary>
        public void Release()
        {
            _buffers[0].Release();
            _buffers[0].Destroy();
            _buffers[1].Release();
            _buffers[1].Destroy();
            _buffers = null;
        }
        #endregion
    }
}
