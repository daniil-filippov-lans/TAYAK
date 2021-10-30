namespace DFA;

using System;
using System.Collections.Generic;

class Program
{
	static void Main(string[] args)
	{
		string[] textDFA;
		DFAState currentState = null;
		DFAState startState = null;

		textDFA = FileReader.Read();

		List<DFAState> automats = CreateDFA(textDFA);

		PrintDFA(automats);

		if (isDeterministic(automats))
		{
			Console.WriteLine("deterministic");
		}
		else
		{
			Console.WriteLine("nondeterministic");

			while (!isDeterministic(automats))
			{
				Determination(ref automats);
				PrintDFA(automats);
			}
		}

		foreach (string item in textDFA)
		{
			if (item[0].Equals(';'))
			{
				currentState = startState;
				DFARun(automats, item.Substring(1));
			}
		}

		Console.ReadKey();
	}

	public static List<DFAState> CreateDFA(string[] textDFA)
	{
		List<DFAState> automats = new List<DFAState>();
		automats.Add(new DFAState("f0", "f0", "f0"));

		foreach (string str in textDFA)
		{
			if (str.Equals("\r") || str.Equals(""))
			{
				break;
			}

			string[] splitStr = str.Split(',');
			string conditionNumber = splitStr[0];

			if (Search(automats, conditionNumber) != null)
			{
				Search(automats, conditionNumber).AddConversion(splitStr[1][0].ToString(), splitStr[1].Substring(2));
			}
			else
			{
				automats.Add(new DFAState(conditionNumber, splitStr[1][0].ToString(), splitStr[1].Substring(2)));
			}
		}

		return automats;
	}

	public static void DFARun(List<DFAState> automats, string checkString)
	{
		DFAState currentState = Search(automats, "q0");
		string conversionstr;

		foreach (char item in checkString)
		{
			conversionstr = currentState.Conversion(item.ToString());

			if (conversionstr != null)
			{
				currentState = Search(automats, conversionstr);
				if (currentState == null || currentState.statusNumber.Equals("f0"))
				{
					break;
				}
			}

			break;
		}

		if (currentState != null && currentState.statusNumber.Equals("f0"))
		{
			Console.WriteLine("automat solved");
		}
		else
		{
			Console.WriteLine("automat not solved");
		}
	}

	public static DFAState Search(List<DFAState> automats, string conditionNumber)
	{
		foreach (DFAState item in automats)
		{
			if (item.statusNumber.Equals(conditionNumber))
			{
				return item;
			}
		}

		return null;
	}

	public static bool isDeterministic(List<DFAState> automats)
	{
		foreach (DFAState item in automats)
		{
			string temporaryConversion = item.conversions[0].accessConversion;

			for (int i = 1; i < item.conversions.Count; i++)
			{
				if (temporaryConversion.Contains(item.conversions[i].accessConversion))
				{
					return false;
				}
				else
				{
					temporaryConversion = temporaryConversion + item.conversions[i].accessConversion;
				}
			}
		}

		return true;
	}

	public static void Determination(ref List<DFAState> automats)
	{
		List<DeterministicStruct> determinationStructs = new List<DeterministicStruct>();

		foreach (DFAState item in automats)
		{
			Dictionary<string, List<string>> conversions = new Dictionary<string, List<string>>();
			List<string> newconversions = new List<string>();

			newconversions.Add(item.conversions[0].newConversion);
			conversions.Add(item.conversions[0].accessConversion, newconversions);

			for (int i = 1; i < item.conversions.Count; i++)
			{
				if (conversions.ContainsKey(item.conversions[i].accessConversion))
				{
					List<string> temporaryconversions = conversions[item.conversions[i].accessConversion];
					temporaryconversions.Add(item.conversions[i].newConversion);
					conversions[item.conversions[i].accessConversion] = temporaryconversions;
				}
				else
				{
					List<string> temporaryconversions = new List<string>();
					temporaryconversions.Add(item.conversions[i].newConversion);
					conversions.Add(item.conversions[i].accessConversion, temporaryconversions);
				}
			}

			foreach (var element in conversions)
			{
				if (element.Value.Count > 1)
				{
					determinationStructs.Add(new DeterministicStruct(item.statusNumber, element.Key, element.Value));
				}
			}
		}

		foreach (DeterministicStruct item in determinationStructs)
		{
			string newOutState = "";

			for (int i = 0; i < item.inConversion.Count; i++)
			{
				newOutState += " " + item.inConversion[i];
			}

			DFAState temporaryDFA = Search(automats, item.outState);
			automats.Remove(temporaryDFA);
			List<Conversion> conversionsToDelete = new List<Conversion>();

			foreach (Conversion elem in temporaryDFA.conversions)
			{
				if (elem.accessConversion.Equals(item.conversion))
				{
					conversionsToDelete.Add(elem);
				}
			}

			foreach (Conversion elem in conversionsToDelete)
			{
				temporaryDFA.conversions.Remove(elem);
			}

			if (!temporaryDFA.conversions.Contains(new Conversion(item.conversion, newOutState.Replace(" ", ""))))
			{
				temporaryDFA.conversions.Add(new Conversion(item.conversion, newOutState.Replace(" ", "")));
			}

			automats.Add(temporaryDFA);

			string[] conditionsOfUnion = newOutState.Split(new string[] { " " }, StringSpliTOPtions.RemoveEmptyEntries);
			DFAState newDFAState = new DFAState(newOutState.Replace(" ", ""));

			int addedNewDFAState = 0;

			foreach (string elem in conditionsOfUnion)
			{
				DFAState conditionForconversions = Search(automats, elem);

				if (conditionForconversions != null)
				{
					foreach (Conversion conversion in conditionForconversions.conversions)
					{
						newDFAState.AddConversion(conversion.accessConversion, conversion.newConversion);
					}

					addedNewDFAState++;
				}
			}

			if (addedNewDFAState != 0)
			{
				automats.Add(newDFAState);
			}
		}
	}

	public static void PrintDFA(List<DFAState> automats)
	{
		foreach (DFAState item in automats)
		{
			Console.Write(item.statusNumber);

			foreach (Conversion elem in item.conversions)
			{
				Console.Write(" " + elem.accessConversion + " " + elem.newConversion);
			}

			Console.Write("\n");
		}
	}

	public struct DeterministicStruct
	{
		public string outState;
		public string conversion;
		public List<string> inConversion;
		public DeterministicStruct(string outState, string conversion, List<string> inConversion)
		{
			this.outState = outState;
			this.conversion = conversion;
			this.inConversion = inConversion;
		}
	}
}
