## What is the application?

Check the signature of the driver for Windows (especially the display driver) and

Calculate the checksum of each file when the authenticity can be confirmed,

After the calculation results,

As it can be verified with the linux `sha256sum -c` command at a later date,

Save in the form of ``(file name).sha256`` in the same hierarchy.

## Awareness of the problem

Is this kind of work boring?

1. I want to check whether the .exe file is damaged (modified) by the driver file dropped from the manufacturer site.

2. In anticipation of the fact that it will not be possible to download due to broken links in the future, I want to check if it is damaged and archived to an optical disk etc.

3. Since it is a great deal, I want to be able to check whether there is a break or not later after confirming whether the manufacturer is in a genuine state as it is released.

The AMD Radeon 780M driver of the GPD WIN Mini was unstable and I thought I was checking it one by one while going back to the version.

As long as you download each exe file, just D&D the multiple files into the window of this application and insert them into the window of this application, signature authenticity verification and checksum calculation / save are realized in one stop.

## How to use

It is for x86-64 only.

.NET 10 runtime is required.

To install dependencies, Run below command on cmd/pwsh as admin:

~~~
winget install Microsoft.DotNet.DesktopRuntime.10
~~~

After downloading from the Release page, start it and D&D display driver into the window.

After verifying the signature, a .sha256 file should be created in the same tier as the .exe file.

The created .sha256 file should be set with .exe so that it will not be modified later and baked into BD-R or M-DISC.

## How to check later

1. Open the directory you want to match the checksum in Explorer.
2. Open the WSL or Git Bash by right-clicking in the directory.
3. Enter the command as follows:

~~~bash

sha256sum -c *.exe.sha256

~~~
