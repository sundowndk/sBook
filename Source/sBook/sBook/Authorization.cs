//
// Authorization.cs
//
// Author:
//       Rasmus Pedersen <rasmus@akvaservice.dk>
//
// Copyright (c) 2010 Rasmus Pedersen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using SNDK;

using Facebook;

namespace sBook
{
	public class Authorization
	{
		#region Public Static Fields
		public static string DatastoreAisle = "sbook_authorizations";
		#endregion
		
		#region Private Fields
		private Guid _id;
		private Token _token;
		#endregion
		
		#region Public Fields
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}
		
		public string FacebookAppId
		{
			get
			{
				return this._token.FacebookAppId;
			}
		}
		
		public string FacebookUserId
		{
			get
			{
				return this._token.FacebookUserId;
			}
		}
		
		public string FacebookAppName
		{
			get
			{
				return this._token.FacebookAppName;
			}
		}
		
		public string FacebookUserName
		{
			get
			{
				return this._token.FacebookUserName;
			}
		}
		
		public Enums.TokenPermission Permissions
		{
			get
			{
				return this._token.Permissions;
			}
		}
		
		public bool Valid
		{
			get
			{
				return this._token.Valid;
			}
		}
		
		public Token Token
		{
			get
			{
				return this._token;
			}
		}
		#endregion
		
		#region Constructor	
		public Authorization (Token Token)
		{
			this._id = Guid.NewGuid ();
			this._token = Token;
		}			
		
		private Authorization ()
		{
		}			
		#endregion
		
		#region Public Methods
		public void Save ()
		{
			try
			{
				Hashtable item = new Hashtable ();

				item.Add ("id", this._id);
				item.Add ("token", this._token);
				
//				Services.Datastore.Meta meta = new Services.Datastore.Meta ();
//				meta.Add ("id", this._id);
//				meta.Add ("username", this._username);
//				meta.Add ("email", this._email);
//
				SorentoLib.Services.Datastore.Set (DatastoreAisle, this._id.ToString (), SNDK.Convert.ToXmlDocument (item, this.GetType ().FullName.ToLower ()));
			}
			catch (Exception exception)
			{
				// LOG: LogDebug.ExceptionUnknown
				SorentoLib.Services.Logging.LogDebug (string.Format (SorentoLib.Strings.LogDebug.ExceptionUnknown, "SBOOK.AUTHORIZATION", exception.Message));

				// EXCEPTION: Exception.Authorization
				throw new Exception (string.Format (Strings.Exception.AuthorizationSave, this._id.ToString ()));
			}			
		}
		
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();

			result.Add ("id", this._id);
			result.Add ("facebookappname", this.FacebookAppName);
			result.Add ("facebookusername", this.FacebookUserName);
			result.Add ("permissions", this.Permissions);
			result.Add ("valid", this.Valid);
			result.Add ("token", this._token);

			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}
		#endregion
		
		#region Public Static Methods
		public static Authorization Load (Guid id)
		{
			Authorization result;
			
			try
			{
				Hashtable item = (Hashtable)SNDK.Convert.FromXmlDocument (SNDK.Convert.XmlNodeToXmlDocument (SorentoLib.Services.Datastore.Get<XmlDocument> (DatastoreAisle, id.ToString ()).SelectSingleNode ("(//sbook.authorization)[1]")));
				result = new Authorization ();

				result._id = new Guid ((string)item["id"]);

				if (item.ContainsKey ("token"))
				{
					result._token = Token.FromXmlDocument ((XmlDocument)item["token"]);
				}		
			}
			catch (Exception exception)
			{
				// LOG: LogDebug.ExceptionUnknown
				SorentoLib.Services.Logging.LogDebug (string.Format (SorentoLib.Strings.LogDebug.ExceptionUnknown, "SBOOK.AUTHORIZATION", exception.Message));

				// EXCEPTION: Excpetion.AuthorizationLoad
				throw new Exception (string.Format (Strings.Exception.AuthorizationLoad, id.ToString ()));
			}
			
			return result;
		}	
		
		public static void Delete (Guid Id)
		{
			try
			{
				SorentoLib.Services.Datastore.Delete (DatastoreAisle, Id.ToString ());
			}
			catch (Exception exception)
			{
				// LOG: LogDebug.ExceptionUnknown
				SorentoLib.Services.Logging.LogDebug (string.Format (SorentoLib.Strings.LogDebug.ExceptionUnknown, "SBOOK.AUTHORIZATION", exception.Message));

				// EXCEPTION: Exception.AuthorizationDelete
				throw new Exception (string.Format (Strings.Exception.AuthorizationDelete, Id.ToString ()));
			}
		}

		public static List<Authorization> List ()
		{
			List<Authorization> result = new List<Authorization> ();

			foreach (string id in SorentoLib.Services.Datastore.ListOfShelfs (DatastoreAisle))
			{
				try
				{
					result.Add (Load (new Guid (id)));
				}
				catch (Exception exception)
				{
					// LOG: LogDebug.ExceptionUnknown
					SorentoLib.Services.Logging.LogDebug (string.Format (SorentoLib.Strings.LogDebug.ExceptionUnknown, "SBOOK.AUTHORIZATION", exception.Message));
					
					// LOG: LogDebug.AuthorizationList
					SorentoLib.Services.Logging.LogDebug (string.Format (Strings.LogDebug.AuthorizationList, id));
				}
			}

			return result;
		}
		
		public static Authorization FromXmlDocument (XmlDocument xmlDocument)
		{
			Hashtable item;
			Authorization result;

			try
			{
				item = (Hashtable)SNDK.Convert.FromXmlDocument (SNDK.Convert.XmlNodeToXmlDocument (xmlDocument.SelectSingleNode ("(//sbook.authorization)[1]")));
			}
			catch
			{
				item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			}

			if (item.ContainsKey ("id"))
			{
				try
				{
					result = Load (new Guid ((string)item["id"]));
				}
				catch
				{
					result = new Authorization ();
					result._id = new Guid ((string)item["id"]);
				}
			}
			else
			{
				// EXCEPTION: Exception.AuthorizationFromXMLDocument
				throw new Exception (Strings.Exception.AuthorizationXMLDocument);
			}
			
			if (item.ContainsKey ("token"))
			{
					result._token = Token.FromXmlDocument ((XmlDocument)item["token"]);
			}
			
			return result;
		}				
		#endregion
	}
}

