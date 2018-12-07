<%@ Page Title="Coursely - Academic Record" Language="C#" MasterPageFile="~/MasterForm.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="Coursely.Content.Web.Grades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="row mt-3">
            <div class="col">
                <asp:Table ID="StudentRecord" CssClass="table table-sm table-bordered table-striped col-5" ClientIDMode="Static" runat="server"></asp:Table>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <asp:Label ID="StatusLabel" CssClass="alert" Font-Bold="true" Font-Size="Large" ForeColor="Red" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
