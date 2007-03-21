/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	public class Pawn:Piece
	{
		internal Pawn(Side side):base(side) {}
				
		public override Kind Type
		{
			get{return Piece.Kind.Pawn;}
		}
		
		public override string ToString()
		{
			return "p";
		}
	}
}