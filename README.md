# ZX Spectrum Toolkit
A simple program to view and extract graphics from .z80 formatted files. Works with most .z80 files that I've tried. For a demonstration watch [this video](https://youtu.be/PphqnNoLGAs).

## Setup
Find the directory where you store your .z80 files. I can't provide any for you, you'll have to source them yourself!

You'll need to change one line of code to get this to work. Open up the `GraphicViewerGame.cs` file and locate line 37 (it's under `// Change the base directory here...`). Type in your folder location here.

# Using It
The menu is brought up using the `/` key. To quit the application, for example, press `/Q`.

To load a snapshot press `/L`. Type in the name of the file e.g. `myprog.z80` and press enter. The snapshot will load and the hex viewer will be shown.

Use page up / down to scroll through memory.

Press the `/` button to be taken back to the main memory where you can choose to go to the `SCREEN%` by pressing `/VS` or the sprites `/VP`.