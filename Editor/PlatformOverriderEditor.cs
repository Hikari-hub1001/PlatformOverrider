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

		private bool _anchorOpen = true;


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

			if (_target.enabled)
			{
				//OverriderGroup�̐ݒ肱���܂�
				if (os != null && os.useDefault)
				{
					//�f�t�H���g���g���Ƃ��̂݃f�t�H���g���Z�b�g
					_target.SetRectTransform(Platform.Default);
				}
				else
				{
					//���̑��͂��ꂼ��̐ݒ���g�p
					_target.SetRectTransform(((Platform)_target.Group?.m_SelecteTab));
				}
			}
			os = SetSetting(_target.Group.m_SelecteTab);

			//Default�v���b�g�t�H�[���ȊO�ł̂�
			//Default���g�p���邩�I�ׂ�
			EditorGUI.BeginDisabledGroup(_target.Group?.m_SelecteTab == Platform.Default);
			os.useDefault = EditorGUILayout.Toggle("UseDefault", os.useDefault);
			EditorGUI.EndDisabledGroup();

			//Default���g�p���Ȃ��ꍇ�̂݊e���ݒ�\
			EditorGUI.BeginDisabledGroup(os.useDefault);
			Layout();
			EditorGUI.EndDisabledGroup();

			serializedObject.ApplyModifiedProperties();
		}

		private OverriderSettings SetSetting(Platform platform)
		{
			Type type = _target.GetType();
			FieldInfo fi = type.GetField(platform.ToString(), BindingFlags.Instance | BindingFlags.Public);
			var val = (OverriderSettings)fi.GetValue(_target);
			return val;
		}

		/// <summary>
		/// �C���X�y�N�^�[�ł̌������𒲐�����
		/// </summary>
		void Layout()
		{
			EditorGUILayout.Space(5);
			os.activation = EditorGUILayout.Toggle("Activation", os.activation);

			Vector3 pos = os.position;
			Vector2 size = os.sizeDelta,
				offMin = os.offsetMin,
				offMax = os.offsetMax;

			EditorGUILayout.BeginHorizontal();

			#region �������̐ݒ�t�B�[���h
			EditorGUILayout.BeginVertical();
			if (os.anchorMin.x == os.anchorMax.x)
			{
				//�A���J�[�̍ŏ��ƍő傪��v���Ă���ꍇ�͍��W�ƕ���ݒ�
				pos.x = CustomFloatField("Pos X", os.position.x);
				size.x = CustomFloatField("Width", os.sizeDelta.x);
			}
			else if (os.anchorMin.x != os.anchorMax.x)
			{
				//�A���J�[�̍ŏ��ƍő傪�s��v�̏ꍇ�͍��ƉE�̋󂫂�ݒ�
				offMin.x = CustomFloatField("Left", os.offsetMin.x);
				offMax.x = CustomFloatField("Right", os.offsetMax.x);
			}
			EditorGUILayout.EndVertical();
			#endregion

			#region �c�����̐ݒ�t�B�[���h
			EditorGUILayout.BeginVertical();
			if (os.anchorMin.y == os.anchorMax.y)
			{
				//�A���J�[�̍ŏ��ƍő傪��v���Ă���ꍇ�͍��W�ƍ�����ݒ�
				pos.y = CustomFloatField("Pos Y", os.position.y);
				size.y = CustomFloatField("Height", os.sizeDelta.y);
			}
			else if (os.anchorMin.y != os.anchorMax.y)
			{
				//�A���J�[�̍ŏ��ƍő傪�s��v�̏ꍇ�͏�Ɖ��̋󂫂�ݒ�
				offMax.y = CustomFloatField("Top", os.offsetMax.y);
				offMin.y = CustomFloatField("Bottom", os.offsetMin.y);
			}
			EditorGUILayout.EndVertical();
			#endregion

			#region Z���W�ݒ�t�B�[���h
			EditorGUILayout.BeginVertical();
			pos.z = CustomFloatField("Pos Z", os.position.z);
			EditorGUILayout.EndVertical();
			#endregion

			os.position = pos;
			os.sizeDelta = size;
			os.offsetMin = offMin;
			os.offsetMax = offMax;

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			#region anchor
			_anchorOpen = EditorGUILayout.Foldout(_anchorOpen, "anchors");
			if (_anchorOpen)
			{
				EditorGUI.indentLevel++;
				os.anchorMin = EditorGUILayout.Vector2Field("Min", os.anchorMin);
				os.anchorMax = EditorGUILayout.Vector2Field("Max", os.anchorMax);
				EditorGUI.indentLevel--;
			}
			#endregion

			os.pivot = EditorGUILayout.Vector2Field("Pivot", os.pivot);
			EditorGUILayout.Space();
			os.rotation = EditorGUILayout.Vector3Field("Rotation", os.rotation);
			os.scale = EditorGUILayout.Vector3Field("Scale", os.scale);

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			if (GUILayout.Button("Set RectTransform"))
			{
				if (!os.useDefault)
				{
					Os2Rect();
				}
				else
				{
					Debug.Log("Change the platform to Default");
				}
			}

		}

		float CustomFloatField(string label, float val)
		{
			Event event_current = Event.current;

			float val_speed = 0.2f;
			EditorGUILayout.BeginVertical();

			float width = EditorGUIUtility.currentViewWidth * 0.3f;

			EditorGUILayout.LabelField(label, GUILayout.MaxWidth(width));
			Rect rect_label = GUILayoutUtility.GetLastRect();
			val = EditorGUILayout.FloatField(val, GUILayout.MaxWidth(width));

			EditorGUILayout.EndVertical();

			//int id = GUIUtility.GetControlID(FocusType.Passive);

			/*Vector2 mous_pos = event_current.mousePosition;
			if (event_current.button == 0)
			{
				switch (event_current.type)
				{
					case EventType.MouseDown:
						if (rect_label.Contains(mous_pos))
						{
							GUIUtility.hotControl = id;
							event_current.Use();
						}
						break;

					case EventType.MouseDrag:
						if (GUIUtility.hotControl == id)
						{
							float dis = event_current.delta.x;
							val = val * 100.0f + dis * 10.0f * val_speed;
							val = Mathf.Floor(Mathf.Abs(val)) / 100f * Mathf.Sign(val);
							Repaint();
						}
						break;

					case EventType.MouseUp:
						if (GUIUtility.hotControl == id)
						{
							GUIUtility.hotControl = 0;
						}
						break;
				}
			}
*/
			//EditorGUIUtility.AddCursorRect(rect_label, MouseCursor.SlideArrow);

			return val;
		}


		void Os2Rect()
		{
			var rect = _target.GetComponent<RectTransform>();

			os.position = rect.anchoredPosition;
			os.anchorMin = rect.anchorMin;
			os.anchorMax = rect.anchorMax;
			os.pivot = rect.pivot;
			os.sizeDelta = rect.sizeDelta;
			os.offsetMin = rect.offsetMin;
			os.offsetMax = rect.offsetMax;
			os.rotation = rect.rotation.eulerAngles;
			os.scale = rect.localScale;
			os.activation = _target.gameObject.activeSelf;
		}


	}
}
#endif