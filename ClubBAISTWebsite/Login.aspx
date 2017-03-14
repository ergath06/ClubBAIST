<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ClubBAISTWebsite.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="server">
<asp:Login runat="server" ID="loginControl" OnAuthenticate="loginControl_Authenticate" />
</asp:Content>