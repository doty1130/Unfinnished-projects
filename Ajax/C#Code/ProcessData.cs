using System.IO;
using System.Web.Script.Serialization;

class ProcessData{
	
	public void ProcessRequest(HttpContext context)
	{
		string jsonString = String.Empty;
		HttpContext.Current.Request.InputStream.Position = 0;
		using (StreamReader inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
		{
			jsonString = inputStream.ReadToEnd();
			JavaScriptSerializer jSerialize = new JavaScriptSerializer();
			var email = jSerialize.Deserialize<mail>(jsonString);
			
			if(email != null)
			{
				
				string from = email.From;
				string to = email.To;
				string body = email.Body;
				//You can write here the code to send Email.
				//see, the class System.Net.Mail.MailMessage on MSDN
				//Once the Mail is sent succefully, you can send back
				//a response to the Client informing him that everything is okay!
				context.Response.Write(jSerialize.Serialize(
				new {
					Response = "Message Has been sent successfully"
				}));
				
			}
		}
	}
	
}