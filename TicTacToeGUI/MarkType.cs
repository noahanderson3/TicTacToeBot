using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGUI
{
    /// <summary>
    /// The type of value a cell in the game is currently at
    /// </summary>
   public enum MarkType
    {
        Free,
        /// <summary>
        /// Cell is open
        /// </summary>
        Nought,
        /// <summary>
        /// Cell is O
        /// </summary>
        Cross
        ///<summary>
        ///Cell is X
        ///</summary>
	}
}
