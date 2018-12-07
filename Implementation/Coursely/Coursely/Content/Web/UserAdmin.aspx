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
    <section id="CreateUserTab" class="tab-content container">
        <div class="row">
            <div class="col">
                <label for="CreateID" class="col-form-label">University ID: </label>
                <asp:TextBox ID="CreateID" CssClass="form-control" ClientIDMode="Static" MaxLength="9" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="CreateRole" class="col-form-label">Role: </label>
                <asp:DropDownList ID="CreateRole" CssClass="form-control" ClientIDMode="Static" onchange="toggleRoleView();" runat="server">
                    <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
                    <asp:ListItem Value="Instructor">Instructor</asp:ListItem>
                    <asp:ListItem Value="Student">Student</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="CreateFirstName" class="col-form-label">First Name: </label>
                <asp:TextBox ID="CreateFirstName" CssClass="form-control" ClientIDMode="Static" MaxLength="32" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="CreateLastName" class="col-form-label">Last Name: </label>
                <asp:TextBox ID="CreateLastName" CssClass="form-control" ClientIDMode="Static" MaxLength="32" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="CreateEmail" class="col-form-label">Email: </label>
                <asp:TextBox ID="CreateEmail" CssClass="form-control" ClientIDMode="Static" MaxLength="64" TextMode="Email" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="CreatePassword" class="col-form-label">Password: </label>
                <asp:TextBox ID="CreatePassword" CssClass="form-control" ClientIDMode="Static" MaxLength="64" TextMode="Password" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="CreateConfirmPassword" class="col-form-label">Confirm Password: </label>
                <asp:TextBox ID="CreateConfirmPassword" CssClass="form-control" ClientIDMode="Static" MaxLength="64" TextMode="Password" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="role-view row" id="CreateInstructorViews" style="display:none;">
            <div class="col">
                <label for="CreateDepartment" class="col-form-label">Department(s): </label>
                <asp:ListBox ID="CreateDepartment" CssClass="form-control" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
            </div>
        </div>
        <div class="role-view row" id="CreateStudentViews" style="display:none;">
            <div class="col justify-content-center">
                <label for="CreateAdvisor" class="col-form-label">Advisor(s): </label>
                <asp:ListBox ID="CreateAdvisor" CssClass="form-control" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
            </div>
            <div class="col">
                <label for="CreateMajor" class="col-form-label">Major(s): </label>
                <asp:ListBox ID="CreateMajor" CssClass="form-control" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
            </div>
        </div>
        <div class="row justify-content-center">
            <input type="button" class="btn btn-primary col-5 mt-2" value="Create User" onclick="createUser();"/>
        </div>
    </section>
    <asp:Label ID="StatusLabel" CssClass="alert" Font-Bold="true" Font-Size="Larger" ForeColor="Red" ClientIDMode="Static" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
