using Keafs_Lazy_WebToon_Downloader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Keafs_Lazy_WebToon_Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            //debug link https://www.webtoons.com/id/horror/creep-in/prolog/viewer?title_no=1742&episode_no=1
            Console.Title = "Keaf's Lazy WebToon Downloader";
            ConsoleHelper.Print("Welcome to Keafs Lazy WebToon Downloader");
            ConsoleHelper.Print("Please Enter WebToon Chapter");
            string input = Console.ReadLine();
            Console.Clear();
            try
            {
                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36");
                ConsoleHelper.Print("Sending Get Request...");
                string source = wc.DownloadString(input);
                ConsoleHelper.Print("Success!", ConsoleColor.Cyan);
                ConsoleHelper.Print("Scraping images");
                List<string> images = new List<string>();
                source.Split(new string[] { "_imageList\">" }, StringSplitOptions.None)[1]
                                           .Split(new string[] { "</div>" }, StringSplitOptions.None)[0]
                                           .Trim()
                                           .Split(new string[] { "data-url=\"" }, StringSplitOptions.None).ToList().ForEach(x =>
                                           {
                                               if (x.StartsWith("https"))
                                                   images.Add(x.Split('"')[0]);
                                           });
                ConsoleHelper.Print("Sucess!", ConsoleColor.Cyan);
                ConsoleHelper.Print("Total Ch Images: " + images.Count);
                string prefix = source.Split(new string[] { "content=\"" }, StringSplitOptions.None)[2].Trim().Split('"')[0];
                if (!Directory.Exists(prefix))
                {
                    ConsoleHelper.Print("Creating Folder " + prefix + "...");
                    Directory.CreateDirectory(prefix);
                }
                else
                {
                    ConsoleHelper.Print("Folder exists...");
                    ConsoleHelper.Print("Are you sure you want to continue?");
                    ConsoleHelper.Print("Press any key to continue...");
                    Console.ReadLine();
                }
                ConsoleHelper.Print("Downloading Chapters...");
                int PageNum = 1;
                Console.WriteLine(prefix);
                foreach (string img in images)
                {
                    
                    wc.Headers.Add(HttpRequestHeader.Referer, "https://webtoons.com/");
                    string ext = "";
                    if (img.Contains(".jpg"))
                        ext = ".jpg";
                    if (img.Contains(".png"))
                        ext = ".png";
                    string filename = prefix + "\\Page " + PageNum + ext;
                    Console.WriteLine(filename);
                    wc.DownloadFile(img, filename);
                    PageNum++;
                }
                ConsoleHelper.Print("Job Done!", ConsoleColor.Cyan);
                

            }
            catch(Exception oof) 
            {
                ConsoleHelper.PrintError("\n"+oof.Message + "\n\n-----StackTrace-----\n" + oof.StackTrace);
            }
            Console.ReadLine();


        }
    }
}
