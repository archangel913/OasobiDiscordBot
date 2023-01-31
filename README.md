# OasobiDiscordBot (music player features)
## Overview
Multi-function bot of discord for our server.
This version mainly includes a music playback function.

## Supported sources
* YouTube

Not supported play from local files.

## Commands
play : Play the music/play list.

exit : Exit from the voice channel.

queue : View the queue.

pause : Stop music temporally.

skip : Skip playing music.

remove : Remove waiting music in queue.

shuffle : Shuffle the queue.

loop : Change loop settings.(normal, queue loop, single loop)

volume : Change volume.

[Demonstration movie(Japanese)](https://youtu.be/Xrl6I_8ZZJo)

## Requirement
.NET 6.0

### Dll file
* libsodium.dll
* libopus.dll

download from [Voice binaries](https://github.com/discord-net/Discord.Net/blob/dev/voice-natives/vnext_natives_win32_x64.zip)

### Third party program
* FFmpeg (5.1.2-full_build)

[ffmpeg](https://www.gyan.dev/ffmpeg/builds/)

add ffmpeg.exe to the environment path.

### Others
* Bot token

[Discord Bot documents](https://discord.com/developers/docs/intro)

* YouTube api

[YouTube api documents](https://developers.google.com/youtube/v3/getting-started)

## SetUp
1. Clone this code for your local folder.
2. Open the solution by Visual Studio 2022.
3. Specify OasobiDiscordBot as the startup project.
4. Build
5. Edit OasobiDiscordBot.dll.config. Set BotToken and YouTubeApiToken.
(GuildID will repeal. input any numbers. ex.1234)
6. libsodium.dll and libopus.dll move folder with OasobiDiscordBot.exe and rename libopus.dll for opus.dll.
7. Run!

## License
OasobiDiscordBot is under the [MIT license](LICENSE)
