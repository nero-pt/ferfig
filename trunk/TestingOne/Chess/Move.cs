/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System.Diagnostics;

namespace Chess.Game
{
	/// <summary>
	/// represents a move made in a chessgame.
	/// 
	/// subclasses are different kinds of moves
	/// currently implemented are:
	/// -regular moves that can be a capture
	/// -towers long or short
	/// -Promotion move
	/// -Enpassant move
	/// -Pawn two-square move
	/// </summary>
	public abstract class Move
	{
		private bool check;
		private bool checkmate;
		private bool stalemate;
		private Side owner;
		private Cell begin;
		private Cell end;

		protected Piece piece;
		protected Piece capturedpiece;

		/// <summary>
		/// create a move object. Validity checking is not done!
		/// </summary>
		/// <param name="begin"> the square the move starts</param>
		/// <param name="end"> the square the move ends</param>
		/// <param name="owner"> the side execuyting the move</param>
		internal Move(Square begin,Square end,Side owner)
		{
			this.begin=new Cell(begin);
			this.end=new Cell(end);
			this.owner=owner;
			
			piece=begin.Piece;

			capturedpiece=end.Piece;
		}

		/// <summary>
		/// retrieve the I(main) piece that made the move.
		/// can not be null.
		/// </summary>
		public Piece Piece
		{
			get{return piece;}
		}

		/// <summary>
		/// retrieve the piece that was captured if any.
		/// can be null
		/// </summary>
		public Piece Capturedpiece
		{
			get{return capturedpiece;}
		}

		/// <summary>
		/// retrieve the cell the move ends on
		/// </summary>
		public Cell End
		{
			get{return end;}
		}
	
		/// <summary>
		/// retrieve the cell the move starts from
		/// </summary>
		public Cell Begin
		{
			get{return begin;}
		}

		/// <summary>
		/// retrieve the side executing the move
		/// </summary>
		public Side Owner
		{
			get{return owner;}
		}

		/// <summary>
		/// true if the move is a capture move
		/// </summary>
		public bool IsCapture()
		{
			return !(capturedpiece==null);
		}
		
		/// <summary>
		/// set the check flag
		/// </summary>
		internal void Check()
		{
			check=true;
		}

		/// <summary>
		/// set the checkmate flag
		/// </summary>
		public void Checkmate()
		{
			checkmate=true;
		}

		/// <summary>
		/// set the stalemate flag
		/// </summary>
		public void Stalemate()
		{
			stalemate=true;
		}

		/// <summary>
		/// true if the move results in check for the opponent
		/// </summary>
		public bool CausesCheck()
		{
			return check;
		}

		/// <summary>
		/// true if the move results in checkmate for the opponent
		/// </summary>
		public bool CausesCheckmate()
		{
			return checkmate;
		}

		/// <summary>
		/// tgrue if the move causes a stalemate situation
		/// </summary>
		public bool CausesStalemate()
		{
			return stalemate;
		}

		/// <summary>
		/// returns the standard chess-notation for a move
		/// Example: PA2-C3  or pA6-B7 e.p.
		/// </summary>
		public override string ToString()
		{
			string s="" + piece + Begin;

			if (IsCapture())
				s+="x";

			s+=end;

			if (CausesCheck())
				s+= "+";

			if (CausesCheckmate())
				s+="+"; //add another +

			return s;
		}
	}
}