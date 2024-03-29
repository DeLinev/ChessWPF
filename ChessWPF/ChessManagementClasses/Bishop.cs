using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Bishop : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Bishop;

		public Bishop(PieceColor color) : base(color) { }

		public Bishop(Bishop obj) : base(obj.Color) { }
	}
}
