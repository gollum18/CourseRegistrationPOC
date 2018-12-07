<%@ Page Title="Coursely - Enrollment" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Enroll.aspx.cs" Inherits="Coursely.Content.Web.Enroll" %>
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
    <section>
        <section>
            <label for="SchoolSelector">School: </label>
            <asp:DropDownList ID="SchoolSelector" ClientIDMode="static" OnSelectedIndexChanged="OnSchoolChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
            <label for="DepartmentSelector">Department: </label>
            <asp:DropDownList ID="DepartmentSelector" ClientIDMode="Static" OnSelectedIndexChanged="OnDepartmentChanged" AutoPostBack="true" runat="server">
                <asp:ListItem Value="-1">-Department-</asp:ListItem>
            </asp:DropDownList>
            <label for="CourseSelector">Course: </label>
            <asp:DropDownList ID="CourseSelector" ClientIDMode="Static" runat="server">
                <asp:ListItem Value="-1">-Course-</asp:ListItem>
            </asp:DropDownList>
        </section>
        <section>
            <label for="SemesterSelector">Semester: </label>
            <asp:DropDownList ID="SemesterSelector" ClientIDMode="Static" runat="server">
                <asp:ListItem Value="Fall">Fall</asp:ListItem>
                <asp:ListItem Value="Spring">Spring</asp:ListItem>
                <asp:ListItem Value="Summer">Summer</asp:ListItem>
            </asp:DropDownList>
            <label for="YearSelector">Year: </label>
            <asp:DropDownList ID="YearSelector" ClientIDMode="Static" runat="server"></asp:DropDownList>
            <input type="button" value="View Sections" onclick="viewSections();" /> 
        </section>
    </section>
    <section>
        <asp:Table ID="SectionsView" ClientIDMode="Static" runat="server"></asp:Table>
        <asp:RadioButtonList ID="SectionSelector" ClientIDMode="Static" runat="server"></asp:RadioButtonList>
        <input type="button" value="Enroll" onclick="enroll();" />
    </section>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
