<%@ Page Title="Coursely - Enroll for Classes" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Enroll.aspx.cs" Inherits="Coursely.Content.Web.Enroll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <script src='<%=ResolveUrl("~/Content/JS/CourseValidation.js") %>' type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/Content/JS/SectionValidation.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/Content/JS/Validation.js") %>" type="text/javascript"></script>
    <script>
        var statusLabel = null;
        
        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        var currentYear = (new Date()).getFullYear();

        // Used to validate information in the dropdowns
        var semesters = ["Fall", "Spring", "Summer"];
        var years = [];

        // Generate the yers to check against later
        for (var year = currentYear; year < currentYear + 2; year++) {
            years.push(year);
        }

        function validateSemester(semester) {
            if (!$.inArray(semester, semesters)) {
                return {result:false, error:"Error: You have selected an invalid semester!"};
            } return {result:true, error:""};
        }

        function validateYear(year) {
            if (!$.inArray(year, years)) {
                return {result:false, error:"Error: You have selected an invalid year!"};
            } return {result:true, error:""};
        }

        function enroll() {
            // Check that a section was selected
            var validationResult = getSelectedItemFromRList("SectionSelector");
            if (validationResult == -1) {
                statusLabel.text("Error: You must select a section to enroll in!");
                return;
            }
            doPostBack('Enroll');
        }

        function viewSections() {
            var validationResult = null;
            // Check that a course, semester, and year were selected
            validationResult = validateSelection($("#CourseSelector").val(), "course");
            if (!validationResult.result) {
                statusLabel.text(validationResult.error);
                return;
            }
            doPostBack('ViewSections');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <label for="SchoolSelector" class="col-form-label">School: </label>
                <asp:DropDownList ID="SchoolSelector" CssClass="form-control" ClientIDMode="static" OnSelectedIndexChanged="OnSchoolChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
            </div>
            <div class="col">
                <label for="DepartmentSelector" class="col-form-label">Department: </label>
                <asp:DropDownList ID="DepartmentSelector" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="OnDepartmentChanged" AutoPostBack="true" runat="server">
                    <asp:ListItem Value="-1">-Department-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="CourseSelector" class="col-form-label">Course: </label>
                <asp:DropDownList ID="CourseSelector" CssClass="form-control" ClientIDMode="Static" runat="server">
                    <asp:ListItem Value="-1">-Course-</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="SemesterSelector" class="col-form-label">Semester: </label>
                <asp:DropDownList ID="SemesterSelector" CssClass="form-control" ClientIDMode="Static" runat="server">
                    <asp:ListItem Value="Fall">Fall</asp:ListItem>
                    <asp:ListItem Value="Spring">Spring</asp:ListItem>
                    <asp:ListItem Value="Summer">Summer</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="YearSelector" class="col-form-label">Year: </label>
                <asp:DropDownList ID="YearSelector" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="row justify-content-center mt-2 mb-2">
            <input type="button" class="btn btn-primary col-6" value="View Sections" onclick="viewSections();" />
        </div>
    </div>
    <asp:Panel ID="EnrollPanel" CssClass="container" Visible="false" runat="server">
        <div class="row">
            <div class="col">
                <asp:Table ID="SectionsView" CssClass="table table-sm table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:RadioButtonList ID="SectionSelector" CssClass="form-control" RepeatDirection="Horizontal" ClientIDMode="Static" runat="server"></asp:RadioButtonList>
            </div>
        </div>
        <div class="row justify-content-center mt-2">
            <input type="button" class="btn btn-primary col-6" value="Enroll" onclick="enroll();" />
        </div>
    </asp:Panel>
    <asp:Label ID="StatusLabel" CssClass="alert" Font-Bold="true" Font-Size="Large" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
