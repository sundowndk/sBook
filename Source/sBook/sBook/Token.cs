//
// Token.cs
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
	public class Token
	{				
		#region Private Fields		
		private string _facebookappid;
		private string _facebookappname;
		private string _facebookuserid;	
		private string _facebookusername;
		private string _facebooktoken;	
		private Enums.TokenPermission _permissions;
		private bool _valid;
		#endregion
		
		#region Public Fields
		public string FacebookAppId
		{
			get
			{
				return this._facebookappid;
			}
		}
		
		public string FacebookUserId
		{
			get
			{
				return this._facebookuserid;
			}
		}
		
		public string FacebookToken
		{
			get
			{
				return this._facebooktoken;
			}
		}
		
		public string FacebookAppName
		{
			get
			{
				return this._facebookappname;
			}
		}		
		
		public string FacebookUserName
		{
			get
			{
				return this._facebookusername;
			}
		}		
		
		public Enums.TokenPermission Permissions
		{
			get
			{
				return this._permissions;
			}
		}
		
		public bool Valid
		{
			get
			{
				return this._valid;
			}
		}		
		#endregion
		
		#region Constructor	
		public Token (string Token)
		{			
			this._facebooktoken = Token;
			this._facebookappid = string.Empty;
			this._facebookappname = string.Empty;
			this._facebookuserid = string.Empty;
			this._facebookusername = string.Empty;
			this._permissions = Enums.TokenPermission.none;
			this._valid = false;
			
			Update ();
		}			
		
		private Token ()
		{
			this._facebooktoken = string.Empty;		
			this._facebookappid = string.Empty;
			this._facebookappname = string.Empty;
			this._facebookuserid = string.Empty;
			this._facebookusername = string.Empty;
			this._permissions = Enums.TokenPermission.none;
			this._valid = false;
		}
		#endregion
		
		#region Private Methods
		private void Update ()
		{
			FacebookClient facebookclient = new FacebookClient (this._facebooktoken);
			
			try
			{				
				try
				{
					this._facebookuserid = (string)((IDictionary<string, object>)facebookclient.Get("/me"))["id"];	
					this._valid = true;
				}
				catch
				{				
				}
				
				if (this._valid)
				{									
					this._facebookappid = (string)((IDictionary<string, object>)facebookclient.Get("/app"))["id"];
					this._facebookuserid = (string)((IDictionary<string, object>)facebookclient.Get("/me"))["id"];	
					
					foreach (Facebook.JsonObject data in (Facebook.JsonArray)((IDictionary<string, object>)facebookclient.Get("/me/permissions"))["data"])
					{    
						foreach (KeyValuePair<string, object> permission in data) 
						{ 
							if (permission.Value.ToString () == "1")
							{
								this._permissions = this._permissions | SNDK.Convert.StringToEnum<Enums.TokenPermission> (permission.Key.ToString ());
							}
						}
					} 
				}
				else
				{
					facebookclient = new FacebookClient ();	
				}
				
				this._facebookappname = (string)((IDictionary<string, object>)facebookclient.Get (this._facebookappid))["name"];
				this._facebookusername = (string)((IDictionary<string, object>)facebookclient.Get (this._facebookuserid))["name"];
			}
			catch (Exception e)
			{
			}
		}
		#endregion
		
		#region Public Methods
		public XmlDocument ToXmlDocument ()
		{
			Hashtable result = new Hashtable ();

			result.Add ("facebookappid", this._facebookappid);
			result.Add ("facebookuserid", this._facebookuserid);
			result.Add ("facebooktoken", this._facebooktoken);
			result.Add ("permissions", this._permissions);

			return SNDK.Convert.ToXmlDocument (result, this.GetType ().FullName.ToLower ());
		}
		#endregion
		
		#region Public Static Methods
		public static Token FromXmlDocument (XmlDocument xmlDocument)
		{
			Hashtable item;
			Token result = new Token ();

			try
			{
				item = (Hashtable)SNDK.Convert.FromXmlDocument (SNDK.Convert.XmlNodeToXmlDocument (xmlDocument.SelectSingleNode ("(//sbook.token)[1]")));
			}
			catch
			{
				item = (Hashtable)SNDK.Convert.FromXmlDocument (xmlDocument);
			}
			
			if (item.ContainsKey ("facebookappid"))
			{
				result._facebookappid = (string)item["facebookappid"];
			}
			
			if (item.ContainsKey ("facebookuserid"))
			{
				result._facebookuserid = (string)item["facebookuserid"];
			}
			
			if (item.ContainsKey ("facebooktoken"))
			{
				result._facebooktoken = (string)item["facebooktoken"];
			}
			
			result.Update ();
			
			return result;
		}				
		#endregion
	}
}

