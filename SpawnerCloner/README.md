# SpawnerCloner

A mod to simply clone existing Spawner objects like the Greydwarf nest and configure it with your ideas.

## Features

This mod includes mainly the features to:

1. Write all currently available spawners from inside the game into a YAML config file
2. Load and clone game spawners with your own settings using the YAML output

**WARNING** to all people that gonna ask the question why the spawner is not appearing in the game: you will still need to
use other mods like [SpawnThat](https://valheim.thunderstore.io/package/ASharpPen/Spawn_That/) to actually make the game
spawn this spawner.

### Generate config from game

To generate a YAML config file from all prefabs Spawners (must comply to ```SpawnArea``` component), you can simply use
the custom console command that this mod adds to the game:

```
spawner_cloner_write_defaults_to_file
```

This will create a file containing the defaults that are loaded from inside the game with all its currently added
spawners. The file can be found inside the BepInEx config folder with
name: ```org.bepinex.plugins.spawner.cloner.defaults.yaml```

#### Example Yaml content

(taken from vanilla game)

```yaml
Spawner_GreydwarfNest:
  originalPrefabName: Spawner_GreydwarfNest
  newSpawnerName: ''
  newSpawnerHoverText: ''
  spawnRadius: 4
  nearRadius: 20
  farRadius: 1000
  triggerDistance: 60
  levelUpChance: 15
  maxNear: 3
  maxTotal: 100
  spawnTimer: 0
  spawnIntervalSeconds: 10
  spawnOnGroundOnly: true
  setPatrolSpawnPoint: true
  sizeScaleFactor: 1
  spawns:
    - prefabName: Greydwarf
      minLevel: 1
      maxLevel: 3
      weight: 5
    - prefabName: Greydwarf_Elite
      minLevel: 1
      maxLevel: 3
      weight: 1
    - prefabName: Greydwarf_Shaman
      minLevel: 1
      maxLevel: 3
      weight: 1
  spawnerHealth: 100
  damageModifiers:
    mBlunt: Resistant
    mSlash: Normal
    mPierce: Resistant
    mChop: Normal
    mPickaxe: Ignore
    mFire: Weak
    mFrost: Immune
    mLightning: Normal
    mPoison: Immune
    mSpirit: Ignore
```

### Load customized config

To load your own clone config, you will need to provide a Yaml file that follows the given structure of the generated
config file. Most of the Yaml config should be rather self explaining for people being used to the game and being
configuring other mods.

The file to load the config from has to match to the given file name pattern:
```
org.bepinex.plugins.spawner.cloner.custom.*.yaml
```

#### Config explained

On top most level you can separate multiple entries inside one file with any custom name you like (only used in config
file, will not affect the spawner name in game).

For example:

```yaml
MyFunkyGrewDrunkenSpawner:
  originalPrefabName: Spawner_GreydwarfNest
  newSpawnerName: Drunken_Greydwarfs_Nest
...
```

* 'MyFunkyGrewDrunkenSpawner' is any arbitrary value that helps you to identify your spawners.
* 'Spawner_GreydwarfNest' is the original prefab from game that will be used as a base for cloning
* 'Drunken_Greydwarfs_Nest' is the new name that you can use with any other mod
  like [SpawnThat](https://valheim.thunderstore.io/package/ASharpPen/Spawn_That/) to make it spawn in game.

This section will contain a list of creature that this spawner will emit.
```yaml
  spawns:
    - prefabName: Greydwarf
      minLevel: 1
      maxLevel: 3
      weight: 5
```

* 'prefabName: Greydwarf' -> like with other mods you can enter the game's internal prefab name that it shall spawn
* each other creature to also maybe spawn will need to be added with the same 4 elements with an introducing '-' 
* all creatures you add there will be rolled to get spawned, the weight will decide how often a creature gets spawned

The damage modifiers set the strengths and weaknesses of this spawner, it always needs to have all the values set like
```yaml
  damageModifiers:
    mBlunt: Resistant
    mSlash: Normal
    mPierce: Resistant
    mChop: Normal
    mPickaxe: Ignore
    mFire: Weak
    mFrost: Immune
    mLightning: Normal
    mPoison: Immune
    mSpirit: Ignore
```

The game does use these values to identify how resistant the spawner will be against the specific damage type:
* VeryWeak -> highest damage
* Weak -> high damage
* Normal -> normal
* Resistant -> low damage
* VeryResistant -> very low damage
* Immune -> no damage (will show 0 damage on hit)
* Ignore -> not even affected by this type (not showing 0s)

### Not included yet

* use own (custom) prefabs, can only use Spawners that are already properly added to the game
* server sync (maybe later, feel free to help ;) )

## Changelog

* 0.1.0 -> initial version

## Contact

* https://github.com/FelixReuthlinger/SpawnerCloner
* Discord: Flux#0062 (you can find me around some of the Valheim modding discords, too)
