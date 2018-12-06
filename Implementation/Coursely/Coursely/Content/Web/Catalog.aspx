<%@ Page Title="Coursely - Catalog" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="Coursely.Content.Web.Catalog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <!-- The content pane. -->
    <section>
        <!-- Selector for the school. -->
        <asp:Label ID="LblSchool" AssociatedControlID="DropSchool" ClientIDMode="Static" 
            runat="server">School:</asp:Label>
        <asp:DropDownList ID="DropSchool" ClientIDMode="Static" OnSelectedIndexChanged="SelectedSchoolChanged" AutoPostBack="true"
            runat="server"></asp:DropDownList>
        <!-- Selector for the department. -->
        <asp:Label ID="LblDepartment" AssociatedControlID="DropDepartment" ClientIDMode="Static" 
            runat="server">Department:</asp:Label>
        <asp:DropDownList ID="DropDepartment" ClientIDMode="Static" OnSelectedIndexChanged="SelectedDepartmentChanged" AutoPostBack="true"
            runat="server">
            <asp:ListItem Value="default" Text="-Department-" Selected="True">-Department-</asp:ListItem>
        </asp:DropDownList>
        <!-- Selector for the course. -->
        <asp:Label ID="LblCourse" AssociatedControlID="DropCourse" ClientIDMode="Static" runat="server">Course:</asp:Label>
        <asp:DropDownList ID="DropCourse" ClientIDMode="Static" OnSelectedIndexChanged="SelectedCourseChanged" AutoPostBack="true"
            runat="server">
            <asp:ListItem Value="default" Text="-Course-" Selected="True"></asp:ListItem>
        </asp:DropDownList>
    </section>
    <section id="contentPane">
        <!-- The header pane. -->
        <section id="headerPane">
            <h3 id="CourseName" runat="server"></h3>
            <span id="CourseHeaders" runat="server"></span>
        </section>
        <!-- The description pane. -->
        <section id="descriptionPane">
            <p id="CourseDescription" runat="server"></p>
        </section>
    </section>
    <section id="errorPane">
        <asp:Label ID="StatusLabel" ClientIDMode="Static" Font-Bold="true" Font-Size="Larger" ForeColor="Red" runat="server"></asp:Label>
    </section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
