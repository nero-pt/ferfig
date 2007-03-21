/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/**interface specifying a listener for chess events
	* @author Chris Meijers*/

	public interface ChessListener
	{
		/**Begin of a new game*/
		void Begin();
		
		/**Endof the current game*/
		void End();
		
		/**a move has been executed by the game*/
		void Move(Move m);

		/**a move has been executed by the game*/
		void WaitForOpponentMove();
		
		/**the last move was undone by the game*/
		void UndoMove();
	}
}