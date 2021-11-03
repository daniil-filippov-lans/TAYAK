namespace Interpreter;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
	static void Main(string[] args)
	{
		var inputString = File.ReadAllText("../../../code.txt");

		inputString = String.Concat(
			inputString
				.Replace("\t", " ")
				.Replace("\r", " ")
				.Replace("\n", " "),
			" ");

		while (inputString.Contains("  "))
		{
			inputString = inputString.Replace("  ", " ");
		}

		var code = new List<string>();
		int i = 0;
		string str = "";

		while (true)
		{
			if (i >= inputString.Length)
			{
				break;
			}

			if (str == "")
			{
				str += inputString[i];
				i++;
				continue;
			}

			if (str.Start() != '\"')
			{
				while (inputString[i] != ' ')
				{
					str += inputString[i];
					i++;
				}

				code.Add(str);
				str = "";
				i++;
			}
			else
			{
				while (inputString[i] != '\"')
				{
					str += inputString[i];
					i++;
				}

				str += inputString[i];
				i++;

				code.Add(str);
				str = "";
				i++;
			}
		}

		int id = 0;

		try
		{
			Interpreter.Start(code, ref id);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
}
