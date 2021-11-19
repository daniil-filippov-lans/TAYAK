namespace eMark;

// Горизонтальное выравнивание
public enum Halign
{
	LEFT,
	CENTER,
	RIGHT
}
public static class HalignConverter
{
	public static Halign? Convert(string halign)
	{
		switch (halign)
		{
			case "left":
				return Halign.LEFT;
			case "center":
				return Halign.CENTER;
			case "right":
				return Halign.RIGHT;
			default:
				return null;
		}
	}
}
