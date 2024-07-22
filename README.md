# MTEC-340_Pinball

**Pinball** is a simple 2D pinball game developed with Unity.

## Table of Contents

- [Key Command](#key-command)
- [Game Mechanics](#game-mechanics)
  - [Points](#points)
  - [Lives](#lives)
  - [Powerups](#powerups)
- [Contact](#contact)

## Key Command

See **Help** section in game.

## Game Mechanics

### Points

- Each bumper hit grants the player **100** points.
- Each coin grants the player **50** points.
  - Coins respawn after 10 seconds.
- Tunnels grant the player **200**, **300**, or **500** points depending on the tunnel the ball has triggered.
  - The direction does not matter.

### Lives

- The player get 3 lives by default. _Game Over_ triggers when life reaches 0.
  - Every **10000** points grants the player an extra life. Maximum life count is 9.

### Powerups

- There are 2 types of powerups both of them last for 10 seconds.
  - **BigBall** makes the ball bigger and let the player hard to lose life from the gap. Each wall hit grants the player 10 pts. The gravity becomes lighter.
  - **DoubleScore** doubles the score increments.

- When there is no powerup item in the game, it respawns after 30 seconds.

## Contact

© nonocut 2024
[Drop a line](https://nonocut.com/contact/)
