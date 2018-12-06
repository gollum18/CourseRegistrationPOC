<%@ Page Title="Coursely - Home Page" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Coursely.Content.Web.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
    <asp:Panel runat="server" CssClass="tab">
        <asp:Panel ID="MenuAdmin" Visible="false" runat="server">
            <div class="tabbutton"><asp:HyperLink NavigateUrl="UniversityAdmin.aspx" CssClass="nav-link" runat="server">Manage University</asp:HyperLink></div>
            <div class="tabbutton"><asp:HyperLink NavigateUrl="CourseAdmin.aspx" CssClass="nav-link" runat="server">Manage Courses</asp:HyperLink></div>
            <div class="tabbutton"><asp:HyperLink NavigateUrl="UserAdmin.aspx" CssClass="nav-link" runat="server">Manage Users</asp:HyperLink></div>
        </asp:Panel>
            
        <asp:Panel ID="MenuInstructor" Visible="false" runat="server">
            <div class="tabbutton"><asp:HyperLink NavigateUrl="ManageAdvisees.aspx" CssClass="nav-link" runat="server">Manage Advisees</asp:HyperLink></div>
            <div class="tabbutton"><asp:HyperLink NavigateUrl="ManageSections.aspx" CssClass="nav-link" runat="server">Manage Sections</asp:HyperLink></div>
        </asp:Panel>

        <asp:Panel ID="MenuStudent" Visible="false" runat="server">
            <div class="tabbutton"><asp:HyperLink NavigateUrl="Enroll.aspx" CssClass="nav-link" runat="server">Enroll for Class</asp:HyperLink></div>
            <div class="tabbutton"><asp:HyperLink NavigateUrl="Schedule.aspx" CssClass="nav-link" runat="server">Manage Schedule</asp:HyperLink></div>
            <div class="tabbutton"><asp:HyperLink NavigateUrl="Grades.aspx" CssClass="nav-link" runat="server">View Record</asp:HyperLink></div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
