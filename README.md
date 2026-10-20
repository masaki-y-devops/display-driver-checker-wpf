## Overview

I developed this tiny apps to check the signature of the driver for Windows, 

especially the display driver, and calculate the checksum of each file when the authenticity can be confirmed.

If it is completed without errors, it can be verified with the linux `sha256sum -c` command at any times,

thanks to the form of ``(file name).sha256`` in the same hierarchy.

## Awareness of the problem

I think these workflows described below is annoying.

1. Checking whether the .exe file is damaged (modified) by someone except genuine publishers.

2. Saving check results to ensure integurity of optical disk backup.

I made this for GPD Win Mini, that its AMD Radeon 780M driver was sometimes unstable when running RPCS3.

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

## Examples to Check multiple files integurity later

1. Open the directory you want to match the checksum in Explorer.

2. Open the WSL or Git Bash by right-clicking in the directory.
   
3. Enter the command as follows:

~~~bash

sha256sum -c *.exe.sha256

~~~

It will return "foo.exe: OK" if it is vaild sha256sum.
