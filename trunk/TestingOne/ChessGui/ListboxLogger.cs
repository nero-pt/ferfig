/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System;
using Chess.Game;
using System.Windows.Forms;

namespace Chess.Display
{
   /**class used to log a chess game to System.out using the ChessListener interface
   * @author Chris Meijers*/
   public class ListboxLogger : ChessListener
   {
      private ListBox mListbox;

      public ListboxLogger(ListBox l)
      {
         mListbox=l;
      }

      protected void WriteMessage(string s)
      {
         mListbox.Items.Insert(0,s);
      }
      public void Begin()
      {
         WriteMessage("Begin Game");
      }
		
      public void End()
      {
         WriteMessage("End");
      }		
				
      public void Move(Move m)
      {
         WriteMessage("Move: " + m);
      }
		
      public void UndoMove()
      {
         WriteMessage("Undo move");
      }
	  
	  public void WaitForOpponentMove()
	  {
		 WriteMessage("Wait opponent move...");
	  }
   }
}
