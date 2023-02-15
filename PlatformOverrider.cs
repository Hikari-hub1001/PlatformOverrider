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

		private RectTransform rect;

		private void Start()
		{
#if UNITY_EDITOR
			if (Group == null) return;
			Debug.Log($"{Group.m_SelecteTab}で実行します");
			SetRectTransform(Group.m_SelecteTab);
			rect = GetComponent<RectTransform>();
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
			if (rect == null)
			{
				rect = (RectTransform)transform;
			}
/*			rect.anchoredPosition = data.position;
			rect.sizeDelta = data.sizeDelta;
			rect.anchorMin = data.anchorMin;
			rect.anchorMax = data.anchorMax;
			rect.pivot = data.pivot;
			rect.rotation = Quaternion.Euler(data.rotation);
			rect.localScale = data.scale;
			rect.offsetMin = data.offsetMin;
			rect.offsetMax = -data.offsetMax;*/

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
