using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

#if TMP_VERSION_2_1_0_OR_NEWER
using TMP_UiEditorPanel = TMPro.EditorUtilities.TMP_EditorPanelUI;
#else
using TMP_UiEditorPanel = TMPro.EditorUtilities.TMP_UiEditorPanel;
#endif

namespace RTLTMPro
{
    [CustomEditor(typeof(RTLTMPTextFixer)), CanEditMultipleObjects]
    public class RTLTMPTextFixerEditor : Editor
    {
        private SerializedProperty originalTextProp;
        private SerializedProperty preserveNumbersProp;
        private SerializedProperty farsiProp;
        private SerializedProperty fixTagsProp;
        private SerializedProperty forceFixProp;

        private bool foldout;
        private RTLTMPTextFixer tmpro;

        bool haveChangedProperties = false;

        protected void OnEnable()
        {
            foldout = true;
            preserveNumbersProp = serializedObject.FindProperty("preserveNumbers");
            farsiProp = serializedObject.FindProperty("farsi");
            fixTagsProp = serializedObject.FindProperty("fixTags");
            forceFixProp = serializedObject.FindProperty("forceFix");
            originalTextProp = serializedObject.FindProperty("originalText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            tmpro = (RTLTMPTextFixer)target;

            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(originalTextProp, new GUIContent("RTL Text Input Box"));

            ListenForZeroWidthNoJoiner();

            if (EditorGUI.EndChangeCheck())
                OnChanged();

            serializedObject.ApplyModifiedProperties();

            //base.OnInspectorGUI();

            foldout = EditorGUILayout.Foldout(foldout, "RTL Settings", TMP_UIStyleManager.boldFoldout);
            if (foldout)
            {
                EditorGUI.BeginChangeCheck();
                DrawOptions();

                if (GUILayout.Button("Re-Fix"))
                    haveChangedProperties = true;

                if (EditorGUI.EndChangeCheck())
                    haveChangedProperties = true;
            }

            if (haveChangedProperties)
                OnChanged();

            serializedObject.ApplyModifiedProperties();
        }

        protected void OnChanged()
        {
            tmpro.UpdateText();
            haveChangedProperties = false;
            tmpro.havePropertiesChanged = true;
            tmpro.GetComponent<TMP_Text>().havePropertiesChanged = true;
            tmpro.GetComponent<TMP_Text>().ComputeMarginSize();
            EditorUtility.SetDirty(target);
        }

        protected virtual void DrawOptions()
        {
            EditorGUILayout.BeginHorizontal();
            farsiProp.boolValue = GUILayout.Toggle(farsiProp.boolValue, new GUIContent("Farsi"));
            forceFixProp.boolValue = GUILayout.Toggle(forceFixProp.boolValue, new GUIContent("Force Fix"));
            preserveNumbersProp.boolValue = GUILayout.Toggle(preserveNumbersProp.boolValue, new GUIContent("Preserve Numbers"));

            if (tmpro.GetComponent<TMP_Text>().richText)
                fixTagsProp.boolValue = GUILayout.Toggle(fixTagsProp.boolValue, new GUIContent("FixTags"));

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void ListenForZeroWidthNoJoiner()
        {
            var editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            bool shortcutPressed = (Event.current.modifiers & EventModifiers.Control) != 0 &&
                                   (Event.current.modifiers & EventModifiers.Shift) != 0 &&
                                   Event.current.type == EventType.KeyUp &&
                                   Event.current.keyCode == KeyCode.Alpha2;

            if (!shortcutPressed) return;

            originalTextProp.stringValue = originalTextProp.stringValue.Insert(editor.cursorIndex, ((char)SpecialCharacters.ZeroWidthNoJoiner).ToString());
            editor.selectIndex++;
            editor.cursorIndex++;
            Event.current.Use();
            Repaint();
        }
    }
}