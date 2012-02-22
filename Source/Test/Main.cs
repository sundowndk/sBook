using System;

using sBook;
using SNDK;
using SNDK.DBI;
using SNDK.Enums;
using SorentoLib;

namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			SorentoLib.Services.Database.Connection = new Connection (SNDK.Enums.DatabaseConnector.Mysql,
			                                                            "localhost",
//			                                                            "10.0.0.40",
			                                                            "sorento",
//								"sorentotest.sundown.dk",
			                                                            "sorentotest",
			                                                            "scumbukket",
			                                                            true);

			SorentoLib.Services.Database.Prefix = "sorento_";
			SorentoLib.Services.Database.Connection.Connect ();

			SorentoLib.Services.Config.Initialize ();
			
			
//			sBook.Enums.TokenPermission p1 = sBook.Enums.TokenPermission.offline_access | sBook.Enums.TokenPermission.publish_stream;
//			sBook.Enums.TokenPermission p2 = sBook.Enums.TokenPermission.none;
//			
//			p2 = p2 | SNDK.Convert.StringToEnum<sBook.Enums.TokenPermission> ("publish_stream");
//			
//			Console.WriteLine (p2); 
//			
//			
//			Environment.Exit (0);
//			
//			Token t1 = new Token ("AAADiiimgZCEQBAC5lzZARxJbjlW0g5C2ZCGc7LrOJm6aqkucdhZBYcMM25x0er4h8PWW3wIG4qpynl6ozLw1xqqWdprsvpUZD");
//			Authorization a1 = new Authorization (t1);
			
			
			
//			a1.Save ();
			
//			Console.WriteLine (a1.FacebookAppName);
//			Console.WriteLine (a1.FacebookUsername);
//			Console.WriteLine (a1.Token.Permissions);
			
			
//			a1.Save ();
			
			
			
			
//			Authorization a1 = new Authorization ("100", "sundowndk", string.Empty);
//			a1.Save ();
			
//			Authorization a2 = Authorization.Load (a1.Id);
//			Console.WriteLine (a2.Username);
			
			foreach (Authorization a in Authorization.List ())
			{
				Console.WriteLine (a.FacebookAppName +"   "+ a.Token.Valid);				
//				Console.WriteLine (a.Token.Valid);
			}
//			
//			foreach (Authorization a in Authorization.List ())
//			{
//				Authorization.Delete (a.Id);
//			}
			
			
//			Console.WriteLine (a1.Username);
		}
	}
}
