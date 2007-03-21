/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System;

namespace Chess.Game
{
	/// <summary>
	/// class tyo represent a towers-move (involving king and rook)
	/// </summary>
	public class Towers : Move
	{
		internal Towers(Square begin,Square end,Side side):base(begin,end,side){}

		public override string ToString()
		{
			if (Math.Abs(Begin.Col-End.Col)==2)
				return "O-O";

			if (Math.Abs(Begin.Col-End.Col)==3)
				return "O-O-O";

			return "?";
		}
	}
}