# Draughts
#Story
Your best friend Tom is board games lover, and he plays checkers all the time like a pro! Recently he is really into digital versions of his favorite games, unfortunately he cannot find any implementation of the Polish Draughts. Give Tom your console version of this game.

#What are you going to learn?
You will learn and practice how to do the following things:

variables,
methods,
loops and conditionals,
classes and instances,
print formatting,
user input validation,
handle edge cases,
access modifiers,
OOP.
#Tasks
Board

There is a class Board which represents square board for Polish draughts.
There is a parameter n in the constructor describing the length of the side of the square, size should be an integer between 10 and 20 provided from user input.
There is a 2D array Pawn[,] Fields which represents fields on a board. Each field can be null (empty) or a Pawn instance.
Pawns are created and placed only at every second field when the board is initialized. Their number is determined by board size as a 2 * n.
There is ToString() method that overrides built-in method. This method should mark rows as a numbers and columns as a letters.
There is RemovePawn() method that removes pawn with given position from.
There is MovePawn() method that moves pawn with given position from one field to another.
Pawn

There is a Pawn class.
There is a property bool IsWhite that returns true if the pawn is white and false if it's black.
There is a property (int x, int y) Coordinates that represents pawn coordinates on a board.
[Extra] There is a property bool IsCrowned that returns true if given pawn is crowned.
Pawn contains a method that validates given move (checks whether it's according to the game's rules) before making it.
[Extra] Pawn can check if it can make multiple jumps according to the rules.
Game

There is a class Game which contains all game logic and actions.
There is a method Start() that starts whole game between players.
There is a method Round() that determines one round actions i.e. check who plays next or is there a winner yet.
There is a method that checks if starting position from user input is a valid pawn and if ending position is within board boundaries. If so, it calls TryToMakeMove() on pawn instance.
There is a method CheckForWinner() that checks after each round is there an a winner.
Method CheckForWinner() checks also for draws.
Optional Tasks

OPTIONAL Try to implement singleton pattern. Try to implement additional features such as crowning, multi-jumping and checking for draw.
Board is a singleton so it can have only one instance.
Crowned feature is implemented. There is a property on Pawn called bool IsCrowned that returns true if given pawn is crowned and it implements crowning rules.
Pawn can check if it can make multiple jumps according to the rules.
