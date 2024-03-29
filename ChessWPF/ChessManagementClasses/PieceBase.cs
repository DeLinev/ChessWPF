using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public abstract class PieceBase
	{
		public abstract ChessPieceType Type { get; }
		public PieceColor Color { get; }
		public bool HasMoved { get; set; } = false;

		public PieceBase(PieceColor color)
		{
			Color = color;
		}
	}
}
