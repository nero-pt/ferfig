/*
This code (c)2001 by Chris Meijers (chrisis@chello.nl) 
No part of this code may be used for other than educational purposes 
without the explicit written permission of the author
*/

namespace Chess.Display
{
   using System;
	using System.Windows.Forms;
	using System.Drawing;
	using Chess.Game;
    

    /// <summary>
    ///    Summary description for ChessImages.
    /// </summary>
    public class ChessImages
    {
		private ImageList thelist;
		
        public ChessImages()
        {
			thelist=new ImageList();
	
			LoadImages();			
        }

		private void LoadImages()
		{
			thelist.ImageSize=new Size(45,45);
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\KW.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\KB.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\QW.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\QB.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\RW.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\RB.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\BW.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\BB.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\NW.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\NB.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\pW.gif"));
			thelist.Images.Add(System.Drawing.Image.FromFile(@"D:\Vodafone\UFE-ROOT\ufe\procs\Chess\ChessGui\pB.gif"));
		}

		public Bitmap this[Piece p]
		{
			get
			{
				return (Bitmap)thelist.Images[GetIndex(GetFilename(p))];
			}
		}
		
		private string GetFilename(Piece p)
		{
			string filename;

			filename=p.ToString();

			if (p.IsOwnedBy(Side.White))
			{
				return filename+"W";
			}
			else
			{
				return filename+"B";
			}
		}

		private int GetIndex(string s)
		{
			switch (s)
			{
				case "KW":
					return 0;
				case "KB":
					return 1;
				case "QW":
					return 2;
				case "QB":
					return 3;
				case "RW":
					return 4;
				case "RB":
					return 5;
				case "BW":
					return 6;
				case "BB":
					return 7;
				case "NW":
					return 8;
				case "NB":
					return 9;
				case "pW":
					return 10;
				case "pB":
					return 11;
				default:
					return 0;
			}
		}
    }
}
