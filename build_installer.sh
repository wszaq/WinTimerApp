#!/bin/bash
echo "Step 1: Publishing Application..."
./publish.sh

echo "Step 2: Compiling Installer..."
# Note: We assume ISCC is available in the environment
ISCC installer/WindowsTimerApp.iss

echo "Build Complete! Check the 'installer_output' folder."
