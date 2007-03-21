/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System.Collections;
using System;

namespace Chess.Game
{
	/**Class to implement all the chessrules.
	* due to the nature of chess it is tightly coupled with the game object
	* because validity of moves is depEndent on the turn and gamehistory etc.
	* @author Chris Meijers */
	public class Rules
	{
		private Game g;
		
		/**Game is needed for situation-information*/
		public Rules(Game g)
		{
			this.g=g;
		}

		/**create a move object to move from source to target if possible. Throws exception when this is illegal*/
		public Move createMove(Square source,Square target) 
		{
	      //get all the possible moves from this square on
	      ArrayList moves=getMoves(source);
		 
		   //get the Move object 
		   return createMove(source,target,moves);
		}

		public Move createMove(Square source,Square target,ArrayList moves) 
		{
			if (!moves.Contains(target))
	 		{
				throw new ChessException("Illegal move!");
			}

			if (isTowers(source,target))
				return new Towers(source,target,g.ActiveSide());

			if (isPawnBegin(source,target))
				return new PawnStart(source,target,g.ActiveSide());
			
			if (isEnPassant(source,target))
			{
				//enpassant move must be notified of the piece it captures
				EnPassant m=new EnPassant(source,target,g.ActiveSide());
				Square enpassant=g[source.Row,target.Col];
				Pawn p=(Pawn)(enpassant.Piece);
				m.SetEnPassantPiece(p);

				return m;
			}

			if (isPromotion(source,target))
			{
				//set a Queen as promotionpiece for the moment so the execution of the move does not fail
				Promotion m=new Promotion(source,target,g.ActiveSide());
				m.SetPromotionPiece(new Queen(g.ActiveSide()));
				
				return m;
			}
			
			return new NormalMove(source,target,g.ActiveSide());
		}

		//is the move a towers move?
		private bool isTowers(Square source,Square target)
		{
			if (source.Piece is King && !source.Piece.HasMoved())
			{
				return (Math.Abs(source.Col-target.Col)>1);
			}

			return false;
		}

		//is the move a pawn Begin move?
		private bool isPawnBegin(Square source,Square target)
		{
			Piece p=source.Piece;

			if (p is Pawn)
			{
				Pawn pawn=(Pawn)p;

				if (!p.HasMoved())
				{
					//first move. possible pawn 2 square move
					if (Math.Abs(source.Row-target.Row)>1)
					{
						return true;
					}
				}
			}
			return false;
		}

		//is the move a enpassant move
		private bool isEnPassant(Square source,Square target)
		{
			Piece sourcepiece=source.Piece;
			Piece targetpiece=target.Piece;

			if (sourcepiece is Pawn)
			{
				if (targetpiece==null && source.Col!=target.Col)
				{
					//capture without a piece there.must be enpassant move
					return true;
				}
			}
			return false;
		}

		//is the move a promotion move?
		private bool isPromotion(Square source,Square target)
		{
			Piece sourcepiece=source.Piece;
			Piece targetpiece=target.Piece;

			if (sourcepiece is Pawn)
			{
				if ((g.ActiveSide()==Side.White && target.Row==8) || (g.ActiveSide()==Side.Black && target.Row==1))
				{
					return true;
				}
			}
			return false;
		}

		/**is the current board setup a check-situation for the passed side?*/
		public bool isCheck(Side side)
		{
			//loop all pieces and collect all the fields they can move to
			ArrayList enemymoves=new ArrayList();
			
			//get all enemy squares and loop them to get all squares the nemey can reach
			Squares enemysquares=g[side.Enemy];
			IEnumerator i=enemysquares.GetEnumerator();

			while (i.MoveNext())
			{
				Square s=(Square)i.Current;
				enemymoves.AddRange(getPossibleMoves(s));
			}

			//if the list of enemy squares contains own king, it is check!
			return (enemymoves.Contains(g.KingSquare(side)));
		}
		
		public int moveCount(Side side)
		{
			//loop all pieces and collect all the fields they can move to
			//use a list to avoid skipping double destination squares
			ArrayList moves=new ArrayList();
			
			//get all squares and loop them to get all squares we can reach
			Squares squares=g[side];
			IEnumerator i=squares.GetEnumerator();

			while (i.MoveNext())
			{
				Square s=(Square)i.Current;
				moves.AddRange(getMoves(s));
			}

			//return the number of moves
			return (moves.Count);		
		}
		
		/**is the current board setup a checkmate-situation for the passed side?*/
		public bool isCheckmate(Side side)
		{
			//checkmate is check and no possible moves!
			return (isCheck(side) && moveCount(side)==0);
		}
		
		/**is the current board setup a stalemate-situation for the passed side?*/
		public bool isStalemate(Side side)
		{
			//checkmate is check and no possible moves!
			return (!isCheck(side) && moveCount(side)==0);
		}

		/**retrieve a collection of all squares that can be moved to from the passed square*/
		public ArrayList getMoves(Square s)
		{
			//retrieve all the moves for the piece
			ArrayList moves=getPossibleMoves(s);

			ArrayList tobeRemoved=new ArrayList();
			
			//execute all the moves and see it they cause check 
			IEnumerator i=moves.GetEnumerator();
			while (i.MoveNext())
			{
				Square dest=(Square)i.Current;
				if (causesCheck(s,dest,moves))
				{
					tobeRemoved.Add(dest);
				}
			}

			//remove all check-causing moves from the total		
			i=tobeRemoved.GetEnumerator();

			while (i.MoveNext())
			{
				moves.Remove(i.Current);
			}
			
			return moves;
		}

		//return the collection of possible moves. This does not include the check-test.
		private ArrayList getPossibleMoves(Square s)
		{
			ArrayList moves=null;

			if (s.IsEmpty())
				return new ArrayList();

			if (s.Piece is Pawn)
				return getMovesForPawn(s);
		
			if (s.Piece is Rook)
				return getMovesForRook(s);
			
			if (s.Piece is Bishop)
				return getMovesForBishop(s);
			
			if (s.Piece is Knight)
				return getMovesForKnight(s);
			
			if (s.Piece is King)
			 	return getMovesForKing(s);
		
			if (s.Piece is Queen)
				return getMovesForQueen(s);
			
			Logger.Assert("Square contains unknown piece");
			
			return moves;
		}

		//true if the move causes a check-situation
		private bool causesCheck(Square source,Square dest,ArrayList moves)
		{
			//try
			//{
				//create the move and execute it
				Move m=createMove(source,dest,moves);
				g.ExecuteMove(m);

				if (isCheck(m.Owner))
				{
					//undo the move and return true
					g.UndoMove();
					return true;
				}
				g.UndoMove();
			//}
			//catch(Exception e)
			//{
			//	Logger.Log(e);
			//	Logger.Assert("Internal error: move causes exception");
			//}

			return false;
		}

		private ArrayList getMovesForKing(Square s)
		{
			ArrayList moves=new ArrayList();
			Side Owner=s.Piece.Owner;
			Square test;

			//test all fields arounfd the piece
			test=g.north(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.northeast(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.east(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.southeast(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.south(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.southwest(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.west(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);

			test=g.northwest(s);
			if (test!=null && !test.IsOwnedBy(Owner))
				moves.Add(test);


			//Check for possible towers move
			if (!s.Piece.HasMoved())
			{
				//proceed to the west to see if there are 3 empty squares and a non-moved rook
				test=g.west(s);
				if (test.IsEmpty())
				{
					test=g.west(test);
					if (test.IsEmpty())
					{
						test=g.west(test);
						if (test.IsEmpty())
						{
							test=g.west(test);
							if (!test.IsEmpty() && !test.Piece.HasMoved())
							{
								//add the square east of this one
								moves.Add(g.east(test));
							}
						}
					}
				}

				//proceed to the east to see if there are two empty squares and a non-moved rook
				test=g.east(s);
				if (test.IsEmpty())
				{
					test=g.east(test);
					if (test.IsEmpty())
					{
						test=g.east(test);
						if (!test.IsEmpty() && !test.Piece.HasMoved())
						{
							moves.Add(g.west(test));
						}
					}
				}
			}

			return moves;
		}

		private ArrayList getMovesForRook(Square s)
		{
			ArrayList moves=new ArrayList();
			Side Owner=s.Piece.Owner;
			Square test;

			test=s;
			//INTERESTING 31 = and != at once
			while ((test=g.east(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			test=s;
			while ((test=g.west(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			test=s;
			while ((test=g.north(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			test=s;
			while ((test=g.south(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			return moves;
		}

		private ArrayList getMovesForBishop(Square s)
		{
			ArrayList moves=new ArrayList();
			Side Owner=s.Piece.Owner;
			Square test;

			test=s;
			while ((test=g.northeast(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			test=s;
			while ((test=g.southeast(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			test=s;
			while ((test=g.northwest(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}

			test=s;
			while ((test=g.southwest(test))!=null)
			{
				if (test.IsEmpty())
				{
					moves.Add(test);
				}

				if (test.IsEnemyOf(Owner))
				{
					moves.Add(test);
					break;
				}

				if (test.IsOwnedBy(Owner))
				{
					break;
				}
			}


			return moves;
		}

		public ArrayList getMovesForQueen(Square s)
		{
			//Queen is union of Rook and Bishop
			ArrayList moves=getMovesForRook(s);

			moves.AddRange(getMovesForBishop(s));

			return moves;
		}

		public ArrayList getMovesForKnight(Square s)
		{
			ArrayList moves=new ArrayList();
			Side Owner=s.Piece.Owner;
			Square test;
			Square test2;
			Square test3;
			
			test=g.east(s);
			if (test!=null)
			{
				test2=g.north(test);
				
				if (test2!=null)
				{
					test3=g.north(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      
					test3=g.east(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      	}

				test2=g.south(test);
				
				if (test2!=null)
				{
					test3=g.south(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      
					test3=g.east(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      	}
			}

			test=g.west(s);
			if (test!=null)
			{
				test2=g.north(test);
				
				if (test2!=null)
				{
					test3=g.north(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      
					test3=g.west(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      	}

				test2=g.south(test);
				
				if (test2!=null)
				{
					test3=g.south(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      
					test3=g.west(test2);
					if (test3!=null && (!test3.IsOwnedBy(Owner)))
					{
						moves.Add(test3);
					}
	      	}
			}

			return moves;
		}

		public ArrayList getMovesForPawn(Square s)
		{
			ArrayList moves=new ArrayList();
			Side Owner=s.Piece.Owner;
			Square test;

			if (Owner.IsWhite())
			{
				test=g.north(s);
				if (test!=null && test.IsEmpty())
				{
					moves.Add(test);
					//first two square move test
					if (!s.Piece.HasMoved())
					{
						test=g.north(test);
						if (test!=null && test.IsEmpty())
						{
							moves.Add(test);
						}
					}
				}
				//strike west
				test=g.northwest(s);
				if (test!=null && test.IsEnemyOf(Owner))
				{
					moves.Add(test);
				}
				//strike east
				test=g.northeast(s);
				if (test!=null && test.IsEnemyOf(Owner))
				{
					moves.Add(test);
				}

				if (s.Row==5)
				{
					//possible enpassant
					Square pawnsquare=g.east(s);

					if (pawnsquare!=null && pawnsquare.IsEnemyOf(Owner) && pawnsquare.Piece is Pawn)
					{
						Pawn enemypawn=(Pawn)pawnsquare.Piece;
						if (g.EnPassantPiece==enemypawn)
						{
							moves.Add(g.northeast(s));
						}
					}

					pawnsquare=g.west(s);

					if (pawnsquare!=null && pawnsquare.IsEnemyOf(Owner) && pawnsquare.Piece is Pawn)
					{
						Pawn enemypawn=(Pawn)pawnsquare.Piece;
						if (g.EnPassantPiece==enemypawn)
						{
							moves.Add(g.northwest(s));
						}
					}
				}
			}
			else
			{
				test=g.south(s);
				if (test!=null && test.IsEmpty())
				{
					moves.Add(test);
					//first two square move test
					if (!s.Piece.HasMoved())
					{
						test=g.south(test);
						if (test!=null && test.IsEmpty())
						{
							moves.Add(test);
						}
					}
				}
				//strike west
				test=g.southwest(s);
				if (test!=null && test.IsEnemyOf(Owner))
				{
					moves.Add(test);
				}

				//strike east
				test=g.southeast(s);
				if (test!=null && test.IsEnemyOf(Owner))
				{
					moves.Add(test);
				}

				if (s.Row==4)
				{
					//possible enpassant
					Square pawnsquare=g.east(s);

					if (pawnsquare!=null && pawnsquare.IsEnemyOf(Owner) && pawnsquare.Piece is Pawn)
					{
						Pawn enemypawn=(Pawn)pawnsquare.Piece;
						if (g.EnPassantPiece==enemypawn)
						{
							moves.Add(g.southeast(s));
						}
					}

					pawnsquare=g.west(s);

					if (pawnsquare!=null && pawnsquare.IsEnemyOf(Owner) && pawnsquare.Piece is Pawn)
					{
						Pawn enemypawn=(Pawn)pawnsquare.Piece;
						if (g.EnPassantPiece==enemypawn)
						{
							moves.Add(g.southwest(s));
						}
					}
				}
			}

			return moves;
		}
	}
}