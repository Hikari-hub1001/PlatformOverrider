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
	public sealed class PlatformOverriderGroup : MonoBehaviour
	{
		/// <summary>
		/// エディターからのみ参照できる変数なので注意
		/// </summary>
		[HideInInspector]
		public Platform m_SelecteTab { get; private set; }

#if UNITY_IOS || UNITY_ANDROID
		private DeviceOrientation old;
#endif

		private void Start()
		{
			Debug.Log($"{m_SelecteTab}で実行します");
		}

#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
		private void Update()
		{
#if UNITY_IOS || UNITY_ANDROID
			//モバイル端末の際のみ縦横判定を行う
			//変更した際のみ更新を行う
			if (Input.deviceOrientation != old)
			{
				old = Input.deviceOrientation;
				if (old == DeviceOrientation.Portrait ||
					old == DeviceOrientation.PortraitUpsideDown)
				{
					m_SelecteTab = Platform.MobilePortrait;
				}
				else if (old == DeviceOrientation.LandscapeLeft ||
					old == DeviceOrientation.LandscapeRight)
				{
					m_SelecteTab = Platform.MobileLandscape;
				}
			}
#endif
			UpdateOverrider(transform);
		}
#endif

		void UpdateOverrider(Transform tran)
		{
			foreach (Transform child in tran.GetComponentInChildren<Transform>())
			{
				var po = child.GetComponent<PlatformOverrider>();
				if (po != null)
				{
					po.SetRectTransform(m_SelecteTab);
				}
				UpdateOverrider(child);
			}
		}

#if UNITY_EDITOR
		public void SetPlatform(Platform pf)
		{
			m_SelecteTab = pf;
		}
#endif

	}

}