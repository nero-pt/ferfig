/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System;

namespace Chess.Game
{
	//INTERESTING 80 custom exception definition
	public class ChessException : Exception
	{
		private int row,col;
		public ChessException(string s,int row,int col):base(s + " (" + row.ToString() + col.ToString() + ")")
		{
			this.row=row;
			this.col=col;
		}
		public ChessException(String s,Square square):base(s + " (" + square.Row.ToString() + square.Col.ToString() + ")")
		{
			row=square.Row;
			col=square.Col;
		}
		public ChessException(String s):base(s){}

		public int Row
		{
			get{return row;}
		}

		public int Col
		{
			get{return col;}
		}
	}
}