// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $(document).keydown(function (e) {

        //All Operations
        if (e.key === "+") {
            document.getElementById("btn_plus").click();
        }

        else if (e.key === "-") {
            document.getElementById("btn_minus").click();
        }

        else if (e.key === "*") {
            document.getElementById("btn_multiply").click();
        }

        else if (e.key === "/") {
            document.getElementById("btn_divide").click();
        }

        else if (e.key === "(" || e.key === ")") {
            document.getElementById("btn_parenthesis").click();
        }

        else if (e.key === "Enter" || e.key === "=") {
            document.getElementById("btn_equals").click();
        }

        else if (e.key === "." || e.key === ",") {
            document.getElementById("btn_dot").click();
        }

        else if (e.key === "%") {
            document.getElementById("btn_percentage").click();
        }

        else if (e.key === "C" || e.key === "c") {
            document.getElementById("btn_clear").click();
        }

        else if (e.key === "Delete") {
            document.getElementById("btn_delete").click();
        }

        if (!isNaN(e.key)) { // If number is clicked
            var btnName = "btn_" + e.key;
            document.getElementById(btnName).click();
        }
    });
});
