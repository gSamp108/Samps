using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tinaland.Interface
{
    public sealed class DebugSystem
    {
        public sealed class PaintingDebug
        {
            public System.Drawing.Point CenterScreen;
            public System.Drawing.Point CurrentTileCornerOffset;
            public System.Drawing.Point CurrentTileCorner;
        }

        public PaintingDebug Painting = new PaintingDebug();
    }
}
