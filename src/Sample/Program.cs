using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CopyRestAPI;
using CopyRestAPI.Models;

namespace Sample
{
    class Program
    {
        public const string Token = "...";
        public const string TokenSecret = "...";
        public const string ConsumerKey = "...";
        public const string ConsumerSecret = "...";

        [STAThread]
        private static void Main()
        {
            bool running = true;
            try
            {
                Run().ContinueWith(task => { running = false; });
                while (running)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        private static async Task Run()
        {
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
        }


        private static Task<bool> Authorize(CopyClient client)
        {
            var completion = new TaskCompletionSource<bool>();

            // Get request token
            var requestToken = client.Authorization.GetRequestTokenAsync().Result;

            string oauth_token = null;
            string oauth_verifier = null;
            var authUriString = requestToken.AuthCodeUri.ToString();

            // Navigate user to the returned Authorization Url
            var browser = new BrowserWindow();
            browser.Navigating += (sender, eventArgs) => Console.WriteLine("Navigating: " + eventArgs.Uri);
            browser.Navigated += (sender, eventArgs) =>
                {
                    // If user logged in, and returned the OAuth Verifier, retrieve that information and close browser
                    var uri = eventArgs.Uri.OriginalString;
                    if (uri.Contains("oauth_verifier="))
                    {
                        var query = uri.Split('?')[1];
                        string[] kvpairs = query.Split('&');
                        Dictionary<string, string> parameters = kvpairs.Select(pair => pair.Split('=')).ToDictionary(kv => kv[0], kv => kv[1]);

                        oauth_token = parameters["oauth_token"];
                        oauth_verifier = parameters["oauth_verifier"];

                        Console.WriteLine("Authorized: " + oauth_verifier);
                        browser.Close();
                    }
                    else
                    {
                        Console.WriteLine("Navigated: " + eventArgs.Uri);
                    }
                };

            browser.Show(authUriString);

            // If we got the OAuth Verifier, convert it to an Access Token, and complete Authorization
            if (oauth_token != null && oauth_verifier != null)
            {
                client.Authorization.GetAccessTokenAsync(oauth_verifier).Wait();

                completion.SetResult(true);
            }
            else
            {
                completion.SetException(new Exception("Failed to authorize"));
            }

            return completion.Task;
        }
    }
}
