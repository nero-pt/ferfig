/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System.Collections;
using System.Text;

namespace Chess.Game
{
	/// <summary>
	/// class to represent a chessboard. main responsibility is the execution
	/// and management of moves 
	/// </summary>
	public class Board 
	{
		private Stack moves;
		private Squares squares;

		public Board()
		{
			//Create 64 squares and add them to the board
			squares=new Squares();
			Reset();

			moves=new Stack();
		}

		/// <summary>
		/// return the board to the begin-state
		/// </summary>
		protected void Reset()
		{
			squares.Clear();

			for (int row=1;row<=8;row++)
			{
				for (int col=1;col<=8;col++)
				{
					Square s=new Square(row,col);
					squares.Add(s);
				}
			}
		}

		// INTERESTING 50 defining indexers
		public Square this[string s]
		{
			get{return squares[s];}
		}

		public Square this[int row,int col]
		{
			get{return squares[row,col];}
		}

		public Square this[Cell c]
		{
			get{return squares[c];}
		}

		public Squares this[Side owner]
		{
			get
			{
				Squares owned=new Squares();

				foreach (Square s in squares)
				{
					if (s.IsOwnedBy(owner))
					{
						owned.Add(s);
					}
				}
				return owned;
			}
		}

		public Square KingSquare(Side owner)
		{
			System.Diagnostics.Debug.Assert(squares.Count<=64);
			
			//INTERESTING 53 foreach construction
			foreach (Square s in squares)
			{
				if (s.IsOwnedBy(owner) && s.Piece is King)
				{
					return s;
				}
			}
				
			//INTERESTING 98 Conditional call
			Logger.Assert("No king on the board!");
			return null;
		}

		protected void setupBoard()
		{
			Reset();

			//INTERESTING 51 using indexers
			this["a1"].setPiece(new Rook(Side.White));
			this["h1"].setPiece(new Rook(Side.White));
			this["a8"].setPiece(new Rook(Side.Black));
			this["h8"].setPiece(new Rook(Side.Black));

			//Knight next to them
			this["b1"].setPiece(new Knight(Side.White));
			this["g1"].setPiece(new Knight(Side.White));
			this["b8"].setPiece(new Knight(Side.Black));
			this["g8"].setPiece(new Knight(Side.Black));

			//Bishops follow
			this["c1"].setPiece(new Bishop(Side.White));
			this["f1"].setPiece(new Bishop(Side.White));
			this["c8"].setPiece(new Bishop(Side.Black));
			this["f8"].setPiece(new Bishop(Side.Black));

			//Queens
			this["d1"].setPiece(new Queen(Side.White));
			this["d8"].setPiece(new Queen(Side.Black));
			//Kings
			this["e1"].setPiece(new King(Side.White));
			this["e8"].setPiece(new King(Side.Black));

			//Pawns are last and least
			for (int col=1;col<=8;col++)
			{
				this[2,col].setPiece(new Pawn(Side.White));
				this[7,col].setPiece(new Pawn(Side.Black));
			}
		}

		/**retrieve the square north (row+1) of the passed square or null*/
		public Square north(Square s)
		{
			return this[s.Row+1,s.Col];
		}

		/**retrieve the square northeast (row+1,col+1) of the passed square or null*/
		public Square northeast(Square s)
		{
			return this[s.Row+1,s.Col+1];
		}

		/**retrieve the square east (col+1) of the passed square or null*/
		public Square east(Square s)
		{
			return this[s.Row,s.Col+1];
		}

		/**retrieve the square southeast (row-1,col+1) of the passed square or null*/
		public Square southeast(Square s)
		{
			return this[s.Row-1,s.Col+1];
		}

		/**retrieve the square south (row-1) of the passed square or null*/
		public Square south(Square s)
		{
			return this[s.Row-1,s.Col];
		}

		/**retrieve the square southwest (row-1,col-1) of the passed square or null*/
		public Square southwest(Square s)
		{
			return this[s.Row-1,s.Col-1];
		}

		/**retrieve the square west of (col-1) the passed square or null*/
		public Square west(Square s)
		{
			return this[s.Row,s.Col-1];
		}

		/**retrieve the square northwest (row+1,col-1) of the passed square or null*/
		public Square northwest(Square s)
		{
			return this[s.Row+1,s.Col-1];
		}

		public Piece EnPassantPiece
		{
			get
			{
				if (moves.Count>0)
				{
					Move m=(Move)moves.Peek();
				
					if (m is PawnStart)
					{
						return m.Piece;
					}
				}
				return null;
			}
		}

		/**execute the passed move object and save it on the stack for undoing*/
		internal void ExecuteMove(Move move)
		{
			if (move is NormalMove)
				ExecuteMove((NormalMove)move);
			
			if (move is Towers)
				ExecuteMove((Towers)move);

			if (move is PawnStart)
				ExecuteMove((PawnStart)move);
			
			if (move is EnPassant)
				ExecuteMove((EnPassant)move);

			if (move is Promotion)
				ExecuteMove((Promotion)move);

			//increase move-count on the piece
			move.Piece.Move();
			//store the move on the moves
			moves.Push(move);

			return;
		}

		/**undo the last move on the stack*/
		public void UndoMove(Move move)
		{
			if (move is NormalMove)
				UndoMove((NormalMove)move);
		
			if (move is Towers)
				UndoMove((Towers)move);
		
			if (move is PawnStart)
				UndoMove((PawnStart)move);
			
			if (move is EnPassant)
				UndoMove((EnPassant)move);

			if (move is Promotion)
				UndoMove((Promotion)move);

			//decrease the movecount of the piece
			move.Piece.UndoMove();
			
			return;
		}
		
		/**execute a normal move*/
		private void ExecuteMove(NormalMove move)
		{
			//move the piece to the destination square
			Square Begin=this[move.Begin];
			Square end=this[move.End];
			
			//clear the Begin-square
			Begin.setPiece(null);
			end.setPiece(move.Piece);
		}

		/**execute a towers move involving rook and king*/
		private void ExecuteMove(Towers move)
		{
			if (move.End.Col==7)
			{
				//short towers over two squares
				Square rooksource=this[move.Begin.Row,8];
				Square rookdest=this[move.Begin.Row,6];
				
				//move the rook
				rookdest.setPiece(rooksource.Piece);
				rooksource.setPiece(null);

				Square kingsource=this[move.Begin.Row,5];
				Square kingdest=this[move.Begin.Row,7];
				
				//move the king
				kingdest.setPiece(kingsource.Piece);
				kingsource.setPiece(null);
			}
			else if (move.End.Col==2)
			{
				//long towers over 3 squares
				Square rooksource=this[move.Begin.Row,1];
				Square rookdest=this[move.Begin.Row,3];
				
				//move the rook
				rookdest.setPiece(rooksource.Piece);
				rooksource.setPiece(null);

				Square kingsource=this[move.Begin.Row,5];
				Square kingdest=this[move.Begin.Row,2];
				
				//move the king
				kingdest.setPiece(kingsource.Piece);
				kingsource.setPiece(null);
			}
			else
			{
				Logger.Assert("Towers move can not be performed!");
				return;
			}
		}

		/**execute a Beginmove for a pawn of 2 squares*/
		private void ExecuteMove(PawnStart move)
		{
			//move the piece to the destination
			Square End=this[move.End];
			End.setPiece(move.Piece);

			//clear the source-square
			Square Begin=this[move.Begin];
			Begin.setPiece(null);
		}
		
		/**execute an en-passant move*/
		private void ExecuteMove(EnPassant move)
		{
			//move the piece to the destination
			Square End=this[move.End];
			End.setPiece(move.Piece);

			//clear the source square
			Square Begin=this[move.Begin];
			Begin.setPiece(null);
			
			if (move.Owner==Side.White)
			{
				//find the suare the pawn that will be captured is now on and clear it
				Square enpassant=this[5,move.End.Col];
				enpassant.setPiece(null);
			}
			else
			{
				//find the suare the pawn that will be captured is now on and clear it
				Square enpassant=this[4,move.End.Col];
				enpassant.setPiece(null);
			}
		}

		/**execute a move where a pawn is promoted*/
		private void ExecuteMove(Promotion move)
		{
			//place the piece the pawn is promoteed to at the destination square
			Square End=this[move.End];
			End.setPiece(move.PromotionPiece);

			//clear the source square
			Square Begin=this[move.Begin];
			Begin.setPiece(null);
		}

		/**undoes the last move and returns true if anything has been undone. Does not raise events*/
		public bool UndoMove()
		{
			if (moves.Count>0)
			{
				//get the move object
				Move move=(Move)moves.Pop();
				UndoMove(move);
				return true;
			}

			return false;
		}

		/**undo a normal move*/
		private void UndoMove(NormalMove move)
		{
			//restore the initial state
			Square Begin=this[move.Begin];
			Square End=this[move.End];

			Begin.setPiece(move.Piece);
			End.setPiece(move.Capturedpiece);
		}

		private void UndoMove(Towers move)
		{
			if (move.End.Col==7)
			{
				//put back the king
				this[move.Begin].setPiece(move.Piece);
				//put back the rook in the corner
				this[move.Begin.Row,8].setPiece(this[move.Begin.Row,6].Piece);
				//empty the squares between
				this[move.Begin.Row,7].setPiece(null);
				this[move.Begin.Row,6].setPiece(null);
			}

			if (move.End.Col==2)
			{
				//put back the king
				this[move.Begin].setPiece(move.Piece);
				//put back the rook in the corner
				this[move.Begin.Row,1].setPiece(this[move.Begin.Row,3].Piece);
				//empty the squares between
				this[move.Begin.Row,2].setPiece(null);
				this[move.Begin.Row,3].setPiece(null);
			}
		}

		private void UndoMove(PawnStart move)
		{
			//restore the original situation
			Square Begin=this[move.Begin];
			Square End=this[move.End];

			Begin.setPiece(move.Piece);
			End.setPiece(null);
		}

		private void UndoMove(EnPassant move)
		{
			//restore the original situation
			Square Begin=this[move.Begin];
			Square End=this[move.End];

			Begin.setPiece(move.Piece);
			End.setPiece(null);

			Square enpassant;
			
			//determine the square the captured pawn was on
			if (move.Owner==Side.White)
			{
				enpassant=this[5,move.End.Col];
			}
			else
			{
				enpassant=this[4,move.End.Col];
			}
			
			//replace the piece at the square
			EnPassant m=(EnPassant)move;
			enpassant.setPiece(m.Capturedpiece);
		}

		private void UndoMove(Promotion move)
		{
			//restore the original situation
			Square Begin=this[move.Begin];
			Square End=this[move.End];

			Begin.setPiece(move.Piece);
			End.setPiece(move.Capturedpiece);
		}

		/**log the current board with pieces for debugging purposes*/
		public override string ToString()
		{
			StringBuilder builder=new StringBuilder();

			foreach (Square s in squares)
			{
				if (s.Piece==null)
				{
					builder.Append(s);
				}
				else
				{
					builder.Append(s + "-" + s.Piece);
				}
			}

			return builder.ToString();
		}
	}
}