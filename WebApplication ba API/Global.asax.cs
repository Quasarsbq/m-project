using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using System.Data.SqlClient;
using WebApplication_ba_API;
using WebApplication_ba_API.Models;
using System.Data;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Data = System.Collections.Generic.KeyValuePair<string, int>;

namespace WebApplication_ba_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
    // Telegram.Bot.TelegramBotClient bot = new Telegram.Bot.TelegramBotClient("443096045:AAG6r5n2SgzhLvi_Pgx4ZvLDjrtm9dOu3xU");// buff
     Telegram.Bot.TelegramBotClient bot = new Telegram.Bot.TelegramBotClient("425489173:AAG2esoDKjjSwT4LSGJCDMD7WnTDbjNhBRk");// testony
        

     //   string connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\Documents\demoDB.mdf;Integrated Security=True;Connect Timeout=30";
     string connection = @"workstation id=meloLOW.mssql.somee.com;packet size=4096;user id=Projectjson_SQLLogin_1;pwd=lh5s8ra91o;data source=meloLOW.mssql.somee.com;persist security info=False;initial catalog=meloLOW";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ////var req = Request.InputStream;
            ////var responsString = new StreamReader(req).ReadToEnd();
            ////var e = JsonConvert.DeserializeObject<Update>(responsString);
            ////Telegram.Bot.TelegramBotClient bot = new Telegram.Bot.TelegramBotClient("425489173:AAG2esoDKjjSwT4LSGJCDMD7WnTDbjNhBRk"); //testony
            ////var x = await bot.SendTextMessageAsync(e.Message.Chat.Id, "succeed");
            
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string chatidmovaqat = e.Message.Chat.Id.ToString();
            try {
                
                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.AudioMessage)
                {
                    bot.SendChatActionAsync(chatidmovaqat, Telegram.Bot.Types.Enums.ChatAction.FindLocation);
                    string FileName = e.Message.Audio.Title.ToString();
                    string Duration = e.Message.Audio.Duration.ToString();
                    string performer = e.Message.Audio.Performer.ToString();
                    if(FileName.Length>50||FileName.Contains("@")|| FileName.Contains("_")|| FileName.Contains("%")|| FileName.Contains("#") || e.Message.Audio.Performer == null|| e.Message.Audio.Performer.Length>50|| e.Message.Audio.Performer.Contains("@")|| e.Message.Audio.Performer.Contains("[")|| e.Message.Audio.Performer.Contains("{")|| e.Message.Audio.Performer.ToLower().Contains(".com")|| e.Message.Audio.Performer.ToLower().Contains(".ir")|| e.Message.Audio.Performer.ToLower().Contains("www") || e.Message.Audio.Title.ToLower().Contains("www")|| e.Message.Audio.Title.ToLower().Contains("remix")|| e.Message.Audio.Title.ToLower().Contains("hha"))
                    {
                        bot.SendTextMessageAsync(chatidmovaqat, "مطابق معیارای استاد نیست.",0,false,true,e.Message.MessageId);
                    }
                    else
                    {
                        Class1 myclass = new Class1();
                        if (myclass.checkdublicate(FileName, performer) == false)
                        {
                            SqlConnection myconnection = new SqlConnection();
                            myconnection.ConnectionString = connection;
                            SqlCommand mycommand = new SqlCommand();
                            mycommand.Connection = myconnection;
                            mycommand.CommandText = "insert into[melo] (fileid,filename,performer,path,duration,size) values(@fileid,@filename,@performer,@path,@duration,@size)";    //sets info 
                            mycommand.Parameters.AddWithValue("@fileid", e.Message.Audio.FileId.ToString());
                            mycommand.Parameters.AddWithValue("@filename", e.Message.Audio.Title.ToString());
                            mycommand.Parameters.AddWithValue("@performer", e.Message.Audio.Performer.ToString());
                            if (e.Message.Audio.FilePath != null)
                                mycommand.Parameters.AddWithValue("@path", e.Message.Audio.FilePath.ToString());
                            else
                                mycommand.Parameters.AddWithValue("@path", "0");
                            mycommand.Parameters.AddWithValue("@duration", e.Message.Audio.Duration.ToString());
                            mycommand.Parameters.AddWithValue("@size", e.Message.Audio.FileSize.ToString());
                            myconnection.Open();
                            mycommand.ExecuteNonQuery();
                            myconnection.Close();
                            bot.SendTextMessageAsync(chatidmovaqat, "ثبت شد.");
                        }
                        else bot.SendTextMessageAsync(chatidmovaqat, "تکراری بود", 0, false, false, e.Message.MessageId);
                    }
                    

                }
                else if(e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                    if (e.Message.Text.Length > 50   &&    !e.Message.Text.Contains("\U0001F464")    &&   !e.Message.Text.Contains("\U0001F3B5")) { bot.SendTextMessageAsync(chatidmovaqat, "Stop spamming \U0001F620 "); }
                else
                {
                    if (e.Message.Text.ToLower()=="count bitch")
                    {
                        int count = 0;
                        SqlConnection myconnection = new SqlConnection();
                        myconnection.ConnectionString = connection;
                        SqlCommand mycommand = new SqlCommand();
                        mycommand.Connection = myconnection;
                        mycommand.CommandText = "select count(*) from [melo] ";
                        myconnection.Open();
                        try
                        {
                            count = (Int32)mycommand.ExecuteScalar();
                            myconnection.Close();
                            bot.SendTextMessageAsync(chatidmovaqat, count.ToString() + " U fCkIn IdIot :/",Telegram.Bot.Types.Enums.ParseMode.Html);

                        }
                        catch { }
                        myconnection.Close();
                    }
                    else if(e.Message.Text.ToLower() == "/today")
                    {
                        int count = 0;
                        SqlConnection myconnection = new SqlConnection();
                        myconnection.ConnectionString = connection;
                        SqlCommand mycommand = new SqlCommand();
                        mycommand.Connection = myconnection;
                        mycommand.CommandText = "SELECT COUNT(*) FROM   [meloii] where [tarix]=@date";
                        mycommand.Parameters.AddWithValue("@date", DateTime.Today.ToShortDateString());
                        myconnection.Open();                       
                         count = (Int32)mycommand.ExecuteScalar();
                         myconnection.Close();
                         bot.SendTextMessageAsync(chatidmovaqat, count.ToString() + " U fCkIn IdIot :/");
                        myconnection.Close();
                    }
                    else
                    {
                        string text = e.Message.Text;
                        try
                        {
                            SqlConnection myconnection = new SqlConnection();
                            myconnection.ConnectionString = connection;
                            SqlCommand mycommand = new SqlCommand();
                            mycommand.Connection = myconnection;
                            DataTable mytable = new DataTable();
                            SqlDataAdapter myadapter = new SqlDataAdapter();
                            Class1 myclass = new Class1();
                            if (myclass.PerformerCheck(text,30)&&!text.Contains("\U0001F464"))  // sheklake Person ro nadarad.
                            {
                                string textORG = text;
                                for (int i = 0; i < text.Length + 1; i = i + 2)
                                {
                                    text = text.Insert(i, "%");
                                }
                                text = text.Replace("'", "''");
                                mycommand.CommandText = "select performer from [melo] where [performer] like '" + text + "' order by performer";
                               
                                myconnection.Open();
                                myadapter.SelectCommand = mycommand;
                                myadapter.Fill(mytable);
                                myconnection.Close();
                                List<string> performers = new List<string>();
                                List<string> filename = new List<string>();
                                foreach (DataRow duck in mytable.Rows)
                                {                                    
                                    if (myclass.CalculateSimilarity(duck["performer"].ToString(), textORG) > 30) 
                                    {
                                        if (performers.Count == 0)
                                        {
                                            performers.Add(duck["performer"].ToString());
                                            filename.Add("");
                                        }   
                                                                           
                                         else if(myclass.CalculateSimilarity( performers.Last().ToLower() , duck["performer"].ToString().ToLower())   <70)
                                        {
                                            performers.Add(duck["performer"].ToString());
                                            filename.Add("");
                                        }
                                    }
                                }
                                // adding some songs here :/

                                SqlConnection myconnection1 = new SqlConnection();
                                myconnection1.ConnectionString = connection;
                                SqlCommand mycommand1 = new SqlCommand();
                                mycommand1.Connection = myconnection1;
                                DataTable mytable1 = new DataTable();
                                SqlDataAdapter myadapter1 = new SqlDataAdapter();
                                mycommand1.CommandText = "select performer,filename from [melo] where [filename] like '%" + textORG + "%' order by rate DESC";
                                myconnection1.Open();
                                myadapter1.SelectCommand = mycommand1;
                                myadapter1.Fill(mytable1);
                                myconnection1.Close();
                                foreach (DataRow duck in mytable1.Rows)
                                {
                                    performers.Add(duck["performer"].ToString());
                                    filename.Add(duck["filename"].ToString());
                                    if (performers.Count > 100) break;
                                }
                                    bot.SendTextMessageAsync(chatidmovaqat,performers.LongCount().ToString()+"مورد مشابه یافت شد" , 0, false, false, 0, Class1.ButtonCreatorObjecti(filename, performers));
                            }


                            else if (text.Contains("\U0001F464")&& !text.Contains("\U0001F3B5"))    //sheklake adamako darad vali MELODI na
                            {
                                text = text.Replace("\U0001F464", "");
                                text = text.Replace("'", "''");
                                if(text.Contains(" "))
                                {
                                    if(text.IndexOf(" ")==0|| text.LastIndexOf(" ") == text.Length - 1) { text = text.Replace(" ", ""); }
                                }
                                string textORG = text;
                                for (int i = 0; i < text.Length + 1; i = i + 2)
                                {
                                    text = text.Insert(i, "%");
                                }
                                mycommand.CommandText = "select filename,performer from [melo] where [performer] like '" + text + "' order by performer";
                                myconnection.Open();
                                myadapter.SelectCommand = mycommand;
                                myadapter.Fill(mytable);
                                myconnection.Close();
                                List<string> performers = new List<string>();
                                List<string> filename = new List<string>();
                                
                                foreach (DataRow duck in mytable.Rows)
                                {
                                    if (performers.Count == 0)
                                    {
                                        performers.Add(duck["performer"].ToString());
                                        filename.Add(duck["filename"].ToString());                                       
                                    }     
                                    else if (((performers.Last() != duck["performer"].ToString()|| filename.Last() != duck["filename"].ToString())     &&     (myclass.CalculateSimilarity(textORG, duck["performer"].ToString()) > 50))|| duck["performer"].ToString().ToLower()== textORG.ToLower())
                                    {
                                        performers.Add(duck["performer"].ToString());
                                        filename.Add(duck["filename"].ToString());
                                    }
                                    
                                    if (performers.Count > 70) break;
                                }
                               
                                    bot.SendTextMessageAsync(chatidmovaqat, "از میان این گزینه ها انتخاب کنید:", 0, false, false, 0, Class1.ButtonCreatorObjecti(filename,performers));
                            }
                            else if  (text.Contains("\U0001F3B5")&& text.Contains("\U0001F464"))   //	musical note darad + adamak
                            {
                                string Fname = text.Substring(text.IndexOf("\U0001F3B5"), text.IndexOf("\U0001F464"));
                                string Performer = text.Substring(text.IndexOf("\U0001F464"));
                                mycommand.CommandText = "select fileid,rate from [melo] where [filename]=@name and [performer]=@perf";
                                mycommand.Parameters.AddWithValue("@name", Fname);
                                mycommand.Parameters.AddWithValue("@perf", Performer);
                                myconnection.Open();
                                myadapter.SelectCommand = mycommand;
                                myadapter.Fill(mytable);
                                myconnection.Close();
                                string id = "";
                                int sum = 0;
                                foreach (DataRow duck in mytable.Rows)
                                {
                                    id =duck["fileid"].ToString();
                                    sum =Convert.ToInt32( duck["rate"]);
                                    //bot.SendAudioAsync(chatidmovaqat, new FileToSend(duck["fileid"].ToString()), "\U0001F916" + "\U0001F194" + ":" + "@WeSongbot", 100, "", "");
                                    bot.SendAudioAsync(chatidmovaqat, new Telegram.Bot.Types.FileToSend(id), "\U0001F916 : @Testonybot \n", 100,"","");                                   
                                }
                                myclass.NewRate(id, sum);
                                myclass.UpdateLastUse(chatidmovaqat);
                              

                            }


                            else if (!e.Message.Text.Contains("\U0001F50D")) //magnifyer nadare    //age chizi k mixad jozve performera nist inja search ishe(searche asli)
                            {
                                string text1 = e.Message.Text.Replace("'", "''");
                                text1 = text1.Replace(" ", "%");                            
                                mycommand.CommandText = "select fileid,duration,performer,filename from [melo] where [filename] like '%" + text1 + "%'" + " or [performer] like '%" + text1 + "%'";
                                myadapter.SelectCommand = mycommand;
                                myconnection.Open();
                                myadapter.Fill(mytable);
                                myconnection.Close();
                                int count = mytable.Rows.Count;
                                if (count == 0)
                                {
                                    string[] tokens = text.ToLower().Split(' ');

                                    SqlConnection myconnection10 = new SqlConnection();
                                    myconnection10.ConnectionString = connection;
                                    SqlCommand mycommand10 = new SqlCommand();
                                    mycommand10.Connection = myconnection10;
                                    DataTable mytable10 = new DataTable();
                                    SqlDataAdapter myadapter10 = new SqlDataAdapter();
                                    mycommand10.CommandText = "select filename,performer from [melo] ";
                                    myconnection10.Open();
                                    myadapter10.SelectCommand = mycommand10;
                                    myadapter10.Fill(mytable10);
                                    myconnection10.Close();
                                    List<int> listmoqayese = new List<int>();
                                    List<int> listmoqayese2 = new List<int>();
                                    var list = new List<Data>();
                                    
                                    Dictionary<string, int> result = new Dictionary<string, int>();
                                    foreach (DataRow duk in mytable10.Rows)
                                    {
                                        string title = duk["filename"].ToString().ToLower();
                                        string raqqas = duk["performer"].ToString().ToLower();
                                        string bounded = title + " " + raqqas;
                                        string[] boundedArr = bounded.Split(' ');
                                        foreach (string S in tokens)
                                        {
                                            foreach (string p in boundedArr)
                                            {
                                                listmoqayese.Add(myclass.CalculateSimilarity(S, p));
                                            }
                                           
                                            listmoqayese2.Add(listmoqayese.Max());
                                            listmoqayese.Clear();
                                        }
                                        if (Convert.ToInt32(listmoqayese2.Average()) < 20) continue;
                                        int av = Convert.ToInt32(listmoqayese2.Average());
                                        list.Add(new Data("\U0001F3B5" + duk["filename"].ToString() + "\U0001F464" + duk["performer"].ToString(), av));
                                        listmoqayese2.Clear();
                                    }
                                    list.OrderBy(pair => pair.Value);
                                    var result1 = from pair in list orderby pair.Value descending select pair;

                                    result1.ToList();
                                    ////resultend = list.ToDictionary<KeyValuePair<string, int>, string, int>(pair => pair.Key, pair => pair.Value);
                                    //for(int i=0; i < 70; i++)
                                    //{
                                    //    resultend.Add(list.ElementAt(i).ToString(), i); 

                                    //}
                                    bot.SendTextMessageAsync(chatidmovaqat," مواردی که ممکنه مطابق نباشه:", 0, false, false, 0, Class1.ButtonOBJEee(result1.ToList()));
                                }

                                    #region bullshits

                                    //////////////////    else
                                    //////////////////    {
                                    //////////////////        bot.SendTextMessageAsync(chatidmovaqat, "مورد مشابهی ندیدم :( میشه اگه پیدا کردی به منم بفرستیش؟");
                                    //////////////////    }
                                    //////////////////}





                                    //if (myclass.PerformerCheck(tokens[0], 80))
                                    //{

                                    //    //SqlConnection myconnection10 = new SqlConnection();
                                    //    //myconnection10.ConnectionString = connection;
                                    //    //SqlCommand mycommand10 = new SqlCommand();
                                    //    //mycommand10.Connection = myconnection10;
                                    //    //DataTable mytable10 = new DataTable();
                                    //    //SqlDataAdapter myadapter10 = new SqlDataAdapter();
                                    //    mycommand10.CommandText = "select filename,performer from [melo] where [performer] like'%" + tokens[0] + "%'";      
                                    //    myconnection10.Open();
                                    //    myadapter10.SelectCommand = mycommand10;
                                    //    myadapter10.Fill(mytable10);
                                    //    myconnection10.Close();


                                    //    Dictionary<string, int> resultend = new Dictionary<string, int>();
                                    //    foreach (DataRow duk in mytable10.Rows)
                                    //    {
                                    //        string performerEND = duk["performer"].ToString();
                                    //        string title = duk["filename"].ToString();
                                    //        string retVal = text.Replace(tokens[0], "");
                                    //        int calc = myclass.CalculateSimilarity(title, retVal);

                                    //        result.Add("\U0001F3B5" + title + "\U0001F464" + performerEND,calc);

                                    //        if (result.Count > 70) break;
                                    //    }
                                    //    var result1 = from pair in result orderby pair.Value descending select pair;

                                    //    foreach(KeyValuePair<string,int> pair in result1)
                                    //    {
                                    //        resultend.Add(pair.Key, pair.Value);
                                    //    }



                                    //else if (myclass.PerformerCheck(tokens[1],80))
                                    //{

                                    //    SqlConnection myconnection10 = new SqlConnection();
                                    //    myconnection10.ConnectionString = connection;
                                    //    SqlCommand mycommand10 = new SqlCommand();
                                    //    mycommand10.Connection = myconnection10;
                                    //    DataTable mytable10 = new DataTable();
                                    //    SqlDataAdapter myadapter10 = new SqlDataAdapter();
                                    //    mycommand10.CommandText = "select filename,performer from [melo] where [filename] like '%" + tokens[0] + "%'" + "and [performer] ='" + tokens[1] + "'";
                                    //    myconnection10.Open();
                                    //    myadapter10.SelectCommand = mycommand10;
                                    //    myadapter10.Fill(mytable10);
                                    //    myconnection10.Close();

                                    //    List<string> result = new List<string>();
                                    //    foreach (DataRow duk in mytable10.Rows)
                                    //    {
                                    //        string performerEND = duk["performer"].ToString();
                                    //        string title = duk["filename"].ToString();
                                    //        result.Add("\U0001F3B5" + title + "\U0001F464" + performerEND);
                                    //        if (result.Count > 70) break;
                                    //    }
                                    //    bot.SendTextMessageAsync(chatidmovaqat, mytable10.Rows.Count.ToString() + "مورد خاص یافت شد که ممکنه مطابق نباشه:", 0, false, false, 0, Class1.ButtonOBJEee(result));

                                    //}

                                    //else if (myclass.PerformerCheck(tokens.Last(), 80))
                                    //{

                                    //    //SqlConnection myconnection10 = new SqlConnection();
                                    //    //myconnection10.ConnectionString = connection;
                                    //    //SqlCommand mycommand10 = new SqlCommand();
                                    //    //mycommand10.Connection = myconnection10;
                                    //    //DataTable mytable10 = new DataTable();
                                    //    //SqlDataAdapter myadapter10 = new SqlDataAdapter();
                                    //    mycommand10.CommandText = "select filename,performer from [melo] where [performer] like'%" + tokens.Last() + "%'";
                                    //    myconnection10.Open();
                                    //    myadapter10.SelectCommand = mycommand10;
                                    //    myadapter10.Fill(mytable10);
                                    //    myconnection10.Close();

                                    //    Dictionary<string, int> result = new Dictionary<string, int>();
                                    //    Dictionary<string, int> resultend = new Dictionary<string, int>();
                                    //    foreach (DataRow duk in mytable10.Rows)
                                    //    {
                                    //        string performerEND = duk["performer"].ToString();
                                    //        string title = duk["filename"].ToString();
                                    //        string retVal = text.Replace(tokens.Last(), "");
                                    //        int calc = myclass.CalculateSimilarity(title, retVal);
                                    //        result.Add("\U0001F3B5" + title + "\U0001F464" + performerEND, calc);
                                    //        if (result.Count > 70) break;
                                    //    }
                                    //    var result1 = from pair in result orderby pair.Value descending select pair;
                                    //    foreach (KeyValuePair<string, int> pair in result1)
                                    //    {
                                    //        resultend.Add(pair.Key, pair.Value);
                                    //    }
                                    //    bot.SendTextMessageAsync(chatidmovaqat, mytable10.Rows.Count.ToString() + "مورد خاص یافت شد که ممکنه مطابق نباشه:", 0, false, false, 0, Class1.ButtonOBJEee(resultend));

                                    //}
                                    #endregion

                                    else
                                    {
                                    if (count < 70)
                                    {
                                        string[] result = new string[count];
                                        string fileid = "0";
                                        int k = 0;
                                        foreach (DataRow duck in mytable.Rows)
                                        {
                                            fileid = duck["fileid"].ToString();
                                            string duration = duck["duration"].ToString();
                                            string performer = duck["performer"].ToString();
                                            string title = duck["filename"].ToString();
                                            result[k] = "\U0001F3B5" + title + "\U0001F464"+performer;
                                            k++;
                                        }
                                        bot.SendTextMessageAsync(chatidmovaqat, mytable.Rows.Count.ToString() + "مورد مشابه یافت شد:", 0, false, false, 0, Class1.ButtonCreator(result));
                                    }
                                    else if (count >= 70)
                                    {
                                        string[] result = new string[72];
                                        string fileid = "0";
                                        int k = 0;
                                        foreach (DataRow duck in mytable.Rows)
                                        {
                                            fileid = duck["fileid"].ToString();
                                            string duration = duck["duration"].ToString();
                                            string performer = duck["performer"].ToString();
                                            string title = duck["filename"].ToString();
                                            result[k] = "\U0001F3B5" + title+"\U0001F464" + performer; ;
                                            k++;
                                            if (k > 70)
                                            {
                                                result[71] = "\U0001F50D" + "ادامه جستجو" + e.Message.Text;
                                                break;
                                            }
                                        }
                                        bot.SendTextMessageAsync(chatidmovaqat, mytable.Rows.Count.ToString() + "مورد مشابه یافت شد:", 0, false, false, 0, Class1.ButtonCreator(result));
                                    }
                                }

                            }
                            else if (e.Message.Text.Contains("\U0001F50D" + "ادامه جستجو"))
                            {
                                text = e.Message.Text.Replace("\U0001F50D" + "ادامه جستجو", "");
                                mycommand.CommandText = "select filename from [melo] where [filename] like '%" + text + "%'" + " or [performer] like '%" +text+ "%'";
                                myadapter.SelectCommand = mycommand;
                                myconnection.Open();
                                myadapter.Fill(mytable);
                                myconnection.Close();                            
                                    string[] result = new string[mytable.Rows.Count-70];
                                int k = 0;
                                int i = 0;
                                foreach (DataRow duck in mytable.Rows)
                                {
                                    if (k < 70) { k++; continue; }                                 
                                    string title = duck["filename"].ToString();
                                    result[i] = "\U0001F3B5" + title;
                                    i++;
                                    if (i > mytable.Rows.Count)
                                    {                                      
                                        break;
                                    }
                                }
                                bot.SendTextMessageAsync(chatidmovaqat,  "مابقی:دی", 0, false, false, 0, Class1.ButtonCreator(result));
                                
                            }
                        }
                        catch
                        {
                            bot.SendTextMessageAsync(e.Message.Chat.Id, "nabud");
                        }
                    }                    
                } 
            }
            catch(Exception ex) {if(chatidmovaqat=="71869354"|| chatidmovaqat == "99270771"|| chatidmovaqat == "462060640") bot.SendTextMessageAsync(chatidmovaqat, ex.ToString(),0,false,true,e.Message.MessageId); }
            
        }
    }
}
