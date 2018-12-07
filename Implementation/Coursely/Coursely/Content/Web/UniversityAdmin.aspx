<%@ Page Title="University Manager" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="UniversityAdmin.aspx.cs" Inherits="Coursely.Content.Web.UniversityAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src='<%=ResolveUrl("~/Content/JS/Validation.js") %>' type="text/javascript"></script>
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
    <section class="container">
        <div class="row">
            <div class="col">
                <label for="BuildingSelector" class="col-form-label">Building: </label>
                <asp:DropDownList ID="BuildingSelector" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnBuildingChanged" runat="server">
                    <asp:ListItem Value="-1">-Building-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="DepartmentSelector" class="col-form-label">Department: </label>
                <asp:DropDownList ID="DepartmentSelector" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnDepartmentChanged" runat="server">
                    <asp:ListItem Value="-1">-Department-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="MajorSelector" class="col-form-label">Major: </label>
                <asp:DropDownList ID="MajorSelector" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnMajorChanged" runat="server">
                    <asp:ListItem Value="-1">-Major-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <label for="SchoolSelector" class="col-form-label">School: </label>
                <asp:DropDownList ID="SchoolSelector" CssClass="form-control" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="OnSchoolChanged" runat="server">
                    <asp:ListItem Value="-1">-School-</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </section>
    <section id="buildingContent" class="tab-content container">
        <div class="row">
            <div class="col">
                <label for="BuildingName" class="col-form-label">Name: </label>
                <asp:TextBox ID="BuildingName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="BuidlingAbbreviation" class="col-form-label">Abbreviation: </label>
                <asp:TextBox ID="BuildingAbbreviation" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col">
                <input type="button" class="btn btn-primary col-6"  value="Add" onclick="modifyBuilding('CreateBuilding');" />
            </div>
            <div class="col">
                <input type="button" class="btn btn-secondary col-6" value="Modify" onclick="modifyBuilding('ModifyBuilding');" />
            </div>
        </div>
    </section>
    <section id="departmentContent" class="tab-content container" style="display:none;">
        <div class="row">
            <div class="col">
                <label for="DepartmentName" class="col-form-label">Name: </label>
                <asp:TextBox ID="DepartmentName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="DepartmentAbbreviation" class="col-form-label">Abbreviation: </label>
                <asp:TextBox ID="DepartmentAbbreviation" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col">
                <input type="button" class="btn btn-primary col-6"  value="Add" onclick="modifyDepartment('CreateDepartment');" />
            </div>
            <div class="col">
                <input type="button" class="btn btn-secondary col-6" value="Modify" onclick="modifyDepartment('ModifyDepartment');" />
            </div>
        </div>
    </section>
    <section id="majorContent" class="tab-content container" style="display:none;">
        <div class="row">
            <div class="col">
                <label for="MajorName" class="col-form-label">Name: </label>
                <asp:TextBox ID="MajorName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col">
                <input type="button" class="btn btn-primary col-6"  value="Add" onclick="modifyMajor('CreateMajor');" />
            </div>
            <div class="col">
                <input type="button" class="btn btn-secondary col-6" value="Modify" onclick="modifyMajor('ModifyMajor');" />
            </div>
        </div>
    </section>
    <section id="schoolContent" class="tab-content container" style="display:none;">
        <div class="row">
            <div class="col">
                <label for="SchoolName" class="col-form-label">Name: </label>
                <asp:TextBox ID="SchoolName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <label for="SchoolAbbreviation" class="col-form-label">Abbreviation: </label>
                <asp:TextBox ID="SchoolAbbreviation" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col">
                <input type="button" class="btn btn-primary col-6" value="Add" onclick="modifySchool('CreateSchool');" />
            </div>
            <div class="col">
                <input type="button" class="btn btn-secondary col-6" value="Modify" onclick="modifySchool('ModifySchool');" />
            </div>
        </div>
    </section>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
