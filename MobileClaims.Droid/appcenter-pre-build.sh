#!/usr/bin/env bash
set -e

# NOTE: This script is for Android build

#-----------------------------------------------------------------------

echo "Running base script first"

../appcenter-pre-build-base.sh

#-----------------------------------------------------------------------

ANDROID_MANIFEST_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Properties/AndroidManifest.xml
VERSION_CODE="${VersionCode:-$APPCENTER_BUILD_ID}"
VERSION_NAME="${VersionName:-0.0.0.x}"
VERSION_NAME="${VERSION_NAME/x/$VERSION_CODE}" # inject versionCode into the name if 'x' placeholder provided

echo "Updating version numbers in the AndroidManifest.xml file. Version code: $VERSION_CODE, Version name: $VERSION_NAME"

sed -i '' 's/versionCode="[0-9.]*"/versionCode="'$VERSION_CODE'"/' $ANDROID_MANIFEST_FILE
sed -i '' 's/versionName="[0-9.]*"/versionName="'$VERSION_NAME'"/' $ANDROID_MANIFEST_FILE

#-----------------------------------------------------------------------

case $Brand in
    ENCON)
        PACKAGE_NAME="ca.encon.now"
        ;;
    CLAC)
        PACKAGE_NAME="ca.clac.myclac"
        ;;
    FPPM)
        PACKAGE_NAME="ca.fppm.assurances"
        ;;
    WestJet)
        PACKAGE_NAME="ca.westjet.hdclaims"
        ;;
	CCQ)
        PACKAGE_NAME="ca.medic.construction"
        ;;
    WWL)
        PACKAGE_NAME="ca.wawanesa.life"
        ;;
    ARTA)
        PACKAGE_NAME="ca.arta.benefits"
        ;;
    GSC)
        PACKAGE_NAME="com.greenshield.mobileclaims"
        ;;
    GSCNet)
        PACKAGE_NAME="com.greenshield.mobileclaims"
        ;;
    *)
        echo "Unknown brand: $Brand. Failing the build."
        exit 1
        ;;
esac

echo "Updating package name in the AndroidManifest.xml file. Package: $PACKAGE_NAME"

sed -i '' 's/package="[a-zA-Z0-9.]*"/package="'$PACKAGE_NAME'"/' $ANDROID_MANIFEST_FILE

#-----------------------------------------------------------------------

if [ "$Environment" != "PROD" ] && [ "$Environment" != "PROD1" ] && [ "$Environment" != "PROD2" ]; then
    echo "Appending environment name to app name"

    STRINGS_EN=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values/BrandStrings.xml
    STRINGS_FR=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values-fr/BrandStrings.xml
    
    PATTERN='s/<string name="app_name">\(.*\)<\/string>/<string name="app_name">'$Environment' \1<\/string>/'
    sed -i '' "$PATTERN" $STRINGS_EN
    sed -i '' "$PATTERN" $STRINGS_FR
fi