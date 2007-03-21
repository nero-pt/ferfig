/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/// <summary>
	/// specifies an abstract player to participate in a chessgame.
	/// </summary>
	public abstract class Player
	{
		/**the side or 'color' of the player*/
		private Side side;

		/**side is the the color of the player user Side.White or Side.Black*/
		protected Player(Side side)
		{
			this.side=side;
		}

		/// <summary>
		/// the side of this player
		/// </summary>
		public Side Side
		{
			get{return side;}
		}

		/**return the type of the piece the pawn should be promoted to. 
		* illegal type results in the promotion to Queen*/
		public abstract Piece.Kind promote();
	}
}