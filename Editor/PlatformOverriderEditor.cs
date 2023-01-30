#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OriginalLib
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(PlatformOverrider))]//�g������N���X���w��
    public class PlatformOverriderEditor : Editor
    {

        private PlatformOverrider _target;
        private OverriderSettings os;
        private void Awake()
        {
            _target = target as PlatformOverrider;
        }

        /// <summary>
        /// Inspector��GUI���X�V
        /// </summary>
        public override void OnInspectorGUI()
        {
            //�@�V���A���C�Y�I�u�W�F�N�g�̍X�V
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            //OverriderGroup�̐ݒ肱������
            //OverriderGroup��ݒ肷��t�B�[���h
            _target.Group = EditorGUILayout.ObjectField("Group", _target.Group, typeof(PlatformOverriderGroup), true) as PlatformOverriderGroup;

            if (_target.Group == null)
            {
                //���ݒ�Ȃ烁�b�Z�[�W
                EditorGUILayout.HelpBox("Please set the PlatformOverrideGroup.", MessageType.Info);
                return;
            }
            else
            {
                //Group�ݒ莞�̓{�^����\��
                PlatformOverriderGroupEditor.PratformButton(_target.Group);
            }
            //OverriderGroup�̐ݒ肱���܂�
            if (os != null && os.UseDefault)
            {
                //�f�t�H���g���g���Ƃ��̂݃f�t�H���g���Z�b�g
                _target.SetRectTransform(Platform.Default);
            }
            else
            {
                //���̑��͂��ꂼ��̐ݒ���g�p
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
                //�f�t�H���g�ȊO�̏ꍇ�͐ݒ肷�邩�`�F�b�N����
                if (_target.Group?.m_SelecteTab != Platform.Default)
                {
                    os.UseDefault = EditorGUILayout.Toggle("UseDefault", os.UseDefault);
                }

                EditorGUI.BeginDisabledGroup(os.UseDefault);
                //�ݒ�l���̓t�B�[���h
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