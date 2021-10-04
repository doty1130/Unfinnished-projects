function loadJsonData()
{
var postdata = JSON.stringify(
	{
	"From": document.getElementById("TxtFrom").value,
	"To": document.getElementById("TxtTo").value,
	"Body": document.getElementById("TxtBody").value
	});
	
	try{
		$.ajax({
			type: "POST",
			url: "MailHandler.ashx",
			cache: false,
			data: postdata,
			dataType:"json",
			success:getSuccess,
			error: getFail
		});
	} catch(e){alert(e);}
	function getSuccess(data, textStatus, jqXHR){
		alert(data.Response);
	};
	function getFail(jqXHR, textStatus, errorThrown)
	{
		alert(jqXHR.status);
	};
};