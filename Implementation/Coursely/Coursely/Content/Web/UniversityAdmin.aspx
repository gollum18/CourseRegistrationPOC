<%@ Page Title="University Manager" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="UniversityAdmin.aspx.cs" Inherits="Coursely.Content.Web.UniversityAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%=ResolveUrl("~/Content/JS/UniversityValidation.js") %>' type="text/javascript"></script>
    <script>
        var statusLabel = null;

        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        var validationResult = null;

        function modifyBuilding(action) {
            if (action === "ModifyBuilding") {
                validationResult = validateSelection($("#BuildingSelector").val(), "building");
                if (!validationResult.result) {
                    statusLabel.css("color", "red");
                    statusLabel.text(validationResult.error);
                    return;
                }
            }
            validationResult = validateName($("#BuildingName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            validationResult = validateAbbreviation($("#BuildingAbbreviation").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            doPostBack(action);
        }

        function modifyDepartment(action) {
            if (action === "ModifyDepartment") {
                validationResult = validateSelection($("#DepartmentSelector").val(), "department");
                if (!validationResult.result) {
                    statusLabel.css("color", "red");
                    statusLabel.text(validationResult.error);
                    return;
                }
            }
            validationResult = validateName($("#DepartmentName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            validationResult = validateAbbreviation($("#DepartmentAbbreviation").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            doPostBack(action);
        }

        function modifyMajor(action) {
            if (action === "ModifyMajor") {
                validationResult = validateSelection($("#MajorSelector").val(), "major");
                if (!validationResult.result) {
                    statusLabel.css("color", "red");
                    statusLabel.text(validationResult.error);
                    return;
                }
            }
            validationResult = validateName($("#MajorName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            validationResult = validateAbbreviation($("#MajorAbbreviation").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            doPostBack(action);
        }

        function modifySchool(action) {
            if (action === "ModifySchool") {
                validationResult = validateSelection($("#SchoolSelector").val(), "school");
                if (!validationResult.result) {
                    statusLabel.css("color", "red");
                    statusLabel.text(validationResult.error);
                    return;
                }
            }
            validationResult = validateName($("#SchoolName").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            validationResult = validateAbbreviation($("#SchoolAbbreviation").val());
            if (!validationResult.result) {
                statusLabel.css("color", "red");
                statusLabel.text(validationResult.error);
                return;
            }
            doPostBack(action);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <section class="btn-group">
        <input type="button" class="btn" value="Manage Buildings" onclick="toggleTabContent('buildingContent');" />
        <input type="button" class="btn" value="Manage Departments" onclick="toggleTabContent('departmentContent');" />
        <input type="button" class="btn" value="Manage Majors" onclick="toggleTabContent('majorContent');" />
        <input type="button" class="btn" value="Manage Schools" onclick="toggleTabContent('schoolContent');" />
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section>
        <label>Building: </label>
        <asp:DropDownList ID="BuildingSelector" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnBuildingChanged" runat="server">
            <asp:ListItem Value="-1">-Building-</asp:ListItem>
        </asp:DropDownList>
        <label for="DepartmentSelector">Department: </label>
        <asp:DropDownList ID="DepartmentSelector" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnDepartmentChanged" runat="server">
            <asp:ListItem Value="-1">-Department-</asp:ListItem>
        </asp:DropDownList>
        <label for="MajorSelector">Major: </label>
        <asp:DropDownList ID="MajorSelector" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnMajorChanged" runat="server">
            <asp:ListItem Value="-1">-Major-</asp:ListItem>
        </asp:DropDownList>
        <label for="SchoolSelector">School: </label>
        <asp:DropDownList ID="SchoolSelector" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnSchoolChanged" runat="server">
            <asp:ListItem Value="-1">-School-</asp:ListItem>
        </asp:DropDownList>
    </section>
    <section id="buildingContent" class="tab-content">
        <section>
            <label for="BuildingName">Name: </label>
            <asp:TextBox ID="BuildingName" ClientIDMode="Static" runat="server"></asp:TextBox>
            <label for="BuidlingAbbreviation">Abbreviation: </label>
            <asp:TextBox ID="BuildingAbbreviation" ClientIDMode="Static" runat="server"></asp:TextBox>
        </section>
        <section>
            <input type="button" value="Add" onclick="modifyBuilding('CreateBuilding');" />
            <input type="button" value="Modify" onclick="modifyBuilding('ModifyBuilding');" />
        </section>
    </section>
    <section id="departmentContent" class="tab-content" style="display:none;">
        <section>
            <label for="DepartmentName">Name: </label>
            <asp:TextBox ID="DepartmentName" ClientIDMode="Static" runat="server"></asp:TextBox>
            <label for="DepartmentAbbreviation">Abbreviation: </label>
            <asp:TextBox ID="DepartmentAbbreviation" ClientIDMode="Static" runat="server"></asp:TextBox>
        </section>
        <section>
            <input type="button" value="Add" onclick="modifyDepartment('CreateDepartment');" />
            <input type="button" value="Modify" onclick="modifyDepartment('ModifyDepartment');" />
        </section>
    </section>
    <section id="majorContent" class="tab-content" style="display:none;">
        <section>
            <label for="MajorName">Name: </label>
            <asp:TextBox ID="MajorName" ClientIDMode="Static" runat="server"></asp:TextBox>
        </section>
        <section>
            <input type="button" value="Add" onclick="modifyMajor('CreateMajor');" />
            <input type="button" value="Modify" onclick="modifyMajor('ModifyMajor');" />
        </section>
    </section>
    <section id="schoolContent" class="tab-content" style="display:none;">
        <section>
            <label for="SchoolName">Name: </label>
            <asp:TextBox ID="SchoolName" ClientIDMode="Static" runat="server"></asp:TextBox>
            <label for="SchoolAbbreviation">Abbreviation: </label>
            <asp:TextBox ID="SchoolAbbreviation" ClientIDMode="Static" runat="server"></asp:TextBox>
        </section>
        <section>
            <input type="button" value="Add" onclick="modifySchool('CreateSchool');" />
            <input type="button" value="Modify" onclick="modifySchool('ModifySchool');" />
        </section>
    </section>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
