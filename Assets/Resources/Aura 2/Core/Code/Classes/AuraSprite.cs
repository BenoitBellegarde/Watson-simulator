
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
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aura2API
{
    /// <summary>
    /// Component allowing the sprites to work with the Aura system
    /// </summary>
    [AddComponentMenu("Aura 2/Aura Sprite", 3)]
    [ExecuteInEditMode]
    [Serializable]
    [RequireComponent(typeof(SpriteRenderer))]
    public class AuraSprite : MonoBehaviour
    {
        #region Public members
        public ShadowCastingMode shadowCastingMode = ShadowCastingMode.TwoSided;
        public bool receiveShadows = true;
        #endregion

        #region Private members
        private SpriteRenderer _spriteRenderer;
        #endregion

        #region Properties
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if(_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                    SetLitShader();
                }

                return _spriteRenderer;
            }
        }

        public Sprite Sprite
        {
            get
            {
                return SpriteRenderer.sprite;
            }
            set
            {
                SpriteRenderer.sprite = value;
            }
        }
        #endregion

        #region Monobehaviour functions
        private void OnEnable()
        {
            if (!Aura.IsCompatible)
            {
                enabled = false;
                return;
            }
        }

        private void Update()
        {
            SetValuesToSpriteRenderer();
        }

        private void Reset()
        {
            _spriteRenderer = null;
            shadowCastingMode = ShadowCastingMode.TwoSided;
            receiveShadows = true;
            SetValuesToSpriteRenderer();
            if (Sprite == null)
            {
                Sprite = Aura.ResourcesCollection.defaultSprite;
            }
        }
        #endregion

        #region Functions
        private void SetValuesToSpriteRenderer()
        {
            SpriteRenderer.shadowCastingMode = shadowCastingMode;
            SpriteRenderer.receiveShadows = receiveShadows;
        }

        public void SetLitShader()
        {
            _spriteRenderer.sharedMaterial.shader = Aura.ResourcesCollection.spriteLitShader;
        }

        public void SetUnlitShader()
        {
            _spriteRenderer.sharedMaterial.shader = Aura.ResourcesCollection.spriteUnlitShader;
        }
        #endregion

        #region GameObject constructor
        /// <summary>
        /// Generic method for crating a GameObject with a AuraSprite component assigned
        /// </summary>
        /// <param name="name">Name of the created GameObject</param>
        /// <param name="sprite">Sprite texture</param>
        /// <returns>The created AuraVolume gameObject</returns>
        public static GameObject CreateGameObject(string name, Sprite sprite)
        {
            GameObject newGameObject = new GameObject(name);
            newGameObject.transform.localScale = Vector3.one * 3.0f;
            AuraSprite auraSprite = newGameObject.AddComponent<AuraSprite>();
            auraSprite.Sprite = sprite;

            return newGameObject;
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Generic method for crating a GameObject with a AuraVolume component assigned
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        /// <param name="name">Name of the created GameObject</param>
        /// <param name="sprite">Sprite texture</param>
        /// <param name="selectAndFocus">Selects and focus on the newly created volume</param>
        /// <returns>The created AuraVolume gameObject</returns>
        private static GameObject CreateGameObject(MenuCommand menuCommand, string name, Sprite sprite)
        {
            GameObject newGameObject = CreateGameObject(name, sprite);

            if (SceneView.lastActiveSceneView != null)
            {
                newGameObject.transform.position = SceneView.lastActiveSceneView.camera.GetSpawnPosition();
            }
            
            GameObjectUtility.SetParentAndAlign(newGameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(newGameObject, "Create " + newGameObject.name);
            Selection.activeObject = newGameObject;
            SceneView.FrameLastActiveSceneViewWithLock();

            return newGameObject;
        }

        /// <summary>
        /// Creates a "cone" volume
        /// </summary>
        /// <param name="menuCommand">Data relative to the invoked menu</param>
        [MenuItem("GameObject/Aura 2/Sprite", false, 50)]
        private static void CreateGameObject(MenuCommand menuCommand)
        {
            CreateGameObject(menuCommand, "Aura Sprite", Aura.ResourcesCollection.defaultSprite);
        }
        #endif
        #endregion
    }
}
