#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OriginalLib
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(PlatformOverriderGroup))]
    public class PlatformOverriderGroupEditor : Editor
    {

		/*private static string[] TAB_NAMES = new string[] {
		"Default",
		"MobilePortrait",
		"MobileLandscape",
		"MacOS",
		"WindowsOS",
		"PS5",
		"PS4"
		};*/

		private PlatformOverriderGroup _target;

        private void Awake()
        {
            _target = target as PlatformOverriderGroup;
        }

        /// <summary>
        /// InspectorÇÃGUIÇçXêV
        /// </summary>
        public override void OnInspectorGUI()
        {
            //å≥ÇÃInspectorïîï™Çï\é¶
            base.OnInspectorGUI();
            PratformButton(_target);
        }


        public static void PratformButton(PlatformOverriderGroup target)
		{
            target.m_SelecteTab = (Platform)GUILayout.SelectionGrid((int)target.m_SelecteTab, Enum2Strings(), 3);
        }

        private static string[] Enum2Strings()
		{
            int enumCount = System.Enum.GetValues(typeof(Platform)).Length;
            string[] TAB_NAMES = new string[enumCount];
            for(int i = 0; i < enumCount; i++)
			{
                TAB_NAMES[i] = ((Platform)i).ToString();
			}
            return TAB_NAMES;
		}


    }

}
#endif