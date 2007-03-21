/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System.Windows.Forms;
using Chess.Game;

namespace Chess.Display
{
	/**implementation of a Chessplayer that work through screen interaction*/
	public class ManualPlayer : Player
	{
		/**player needs a side and a frame to work with*/
		internal ManualPlayer(Side side):base(side)
		{
		}
		
		/**implementation of Player*/
		public override Piece.Kind promote()
		{
			return Piece.Kind.Queen;	
		}
	}
}