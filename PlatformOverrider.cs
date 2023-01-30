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
		public bool UseDefault = false;

		[SerializeField]
		public Vector3 Position;

		[SerializeField]
		public Vector2 SizeDelta;

		[SerializeField]
		public Vector2 AnchorMin = new(0.5f, 0.5f);

		[SerializeField]
		public Vector2 AnchorMax = new(0.5f, 0.5f);

		[SerializeField]
		public Vector2 Pivot = new(0.5f, 0.5f);

		[SerializeField]
		public Vector3 Rotation;

		[SerializeField]
		public Vector3 Scale = new(1, 1, 1);

		[SerializeField]
		public bool Activation = true;

		public OverriderSettings() { }
		public OverriderSettings(bool useDef)
		{
			UseDefault = useDef;
		}
	}

	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class PlatformOverrider : MonoBehaviour
	{
		public PlatformOverriderGroup Group;
#if UNITY_IOS || UNITY_ANDROID
		private DeviceOrientation old;
#endif
		[SerializeField]
		public OverriderSettings Default = new(false);
		[SerializeField]
		public OverriderSettings MobilePortrait;
		[SerializeField]
		public OverriderSettings MobileLandscape;
		[SerializeField]
		public OverriderSettings TabletPortrait;
		[SerializeField]
		public OverriderSettings TabletLandscape;
		[SerializeField]
		public OverriderSettings MacOS = new(true);
		[SerializeField]
		public OverriderSettings WindowsOS = new(true);
		[SerializeField]
		public OverriderSettings PS5 = new(true);
		[SerializeField]
		public OverriderSettings PS4 = new(true);


		private void Start()
		{
#if UNITY_EDITOR
			Debug.Log($"{Group.m_SelecteTab}で実行します");
			SetRectTransform(Group.m_SelecteTab);
#elif UNITY_IOS || UNITY_ANDROID
				old = Input.deviceOrientation;
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
			SetRectTransform(Platform.PS4);
#elif UNITY_PS5
			SetRectTransform(Platform.PS5);
#else
			Debug.Log("その他OSです");
			SetRectTransform(Platform.Default);
#endif
		}

		private void Update()
		{
#if UNITY_IOS || UNITY_ANDROID
			//モバイル端末の際のみ縦横判定を行う
			//変更した際のみ更新を行う
			if(Input.deviceOrientation != old)
			{
				old = Input.deviceOrientation;
				if(old == DeviceOrientation.Portrait||
					old == DeviceOrientation.PortraitUpsideDown)
				{
					SetRectTransform(Platform.MobilePortrait);
				}
				else if(old == DeviceOrientation.LandscapeLeft||
					old == DeviceOrientation.LandscapeRight)
				{
					SetRectTransform(Platform.MobileLandscape);
				}
			}
#endif
		}


		public void SetRectTransform(Platform type)
		{
			switch (type)
			{
				case Platform.Default:
					SetData(Default);
					break;
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
				case Platform.MacOS:
					SetData(MacOS);
					break;
				case Platform.WindowsOS:
					SetData(WindowsOS);
					break;
				case Platform.PS5:
					SetData(PS5);
					break;
				case Platform.PS4:
					SetData(PS4);
					break;
			}
		}

		private void SetData(OverriderSettings data)
		{
			((RectTransform)transform).anchoredPosition = data.Position;
			((RectTransform)transform).sizeDelta = data.SizeDelta;
			((RectTransform)transform).anchorMin = data.AnchorMin;
			((RectTransform)transform).anchorMax = data.AnchorMax;
			((RectTransform)transform).pivot = data.Pivot;
			((RectTransform)transform).rotation = Quaternion.Euler(data.Rotation) ;
			((RectTransform)transform).localScale = data.Scale;
			gameObject.SetActive( data.Activation);

			//デフォルトが使われる場合のみデフォルトデータで更新する
			if (data.UseDefault)
			{
				SetData(Default);
			}
		}
	}
}
