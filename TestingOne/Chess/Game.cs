/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System.Collections;
using System;

namespace Chess.Game
{
	// INTERESTING 70 delegate definition for events
	public delegate void ChessSimpleEvent();
	public delegate void ChessMoveEvent(Move m);

	public class Game : Board
	{
		private Player white;
		private Player black;
		private Rules rules;
		private Side active;
		
		// INTERESTING 72 event definitions
		public event ChessSimpleEvent StartGame;
		public event ChessSimpleEvent EndGame;
		public event ChessSimpleEvent MoveUndone;
		public event ChessMoveEvent MoveExecuted;

		public Game(Player white, Player black)
		{
			this.white=white;
			this.black=black;

			//white Begins the game
			active=white.Side;

			rules=new Rules(this);
		}

		public Side ActiveSide()
		{
			return active;
		}
	
		public ArrayList GetValidMoves(Cell cell)
		{
			return GetValidMoves(this[cell]);
		}
		
		/**return the list of Square objects that are valid destinations for the specified Square*/
		internal ArrayList GetValidMoves(Square s)
		{
			if (s.IsEmpty())
			{
				return new ArrayList();
			}

			if (s.Piece.IsEnemyOf(active))
			{
				//this piece does not belong to the active side, no moves possible
				return new ArrayList();
			}
			
			//forward the call to the Rule-class
			return rules.getMoves(s);
		}

		/**register a ChessListener class to receive notification of events of the game*/
		public void addChessListener(ChessListener listener)
		{
			StartGame+=new ChessSimpleEvent(listener.Begin);
			EndGame+=new ChessSimpleEvent(listener.End);
			MoveUndone+=new ChessSimpleEvent(listener.UndoMove);
			MoveExecuted+=new ChessMoveEvent(listener.Move);
		}

		/**unregister a ChessListener class to stop receiving events*/
		public void removeChessListener(ChessListener listener)
		{
			StartGame-=new ChessSimpleEvent(listener.Begin);
			EndGame-=new ChessSimpleEvent(listener.End);
			MoveUndone-=new ChessSimpleEvent(listener.UndoMove);
			MoveExecuted-=new ChessMoveEvent(listener.Move);
		}

		/**Begin a new game and raise Begin-event*/
		public void Start(Side s)
		{
			setupBoard();
			active=s;

			raiseBeginEvent();
		}

		/**End the current game and raise End-event*/
		public void End()
		{
			raiseEndEvent();
		}

		private void raiseBeginEvent()
		{
		// INTERESTING 75 raising events
		if (StartGame!=null)
				StartGame();
		}

		private void raiseEndEvent()
		{
			if (EndGame!=null)
				EndGame();
		}

		private void raiseMoveEvent(Move move)
		{
			if (MoveExecuted!=null)
				MoveExecuted(move);
		}

		private void raiseUndoEvent()
		{
			if (MoveUndone!=null)
				MoveUndone();
		}

		/**undo the last move by rolling back the top of the stack
		* raises UndoMove event if anything to undo*/
		public void undo()
		{
			//call the actual move and raise the event
			if (base.UndoMove())
			{
				switchPlayer();
				raiseUndoEvent();
			}
		}

		private void switchPlayer()
		{
			active=active.Enemy;
		}

		public void movePiece(string s)
		{
			Cell Begin=new Cell(s.Substring(0,2));
			Cell end=new Cell(s.Substring(2,2));
	
			movePiece(Begin.Row,Begin.Col,end.Row,end.Col);
		}

		public void movePiece(Cell Begin,Cell end)
		{
			movePiece(Begin.Row,Begin.Col,end.Row,end.Col);
		}
		
		/**move the piece that is on the specified location to the specified destination location
		* @throws exception when this move is not legal (@see getValidMoves for a listing of possible and legal moves
		* raises event when succesful
		*/
		public void movePiece(int rowBegin,int colBegin,int rowEnd,int colEnd)
		{
			Square s=this[new Cell(rowBegin,colBegin)];
			Square t=this[new Cell(rowEnd,colEnd)];

			if (!s.IsOwnedBy(active))
			{
				//INTERESTING 81 throwing an exception
				throw(new ChessException("Illegal move",rowBegin,rowEnd));
			}

			if (s.IsEmpty())
			{
				//can't move with an empty field
				throw new ChessException("move attempted at an empty field");
			}

			//attempt to create a move object. Succeeds if the move is legal
			Move m=rules.createMove(s,t);

			//if this is a promotion, the player must be prompted for a promotion piece
			if (m is Promotion)
			{
				setPromotionPiece((Promotion)m);
			}
			
			//execute the move, switch turn and raise event
			ExecuteMove(m);
			
			switchPlayer();
			
			//after players have switched, see if we have check or mate situtaions and update the move object in that case
			if (rules.isCheck(ActiveSide()))
			{
				m.Check();
				
				if (rules.isCheckmate(ActiveSide()))
				{
					m.Checkmate();
				}
			}
			
			if (rules.isStalemate(ActiveSide()))
			{
				m.Stalemate();
			}
			
			raiseMoveEvent(m);
		}
		
		private void setPromotionPiece(Promotion m)
		{
			Player p;
			
			if (m.Owner==Side.White)
				p=white;
			else
				p=black;
			
			Piece.Kind type=p.promote();
			
			Piece promotepiece;
					
			//INTERESTING 32 switch statement
			//determine the piece to promote to
			switch (type)
			{
				case Piece.Kind.Rook:
					promotepiece=new Rook(m.Owner);
					break;
				case Piece.Kind.Bishop:
					promotepiece=new Bishop(m.Owner);
					break;
				case Piece.Kind.Knight:	
					promotepiece=new Knight(m.Owner);
					break;
				default:
					promotepiece=new Queen(m.Owner);
					break;
			}
			
			//set the move object with the piece
			m.SetPromotionPiece(promotepiece);
		}
	}
}
