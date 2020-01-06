using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
 
public class SelectLightmapStatic : EditorWindow
{
    [MenuItem("Custom/Select Lightmap Static")]
    static void Init()
    {
 
        // Find all the lightmap static objects
 
        Object[] All_GOs = FindObjectsOfType(typeof(GameObject));
 
        GameObject[] GOArray;
        GOArray = new GameObject[All_GOs.Length];
 
        int ArrayPointer = 0;
 
 
        foreach(GameObject EachGO in All_GOs)
        {
            StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags( EachGO );
 
            if ((flags & StaticEditorFlags.LightmapStatic)!=0)
            {
                if( EachGO.GetComponent<MeshFilter>() != null )
                {
                    GOArray[ArrayPointer] = EachGO;
                    ArrayPointer += 1;
                }
            }
 
        }
 
        Selection.objects = GOArray;
 
    }
 
}
 