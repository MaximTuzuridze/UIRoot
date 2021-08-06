using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Prototype.AudioCore;

namespace Prototype
{
	public static class Utils
	{
#if UNITY_ANDROID || UNITY_IPHONE
        public static T GetAsset2<T>() where T : Object
        {
            return Resources.Load<T>("Databases/" + typeof(T).Name);
        }
#endif

#if UNITY_EDITOR
        public static T GetAsset<T>() where T : Object
        {
            var assets = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            if (assets.Length > 0)
            {
                return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(assets[0]), typeof(T));
            }

            return default(T);
        }

        public static T[] GetAssets<T>() where T : Object
        {
            var assetsPath = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            if (assetsPath.Length > 0)
            {
                var assets = new T[assetsPath.Length];

                for (var i = 0; i < assets.Length; i++)
                {
                    assets[i] = (T)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(assetsPath[i]), typeof(T));
                }

                return assets;
            }

            return null;
        }

        public static bool HasAsset<T>() where T : Object
        {
            var assets = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
            return assets.Length > 0;
        }

        public static T CreateAsset<T>(System.Type type, string path, bool refresh = false) where T : ScriptableObject
        {
            var scriptableObject = (T)ScriptableObject.CreateInstance(type);

            var itemPath = path + ".asset";

            UnityEditor.AssetDatabase.CreateAsset(scriptableObject, itemPath);

            UnityEditor.AssetDatabase.SaveAssets();

            if (refresh)
                UnityEditor.AssetDatabase.Refresh();

            return scriptableObject;
        }

        public static T CreateAsset<T>(string path, bool refresh = false) where T : ScriptableObject
        {
            T scriptableObject = (T)ScriptableObject.CreateInstance(typeof(T));

            var itemPath = path + ".asset";

            UnityEditor.AssetDatabase.CreateAsset(scriptableObject, itemPath);

            UnityEditor.AssetDatabase.SaveAssets();

            if (refresh)
                UnityEditor.AssetDatabase.Refresh();

            return scriptableObject;
        }
#endif

        public static bool IsMobilePlatform ()
		{
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA
			return true;
#else
			return false;
#endif
		}


		public static Languages.Language GetSystemLanguage ()
		{
			var res = Languages.Language.English;
#if UNITY_ANDROID
			res = (Languages.Language) System.Enum.Parse ( typeof ( Languages.Language ), GetAndroidDisplayLanguage ( ), true );
#else
			res = (Languages.Language) System.Enum.Parse ( typeof ( Languages.Language ), Application.systemLanguage.ToString ( ), true );
#endif
			return res;

		}


		private static readonly List<string>
			MuslimLocales = new List<string> ( )
			{
			"Arabic",
			"Albanian",
			"Bengali",
			"Indonesian",
			"Kyrgyz",
			"Kazakh",
			"Sinhala",
			"Turkish",
			};

		public static bool IsMuslim ()
		{
			return MuslimLocales.IndexOf ( GetAndroidDisplayLanguage ( ) ) >= 0;
		}

		public static string GetAndroidDisplayLanguage ()
		{
#if UNITY_EDITOR
			return Application.systemLanguage.ToString ( );
#elif UNITY_ANDROID
        AndroidJavaClass localeClass = new AndroidJavaClass ( "java/util/Locale" );
        AndroidJavaObject defaultLocale = localeClass.CallStatic<AndroidJavaObject> ( "getDefault" );
        AndroidJavaObject usLocale = localeClass.GetStatic<AndroidJavaObject> ( "US" );
        string systemLanguage = defaultLocale.Call<string> ( "getDisplayLanguage", usLocale );
        Debug.Log ( "Android language is " + systemLanguage + " detected as " + systemLanguage );
        return systemLanguage;
#else
        return "";
#endif
		}

		public static string ConvertToString (this string [] arr)
		{
			var res = "Length = " + arr.Length + "\n";
			for (var i = 0; i < arr.Length; i++)
			{
				res += i + ":\t" + arr [i] + "\n";
			}
			return res;
		}

		public static string ConvertToString<T> (this T [] arr)
		{
			var res = "";
			for (var i = 0; i < arr.Length; i++)
			{
				res += i + ":\t" + arr [i].ToString ( ) + "\n";
			}
			return res;
		}

		public static string ConvertToString<T> (this Dictionary<T, string> dic)
		{
			var res = "count = " + dic.Count + "\n";

			foreach (var item in dic)
			{
				res += item.Key.ToString ( ) + ":\t\t" + item.Value.ToString ( ) + "\n";
			}
			return res;
		}

		public static Vector2 ConvertPoint (Vector2 point, Camera Cam)
		{
			return Cam.ScreenToWorldPoint ( Camera.main.WorldToScreenPoint ( point ) );
		}


		public static T ToEnum<T> (this string value)
		{
			return (T) System.Enum.Parse ( typeof ( T ), value, true );
		}


		public static T GetRandomEnum<T> ()
		{
			var a = System.Enum.GetValues ( typeof ( T ) );
			var v = (T) a.GetValue ( UnityEngine.Random.Range ( 0, a.Length ) );
			return v;
		}

		public static bool RandomBool ()
		{
			return Random.value > 0.5f;
		}

		public static List<T> Shuffle<T> (this List<T> list)
		{
			var n = list.Count;
			var newList = list.CopyList ( );
			while (n > 0)
			{
				var k = Random.Range ( 0, n );
				var item = newList [n - 1];
				newList [n - 1] = newList [k];
				newList [k] = item;
				n--;
			}
			return newList;
		}

		public static T [] Shuffle<T> (this T [] array)
		{
			var n = array.Length;
			var newArray = array.CopyArray ( );
			while (n > 0)
			{
				var k = Random.Range ( 0, n );
				var item = newArray [n - 1];
				newArray [n - 1] = newArray [k];
				newArray [k] = item;
				n--;
			}
			return newArray;
		}

		public static List<T> CopyList<T> (this List<T> list)
		{
			var n = list.Count;
			var newList = new List<T> ( );
			;
			for (var i = 0; i < n; i++)
			{
				newList.Add ( list [i] );
			}
			return newList;
		}

		public static T [] CopyArray<T> (this T [] array)
		{
			var n = array.Length;
			var newArray = new T [n];
			for (var i = 0; i < n; i++)
			{
				newArray [i] = array [i];
			}
			return newArray;
		}

		public static T GetRandomElement<T> (this List<T> list)
		{
			return list [Random.Range ( 0, list.Count )];
		}


		public static T GetRandomElement<T> (this T [] array)
		{
			return array [Random.Range ( 0, array.Length )];
		}

		public static List<T> GetRandomElements<T>(this List<T> list, int count)
        {
            var n = list.Count;
            var newList = new List<T>(count);
            while (count > 0)
            {
                var k = Random.Range(0, n);
                var item = newList[n - 1];
                newList[n - 1] = newList[k];
                newList[k] = item;
                n--;
                count--;
            }

            return newList;
        }

		public static T[] GetRandomElements<T>(this T[] array, int count)
        {
            var temp = new List<int>();
            for (var i = 0; i < array.Length; i++)
                temp.Add(i);

            var newArray = new T[count];
            int random;
            for (var i = 0; i < count; i++)
            {
                random = Random.Range(0, temp.Count);
                newArray[i] = array[temp[random]];
                temp.RemoveAt(random);
            }

            return newArray;
        }

		public static string GetUID ()
		{
			var UID = System.Guid.NewGuid ( ).ToString ( );
			if (PlayerPrefs.HasKey ( "UID" ))
			{
				UID = PlayerPrefs.GetString ( "UID" );
			}
			else
			{
				PlayerPrefs.SetString ( "UID", UID );
				PlayerPrefs.Save ( );
			}
			return UID;
		}

        public static void ChangeColor(Color color)
        {
            GUI.color = color;
        }

        public static void CheckColor(float field = -10, float defaultValue = 0)
        {
            if (field == -10)
            {
                ChangeColor(Color.green);
            }
            else
            {
                if (field == defaultValue)
                {
                    ChangeColor(Color.yellow);
                }
                else
                {
                    ChangeColor(Color.green);
                }
            }

        }

        public static void CheckColor(bool value, bool value2)
        {
            if (value == value2)
            {
                ChangeColor(Color.green);
            }
            else
            {
                ChangeColor(Color.yellow);
            }
        }

        public static void StopTween(Tween tween)
        {
            if (tween != null)
            {
                tween.Kill();

                tween = null;
            }
        }
    }

	public static class WeightedRandom
	{
		public static float [] CalcLookups (float [] weights)
		{
			float totalWeight = 0;
			for (var i = 0; i < weights.Length; i++)
			{
				totalWeight += weights [i];
			}
			var lookups = new float [weights.Length];
			for (var i = 0; i < weights.Length; i++)
			{
				lookups [i] = (weights [i] / totalWeight) + (i == 0 ? 0 : lookups [i - 1]);
			}
			return lookups;
		}

		private static int binary_search (float needle, float [] lookups)
		{
			var high = lookups.Length - 1;
			var low = 0;
			var probe = 0;
			if (lookups.Length < 2)
			{
				return 0;
			}
			while (low < high)
			{
				probe = (int) ((high + low) / 2);

				if (lookups [probe] < needle)
				{
					low = probe + 1;
				}
				else if (lookups [probe] > needle)
				{
					high = probe - 1;
				}
				else
				{
					return probe;
				}
			}

			if (low != high)
			{
				return probe;
			}
			else
			{
				return (lookups [low] >= needle) ? low : low + 1;
			}
		}


		public static int RandomW (float [] weights)
		{
			var lookups = CalcLookups ( weights );

			return PrecalculatedRandomW ( lookups );
		}


		public static int PrecalculatedRandomW (float [] lookups)
		{
			if (lookups.Length > 0)
				return binary_search ( Random.value, lookups );
			else
				return -1;
		}
    }
}