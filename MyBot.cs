using Discord;
using Discord.Commands;
using Discord.Modules;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace DiscoBot
{
    class MyBot
    {
        DiscordClient discord;
        CommandService commands;

        Random rand;

        string[] freshestMemes;
        string[] remPics;
        string[] coinFlip;
        ulong[] whitelist;


        //ArrayList myWhite = new ArrayList();
        List<ulong> mywhiteList = new List<ulong>();
        List<ulong> whServer = new List<ulong>();
        //List<string> afkList = new List<string>();
        List<ulong> afkList2 = new List<ulong>();
        List<ulong> afkServer = new List<ulong>();
        List<string> afkText = new List<string>();
        string token = LoadToken();
        
        

        public MyBot()
        {
            //rand = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            rand = new Random();
            char prefix = '+';

            remPics = new string[] {
                "Rem/rem1.gif", "Rem/rem2.png", "Rem/rem3.jpg", "Rem/rem4.jpg", "Rem/rem5.jpg",
                "Rem/rem6.png", "Rem/rem7.jpg", "Rem/rem8.png", "Rem/rem9.jpg", "Rem/rem10.png",
                "Rem/rem11.jpg", "Rem/rem12.jpg", "Rem/rem13.jpg", "Rem/rem14.png", "Rem/rem15.jpg",
                "Rem/rem16.jpg", "Rem/rem17.jpg", "Rem/rem18.jpg", "Rem/rem19.jpg", "Rem/rem20.png",
                "Rem/rem21.png", "Rem/rem22.jpg"

            };

            freshestMemes = new string[]{
                "memes/meme1.jpg", //0
                "memes/meme2.jpg", //1
                "memes/meme3.png" //2
            };

            coinFlip = new string[]
            {
                "Coin/obverse.png",
                "Coin/reverse.png"
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
                x.AppName = "Rem";
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
            RegisterOPCommand();
            RegisterCoinFlipCommand();
            //RegisterAFKCommand2();

            //Prefix TODO ;_;
            commands.CreateCommand("set_PREFIX")
                .Hide()
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


            /*discord.ExecuteAndWait(async () =>
            {
                //await discord.Connect(token, TokenType.Bot); //REAL BOT
                await discord.Connect("MjI5MDAwOTQ2MTk2MjgzMzky.CskvdA.ZfKbgo_nKJNkHcTKiWGbyYYx_rY", TokenType.Bot);//TESTBOT    
            });*/

            discord.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        //await discord.Connect(token, TokenType.Bot);//REAL BOT
                        await discord.Connect("MjI5MDAwOTQ2MTk2MjgzMzky.Cswfbg.Xn2GPF1G8mVpmzjLvLFBhXTB80U", TokenType.Bot);//TESTBOT   




                        break;
                    }
                    catch (Exception ex)
                    {
                        discord.Log.Error("Login Failed", ex);
                        await Task.Delay(discord.Config.FailedReconnectDelay);
                    }
                }
            });

        }



        private void Discord_MessageReceived(object sender, MessageEventArgs e)
        {
            String message = e.Message.RawText;

            if (e.Message.RawText.StartsWith("fp") || e.Message.RawText.StartsWith("-fp-"))
            {
                e.Channel.SendFile("memes/facepalm.png");
            }
            else if (e.Message.RawText.Contains("Kappa") || e.Message.RawText.Contains("kappa"))
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
            /*foreach (var m in e.Message.MentionedUsers)
            {
                Console.WriteLine("ID TEST: " + m.Id);
                if (afkList2.Contains(m.Id))
                {
                    e.Channel.SendMessage("AFK");
                }
            }*/
            foreach (var m in e.Message.MentionedUsers)
            {
                Console.WriteLine("ID of Sender: " + m.Id);
                string mentionedName = "++" + m.Id;
               
                ulong userID = mentionID(mentionedName, e.Server.Id);
                Console.WriteLine("ID MENTION: " + userID);
                if (afkServer.Contains(userID))
                {

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
            string load = "config/whserver.txt";
            string loadAFK = "config/afklist.txt";
            StreamReader sr0 = new StreamReader(load);
            while (sr0.Peek() > -1)
            {
                string UserIDs = sr0.ReadLine();
                ulong userID = 0;
                if (UInt64.TryParse(UserIDs, out userID))
                {
                    if (whServer.IndexOf(userID) < 0)
                    {
                        whServer.Add(userID);
                        Console.WriteLine("Successfully loaded initial whitelist!");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to Initially Load Whitelist!");
                }

            }
            StreamReader sr1 = new StreamReader(loadAFK);
            while (sr1.Peek() > -1)
                {
                    string afkIDs = sr1.ReadLine();
                    ulong afkID = 0;
                    if (UInt64.TryParse(afkIDs, out afkID))
                    {
                        if (afkServer.IndexOf(afkID) < 0)
                        {
                            afkServer.Add(afkID);
                            Console.WriteLine("Successfully loaded initial afklist!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to Initially Load afklist!");
                    }

            }
        }


        private void RegisterOPCommand()
        {
            //192750776005689344
            commands.CreateCommand("op")
                .Hide()
                .Do(async (e) =>
                {
                    if (e.User.Id == 192750776005689344)
                    {
                        ulong opID = checkID(e.User.Mention, e.Server.Id);
                        whServer.Add(opID);
                        await e.Channel.SendMessage("The Godfather himselfe is here.");
                    }
                    else
                    {
                        await e.Channel.SendMessage("You are not worthy of the OP command");
                    }
                });
        }

        private void RegisterLastActiveCommand()
        {
            commands.CreateCommand("lastactive")
                .Hide()
                .Description("Doesnt do much yet...")
                //.Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Last Active UTC: " + e.User.LastActivityAt);
                });
        }

        private void RegisterSLCommand() 
        {
            commands.CreateCommand("save")
                .Hide()
                .Description("Saves the current Whitelist to a file so it can be loaded")
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {
                        string save = "config/whserver.txt";
                        //mywhiteList.ForEach(Console.WriteLine);
                        //File.WriteAllLines(save, mywhiteList);
                        StreamWriter file = new System.IO.StreamWriter(save);
                        whServer.ForEach(file.WriteLine);
                        file.Close();
                        await e.Channel.SendMessage("Succesfully Saved!");
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });

            commands.CreateCommand("load")
                .Hide()
                .Description("Loads the current Whitelist from the file, overriding the whole active list!")
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {
                        string load = "config/whserver.txt";
                        StreamReader sr1 = new StreamReader(load);
                        while (sr1.Peek() > -1)
                        {
                            string UserIDs = sr1.ReadLine();
                            ulong userID = 0;
                            if (UInt64.TryParse(UserIDs, out userID))
                            {
                                if (whServer.IndexOf(userID) < 0)
                                {
                                    whServer.Add(userID);
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
                .Hide()
                .Description("List of IDs on the Whitelist")
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {

                        for (int i = 0; i < whServer.Count; i++)
                        {
                            await e.Channel.SendMessage("User: " + whServer[i]);
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                    
                });


            commands.CreateCommand("mentionList")
                .Hide()
                .Description("Dont use. Will get users to hate you...")
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {
                        for (int i = 0; i < whServer.Count; i++)
                        {
                            await e.Channel.SendMessage("User: " + "<@" + whServer[i] + ">");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You do not have Permission to use this command!");
                    }
                });

            commands.CreateCommand("afkList")
                .Hide()
                .Description("ID list of all AFKs.")
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {
                        for (int i = 0; i < afkServer.Count; i++)
                        {
                            await e.Channel.SendMessage("UserID: " + afkServer[i]);
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
                .Description("If you are not yet set AFK the command will set you AFK so that every user gets a response from Rem when mentioning you. Triggering the command again will remove the AFK status.")
                .Do(async (e) =>
                {

                    string nameofUser = createID(e.User.Mention, e.Server.Id);
                    ulong UserID = 0;
                    if (UInt64.TryParse(nameofUser, out UserID))
                    {
                        if (afkServer.IndexOf(UserID) < 0)
                        {
                            afkServer.Add(UserID);
                            await e.Channel.SendMessage("You are set AFK");
                            string save = "config/afklist.txt";
                            StreamWriter file = new System.IO.StreamWriter(save);
                            afkServer.ForEach(file.WriteLine);
                            file.Close();
                            Console.WriteLine("Succesfully Saved AKFList!");
                        }
                        else
                        {
                            afkServer.Remove(UserID);
                            await e.Channel.SendMessage("You are no longer AFK");
                            string save = "config/afklist.txt";
                            StreamWriter file = new System.IO.StreamWriter(save);
                            afkServer.ForEach(file.WriteLine);
                            file.Close();
                            Console.WriteLine("Succesfully Saved AFKList!");
                        }
                        for (int i = 0; i < afkServer.Count; i++)
                        {
                            Console.WriteLine("AFK " + i + ": " + afkServer[i]);
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
            /*commands.CreateCommand("wh2")
                .Description("A whitelisted user can Whitelist other users.")
                .Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {
                    if (mywhiteList.IndexOf(e.User.Id) >= 0 || e.User.Id == e.Server.Owner.Id)
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
                                string save = "config/whitelist.txt";
                                StreamWriter file = new System.IO.StreamWriter(save);
                                mywhiteList.ForEach(file.WriteLine);
                                file.Close();
                                Console.WriteLine("Succesfully Saved Whitelist!");
                            }
                            else
                            {
                                await e.Channel.SendMessage("The User is already whitelisted!");
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
                });*/

            commands.CreateCommand("whitelist")
                .Description("A whitelisted user can Whitelist other users.")
                .Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {
                        //string nameOfUser = e.GetArg("name");//
                        //string ServerIDs = "" +e.Server.Id;//
                        //int lenghtServer = ServerIDs.Length - 13;//
                        //ulong serverID = UInt64.Parse(ServerIDs.Substring(0,lenghtServer));//
                        //int length = nameOfUser.Length - 3;
                        //int length2 = nameOfUser.Length - 8;//
                        //string UserIDs = nameOfUser.Substring(2, length);
                        //string UserIDst = nameOfUser.Substring(2, length2);//
                        //string UserIDs2 = UserIDst + serverID;//
                        string UserIDFinal = createID(e.GetArg("name"), e.Server.Id);
                        ulong UserID = 0;
                        Console.WriteLine("Combined: " + UserIDFinal);
                        if (UInt64.TryParse(UserIDFinal, out UserID))
                        {
                            if (whServer.IndexOf(UserID) < 0)
                            {
                                await e.Channel.SendMessage("Successfully added User " + e.GetArg("name"));
                                whServer.Add(UserID);
                                string save = "config/whserver.txt";
                                StreamWriter file = new System.IO.StreamWriter(save);
                                whServer.ForEach(file.WriteLine);
                                file.Close();
                                Console.WriteLine("Succesfully Saved WhitelistSERVER!");
                            }
                            else
                            {
                                await e.Channel.SendMessage("The User is already whitelisted!");
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

            /*commands.CreateCommand("rmWhite")
                .Description("Remove a Whitelisted user from the list.")
                .Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {
                    if (mywhiteList.IndexOf(e.User.Id) >= 0)
                    {
                        string nameOfUser = e.GetArg("name");
                        int length = nameOfUser.Length - 3;
                        string UserIDs = nameOfUser.Substring(2, length);
                        ulong UserID = 0;
                        if (UInt64.TryParse(UserIDs, out UserID))
                        {
                            if (mywhiteList.IndexOf(UserID) >= 0)
                            {
                                await e.Channel.SendMessage("Successfully Removed User " + e.GetArg("name"));
                                mywhiteList.Remove(UserID);
                                string save = "config/whitelist.txt";
                                StreamWriter file = new System.IO.StreamWriter(save);
                                mywhiteList.ForEach(file.WriteLine);
                                file.Close();
                                Console.WriteLine("Succesfully Saved Whitelist!");
                            }
                            else
                            {
                                await e.Channel.SendMessage("The User is not on the whitelist!");
                            }
                        }
                        else
                        {
                            await e.Channel.SendMessage("Failed");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You dont have the permission to Remove anyone from the whitelist");
                    }
                });*/

            commands.CreateCommand("rmWhite")
                .Description("Remove a Whitelisted user from the list.")
                .Parameter("name", ParameterType.Required)
                .Do(async (e) =>
                {
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
                    {
                        string UserIDT = createID(e.GetArg("name"), e.Server.Id);
                        ulong UserID = 0;
                        if (UInt64.TryParse(UserIDT, out UserID))
                        {
                            if (whServer.IndexOf(UserID) >= 0)
                            {
                                await e.Channel.SendMessage("Successfully Removed User " + e.GetArg("name"));
                                whServer.Remove(UserID);
                                string save = "config/whserver.txt";
                                StreamWriter file = new System.IO.StreamWriter(save);
                                whServer.ForEach(file.WriteLine);
                                file.Close();
                                Console.WriteLine("Succesfully Saved Whitelist!");
                            }
                            else
                            {
                                await e.Channel.SendMessage("The User is not on the whitelist!");
                            }
                        }
                        else
                        {
                            await e.Channel.SendMessage("Failed");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("You dont have the permission to Remove anyone from the whitelist");
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
                    ulong usID = checkID(e.User.Mention, e.Server.Id);
                    if (whServer.IndexOf(usID) >= 0 || e.User.Id == e.Server.Owner.Id)
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

        private void RegisterCoinFlipCommand()
        {
            commands.CreateCommand("flip")
                .Description("Flips a coin for you")
                .Do(async (e) =>
                {
                    int randomFlipIndex = rand.Next(coinFlip.Length);
                    string coinToPost = coinFlip[randomFlipIndex];
                    await e.Channel.SendFile(coinToPost);
                });
        }

        private string createID(string Username, ulong ServerID)
        {
            string nameOfUser = Username;
            string ServerIDs = "" + ServerID;
            int lenghtServer = ServerIDs.Length - 13;
            string serverID = ServerIDs.Substring(0, lenghtServer);
            int length2 = nameOfUser.Length - 8;
            string UserIDst = nameOfUser.Substring(3, length2);
            string UserIDs2 = UserIDst + serverID;
            return UserIDs2;
        }

        private ulong checkID(string Username, ulong ServerID)
        {
            string nameOfUser = Username;
            string ServerIDs = "" + ServerID;
            int lenghtServer = ServerIDs.Length - 13;
            string serverID = ServerIDs.Substring(0, lenghtServer);
            int length2 = nameOfUser.Length - 8;
            string UserIDst = nameOfUser.Substring(3, length2);
            string UserIDs2 = UserIDst + serverID;
            ulong FinalID = UInt64.Parse(UserIDs2);

            return FinalID;
        }

        private ulong mentionID(string Username, ulong ServerID)
        {
            string nameOfUser = Username;
            string ServerIDs = "" + ServerID;
            int lenghtServer = ServerIDs.Length - 13;
            string serverID = ServerIDs.Substring(0, lenghtServer);
            int length2 = nameOfUser.Length - 7;
            string UserIDst = nameOfUser.Substring(3, length2);
            string UserIDs2 = UserIDst + serverID;
            ulong FinalID = UInt64.Parse(UserIDs2);

            return FinalID;
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
        }

    }
}
