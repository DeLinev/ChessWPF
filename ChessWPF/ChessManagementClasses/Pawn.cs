using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Pawn : PieceBase
	{
		public override ChessPieceType Type { get => ChessPieceType.Pawn; }

		public Pawn(PieceColor color) : base(color) { }

		public Pawn(Pawn obj) : base(obj.Color) { }
	}
}
