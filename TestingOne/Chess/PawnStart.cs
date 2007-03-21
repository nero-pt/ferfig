/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/// <summary>
	/// class to represent the pawn start move of two squares
	/// </summary>
	public class PawnStart : Move
	{
		internal PawnStart(Square begin,Square end,Side side):base(begin,end,side){}
	}
}