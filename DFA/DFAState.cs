namespace DFA;

using System.Collections.Generic;

public class DFAState
{
	public string statusNumber;
	public List<Conversion> conversions;

	public DFAState(string statusNumber, string accessConversion, string newConversion)
	{
		conversions = new List<Conversion>();
		this.statusNumber = statusNumber;
		string[] temporaryNewConversion = newConversion.Split('\r');
		conversions.Add(new Conversion(accessConversion, temporaryNewConversion[0]));
	}

	public DFAState(string statusNumber)
	{
		this.statusNumber = statusNumber;
		conversions = new List<Conversion>();
	}

	public void AddConversion(string accessConversion, string newConversion)
	{
		string[] temporaryNewConversion = newConversion.Split('\r');
		conversions.Add(new Conversion(accessConversion, temporaryNewConversion[0]));
	}

	public string Conversion(string symbol)
	{
		foreach (Conversion item in conversions)
		{
			if (item.accessConversion.Equals(symbol))
			{
				return item.newConversion;
			}
		}

		return null;
	}
}

public struct Conversion
{
	public string accessConversion;
	public string newConversion;
	public Conversion(string accessConversion, string newConversion)
	{
		this.accessConversion = accessConversion;
		this.newConversion = newConversion;
	}
}
