# VRJesterAPI-RoR2VRMod
A VR Gesture Recognition API that allows Risk of Rain 2 mod developers to integrate their mods with VR.

## Usage:

1. After installing the mod and its dependencies, run the modded game so the config files can be generated.
2. To create & modify gestures:
   - Modify the `config/VRGestureStore.json` directly where gestures are stored.
   - Consider each object within the square brackets `[]` a mere *piece* of the gesture in a point in time. You can add multiple of these **GestureComponent** objects. This API recognizes gestures as complex as you want! Just know the more conplex, the more difficult it is to perform a gesture correctly to match your stored gesture.
   - Different values for *"VrDevice"* are `RIGHT_CONTROLLER, LEFT_CONTROLLER, HEAD_MOUNTED_DISPLAY` and if you want a gesture recognized on multiple VR devices you can incorporate a logical or using a pipe `|` like so `RIGHT_CONTROLLER|LEFT_CONTROLLER|HEAD_MOUNTED_DISPLAY`
   - Different values for *"Movement"* are `forward, back, left, right, up, down`
   - The *"Direction"* field takes `forward, back, left, right, up, down, *` values that create a 3D normalized vector. Example usage of this would be having `up` to recognize when the player's hand is facing upwards. The `*` is like a wild card and means any direction is acceptable. There's a certain threshold based on a virtual cone shape centered around the initial direction of the gesture. So it doesn't have to be *exact*.
   - The *"ElapsedTime"* is in milliseconds. So putting a value of `2000` would mean that part of the gesture lasts 2 seconds. This would be good for a charge up sort of move.
   - The *"Speed"* field is a float value representing the velocity of that GestureComponent. A decent threshold to recognize the speed of a punch would be `1500.0-2000.0` and feel free to play around with the values as much as you want.
   - Finally there's *"DevicesInProximity"* which is formatted like so:
   ```json
   "DevicesInProximity": {
       "LEFT_CONTROLLER": 20
   }
   ```
   What this example means is that the VrDevice of this GestureComponent has to be within proximity of the left controller for 20 frames.

   Sample gesture store:
   ```json
   "GESTURE 1": {
      "RIGHT_CONTROLLER|LEFT_CONTROLLER": [
        {
          "VrDevice": "RIGHT_CONTROLLER|LEFT_CONTROLLER",
          "Movement": "forward",
          "Direction": "*",
          "ElapsedTime": 0,
          "Speed": 0.0,
          "DevicesInProximity": {}
        }
      ]
    },
    ...
   ```
3. Once your gestures are created, you can map them to key binds in `config/VRJesterMod.cfg` by creating key value objects under the field *"GESTURE_ACTIONS"*. The *"KEY_ACTION"* field determines whether the recognized gesture triggers a single *click* key press, a *hold* down key press or *release* of said key.
```json
"GESTURE_ACTIONS": {
   "GESTURE 1": {
      "KEY_BIND": "M1",
      "KEY_ACTION": "click"
    },
    "DETROIT SMASH": {
      "KEY_BIND": "M2",
      "KEY_ACTION": "hold"
    },
    "DELAWARE SMASH": {
      "KEY_BIND": "LSHIFT",
      "KEY_ACTION": "click"
    },
    "DETROIT RELEASE": {
      "KEY_BIND": "M2",
      "KEY_ACTION": "release"
    }
}
```