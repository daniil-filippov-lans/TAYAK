namespace eMark;

using System;

class Program
{
	static void Main(string[] args)
	{
		new EMarkProcessor(@"emark.txt").Render();
		Console.ReadLine();
	}
}
