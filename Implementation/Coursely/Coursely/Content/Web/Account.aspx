<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="Coursely.Content.Web.Account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=ResolveUrl("~/Content/JS/UserValidation.js") %>" type="text/javascript"></script>
    <script>
        var statusLabel = null;

        // Load in the status label when the page loads
        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        /*
         * Validates and updates the users password.
         */
        function updatePassword() {
            // Validate the old password
            var validationResult = validatePassword($("#CurrentPasswordTextBox").val(),
                $("#NewPasswordTextBox").val(), $("#ConfirmPasswordTextBox").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Clear out the password
            currentPassword = "";
            newPassword = "";
            confirmPassword = "";

            doPostBack('UpdatePassword');
        }

        /*
         * 
         */
        function updateEmail() {
            var oldEmail = $("#CurrentEmailTextBox").val();
            var newEmail = $("#NewEmailTextBox").val();
            var confirmEmail = $("#ConfirmEmailTextBox").val();
            var validationResult = null;

            validationResult = validateEmail(oldEmail);
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            validationResult = validateEmail(newEmail);
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            validationResult = validateEmail(confirmEmail);
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            if (newEmail !== confirmEmail) {
                statusLabel.css("color", "red");
                statusLabel.text("Error: Confirmation password does not match new password!");
                return;
            }

            doPostBack('UpdateEmail');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <label for="UnivIDTextBox" class="col-form-label">University Identifer: </label>
                <input id="UnivIDTextBox" type="text" class="form-control" readonly="readonly" runat="server"/>
            </div>
            <div class="col">
                <label for="RoleTextBox" class="col-form-label">Role: </label>
                <input id="RoleTextBox" type="text" class="form-control" readonly="readonly" runat="server"/>
            </div>
            <div class="col">
                <label for="NameTextBox" class="col-form-label">Name: </label>
                <input id="NameTextBox" type="text" class="form-control" readonly="readonly" runat="server"/>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="EmailTextBox" class="col-form-label">Email: </label>
                <input id="CurrentEmailTextBox" class="form-control" type="email" readonly="readonly" runat="server"/>
            </div>
            <div class="col">
                <label for="NewEmailTextBox" class="col-form-label">New Email: </label>
                <input id="NewEmailTextBox" class="form-control" type="email" runat="server"/>
            </div>
            <div class="col">
                <label for="ConfirmEmailTextBox" class="col-form-label">Confirm Email: </label>
                <input id="ConfirmEmailTextBox" class="form-control" type="email" runat="server"/>
            </div>
        </div>
        <div class="row justify-content-center">
            <input id="UpdateEmailButton" type="button" class="btn btn-primary col-4 mt-2" onclick="updateEmail();" value="Update Email"/>
        </div>
        <div class="row">
            <div class="col">
                <label for="CurrentPasswordTextBox" class="col-form-label">Current Password: </label>
                <asp:TextBox ID="CurrentPasswordTextBox" CssClass="form-control" ClientIDMode="Static" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="NewPasswordTextBox" class="col-form-label">New Password: </label>
                <asp:TextBox ID="NewPasswordTextBox" CssClass="form-control" ClientIDMode="Static" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="ConfirmPasswordTextBox" class="col-form-label">Confirm Password: </label>
                <asp:TextBox ID="ConfirmPasswordTextBox" CssClass="form-control" ClientIDMode="Static" TextMode="Password" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row justify-content-center">
            <input id="UpdatePasswordButton" type="button" class="btn btn-primary col-4 mt-2 mb-2" onclick="updatePassword();" value="Update Password"/>
        </div>
        <div class="row justify-content-center">
            <asp:Label ID="StatusLabel" CssClass="col" Font-Bold="true" Font-Size="Medium" ClientIDMode="Static" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
