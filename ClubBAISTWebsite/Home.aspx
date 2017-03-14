<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ClubBAISTWebsite.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
    <div>
    Welcome!
    <asp:LoginName ID="LoginName1" runat="server" Font-Bold = "true" />
    <br />
    <asp:LoginStatus ID="LoginStatus1" runat="server" />
</div>
</asp:Content>