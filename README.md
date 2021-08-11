Zoophobia

Video Available:
https://youtu.be/gAd0Nb8dP8Y


1. Introduction
The game is built by using the assets given from the Unity Learn - Junior Programmer, and some self-made textures to match the game’s UI. 
The game’s gameplay is self-designed and is coded from scratch using the knowledge learned from the Unity Learn – Junior Programmer Pathway.  
This game is made to showcase my skills developed in Game Development. The project is free to browse to see how it is coded. The project is available on GitHub.

2. Concept
Run away from the animals they don’t like you and they will bite you. While running away you should dodge all the incoming obstacles, 
the animals are so much on your tail they will just run into any obstacle. If you’re lucky, you can pick up some food to restore your health, 
bombs or balls to throw at the at the animals, a shield to run through anything or double your diamond coins you pick up.

2.1.	Player control
The player controls a character that will continuously run from left to right in a side view game, the player has 3 lanes that he 
can move up and down with, using the arrow keys and can also run faster and slower using the arrow keys. The player can also jump using the space bar.

2.2.	Basic Gameplay
In the game obstacles will appear, moving from right to left on the screen, the player has the decision to jump over the obstacle or to move away from it. 
The obstacles must be used to destroy the enemies chasing the player. The player and the enemies both have health, when the player or the enemy hits an 
object the health would be reduced. The score will increase as the player collects the coins or kill an enemy.

2.3.	Sound & Effects
No sound was added in the project.
Particle effects is used when running into obstacles, being bitten, when bomb explodes, when enemies run into obstacles and when bosses use their ability. 

2.4.	Gameplay Mechanics
As the game progresses, more enemies spawn, making it hard to survive, after killing a few enemies a boss will spawn after the boss has been killed, 
the game has ended.

2.5.	User Interface

2.5.1. Main menu
  New Game > Enter Name > Character select > Level select
  High Score	
  Options > Info + Controls + Back
  Exit
2.5.2. In-game UI
  Time
  Score
  Player Health + Boss Health
  Current Powerup
2.5.3. Pause Menu
  Continue
  Options > Info + Controls + Back
  Exit to Main Menu
2.5.4. End Game
  Play Again
  Exit to Main Menu

2.6.	Levels 

Level 1 - City:
  Dogs chasing player
  Dogging Barriers and cars
  Pug Boss

Level 2 - Town:
  Farm animals chasing player
  Dogging Barrels, Crates and Cars

Level 3 - Nature:
  Forest animals chasing player
  Dogging Rocks, Tree logs, Cars and Busses 

2.7.	Powerups
  Dropping balls that does damage to the animals.
  Shield that will grant invulnerability (star)
  Double Coins
  Heal 
  Bomb




3.	Programming
  Using Singleton pattern to create UIManager and GameManager
  Using State Pattern to control the state of the game (Main menu, In Game, Pause, Game End)
  Using Object Pooling to choose Characters and to spawn enemies, powerups and objects

3.1.	Scripts Layout
  AI
    EnemyMain – IDamageable	
      Follow Player
      When dead add score to player	
      Attack when close
      Run closer to player, sometimes
    BasicEnemy (inherited from EnemyMain)
      Will be counted to maintain amount spawn
      And how many have been killed
    BossMain (inherited from EnemyMain)
      Control HealthBar
    BossPugDog (inherited from BossMain)
      Stink breath (damage per second)
    BossChicken (inherited from BossMain)
      Fire breath (damage per second)
    BossRacoon (inherited from BossMain)
      Black breath (damage per second)
      
  Interfaces
    IDamageable
      Damage (damageTaken, damageType, damageLocation)
   
  Lists
    CharacterList
    HighScoreList
    LevelList
      LevelSelectList
      BackgroundList
      GroundList
    SpawnList
      Enemies
      Obstacles
      Bosses

  Obstacles
    ObstacleHitCollider
      When hit it does damage and gets disables
    Obstacles - IEndGameObserver	
      Moves left, and speed changes when game progresses

  Player
    PlayerController  - IDamageable
      Player movement
      Control Powerups 		

  Pickups
    BombDrop 
      When the player picks up a bomb
      The bomb drops and waiting to hit an enemy
      Then will spawn an explosion
    Explosion
      Find all Enemies in radius and do damage
    Pickups 
      Contain pickup type from the Enums
      Rotate the pickups
      
  System
    SpawnManager
      Spawn Objects + Enemies
    BackgroundRepeat
      Moves the background + ground
      Sets to new position when a curtain position is reached
    GameManager
      Control the Load and unloading of scenes
      Control the game states
      Control the gameplay
      Load/Saves high scores
      Stores the player’s name, character selected and level
      Create + Destroy Managers
    LoadSave
      Contains HighScoreList
      Save this script as json
      
  UI
  UIManager
    Contains all states, when to show which UI. Always run-in background (singleton)
    Create a prefab of all the UI’s that will be controlled with the UIManager
  InGameUI
    Contains Time, score and current powerup, Player’s Health, Boss Health
  MainMenu
    Contains all the buttons + functions
  PauseMenu
  Contains all the buttons + functions
  EndGameMenu
  Contains all the buttons + functions
    GameOverMenu
  Contains all the buttons + functions
    HighScoreMenu
      Shows the highscores
    OptionsMenu
  Contains all the buttons + functions
  
  Utils
    Enums
      DamageType
      Particles To Spawn
      Pickups
    Singleton

3.2.	Events:
  When damage has been delt to player or boss

3.3.	Statics:
  Level Selected (GameManager)
  Speed of the background movement (GameManager)

3.4.	Save: 
  High scores: Player Name, score, level, time
