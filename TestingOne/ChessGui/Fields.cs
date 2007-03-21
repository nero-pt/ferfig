/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

using Chess.Game;
using System.Collections;
using System.Drawing;

namespace Chess.Display
{
	public delegate void ChessButtonHandler(Cell cell);

    /// <summary>
    ///    Summary description for Fields.
    /// </summary>
    public class Fields : IEnumerable 
    { 
		private ChessButton[,] buttons;

		public event ChessButtonHandler onSelect;
		public event ChessButtonHandler onMove;

		public ChessButton this[Cell cell]
		{
			get
			{
				return buttons[cell.Col-1,cell.Row-1];
			}
		}

		public IEnumerator GetEnumerator()
		{
			return buttons.GetEnumerator();
		}

		private void RaiseSelectEvent(Cell cell)
		{
			if (onSelect!=null)
			{
				onSelect(cell);
			}
		}

		private void RaiseMoveEvent(Cell cell)
		{
			if (onMove!=null)
			{
				onMove(cell);
			}
		}

		private void button_Click(object Sender, System.EventArgs e)
		{
			if ( !Form1.isMyTurn ) 
				return;
			
			ChessButton b=(ChessButton)Sender;
			if (b.BackColor==Color.DarkGreen)
			{
				RaiseMoveEvent(b.Cell);
			}
			else
			{
				RaiseSelectEvent(b.Cell);
			}
		}

        public Fields()
        {
			buttons=new ChessButton[8,8];

			for (int lRow=0;lRow<8;lRow++)
			{
				for (int lCol=0;lCol<8;lCol++)
				{
					buttons[lRow,lCol]=new ChessButton(lCol,lRow);
					buttons[lRow,lCol].Location=new Point(lRow*64,(7-lCol)*64);
					buttons[lRow,lCol].Size=new Size(64,64);
					buttons[lRow,lCol].Click += new System.EventHandler (this.button_Click);
				}
			}
            
        }
    }
}
