$(document).ready(function () {         
    $("#filter-input").on("keyup click input", function () {
        if (this.value.length > 0) {
            $(".item-row").removeClass("match").hide().filter(function () {
                return $(this).text().toLowerCase().indexOf($("#filter-input").val().toLowerCase()) != -1;
            }).addClass("match").show();
        }
        else {
            $(".item-row").addClass("match").show();
        }
    });
    $('.ui.dropdown').dropdown({ 
        onChange: OnTagChanged,
    });
    var tag = GetTagFromURL();
    $('.ui.dropdown').dropdown('set selected', tag);
    OnTagChanged(tag, null, null);
});

function OnTagChanged(value, text, $selectedItem)
{
    if (value == 'all') 
    {
    	$(".item-row").show();
    	window.location.hash = "";
    	$('.ui.dropdown').dropdown('set selected', null);
    }
    else
	{
		window.location.hash = encodeURIComponent(value);
        $(".item-row").each(function(index, element) {
			var visible = false;
			$(this).find(".item-row-tag").each(function(index, element) {
                if ($(this).text() == value) visible = true;
            });
            
			if (visible) $(this).show();
			else $(this).hide();
        });
	}
	$("html, body").animate({ scrollTop: 0 }, "fast");
}

function GetTagFromURL()
{
    (document.getElementById) ? dom = true : dom = false;
	
	if (window.location.hash != "") 
	{
		return filter = decodeURIComponent(window.location.hash.substring(1));
	}
	else return "all";
}