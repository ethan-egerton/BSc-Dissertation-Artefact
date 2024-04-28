<h1 align="center">
  <br>
  <img src="source/assets/midimeld.png" width="275" alt="MidiMeld Logo, Two M's with an orange background"/>
  <br>
  <B>M</B>idi<B>M</B>eld
  <br>
</h1>

<p align="center">
    <B>MidiMeld is an application which translates MIDI signals to other actions, available only on Windows and developed using C#.</B>
</p>

## Available Translations

* Keystroke and Keyboard Shortcut

* Windows Volume

## How to Use

With the application running, connect any MIDI controller to your computer and use the topbar buttons to select which device to add a translation to. Input which MIDI signal should be translated using the "Record" button next to the MIDI signal text box or by observing the MIDI Signal viewer in the topbar.

Now, with the MIDI signal selected, change the translation type to the desired reaction.

The keystroke type allows for simple key sending and hotkey activation. Use curly brackets surrounding a unique key's name for special keys such as "Enter" or function keys like "F5". For modifiers, simply type "Control+", "Alt+", or "Shift+" before the key activates the modifier. For example:

```
Alt+{F4}
Control+{Enter}
Shift+e
```

Windows Parameter type only currently contains "Volume" which controls allows the control over the default Windows audio device output volume. Primarily used for knobs and dials, this controls volume based on the velocity of the MIDI signal.

Once a translation is set up, it will activate when the application detects the desired MIDI signal.

## Installation

Currently, to install this application, download the .zip folder from the "Releases" section of this GitHub page, unzip the folder, and run the "MidiMeld.exe" file.

This application <b>only runs on Windows</b>. Whilst this application was developed and tested on Windows 11, it should work from Windows 7+.

## Future Work

Some additional features that should be added to this project are:

* Script Execution
* Run in the background
* Velocity specific reactions

## Development

This application was developed in C# using Microsoft's WPF for UI as an artefact for my dissertation for my Bachelor of Science degree in Software Engineering. It is still in early development but aims to rival premium and priced applications in the MIDI translation market.

## Acknowledgments

This project would not have been possible without Melanchall, Mike Meinz, and Hamish Chapman. Thank you dearly.
