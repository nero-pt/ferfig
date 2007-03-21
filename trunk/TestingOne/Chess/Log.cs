/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

#define DEBUG
using System.Diagnostics;

public class Logger
{
	private static void DisplayMessage(string s)
	{
		System.Console.WriteLine(s);
	}
	
	// INTERESTING 18 overloading function names
	public static void Log(string s)
	{
		DisplayMessage(s);
	}
	
	public static void Log(int i)
	{
		DisplayMessage(i.ToString());	
	}
	
	public static void Log(int i,int j)
	{
		DisplayMessage( i.ToString() + " " + j.ToString());	
	}
	
	//INTERESTING 22 use of ToString
	public static void log(object p)
	{
		if (p==null)
			DisplayMessage("null");
		else
			DisplayMessage(p.ToString());	
	}
	
	//INTERESTING 99 debug attribute.calls are deleted
	[Conditional("DEBUG")]
	public static void Assert(string s)
	{
		DisplayMessage("ASSERT:  " + s);
	}
}
