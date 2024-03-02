#!/bin/bash

message=$NOTIFICATION_MESSAGE

versionPlaceholder="<version>"

buildUrl="$GOOGLE_DRIVE_FOLDER_URL/$BUILD_FULL_NAME.zip"

message=${message//$versionPlaceholder/$VERSION_NAME}

message+=" $buildUrl"

echo "$message"

echo "notificationMessage=$message" >> "$GITHUB_OUTPUT"