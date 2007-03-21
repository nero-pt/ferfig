/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/**Class to indicate an enpassant move
	* @author Chris Meijers*/
	public class EnPassant : Move
	{
		internal EnPassant(Square begin,Square end,Side side):base(begin,end,side) {}
		
		public override string ToString()
		{
			return base.ToString() + "e.p";
		}
		
		/*set the pawn that waas captured by the move
		* 'overrides' the capturedpiece of the constructor*/
		internal void SetEnPassantPiece(Pawn p)
		{
			capturedpiece=p;
		}
	}
}