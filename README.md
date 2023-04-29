# Bal Converter

This is a free and open source software that is wrapping FFmpeg, yt-dlp and imagick with an UI. It allows to download videos, edit them, add tags and more. 

![image](https://user-images.githubusercontent.com/29146363/235271749-f4505e58-6d6a-4285-ab83-659daeed0f68.png)

![image](https://user-images.githubusercontent.com/29146363/235271910-98f62d74-40ca-4f43-b366-c1277701be17.png)


This software is still in development. In this state I would not recommend anyone to use it.

## Features

- Download videos (Only YouTube tested)
- Edit videos by adding tags and filters (FFmpeg)
- Convert files to different formats e.g. MP4 to MP3
- Download videos in the background

# Roadmap

- Fixing many issues
- Allow to convert files from file explorer (shell extensions)
- Adding playlist to download
- More options when converting files
- More editing features
- ...

# How to build

Before starting to build or debug you will need to copy `yt-dlp.exe`, `ffmpeg.exe` and `ffprobe.exe` to the following folders `\Bal.Converter\Tools\` and `Bal.Converter.CLI\Tools\`.
After that you can just restore packages and build the application.

# Help needed

I am struggling to package this app and set it up in a continues integration environment. It would be nice if you could help me with it.

# License

- GNU General Public License v3
- https://github.com/bozali/bal-converter/blob/dev/LICENSE
