using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace DiscoBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;

        Random rand;

        string[] freshestMemes;
        string[] remPics;
        ulong[] whitelist;


        //ArrayList myWhite = new ArrayList();
        List<ulong> mywhiteList = new List<ulong>();
        //List<string> afkList = new List<string>();
        List<ulong> afkList2 = new List<ulong>();
        string token = LoadToken();
        
        

        public MyBot()
        {
            rand = new Random();
            char prefix = '+';

            remPics = new string[] {
                "Rem/rem1.gif",
                "Rem/rem2.png",
                "Rem/rem3.jpg",
                "Rem/rem4.jpg",
                "Rem/rem5.jpg",
                "Rem/rem6.png",
                "Rem/rem7.jpg",
                "Rem/rem8.png",
            };

            freshestMemes = new string[]{
                "memes/meme1.jpg", //0
                "memes/meme2.jpg", //1
                "memes/meme3.png" //2
            };

            whitelist = new ulong[]{
                192750776005689344
            };

            


 
            //myWhite.Add(192750776005689344);
            mywhiteList.Add(192750776005689344);
            initialLoad();
            //File.WriteAllLines(@"C:\Users\Daniele\OneDrive\Dokumente\RemBot\DiscoBot\whitelist.txt", myWhite.Cast<string>());
            


            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.MessageReceived += Discord_MessageReceived;

            

            

            discord.UsingCommands(x =>
                {
                    x.PrefixChar = prefix;
                    x.AllowMentionPrefix = true;
                    x.HelpMode = HelpMode.Public;
                });

            commands = discord.GetService<CommandService>();
            RegisterMemeCommand();
            RegisterPurgeCommand();
            RegisterRemCommand();
            RegisterWhitelistcommand();
            RegisterListCommand();
            RegisterSLCommand();
            RegisterLastActiveCommand();
            RegisterAFKCommand();
            //RegisterAFKCommand2();

            //Prefix TODO ;_;
            commands.CreateCommand("set_PREFIX")
                .Parameter("prefix", ParameterType.Unparsed)
                .Do(async (e) =>
                {
                    char tempPrefix;
                    if (char.TryParse(e.GetArg("prefix"), out tempPrefix))
                    {
                        prefix = tempPrefix;
                        await e.Channel.SendMessage("Prefix Changed to: " + prefix);
                    }
                    else
                    {
                        await e.Channel.SendMessage("Wrong Prefix");
                    }
                    
                });

            // if(Int32.TryParse(e.GetArg("amount"), out toDel)){


            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(token, TokenType.Bot); //REAL BOT
                
            });

        }



        private void Discord_MessageReceived(object sender, MessageEventArgs e)
        {
            String message = e.Message.RawText;

            if (e.Message.RawText.StartsWith("fp") || e.Message.RawText.StartsWith("-fp-"))
            {
                e.Channel.SendFile("memes/facepalm.png");
            }
            else if (e.Message.RawText.StartsWith("Kappa") || e.Message.RawText.StartsWith("kappa"))
            {
                e.Channel.SendFile("memes/Kappa.png");
            }
            else if (e.Message.RawText.StartsWith("feelsbadman"))
            {
                e.Channel.SendFile("memes/feelsbadman.png");
            }
            /*for(int i = 0; i < afkList.Count; i++){
               if (e.Message.RawText.Contains(afkList[i]))
                {
                    if (!e.Message.IsAuthor)
                    {
                        e.Channel.SendMessage("User " + afkList[i] + " is AFK");
                    }

                }
            }*/
            foreach (var m in e.Message.MentionedUsers){
                Console.WriteLine("ID TEST: " + m.Id);
                if(afkList2.Contains(m.Id)){
                    e.Channel.SendMessage("The Mentioned user is set AFK");
                }
            }
            /*if (e.Message.RawText.Contains(afkList[i]))
            {
                if (!e.Message.IsAuthor)
                {
                    e.Channel.SendMessage("User " + afkList[i] + " is AFK");
                }

            }
        }*/
            /*
           switch (message)
           {
               case "fp":
               case "-fp-":
                   e.Channel.SendFile("memes/facepalm.png");
                   break;

           }*/
        }

        private void initialLoad(){
            string load = "config/whitelist.txt";
            StreamReader sr0 = new StreamReader(load);
            while (sr0.Peek() > -1)
            {
                string UserIDs = sr0.ReadLine();
                ulong userID = 0;
                if (UInt64.TryParse(UserIDs, out userID))
                {
                    if (mywhiteList.IndexOf(userID) < 0)
                    {
                        mywhiteList.Add(userID);
                        Console.WriteLine("Successfully loaded initial whitelist!");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to Initially Load Whitelist!");
                }

            }
        }


        private void RegisterLastActiveCommand()
        {
            commands.CreateCommand("lastactive")
                //.Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Last Active UTC: " + e.User.LastActivityAt);
                });
        }

        private void RegisterSLCommand() 
        {
            commands.CreateCommand("save")
                .Do(async (e) =>
                {
                    if (e.User.Id == mywhiteList[0])
                    {
                        string save = "config/whitelist.txt";
                        //mywhiteList.ForEach(Console.WriteLine);
                        //File.WriteAllLines(save, mywhiteList);
                        StreamWriter file = new System.IO.StreamWriter(save);
                        mywhiteList.ForEach(file.WriteLine);
                        file.Close();
                        await e.Channel.SendMessage("Succesfully Saved!");
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });

            commands.CreateCommand("load")
                .Do(async (e) =>
                {
                    if (e.User.Id == mywhiteList[0])
                    {
                        string load = "config/whitelist.txt";
                        StreamReader sr1 = new StreamReader(load);
                        while (sr1.Peek() > -1)
                        {
                            string UserIDs = sr1.ReadLine();
                            ulong userID = 0;
                            if (UInt64.TryParse(UserIDs, out userID))
                            {
                                if (mywhiteList.IndexOf(userID) < 0)
                                {
                                    mywhiteList.Add(userID);
                                    await e.Channel.SendMessage("Successfully Loaded from File!");
                                }
                            }
                            else
                            {
                                await e.Channel.SendMessage("Failed to Load from file!");
                            }

                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });
        }



        private static string LoadToken()
        {
            string load = "config/config.txt";
            StreamReader sr1 = new StreamReader(load);
            string temp = sr1.ReadLine();
            return temp;
        }


        private void RegisterListCommand()
        {
            commands.CreateCommand("idList")
                .Do(async (e) =>
                {
                    if (mywhiteList.IndexOf(e.User.Id) >= 0)
                    {
                        for (int i = 0; i < mywhiteList.Count; i++)
                        {
                            await e.Channel.SendMessage("User: " + mywhiteList[i]);
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });

            commands.CreateCommand("mentionList")
                .Do(async (e) =>
                {
                    if (mywhiteList.IndexOf(e.User.Id) >= 0)
                    {
                        for (int i = 0; i < mywhiteList.Count; i++)
                        {
                            await e.Channel.SendMessage("User: " + "<@" + mywhiteList[i] + ">");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });

            commands.CreateCommand("afkList")
                .Do(async (e) =>
                {
                    if (mywhiteList.IndexOf(e.User.Id) >= 0)
                    {
                        for (int i = 0; i < afkList2.Count; i++)
                        {
                            await e.Channel.SendMessage("UserID: " +afkList2[i]);
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });

        }

       /* private void RegisterAFKCommand2()
        {
            commands.CreateCommand("afk2")
                .Do(async (e) =>
                {

                    string nameofUser = e.User.Mention;
                    if (afkList.IndexOf(nameofUser) < 0)
                    {
                        afkList.Add(nameofUser);
                        await e.Channel.SendMessage("You are set AFK");
                    }
                    else
                    {
                        afkList.Remove(nameofUser);
                        await e.Channel.SendMessage("You are no longer AFK");
                    }
                    for (int i = 0; i < afkList.Count; i++)
                    {
                        Console.WriteLine("AFK " + i + ": " + afkList[i]);
                    }
                });
        }*/

        private void RegisterAFKCommand()
        {
            commands.CreateCommand("afk")
                .Do(async (e) =>
                {

                    string nameofUser = e.User.Mention;
                    int length = nameofUser.Length - 3;
                    string UserIDs = nameofUser.Substring(2, length);
                    ulong UserID = 0;
                    if (UInt64.TryParse(UserIDs, out UserID))
                    {
                        if (afkList2.IndexOf(UserID) < 0)
                        {
                            afkList2.Add(UserID);
                            await e.Channel.SendMessage("You are set AFK");
                        }
                        else
                        {
                            afkList2.Remove(UserID);
                            await e.Channel.SendMessage("You are no longer AFK");
                        }
                        for (int i = 0; i < afkList2.Count; i++)
                        {
                            Console.WriteLine("AFK " + i + ": " + afkList2[i]);
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("Failed to set AFK state");
                    }
                    
                });
        }


        private void RegisterWhitelistcommand()
        {
            commands.CreateCommand("whitelist")
                .Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {

                    //if (myWhite.IndexOf(e.User.Id) >= 0)
                    if (mywhiteList.IndexOf(e.User.Id) >= 0)
                    {
                        string nameOfUser = e.GetArg("name");
                        int length = nameOfUser.Length - 3;
                        string UserIDs = nameOfUser.Substring(2, length);
                        ulong UserID = 0;
                        if (UInt64.TryParse(UserIDs, out UserID))
                        {
                            if (mywhiteList.IndexOf(UserID) < 0)
                            {
                                await e.Channel.SendMessage("Successfully added User " + e.GetArg("name"));
                                mywhiteList.Add(UserID);
                            }
                            else
                            {
                                await e.Channel.SendMessage("User " + nameOfUser + " is already whitelisted!");
                            }
                        }
                        else
                        {
                            await e.Channel.SendMessage("Failed");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You dont have the permission to whitelist anyone");
                    }
                });
        }


        /*private void RegisterWhitelistcommand()
        {
 	        commands.CreateCommand("whitelist")
                .Parameter("name", ParameterType.Required)
                .Do(async (e) => {

                    string nameOfUser = e.GetArg("name");
                    int length = nameOfUser.Length - 3;
                    //await e.Channel.SendMessage("Test: " + nameTesting.Substring(3, nameTesting.Length));
                  //  Console.WriteLine("Test: " + test.Substring(3, test.Length));
                    await e.Channel.SendMessage("" + nameOfUser);
                    await e.Channel.SendMessage("Lenght: " + length);
                    await e.Channel.SendMessage("" + nameOfUser.Substring(2, length));
                    
                    //await e.Channel.SendMessage(e.GetArg("name"));
                    //Console.WriteLine("Argument: " + e.GetArg("name"));
                    //await e.Channel.SendMessage("ID: " + e.User.Id);
                    //Console.WriteLine("ID: " + e.User.Id);
                    //await e.Channel.SendMessage("Server Permissions: " + e.User.ServerPermissions);
                    //await e.Channel.SendMessage("Status: " + e.User.Status);
                    //await e.Channel.SendMessage("Server: " + e.User.Server);
                    //await e.Channel.SendMessage("Roles: " + e.User.Roles);
                    //await e.Channel.SendMessage("Private Channel: " + e.User.PrivateChannel);
                    //Console.WriteLine("private channel: " + e.User.Nickname);
                    //await e.Channel.SendMessage("NickMention: " + e.User.NicknameMention);
                    //Console.WriteLine("Nickmention: " + e.User.NicknameMention);
                    //await e.Channel.SendMessage("Nick: " + e.User.Nickname);
                    //Console.WriteLine("Nick: " + e.User.Nickname);
                    //await e.Channel.SendMessage("Mention: " + e.User.Mention);
                    //Console.WriteLine(e.User.Mention);
                    //await e.Channel.SendMessage("LastActive: " + e.User.LastActivityAt);
                    //await e.Channel.SendMessage("Client: " + e.User.Client);
                    string UserIDs = nameOfUser.Substring(2, length);
                    await e.Channel.SendMessage("String before: " + UserIDs);
                    ulong UserID = 0;
                    if(UInt64.TryParse(UserIDs, out UserID)){
                        await e.Channel.SendMessage("Int Value: " + UserID);
                    }
                    else {
                        await e.Channel.SendMessage("Failed");
                    }
                    for (int i = 0; i < myWhite.Count; i++)
                    {
                        Console.WriteLine("Array" + i +": " + myWhite[i]);
                    }
                    myWhite.Add(UserID);
                    Console.WriteLine("2: " + myWhite[1]);
                    for (int i = 0; i < myWhite.Count; i++)
                    {
                        Console.WriteLine("Array" + i + ": " + myWhite[i]);
                    }
                    //Array.Add(whitelist, UserID);


                    
                });
        }*/



        private void RegisterPurgeCommand(){

            commands.CreateCommand("purge")
                .Description("Will delete the amount of messages that is set in the parameter. Max amount is 100!")
                .Parameter("amount", ParameterType.Required)
                .Do(async (e) =>
                {
                   // await e.Channel.SendMessage("" + e.User.Id);
                   // ulong whitelist = 192750776005689344;
                    //if (Array.IndexOf(whitelist, e.User.Id) >= 0)
                    if (mywhiteList.IndexOf(e.User.Id) >= 0)
                    {
                        Message[] messagesToDelete;
                        int toDel = 0;
                        if (Int32.TryParse(e.GetArg("amount"), out toDel))
                        {
                            if (toDel <= 101)
                            {
                                messagesToDelete = await e.Channel.DownloadMessages(toDel);

                                await e.Channel.DeleteMessages(messagesToDelete);
                            }
                            else
                            {
                                messagesToDelete = await e.Channel.DownloadMessages(101);

                                await e.Channel.DeleteMessages(messagesToDelete);
                            }
                        }
                        else
                        {
                            await e.Channel.SendMessage("Wrong Parameter. Use '+help purge' for more info");
                        }
                    }
                    else {
                        await e.Channel.SendMessage("You are not Whitelisted");
                    }

                    //await e.Channel.SendMessage("Purge has been temporarily disabled due to permission issues.");
                   // await e.Channel.SendMessage(e.User.Name);
                   // await e.Channel.SendMessage("" + e.User.Id);
                    /*Message[] messagesToDelete;
                    int toDel = 0;
                    if(Int32.TryParse(e.GetArg("amount"), out toDel)){
                        if (toDel <= 100)
                        {
                            messagesToDelete = await e.Channel.DownloadMessages(toDel);

                            await e.Channel.DeleteMessages(messagesToDelete);
                        }
                        else
                        {
                            messagesToDelete = await e.Channel.DownloadMessages(100);

                            await e.Channel.DeleteMessages(messagesToDelete);
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("Wrong Parameter. Use '+help purge' for more info");
                    }*/
                });
        }

        private void RegisterMemeCommand()
        {
            commands.CreateCommand("meme")
                .Description("Posts Random Memes")
                .Do(async (e) =>
                {
                    int randomMemeIndex = rand.Next(freshestMemes.Length);
                    string memeToPost = freshestMemes[randomMemeIndex];
                    await e.Channel.SendFile(memeToPost);
                });
        }

        private void RegisterRemCommand()
        {
            commands.CreateCommand("rem")
                .Description("Posts Random Pics of Rem")
                .Do(async (e) =>
                {
                    int randomRemIndex = rand.Next(remPics.Length);
                    string remToPost = remPics[randomRemIndex];
                    await e.Channel.SendFile(remToPost);
                });

            commands.CreateCommand("morning")
                .Description("Sends a nice good Morning pic of Rem")
                .Do(async (e) =>
                {
                    await e.Channel.SendFile("Rem/morning.jpg");
                    await e.Channel.SendMessage("Good Morning " + e.User.Mention);
                });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

    }
}
