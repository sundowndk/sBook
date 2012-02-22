new : function (token)
{
	var content = new Array ();
	content["token"] = token;

	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=sBook.Authorization.New", "data", "POST", false);
	request.send (content);

	return request.respons ()["sbook.authorization"];
},

load : function (id)
{
	var content = new Array ();
	content["id"] = id;

	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=sBook.Authorization.Load", "data", "POST", false);	
	request.send (content);

	return request.respons ()["sbook.authorization"];
},

save : function (authorization)
{					
	var content = new Array ();
	content["sbook.authorization"] = form;

	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=sBook.Authorization.Save", "data", "POST", false);		
	request.send (content);

	return true;
},		

delete : function (id)
{
	var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=sBook.Authorization.Delete", "data", "POST", false);	

	var content = new Array ();
	content["id"] = id;

	request.send (content);

	return true;
},

list : function (attributes)
{
	if (!attributes) attributes = new Array ();

	if (attributes.async)
	{
		var ondone = 	function (respons)
						{						
							attributes.onDone (request.respons ()["sbook.authorizations"]);
						};
						
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=sBook.Authorization.List", "data", "POST", true);
		request.onLoaded (ondone);
		request.send ();		
	}
	else
	{
		var request = new SNDK.ajax.request ("/", "cmd=Ajax;cmd.function=sBook.Authorization.List", "data", "POST", false);		
		request.send ();		
		
		return request.respons ()["sbook.authorizations"];
	}
}	
