using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Moon))]
public class MoonEditor : Editor
{
    Moon moon;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI(){
        using (var check = new EditorGUI.ChangeCheckScope()){
            base.OnInspectorGUI();
            if(check.changed){
                moon.Generate();
            }
        }

        if(GUILayout.Button("Generate")){
            moon.Generate();
        }
        
        DrawSettingsEditor(moon.shapeSettings, moon.OnShapeSettingsUpdated, ref moon.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(moon.colorSettings, moon.OnColorSettingsUpdated, ref moon.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action OnSettingsUpdated, ref bool foldout, ref Editor editor){
        if(settings != null){
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope()){

                if(foldout){
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                    if(check.changed){
                        if(OnSettingsUpdated != null){
                            OnSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable(){
        moon = (Moon)target;
    }
}
