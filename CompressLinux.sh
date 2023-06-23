#!/bin/sh
rm -r ./Compressed/
chmod +x ./7zip/7zz
./7zip/7zz -v8m a -t7z ./Compressed/Externals.7z ./Assets/Externals/