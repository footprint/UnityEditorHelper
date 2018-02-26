using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// 屏幕的宽高比
public enum AspectRatio {
	Unknown, //未知
	W2H3, //2:3: iphone4
	W3H4, //3:4: ipad
	W9H16, //9:16: iphone5
	IphoneX, //iphoneX: 9:19.5
};

[System.Serializable]
public struct AspectRatioLayout {
	private static AspectRatioLayout sNull = new AspectRatioLayout();

	public AspectRatio aspectRatio; // 屏幕的宽高比
	public Vector3 scale; // 该分辨率下的localScale
	public Vector3 position; // 该分辨率下的anchoredPosition

	public static AspectRatioLayout Null {
		get {
			return sNull;
		}
	}
};

[System.Serializable]
public struct Layout {
	private static Layout sNull = new Layout();

	public RectTransform transform;
	public AspectRatioLayout[] layouts; //layout适配

	public static Layout Null { 
		get {
			return sNull;
		}
	}

	public bool HasLayout(AspectRatio aspectRatio) {
		if (null != layouts) {
			for (int i = 0; i < layouts.Length; ++i) {
				if (layouts[i].aspectRatio == aspectRatio) {
					return true;
				}
			}
		}
		return false;
	}

	public AspectRatioLayout GetLayout(AspectRatio aspectRatio) {
		if (null != layouts) {
			for (int i = 0; i < layouts.Length; ++i) {
				if (layouts[i].aspectRatio == aspectRatio) {
					return layouts[i];
				}
			}
		}
		return AspectRatioLayout.Null;
	}

	public void SetLayout(AspectRatio aspectRatio, AspectRatioLayout layout) {
		if (null != layouts) {
			for (int i = 0; i < layouts.Length; ++i) {
				if (layouts[i].aspectRatio == aspectRatio) {
					layouts[i] = layout;
					return;
				}
			}
		}
	}
};

public class Layouts : MonoBehaviour {
	public Layout[] layouts;
	public bool modifyDesign = true;

	private bool inited = false;
	private AspectRatio aspectRatio = AspectRatio.Unknown;

	public static AspectRatio GetAspectRatio_() {
		Vector2 size = Utils.GetCanvasSize();
		float w = Mathf.Min(size.x, size.y);
		float h = Mathf.Max(size.x, size.y);
		float s = w / h;

		if (Mathf.Abs(s - 2.0f/3.0f) <= 0.01f) {
			return AspectRatio.W2H3;
		}else if (Mathf.Abs(s - 3.0f/4.0f) <= 0.01f) {
			return AspectRatio.W3H4;
		}else if (Mathf.Abs(s - 9.0f/16.0f) <= 0.01f) {
			return AspectRatio.W9H16;
		}else if (Mathf.Abs(s - 9.0f/19.5f) <= 0.01f) {
			return AspectRatio.IphoneX;
		}

		return AspectRatio.Unknown;
	}

	public static void SetDesignRes_(CanvasScaler scaler, AspectRatio ratio) {
		Vector2 size = Utils.GetCanvasSize();
		bool bh = size.x < size.y;

		if (AspectRatio.W2H3 == ratio) {
			scaler.referenceResolution = bh ? new Vector2(640, 960) : new Vector2(960, 640);
		}else if (AspectRatio.W3H4 == ratio) {
			scaler.referenceResolution = bh ? new Vector2(768, 1024) : new Vector2(1024, 768);
		}else if (AspectRatio.W9H16 == ratio) {
			scaler.referenceResolution = bh ? new Vector2(640, 1136) : new Vector2(1136, 640);
		}else if (AspectRatio.IphoneX == ratio) {
			scaler.referenceResolution = bh ? new Vector2(1125, 2436) : new Vector2(2436, 1125);
		}
	}

	//1 必须在 Canvas Scaler 之后执行（修改Script Execution Order > 0）;
	//2 其他修改ui位置和缩放的脚本请放Start里执行！
	void Awake() {
		if (modifyDesign) {
			AspectRatio ratio = GetAspectRatio();
			if (AspectRatio.Unknown != ratio) {
				CanvasScaler scaler = GetComponent<CanvasScaler>();
				if (null != scaler) {
					SetDesignRes_(scaler, ratio);
				}
			}
		}

		UpdateLayouts();
	}

	public void UpdateLayouts() {
		if (null != layouts) {
			AspectRatio ratio = GetAspectRatio();
			for (int i = 0; i < layouts.Length; ++i) {
				Layout layout = layouts[i];
				if (layout.HasLayout(ratio)) {
					AspectRatioLayout aspect = layout.GetLayout(ratio);
					if (!aspect.Equals(AspectRatioLayout.Null)) {
						layout.transform.localScale = aspect.scale;
						layout.transform.anchoredPosition = aspect.position;
					}
				}
			}
		}
	}

	public AspectRatio GetAspectRatio() {
		if (!inited) {
			inited = true;
			aspectRatio = GetAspectRatio_();
		}
		return aspectRatio;
	}

	public Layout GetLayout(RectTransform transform) {
		if (null != layouts) {
			for (int i = 0; i < layouts.Length; ++i) {
				if (layouts[i].transform.Equals(transform)) {
					return layouts[i];
				}
			}
		}
		
		return Layout.Null;
	}

	public bool NeedsLayout(RectTransform transform) {
		Layout layout = GetLayout(transform);
		if (!layout.Equals(Layout.Null)) {
			return layout.HasLayout(GetAspectRatio());
		}
		return false;
	}
}
