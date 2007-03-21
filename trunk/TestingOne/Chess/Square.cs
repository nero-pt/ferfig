/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Game
{
	//INTERESTING 05 Subclass
	public class Square:Cell
	{
		private Piece piece;
			
		internal Square(int row,int col):base(row,col){}

		internal Square(string s):base(s){}		
		
		/// <summary>
		/// set the passed Piece to this square
		/// </summary>
		/// <param name="piece"> the piece to be put on this square. 
		/// use 'null' to clear the square</param>
		internal void setPiece(Piece piece)
		{
			this.piece=piece;
		}
		
		/// <summary>
		/// retrieve the piece on the square.
		/// returns 'null' when empty
		/// </summary>
		public Piece Piece
		{
			get{return piece;}
		}
				
		/// <summary>
		/// true if the square contains a piece owned by the passed side
		/// </summary>
		/// <param name="side"> </param>
		public bool IsOwnedBy(Side side)
		{
			if (IsEmpty())
			{
				return false;
			}	
			
			return piece.Owner==side;	
		}
			
		/// <summary>
		/// true if the square contains a piece that is an enemy of the passed side
		/// </summary>
		/// <param name="side"> </param>
		public bool IsEnemyOf(Side side)
		{
			if (IsEmpty())
			{
				return false;
			}	
			
			return (piece.Owner!=side);	
		}
		
		/// <summary>
		/// true if the square does not contain a piece
		/// </summary>
		public bool IsEmpty()
		{
			return (piece==null);
		}
		
		/// <summary>
		/// true is the square contains a piece of the specified kind.
		/// false if empty or contains other type
		/// </summary>
		/// <param name="type"> </param>
		public bool Contains(Piece.Kind type)
		{
			if (!IsEmpty())
			{
				return (piece.Type==type);
			}
			return false;
		}
	}
}