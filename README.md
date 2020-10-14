# Memory Card Game W\AI

# Goals:
• Implementation of work with object-oriented departments and programming
• Work with arrays / collections departments
• Using the string class
• Use of external Dll (assembly)
# Knowledge required:
• Work with arrays / collections departments
Access Modifiers, Constructors, Properties, Indexers
• Using the string class
• Use of external Dll (assembly)

# The Exercise:
implement the memory game (relatively primitively) when the console is the user interface.
The program will first ask the user for the desired board size (number of columns and number of rows).
The initial state will be a symmetrically sized panel of X rows of columns at the request of the user.
The program will not allow the selection of an odd number of squares (e.g. 5X5).
At each stage the user will be asked to select a slot in the board to reveal the signal that is in the above box.
At this point the user will be asked to select another box in which he thinks the additional letter T is located. The slot
That you selected will be exposed in a similar way to where the first slot was exposed (the board will be drawn again)

# the progress of the game
1. The user is asked to enter his name (he is the first player).
2. The user is asked to decide whether the game is for two players or against the computer.
3. If he chooses the option of two players, he is asked to enter the name of the player
The second.
4. The user is asked to determine the height and width of the board (minimum 4x4 maximum 6x6 and Double amount of squares required!).
5. At the beginning of the rotation the matrix is empty and will be drawn in this way (in the case of 6x4).
6 Each player in turn will be asked to choose the first slot he wants to reveal. If the selection is invalid a suitable message will be given and so on until it is selected a Valid slot (valid slot is a slot hidden within the board boundaries. Any other input is considered invalid And a notice must be issued explaining the reason for its illegality).
7. Once the player has selected a square, clear the screen, draw the board again as it is with the exposure of the relevant box
8. At this point the player must select the slot that he considers to be a match, according to the instructions in sections 6 and 7-9. If the user has managed to reveal a matching pair, the pair will remain exposed until the end of the game, the player will receive a point, and the queue will remain with him for new input under the previous section (i.e. returns to section 6).
10. If the user reveals an mismatched pair, the mismatched pair will remain exposed for 2 seconds and after 2 seconds the two slots selected will be empty again and the turn goes to the other player for input.
(I.e. returns to section 6).
11. Board Mode After the first player selects squares E3 and: B4
12. When the last pair is revealed, the game is over and a victory is determined for the player with the greater number of exposures (an appropriate message will be issued that includes the status of the points between the players. Each player receives points according to the number of his exposures).
13. After the end of a game, the user will be asked if he wants to start another round. If so, return to section 4.
If not, a message will be displayed and the program will end.
14. Starting from section 6, at each stage the game can be retired by selecting "Q" instead of a square
(General retirement from the software).
