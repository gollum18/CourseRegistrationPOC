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

        function toggleView(id) {
            var element = document.getElementById(id);
            if (element !== null) {
                if (element.style.display === 'block') {
                    element.style.display = 'none';
                } else {
                    element.style.display = 'block';
                }
            }
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
    <section>
        <section>
            <label for="UnivIDTextBox">University Identifer: </label>
            <input id="UnivIDTextBox" type="text" readonly="readonly" runat="server"/>
            <label for="RoleTextBox">Role: </label>
            <input id="RoleTextBox" type="text" readonly="readonly" runat="server"/>
            <label for="NameTextBox">Name: </label>
            <input id="NameTextBox" type="text" readonly="readonly" runat="server"/>
        </section>
        <section>
            <div id="EmailContainer" style="display:none;">
                <label for="EmailTextBox">Email: </label>
                <input id="CurrentEmailTextBox" type="email" runat="server"/>
                <label for="NewEmailTextBox">New Email: </label>
                <input id="NewEmailTextBox" type="email" runat="server"/>
                <label for="ConfirmEmailTextBox">Confirm Email: </label>
                <input id="ConfirmEmailTextBox" type="email" runat="server"/>
                <input id="UpdateEmailButton" type="button" onclick="updateEmail();" value="Update Email"/>
            </div>
            <input id="ToggleButtonEmail" type="button" onclick="toggleView('EmailContainer');" value="Toggle Update Email"/>
        </section>
        <section>
            <div id="PasswordContainer" style="display:none;">
                <label for="CurrentPasswordTextBox">Current Password: </label>
                <asp:TextBox ID="CurrentPasswordTextBox" ClientIDMode="Static" TextMode="Password" runat="server"></asp:TextBox>
                <label for="NewPasswordTextBox">New Password: </label>
                <asp:TextBox ID="NewPasswordTextBox" ClientIDMode="Static" TextMode="Password" runat="server"></asp:TextBox>
                <label for="ConfirmPasswordTextBox">Confirm Password: </label>
                <asp:TextBox ID="ConfirmPasswordTextBox" ClientIDMode="Static" TextMode="Password" runat="server"></asp:TextBox>
                <input id="UpdatePasswordButton" type="button" onclick="updatePassword();" value="Update Password"/>
            </div>
            <input id="TogglePasswordButton" onclick="toggleView('PasswordContainer');" type="button" value="Toggle Update Password" />
        </section>
        <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Medium" ClientIDMode="Static" runat="server"></asp:Label>
    </section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
