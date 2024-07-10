# MTEC-340_Pinball

**Pinball** is a simple 2D pinball game developed with Unity.

## Table of Contents

- [Key Command](#key-command)
- [Game Mechanics](#game-mechanics)
  - [Points](#points)
  - [Lives](#lives)
- [Contact](#contact)

## Key Command

| Key    |Function                 |
|:-------|:------------------------|
| F      | Left flipper            |
| J      | Right flipper           |
| P      | Pause game              |
| space  | Launch ball             |
| delete | Kill ball               |
| esc    | Back to title screen    |
| return | Start game from title   |

## Game Mechanics

### Points

- Each bumper hit grants the player **100** points.
- Each coin grants the player **50** points.
  - Coins respawn after 10 seconds.
- Tunnels grant the player **200**, **300**, or **500** points depending on the tunnel the ball has triggered.
  - The direction does not matter.

### Lives

- The player get 3 lives by default. _Game Over_ triggers when life reaches 0.
  - Every **5000** points grants the player an extra life. Maximum life count is 9.

## Contact

© nonocut 2024
[Drop a line](https://nonocut.com/contact/)
