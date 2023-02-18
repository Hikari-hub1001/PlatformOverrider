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

		private PlatformOverriderGroup pog;

		private bool _anchorOpen = true;


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

			GetPOG();
			if (pog == null)
			{
				//親にPOGがない場合
				EditorGUILayout.HelpBox("Please set the 'PlatformOverriderGroup' component to the parent element.", MessageType.Info);
				return;
			}
			else
			{
				//Group�ݒ莞�̓{�^����\��
				PlatformOverriderGroupEditor.PratformButton(pog);
			}
			EditorGUI.BeginChangeCheck();

			os = SetSetting(pog.m_SelecteTab);

			//Defaultプラットフォーム以外でのみ
			//Defaultを使用するか選べる
			EditorGUI.BeginDisabledGroup(pog.m_SelecteTab == Platform.Default);
			os.useDefault = EditorGUILayout.Toggle("UseDefault", os.useDefault);
			EditorGUI.EndDisabledGroup();

			//Defaultを使用しない場合のみ各自設定可能
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
		/// インスペクターでの見え方を調整する
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

			#region 横方向の設定フィールド
			EditorGUILayout.BeginVertical();
			if (os.anchorMin.x == os.anchorMax.x)
			{
				//アンカーの最小と最大が一致している場合は座標と幅を設定
				pos.x = CustomFloatField("Pos X", os.position.x);
				size.x = CustomFloatField("Width", os.sizeDelta.x);
			}
			else if (os.anchorMin.x != os.anchorMax.x)
			{
				//アンカーの最小と最大が不一致の場合は左と右の空きを設定
				offMin.x = CustomFloatField("Left", os.offsetMin.x);
				offMax.x = CustomFloatField("Right", os.offsetMax.x);
			}
			EditorGUILayout.EndVertical();
			#endregion

			#region 縦方向の設定フィールド
			EditorGUILayout.BeginVertical();
			if (os.anchorMin.y == os.anchorMax.y)
			{
				//アンカーの最小と最大が一致している場合は座標と高さを設定
				pos.y = CustomFloatField("Pos Y", os.position.y);
				size.y = CustomFloatField("Height", os.sizeDelta.y);
			}
			else if (os.anchorMin.y != os.anchorMax.y)
			{
				//アンカーの最小と最大が不一致の場合は上と下の空きを設定
				offMax.y = CustomFloatField("Top", os.offsetMax.y);
				offMin.y = CustomFloatField("Bottom", os.offsetMin.y);
			}
			EditorGUILayout.EndVertical();
			#endregion

			#region Z座標設定フィールド
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

		void GetPOG()
		{
			if (pog == null)
			{
				Transform parent = _target.transform.parent;
				while (true)
				{
					pog = parent.GetComponent<PlatformOverriderGroup>();
					if (pog != null) return;
					parent = parent.parent;
					if (parent == null) return;
				}
			}
		}

	}
}
#endif