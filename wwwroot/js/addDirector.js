var checkbox = document.getElementById("checkbox");
var dropdown = document.getElementById("dropdown");
var textbox = document.getElementById("textbox");

function toggleCheck()
{
    dropdown.classList.toggle("inactive");
    textbox.classList.toggle("inactive");
    textbox.value = "";
    dropdown.value = "";
}