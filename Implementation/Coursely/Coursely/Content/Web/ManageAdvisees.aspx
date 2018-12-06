<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="ManageAdvisees.aspx.cs" Inherits="Coursely.Content.Web.ManageAdvisees" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var statusLabel = null;

        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        function overrideSection() {
            // Validate that an advisee was selected
            var validationResult = validateAdvisee();
            if (!validationResult.result) {
                statusLabel.text(validationResult.error);
                statusLabel.css("color", "red");
                return;
            }
            // Validate that a section was selected
            if (getSelectedItemFromRList("SectionSelector") === -1) {
                statusLabel.text("Error: You must select a course section!");
                statusLabel.css("color", "red");
                return;
            }
            doPostBack("OverrideSection");
        }

        function validateAdvisee() {
            var advisee = $("#AdviseeDD").val();
            if (isEmptyOrWhiteSpace(advisee)) {
                return { result: false, error: "Error: Invalid advisee selected!" };
            }
            if (isNaN(advisee)) {
                return { result: false, error: "Errro: Invalid advisee selected!" };
            }
            return { result: true, error: "" };
        }

        function viewAction(action) {
            // Validate that an advisee was selected
            var validationResult = validateAdvisee();
            if (!validationResult.result) {
                statusLabel.text(validationResult.error);
                statusLabel.css("color", "red");
                return;
            }
            doPostBack(action);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <span class="form-row">
        <label for="AdviseeDD">Advisee: </label>
        <asp:DropDownList ID="AdviseeDD" ClientIDMode="Static" runat="server"></asp:DropDownList>
        <input type="button" value="View Record" onclick="viewAction('ViewRecord');" />
    </span>
    <span>
    <label for="SemesterDD">Semester: </label>
    <asp:DropDownList ID="SemesterDD" ClientIDMode="Static" runat="server">
        <asp:ListItem Value="Fall">Fall</asp:ListItem>
        <asp:ListItem Value="Spring">Spring</asp:ListItem>
        <asp:ListItem Value="Summer">Summer</asp:ListItem>
    </asp:DropDownList>
        <label for="YearDD">Year: </label>
        <asp:DropDownList ID="YearDD" ClientIDMode="Static" runat="server"></asp:DropDownList>
        <input type="button" value="View Schedule" onclick="viewAction('ViewSchedule');" />
    </span>
    <asp:Panel ID="RecordView" CssClass="form-group" Visible="false" runat="server">
        <asp:Table ID="RecordTable" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
    </asp:Panel>
    <asp:Panel ID="ScheduleView" CssClass="form-group" Visible="false" runat="server">
        <asp:Table ID="ScheduleTable" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
    </asp:Panel>
    <asp:Panel ID="OverrideView" CssClass="form-group" runat="server">
        <section>
            <h6>To override a section for a student:</h6>
            <ol>
                <li>Select the semester and year using the dropdown above.</li>
                <li>Select the school using the dropdown below.</li>
                <li>Select the department using the dropdown below.</li>
                <li>Select the course using the dropdown below.</li>
                <li>Select the section from the list below.</li>
                <li>Click on the Override button below.</li>
            </ol>
            <label for="SchoolDD">School: </label>
            <asp:DropDownList ID="SchoolDD" OnSelectedIndexChanged="OnSchoolChanged" AutoPostBack="true" runat="server">
                <asp:ListItem Value="-1">-School-</asp:ListItem>
            </asp:DropDownList>
            <label for="DepartmentDD">Department: </label>
            <asp:DropDownList ID="DepartmentDD" OnSelectedIndexChanged="OnDepartmentChanged" AutoPostBack="true" runat="server">
                <asp:ListItem Value="-1">-Department-</asp:ListItem>
            </asp:DropDownList>
            <label for="CourseDD">Course: </label>
            <asp:DropDownList ID="CourseDD" OnSelectedIndexChanged="OnCourseChanged" AutoPostBack="true" runat="server">
                <asp:ListItem Value="-1">-Course-</asp:ListItem>
            </asp:DropDownList>
        </section>
        <section>
            <asp:Table ID="SectionTable" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            <asp:RadioButtonList ID="SectionSelector" ClientIDMode="Static" runat="server"></asp:RadioButtonList>
        </section>
        <input type="button" value="Override" onclick="overrideSection();"/>
    </asp:Panel>
    <section>
        <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
    </section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
