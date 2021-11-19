namespace eMark;

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

class EMarkProcessor
{
	private XmlDocument _xDoc;
	private Block _block;

	public List<string> Exceptions { get; }

	public EMarkProcessor(string filename)
	{
		Exceptions = new List<string>();

		try
		{
			_xDoc = new XmlDocument();
			_xDoc.Load(filename);
			if (_xDoc.DocumentElement.Name != "block")
			{
				Exceptions.Add("File should start with <block>");
			}
			else
			{
				_block = new Block(null, _xDoc.DocumentElement);
			}
		}
		catch (XmlException exception)
		{
			Exceptions.Add(exception.Message);
		}
		catch (Exception exception)
		{
			Exceptions.Add(exception.Message);
		}
	}

	public void Render()
	{
		if (Exceptions.Count != 0)
		{
			foreach (var exception in Exceptions)
			{
				Console.WriteLine(exception);
			}
			return;
		}
		try
		{
			var text = _block.GetText();
			for (int i = 0; i < text.Length; i++)
			{
				for (int j = 0; j < text[i].Length; j++)
				{
					Console.ForegroundColor = (ConsoleColor)text[i][j].TextColor;
					Console.BackgroundColor = (ConsoleColor)text[i][j].BgColor;
					Console.Write(text[i][j].Sym);
				}
				Console.WriteLine();
			}
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception.Message);
		}
		finally
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}
