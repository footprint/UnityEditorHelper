using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public class LayoutEditor {

	[MenuItem("Edit/LayoutEditor/Update Layouts")]
	public static void UpdateLayouts() {
		GameObject activeObject = Selection.activeObject as GameObject;
		if (null == activeObject) {
			UnityEngine.Debug.LogError("Please select Layouts GameObject!");
			return;
		}
		Layouts layouts = activeObject.GetComponent<Layouts>();
		if (null == layouts) {
			UnityEngine.Debug.LogError("Please select the GameObject whitch Layouts attached!");
			return;
		}

		AspectRatio ratio = Layouts.GetAspectRatio_();
		if (ratio == AspectRatio.Unknown) {
			UnityEngine.Debug.LogError("AspectRatio.Unknow: Not Update!");
			return;
		}

		for (int i = 0; i < layouts.layouts.Length; ++i) {
			Layout layout = layouts.layouts[i];
			RectTransform trans = layout.transform;
			AspectRatioLayout aspectLayout = layout.GetLayout(ratio);
			if (!aspectLayout.Equals(AspectRatioLayout.Null)) {
				aspectLayout.scale = trans.localScale;
				aspectLayout.position = trans.anchoredPosition;
				layout.SetLayout(ratio, aspectLayout);
			}
		}

		Debug.Log("Update Layouts Success!");
	}

	[MenuItem("Edit/LayoutEditor/Restore Scene")]
	public static void RestoreScene() {
		GameObject activeObject = Selection.activeObject as GameObject;
		if (null == activeObject) {
			UnityEngine.Debug.LogError("Please select Layouts GameObject!");
			return;
		}
		Layouts layouts = activeObject.GetComponent<Layouts>();
		if (null == layouts) {
			UnityEngine.Debug.LogError("Please select the GameObject whitch Layouts attached!");
			return;
		}

		AspectRatio ratio = Layouts.GetAspectRatio_();
		if (ratio == AspectRatio.Unknown) {
			UnityEngine.Debug.LogError("AspectRatio.Unknow: Not Update!");
			return;
		}

		if (layouts.modifyDesign) {
			CanvasScaler scaler = layouts.GetComponent<CanvasScaler>();
			if (null != scaler) {
				Layouts.SetDesignRes_(scaler, ratio);
			}else {
				Debug.LogError("CanvasScaler is Null!");
			}
		}

		for (int i = 0; i < layouts.layouts.Length; ++i) {
			Layout layout = layouts.layouts[i];
			RectTransform trans = layout.transform;
			AspectRatioLayout aspectLayout = layout.GetLayout(ratio);
			
			if (!aspectLayout.Equals(AspectRatioLayout.Null)) {
				trans.localScale = aspectLayout.scale;
				trans.anchoredPosition = aspectLayout.position;
			}else {
				Debug.LogError("AspectRatioLayout.Null");
			}
		}

		Debug.Log("Restore Scene Success!");
	}
	
}
