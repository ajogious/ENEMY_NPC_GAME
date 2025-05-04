# Unity Enemy NPC AI Project

This is a Unity project showcasing an enemy NPC (Non-Player Character) with AI behaviors like patrolling, chasing, attacking, and adapting based on the player's actions.

## Features

- **Basic Navigation**: The NPC uses Unity's NavMesh system to patrol and chase the player.
- **Perception System**: The enemy can detect the player based on line of sight (FOV).
- **Finite State Machine (FSM)**: The enemy has different states: Patrolling, Chasing, Attacking, Retreating, Searching.
- **Adaptation System**: The enemy adapts based on the player’s behavior (aggressive, stealthy, or balanced).
- **UI Enhancements**: Displays the player’s current playstyle (Aggressive, Stealthy, Balanced) and other stats.

## Installation

To get started with this project, follow the steps below:

### Prerequisites

- Unity Hub (any version from 2020 onward should work)
- Unity Editor (preferably 2020.3 or higher)

### Steps to set up

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/ajogious/ENEMY_NPC_GAME.git
   ```
2. Open the Project in Unity:
   Launch Unity Hub.
   Click on "Add" and select the cloned repository folder.

3. Install Required Packages (if any):
   Ensure the NavMesh system is set up (it should be by default in newer Unity versions).
   If using TextMesh Pro, ensure it's installed via the Unity Package Manager.

4. Run the Project:
   Press the Play button in Unity to test the NPC's behavior.
   Use the arrow keys (or WASD) to move the player and press spacebar to attack.

### How to Use

1. Player Controls:
   Movement: Use arrow keys to move.
   Attack: Press the Spacebar to perform an attack.

2. Enemy NPC:
   The enemy will patrol, chase, and attack the player based on the current state (Patrolling, Chasing, Attacking, etc.).
   The NPC adapts its behavior based on your playstyle (aggressive, stealthy, or balanced).

3. UI:
   The UI will display the player's current playstyle, the number of attacks performed, and the distance moved.

### Contributing

1. Fork the repository.
2. Create a new branch (git checkout -b feature/your-feature).
3. Make your changes and commit them (git commit -am 'Add new feature').
4. Push to the branch (git push origin feature/your-feature).
5. Create a pull request.

### License

This project is licensed under the MIT License - see the LICENSE.md file for details.

## Acknowledgments

Thanks to Unity for providing the tools and documentation that made this project possible.

Special thanks to the community for their contributions and tutorials.

Contact
Your Name: [Your GitHub Profile Link]

Email: [Your Email Address]
