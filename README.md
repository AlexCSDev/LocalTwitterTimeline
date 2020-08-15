# Local Twitter Timeline
Ever been tired of not being able to scroll past a specific amount of posts in your timeline? This application is designed to help you with that.

This application uses twitter's api data (obtained using external tool like [twarc](https://github.com/DocNow/twarc)) to create a local version of your timeline viewable via any web browser.

**Important notice: This application does not save avatars, videos or images. They will be displayed as long as they are available on twitter servers.**

## Usage
1) Install and start mongodb on 127.0.0.1:27017
2) Configure external application (like [twarc](https://github.com/DocNow/twarc)) to make dumps of your timeline. The dump should contain tweet objects as they returned by twitter api, separated by new line (\n) character. (JSON Lines format) Example scripts for twarc are included in the "twarc" directory.
3) Execute TweetImporter #path_to_json_file#, it will read all tweets in the file and import them into mongodb for further use. Duplicate tweets will not be imported.
4) Launch WebFrontend and use https://localhost:5001 ( or https://localhost:5001/(ID of the starting tweet)/(asc/desc) ) to access it.

## License
All files in this repository are licensed under the license listed in LICENSE.md file unless stated otherwise.

## Acknowledgments
This project uses some code from the following projects:
* [Tweetz](https://github.com/mike-ward/tweetz) - Classes for binding twitter json into .NET objects