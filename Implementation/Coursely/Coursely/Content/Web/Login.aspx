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
    <div class="container col-3">
        <div class="form-group row">
            <label for="TextID" class="col-form-label">University ID: </label>
            <asp:TextBox ID="TextID" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:TextBox>    
        </div>
        <div class="form-group row">
            <label for="TextPassword" class="col-form-label">Password: </label>
            <asp:TextBox ID="TextPassword" CssClass="form-control" TextMode="Password" runat="server" ClientIDMode="Static"></asp:TextBox>
        </div>
        <div class="form-group row">
            <input type="button" class="form-control btn btn-primary" id="ButtonLogin" onclick="validateLogin();" value="Login"/>
        </div>
    </div>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" ForeColor="Red" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
