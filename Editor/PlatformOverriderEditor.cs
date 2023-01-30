#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OriginalLib
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(PlatformOverrider))]//拡張するクラスを指定
    public class PlatformOverriderEditor : Editor
    {

        private PlatformOverrider _target;
        private OverriderSettings os;
        private void Awake()
        {
            _target = target as PlatformOverrider;
        }

        /// <summary>
        /// InspectorのGUIを更新
        /// </summary>
        public override void OnInspectorGUI()
        {
            //　シリアライズオブジェクトの更新
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            //OverriderGroupの設定ここから
            //OverriderGroupを設定するフィールド
            _target.Group = EditorGUILayout.ObjectField("Group", _target.Group, typeof(PlatformOverriderGroup), true) as PlatformOverriderGroup;

            if (_target.Group == null)
            {
                //未設定ならメッセージ
                EditorGUILayout.HelpBox("Please set the PlatformOverrideGroup.", MessageType.Info);
                return;
            }
            else
            {
                //Group設定時はボタンを表示
                PlatformOverriderGroupEditor.PratformButton(_target.Group);
            }
            //OverriderGroupの設定ここまで
            if (os != null && os.UseDefault)
            {
                //デフォルトを使うときのみデフォルトをセット
                _target.SetRectTransform(Platform.Default);
            }
            else
            {
                //その他はそれぞれの設定を使用
                _target.SetRectTransform(((Platform)_target.Group?.m_SelecteTab));
            }
            //if (EditorGUI.EndChangeCheck())
            {
                os = SetSetting(_target.Group.m_SelecteTab);
            }

            SerializedProperty o = serializedObject.FindProperty(_target.Group?.m_SelecteTab.ToString());
            if (o != null)
            {
                //var use = serializedObject.FindProperty($"{_target.Group?.m_SelecteTab.ToString()}.UseDefault").boolValue;
                //デフォルト以外の場合は設定するかチェックする
                if (_target.Group?.m_SelecteTab != Platform.Default)
                {
                    os.UseDefault = EditorGUILayout.Toggle("UseDefault", os.UseDefault);
                }

                EditorGUI.BeginDisabledGroup(os.UseDefault);
                //設定値入力フィールド
                EditorGUILayout.PropertyField(o);
                EditorGUI.EndDisabledGroup();

            }
            serializedObject.ApplyModifiedProperties();
        }

        private OverriderSettings SetSetting(Platform platform)
        {
            Type type = _target.GetType();
            FieldInfo fi = type.GetField(platform.ToString(), BindingFlags.Instance | BindingFlags.Public);
            var val = (OverriderSettings)fi.GetValue(_target);
            return val;
        }

    }

}
#endif