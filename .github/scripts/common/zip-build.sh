#!/bin/bash

mkdir release

cp -R "build/$BUILD_TARGET" release

cd release || exit

oldStr=" "
newStr="_"
zipName=${BUILD_FULL_NAME//$oldStr/$newStr}

zip -r "$zipName.zip" "$BUILD_TARGET"

echo "targetZipName=$zipName" >> "$GITHUB_OUTPUT"