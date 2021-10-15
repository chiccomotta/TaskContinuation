using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TaskDemo
{
    internal class Program
    {
        private static readonly string url = "http://deelay.me/100/http://www.delsink.com";
        private static int count = 0;

        private static void Main()
        {
            var httpRequestInfo = WebRequest.CreateHttp(url);
            var taskWebResponse = httpRequestInfo.GetResponseAsync();
            var taskContinuation = taskWebResponse.ContinueWith(task =>
                    {
                        var httpResponseInfo = task.Result as HttpWebResponse;

                        var responseStream = httpResponseInfo.GetResponseStream(); // downloading the page contents
                        using (var sr = new StreamReader(responseStream))
                        {
                            var webPage = sr.ReadToEnd();
                            Console.WriteLine(webPage.Length / count);
                        }
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith((task, state) =>
                    {
                        Console.WriteLine($"State {state}");
                        Console.WriteLine($"Task status {task.Status}");
                    },
                    count,
                    TaskContinuationOptions.OnlyOnFaulted)
                .ContinueWith((task) =>
                {
                    Console.WriteLine("Final continuation");
                });

            //Task.WaitAll(taskWebResponse, taskContinuation);
            Console.WriteLine("End main method");
            Console.ReadLine();
        }
    }
}