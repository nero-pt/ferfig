/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	// INTERESTING 10 abstract class definition
	public abstract class Piece
	{
		private int moves=0;
		private Side owner;
		
		//INTERESTING 48 enumerations
		public enum Kind {King, Queen, Rook, Bishop, Knight, Pawn};

		protected Piece(Side owner)
		{
			this.owner=owner;
		}
		
		public bool IsOwnedBy(Side side)
		{
			return (side==owner);
		}
		
		public bool IsEnemyOf(Side side)
		{
			return !IsOwnedBy(side);
		}
		
		public bool IsEnemyOf(Piece piece)
		{
			return !(owner==piece.Owner);	
		}
		
		public int Moves
		{
			get{return moves;}
		}

		public Side Owner
		{
			get{return owner;}
		}
		
		public bool HasMoved()
		{
			return (moves>0);
		}

		internal void Move()
		{
			moves++;
		}
		
		internal void UndoMove()
		{
			if (--moves<0)
			    Logger.Assert("Undo move called on piece that has not moved yet");
			
			//INTERESTING 30 use of  -- operator
			/*if (moves>0)
			{
				moves--;
			}
			else
			{
				Logger.Assert("Undo move called on piece that has not moved yet");
			}
			*/
		}
		
		//INTERESTING 15 abstract method
		public abstract Kind Type
		{
			get;
		}
	}
}