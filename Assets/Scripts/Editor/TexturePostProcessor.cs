using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class TexturePostProcessor : AssetPostprocessor
{
    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = assetImporter as TextureImporter;
        importer.anisoLevel = 1;
        importer.filterMode = FilterMode.Bilinear;
        importer.npotScale = TextureImporterNPOTScale.ToLarger;
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePivot = new Vector2(0,1);//TOPLEFT
        int width, height; 
        GetImageSize(importer,out width,out height);
        importer.mipmapEnabled = true;
        importer.spritePixelsPerUnit =(float) Math.Min(height,width)/10;
    }
    public static bool GetImageSize(TextureImporter importer, out int width, out int height) {
            if (importer != null) {
                object[] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);
     
                width = (int)args[0];
                height = (int)args[1];
     
                return true;
            
        }
     
        height = width = 0;
        return false;
    }
}