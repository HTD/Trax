# Trax Documentation

> Some basic concepts about Trax modules and development.

## Main window

Main window is a standard window form contained in `Main.cs` file.
As a good main window it's singleton and it's statically accessible with `Main.Instance`.

It's main purpose is to contain `DockPanel` for other windows and to provide main menu.

## Project file structure

`ProjectFile` class is designed to represent a scenery project file as logical structure object.
It contains static project structure and provides basic text manipulation methods.
`ProjectFile.All` contains all files included in the project.

Use `ProjectFile.CloseProject` to close the current project and return to initial state.

## Editor file structure

`EditorFile` class is designed to represent a file open as a tab in text editor.
It binds a project or non project file to a text editor and its tab.
`EditorFile.All` contains all files currently open in tabs.

## Text editor

`Editor` class extends `FastColoredTextBox` - custom control used for editing text.
It was aditionally customized to provide scenery specific syntax highlighting in large files.

I wonder if it could bind any advanced text editor module instead being tightly coupled with
`FastColoredTextBox`. I consider `Scintilla` replacement, however it would be not possible
until full 16-bit Unicode support would be available in `Scintilla`.

