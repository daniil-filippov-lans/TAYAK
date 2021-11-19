namespace eMark;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class Content : Element
{
	public string Text { get; private set; }

	public Content(Element parent, XmlText xml) : base(parent, null)
	{
		Text = xml.Value;

		while (Text.Contains('\n'))
		{
			Text = Text.Remove(Text.IndexOf('\n'), 1);
		}

		while (Text.Contains('\r'))
		{
			Text = Text.Remove(Text.IndexOf('\r'), 1);
		}

		while (Text.Contains('\t'))
		{
			Text = Text.Remove(Text.IndexOf('\t'), 1);
		}

		while (Text.Contains(' '))
		{
			Text = Text.Remove(Text.IndexOf(' '), 1);
		}
	}

	public override ConsoleText[][] GetText()
	{
		var text = new ConsoleText[Height ?? 0][];
		int k = 0;

		for (int i = 0; i < text.Length; i++)
		{
			text[i] = new ConsoleText[Width ?? 0];

			for (int j = 0; j < text[i].Length; j++)
			{
				if (Text.Length > k)
				{
					text[i][j] = new ConsoleText { Sym = Text[k], BgColor = this.BgColor ?? 0, TextColor = this.TextColor ?? 0 };
				}
				else
				{
					text[i][j] = new ConsoleText { Sym = ' ', BgColor = this.BgColor ?? 0, TextColor = this.TextColor ?? 0 };
				}

				k++;
			}
		}

		for (int i = 0; i < text.Length; i++)
		{
			int len = 0;

			switch (Halign)
			{
				case eMark.Halign.RIGHT:
					len = text[i].Length - Array.IndexOf(text[i].Select(_ => _.Sym).ToArray(), ' ');
					break;

				case eMark.Halign.CENTER:
					len = (text[i].Length - Array.IndexOf(text[i].Select(_ => _.Sym).ToArray(), ' ')) / 2;
					break;

				case eMark.Halign.LEFT:
					len = 0;
					break;
			}

			while (len != 0)
			{
				var tmp = text[i].Last();

				for (int j = text[i].Length - 1; j > 0; j--)
				{
					text[i][j] = text[i][j - 1];
				}

				text[i][0] = tmp;
				len--;
			}
		}

		int hgn = 0;

		for (int i = 0; i < text.Length; i++)
		{
			hgn += text[i][0].Sym == ' ' ? 1 : 0;
		}

		switch (Valign)
		{
			case eMark.Valign.BOTTOM:
				hgn = hgn - 1;
				break;

			case eMark.Valign.CENTER:
				hgn = hgn / 2;
				break;

			case eMark.Valign.TOP:
				hgn = 0;
				break;
		}

		if (hgn != 0)
		{
			for (int i = 0; i < text[0].Length; i++)
			{
				int heightTemp = hgn;

				while (heightTemp != 0)
				{
					var tmp = text[text.Length - 1][i];
					for (int j = text.Length - 1; j > 0; j--)
					{
						text[j][i] = text[j - 1][i];
					}
					text[0][i] = tmp;
					heightTemp--;
				}
			}
		}

		return text;
	}
}
