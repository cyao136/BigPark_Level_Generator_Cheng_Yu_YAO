using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigPark_Level_Generator_Cheng_Yu_YAO
{
    class Level
    {
        private static char PADDING_CHAR_X = 'X';
        private static char PADDING_CHAR_O = 'O';
        private static char VERTICAL_WALL = '|';
        private static char HORIZONTAL_WALL = '-';
        private static char STARTING_POINT = '*';
        private static char EMPTY = ' ';
        private static int PATH_SIZE = 1;

        private List<Room> rooms;
        private int height;
        private int width;
        private string[] map;

        /// <summary>
        /// Creates an empty level
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Level(int width, int height)
        {
            string[] vectY = new string[height];
            for (int i = 0 ; i < vectY.Length ; i++)
            {
                vectY[i] = new String(PADDING_CHAR_X, width);

            }
            this.height = height;
            this.width = width;
            this.map = vectY;
            this.rooms = new List<Room>();
        }

        /// <summary>
        /// Creates a level with the rooms added to the map
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rooms"></param>
        public Level(int width, int height, List<Room> rooms)
        {
            string[] vectY = new string[height];
            for (int i = 0; i < vectY.Length; i++)
            {
                vectY[i] = new String(PADDING_CHAR_X, width);
            }
            this.height = height;
            this.width = width;
            this.map = vectY;
            this.rooms = new List<Room>();
            //Add all the rooms
            for (int i = 0; i < rooms.Count; i++ )
            {
                addRoom(rooms[i], this);
                //Add path if it's not connected
                if (i > 0 && !isConnected(rooms[i], this))
                {
                    addPath(rooms[i], rooms[i - 1], this);
                }
            }
            //Always start at the first available location.
            for (int i = 1; i < this.map.Length; i++)
            {int index = this.map[i].IndexOf(PADDING_CHAR_O);
                if (index > 0)
                {
                    StringBuilder strBuilder = new StringBuilder(this.map[i]);
                    string start = STARTING_POINT.ToString();

                    strBuilder.Remove(index, 1);
                    strBuilder.Insert(index, start);
                    this.map[i] = strBuilder.ToString();
                    break;
                }
            }

            //Console.Write("DEBUG: Number of Rooms in this Level: " + this.rooms.Count.ToString());
        }
        /// <summary>
        /// Adds a room to the level and maps it
        /// </summary>
        /// <param name="room"></param>
        /// <param name="level"></param>
        public void addRoom(Room room, Level level)
        {
            int startX = room.getStartX();
            int startY = room.getStartY();
            int endX = room.getEndX();
            int endY = room.getEndY();
            int diffX = endX - startX;
            int diffY = endY - startY;

            for (int i = startY; i < (diffY + startY); i++)
            {
                //Console.Write("DEBUG: Line #: "+ i);
                
                StringBuilder strBuilder = new StringBuilder(level.map[i]);
                string padding = new String(PADDING_CHAR_O,diffX);

                strBuilder.Remove(startX, diffX);
                strBuilder.Insert(startX, padding);
                level.map[i] = strBuilder.ToString();

                //Console.Write("DEBUG: Level line "+ i + " : " + map[i]);
            }

            level.rooms.Add(room);

        }
        /// <summary>
        /// Add a path as a room to the level and maps it
        /// </summary>
        /// <param name="room"></param>
        /// <param name="prevRoom"></param>
        /// <param name="level"></param>
        public void addPath(Room room, Room prevRoom, Level level)
        {
            int pathX1 = prevRoom.getStartX();
            int pathY1 = prevRoom.getStartY();
            int pathX2 = room.getStartX();
                int pathY2 = room.getStartY();
                int intersectX = room.getStartX();
                int intersectY = prevRoom.getStartY();
                int diffX = room.getStartX() - prevRoom.getStartX();
                int diffY = room.getStartY() - prevRoom.getStartY();

                Room newPathX;
                Room newPathY;

                if (diffX >= 0)
                {
                    newPathX = new Room(pathX1, pathY1, (intersectX+1), (intersectY+PATH_SIZE));
                }
                else
                {
                    newPathX = new Room(intersectX, intersectY, pathX1, (pathY1+PATH_SIZE));
                }
                if (diffY >= 0)
                {
                    newPathY = new Room(intersectX, intersectY, (pathX2+PATH_SIZE), pathY2);
                }
                else
                {
                    newPathY = new Room(pathX2, pathY2, (intersectX+PATH_SIZE), intersectY);
                }
                int tmpStartX = room.getStartX();
            addRoom(newPathY, this);
            addRoom(newPathX, this);
        }
        /// <summary>
        /// Determine whether a room is connected in a level
        /// </summary>
        /// <param name="room"></param>
        /// <param name="level"></param>
        /// <returns>true if room is connected to a path or another room,
        /// false otherwise.</returns>
        private bool isConnected(Room room, Level level)
        {
            int sX = room.getStartX();
            int sY = room.getStartY();
            int eX = room.getEndX();
            int eY = room.getEndY();
            int size = room.getSize();
            string[] curMap = level.map;

            //Check if it's connected on top
            if (sY > 0)
            {
                if (curMap[sY - 1].Substring(sX, size).ToCharArray().Contains(PADDING_CHAR_O))
                {
                    return true;
                }
            }
            //Check bottom
            if (eY < (curMap.Length - 1))
            {
                if (curMap[eY].Substring(sX, size).ToCharArray().Contains(PADDING_CHAR_O))
                {
                    return true;
                }
            }
            //Check left
            if (sX > 0)
            {
                for (int i = sY; i < eY; i++)
                {
                    if (curMap[i].ToCharArray()[sX - 1] == 'O')
                    {
                        return true;
                    }
                }
            }
            //Check right
            if (eX < (curMap[0].Length - 1))
            {
                for (int i = sY; i < eY; i++)
                {
                    if (curMap[i].ToCharArray()[eX] == 'O')
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Get all the rooms in the level
        /// </summary>
        /// <returns>list of Room in level</returns>
        public List<Room> getRooms()
        {
            return rooms;
        }

        //Oh man this function is a mess... It works! though
        /// <summary>
        /// Prints the level in the console.
        /// It prints using '|' as vertical wall,
        /// '-' as horizontal wall,
        /// '*' as starting point, 
        /// and fills the rooms with 'X'.
        /// </summary>
        public void print()
        {
            Console.WriteLine("\n");
            for (int i = 0; i < this.map.Length ; i++)
            {
                char[] line = map[i].ToCharArray();
                char[] printLine = new char[map[i].Length];
                if (i == 0)
                {
                    for (int s = 0; s < map[i].Length; s++)
                    {
                        bool replaced = false;
                        bool isReplaceable = line[s] == PADDING_CHAR_X;
                        //Find Top wall
                        if (isReplaceable &&
                            (map[i + 1].ToCharArray()[s] == PADDING_CHAR_O ||
                            map[i + 1].ToCharArray()[s] == STARTING_POINT))
                        {
                            if (map[i + 1].ToCharArray()[s] == STARTING_POINT)
                            {
                                printLine[s] = STARTING_POINT;
                                replaced = true;
                            }
                            else
                            {
                                printLine[s] = HORIZONTAL_WALL;
                                replaced = true;
                            }
                        }
                        //Find left side walls
                        if (s != map[i].Length - 1)
                        {
                            if (!replaced &&
                            isReplaceable &&
                            (line[s + 1] == PADDING_CHAR_O || line[s + 1] == STARTING_POINT))
                            {
                                printLine[s] = VERTICAL_WALL;
                                replaced = true;
                            }
                        }
                        //Find right side wall
                        if (s != 0)
                        {
                            if (!replaced &&
                            isReplaceable &&
                            line[s - 1] == PADDING_CHAR_O)
                            {
                                printLine[s] = VERTICAL_WALL;
                                replaced = true;
                            }
                        }
                        if (!replaced)
                        {
                            if (!isReplaceable)
                            {
                                printLine[s] = PADDING_CHAR_X;
                            }
                            else
                            {
                                printLine[s] = EMPTY;
                            }
                        }
                    }
                    Console.Write(printLine);
                    Console.Write("\n");
                }
                else if (i == (this.map.Length - 1))
                {
                    for (int s = 0; s < map[i].Length; s++)
                    {
                        bool replaced = false;
                        bool isReplaceable = line[s] == PADDING_CHAR_X;
                        //Find Bottom wall
                        if (isReplaceable &&
                            (map[i - 1].ToCharArray()[s] == PADDING_CHAR_O ||
                            map[i - 1].ToCharArray()[s] == STARTING_POINT))
                        {
                            printLine[s] = HORIZONTAL_WALL;
                            replaced = true;
                        }
                        //Find left side walls
                        if (s != map[i].Length - 1)
                        {
                            if (!replaced &&
                            isReplaceable &&
                            (line[s + 1] == PADDING_CHAR_O || line[s + 1] == STARTING_POINT))
                            {
                                printLine[s] = VERTICAL_WALL;
                                replaced = true;
                            }
                        }
                        //Find right side wall
                        if (s != 0)
                        {
                            if (!replaced &&
                            isReplaceable &&
                            line[s - 1] == PADDING_CHAR_O)
                            {
                                printLine[s] = VERTICAL_WALL;
                                replaced = true;
                            }
                        }
                        if (!replaced)
                        {
                            if (!isReplaceable)
                            {
                                printLine[s] = PADDING_CHAR_X;
                            }
                            else
                            {
                                printLine[s] = EMPTY;
                            }
                        }
                    }
                    Console.Write(printLine);
                    Console.Write("\n");

                }
                else
                {
                    for (int s = 0; s < map[i].Length; s++)
                    {
                        bool replaced = false;
                        bool isReplaceable = line[s] == PADDING_CHAR_X;
                        //Find Top wall
                        if (isReplaceable &&
                            (map[i + 1].ToCharArray()[s] == PADDING_CHAR_O ||
                            map[i + 1].ToCharArray()[s] == STARTING_POINT))
                        {
                            if (map[i + 1].ToCharArray()[s] == STARTING_POINT)
                            {
                                printLine[s] = STARTING_POINT;
                                replaced = true;
                            }
                            else
                            {
                                printLine[s] = HORIZONTAL_WALL;
                                replaced = true;
                            }
                        }
                        //Find Bottom wall
                        if (isReplaceable &&
                            (map[i - 1].ToCharArray()[s] == PADDING_CHAR_O ||
                            map[i - 1].ToCharArray()[s] == STARTING_POINT))
                        {
                            printLine[s] = HORIZONTAL_WALL;
                            replaced = true;
                        }
                        //Find left side walls
                        if (s != map[i].Length - 1)
                        {
                            if (!replaced &&
                            isReplaceable &&
                            (line[s + 1] == PADDING_CHAR_O || line[s + 1] == STARTING_POINT))
                            {
                                printLine[s] = VERTICAL_WALL;
                                replaced = true;
                            }
                        }
                        //Find right side wall
                        if (s != 0)
                        {
                            if (!replaced &&
                            isReplaceable &&
                            line[s - 1] == PADDING_CHAR_O)
                            {
                                printLine[s] = VERTICAL_WALL;
                                replaced = true;
                            }
                        }
                        if (!replaced)
                        {
                            if (!isReplaceable)
                            {
                                printLine[s] = PADDING_CHAR_X;
                            }
                            else
                            {
                                printLine[s] = EMPTY;
                            }
                        }
                    }
                    Console.Write(printLine);
                    Console.Write("\n");
                }
            }
        }
    }
}
