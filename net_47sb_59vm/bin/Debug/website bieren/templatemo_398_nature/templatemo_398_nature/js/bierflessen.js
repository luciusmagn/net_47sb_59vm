function Showbeer(img) {

	if(img.cells[0].childNodes[0])
	{
		img.cells[0].childNodes[0].style.display = "";
	}
};

function Hidebeer(img) {
	if(img.cells[0].childNodes[0])
	{
		img.cells[0].childNodes[0].style.display = "none";
	}
};