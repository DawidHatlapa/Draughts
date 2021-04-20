using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace polish_draughts_csharp_Boner1975
{
    public class Player
    {
        public string playerName;
        public char playerSymbol;
        private bool isAi = false;
        public int itemNumber;
        public int startItems;
        public string startCoordinates;
        public string stopCoordinates;
        public string startTakeCoordinates;
        public string stopTakeCoordinates;
        public int row;
        public int col;
        public int i;
        public int j;
        bool isTakeMade = false;

        public Player() // Construktor for normal Player
        {
            this.playerName = CreateName();
            this.playerSymbol = CreateSymbol();
        }

        public Player(string aiName, char playerSymbol = 'c')  // Constructor for AI
        {
            this.playerName = aiName;
            this.playerSymbol = CreateSymbol(playerSymbol);
            this.isAi = true;
        }

        private static string CreateName() // Function for normal Player
        {
            Comunicator.Talking("Lets create your nickname.");
            string name = Comunicator.GetWord("Your nickname - ");
            Console.Clear();
            Comunicator.Talking("Hi " + name + "!");
            return name;
        }

        private static char CreateSymbol() // Function for normal Player
        {
            Comunicator.Talking("Lets create your symbol.");
            Comunicator.Talking("Use only one for example \"x\", \"y\" or \"o\".");
            char symbol = Char.Parse(Comunicator.GetWord("Your symbol - "));
            Console.Clear();
            return symbol;

        }

        private static char CreateSymbol(char playerSymbol) // Function for AI
        {
            if (playerSymbol == 'c') return 'x';
            else return 'c';
        }

        public Board Move(Board board, Game game)
        {
            if (this.isAi) Thread.Sleep(1000);
            Console.Clear();
            Interface.DrawBoard(board, game, this);
            if (this.isAi) board = AIMove(board, game);
            else board = PlayerMove(board, game);

            return board;
        }

        public Board PlayerMove(Board board, Game game)  // Function for normal Player
        {
            if (checkForPawnToTake(board).Count != 0)
            {
                bool isValidPick = false;
                var coordinatesDictionary = checkForPawnToTake(board);
                var listOfValues = new List<string>();
                var toTakeCoordinates = new List<string>();
                foreach (string value in coordinatesDictionary.Values)
                {
                    if (!listOfValues.Contains(value))
                    {
                        listOfValues.Add(value);
                    }
                }
                foreach (string key in coordinatesDictionary.Keys)
                {
                    toTakeCoordinates.Add(key);
                }
                while (!isValidPick)
                {
                    Console.Write("You have pawn to take. Please choose from coordinates: ");
                    foreach (string element in listOfValues)
                    {
                        Console.Write(element + "  ");
                    }
                    Console.WriteLine();
                    if (checkForCordsToTake(coordinatesDictionary).Count == 0)
                    {
                        Console.Write("Wrong choice.");
                    }
                    else
                    {
                        isValidPick = !isValidPick;
                    }
                }
                //toTakeCoordinates = checkForCordsToTake(coordinatesDictionary);
                int chosenPawnToMakeATakeRow = stringToIntTransformation(startTakeCoordinates)[0];
                int chosenPawnToMakeATakeCol = stringToIntTransformation(startTakeCoordinates)[1];
                string chosenPawnToMakeATake = board.pawn[chosenPawnToMakeATakeRow, chosenPawnToMakeATakeCol];

                bool isValidPlace = false;
                while (!isValidPlace)
                {
                    Console.Write("Where do you want to go? You can choose from coordinates: ");
                    if (listOfValues.Count == 1)
                    {
                        foreach (string element in toTakeCoordinates)
                        {
                            Console.Write(element + "  ");
                        }
                        Console.WriteLine();
                        if (checkForCordsToPlace(toTakeCoordinates) == null)
                        {
                            Console.Write("Wrong choice.");
                        }
                        else
                        {
                            isValidPlace = !isValidPlace;
                        }
                    }
                    else
                    {
                        string placeAfterTake = coordinatesDictionary.FirstOrDefault(x => x.Value == startTakeCoordinates).Key;
                        Console.Write(placeAfterTake);
                        Console.WriteLine();
                        if (checkForCordToPlace(placeAfterTake) == null)
                        {
                            Console.Write("Wrong choice.");
                        }
                        else
                        {
                            isValidPlace = !isValidPlace;
                        }
                    }
                }
                int placeAfterTakeRow = stringToIntTransformation(stopTakeCoordinates)[0];
                int placeAfterTakeCol = stringToIntTransformation(stopTakeCoordinates)[1];
                string placeTfterTake = board.pawn[placeAfterTakeRow, placeAfterTakeCol];
                board.pawn[chosenPawnToMakeATakeRow, chosenPawnToMakeATakeCol] = "_";
                board.pawn[placeAfterTakeRow, placeAfterTakeCol] = chosenPawnToMakeATake;
                board.pawn[(chosenPawnToMakeATakeRow + placeAfterTakeRow) / 2, (chosenPawnToMakeATakeCol + placeAfterTakeCol) / 2] = "_";

                if (this == game.firstPlayer)
                {
                    game.secondPlayer.itemNumber -= 1;
                }
                else
                {
                    game.firstPlayer.itemNumber -= 1;
                }


                Interface.DrawBoard(board, game, this);
                isTakeMade = !isTakeMade;
                this.PlayerMove(board, game);
            }
            else
            if (!isTakeMade)
            {
                bool isValidMove = false;
                while (!isValidMove)
                {
                    var pickCoordinatesList = this.getPickCoordinates();
                    while (!this.checkForPawn(board, pickCoordinatesList))
                    {
                        Comunicator.Talking("This is not your pawn, please choose cords again ");
                        pickCoordinatesList = this.getPickCoordinates();
                        this.checkForPawn(board, pickCoordinatesList);
                    }

                    if (!this.checkForMove(board, pickCoordinatesList))
                    {
                        Comunicator.Talking("There is no valid move for your pawn, please choose different one ");
                    }
                    else
                    {
                        isValidMove = !isValidMove;
                    }
                }
                string chosenPawn = board.pawn[board.row, board.col];
                bool isValidChoice = false;
                while (!isValidChoice)
                {
                    var placeCoordinatesList = this.getPlaceCoordinates();
                    while (!(this.checkForDistance(board, placeCoordinatesList) && this.checkForEmpty(board, placeCoordinatesList)))
                    {
                        Comunicator.Talking("You tried to move to forbidden place, either place is taken or is excluded from game. ");
                        placeCoordinatesList = this.getPlaceCoordinates();
                        this.checkForDistance(board, placeCoordinatesList);
                        this.checkForEmpty(board, placeCoordinatesList);
                    }
                    isValidChoice = !isValidChoice;
                    row = placeCoordinatesList[0];
                    col = placeCoordinatesList[1];
                }

                board.pawn[board.row, board.col] = "_";
                board.pawn[row, col] = chosenPawn;
                Interface.DrawBoard(board, game,this);
            }
            isTakeMade = false;
            return board;
        }

        public List<int> getPickCoordinates()
        {
            bool isValidEntry = false;
            var list = new List<int>();
            while (!isValidEntry)
            {
                startCoordinates = Comunicator.GetWord("Provide coordinates for pawn to be moved from in a1/A1 format: ");
                if (startCoordinates.Length == 2 && int.TryParse(startCoordinates[1].ToString(), out _) && Char.IsLetter(startCoordinates[0]))
                {
                    list.Add(int.Parse(startCoordinates[1].ToString()) - 1);
                    list.Add(((int)char.Parse(startCoordinates[0].ToString().ToUpper())) - 65);
                    isValidEntry = true;
                }
                else if (startCoordinates.Length == 3 && int.TryParse((startCoordinates[1].ToString() + startCoordinates[2].ToString()), out _) && Char.IsLetter(startCoordinates[0]))
                {
                    list.Add(int.Parse(startCoordinates[1].ToString() + startCoordinates[2].ToString()));
                    list.Add(((int)char.Parse(startCoordinates[0].ToString().ToUpper())) - 65);
                    isValidEntry = true;
                }
                else
                {
                    Console.WriteLine("Wrong format, try again.");
                }
            }
            return list;
        }

        public List<int> getPlaceCoordinates()
        {
            bool isValidEntry = false;
            var list = new List<int>();
            while (!isValidEntry)
            {
                stopCoordinates = Comunicator.GetWord("Provide coordinates for pawn to be moved to in a1/A1 format: ");
                if (stopCoordinates.Length == 2 && int.TryParse(stopCoordinates[1].ToString(), out _) && Char.IsLetter(stopCoordinates[0]))
                {
                    list.Add(int.Parse(stopCoordinates[1].ToString()) - 1);
                    list.Add(((int)char.Parse(stopCoordinates[0].ToString().ToUpper())) - 65);
                    isValidEntry = true;
                }
                else if (stopCoordinates.Length == 3 && int.TryParse((stopCoordinates[1].ToString() + stopCoordinates[2].ToString()), out _) && Char.IsLetter(stopCoordinates[0]))
                {
                    list.Add(int.Parse(stopCoordinates[1].ToString() + stopCoordinates[2].ToString()));
                    list.Add(((int)char.Parse(stopCoordinates[0].ToString().ToUpper())) - 65);
                    isValidEntry = true;
                }
                else
                {
                    Console.WriteLine("Wrong format, try again.");
                }
            }
            return list;
        }

        public bool checkForPawn(Board board, List<int> pickCoordinatesList)
        {
            string chosenPawn = board.pawn[pickCoordinatesList[0], pickCoordinatesList[1]];
            if (chosenPawn == playerSymbol.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkForMove(Board board, List<int> pickCoordinatesList)
        {
            board.row = pickCoordinatesList[0];
            board.col = pickCoordinatesList[1];
            if ((board.row > 0 && board.row <= board.boardSize - 2) && (board.col > 0 && board.col <= board.boardSize - 2))
            {
                if (board.pawn[board.row - 1, board.col - 1] == "_" || board.pawn[board.row - 1, board.col + 1] == "_"
                    || board.pawn[board.row + 1, board.col - 1] == "_" || board.pawn[board.row + 1, board.col + 1] == "_")
                {
                    return true;
                }
            }
            else if (board.row == 0 && board.col == 0)
            {
                if (board.pawn[board.row + 1, board.col + 1] == "_")
                {
                    return true;
                }
            }
            else if (board.row == board.boardSize - 1 && board.col == board.boardSize - 1)
            {
                if (board.pawn[board.row - 1, board.col - 1] == "_")
                {
                    return true;
                }
            }
            else if (board.row == 0 && (board.col > 0 && board.col <= board.boardSize - 2))
            {
                if (board.pawn[board.row + 1, board.col - 1] == "_" || board.pawn[board.row + 1, board.col + 1] == "_")
                {
                    return true;
                }
            }
            else if (board.row == board.boardSize - 1 && (board.col > 0 && board.col <= board.boardSize - 2))
            {
                if (board.pawn[board.row - 1, board.col - 1] == "_" || board.pawn[board.row - 1, board.col + 1] == "_")
                {
                    return true;
                }
            }
            else if (board.col == 0 && (board.row > 0 && board.row <= board.boardSize - 2))
            {
                if (board.pawn[board.row - 1, board.col + 1] == "_" || board.pawn[board.row + 1, board.col + 1] == "_")
                {
                    return true;
                }
            }
            else if (board.col == board.boardSize - 1 && (board.row > 0 && board.row <= board.boardSize - 2))
            {
                if (board.pawn[board.row - 1, board.col - 1] == "_" || board.pawn[board.row + 1, board.col - 1] == "_")
                {
                    return true;
                }
            }
            return false;   
        }

        public bool checkForDistance(Board board, List<int> placeCoordinatesList)
        {
            int pickX = board.col, pickY = board.row, placeX = placeCoordinatesList[1], placeY = placeCoordinatesList[0];
            if (Math.Abs(placeX - pickX) == 1 && Math.Abs(placeY - pickY) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkForEmpty(Board board, List<int> placeCoordinatesList)
        {
            if (board.pawn[placeCoordinatesList[0], placeCoordinatesList[1]] == "_")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<string, string> checkForPawnToTake(Board board)
        {
            var dict = new Dictionary<string, string>();
            for (i = 0; i < board.boardSize; i++)
            {
                for (j = 0; j < board.boardSize; j++)
                {
                    if (board.pawn[i, j] == playerSymbol.ToString())
                    {
                        if (i <= 1 && j <= 1)
                        {
                            if (checkRightDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if (i <= 1 && (j > 1 && j < board.boardSize - 2))
                        {
                            if (checkLeftDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkRightDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if (i <= 1 && j >= board.boardSize - 2)
                        {
                            if (checkLeftDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if ((i > 1 && i < board.boardSize - 2) && j <= 1)
                        {
                            if (checkRightUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkRightDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if ((i > 1 && i < board.boardSize - 2) && (j > 1 && j < board.boardSize - 2))
                        {
                            if (checkRightUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkRightDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkLeftUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkLeftDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if ((i > 1 && i < board.boardSize - 2) && (j >= board.boardSize - 2))
                        {
                            if (checkLeftUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkLeftDownDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i + 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if (i >= board.boardSize - 2 && j <= 1)
                        {
                            if (checkRightUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if ((i >= board.boardSize - 2) && (j > 1 && j < board.boardSize - 2))
                        {
                            if (checkLeftUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                            if (checkRightUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j + 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                        if (i >= board.boardSize - 2 && j >= board.boardSize - 2)
                        {
                            if (checkLeftUpDirection(board))
                            {
                                dict.Add(coordinatesTransformation(i - 2, j - 2).ToUpper(), coordinatesTransformation(i, j).ToUpper());
                            }
                        }
                    }
                }
            }
            return dict;
        }

        public bool checkRightDownDirection(Board board)
        {
            if ((board.pawn[i + 1, j + 1] != playerSymbol.ToString() && board.pawn[i + 1, j + 1] != "_") && board.pawn[i + 2, j + 2] == "_")
            {
                return true;
            }
            return false;
        }

        public bool checkLeftDownDirection(Board board)
        {
            if ((board.pawn[i + 1, j - 1] != playerSymbol.ToString() && board.pawn[i + 1, j - 1] != "_") && board.pawn[i + 2, j - 2] == "_")
            {
                return true;
            }
            return false;
        }

        public bool checkRightUpDirection(Board board)
        {
            if ((board.pawn[i - 1, j + 1] != playerSymbol.ToString() && board.pawn[i - 1, j + 1] != "_") && board.pawn[i - 2, j + 2] == "_")
            {
                return true;
            }
            return false;
        }

        public bool checkLeftUpDirection(Board board)
        {
            if ((board.pawn[i - 1, j - 1] != playerSymbol.ToString() && board.pawn[i - 1, j - 1] != "_") && board.pawn[i - 2, j - 2] == "_")
            {
                return true;
            }
            return false;
        }

        public string coordinatesTransformation(int i, int j)
        {
            string stringRow = ((char)(j + 65)).ToString().ToUpper();
            string stringCol = (i + 1).ToString();
            return (stringRow + stringCol);
        }

        public List<int> stringToIntTransformation(string coordinates)
        {
            var list = new List<int>();
            if (coordinates.Length == 2)
            {
                list.Add(int.Parse(coordinates[1].ToString()) - 1);
            }
            else
            {
                list.Add(int.Parse(coordinates[1].ToString() + coordinates[2].ToString()));
            }
            list.Add(((int)char.Parse(coordinates[0].ToString().ToUpper())) - 65);
            return list;
        }

        public List<string> checkForCordsToTake(Dictionary<string, string> coordinatesToTake)
        {
            var list = new List<string>();
            startTakeCoordinates = Comunicator.GetWord("Please choose your pawn to make a take: ").ToUpper();
            if (coordinatesToTake.ContainsValue(startTakeCoordinates))
            {
                foreach(string value in coordinatesToTake.Values)
                {
                    list.Add(coordinatesToTake.FirstOrDefault(x => x.Value == value).Key);
                }
            }
            return list;
        }

        public string checkForCordsToPlace(List<string> toTakeCoordinates)
        {
            stopTakeCoordinates = Comunicator.GetWord("Please provide coordinates: ").ToUpper();
            if (toTakeCoordinates.Contains(stopTakeCoordinates))
            {
                return stopTakeCoordinates;
            }
            else
            {
                return null;
            }
        }

        public string checkForCordToPlace(string placeAfterTake)
        {
            stopTakeCoordinates = Comunicator.GetWord("Please provide coordinates: ").ToUpper();
            if (placeAfterTake == stopTakeCoordinates)
            {
                return stopTakeCoordinates;
            }
            else
            {
                return null;
            }
        }

        /*
        public bool checkForPawnToTake(Board board, Player firstPlayer, Player secondPlayer)
        {
            for (i = 0; i < board.boardSize; i++)
            {
                for (j = 0; j < board.boardSize; j++)
                {
                    if (board.pawn[i, j] == firstPlayer.playerSymbol.ToString())
                    {
                        if (i <= 1 && j <= 1)
                        {
                            if(checkRightDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if (i <= 1 && (j > 1 && j <= board.boardSize - 2))
                        {
                            if (checkLeftDownDirection(board, firstPlayer, secondPlayer) || checkRightDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if (i <= 1 && j > board.boardSize - 2)
                        {
                            if (checkLeftDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if ((i > 1 && i <= board.boardSize - 2) && j <= 1)
                        {
                            if (checkRightUpDirection(board, firstPlayer, secondPlayer) || checkRightDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if ((i > 1 && i <= board.boardSize - 2) && (j > 1 && j <= board.boardSize - 2))
                        {
                            if (checkRightUpDirection(board, firstPlayer, secondPlayer) || checkRightDownDirection(board, firstPlayer, secondPlayer)
                                || checkLeftUpDirection(board, firstPlayer, secondPlayer) || checkLeftDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    else if (board.pawn[i, j] == secondPlayer.playerSymbol.ToString())
                    {
                        if (i <= 1 && j <= 1)
                        {
                            if (checkRightDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if (i <= 1 && (j > 1 && j <= board.boardSize - 2))
                        {
                            if (checkLeftDownDirection(board, firstPlayer, secondPlayer) || checkRightDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if (i <= 1 && j > board.boardSize - 2)
                        {
                            if (checkLeftDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if ((i > 1 && i <= board.boardSize - 2) && j <= 1)
                        {
                            if (checkRightUpDirection(board, firstPlayer, secondPlayer) || checkRightDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                        else if ((i > 1 && i <= board.boardSize - 2) && (j > 1 && j <= board.boardSize - 2))
                        {
                            if (checkRightUpDirection(board, firstPlayer, secondPlayer) || checkRightDownDirection(board, firstPlayer, secondPlayer)
                                || checkLeftUpDirection(board, firstPlayer, secondPlayer) || checkLeftDownDirection(board, firstPlayer, secondPlayer))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return false;
        }
        */

        /*
        public bool checkRightDownDirection(Board board, Player firstPlayer, Player secondPlayer)
        {
            if (board.pawn[i, j] == firstPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i + 1, j + 1] == secondPlayer.playerSymbol.ToString() && board.pawn[i + 2, j + 2] == "_")
                {
                    return true;
                }
            }
            else if (board.pawn[i, j] == secondPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i + 1, j + 1] == firstPlayer.playerSymbol.ToString() && board.pawn[i + 2, j + 2] == "_")
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkLeftDownDirection(Board board, Player firstPlayer, Player secondPlayer)
        {
            if (board.pawn[i, j] == firstPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i + 1, j - 1] == secondPlayer.playerSymbol.ToString() && board.pawn[i + 2, j - 2] == "_")
                {
                    return true;
                }
            }
            else if (board.pawn[i, j] == secondPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i + 1, j - 1] == firstPlayer.playerSymbol.ToString() && board.pawn[i + 2, j - 2] == "_")
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkRightUpDirection(Board board, Player firstPlayer, Player secondPlayer)
        {
            if (board.pawn[i, j] == firstPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i - 1, j + 1] == secondPlayer.playerSymbol.ToString() && board.pawn[i - 2, j + 2] == "_")
                {
                    return true;
                }
            }
            else if (board.pawn[i, j] == secondPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i - 1, j + 1] == firstPlayer.playerSymbol.ToString() && board.pawn[i - 2, j + 2] == "_")
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkLeftUpDirection(Board board, Player firstPlayer, Player secondPlayer)
        {
            if (board.pawn[i, j] == firstPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i - 1, j - 1] == secondPlayer.playerSymbol.ToString() && board.pawn[i - 2, j - 2] == "_")
                {
                    return true;
                }
            }
            else if (board.pawn[i, j] == secondPlayer.playerSymbol.ToString())
            {
                if (board.pawn[i - 1, j - 1] == firstPlayer.playerSymbol.ToString() && board.pawn[i - 2, j - 2] == "_")
                {
                    return true;
                }
            }
            return false;
        }
        */

        private Board AIMove(Board board, Game game)  // Function for AI
        {
            Dictionary<string, List<string>> frontMoves = AvailableFrontAI(board, game);
            Dictionary<string, List<string>> destroyMoves = new Dictionary<string, List<string>>();

            return MakeBetterMoveAI(frontMoves, destroyMoves, board);
        }

        private Board MakeBetterMoveAI(Dictionary<string, List<string>> frontMoves, Dictionary<string, List<string>> destroyMoves, Board board)
        {
            if (destroyMoves.Count > 0)
            {
                board = MakedestroyMoveAI(destroyMoves, board);
            }
            else if (frontMoves.Count > 0)
            {
                board = MakeMoveAI(frontMoves, board);
            }

            return board;
        }

        private Board MakedestroyMoveAI(Dictionary<string, List<string>> destroyMoves, Board board)
        {
            Random random = new Random();

            List<string> elemntList = new List<string>(destroyMoves.Keys);
            string elementToMove = elemntList[random.Next(elemntList.Count)];
            Dictionary<int, int> elementToMoveCoordinate = ConvertStringToCoordinate(elementToMove);

            List<string> moves = destroyMoves[elementToMove];
            string move = moves[random.Next(moves.Count)];
            Dictionary<int, int> moveCoodinate = ConvertStringToCoordinate(move);
            return ChangeBoardWithAIDestroy(board, elementToMoveCoordinate, moveCoodinate);
        }

        private Board MakeMoveAI(Dictionary<string, List<string>> moveAndElements, Board board)
        {
            Random random = new Random();

            List<string> elemntList = new List<string>(moveAndElements.Keys);
            string elementToMove = elemntList[random.Next(elemntList.Count)];
            Dictionary<int, int> elementToMoveCoordinate = ConvertStringToCoordinate(elementToMove);

            List<string> moves = moveAndElements[elementToMove];
            string move = moves[random.Next(moves.Count)];
            Dictionary<int, int> moveCoodinate = ConvertStringToCoordinate(move);
            return ChangeBoardWithAIMove(board, elementToMoveCoordinate, moveCoodinate);
        }

        private Board ChangeBoardWithAIDestroy(Board board, Dictionary<int, int> elementToMoveCoordinate, Dictionary<int, int> moveCoodinate)
        {
            board.pawn[elementToMoveCoordinate.Keys.First(), elementToMoveCoordinate.Values.First()] = "_";
            board.pawn[moveCoodinate.Keys.First(), moveCoodinate.Values.First()] = this.playerSymbol.ToString();
            return board;
        }

        private Board ChangeBoardWithAIMove(Board board, Dictionary<int, int> elementToMoveCoordinate, Dictionary<int, int> moveCoodinate)
        {
            board.pawn[elementToMoveCoordinate.Keys.First(), elementToMoveCoordinate.Values.First()] = "_";
            board.pawn[moveCoodinate.Keys.First(), moveCoodinate.Values.First()] = this.playerSymbol.ToString();
            return board;
        }

        private Dictionary<int, int> ConvertStringToCoordinate(string coordinateString)
        {
            Dictionary<int, int> coordinate = new Dictionary<int, int> 
            { 
                { int.Parse(coordinateString.ToCharArray()[0].ToString()), int.Parse(coordinateString.ToCharArray()[2].ToString()) } 
            };
            return coordinate;
        }

        private Dictionary<string, List<string>> AvailableFrontAI(Board board, Game game)
        {
            Dictionary<string, List<string>> coordinates = new Dictionary<string, List<string>>();
            for (int row = 0; row < board.boardSize; row++)
            {
                for (int col = 0; col< board.boardSize; col++)
                {
                    if (board.pawn[row, col] == this.playerSymbol.ToString())
                    {
                        List<string> availableFrontMovesAI = AvailableFrontMoveAI(row, col, board.pawn, game);
                        if (availableFrontMovesAI.Count > 0)
                        {
                            coordinates.Add(row.ToString() + "," + col.ToString(), availableFrontMovesAI);
                        }
                    }
                }
            }
            return coordinates;
        }

        private List<string> AvailableFrontMoveAI(int row, int col, string[,] pawn, Game game)
        {
            List<string> moves = new List<string>();
            try
            {
                if (pawn[row + 1, col - 1] == "_" && this == game.firstPlayer)
                {
                    moves.Add((row + 1).ToString() + "," + (col - 1).ToString());
                }
                else if (pawn[row - 1, col - 1] == "_" && this == game.secondPlayer)
                {
                    moves.Add((row - 1).ToString() + "," + (col - 1).ToString());
                }
            }
            catch { }
            try
            {
                if (pawn[row + 1, col + 1] == "_" && this == game.firstPlayer)
                {
                    moves.Add((row + 1).ToString() + "," + (col + 1).ToString());
                }
                else if (pawn[row - 1, col + 1] == "_" && this == game.secondPlayer)
                {
                    moves.Add((row - 1).ToString() + "," + (col + 1).ToString());
                }
            }
            catch { }
            return moves;

        }
    }
}

/*
 _ _ _ _ _ _ _ _
|X|_|X|_|X|_|X|_|
|_|X|_|X|_|X|_|X|
|_|_|_|_|_|_|_|_|
|_|_|_|_|_|_|_|_|
|_|_|_|_|_|_|_|_|
|_|_|_|_|_|_|_|_|
|O|_|O|_|O|_|O|_|
|_|O|_|O|_|O|_|O|

*/
