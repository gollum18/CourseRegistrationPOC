<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Coursely.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        "use strict";

        function isEmptyOrWhiteSpace(str){
            return str === null || str.match(/^ *$/) !== null;
        }

        /*
         * Toggles the display of a tab.
         */
        function toggleTabContent(id) {
            // Hide all of the tab-content elements
            var elements = document.getElementsByClassName("tab-content");
            for (var i = 0; i < elements.length; i++) {
                elements[i].style.display = "none";
            }
            // Toggle the visibility of the tabs contant 
            document.getElementById(id).style.display = "block";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
