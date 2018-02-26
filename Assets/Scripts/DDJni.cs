using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class DDJni {
	public static bool IsIphoneX() {
		#if UNITY_IOS && !UNITY_EDITOR
		return DDJni_IOS.IsIphoneX();
		#elif UNITY_EDITOR && !UNITY_ANDROID
		//Portrait
		int xWidth = 1125;
		int xHeight = 2436;
		float f1 = xWidth * 1.0f / xHeight;
		float f2 = Screen.width * 1.0f / Screen.height;
		return Mathf.Abs(f1 - f2) < 0.1f;
		#else
		return false;
		#endif
	}

	//注意返回的 Rect 是屏幕坐标！ 
	public static Rect GetSafetyArea(bool scaled = true) {
		#if UNITY_IOS && !UNITY_EDITOR
		string json = DDJni_IOS.GetSafetyArea(scaled);
		if (null != json && json.Length > 2) {
			JsonData data = JsonMapper.ToObject(json);
			if (null != data) {
				float scale = Utils.GetCanvasScaleY();
				
				float x = JsonHelper.GetFloat(data, "x") * scale;
				float y = JsonHelper.GetFloat(data, "y") * scale;
				float w = JsonHelper.GetFloat(data, "w") * scale;
				float h = JsonHelper.GetFloat(data, "h") * scale;
				
				return new Rect(x, y, w, h);
			}
		}
		return new Rect(0, 0, 0, 0);
		#elif UNITY_EDITOR && !UNITY_ANDROID
		//Portrait
		int xHeight = 2436;
		Vector2 size = Utils.GetCanvasSize();
		float factor = size.y / xHeight;
		float scale = (scaled ? 3.0f : 1.0f) * factor;
		int top = (int)(44 * scale);
		int bottom = (int)(34 * scale);
		int height = (int)size.y - top - bottom;
		return new Rect(0, top, size.x, height);
		#else
		return new Rect(0, 0, 0, 0);
		#endif
	}
}
