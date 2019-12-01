using UnityEngine;

public class ColorReceiver : MonoBehaviour {
	
    Color color;

    public Color Color { get => color; set => color = value; }

    void OnColorChange(HSBColor color) 
	{
        this.Color = color.ToColor();
	}

    void OnGUI()
    {
		var r = Camera.main.pixelRect;
		var rect = new Rect(r.center.x + r.height / 6 + 50, r.center.y, 100, 100);
		GUI.Label (rect, "#" + ToHex(Color.r) + ToHex(Color.g) + ToHex(Color.b));	
    }

	string ToHex(float n)
	{
		return ((int)(n * 255)).ToString("X").PadLeft(2, '0');
	}
}
