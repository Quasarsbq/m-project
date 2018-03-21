using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Data = System.Collections.Generic.KeyValuePair<string, int>;

namespace WebApplication_ba_API.Models
{
    public class Class1
    {
       
       // string connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\Documents\demoDB.mdf;Integrated Security=True;Connect Timeout=30";
       string connection = @"workstation id=meloLOW.mssql.somee.com;packet size=4096;user id=Projectjson_SQLLogin_1;pwd=lh5s8ra91o;data source=meloLOW.mssql.somee.com;persist security info=False;initial catalog=meloLOW";
        public bool checkdublicate(string Filename,string PerformerIN)
        {
            SqlConnection myconnection = new SqlConnection();
            myconnection.ConnectionString = connection;
            SqlCommand mycommand = new SqlCommand();
            mycommand.Connection = myconnection;
            mycommand.CommandText = "SELECT count(*) FROM   [melo] where [filename]=@filename and performer=@performer";
            mycommand.Parameters.AddWithValue("@filename", Filename);
            mycommand.Parameters.AddWithValue("@performer", PerformerIN);
            int count = 0;
            myconnection.Open();
            count = (Int32)mycommand.ExecuteScalar();
            myconnection.Close();
            if (count == 0)
                return false;
            else 
                return true;
        }
        public static Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup ButtonCreator(string[] lst)
        {
            int rowsCount = lst.Length;
            Telegram.Bot.Types.KeyboardButton[][] buttons = new Telegram.Bot.Types.KeyboardButton[rowsCount][];
            for (int i = 0; i < rowsCount; i++)
            {
              buttons[i] = new Telegram.Bot.Types.KeyboardButton[1];
              buttons[i][0] = lst[i];
            }
            Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup result = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttons);
            result.OneTimeKeyboard = true;
            return result;
        }

        public bool PerformerCheck(string matn_srch,int darsadtashaboh)
        {
            string textORG = matn_srch;
            int count=0;
            SqlConnection myconnection = new SqlConnection();
            myconnection.ConnectionString = connection;
            SqlCommand mycommand = new SqlCommand();
            try
            {
                for (int i = 0; i < matn_srch.Length + 1; i = i + 2)
                {
                    matn_srch = matn_srch.Insert(i, "%");
                }


                mycommand.Connection = myconnection;
                mycommand.CommandText = "select performer from [melo] where [performer] like'" + matn_srch.Replace("'", "''")+"'";
                DataTable mytable = new DataTable();
                SqlDataAdapter myadapter = new SqlDataAdapter();
                myconnection.Open();
                myadapter.SelectCommand = mycommand;
                myadapter.Fill(mytable);

                myconnection.Close();
                foreach (DataRow duck in mytable.Rows)
                {
                    int iii = CalculateSimilarity(duck["performer"].ToString(), textORG);
                    if (CalculateSimilarity(duck["performer"].ToString(), textORG) > darsadtashaboh)
                    { count++; }

                }
            }
            catch
            {                
            }
            myconnection.Close();
            if (count == 0)
                return false;
            else     
                return true;
        }


        public static Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup ButtonCreatorObjecti(List<string> filename, List<string> performer)
        {
            int rowsCount = filename.Count;
            Telegram.Bot.Types.KeyboardButton[][] buttons = new Telegram.Bot.Types.KeyboardButton[rowsCount][];
            for (int i = 0; i < rowsCount; i++)
            {
                if (filename[i] != "")
                {
                    buttons[i] = new Telegram.Bot.Types.KeyboardButton[1];
                    buttons[i][0] = "\U0001F3B5" + filename[i] + "\U0001F464" + performer[i];
                }
                else
                {
                    buttons[i] = new Telegram.Bot.Types.KeyboardButton[1];
                    buttons[i][0] = "\U0001F464" + performer[i];
                }
                
            }
            Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup result = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttons);
            result.OneTimeKeyboard = true;
            return result;
        }

        public static Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup ButtonCreator1(List<string> performer)
        {
            int rowsCount = performer.Count;
            Telegram.Bot.Types.KeyboardButton[][] buttons = new Telegram.Bot.Types.KeyboardButton[rowsCount][];
            for (int i = 0; i < rowsCount; i++)
            {
                buttons[i] = new Telegram.Bot.Types.KeyboardButton[1];
                buttons[i][0] = "\U0001F464" + performer[i];
            }
            Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup result = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttons);
            result.OneTimeKeyboard = true;
            return result;
        }

        public static Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup ButtonOBJEee(List<Data> listkamel)
        {
            int rowsCount = listkamel.Count;
            if (rowsCount < 70)
            {
                Telegram.Bot.Types.KeyboardButton[][] buttons = new Telegram.Bot.Types.KeyboardButton[rowsCount][];
                int i = 0;
                foreach (KeyValuePair<string, int> pair in listkamel)
                {
                    buttons[i] = new Telegram.Bot.Types.KeyboardButton[1];
                    buttons[i][0] = pair.Key;
                    i++;
                    if (i > rowsCount-1) break;
                }
                Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup result = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttons);
                result.OneTimeKeyboard = true;
                return result;
            }
            else
            {
                Telegram.Bot.Types.KeyboardButton[][] buttons = new Telegram.Bot.Types.KeyboardButton[70][];
                int i = 0;
                foreach (KeyValuePair<string, int> pair in listkamel)
                {
                    buttons[i] = new Telegram.Bot.Types.KeyboardButton[1];
                    buttons[i][0] = pair.Key;
                    i++;
                    if (i > 69) break;
                }
                Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup result = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(buttons);
                result.OneTimeKeyboard = true;
                return result;
            }
            
        }



        public int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }




        public int CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return 100;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            int tttt= Convert.ToInt32((1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length))) * 100);
            return Convert.ToInt32(        100* (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)))         );
        }

        public void NewRate(string id,int sum)
        {
            SqlConnection myconnection2 = new SqlConnection();
            myconnection2.ConnectionString = connection;
            SqlCommand mycommand2 = new SqlCommand();
            mycommand2.Connection = myconnection2;
            DataTable mytable2 = new DataTable();
            SqlDataAdapter myadapter2 = new SqlDataAdapter();
            mycommand2.CommandText = "update [melo] set rate=@rate where [fileid] ='"+id+"'";
            mycommand2.Parameters.AddWithValue("@rate", sum+1);
            myconnection2.Open();
            mycommand2.ExecuteScalar();
            myconnection2.Close();
        }
        public bool meloiiExistancy(string chatid)
        {

            int count = 0;
            try
            {
                SqlConnection myconnection = new SqlConnection();
                myconnection.ConnectionString = connection;               
                SqlCommand mycommand = new SqlCommand();                
                mycommand.Connection = myconnection;
                mycommand.CommandText = "SELECT COUNT(*) FROM   [meloii] where [chatid]=" + chatid;
                myconnection.Open();
                count = (Int32)mycommand.ExecuteScalar();
                myconnection.Close();
            }
            catch { }


            if (count == 0)
            {
                return false;
            }
            else return true;
        }
        public void UpdateLastUse(string chatid)
        {
            if (meloiiExistancy(chatid) == false)
            {
                SqlConnection myconnection = new SqlConnection();
                myconnection.ConnectionString = connection;
                SqlCommand mycommand = new SqlCommand();
                mycommand.Connection = myconnection;
                mycommand.CommandText = "insert into [meloii] (chatid) values("+chatid+")";
                myconnection.Open();
                mycommand.ExecuteNonQuery();
                myconnection.Close();
            }
            else
            {
                SqlConnection myconnection = new SqlConnection();
                myconnection.ConnectionString = connection;
                SqlCommand mycommand = new SqlCommand();
                mycommand.Connection = myconnection;
                mycommand.CommandText = "update [meloii] set tarix= getdate() where chatid="+chatid;
                myconnection.Open();
                mycommand.ExecuteNonQuery();
                myconnection.Close();
            }
        }


    }

    
}