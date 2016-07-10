using System;
using UnityEngine;

namespace WhoAmI
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class WhoAmI: MonoBehaviour
	{
		private GUIStyle labelStyle;
		private string InfoText;
		private Texture2D Background;
		private bool isVisible = true;
		private bool isActive = true;
		private DateTime activatedAt = DateTime.Now;

		void Awake() {
			// Label background
			Background = new Texture2D (1, 1);
			Background.SetPixel(0, 0, Color.white);
			// Label style
			labelStyle = new GUIStyle ();
			labelStyle.stretchWidth = true;
			labelStyle.stretchHeight = true;
			labelStyle.alignment = TextAnchor.MiddleCenter;
			labelStyle.fontSize = (Screen.height/30);
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.normal.textColor = Color.black;
			labelStyle.normal.background = Background;
			GameEvents.onHideUI.Add (onHideUI);
			GameEvents.onShowUI.Add (onShowUI);
			GameEvents.OnIVACameraKerbalChange.Add (OnIVACameraKerbalChange);
			GameEvents.OnCameraChange.Add (OnCameraChange);
		}

		void OnDestroy()
		{
			Destroy (Background);
			GameEvents.onHideUI.Remove (onHideUI);
			GameEvents.onShowUI.Remove (onShowUI);
			GameEvents.OnIVACameraKerbalChange.Remove (OnIVACameraKerbalChange);
			GameEvents.OnCameraChange.Remove (OnCameraChange);
		}

		private void onHideUI() {
			isVisible = false;
		}

		private void onShowUI() {
			isVisible = true;
		}

		private void OnCameraChange(CameraManager.CameraMode cameraMode) {
			if (cameraMode == CameraManager.CameraMode.IVA) {
				isActive = true;
				activatedAt = DateTime.Now;
			} else
				isActive = false;
		}

		private void OnIVACameraKerbalChange(Kerbal kerbal) {
			activatedAt = DateTime.Now;
		}

		private void OnGUI() {
			if (isActive && isVisible && activatedAt.AddSeconds(4) > DateTime.Now) {
				Kerbal kerbinaut = CameraManager.Instance.IVACameraActiveKerbal;
				InfoText = String.Format (
					"{0} - {1} ({2})",
					kerbinaut.name,
					kerbinaut.protoCrewMember.experienceTrait.TypeName,
					kerbinaut.InPart.partInfo.title
				);
				Vector2 labelSize = labelStyle.CalcSize (new GUIContent(InfoText));
				GUILayout.BeginArea (
					new Rect (
						(Screen.width - labelSize.x - 10)/2,
						Screen.height - Screen.height / 25 - 10,
						labelSize.x + 10,
						Screen.height / 25
					),
					labelStyle
				);
				GUILayout.Label (InfoText, labelStyle);
				GUILayout.EndArea ();
			}
		}
	}
}
