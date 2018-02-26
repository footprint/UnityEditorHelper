using UnityEngine;
using System.Collections;

public class FixIphoneX : MonoBehaviour {
	public bool top = false;

	void Start () {
		if (DDJni.IsIphoneX()) {
			RectTransform trans = GetComponent<RectTransform>();
			
			if (null != trans) {
				Rect area = DDJni.GetSafetyArea();

				if (top) {
					int y = (int)area.yMin;
					trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, 
					trans.anchoredPosition.y - y);
				}else {
					int y = (int)(Utils.GetCanvasSize().y - area.yMax);
					trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, 
					trans.anchoredPosition.y + y);
				}				
			}
		}
	}
}
