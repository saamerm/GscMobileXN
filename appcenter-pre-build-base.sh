#!/usr/bin/env bash
set -e

# NOTE: This script is for both Android and iOS builds

#-----------------------------------------------------------------------

if [ ! -n "$Brand" ] || [ ! -n "$Environment" ] || [ ! -n "$Logger" ] || [ ! -n "$Language" ] || [ ! -n "$Data" ] || [ ! -n "$AppSecret" ]; then
    echo "You need to define the Brand, Environment, Logger, Language, Data, AppSecret variables in App Center."
    exit 1
fi
if [ ! -n "$LoggerX" ] || [ ! -n "$LanguageX" ] || [ ! -n "$DataX" ]; then
    echo "LoggerX, LanguageX or DataX value not provided. Xerox users support will be disabled."
fi

#-----------------------------------------------------------------------

echo "Injecting token fragments"

APP_LOGGER_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/LoggerService.cs
APP_LANGUAGE_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/LanguageService.cs
APP_DATA_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/JSONDataService.cs

sed -i '' 's/ahValue = "[^"]*";/ahValue = "'$Logger'";/' $APP_LOGGER_FILE
sed -i '' 's/ahxValue = "[^"]*";/ahxValue = "'$LoggerX'";/' $APP_LOGGER_FILE

sed -i '' 's/ahValue = "[^"]*";/ahValue = "'$Language'";/' $APP_LANGUAGE_FILE
sed -i '' 's/ahxValue = "[^"]*";/ahxValue = "'$LanguageX'";/' $APP_LANGUAGE_FILE
    
sed -i '' 's/ahValue = "[^"]*";/ahValue = "'$Data'";/' $APP_DATA_FILE
sed -i '' 's/ahxValue = "[^"]*";/ahxValue = "'$DataX'";/' $APP_DATA_FILE

#-----------------------------------------------------------------------

CORE_PROJECT_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/MobileClaims.Core.csproj
ANDROID_PROJECT_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/MobileClaims.Droid.csproj
IOS_PROJECT_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.iOS/MobileClaims.iOS.csproj

echo "Injecting Brand: $Brand and Environment: $Environment into project files"

sed -i '' "s/<DefineConstants>/<DefineConstants>$Environment;$Brand;/" $CORE_PROJECT_FILE
sed -i '' "s/<DefineConstants>/<DefineConstants>$Environment;$Brand;/" $ANDROID_PROJECT_FILE
sed -i '' "s/<DefineConstants>/<DefineConstants>$Environment;$Brand;/" $IOS_PROJECT_FILE

if [ "$Brand" == "CCQ" ]; then
echo "Injecting CCQ Card View files in $IOS_PROJECT_FILE"
sed -i '' 's#\<Compile Include=\"Views\\IdCard\\CardView.cs" \/>#\<InterfaceDefinition Include=\"Views\\IdCard\\CardView.xib" \/>\
\<Compile Include=\"Views\\IdCard\\CardView.cs" \/>\
\<Compile Include=\"Views\\IdCard\\CardView.designer.cs"\>\
\   <DependentUpon\>CardView.cs\<\/DependentUpon\>\
\<\/Compile\>#g' $IOS_PROJECT_FILE
fi
#-----------------------------------------------------------------------

BRAND_FOLDER=$APPCENTER_SOURCE_DIRECTORY/Brands/$Brand

if [ -d "$BRAND_FOLDER" ]; then
    echo "Copying brand: $Brand resources into the project"
    /bin/cp -Rf $BRAND_FOLDER/ $APPCENTER_SOURCE_DIRECTORY
else
    echo "Resource folder for brand: $Brand not found. Skipping brand resources copying."
fi

#-----------------------------------------------------------------------

echo "Injecting AppCenter app secret"

GSCHELPER_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Helpers/GSCHelper.cs

sed -i '' 's/AppCenterSecret = "[^"]*";/AppCenterSecret = "'$AppSecret'";/' $GSCHELPER_FILE