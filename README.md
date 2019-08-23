# VolumetricPlayer

Plays volumetric capture in Unity

How to:
Start an empty Unity project and import these assets into it

The cool parts:
- Uses coroutine to load/destroy assets concurrently = not filling up memory at initialization, or pause to load chunks of frames
- Proper destroying = no memory leak