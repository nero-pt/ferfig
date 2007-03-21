/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Display
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Data;

	//INTERESTING 41 using directive
	using Chess.Game;
   
    public class Form1 : pt.vodafone.ufe.main.XProc
    {
		//INTERESTING 0 vstudio environment
		//				new look and feel
		//				intellisense with overloading
		//				comments summary and attributes
		//				dynamic help 
		//				collapse/expand code
		//				task list									
		//				tabs for files
		//				fullscreen (shift-alt-enter)
		//				better syntax checking
		//				more than 1 compiler error at once
		//				compile without running!
		
        private System.ComponentModel.Container components;
		
		private Fields buttons;
		private Cell selectedcell;
		private Game thegame;
       private System.Windows.Forms.ListBox lstLog;
       private System.Windows.Forms.Label label1;
		private ChessImages images;
		
		private Side mySide = Side.White;
		private System.Windows.Forms.Panel panProc;
			
		public static bool isMyTurn = true;
		
        public Form1()
        {
            thegame=new Game(new ManualPlayer(Side.White),new ManualPlayer(Side.Black));
			buttons=new Fields();
			images=new ChessImages();
		
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			Init();
			
			//INTERESTING 74 subscribing on custom events
            thegame.StartGame+=new ChessSimpleEvent(StartTheGame);
			thegame.EndGame+=new ChessSimpleEvent(EndTheGame);
			thegame.MoveUndone+=new ChessSimpleEvent(UndoTheMove);
			thegame.MoveExecuted+=new ChessMoveEvent(MakeMove);
			thegame.addChessListener(new ChessLogger());
			thegame.addChessListener(new ListboxLogger(lstLog));
			
			thegame.Start(mySide);
        }

		private void Init()
		{
			this.components = new System.ComponentModel.Container ();
			this.Scale(5, 13);
			//FF this.AutoScaleBaseSize = new System.Drawing.Size (5, 13);
			//this.Size=new Size(130+8*64+10,8*64+28);
			this.panProc.BackColor=Color.RosyBrown;
			//this.Text="C#ESS (c) Chris Meijers 2001 ";
			
			foreach (ChessButton b in buttons)
			{
				this.panProc.Controls.Add(b);
			}

			// INTERESTING 76 subscribing on UI events
			buttons.onSelect+=new ChessButtonHandler(buttonselect);
			buttons.onMove+=new ChessButtonHandler(fieldmove);

			//INTERSTING creating menu's on the fly
			//FF this.Menu=new MainMenu();
			MenuItem topitem=new MenuItem("Game");
			MenuItem undo=new MenuItem("Undo");
			MenuItem start=new MenuItem("Start");
			
			topitem.MenuItems.Add(start);
			topitem.MenuItems.Add(undo);
			
			undo.Click+=new EventHandler(MenuUndo);
			//FF start.Click+=new EventHandler(MenuStart);
			
			//FF: this.Menu.MenuItems.Add(topitem);
			
			ClearScreen();
		}
		//INTERESTING 73 custom eventhandlers
		private void StartTheGame()
		{
			RefreshScreen();
		}

		private void EndTheGame()
		{
			RefreshScreen();
		}

		private void UndoTheMove()
		{
			RefreshScreen();
		}

		private void MakeMove(Move m)
		{
			if (m.Piece.IsOwnedBy(mySide))
			{
				//"Wait opponent move...";
				isMyTurn = false;
			}
			else
			{
				//"My move...";
				isMyTurn = true;
			}
			RefreshScreen();
			
			SendMoveToOpponent(m);
		}

		private void MenuUndo(object sender,EventArgs a)
		{
			thegame.undo();
		}

//FF		private new void MenuStart(object sender,EventArgs a)
//		{
//			thegame.Start();
//		}

		private void InitializeComponent ()
		{
			this.lstLog = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panProc = new System.Windows.Forms.Panel();
			this.panProc.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstLog
			// 
			this.lstLog.BackColor = System.Drawing.Color.Silver;
			this.lstLog.Location = new System.Drawing.Point(512, 16);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(120, 485);
			this.lstLog.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(520, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Game logger:";
			// 
			// panProc
			// 
			this.panProc.Controls.Add(this.label1);
			this.panProc.Controls.Add(this.lstLog);
			this.panProc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panProc.Location = new System.Drawing.Point(0, 0);
			this.panProc.Name = "panProc";
			this.panProc.Size = new System.Drawing.Size(848, 576);
			this.panProc.TabIndex = 2;
			// 
			// Form1
			// 
			this.Controls.Add(this.panProc);
			this.Name = "Form1";
			this.Size = new System.Drawing.Size(848, 576);
			this.panProc.ResumeLayout(false);
			this.ResumeLayout(false);

		}

      //TODO: For Beta2 uncomment this method
      /*public override void Dispose()
      {
         base.Dispose();
         components.Dispose();
      }
      */

		protected void buttonselect(Cell cell)
		{
			selectedcell=cell;

			ClearScreen();

			ArrayList l=thegame.GetValidMoves(cell);

			IEnumerator i=l.GetEnumerator();

			while (i.MoveNext())
			{
				Square test=(Square)i.Current;
				
				buttons[test].BackColor=Color.DarkGreen;
			}
			buttons[selectedcell].BackColor=Color.DarkRed;
		}

		protected void fieldmove (Cell cell)
		{
			try 
			{
				//INTERESTING 83 catching exceptions
				//thegame.movePiece("e2e5");  //Illegal move
				thegame.movePiece(selectedcell,cell);
				ClearScreen(); 
			}
			catch(ChessException e)
			{
				MessageBox.Show(e.ToString() + "\n at row " + e.Row + " and column " + e.Col);
			}
		}

//FF		//INTERESTING 00 entry point for application
//		public static void Main(string[] args) 
//        {
//            Application.Run(new Form1());
//        }

		private void RefreshScreen()
		{
			foreach (ChessButton b in buttons)
			{
				Piece p=thegame[b.Cell].Piece;
					
				if (p==null)
				{
					b.Text="";
					b.Image=null;
				}
				else
				{
					b.Text=p.ToString();

					if (p.IsOwnedBy(Side.White))
					{
						b.ForeColor=Color.White;
					}
					else
					{
						b.ForeColor=Color.Black;
					}
					
					b.Image=images[p];
				}
			}
		}

		private void ClearScreen()
		{
			foreach (ChessButton b in buttons)
			{
				if (b.Cell.IsDark)
				{
					b.BackColor=Color.FromArgb(114,89,3);
				}
				else
				{
					b.BackColor=Color.FromArgb(200,166,64);
				}
			}
		}
		
		private void SendMoveToOpponent(Move m)
		{
			//TODO: send move
			Console.WriteLine("sending move to opponent...");
		}
    }
}