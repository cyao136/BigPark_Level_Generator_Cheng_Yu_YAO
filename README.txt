README

Project Title: BigPark_Level_Generator_Cheng_Yu_YAO
Author: Cheng Yu Yao
Description: A submission for BigPark programming test.

Files included:
- README.txt
- AssemblyInfo.cs
- Level.cs
- Room.cs
- Main.cs
- BigPark_Level_Generator_Cheng_Yu_YAO.csproj

Program written using only System extensions

Main.cs:

The core of the program, it serves as an UI class where it displays messages to the user and acquires
input from the user.
Has a reset option for users to re-seed a level.
The seed and rooms are also generated in this class.

Seed:

Width and Height of the level is restricted to 5-249 (inclusive) for room generation and
consistency with the seed. (can be increased by change the size of the seed).

NNNNNNNNNXXXYYY = 15 length
N = randomized number
X = (width * 4) - 3
Y = (height * 4) - 3
Seed uses a hash-like function to determine where a room is generated

Room.cs:

Has a starting and ending position in int X and int Y format. Vector was not available.
Thus, had to use 4 variables for the position of the room.

Level.cs:

Has a list of rooms in the level. When created, it adds rooms by mapping it to 
an empty map full of 'X' using 'O' as the taken position of the room. (method addRoom) It, then, calculates
whether there are any connections ('O') around the room it just put down. If there aren't,
the method calls addPath to add a path from the current Room being added to the previously added Room.
Paths are just 2 rooms that connects the top left corner of two room.
After the rooms have been added, the starting point ('*') is added to the map by replacing the first 'O' it sees.

Print method:

The print method is within the Level class. When called, it takes the map of the level which is full of 'X' and 'O'
with a starting point '*', and transforms it into a more readable map to output in the console.