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
    <div class="container">
        <div class="row">
            <div class="col">
                <label for="AdviseeDD" class="col-form-label">Advisee: </label>
                <asp:DropDownList ID="AdviseeDD" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="row mt-2 mb-2 justify-content-lg-center">
            <div class="col">
                <input type="button" class="btn btn-primary col-4" value="View Record" onclick="viewAction('ViewRecord');" />
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="SemesterDD" class="col-form-label">Semester: </label>
                <asp:DropDownList ID="SemesterDD" CssClass="form-control" ClientIDMode="Static" runat="server">
                    <asp:ListItem Value="Fall">Fall</asp:ListItem>
                    <asp:ListItem Value="Spring">Spring</asp:ListItem>
                    <asp:ListItem Value="Summer">Summer</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="YearDD" class="col-form-label">Year: </label>
                <asp:DropDownList ID="YearDD" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="row mt-2 mb-2 justify-content-lg-center">
            <div class="col">
                <input type="button" class="btn btn-primary col-4" value="View Schedule" onclick="viewAction('ViewSchedule');" />
            </div>
        </div>
    </div>
    <asp:Panel ID="RecordView" CssClass="container" Visible="false" runat="server">
        <div class="row">
            <div class="col">
                <asp:Table ID="RecordTable" CssClass="table table-sm table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="ScheduleView" CssClass="container" Visible="false" runat="server">
        <div class="row">
            <div class="col">
                <asp:Table ID="ScheduleTable" CssClass="table table-sm table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="OverrideView" CssClass="container" runat="server">
        <div class="row">
            <div class="col">
                <h5>To override a section for a student:</h5>
                <ol class="list-group">
                    <li class="list-group-item">Select the semester and year using the dropdown above.</li>
                    <li class="list-group-item">Select the school using the dropdown below.</li>
                    <li class="list-group-item">Select the department using the dropdown below.</li>
                    <li class="list-group-item">Select the course using the dropdown below.</li>
                    <li class="list-group-item">Select the section from the list below.</li>
                    <li class="list-group-item">Click on the Override button below.</li>
                </ol>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <label for="SchoolDD" class="col-form-label">School: </label>
                <asp:DropDownList ID="SchoolDD" CssClass="form-control" OnSelectedIndexChanged="OnSchoolChanged" AutoPostBack="true" runat="server">
                    <asp:ListItem Value="-1">-School-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="DepartmentDD" class="col-form-label">Department: </label>
                <asp:DropDownList ID="DepartmentDD" CssClass="form-control" OnSelectedIndexChanged="OnDepartmentChanged" AutoPostBack="true" runat="server">
                    <asp:ListItem Value="-1">-Department-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="CourseDD" class="col-form-label">Course: </label>
                <asp:DropDownList ID="CourseDD" CssClass="form-control" OnSelectedIndexChanged="OnCourseChanged" AutoPostBack="true" runat="server">
                    <asp:ListItem Value="-1">-Course-</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:Table ID="SectionTable" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:RadioButtonList ID="SectionSelector" CssClass="form-control" RepeatDirection="Horizontal" ClientIDMode="Static" runat="server"></asp:RadioButtonList>
            </div>
        </div>
        <div class="row mt-2 justify-content-center">
            <div class="col">
                <input type="button" class="btn btn-primary col-4" value="Override" onclick="overrideSection();"/>
            </div>
        </div>
    </asp:Panel>
    <div>
        <asp:Label ID="StatusLabel" CssClass="alert" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
