# OasobiDiscordBot
## Overview
music bot of discord for our server.

## music player

### Supported sources
* YouTube

Not supported play from local files.

### Commands
musicplayer: Send the control panel for the music player.

### Requirement
.NET 6.0

#### Dll file
* libsodium.dll
* libopus.dll

download from [Voice binaries](https://github.com/discord-net/Discord.Net/blob/dev/voice-natives/vnext_natives_win32_x64.zip)

#### Third party program
* FFmpeg (5.1.2-full_build)

[ffmpeg](https://www.gyan.dev/ffmpeg/builds/)

add ffmpeg.exe to the environment path.

#### Others
* Bot token

[Discord Bot documents](https://discord.com/developers/docs/intro)

* YouTube api

[YouTube api documents](https://developers.google.com/youtube/v3/getting-started)

### SetUp
1. Clone this code for your local folder.
2. Open the solution by Visual Studio 2022.
3. Specify OasobiDiscordBot as the startup project.
4. Build
5. libsodium.dll and libopus.dll move folder with OasobiDiscordBot.exe and rename libopus.dll for opus.dll.
6. Run!
7. In settings tab, set discord token and YouTube API.

note: Restart application when update settings.

## License
OasobiDiscordBot is under the [MIT license](LICENSE)
