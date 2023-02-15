using UnityEngine;

namespace OriginalLib
{


	public enum Platform
	{
		Default,
		MobilePortrait,
		MobileLandscape,
		/*        TabletPortrait,
				TabletLandscape,*/
		MacOS,
		WindowsOS,
		PS5,
		PS4
	}

	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class PlatformOverriderGroup : MonoBehaviour
	{
#if UNITY_EDITOR
		/// <summary>
		/// エディターからのみ参照できる変数なので注意
		/// </summary>
		[HideInInspector]
		public Platform m_SelecteTab;

		private void Change(Platform pratform)
		{
			foreach (Transform o in transform.GetComponentsInChildren<Transform>())
			{
				o.gameObject.GetComponent<PlatformOverrider>()?.SetRectTransform(pratform);
			}
		}

		private void Update()
		{
			//UpdateOverrider(transform);
		}

		private void OnValidate()
		{
			Change(m_SelecteTab);
		}

		void UpdateOverrider(Transform tran)
		{
			foreach (Transform child in tran.GetComponentInChildren<Transform>())
			{
				var po = child.GetComponent<PlatformOverrider>();
				if (po != null && po.Group!=null)
				{
					po.SetRectTransform(m_SelecteTab);
				}
				UpdateOverrider(child);
			}
		}

#endif
	}

}