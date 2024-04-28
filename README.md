<h1 align="center">
  <br>
  <img src="assets/midimeld.png" width="275" alt="MidiMeld Logo, Two M's with an orange background"/>
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

With the application running, connect any MIDI controller to your computer and select which device to add a translation to using the topbar buttons. Using the "Record" button next to the MIDI signal text box or by observing the MIDI Signal veiwer in the topbar, input which MIDI signal that should be translated.

Now with the MIDI signal selected, change the translation type to the desired reaction.

Keystroke type allows for simple keysending and also hotkey activation. For special keys such as "Enter" or function keys like "F5", using curly brackets surrounding a special key's name. For modifers, simply typing "Control+", "Alt+", or "Shift+" before the key will activate the modifer. For example:

```
Alt+{F4}
Control+{Enter}
Shift+e
```

Windows Parameter type only currently contains "Volume" which controls allows the control over the default Windows audio device output volume. Primarily used for knobs and dials, this controls volume based on the velocity of the MIDI signal.

Once a translation is set up, when the application detects the desired MIDI signal, it will activate.

## Installation

Currently, to install this application for use, simply download the .zip folder from the "Releases" section of this Github page. Unzip the folder and run the "MidiMeld.exe" file.

This application <b>only runs on Windows</b>. Whilst this application was developed and tested on Windows 11, it should work from Windows 7+.

## Future Work

Some additional features that should be added to this project are:

* Script Execution
* Run in background
* Velocity specific reactions

## Development

This application was developed in C# using Microsoft's WPF for UI. The application was made as an artefact for my dissertation for my Bachelors Of Science degree in Software Engineering. This project is still in early development but aims to rival premium and priced applications in the MIDI translation market.

## Acknowledgments

Without "DryWetMIDI" by "Melanchall", Mike Meinz, and Hamish Chapman, this project would have never been possible. Thank you dearly.
