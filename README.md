A very simple command-line utility to set an environment variable from the command line so
that it stays around after a command window exits.  The utility also notifies all other windows
that the environment has changed, so that (for instance) the next command shell will show
the change, as will any services that are restarted.

Adapted from http://smartypeeps.blogspot.com/2006/12/windows-environment-variables.html

See also http://support.microsoft.com/kb/104011

See also http://www.switchonthecode.com/tutorials/csharp-snippet-tutorial-editing-the-windows-registry

SYNOPSIS: SetEnvVar.exe system|user <Name> <Value>


Example: 

C:\> SetEnvVar.exe system A_VAR_NAME keepThisValue
