<%@ Page Title="" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="ManageSections.aspx.cs" Inherits="Coursely.Content.Web.ManageSections" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
    <script src="<%=ResolveUrl("~/Content/JS/Validation.js") %>" type="text/javascript"></script>
    <script>
        var statusLabel = null;

        $(document).ready(function () {
            statusLabel = $("#StatusLabel");
        });

        function viewSections() {
            var validationResult = null;

            // Validate the semester
            validationResult = validateSelectedSemester($("#SemesterDD").val());
            if (!validationResult.result) {
                statusLabel.text(validationResult.error);
                statusLabel.css("color", "red");
                return;
            }
            validationResult = validateSelectedYear($("#YearDD").val());
            if (!validationResult.result) {
                statusLabel.text(validationResult.error);
                statusLabel.css("color", "red");
                return;
            }
            doPostBack("ViewSections");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row">
            <div class="col">
                <h5>To see your courses for a term, first, select a semester, then a year, and finally, click the 'View Courses' button.</h5>
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
        <div class="row mt-2 mb-2 justify-content-center">
            <div class="col">
                <input type="button" class="btn btn-primary col-4" value="View Courses" onclick="viewSections();" />
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:Table ID="ScheduleView" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:Label ID="StatusLabel" CssClass="alert" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
