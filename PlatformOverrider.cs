using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

namespace OriginalLib
{
	[Serializable]
	public class OverriderSettings
	{
		[SerializeField, HideInInspector]
		public bool useDefault = false;

		[SerializeField]
		public Vector3 position;

		[SerializeField]
		public Vector2 sizeDelta = new(100.0f, 100.0f);
		[SerializeField]
		public Vector2 offsetMin;
		[SerializeField]
		public Vector2 offsetMax;

		[SerializeField]
		public Vector2 anchorMin = new(0.5f, 0.5f);
		[SerializeField]
		public Vector2 anchorMax = new(0.5f, 0.5f);

		[SerializeField]
		public Vector2 pivot = new(0.5f, 0.5f);

		[SerializeField]
		public Vector3 rotation;

		[SerializeField]
		public Vector3 scale = new(1.0f, 1.0f, 1.0f);

		[SerializeField]
		public bool activation = true;

		public OverriderSettings() { }
		public OverriderSettings(bool useDef)
		{
			useDefault = useDef;
		}
	}

	[DisallowMultipleComponent]
	public sealed class PlatformOverrider : MonoBehaviour
	{
		#region 変数宣言
		[SerializeField]
		public OverriderSettings Default = new(false);
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
		[SerializeField]
		public OverriderSettings MobilePortrait;
		[SerializeField]
		public OverriderSettings MobileLandscape;
		[SerializeField]
		public OverriderSettings TabletPortrait;
		[SerializeField]
		public OverriderSettings TabletLandscape;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		[SerializeField]
		public OverriderSettings MacOS = new(true);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		[SerializeField]
		public OverriderSettings WindowsOS = new(true);
#endif
#if UNITY_EDITOR || UNITY_PS5
		[SerializeField]
		public OverriderSettings PS5 = new(true);
#endif
#if UNITY_EDITOR || UNITY_PS4
		[SerializeField]
		public OverriderSettings PS4 = new(true);
#endif

		private RectTransform rect;
		#endregion

		private void Start()
		{
#if UNITY_EDITOR

			rect = GetComponent<RectTransform>();
#elif UNITY_IOS || UNITY_ANDROID
			if(Input.deviceOrientation == DeviceOrientation.Portrait||
				Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown){
				SetRectTransform(Platform.MobilePortrait);
			}
			else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft||
				Input.deviceOrientation == DeviceOrientation.LandscapeRight){
				SetRectTransform(Platform.MobileLandscape);
			}
#elif UNITY_STANDALONE_WIN
			Debug.Log("エディターです");
			SetRectTransform(Platform.WindowsOS);
#elif UNITY_STANDALONE_OSX
			Debug.Log("macOSです");
			SetRectTransform(Platform.MacOS);
#elif UNITY_PS4
			Debug.Log("PS4です");
			SetRectTransform(Platform.PS4);
#elif UNITY_PS5
			Debug.Log("PS5です");
			SetRectTransform(Platform.PS5);
#else
			Debug.Log("その他OSです");
			SetRectTransform(Platform.Default);
#endif
		}


		public void SetRectTransform(Platform type)
		{
			switch (type)
			{
				case Platform.Default:
					SetData(Default);
					break;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID

				case Platform.MobilePortrait:
					SetData(MobilePortrait);
					break;
				case Platform.MobileLandscape:
					SetData(MobileLandscape);
					break;
				/*				case Platform.TabletPortrait:
									SetData(MobilePortrait);
									break;
								case Platform.TabletLandscape:
									SetData(MobileLandscape);
									break;*/
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_OSX

				case Platform.MacOS:
					SetData(MacOS);
					break;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

				case Platform.WindowsOS:
					SetData(WindowsOS);
					break;
#endif
#if UNITY_EDITOR || UNITY_PS5

				case Platform.PS5:
					SetData(PS5);
					break;
#endif
#if UNITY_EDITOR || UNITY_PS4

				case Platform.PS4:
					SetData(PS4);
					break;
#endif
			}
		}

		private void SetData(OverriderSettings data)
		{
			if (rect == null)
			{
				rect = (RectTransform)transform;
			}

			gameObject.SetActive(data.activation);

			rect.anchoredPosition = data.position;
			rect.anchorMin = data.anchorMin;
			rect.anchorMax = data.anchorMax;
			rect.pivot = data.pivot;
			rect.rotation = Quaternion.Euler(data.rotation);
			rect.localScale = data.scale;

			Vector2 bk = rect.sizeDelta;
			if (data.anchorMin.x == data.anchorMax.x)
			{
				bk.x = data.sizeDelta.x;
			}
			if (data.anchorMin.y == data.anchorMax.y)
			{
				bk.y = data.sizeDelta.y;
			}
			rect.sizeDelta = bk;

			Vector2 bkmin = rect.offsetMin;
			Vector2 bkmax = rect.offsetMax;
			if (data.anchorMin.x != data.anchorMax.x)
			{
				bkmin.x = data.offsetMin.x;
				bkmax.x = -data.offsetMax.x;
			}
			if (data.anchorMin.y != data.anchorMax.y)
			{
				bkmin.y = data.offsetMin.y;
				bkmax.y = -data.offsetMax.y;
			}
			rect.offsetMin = bkmin;
			rect.offsetMax = bkmax;

			//デフォルトが使われる場合のみデフォルトデータで更新する
			if (data.useDefault)
			{
				SetData(Default);
			}
		}
	}
}
