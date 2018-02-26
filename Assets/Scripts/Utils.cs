using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Text;

public class Utils {
	public static void Log(object message) {
		#if UNITY_EDITOR
			Debug.Log(message);
		#endif
	}

	public static bool IsIpad() {
		#if UNITY_IOS
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad1Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad2Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad3Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad4Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad5Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadAir2 ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini1Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini2Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini3Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadMini4Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadPro1Gen ||
			    UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPadUnknown) {
				return true;
			}
		}
		#endif
		return false;
	}

	public static Vector2 GetCanvasSize() {
		GameObject canvas = GameObject.Find("Canvas");
		if (null != canvas) {
			RectTransform trans = canvas.GetComponent<RectTransform>();
			if (null != trans) {
				return trans.sizeDelta;
			}
		}
		return Vector2.zero;
	}

	public static float GetCanvasScaleX() {
		Vector2 size = Utils.GetCanvasSize();
		return size.x / Screen.width;
	}

	public static float GetCanvasScaleY() {
		Vector2 size = Utils.GetCanvasSize();
		return size.y / Screen.height;
	}

	public static float GetUIScaleFactor() {
		GameObject canvas = GameObject.Find("Canvas");
		if (null != canvas) {
			CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
			if (null != scaler) {
				if (CanvasScaler.ScreenMatchMode.MatchWidthOrHeight == scaler.screenMatchMode) {
					return scaler.referenceResolution.x / Screen.width;
				}
			}
		}
		return 1.0f;
	}

	public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float t2 = t * t;
		float t3 = t * t * t;
		float u = 1.0f - t;
		float u2 = u * u;
		float u3 = u * u * u;

		Vector3 p = u3 * p0; //first term
		p += 3 * u2 * t * p1; //second term
		p += 3 * u * t2 * p2; //third term
		p += t3 * p3; //fourth term
		
		return p;
	}

	public static DateTime ConvertFromUnixTimestamp(double timestamp) {
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return origin.AddSeconds(timestamp);
	}
	
	public static double ConvertToUnixTimestamp(DateTime date) {
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		TimeSpan diff = date.ToUniversalTime() - origin;
		return Math.Floor(diff.TotalSeconds);
	}

	public static DateTime ConvertFromLocalTimestamp(double timestamp) {
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
		return origin.AddSeconds(timestamp);
	}
	
	public static double ConvertToLocalTimestamp(DateTime date) {
		DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
		TimeSpan diff = date.ToLocalTime() - origin;
		return Math.Floor(diff.TotalSeconds);
	}

	public static bool IsPastDays(double timestamp) {
		DateTime now = DateTime.Now;
		DateTime past = ConvertFromLocalTimestamp(timestamp);

//		Debug.Log("now:"+now);
//		Debug.Log("past:"+past);

		return (now.Year > past.Year || now.Month > past.Month || now.Day > past.Day);
	}

	public static void SetAlpha(Transform trans, float opacity) {
		CanvasRenderer render = trans.GetComponent<CanvasRenderer>();
		if (null != render) {
			render.SetAlpha(opacity);
		}
		for (int i = 0; i < trans.childCount; ++i) {
			Transform child = trans.GetChild(i);
			SetAlpha(child, opacity);
		}
	}

	public static void CrossFadeAlpha(Transform trans, float opacity, float duration) {
		Graphic graphic = trans.GetComponent<Graphic>();
		if (null != graphic) {
			graphic.CrossFadeAlpha(opacity, duration, false);
		}
		for (int i = 0; i < trans.childCount; ++i) {
			Transform child = trans.GetChild(i);
			CrossFadeAlpha(child, opacity, duration);
		}
	}

	public static string xor (string msg, string key)
	{
		char[] msgArray = msg.ToCharArray();
		char[] keyArray = key.ToCharArray();

		for (int i = 0; i < msgArray.Length; i++)
		{
			msgArray[i] ^= keyArray[i % keyArray.Length];
		}

		return new string(msgArray);
	}
	//2d
	public static float SqrDistance2(Vector2 p1, Vector2 p2) {
		float x = p1.x - p2.x;
		float y = p1.y - p2.y;
		return (x * x + y * y);
	}
	public static float SqrDistance2(Vector2 p1, Vector3 p2) {
		float x = p1.x - p2.x;
		float y = p1.y - p2.y;
		return (x * x + y * y);
	}
	public static float SqrDistance2(Vector3 p1, Vector2 p2) {
		float x = p1.x - p2.x;
		float y = p1.y - p2.y;
		return (x * x + y * y);
	}
	
	#region array helper
	public static bool ArrayContains<T>(T[] array, T element) {
		return -1 != ArrayIndexOf<T>(array, element);
	}

	public static int ArrayIndexOf<T>(T[] array, T element) {
		if (null != array) {
			for (int i = 0; i < array.Length; ++i) {
				if (array[i].Equals(element)) {
					return i;
				}
			}
		}
		return -1;
	}

	public static void ArrayAdd<T>(ref T[] array, T element) {
		if (null == array) {
			array = new T[]{element};
		}else {
			T[] news = new T[array.Length + 1];
			CopyArray(array, news);
			news[array.Length] = element;
			array = news;
		}
	}

	public static void ArrayInsert<T>(ref T[] array, T element, int index) {
		if (null == array || index == array.Length) {
			ArrayAdd<T>(ref array, element);
		}else if (index >= 0) {
			for (int i = array.Length - 1; i > index; --i) {
				array[i] = array[i - 1];
			}
			array[index] = element;
		}
	}

	public static T[] SubArray<T>(T[] array, int from, int to) {
		T[] temp = new T[to - from + 1];
		for (int i = 0; i < temp.Length; ++i) {
			temp[i] = array[from + i];
		}
		return temp;
	}

	public static void CopyArray<T>(T[] from, T[] to, int begin = 0) {
		CopyArray<T>(from, to, begin, from.Length);
	}

	public static void CopyArray<T>(T[] from, T[] to, int begin, int len) {
		if (to.Length >= (len + begin)) {
			for (int i = 0; i < len; ++i) {
				to[begin + i] = from[i];
			}
		}
	}

	public static T[] MergeArray<T>(T[] array1, int len1, T[] array2, int len2) {
		T[] temp = new T[len1 + len2];

		for (int i = 0; i < len1; ++i) {
			temp[i] = array1[i];
		}
		for (int i = 0; i < len2; ++i) {
			temp[len1 + i] = array2[i];
		}
		return temp;
	}

	#endregion

	public static int RandomIndex(int[] prs) {
		int max = 0;
		for (int i = 0; i < prs.Length; ++i) {
			max += prs[i];
		}
		int rand = UnityEngine.Random.Range(0, max + 1);
		int ret = 0;
		for (int i = 0; i < prs.Length; ++i) {
			if (prs[i] > 0) { //skip 0
				ret += prs[i];
				if (rand <= ret) {
					return i;
				}
			}
		}
		return 0; //should never reach!
	}
}
