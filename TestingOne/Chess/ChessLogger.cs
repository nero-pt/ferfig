/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	/**class used to log a chess game to System.out using the ChessListener interface
	* @author Chris Meijers*/
	public class ChessLogger : ChessListener
	{
		protected void WriteMessage(string s)
		{
			System.Console.WriteLine(s);
		}
				public void Begin()
		{
			WriteMessage("Begin Game");
		}
		
		public void End()
		{
			WriteMessage("End");
		}		

		public void WaitForOpponentMove()
		{
			WriteMessage("Wait. Oppenont move!");
		}		
				
		public void Move(Move m)
		{
			WriteMessage("Move: " + m);
		}
		
		public void UndoMove()
		{
			WriteMessage("Undo move");
		}
	}
}