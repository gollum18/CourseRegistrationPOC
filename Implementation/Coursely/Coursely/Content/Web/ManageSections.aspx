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
    <section>
        <h6>To see your courses for a term, first, select a semester, then a year, and finally, click the 'View Courses' button.</h6>
        <label for="SemesterDD">Semester: </label>
        <asp:DropDownList ID="SemesterDD" ClientIDMode="Static" runat="server">
            <asp:ListItem Value="Fall">Fall</asp:ListItem>
            <asp:ListItem Value="Spring">Spring</asp:ListItem>
            <asp:ListItem Value="Summer">Summer</asp:ListItem>
        </asp:DropDownList>
        <label for="YearDD">Year: </label>
        <asp:DropDownList ID="YearDD" ClientIDMode="Static" runat="server"></asp:DropDownList>
        <input type="button" value="View Courses" onclick="viewSections();" />
    </section>
    <section>
        <asp:Table ID="ScheduleView" CssClass="table table-bordered table-striped" ClientIDMode="Static" runat="server"></asp:Table>
    </section>
    <section>
        <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ClientIDMode="Static" runat="server"></asp:Label>
    </section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
