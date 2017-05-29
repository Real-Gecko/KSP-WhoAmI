using System;
using UnityEngine;

namespace WhoAmI
{
	[KSPAddon (KSPAddon.Startup.Flight, false)]
	public class WhoAmI: MonoBehaviour
	{
		void Awake ()
		{
			GameEvents.OnIVACameraKerbalChange.Add (OnIVACameraKerbalChange);
			GameEvents.OnCameraChange.Add (OnCameraChange);
		}

		void OnDestroy ()
		{
			GameEvents.OnIVACameraKerbalChange.Remove (OnIVACameraKerbalChange);
			GameEvents.OnCameraChange.Remove (OnCameraChange);
		}

		public void Update ()
		{
			if (Input.GetKeyDown (KeyCode.B) &&
				GameSettings.MODIFIER_KEY.GetKey() &&
			    CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.IVA &&
				CrewHatchController.fetch.CrewDialog == null)
			{
				CrewHatchController.fetch.SpawnCrewDialog (
					CameraManager.Instance.IVACameraActiveKerbal.InPart,
					false,
					true
				);
			}
		}

		private void OnCameraChange (CameraManager.CameraMode cameraMode)
		{
			if (cameraMode == CameraManager.CameraMode.IVA)
				ShowInfo ();
		}

		private void OnIVACameraKerbalChange (Kerbal kerbal)
		{
			ShowInfo ();
		}

		private void ShowInfo ()
		{
			Kerbal kerbinaut = CameraManager.Instance.IVACameraActiveKerbal;
			ScreenMessages.PostScreenMessage (
				String.Format (
					"{0} - {1} Level {2}\n({3})",
					kerbinaut.crewMemberName,
					kerbinaut.protoCrewMember.experienceTrait.TypeName,
					kerbinaut.protoCrewMember.experienceLevel,
					kerbinaut.InPart.partInfo.title
				)
			);
		}
	}
}
