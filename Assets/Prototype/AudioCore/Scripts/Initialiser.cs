using UnityEngine;
using Prototype.AudioCore;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Prototype
{
	public class Initialiser : MonoBehaviour
	{
		[FormerlySerializedAs("dont_sleep")] public bool dontSleep;

		private void Awake ()
		{
			if (dontSleep)
			{
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
			}

            FeedbackStorage.UpdateSettings ( );

            AudioController.InitStreams(gameObject);

			Languages.Init ( );
            
            DG.Tweening.DOTween.Init ( false, true, DG.Tweening.LogBehaviour.Verbose ).SetCapacity ( 200, 10 );

#if UNITY_STANDALONE
			StartCoroutine ( ResolutionChecker ( ) );
#endif
		}

		[ContextMenu ( "ClearPlayerPrefs" )]
		private void ClearPlayerPrefs ()
		{
			PlayerPrefs.DeleteAll ( );
			Debug.Log ( "Warning: player prefs cleared" );
		}

#if UNITY_STANDALONE
		public enum Aspects
		{
			_9x16,
			_3x4,
			_16x9,
			_4x3,
		}

		public Aspects
			current_aspect = Aspects._16x9;

		private int
			saved_height = 0,
			saved_width = 0;

		private Dictionary<Aspects, float>
			aspects = new Dictionary<Aspects, float> ( )
			{
				{ Aspects._9x16, 9f / 16f },
				{ Aspects._3x4, 3f / 4f },
				{ Aspects._16x9, 16f / 9f },
				{ Aspects._4x3, 4f / 3f },
			};
		private static bool
			portrait = false;

		private void CheckResolution ()
		{
			if ((current_aspect == Aspects._3x4 || current_aspect == Aspects._9x16) && (Screen.height != saved_height || Screen.width != saved_width))
			{
				saved_height = Screen.height;
				saved_width = (int) ((float) saved_height * aspects[current_aspect]);
				Debug.Log ( "CheckResolution " + saved_width + "x" + saved_height );
				Screen.SetResolution ( saved_width, saved_height, false );
			}
		}

		private IEnumerator ResolutionChecker ()
		{
			while (true)
			{
				CheckResolution ( );
				yield return new WaitForSeconds ( 1 );
			}
		}
#endif
	}
}
