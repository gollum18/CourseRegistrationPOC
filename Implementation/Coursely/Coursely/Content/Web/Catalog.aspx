<%@ Page Title="Coursely - Catalog" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="Coursely.Content.Web.Catalog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="container">
        <h5 class="row alert alert-header">To view a course, select a school, then a department, and finally a course.</h5>
        <div class="row">
            <div class="col">
                <asp:Label ID="LblSchool" CssClass="col-form-label" AssociatedControlID="DropSchool" ClientIDMode="Static" 
                    runat="server">School:</asp:Label>
                <asp:DropDownList ID="DropSchool" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="SelectedSchoolChanged" AutoPostBack="true"
                    runat="server"></asp:DropDownList>
            </div>
            <div class="col">
                <asp:Label ID="LblDepartment" CssClass="col-form-label" AssociatedControlID="DropDepartment" ClientIDMode="Static" 
                    runat="server">Department:</asp:Label>
                <asp:DropDownList ID="DropDepartment" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="SelectedDepartmentChanged" AutoPostBack="true"
                    runat="server">
                    <asp:ListItem Value="default" Text="-Department-" Selected="True">-Department-</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col">
                <asp:Label ID="LblCourse" CssClass="col-form-label" AssociatedControlID="DropCourse" ClientIDMode="Static" runat="server">Course:</asp:Label>
                <asp:DropDownList ID="DropCourse" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="SelectedCourseChanged" AutoPostBack="true"
                    runat="server">
                    <asp:ListItem Value="default" Text="-Course-" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <h3 id="CourseName" class="row alert alert-heading" runat="server"></h3>
        <span id="CourseHeaders" class="row" runat="server"></span>
        <p id="CourseDescription" class="row" runat="server"></p>
        <asp:Label ID="StatusLabel" CssClass="row" ClientIDMode="Static" Font-Bold="true" Font-Size="Larger" ForeColor="Red" runat="server"></asp:Label>
    </section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
