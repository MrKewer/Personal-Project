Zoophobia<br/>
<br/>
Video Available:<br/>
https://youtu.be/gAd0Nb8dP8Y<br/>
<br/>
Play game in browser:<br/>
https://play.unity.com/mg/other/zoophobia<br/>
<br/>
<br/>
1.Introduction<br/>
The game is built by using the assets given from the Unity Learn - Junior Programmer, and some self-made textures to match the game’s UI. <br/>
The game’s gameplay is self-designed and is coded from scratch using the knowledge learned from the Unity Learn – Junior Programmer Pathway.  <br/>
This game is made to showcase my skills developed in Game Development. The project is free to browse to see how it is coded. The project is available on GitHub.<br/>
<br/>
2.Concept<br/>
Run away from the animals they don’t like you and they will bite you. While running away you should dodge all the incoming obstacles, <br/>
the animals are so much on your tail they will just run into any obstacle. If you’re lucky, you can pick up some food to restore your health, <br/>
bombs or balls to throw at the at the animals, a shield to run through anything or double your diamond coins you pick up.<br/>
<br/>
2.1.	Player control<br/>
The player controls a character that will continuously run from left to right in a side view game, the player has 3 lanes that he <br/>
can move up and down with, using the arrow keys and can also run faster and slower using the arrow keys. The player can also jump using the space bar.<br/>
<br/>
2.2.	Basic Gameplay<br/>
In the game obstacles will appear, moving from right to left on the screen, the player has the decision to jump over the obstacle or to move away from it. <br/>
The obstacles must be used to destroy the enemies chasing the player. The player and the enemies both have health, when the player or the enemy hits an <br/>
object the health would be reduced. The score will increase as the player collects the coins or kill an enemy.<br/>
<br/>
2.3.	Sound & Effects<br/>
No sound was added in the project.<br/>
Particle effects is used when running into obstacles, being bitten, when bomb explodes, when enemies run into obstacles and when bosses use their ability. <br/>
<br/>
2.4.	Gameplay Mechanics<br/>
As the game progresses, more enemies spawn, making it hard to survive, after killing a few enemies a boss will spawn after the boss has been killed, <br/>
the game has ended.<br/>
<br/>
2.5.	User Interface<br/>
2.5.1. Main menu<br/>
&emsp;  New Game > Enter Name > Character select > Level select<br/>
&emsp;  High Score	<br/>
&emsp;  Options > Info + Controls + Back<br/>
&emsp;  Exit<br/>
  <br/>
2.5.2. In-game UI<br/>
&emsp;  Time<br/>
&emsp;  Score<br/>
&emsp;  Player Health + Boss Health<br/>
&emsp;  Current Powerup<br/>
  <br/>
2.5.3. Pause Menu<br/>
&emsp;  Continue<br/>
&emsp;  Options > Info + Controls + Back<br/>
&emsp;  Exit to Main Menu<br/>
  <br/>
2.5.4. End Game<br/>
&emsp;  Play Again<br/>
&emsp;  Exit to Main Menu<br/>
<br/>
2.6.	Levels <br/>
Level 1 - City:<br/>
&emsp;  Dogs chasing player<br/>
&emsp;  Dogging Barriers and cars<br/>
&emsp;  Pug Boss<br/>
<br/>
Level 2 - Town:<br/>
&emsp;  Farm animals chasing player<br/>
&emsp;  Dogging Barrels, Crates and Cars<br/>
<br/>
Level 3 - Nature:<br/>
&emsp;  Forest animals chasing player<br/>
&emsp;  Dogging Rocks, Tree logs, Cars and Busses <br/>
<br/>
2.7.	Powerups<br/>
&emsp;  Dropping balls that does damage to the animals.<br/>
&emsp;  Shield that will grant invulnerability (star)<br/>
&emsp;  Double Coins<br/>
&emsp;  Heal <br/>
&emsp;  Bomb<br/>
<br/>
3.	Programming<br/>
&emsp;  Using Singleton pattern to create UIManager and GameManager<br/>
&emsp;  Using State Pattern to control the state of the game (Main menu, In Game, Pause, Game End)<br/>
&emsp;  Using Object Pooling to choose Characters and to spawn enemies, powerups and objects<br/>
<br/>
3.1.	Scripts Layout<br/>
&emsp;  AI<br/>
&emsp;&emsp;    EnemyMain – IDamageable	<br/>
&emsp;&emsp;&emsp;      Follow Player<br/>
&emsp;&emsp;&emsp;      When dead add score to player	<br/>
&emsp;&emsp;&emsp;     Attack when close<br/>
&emsp;&emsp;&emsp;      Run closer to player, sometimes<br/>
&emsp;&emsp;    BasicEnemy (inherited from EnemyMain)<br/>
&emsp;&emsp;&emsp;      Will be counted to maintain amount spawn<br/>
&emsp;&emsp;&emsp;      And how many have been killed<br/>
&emsp;&emsp;    BossMain (inherited from EnemyMain)<br/>
&emsp;&emsp;&emsp;      Control HealthBar<br/>
&emsp;&emsp;    BossPugDog (inherited from BossMain)<br/>
&emsp;&emsp;&emsp;      Stink breath (damage per second)<br/>
&emsp;&emsp;    BossChicken (inherited from BossMain)<br/>
&emsp;&emsp;&emsp;      Fire breath (damage per second)<br/>
&emsp;&emsp;    BossRacoon (inherited from BossMain)<br/>
&emsp;&emsp;&emsp;      Black breath (damage per second)<br/>
      <br/>
&emsp;  Interfaces<br/>
&emsp;&emsp;    IDamageable<br/>
 &emsp;&emsp;&emsp;     Damage (damageTaken, damageType, damageLocation)<br/>
   <br/>
&emsp;  Lists<br/>
&emsp;&emsp;    CharacterList<br/>
&emsp;&emsp;    HighScoreList<br/>
 &emsp;&emsp;   LevelList<br/>
 &emsp;&emsp;&emsp;     LevelSelectList<br/>
&emsp;&emsp;&emsp;      BackgroundList<br/>
&emsp;&emsp;&emsp;      GroundList<br/>
&emsp;&emsp;    SpawnList<br/>
&emsp;&emsp;      Enemies<br/>
&emsp;&emsp;      Obstacles<br/>
&emsp;&emsp;      Bosses<br/>
<br/>
&emsp;  Obstacles<br/>
 &emsp;&emsp;   ObstacleHitCollider<br/>
 &emsp;&emsp;&emsp;     When hit it does damage and gets disables<br/>
 &emsp;&emsp;   Obstacles - IEndGameObserver	<br/>
 &emsp;&emsp;&emsp;     Moves left, and speed changes when game progresses<br/>
<br/>
&emsp;  Player<br/>
 &emsp;&emsp;   PlayerController  - IDamageable<br/>
 &emsp;&emsp;     Player movement<br/>
 &emsp;&emsp;     Control Powerups 		<br/>
<br/>
 &emsp; Pickups<br/>
 &emsp;&emsp;   BombDrop <br/>
 &emsp;&emsp;&emsp;     When the player picks up a bomb<br/>
  &emsp;&emsp;&emsp;    The bomb drops and waiting to hit an enemy<br/>
 &emsp;&emsp;&emsp;     Then will spawn an explosion<br/>
&emsp;&emsp;    Explosion<br/>
&emsp;&emsp;&emsp;      Find all Enemies in radius and do damage<br/>
&emsp;&emsp;    Pickups <br/>
 &emsp;&emsp;&emsp;     Contain pickup type from the Enums<br/>
  &emsp;&emsp;&emsp;    Rotate the pickups<br/>
 <br/>     
  &emsp;System<br/>
 &emsp;&emsp;   SpawnManager<br/>
&emsp;&emsp;&emsp;      Spawn Objects + Enemies<br/>
 &emsp;&emsp;   BackgroundRepeat<br/>
 &emsp;&emsp;&emsp;     Moves the background + ground<br/>
  &emsp;&emsp;&emsp;    Sets to new position when a curtain position is reached<br/>
&emsp;&emsp;    GameManager<br/>
&emsp;&emsp;&emsp;      Control the Load and unloading of scenes<br/>
&emsp;&emsp;&emsp;      Control the game states<br/>
&emsp;&emsp; &emsp;     Control the gameplay<br/>
 &emsp;&emsp; &emsp;    Load/Saves high scores<br/>
 &emsp;&emsp;&emsp;     Stores the player’s name, character selected and level<br/>
&emsp;&emsp;&emsp;      Create + Destroy Managers<br/>
&emsp;&emsp;    LoadSave<br/>
 &emsp;&emsp;&emsp;     Contains HighScoreList<br/>
  &emsp;&emsp;&emsp;    Save this script as json<br/>
      <br/>
&emsp;  UI<br/>
 &emsp;&emsp; UIManager<br/>
  &emsp;&emsp;&emsp;  Contains all states, when to show which UI. Always run-in background (singleton)<br/>
  &emsp;&emsp;&emsp;  Create a prefab of all the UI’s that will be controlled with the UIManager<br/>
 &emsp;&emsp; InGameUI<br/>
 &emsp;&emsp; &emsp;  Contains Time, score and current powerup, Player’s Health, Boss Health<br/>
 &emsp;&emsp; MainMenu<br/>
 &emsp;&emsp;&emsp;   Contains all the buttons + functions<br/>
 &emsp;&emsp; PauseMenu<br/>
  &emsp;&emsp;&emsp;Contains all the buttons + functions<br/>
&emsp;&emsp;  EndGameMenu<br/>
&emsp;&emsp;&emsp; Contains all the buttons + functions<br/>
&emsp;&emsp;  GameOverMenu<br/>
&emsp;&emsp;&emsp;Contains all the buttons + functions<br/>
&emsp;&emsp;  HighScoreMenu<br/>
&emsp;&emsp;&emsp;    Shows the highscores<br/>
&emsp;&emsp; OptionsMenu<br/>
&emsp;&emsp;&emsp;  Contains all the buttons + functions<br/>
  <br/>
 &emsp; Utils<br/>
 &emsp;&emsp;   Enums<br/>
  &emsp;&emsp;&emsp;    DamageType<br/>
  &emsp;&emsp;&emsp;    Particles To Spawn<br/>
 &emsp;&emsp;&emsp;     Pickups<br/>
  &emsp;&emsp;  Singleton<br/>
<br/>
3.2.	Events:<br/>
When damage has been delt to player or boss<br/>
<br/>
3.3.	Statics:<br/>
Level Selected (GameManager)<br/>
Speed of the background movement (GameManager)<br/>
<br/>
3.4.	Save: <br/>
High scores: Player Name, score, level, time<br/>
