# media-saver aka PhotoHelper
Fun little weekend project that grew into something more. Probably won't release this is an official app of any kind.


## What the heck is this?
Just a Xamarin app (currently Android, iOS, and UWP) that lets you save media from Instagram, i.e. photos and videos. This app doesn't do anything fancy with the Instagram API, rather just uses the basic public API call to get info about a specific page and parses the json response to get the raw media URL. That call looks like this:
```
https://www.instagram.com/p/imageurl/?__a=1
```

**Note:** I don't think this is against any rules set forth by Instagram, at least none that I could find (there are plenty of other apps that do this and other projects on github that do much more). 

**ALSO NOTE:** If there are any issues with this please let me know and I will gladly remove this project. Just meant for this to be a fun problem to solve for myself.