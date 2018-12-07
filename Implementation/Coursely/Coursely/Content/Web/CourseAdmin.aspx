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
    <section class="container">
        <div class="row">
            <div class="col">
                <label for="SchoolSelector" class="col-form-label">School: </label>
                <asp:DropDownList ID="SchoolSelector" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="OnSchoolIndexChanged" runat="server">
                    <asp:ListItem Selected="True" Value="-1">-School-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="DepartmentSelector" class="col-form-label">Department: </label>
                <asp:DropDownList ID="DepartmentSelector" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="OnDepartmentIndexChanged" runat="server">
                    <asp:ListItem Selected="True" Value="-1">-Department-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="CourseSelector" class="col-form-label">Course: </label>
                <asp:DropDownList ID="CourseSelector" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="OnCourseIndexChanged" runat="server">
                    <asp:ListItem Selected="True" Value="-1">-Course-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="SectionSelector" class="col-form-label">Section: </label>
                <asp:DropDownList ID="SectionSelector" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="OnSectionIndexChanged" runat="server">
                    <asp:ListItem Value="-1">-Section-</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="SemesterSelector" class="col-form-label">Semester: </label>
                <asp:DropDownList ID="SemesterSelector" CssClass="form-control" runat="server">
                    <asp:ListItem Value="Fall">Fall</asp:ListItem>
                    <asp:ListItem Value="Spring">Spring</asp:ListItem>
                    <asp:ListItem Value="Summer">Summer</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="YearSelector" class="col-form-label">Year: </label>
                <asp:DropDownList ID="YearSelector" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
        </div>
    </section>
    <section id="CourseView" class="tab-content container">
        <div class="row">
            <div class="col col-1">
                <label for="CourseArchived" class="col-form-label">Archived: </label>
                <asp:CheckBox ID="CourseArchived" CssClass="form-check" runat="server" />
            </div>
            <div class="col">
                <label for="CourseName" class="col-form-label">Name: </label>
                <asp:TextBox ID="CourseName" CssClass="form-control" MaxLength="64" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="CourseNumber" class="col-form-label">Number: </label>
                <asp:TextBox ID="CourseNumber" CssClass="form-control" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="CourseCredits" class="col-form-label">Credits: </label>
                <asp:TextBox ID="CourseCredits" CssClass="form-control" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group">
            <label for="CoursePrereqs" class="col-form-label">Prerequisites: </label>
            <asp:TextBox ID="CoursePrereqs" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="CourseDescription" class="col-form-label">Description: </label>
            <asp:TextBox ID="CourseDescription" CssClass="form-control" ClientIDMode="Static" MaxLength="512" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
        <div class="row">
            <div class="col">
                <input type="button" class="btn btn-primary col-8" value="Add" onclick="validateCourse('AddCourse');" />
            </div>
            <div class="col">
                <input type="button" class="btn btn-secondary col-8" value="Modify" onclick="validateCourse('ModifyCourse');" />
            </div>
        </div>
    </section>
    <section id="SectionView" class="tab-content container" style="display:none;">
        <div class="row">
            <div class="col">
                <label for="SectionNumber" class="col-form-label">Number: </label>
                <asp:TextBox ID="SectionNumber" CssClass="form-control" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="SectionBuilding">Building: </label>
                <asp:DropDownList ID="SectionBuilding" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:DropDownList>
            </div>
            <div class="col">
                <label for="SectionRoom" class="col-form-label">Room: </label>
                <asp:TextBox ID="SectionRoom" CssClass="form-control" ClientIDMode="Static" TextMode="number" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="SectionEnrollment" class="col-form-label">Max Enrollment: </label>
                <asp:TextBox ID="SectionEnrollment" CssClass="form-control" ClientIDMode="Static" TextMode="Number" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="SectionStartDate" class="col-form-label">Start Date: </label>
                <asp:TextBox ID="SectionStartDate" CssClass="form-control" ClientIDMode="Static" TextMode="Date" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="SectionEndDate" class="col-form-label">End Date: </label>
                <asp:TextBox ID="SectionEndDate" CssClass="form-control" ClientIDMode="Static" TextMode="Date" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="SectionStartTime" class="col-form-label">Start Time: </label>
                <asp:TextBox ID="SectionStartTime" CssClass="form-control" ClientIDMode="Static" TextMode="Time" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="SectionEndTime" class="col-form-label">End Time: </label>
                <asp:TextBox ID="SectionEndTime" CssClass="form-control" ClientIDMode="Static" TextMode="Time" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <label for="SectionDays" class="col-form-label">Days Held: </label>
            <asp:CheckBoxList ID="SectionDays" CssClass="form-control" RepeatDirection="Horizontal" ClientIDMode="Static" runat="server"></asp:CheckBoxList>
        </div>
        <div class="row">
            <label for="SectionInstructors" class="col-form-label">Instructors: </label>
            <asp:ListBox ID="SectionInstructors" CssClass="form-control" ClientIDMode="Static" SelectionMode="Multiple" runat="server"></asp:ListBox>
        </div>
        <div class="mt-3 row">
            <div class="col align-content-center">
                <input type="button" class="btn btn-primary col-8" value="Add" onclick="validateSection('AddSection');" />
            </div>
            <div class="col align-content-center">
                <input type="button" class="btn btn-secondary col-8" value="Modify" onclick="validateSection('ModifySection');" />
            </div>
        </div>
    </section>
    <asp:Label ID="StatusLabel" CssClass="" ClientIDMode="Static" Font-Bold="true" ForeColor="Red" Font-Size="Large" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
