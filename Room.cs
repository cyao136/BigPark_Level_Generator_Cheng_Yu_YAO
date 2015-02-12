using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigPark_Level_Generator_Cheng_Yu_YAO
{
    class Room
    {
        private int startX;
        private int startY;
        private int endX;
        private int endY;
        private int size;

        /// <summary>
        /// Creates a Room
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public Room(int startX, int startY, int endX, int endY)
        {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
            this.size = Math.Min((endX - startX),(endY - startY));
        }
        public int getStartX()
        {
            return this.startX;
        }
        public int getStartY()
        {
            return this.startY;
        }
        public int getEndX()
        {
            return this.endX;
        }
        public int getEndY()
        {
            return this.endY;
        }
        public int getSize()
        {
            return this.size;
        }
    }
}
