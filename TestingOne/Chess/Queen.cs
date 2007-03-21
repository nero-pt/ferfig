/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	//INTERESTING 05 subclass definition
	public class Queen : Piece
	{
		internal Queen(Side side):base(side){}
		
		public override Kind Type
		{
			get{return Piece.Kind.Queen;}
		}
		
		public override string ToString()
		{
			return "Q";
		}
	}
}
	

