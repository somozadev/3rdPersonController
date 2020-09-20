using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// @author: Sergio Vozmediano Ávilas
/// 31/03/2019
/// 
/// 
/// Simple tool that takes all the fbx files from a folder (loadPath), and looks for animation clips inside each fbx.
/// If there are any animations, it duplicates them in the savePath folder with the same name as the fbx files.
/// </summary>

public class AnimationClipDuplicationTool : EditorWindow {

    public string loadPath = "Load Path";
    public string savePath = "Save Path";
    public List<AnimationClipOverrider> clipsToDuplicate = new List<AnimationClipOverrider>();

    Vector2 scrollPos;

    [MenuItem("Window/ACDT")]
    public static void ShowWindow() {
        GetWindow(typeof(AnimationClipDuplicationTool));
    }


    void OnGUI() {

        LoadPathGUI();       

        EditorGUILayout.Space();

        SavePathGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button(" Duplicate ")) {
            duplicateClips();
        }

        EditorGUILayout.Space();

        ClipPathListGUI();
    }


    private void poolClipsToDuplicateList(string[] files) {

        clipsToDuplicate = new List<AnimationClipOverrider>();

        for (int i = 0; i < files.Length; i++) {
            if (files[i].EndsWith(".fbx")) {
                clipsToDuplicate.Add(new AnimationClipOverrider {file = "Assets" + files[i].Substring(Application.dataPath.Length), duplicate = true });
            }
        }
    }

    private void LoadPathGUI() {
        // Loading Path
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Load Path");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(loadPath);
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button(" Open ")) {
            string sP = EditorUtility.OpenFolderPanel("Duplicate Animation Clips From...", "", "");
            loadPath = sP != null && !sP.Equals("") ? sP : loadPath;
            string[] files = Directory.GetFiles(loadPath);
            poolClipsToDuplicateList(files);
        }

        EditorGUILayout.EndVertical();
    }

    private void SavePathGUI() {
        // Saving Path
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Save Path");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(savePath);
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button(" Open ")) {
            string sP = EditorUtility.OpenFolderPanel("Save Duplicated Animation Clips to...", "", "");
            savePath = sP != null && !sP.Equals("") ? sP : savePath;
        }

        EditorGUILayout.EndVertical();
    }

    private void ClipPathListGUI() {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // Print ugly inspector list
        ScriptableObject scriptableObj = this;
        SerializedObject serialObj = new SerializedObject(scriptableObj);
        SerializedProperty serialProp = serialObj.FindProperty("clipsToDuplicate");

        EditorGUILayout.PropertyField(serialProp, true);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        serialObj.ApplyModifiedProperties();
    }

    private void duplicateClips() {

        for (int i = 0; i < clipsToDuplicate.Count; i++) {
            if (clipsToDuplicate[i].duplicate) {
                // Load the original animation clip from the prefab
                AnimationClip orgClip = Instantiate((AnimationClip)AssetDatabase.LoadAssetAtPath(clipsToDuplicate[i].file, typeof(AnimationClip)));

                if (orgClip) {
                   AssetDatabase.CreateAsset(orgClip, "Assets" + savePath.Substring(Application.dataPath.Length) + "/" + Path.GetFileNameWithoutExtension(clipsToDuplicate[i].file) + ".anim");
                }   
            }
        }

        AssetDatabase.Refresh();
    }

    [System.Serializable]
    public struct AnimationClipOverrider {
        public string file;
        public bool duplicate;
    }
}