namespace Interpreter;

using System;
using System.Collections.Generic;
using System.Linq;

public static class Interpreter
{
	private static Dictionary<string, string> Dictionary = new Dictionary<string, string>();
	private static string Alphabet => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_";
	public static List<string> KeyWords => new List<string>()
	{
		"print", "scan", "for", "if", "to", "else", "{", "}", "+", "-", "*", "/", "<", ">", "==", "!=", "=",
	};

	public static string? Start(List<string> code, ref int id)
	{
		Operator(code, ref id);

		return null;
	}

	public static string? Operator(List<string> code, ref int id, bool isLoop = false)
	{
		switch (code[id])
		{
			case "print":
				id += 1;
				Print(code, ref id);
				break;

			case "scan":
				id += 1;
				Scan(code, ref id);
				break;

			case "for":
				id += 1;
				For(code, ref id);
				break;

			case "if":
				id += 1;
				If(code, ref id);
				break;

			case "else":
				Else(code, ref id);
				break;

			default:
				if (IsIdentifier(code[id]))
				{
					Assign(code, ref id);
				}
				break;
		}

		if (id < code.Count && code[id] == "}")
		{
			return null;
		}

		if (id < code.Count - 1)
		{
			Operator(code, ref id);
		}

		return null;
	}

	public static string? Print(List<string> code, ref int id)
	{
		Console.WriteLine(PrintEnd(code, ref id));

		id += 1;

		return null;
	}

	public static string? Else(List<string> code, ref int id)
	{
		id += 1;

		if (id < code.Count && code[id] != "{")
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		id += 1;

		if (id < code.Count)
		{
			Operator(code, ref id, true);
		}

		if (id < code.Count && code[id] != "}")
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		return null;
	}

	public static string? If(List<string> code, ref int id)
	{
		var excpression = BoolExcpression(code, ref id);

		if (bool.Parse(excpression))
		{
			id += 1;

			if (id < code.Count && code[id] != "{")
			{
				throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
			}

			id += 1;

			if (id < code.Count)
			{
				Operator(code, ref id, true);
			}

			if (id < code.Count && code[id] != "}")
			{
				throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
			}

			if (id + 1 < code.Count && code[id + 1] == "else")
			{
				id += 2;

				if (id < code.Count && code[id] != "{")
				{
					throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
				}

				int count = 1;

				while (count != 0)
				{
					id += 1;

					if (code[id] == "{")
					{
						count++;
					}

					if (code[id] == "}")
					{
						count--;
					}
				}
			}
		}
		else
		{
			id += 1;

			if (id < code.Count && code[id] != "{")
			{
				throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
			}

			int count = 1;

			while (count != 0)
			{
				id += 1;

				if (code[id] == "{")
				{
					count++;
				}

				if (code[id] == "}")
				{
					count--;
				}
			}

			id += 1;

			if (id < code.Count && code[id] == "else")
			{
				Else(code, ref id);
			}
		}

		id += 1;

		return null;
	}

	public static string BoolExcpression(List<string> code, ref int id)
	{
		var LEFT = Expression(code, ref id);
		string loop = "";
		string RIGHT = "";

		id += 1;

		if (id < code.Count - 1)
		{
			loop = code[id];
		}

		id += 1;

		if (id < code.Count - 1)
		{
			RIGHT = Expression(code, ref id);
		}

		bool expression = false;

		switch (loop)
		{
			case "<":
				expression = int.Parse(LEFT) < int.Parse(RIGHT);
				break;

			case ">":
				expression = int.Parse(LEFT) > int.Parse(RIGHT);
				break;

			case "==":
				expression = int.Parse(LEFT) == int.Parse(RIGHT);
				break;

			case "!=":
				expression = int.Parse(LEFT) != int.Parse(RIGHT);
				break;
		}

		return expression.ToString();
	}

	public static string? For(List<string> code, ref int id)
	{
		string key1 = "", key2 = "";
		var key = code[id];

		id += 1;

		if (id < code.Count && code[id] != "=")
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		if (id + 1 < code.Count)
		{
			id += 1;
			key1 = Expression(code, ref id);
		}

		id += 1;

		if (id < code.Count && !(code[id] == "to"))
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		if (id + 1 < code.Count)
		{
			id += 1;
			key2 = Expression(code, ref id);
		}

		id += 1;

		if (id < code.Count && code[id] != "{")
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		int index = id + 1;

		if (index < code.Count)
		{
			for (int i = int.Parse(key1); i <= int.Parse(key2); i++)
			{
				id = index;

				if (Dictionary.ContainsKey(key))
				{
					Dictionary[key] = i.ToString();
				}
				else
				{
					Dictionary.Add(key, i.ToString());
				}

				Operator(code, ref id, true);
			}
		}

		if (id < code.Count && code[id] != "}")
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		id += 1;

		return null;
	}

	public static string? Assign(List<string> code, ref int id)
	{
		var key = code[id];

		id += 1;

		if (id < code.Count && code[id] != "=")
		{
			throw new Exception($"Wrong operation {code[id - 1]} {code[id]}");
		}

		if (id + 1 < code.Count)
		{
			id += 1;
			var val = Expression(code, ref id);

			if (Dictionary.ContainsKey(key))
			{
				Dictionary[key] = val;
			}
			else
			{
				Dictionary.Add(key, val);
			}
		}

		id += 1;

		return null;
	}

	public static string? Scan(List<string> code, ref int id)
	{
		var val = Console.ReadLine();
		var result = int.TryParse(val, out int _);

		if (result)
		{
			if (id < code.Count)
			{
				var key = code[id];

				if (Dictionary.ContainsKey(key))
				{
					Dictionary[key] = val!;
				}
				else
				{
					Dictionary.Add(key, val!);
				}
			}

			id += 1;
		}
		else
		{
			throw new Exception("Not number!");
		}

		return null;
	}

	public static string PrintEnd(List<string> code, ref int id)
	{
		string rtn;

		if (code[id].Start() == '\"')
		{
			if (code[id].Last() == '\"')
			{
				rtn = String(code, ref id);
			}
			else
			{
				throw new Exception($"Wrong string format {code[id]}");
			}
		}
		else
		{
			rtn = Expression(code, ref id);
		}

		if (id + 1 < code.Count && code[id + 1] == ",")
		{
			id += 2;
			rtn += PrintEnd(code, ref id);
		}

		return rtn;
	}

	public static string String(List<string> code, ref int id)
	{
		return code[id].Substring(1, code[id].Length - 2);
	}

	public static string Expression(List<string> code, ref int id)
	{
		var rtn = Term(code, ref id);

		if (id + 1 < code.Count && code[id + 1] == "+")
		{
			id += 2;
			rtn = (int.Parse(rtn) + int.Parse(Expression(code, ref id))).ToString();
		}

		if (id + 1 < code.Count && code[id + 1] == "-")
		{
			id += 2;
			rtn = (int.Parse(rtn) - int.Parse(Expression(code, ref id))).ToString();
		}

		return rtn;
	}

	public static string Term(List<string> code, ref int id)
	{
		var rtn = Factor(code, ref id);

		if (id + 1 < code.Count && code[id + 1] == "*")
		{
			id += 2;
			rtn = (int.Parse(rtn) * int.Parse(Term(code, ref id))).ToString();
		}

		if (id + 1 < code.Count && code[id + 1] == "/")
		{
			id += 2;
			rtn = (int.Parse(rtn) / int.Parse(Term(code, ref id))).ToString();
		}

		return rtn;
	}

	public static string Factor(List<string> code, ref int id)
	{
		if (char.IsDigit(code[id].Start()))
		{
			return Number(code, ref id);
		}

		if (code[id].Start() == '(')
		{
			if (code[id].Last() == ')')
			{
				return Expression(code, ref id);
			}
			else
			{
				throw new Exception($"Bad inner expression format {code[id]}");
			}
		}

		return Identifier(code, ref id);
	}

	public static string Identifier(List<string> code, ref int id)
	{
		if (IsIdentifier(code[id]))
		{
			if (KeyWords.Contains(code[id]))
			{
				throw new Exception($"Identifier cant be service word {code[id]}");
			}

			if (Dictionary.ContainsKey(code[id]))
			{
				return Dictionary[code[id]];
			}
			else
			{
				throw new Exception($"Not identified value {code[id]}");
			}
		}
		else
		{
			throw new Exception($"Wrong identifier format {code[id]}");
		}
	}

	public static string Number(List<string> code, ref int id)
	{
		if (int.TryParse(code[id], out _))
		{
			return code[id];
		}

		throw new Exception($"Wrong number format {code[id]}");
	}

	private static bool IsIdentifier(string str)
	{
		var check = true;

		foreach (var c in str)
		{
			check &= Alphabet.Contains(c);
		}

		return check;
	}

}
