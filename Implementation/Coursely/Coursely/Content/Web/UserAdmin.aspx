<%@ Page Title="Coursely - Manage Users" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="Coursely.Content.Web.UserAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%=ResolveUrl("~/Content/JS/UserValidation.js") %>' type="text/javascript"></script>
    <script>
        var statusLabel = null;

        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
            // Show the proper user menu based on the selected role
            toggleRoleView();
        });

        var validationResult = null;

        function createUser() {
            // Validate the id
            validationResult = validateUID($("#CreateID").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            // Validate the first name
            validationResult = validateName($("#CreateFirstName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            // Validate the last name
            validationResult = validateName($("#CreateLastName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            // Validate the email
            validationResult = validateEmail($("#CreateEmail").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            // Validate the passwords
            validationResult = validatePassword("NULL", $("#CreatePassword").val(), $("#CreateConfirmPassword").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            // Post back to the server
            doPostBack('CreateUser');
        }

        /*
         * Handles change notifications from the role dropdown.
         */
        function toggleRoleView() {
            $('.role-view').each(function () {
                $(this).css('display', 'none');
            });
            if ($('#CreateRole').val() === 'Instructor') {
                $('#CreateInstructorViews').css('display', 'block');
            } else if ($('#CreateRole').val() === 'Student') {
                $('#CreateStudentViews').css('display', 'block');
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <section class="btn-group">
        <input class="btn" type="button" value="Create User" onclick="toggleTabContent('CreateUserTab');" />
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <!-- Show the create user tab by default. -->
    <section id="CreateUserTab" class="tab-content">
        <section>
            <label for="CreateID">University ID: </label>
            <asp:TextBox ID="CreateID" ClientIDMode="Static" MaxLength="9" runat="server"></asp:TextBox>
            <label for="CreatePassword">Password: </label>
            <asp:TextBox ID="CreatePassword" ClientIDMode="Static" MaxLength="64" TextMode="Password" runat="server"></asp:TextBox>
            <label for="CreateConfirmPassword">Confirm Password: </label>
            <asp:TextBox ID="CreateConfirmPassword" ClientIDMode="Static" MaxLength="64" TextMode="Password" runat="server"></asp:TextBox>
            <label for="CreateRole">Role: </label>
            <asp:DropDownList ID="CreateRole" ClientIDMode="Static" onchange="toggleRoleView();" runat="server">
                <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
                <asp:ListItem Value="Instructor">Instructor</asp:ListItem>
                <asp:ListItem Value="Student">Student</asp:ListItem>
            </asp:DropDownList>
        </section>
        <section>
            <label for="CreateFirstName">First Name: </label>
            <asp:TextBox ID="CreateFirstName" ClientIDMode="Static" MaxLength="32" runat="server"></asp:TextBox>
            <label for="CreateLastName">Last Name: </label>
            <asp:TextBox ID="CreateLastName" ClientIDMode="Static" MaxLength="32" runat="server"></asp:TextBox>
            <label for="CreateEmail">Email: </label>
            <asp:TextBox ID="CreateEmail" ClientIDMode="Static" MaxLength="64" TextMode="Email" runat="server"></asp:TextBox>
        </section>
        <section class="role-view" id="CreateInstructorViews" style="display:none;">
            <label for="CreateDepartment">Department(s): </label>
            <asp:ListBox ID="CreateDepartment" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
        </section>
        <section class="role-view" id="CreateStudentViews" style="display:none;">
            <label for="CreateAdvisor">Advisor(s): </label>
            <asp:ListBox ID="CreateAdvisor" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
            <label for="CreateMajor">Major(s): </label>
            <asp:ListBox ID="CreateMajor" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
        </section>
        <input type="button" value="Create User" onclick="createUser();"/>
    </section>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Larger" ForeColor="Red" ClientIDMode="Static" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
