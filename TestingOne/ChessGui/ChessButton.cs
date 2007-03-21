/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using System;
using System.Windows.Forms;
using Chess.Game;

namespace Chess.Display
{
    
    /// <summary>
    ///    Summary description for ChessButton.
    /// </summary>
    public class ChessButton:Button
    {
		private Cell mcell;
		
		private void InitializeComponent () {}

        public ChessButton(int r,int c)
        {
			mcell=new Cell(r+1,c+1);
            
        }

		internal Cell Cell
		{
			get
			{
				return mcell;
			}
		}
    }
}
