# CopyRestAPI
CopyRestAPI is a C# client for the Copy cloud storage service through its RESTful API.


## About

CopyRestAPI is a C# client for the Copy cloud storage service through its RESTful API.

CopyRestAPI aims towards being the most comprehensive implementation for the [Copy REST API](https://developers.copy.com/) in .Net/C#.

## Features

+ Full Async support
+ Full implementation of the [API](https://developers.copy.com/documentation):
  - Support for OAuth Handshare
  - Support for User Profile methods
  - Support for Filesystem methods
  - Support for Links methods

## Contact

You can contact me on twitter [@saguiitay](https://twitter.com/saguiitay).

## NuGet

CopyRestAPI is available as a [NuGet package](https://www.nuget.org/packages/CopyRestAPI)

## Release Notes

+ **1.0.0**   Initial release.

## Usage

```csharp
var client = new CopyClient(
    new Config
        {
            CallbackUrl = "https://www.yourapp.com/Copy",
            ConsumerKey = ConsumerKey,
            ConsumerSecret = ConsumerSecret,

            //Token = Token,
            //TokenSecret = TokenSecret
        });

// Perform authorization (alternatively, provide a Token and TokenSecret in the Config object passed to CopyClient
await Authorize(client);

// Retrieve information on the logged-in user
var user = await client.UserManager.GetUserAsync();

// Retrieve the root folder of the logged-in user
var rootFolder = await client.GetRootFolder();

foreach (var child in rootFolder.Children)
{
    var childInfo = await client.FileSystemManager.GetFileSystemInformationAsync(child.Id);
}
```
