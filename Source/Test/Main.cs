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
			
			
			Authorization a1 = new Authorization ("100", "sundowndk", string.Empty);
			a1.Save ();
			
//			Authorization a2 = Authorization.Load (a1.Id);
//			Console.WriteLine (a2.Username);
			
			foreach (Authorization a in Authorization.List ())
			{
				Console.WriteLine (a.Username);
			}
			
			foreach (Authorization a in Authorization.List ())
			{
				Authorization.Delete (a.Id);
			}
			
			
//			Console.WriteLine (a1.Username);
		}
	}
}
