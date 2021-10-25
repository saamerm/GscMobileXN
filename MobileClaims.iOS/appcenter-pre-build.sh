#!/usr/bin/env bash
# For Xamarin, change some constants located in some class of the app.
#
if [ ! -n "$Logger" ] 
then
    echo "You need define the Logger variable in App Center"
    exit
fi
if [ ! -n "$LoggerX" ] 
then
    echo "You need define the LoggerX variable in App Center"
    exit
fi

if [ ! -n "$Language" ] 
then
    echo "You need define the Language variable in App Center"
    exit
fi
if [ ! -n "$LanguageX" ] 
then
    echo "You need define the LanguageX variable in App Center"
    exit
fi

if [ ! -n "$Data" ] 
then
    echo "You need define the Data variable in App Center"
    exit
fi
if [ ! -n "$DataX" ] 
then
    echo "You need define the DataX variable in App Center"
    exit
fi
if [ ! -n "$AppSymbol" ] 
then
    echo "You need define the AppSymbol variable in App Center"
    exit
fi
if [ ! -n "$EnvironmentSymbol" ] 
then
    echo "You need define the EnvironmentSymbol variable in App Center"
    exit
fi



APP_LOGGER_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/LoggerService.cs
APP_LANGUAGE_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/LanguageService.cs
APP_DATA_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/JSONDataService.cs

if [ -e "$APP_LOGGER_FILE" ] 
then 
    echo "Updating ahValue in LoggerService.cs"
    sed -i '' 's#ahValue = "[a-zA-Z0-9]*";#ahValue = "'$Logger'";#' $APP_LOGGER_FILE
    echo "Updating ahxValue in LoggerService.cs"
    sed -i '' 's#ahxValue = "[a-zA-Z0-9]*";#ahxValue = "'$LoggerX'";#' $APP_LOGGER_FILE

#    echo "File content:"
#    cat $APP_LOGGER_FILE
fi

if [ -e "$APP_LANGUAGE_FILE" ] 
then 
    echo "Updating ahValue in LanguageService.cs"
    sed -i '' 's#ahValue = "[a-zA-Z0-9]*";#ahValue = "'$Language'";#' $APP_LANGUAGE_FILE
    echo "Updating ahxValue in LanguageService.cs"
    sed -i '' 's#ahxValue = "[a-zA-Z0-9]*";#ahxValue = "'$LanguageX'";#' $APP_LANGUAGE_FILE

#    echo "File content:"
#    cat $APP_LANGUAGE_FILE
fi

if [ -e "$APP_DATA_FILE" ] 
then 
    echo "Updating ahValue in JSONDataService.cs"
    sed -i '' 's#ahValue = "[a-zA-Z0-9]*";#ahValue = "'$Data'";#' $APP_DATA_FILE
    echo "Updating ahxValue in JSONDataService.cs"
    sed -i '' 's#ahxValue = "[a-zA-Z0-9]*";#ahxValue = "'$DataX'";#' $APP_DATA_FILE

#    echo "File content:"
#    cat $APP_DATA_FILE
fi

GSC_HELPER_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Helpers/GSCHelper.cs
APP_UPGRADE_SERVICE_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Core/Services/AppUpgradeService.cs
DROID_LOGIN_VIEW_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Views/LoginView.cs
DROID_CARD_VIEW_FRAGMENT_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Views/Fragments/CardViewFragment.cs
IOS_LOGIN_VIEW_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.iOS/Views/LoginView.cs
IOS_TERMS_AND_CONDITIONS_VIEW_FILE=$APPCENTER_SOURCE_DIRECTORY/MobileClaims.iOS/Views/TermsAndConditionsView.cs

if [ -e "$GSC_HELPER_FILE" ] 
then 
    echo "Updating APP_SYMBOL in GSCHelper.cs"
    sed -i '' 's#define APP_SYMBOL#define '$AppSymbol'#' $GSC_HELPER_FILE
    echo "Updating ENVIRONMENT_SYMBOL in GSCHelper.cs"
    sed -i '' 's#define ENVIRONMENT_SYMBOL#define '$EnvironmentSymbol'#' $GSC_HELPER_FILE

    #echo "File content:"
    #cat $GSC_HELPER_FILE
fi

if [ -e "$APP_UPGRADE_SERVICE_FILE" ] 
then 
    echo "Updating APP_SYMBOL in AppUpgradeService.cs"
    sed -i '' 's#define APP_SYMBOL#define '$AppSymbol'#' $APP_UPGRADE_SERVICE_FILE
    echo "Updating ENVIRONMENT_SYMBOL in AppUpgradeService.cs"
    sed -i '' 's#define ENVIRONMENT_SYMBOL#define '$EnvironmentSymbol'#' $APP_UPGRADE_SERVICE_FILE

    #echo "File content:"
    #cat $APP_UPGRADE_SERVICE_FILE
fi

if [ -e "$DROID_LOGIN_VIEW_FILE" ] 
then 
    echo "Updating APP_SYMBOL in Droid LoginView.cs"
    sed -i '' 's#define APP_SYMBOL#define '$AppSymbol'#' $DROID_LOGIN_VIEW_FILE
    echo "Updating ENVIRONMENT_SYMBOL in Droid LoginView.cs"
    sed -i '' 's#define ENVIRONMENT_SYMBOL#define '$EnvironmentSymbol'#' $DROID_LOGIN_VIEW_FILE

    #echo "File content:"
    #cat $DROID_LOGIN_VIEW_FILE
fi

if [ -e "$DROID_CARD_VIEW_FRAGMENT_FILE" ] 
then 
    echo "Updating APP_SYMBOL in Droid CardViewFragment.cs"
    sed -i '' 's#define APP_SYMBOL#define '$AppSymbol'#' $DROID_CARD_VIEW_FRAGMENT_FILE
    echo "Updating ENVIRONMENT_SYMBOL in Droid CardViewFragment.cs"
    sed -i '' 's#define ENVIRONMENT_SYMBOL#define '$EnvironmentSymbol'#' $DROID_CARD_VIEW_FRAGMENT_FILE

    #echo "File content:"
    #cat $DROID_CARD_VIEW_FRAGMENT_FILE
fi

if [ -e "$IOS_LOGIN_VIEW_FILE" ] 
then 
    echo "Updating APP_SYMBOL in iOS LoginView.cs"
    sed -i '' 's#define APP_SYMBOL#define '$AppSymbol'#' $IOS_LOGIN_VIEW_FILE
    echo "Updating ENVIRONMENT_SYMBOL in iOS LoginView.cs"
    sed -i '' 's#define ENVIRONMENT_SYMBOL#define '$EnvironmentSymbol'#' $IOS_LOGIN_VIEW_FILE

    #echo "File content:"
    #cat $IOS_LOGIN_VIEW_FILE
fi

if [ -e "$IOS_TERMS_AND_CONDITIONS_VIEW_FILE" ] 
then 
    echo "Updating APP_SYMBOL in iOS TermsAndConditionsView.cs"
    sed -i '' 's#define APP_SYMBOL#define '$AppSymbol'#' $IOS_TERMS_AND_CONDITIONS_VIEW_FILE
    echo "Updating ENVIRONMENT_SYMBOL in iOS TermsAndConditionsView.cs"
    sed -i '' 's#define ENVIRONMENT_SYMBOL#define '$EnvironmentSymbol'#' $IOS_TERMS_AND_CONDITIONS_VIEW_FILE

    #echo "File content:"
    #cat $IOS_TERMS_AND_CONDITIONS_VIEW_FILE
fi

cp $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values/$AppSymbol.LinksResource.xml $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values/LinksResource.xml
cp $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values-fr/$AppSymbol.LinksResource.xml $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values-fr/LinksResource.xml
cp $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values/$AppSymbol.SplashStyle.xml $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Resources/Values/SplashStyle.xml
cp $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Properties/$AppSymbol.AndroidManifest.xml $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Properties/AndroidManifest.xml
cp $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Assets/$AppSymbol.TermsAndConditions_en.html $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Assets/TermsAndConditions_en.html
cp $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Assets/$AppSymbol.TermsAndConditions_fr.html $APPCENTER_SOURCE_DIRECTORY/MobileClaims.Droid/Assets/TermsAndConditions_fr.html




