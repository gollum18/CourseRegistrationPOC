<%@ Page Title="Coursely - Login" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Coursely.Content.Web.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var statusLabel = null;

        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        function validateLogin() {
            if (isEmptyOrWhiteSpace($("#TextID").val()) || 
                    isEmptyOrWhiteSpace($("#TextPassword").val()))
            {
                statusLabel.text("Error: You have entered an invalid username or password!");
                statusLabel.css("color", "red");
                return;
            }
            doPostBack();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="form-group">
        <label for="TextID">University ID: </label>
        <asp:TextBox ID="TextID" runat="server" ClientIDMode="Static"></asp:TextBox>    
        <label for="TextPassword"></label>
        <asp:TextBox ID="TextPassword" TextMode="Password" runat="server" ClientIDMode="Static"></asp:TextBox>
        <input type="button" id="ButtonLogin" onclick="validateLogin();" value="Login"/>
    </div>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" ForeColor="Red" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
