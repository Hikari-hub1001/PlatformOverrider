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
		/// �G�f�B�^�[����̂ݎQ�Ƃł���ϐ��Ȃ̂Œ���
		/// </summary>
		[HideInInspector]
		public Platform m_SelecteTab { get; private set; }

#if UNITY_IOS || UNITY_ANDROID
		private DeviceOrientation old;
#endif

		private void Start()
		{
			Debug.Log($"{m_SelecteTab}�Ŏ��s���܂�");
		}

#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
		private void Update()
		{
#if UNITY_IOS || UNITY_ANDROID
			//���o�C���[���̍ۂ̂ݏc��������s��
			//�ύX�����ۂ̂ݍX�V���s��
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