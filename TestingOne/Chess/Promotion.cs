/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/// <summary>
	/// class to represent a promotion-move
	/// </summary>
	public class Promotion : Move
	{
		private Piece promotionpiece;
		
		internal Promotion(Square begin,Square end,Side side):base(begin,end,side){}
			
		/// <summary>
		/// set the piece the pawn will be promoted to
		/// </summary>
		/// <param name="piece"> </param>
		internal void SetPromotionPiece(Piece piece)
		{
			promotionpiece=piece;
		}
		
		/// <summary>
		/// retrieve the piece the pawn is promoted to
		/// </summary>
		public Piece PromotionPiece
		{
			get{return promotionpiece;}
		}

		public override string ToString()
		{
			return base.ToString() + promotionpiece;
		}	
	}
}