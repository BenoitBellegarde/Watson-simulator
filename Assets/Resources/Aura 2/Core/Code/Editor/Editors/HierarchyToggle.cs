
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
using UnityEngine;

namespace Aura2API
{
    [InitializeOnLoad]
    public static class HierarchyToggle
    {
        static HierarchyToggle()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

            if (gameObject != null)
            {
                float size = selectionRect.height;
                Rect rect = new Rect(selectionRect.x + selectionRect.width, selectionRect.y, size, size);
                string tooltip;
                bool state;


                //// Cameras
                
                Camera camera = gameObject.GetComponent<Camera>();
                AuraCamera auraCamera = gameObject.GetComponent<AuraCamera>();
                if (camera != null && auraCamera == null)
                {
                    tooltip = "Add Aura Camera Component";
                    rect.x -= size;

                    if (GUI.Button(rect, new GUIContent(Aura.ResourcesCollection.addIconTexture, tooltip), GuiStyles.ButtonImageOnlyNoBorder))
                    {
                        gameObject.AddComponent<AuraCamera>();
                    }
                }

                if (auraCamera != null)
                {
                    state = auraCamera.enabled;
                    tooltip = (state ? "Disable" : "Enable") + " Aura Camera";
                    rect.x -= size;

                    if( GUI.Button(rect, new GUIContent(Aura.ResourcesCollection.cameraMiniIconTexture, tooltip), state ? GuiStyles.ButtonPressedImageOnlyNoBorder : GuiStyles.ButtonImageOnlyNoBorder)) // TODO : MAKE THIS A GENERIC HELPER
                    {
                        auraCamera.enabled = !state;
                    }
                }

                //// Lights

                Light light = gameObject.GetComponent<Light>();
                AuraLight auraLight = gameObject.GetComponent<AuraLight>();
                if (light != null && auraLight == null)
                {
                    tooltip = "Add Aura Light Component";
                    rect.x -= size;

                    if (GUI.Button(rect, new GUIContent(Aura.ResourcesCollection.addIconTexture, tooltip), GuiStyles.ButtonImageOnlyNoBorder))
                    {
                        gameObject.AddComponent<AuraLight>();
                    }
                }

                if (auraLight != null && auraLight.Type != LightType.Area)
                {
                    state = auraLight.enabled;
                    tooltip = (state ? "Disable" : "Enable") + " Aura Light";
                    rect.x -= size;
                    Texture2D texture = Aura.ResourcesCollection.pointLightMiniIconTexture;
                    switch(auraLight.Type)
                    {
                        case LightType.Directional:
                            {
                                texture = Aura.ResourcesCollection.directionalLightMiniIconTexture;
                            }
                            break;

                        case LightType.Spot:
                            {
                                texture = Aura.ResourcesCollection.spotLightMiniIconTexture;
                            }
                            break;
                    }

                    if (GUI.Button(rect, new GUIContent(texture, tooltip), state ? GuiStyles.ButtonPressedImageOnlyNoBorder : GuiStyles.ButtonImageOnlyNoBorder)) // TODO : MAKE THIS A GENERIC HELPER
                    {
                        auraLight.enabled = !state;
                    }
                }
                
                //// Volumes
                
                AuraVolume auraVolume = gameObject.GetComponent<AuraVolume>();
                if (auraVolume != null)
                {
                    state = auraVolume.enabled;
                    tooltip = (state ? "Disable" : "Enable") + " Aura Volume";
                    rect.x -= size;

                    if (GUI.Button(rect, new GUIContent(Aura.ResourcesCollection.shapeMiniIconTexture, tooltip), state ? GuiStyles.ButtonPressedImageOnlyNoBorder : GuiStyles.ButtonImageOnlyNoBorder)) // TODO : MAKE THIS A GENERIC HELPER
                    {
                        auraVolume.enabled = !state;
                    }
                }

                //// Sprites

                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                AuraSprite auraSprite = gameObject.GetComponent<AuraSprite>();
                if (spriteRenderer != null && auraSprite == null)
                {
                    tooltip = "Add Aura Sprite Component";
                    rect.x -= size;

                    if (GUI.Button(rect, new GUIContent(Aura.ResourcesCollection.addIconTexture, tooltip), GuiStyles.ButtonImageOnlyNoBorder))
                    {
                        gameObject.AddComponent<AuraSprite>();
                    }
                }

                if (auraSprite != null)
                {
                    tooltip = "Aura Sprite";
                    rect.x -= size;
                    GUI.Label(rect, new GUIContent(Aura.ResourcesCollection.spriteMiniIconTexture, tooltip), GuiStyles.ButtonPressedImageOnlyNoBorder);
                }
            }
        }
    }
}