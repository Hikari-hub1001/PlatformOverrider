#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OriginalLib
{
	[ExecuteInEditMode]
	[CustomEditor(typeof(PlatformOverriderGroup))]
	public class PlatformOverriderGroupEditor : Editor
	{
		private PlatformOverriderGroup _target;

		private void Awake()
		{
			_target = target as PlatformOverriderGroup;
		}

		/// <summary>
		/// InspectorのGUIを更新
		/// </summary>
		public override void OnInspectorGUI()
		{
			//元のInspector部分を表示
			base.OnInspectorGUI();
			PratformButton(_target);
		}


		public static void PratformButton(PlatformOverriderGroup target)
		{
			target.SetPlatform((Platform)GUILayout.SelectionGrid((int)target.m_SelecteTab, Enum2Strings(), 3));
		}

		private static string[] Enum2Strings()
		{
			int enumCount = System.Enum.GetValues(typeof(Platform)).Length;
			string[] TAB_NAMES = new string[enumCount];
			for (int i = 0; i < enumCount; i++)
			{
				TAB_NAMES[i] = ((Platform)i).ToString();
			}
			return TAB_NAMES;
		}


	}

}
#endif