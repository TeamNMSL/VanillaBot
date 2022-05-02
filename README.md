# Vanilla
A QBot based on [Konata](https://github.com/KonataDev/Konata.Core)

## Commands
| Basic Commands | Description |
|---------|-------------|
| /help   | Send the manual of this bot|
| /ping   | Check if this bot online|
| /v module \<on\|off\> \<ModuleName\>|Enable or Disable a Module.Just GroupOwner,BotAdmin and GroupAdmin are allowed to use this command|
| /v module list| Check status of bot modules|
| /v stat | Show some infomation of the bot|
| /v clear immersion | Set ImmersionStatus to \"null\" that make bot reply commands as usual|

These commands aren't controlled by module manager,that means these commands can't be disabled.

## Licenses
Licensed in GNU AGPLv3

In addition, anyone should shout "Lowiro SillyB" before using this software

## Start Using

BotSettings is a file used to initialise bot's data directory and specify the bot administrator.

First copy this into a new file, replace 'D\:\\ServerData\\Vanilla' to the path of your bot's data, replace 1848200159 to your QUin. Rename the file to "BotSettings" and put it in the same directory of VanillaForKonata.exe
```
Path=D:\ServerData\Vanilla
AdminUin=1848200159
```

You can also double click VanillaForKonata.exe , then stop this program and edit "BotSettings"  in the same directory of VanillaForKonata.exe.

Double click VanillaForKonata.exe,  and input your Bot's QUin and password.

Send '/ping' in a group. if bot reply you 'pong', congratulations, its basic function can work normally

## Custom and Advanced Features

### Theme
What you should do is that find a picture with a resolution of 640*1136, put it into BotDataDirectory\Images\ThemeBack\Ver1, rename it to "Back_i.png"

You can add new theme by editing picturedrawer\Drawer.cs and create ThemeList in GlobalScope.cs

### Manual
Before you starting edit your own manual, you must know what the manual looks like.You can download a demo and decompress the demo into your botdata directory.

In fact, we recommend that you edit Demo directly

### Arcaea
If you dont have api and api key,delete this module.

If you dont know what Arcaea is,delete this module and shout "Lowiro SillyB".

Write your seemingly "coco.natsu.suki/botarcapi" API into "BOTDATA/Arcaea/API" (this is a file)

Write your User-Agent into "BOTDATA/Arcaea/user-agent" (this is a file)

If you didnt find these files,you should create it by yourself.

Now update your arcaea sources

Send "/v arc update" to bot in a group,waiting for downloading complete.

Decompress BOTDATA/Arcaea/Arcaea.zip/assets to BOTDATA/Arcaea/assets

YOU MUST UPDATE YOUR ARCAEA SOURCES AFTER THIS GAME UPDATING OR ARCAEA MODULE WONT WORK

### SDVX
Delete this module please
### Pictures
Bot have the ablity to send some pictures such as 龙图

Copy your fold filled with 龙图 into botdata/images, then send 龙图来

You can add something others by edit code and edit fold

### Voice Record
Sometime bot may send voice record,you must do this to make it work normally
Compile [FFmpeg](https://github.com/FFmpeg/FFmpeg) and put it into 'Botdata/bin/',rename it to ffmpeg.exe
Compile [libSilkCodec](https://github.com/KonataDev/libSilkCodec) and copy it to the same directory of VanillaForKonata.exe
Reboot the bot

