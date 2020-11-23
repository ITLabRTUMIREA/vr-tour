using Excursion360_Builder.Shared.States.Items.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace Excursion360_Builder.Editor.States.Items
{
    class FieldItemEditor : EditorBase
    {
        public void Draw(State state)
        {
            if (GUILayout.Button("Add"))
            {
                FieldItem item = Undo.AddComponent<FieldItem>(state.gameObject);
                item.Reset();
            }
            EditorGUILayout.Space();

            var fieldItems = state.GetComponents<FieldItem>();
            foreach (var fieldItem in fieldItems)
            {
                EditorGUILayout.BeginHorizontal();
                
                var fieldItemTitle = GetTitleStringOf(fieldItem.title);
                GUILayout.Label(fieldItemTitle, EditorStyles.boldLabel);

                if (GUILayout.Button("Reset"))
                {
                    fieldItem.Reset();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                Undo.RecordObject(fieldItem, "Edit fieldItem title");

                fieldItem.title = EditorGUILayout.TextField("Title:", fieldItem.title);

                EditorGUILayout.LabelField("Corners:");
                EditorGUILayout.BeginHorizontal();

                for (int i = 0; i < fieldItem.vertices.Length; i++)
                {
                    var vertex = fieldItem.vertices[i];
                    var buttonStyle = Styles.ToggleButtonStyleNormal;
                    if (StateItemPlaceEditor.EditableItem == (object)vertex)
                        buttonStyle = Styles.ToggleButtonStyleToggled;
                    if (GUILayout.Button(vertex.index.ToString(), buttonStyle))
                    {
                        if (StateItemPlaceEditor.EditableItem == (object)vertex)
                        {
                            StateItemPlaceEditor.CleadEditing();
                        }
                        else
                        {
                            StateItemPlaceEditor.EnableEditing(state, vertex, Color.green);
                        }
                    }

                }
                EditorGUILayout.EndHorizontal();

                string[] content = Enum.GetNames(typeof(FieldItem.ContentType));
                int value_now = (int) fieldItem.contentType;
                fieldItem.contentType = (FieldItem.ContentType)EditorGUILayout.Popup(new GUIContent("Content:"), value_now, content);

                switch (fieldItem.contentType)
                {
                    case FieldItem.ContentType.Photo:
                        var previewTexture = fieldItem.texture;
                        if (previewTexture == null)
                        {
                            previewTexture = EditorGUIUtility.whiteTexture;
                        }
                        fieldItem.texture = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(false, 150.0f), fieldItem.texture, typeof(Texture2D), false) as Texture;
                        break;
                    case FieldItem.ContentType.Video:
                        var previewVideoClip = fieldItem.videoClip;
                        if (previewVideoClip == null)
                        {
                            previewTexture = EditorGUIUtility.whiteTexture;
                        }
                        fieldItem.videoClip = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(false, 150.0f), fieldItem.videoClip, typeof(VideoClip), false) as VideoClip;
                        break;
                }
            }
        }
    }
}
