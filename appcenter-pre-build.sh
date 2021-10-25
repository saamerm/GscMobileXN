#!/usr/bin/env bash
set -e

# NOTE: This script is for iOS build

#-----------------------------------------------------------------------

echo "Running base script first"

./appcenter-pre-build-base.sh

#-----------------------------------------------------------------------

INFO_PLIST_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.iOS/Info.plist
VERSION_CODE="${VersionCode:-$APPCENTER_BUILD_ID}"
VERSION_NAME="${VersionName:-0.0.0.x}"
VERSION_NAME="${VERSION_NAME/x/$VERSION_CODE}" # inject versionCode into the name if 'x' placeholder provided

echo "Updating bundle versions in Info.plist file. Bundle version: $VERSION_CODE, Short version string: $VERSION_NAME"

plutil -replace CFBundleVersion -string $VERSION_CODE $INFO_PLIST_FILE
plutil -replace CFBundleShortVersionString -string $VERSION_NAME $INFO_PLIST_FILE

#-----------------------------------------------------------------------

case $Brand in
    ENCON)
        BUNDLE_ID="ca.encon.now"
        BUNDLE_DISPLAY_NAME_EN="Victor Now"        
        BUNDLE_DISPLAY_NAME_FR="Victor Ici"
        BUNDLE_NAME_EN="Victor Now"
        ;;
    CLAC)
        BUNDLE_ID="ca.clac.myclac"
        BUNDLE_DISPLAY_NAME_EN="GSC4myCLAC"
        BUNDLE_DISPLAY_NAME_FR="GSC4myCLAC"
        BUNDLE_NAME_EN="GSC4myCLAC"
        ;;
    FPPM)
        BUNDLE_ID="ca.fppm.assurances"
        BUNDLE_DISPLAY_NAME_EN="FPPM Assurances"
        BUNDLE_DISPLAY_NAME_FR="FPPM Assurances"
        BUNDLE_NAME_EN="FPPM Assurances"
        ;;
    WestJet)
        BUNDLE_ID="ca.westjet.hdclaims"
        BUNDLE_DISPLAY_NAME_EN="H and D Claims"
        BUNDLE_DISPLAY_NAME_FR="Règlements"
        BUNDLE_NAME_EN="H and D Claims"
        ;;
    ARTA)
        BUNDLE_ID="ca.arta.benefits"
        BUNDLE_DISPLAY_NAME_EN="ARTA Benefits"
        BUNDLE_DISPLAY_NAME_FR="Prestations ARTA"
        BUNDLE_NAME_EN="ARTA Benefits"
        ;;
	CCQ)
        BUNDLE_ID="ca.medic.construction"
        BUNDLE_DISPLAY_NAME_EN="MÉDIC Construction"
        BUNDLE_DISPLAY_NAME_FR="MÉDIC Construction"
        BUNDLE_NAME_EN="MÉDIC Construction"
        ;;
	WWL)
        BUNDLE_ID="ca.wawanesa.life"
        BUNDLE_DISPLAY_NAME_EN="Wawanesa Life"
        BUNDLE_DISPLAY_NAME_FR="Wawanesa Vie"
        BUNDLE_NAME_EN="Wawanesa Life"
        ;;
    GSC)
        BUNDLE_ID="ca.greenshield.onthego"
        BUNDLE_DISPLAY_NAME_EN="GSC on the Go"
        BUNDLE_DISPLAY_NAME_FR="GSC à votre portée"
        BUNDLE_NAME_EN="GSC on the Go"
        ;;
    *)
        echo "Unknown brand: $Brand. Failing the build."
        exit 1
        ;;
esac

if [ "$Environment" != "PROD" ] && [ "$Environment" != "PROD1" ] && [ "$Environment" != "PROD2" ]; then
    BUNDLE_DISPLAY_NAME_EN="$Environment $BUNDLE_DISPLAY_NAME_EN"
    BUNDLE_DISPLAY_NAME_FR="$Environment $BUNDLE_DISPLAY_NAME_FR"
    BUNDLE_NAME_EN="$Environment $BUNDLE_NAME_EN"
fi

echo "Updating Info.plist and infoPlist.strings files. Bundle ID: $BUNDLE_ID, Display name: $BUNDLE_DISPLAY_NAME_EN, Display name FR: $BUNDLE_DISPLAY_NAME_FR, Bundle name: $BUNDLE_NAME_EN"

INFO_PLIST_STRINGS_EN_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.iOS/Resources/en.lproj/infoPlist.strings
INFO_PLIST_STRINGS_FR_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.iOS/Resources/fr.lproj/infoPlist.strings

plutil -replace CFBundleIdentifier -string "$BUNDLE_ID" $INFO_PLIST_FILE
plutil -replace CFBundleName -string "$BUNDLE_NAME_EN" $INFO_PLIST_FILE
plutil -replace CFBundleDisplayName -string "$BUNDLE_DISPLAY_NAME_EN" $INFO_PLIST_FILE
plutil -replace CFBundleDisplayName -string "$BUNDLE_DISPLAY_NAME_EN" $INFO_PLIST_STRINGS_EN_FILE
plutil -replace CFBundleDisplayName -string "$BUNDLE_DISPLAY_NAME_FR" $INFO_PLIST_STRINGS_FR_FILE