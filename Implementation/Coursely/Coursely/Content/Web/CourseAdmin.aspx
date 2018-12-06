<%@ Page Title="Coursely - Manage Courses" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="CourseAdmin.aspx.cs" Inherits="Coursely.Content.Web.CourseAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%=ResolveUrl("~/Content/JS/CourseValidation.js") %>' type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/Content/JS/SectionValidation.js") %>" type="text/javascript"></script>
    <script>
        var statusLabel = null;

        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        var validationResult = null;

        function validateCourse(action) {
            // Validate department
            validationResult = validateCourseDepartment($("#DepartmentSelector").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate course name
            validationResult = validateCourseName($("#CourseName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate course number
            validationResult = validateCourseNumber($("#CourseNumber").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate course credits
            validationResult = validateCourseCredits(parseInt($("#CourseCredits").val()));
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate course prerequisites
            validationResult = validateCoursePrereqs($("#CoursePrereqs").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate course description
            validationResult = validateCourseDescription($("#CourseDescription").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Post back to the server if everything checks out.
            doPostBack(action);
        }

        /*
         * Validates section information and adds the section to the database.
         */
        function validateSection(action) {
            // Validate course id
            validationResult = validateSectionCourse(parseInt($("#CourseSelector").val()));
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate building id
            validationResult = validateSectionBuilding(parseInt($("#SectionBuilding").val()));
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate section number
            validationResult = validateSectionNumber(parseInt($("#SectionNumber").val()));
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate room number
            validationResult = validateSectionRoom(parseInt($("#SectionRoom").val()));
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate dates
            validationResult = validateSectionDates($("#SectionStartDate").val(), $("SectionEndDate").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate times
            validationResult = validateSectionTimes($("#SectionStartTime").val(), $("SectionEndTime").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Validate enrollment
            validationResult = validateSectionMaxEnrollment(parseInt($("#SectionEnrollment").val()))
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }

            // Post back to the server if everything checks out.
            doPostBack(action);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <section class="btn-group">
        <input class="btn" type="button" value="Courses" onclick="toggleTabContent('CourseView');" />
        <input class="btn" type="button" value="Sections" onclick="toggleTabContent('SectionView');" />
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section>
        <label for="SchoolSelector">School: </label>
        <asp:DropDownList ID="SchoolSelector" AutoPostBack="true" OnSelectedIndexChanged="OnSchoolIndexChanged" runat="server">
            <asp:ListItem Selected="True" Value="-1">-School-</asp:ListItem>
        </asp:DropDownList>
        <label for="DepartmentSelector">Department: </label>
        <asp:DropDownList ID="DepartmentSelector" AutoPostBack="true" OnSelectedIndexChanged="OnDepartmentIndexChanged" runat="server">
            <asp:ListItem Selected="True" Value="-1">-Department-</asp:ListItem>
        </asp:DropDownList>
        <label for="CourseSelector">Course: </label>
        <asp:DropDownList ID="CourseSelector" AutoPostBack="true" OnSelectedIndexChanged="OnCourseIndexChanged" runat="server">
            <asp:ListItem Selected="True" Value="-1">-Course-</asp:ListItem>
        </asp:DropDownList>
    </section>
    <span>You must select a school and department using the dropdown menus above before creating your course.</span>
    <span>To modify a course, select the school, department, and course to edit in that order using the dropdowns above.</span>
    <section id="CourseView" class="tab-content">
        <section>
            <label for="CourseArchived">Archived: </label>
            <asp:CheckBox ID="CourseArchived" runat="server" />
            <label for="CourseName">Name: </label>
            <asp:TextBox ID="CourseName" MaxLength="64" ClientIDMode="Static" runat="server"></asp:TextBox>
            <label for="CourseNumber">Number: </label>
            <asp:TextBox ID="CourseNumber" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            <label for="CourseCredits">Credits: </label>
            <asp:TextBox ID="CourseCredits" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            <label for="CoursePrereqs">Prerequisites: </label>
            <asp:TextBox ID="CoursePrereqs" ClientIDMode="Static" runat="server"></asp:TextBox>
            <label for="CourseDescription">Description: </label>
            <asp:TextBox ID="CourseDescription" ClientIDMode="Static" MaxLength="512" TextMode="MultiLine" runat="server"></asp:TextBox>
        </section>
        <input type="button" value="Add" onclick="validateCourse('AddCourse');" />
        <input type="button" value="Modify" onclick="validateCourse('ModifyCourse');" />
    </section>
    <section id="SectionView" class="tab-content" style="display:none;">
        <section>
            <label for="SemesterSelector">Semester: </label>
            <asp:DropDownList ID="SemesterSelector" runat="server">
                <asp:ListItem Value="Fall">Fall</asp:ListItem>
                <asp:ListItem Value="Spring">Spring</asp:ListItem>
                <asp:ListItem Value="Summer">Summer</asp:ListItem>
            </asp:DropDownList>
            <label for="YearSelector">Year: </label>
            <asp:DropDownList ID="YearSelector" runat="server"></asp:DropDownList>
            <label for="SectionSelector">Section: </label>
            <asp:DropDownList ID="SectionSelector" AutoPostBack="true" OnSelectedIndexChanged="OnSectionIndexChanged" runat="server">
                <asp:ListItem Value="-1">-Section-</asp:ListItem>
            </asp:DropDownList>
        </section>
        <section>
            <label for="SectionNumber">Number: </label>
            <asp:TextBox ID="SectionNumber" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            <label for="SectionBuilding">Building: </label>
            <asp:DropDownList ID="SectionBuilding" ClientIDMode="Static" runat="server"></asp:DropDownList>
            <label for="SectionRoom">Room: </label>
            <asp:TextBox ID="SectionRoom" ClientIDMode="Static" TextMode="number" runat="server"></asp:TextBox>
            <label for="SectionEnrollment">Max Enrollment: </label>
            <asp:TextBox ID="SectionEnrollment" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            <label for="SectionStartDate">Start Date: </label>
            <asp:TextBox ID="SectionStartDate" ClientIDMode="Static" TextMode="Date" runat="server"></asp:TextBox>
            <label for="SectionEndDate">End Date: </label>
            <asp:TextBox ID="SectionEndDate" ClientIDMode="Static" TextMode="Date" runat="server"></asp:TextBox>
            <label for="SectionStartTime">Start Time: </label>
            <asp:TextBox ID="SectionStartTime" ClientIDMode="Static" TextMode="Time" runat="server"></asp:TextBox>
            <label for="SectionEndTime">End Time: </label>
            <asp:TextBox ID="SectionEndTime" ClientIDMode="Static" TextMode="Time" runat="server"></asp:TextBox>
            <label for="SectionDays">Days Held: </label>
            <asp:CheckBoxList ID="SectionDays" ClientIDMode="Static" runat="server"></asp:CheckBoxList>
            <label for="SectionInstructors">Instructors: </label>
            <asp:ListBox ID="SectionInstructors" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
        </section>
        <section>
            <input type="button" value="Add" onclick="validateSection('AddSection');" />
            <input type="button" value="Modify" onclick="validateSection('ModifySection');" />
        </section>
    </section>
    <asp:Label ID="StatusLabel" ClientIDMode="Static" Font-Bold="true" ForeColor="Red" Font-Size="Large" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
