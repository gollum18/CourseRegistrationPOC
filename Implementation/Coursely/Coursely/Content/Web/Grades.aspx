<%@ Page Title="Coursely - Academic Record" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="Coursely.Content.Web.Grades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <asp:Label ID="RecordHeader" Text="" AssociatedControlID="StudentRecordList" ClientIDMode="Static" runat="server"></asp:Label>
    <asp:ListBox ID="StudentRecordList" ClientIDMode="Static" runat="server"></asp:ListBox>
    <asp:Label ID="StatusLabel" Font-Bold="true" Font-Size="Large" ForeColor="Red" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
