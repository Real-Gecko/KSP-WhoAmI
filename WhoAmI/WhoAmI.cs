using System;
using UnityEngine;

namespace WhoAmI
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class WhoAmI: MonoBehaviour
	{
		private GUIStyle LabelStyle;
		private string InfoText;
		private Texture2D Background;

		void Awake() {
			// Label background
			Background = new Texture2D (1, 1);
			Background.SetPixel(0, 0, Color.white);
			// Label style
			LabelStyle = new GUIStyle ();
			LabelStyle.stretchWidth = true;
			LabelStyle.stretchHeight = true;
			LabelStyle.alignment = TextAnchor.MiddleCenter;
			LabelStyle.fontSize = (Screen.height/25);
			LabelStyle.fontStyle = FontStyle.Bold;
			LabelStyle.normal.textColor = Color.black;
			LabelStyle.normal.background = Background;
		}

		void OnDestroy()
		{
			Destroy (Background);
		}

		private void OnGUI() {
			if (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA ||
				CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal)
			{
				Kerbal kerbinaut = CameraManager.Instance.IVACameraActiveKerbal;
				InfoText = String.Format (
					"{0} - {1} ({2})",
					kerbinaut.name,
					kerbinaut.protoCrewMember.experienceTrait.TypeName,
					kerbinaut.InPart.name
				);
				GUILayout.BeginArea (
					new Rect (
						0,
						Screen.height - Screen.height/20,
						Screen.width,
						Screen.height/20
					),
					LabelStyle
				);
				GUILayout.Label (InfoText, LabelStyle);
				GUILayout.EndArea ();
			}
		}
	}
}
