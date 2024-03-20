#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AnimationHierarchyEditor : EditorWindow
{
    private const int ColumnWidth = 300;
    private readonly List<AnimationClip> _animationClips = new();

    private Animator _animatorObject;
    private Hashtable _paths;
    private ArrayList _pathsKeys;

    private Vector2 _scrollPos = Vector2.zero;
    private string _newRoot = "SomeNewObject/Root";

    private string _originalRoot = "Root";
    private string _replacementNewRoot;

    private string _replacementOldRoot;

    private readonly Dictionary<string, string> _tempPathOverrides = new();


    private void OnGUI()
    {
        if (Event.current.type == EventType.ValidateCommand)
            switch (Event.current.commandName)
            {
                case "UndoRedoPerformed":
                    FillModel();
                    break;
            }

        if (_animationClips.Count > 0)
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUIStyle.none);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Referenced Animator (Root):", GUILayout.Width(ColumnWidth));

            _animatorObject = (Animator)EditorGUILayout.ObjectField(
                _animatorObject,
                typeof(Animator),
                true,
                GUILayout.Width(ColumnWidth));


            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Animation Clip:", GUILayout.Width(ColumnWidth));

            if (_animationClips.Count == 1)
                _animationClips[0] = (AnimationClip)EditorGUILayout.ObjectField(
                    _animationClips[0],
                    typeof(AnimationClip),
                    true,
                    GUILayout.Width(ColumnWidth));
            else
                GUILayout.Label("Multiple Anim Clips: " + _animationClips.Count, GUILayout.Width(ColumnWidth));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();

            _originalRoot = EditorGUILayout.TextField(_originalRoot, GUILayout.Width(ColumnWidth));
            _newRoot = EditorGUILayout.TextField(_newRoot, GUILayout.Width(ColumnWidth));
            if (GUILayout.Button("Replace Root"))
            {
                Debug.Log("O: " + _originalRoot + " N: " + _newRoot);
                ReplaceRoot(_originalRoot, _newRoot);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Reference path:", GUILayout.Width(ColumnWidth));
            GUILayout.Label("Animated properties:", GUILayout.Width(ColumnWidth * 0.5f));
            GUILayout.Label("(Count)", GUILayout.Width(60));
            GUILayout.Label("Object:", GUILayout.Width(ColumnWidth));
            EditorGUILayout.EndHorizontal();

            if (_paths != null)
                foreach (string path in _pathsKeys)
                    GUICreatePathItem(path);

            GUILayout.Space(40);
            GUILayout.EndScrollView();
        }
        else
        {
            GUILayout.Label("Please select an Animation Clip");
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

    private void OnSelectionChange()
    {
        if (Selection.objects.Length > 1)
        {
            Debug.Log("Length? " + Selection.objects.Length);
            _animationClips.Clear();
            foreach (Object o in Selection.objects)
                if (o is AnimationClip clip)
                    _animationClips.Add(clip);
        }
        else if (Selection.activeObject is AnimationClip)
        {
            _animationClips.Clear();
            _animationClips.Add((AnimationClip)Selection.activeObject);
            FillModel();
        }
        else
        {
            _animationClips.Clear();
        }

        Repaint();
    }

    [MenuItem("Window/Animation Hierarchy Editor")]
    private static void ShowWindow()
    {
        GetWindow<AnimationHierarchyEditor>();
    }


    private void GUICreatePathItem(string path)
    {
        string newPath = path;
        GameObject obj = FindObjectInRoot(path);
        GameObject newObj;
        ArrayList properties = (ArrayList)_paths[path];

        string pathOverride = path;

        if (_tempPathOverrides.ContainsKey(path)) pathOverride = _tempPathOverrides[path];

        EditorGUILayout.BeginHorizontal();

        pathOverride = EditorGUILayout.TextField(pathOverride, GUILayout.Width(ColumnWidth));
        if (pathOverride != path) _tempPathOverrides[path] = pathOverride;

        if (GUILayout.Button("Change", GUILayout.Width(60)))
        {
            newPath = pathOverride;
            _tempPathOverrides.Remove(path);
        }

        EditorGUILayout.LabelField(
            properties != null ? properties.Count.ToString() : "0",
            GUILayout.Width(60)
        );

        Color standardColor = GUI.color;

        GUI.color = obj != null 
            ? Color.green 
            : Color.red;

        newObj = (GameObject)EditorGUILayout.ObjectField(
            obj,
            typeof(GameObject),
            true,
            GUILayout.Width(ColumnWidth)
        );

        GUI.color = standardColor;

        EditorGUILayout.EndHorizontal();

        try
        {
            if (obj != newObj) UpdatePath(path, ChildPath(newObj));

            if (newPath != path) UpdatePath(path, newPath);
        }
        catch (UnityException ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void FillModel()
    {
        _paths = new Hashtable();
        _pathsKeys = new ArrayList();

        foreach (AnimationClip animationClip in _animationClips)
        {
            FillModelWithCurves(AnimationUtility.GetCurveBindings(animationClip));
            FillModelWithCurves(AnimationUtility.GetObjectReferenceCurveBindings(animationClip));
        }
    }

    private void FillModelWithCurves(EditorCurveBinding[] curves)
    {
        foreach (EditorCurveBinding curveData in curves)
        {
            string key = curveData.path;

            if (_paths.ContainsKey(key))
            {
                ((ArrayList)_paths[key]).Add(curveData);
            }
            else
            {
                ArrayList newProperties = new ArrayList();
                newProperties.Add(curveData);
                _paths.Add(key, newProperties);
                _pathsKeys.Add(key);
            }
        }
    }


    private void ReplaceRoot(string oldRoot, string newRoot)
    {
        float fProgress = 0.0f;
        _replacementOldRoot = oldRoot;
        _replacementNewRoot = newRoot;

        AssetDatabase.StartAssetEditing();

        for (int iCurrentClip = 0; iCurrentClip < _animationClips.Count; iCurrentClip++)
        {
            AnimationClip animationClip = _animationClips[iCurrentClip];
            Undo.RecordObject(animationClip, "Animation Hierarchy Root Change");

            for (int iCurrentPath = 0; iCurrentPath < _pathsKeys.Count; iCurrentPath++)
            {
                string path = _pathsKeys[iCurrentPath] as string;
                ArrayList curves = (ArrayList)_paths[path];

                for (int i = 0; i < curves.Count; i++)
                {
                    EditorCurveBinding binding = (EditorCurveBinding)curves[i];

                    if (path.Contains(_replacementOldRoot))
                        if (!path.Contains(_replacementNewRoot))
                        {
                            string sNewPath = Regex.Replace(path, "^" + _replacementOldRoot, _replacementNewRoot);

                            AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                            if (curve != null)
                            {
                                AnimationUtility.SetEditorCurve(animationClip, binding, null);
                                binding.path = sNewPath;
                                AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                            }
                            else
                            {
                                ObjectReferenceKeyframe[] objectReferenceCurve =
                                    AnimationUtility.GetObjectReferenceCurve(animationClip, binding);
                                AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);
                                binding.path = sNewPath;
                                AnimationUtility.SetObjectReferenceCurve(animationClip, binding, objectReferenceCurve);
                            }
                        }
                }

                // Update the progress meter
                float fChunk = 1f / _animationClips.Count;
                fProgress = iCurrentClip * fChunk + fChunk * (iCurrentPath / (float)_pathsKeys.Count);

                EditorUtility.DisplayProgressBar(
                    "Animation Hierarchy Progress",
                    "How far along the animation editing has progressed.",
                    fProgress);
            }
        }

        AssetDatabase.StopAssetEditing();
        EditorUtility.ClearProgressBar();

        FillModel();
        Repaint();
    }

    private void UpdatePath(string oldPath, string newPath)
    {
        if (_paths[newPath] != null) throw new UnityException("Path " + newPath + " already exists in that animation!");
        AssetDatabase.StartAssetEditing();
        for (int iCurrentClip = 0; iCurrentClip < _animationClips.Count; iCurrentClip++)
        {
            AnimationClip animationClip = _animationClips[iCurrentClip];
            Undo.RecordObject(animationClip, "Animation Hierarchy Change");

            //recreating all curves one by one
            //to maintain proper order in the editor - 
            //slower than just removing old curve
            //and adding a corrected one, but it's more
            //user-friendly
            for (int iCurrentPath = 0; iCurrentPath < _pathsKeys.Count; iCurrentPath++)
            {
                string path = _pathsKeys[iCurrentPath] as string;
                ArrayList curves = (ArrayList)_paths[path];

                for (int i = 0; i < curves.Count; i++)
                {
                    EditorCurveBinding binding = (EditorCurveBinding)curves[i];
                    AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, binding);
                    ObjectReferenceKeyframe[] objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(animationClip, binding);


                    if (curve != null)
                        AnimationUtility.SetEditorCurve(animationClip, binding, null);
                    else
                        AnimationUtility.SetObjectReferenceCurve(animationClip, binding, null);

                    if (path == oldPath)
                        binding.path = newPath;

                    if (curve != null)
                        AnimationUtility.SetEditorCurve(animationClip, binding, curve);
                    else
                        AnimationUtility.SetObjectReferenceCurve(animationClip, binding, objectReferenceCurve);

                    float fChunk = 1f / _animationClips.Count;
                    float fProgress = iCurrentClip * fChunk + fChunk * (iCurrentPath / (float)_pathsKeys.Count);

                    EditorUtility.DisplayProgressBar(
                        "Animation Hierarchy Progress",
                        "How far along the animation editing has progressed.",
                        fProgress);
                }
            }
        }

        AssetDatabase.StopAssetEditing();
        EditorUtility.ClearProgressBar();
        FillModel();
        Repaint();
    }

    private GameObject FindObjectInRoot(string path)
    {
        if (_animatorObject == null) return null;

        Transform child = _animatorObject.transform.Find(path);

        if (child != null)
            return child.gameObject;
        return null;
    }

    private string ChildPath(GameObject obj, bool sep = false)
    {
        if (_animatorObject == null) throw new UnityException("Please assign Referenced Animator (Root) first!");

        if (obj == _animatorObject.gameObject) return "";

        if (obj.transform.parent == null)
            throw new UnityException("Object must belong to " + _animatorObject + "!");
        return ChildPath(obj.transform.parent.gameObject, true) + obj.name + (sep ? "/" : "");
    }
}

#endif