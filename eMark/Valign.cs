namespace eMark;

// Вертикальное выравнивание
public enum Valign
{
	TOP,
	CENTER,
	BOTTOM
}

public static class ValignConverter
{
	public static Valign? Convert(string valign)
	{
		switch (valign)
		{
			case "top":
				return Valign.TOP;
			case "center":
				return Valign.CENTER;
			case "bottom":
				return Valign.BOTTOM;
			default:
				return null;
		}
	}
}
